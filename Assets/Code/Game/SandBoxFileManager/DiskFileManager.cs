using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using RO;

[SLua.CustomLuaClassAttribute]
public class DiskFileManager : Singleton<DiskFileManager>
{
	public static DiskFileManager Instance
	{
		get
		{
			return ins;
		}
	}

	private int m_ider;
	private string m_filesUsedRecordRootPath;
	private Dictionary<int, FilesUsedDetail> m_dictFilesUsedDetail;
	private Dictionary<int, LRUSandBox> m_dictDirectoryLRU;
	private List<int> m_listInitializedDirectory;

	public DiskFileManager()
	{
		m_ider = GetIDer();
		m_filesUsedRecordRootPath = "FilesUsedRecord";
		FileDirectoryHandler.CreateDirectory(m_filesUsedRecordRootPath);
		m_dictFilesUsedDetail = new Dictionary<int, FilesUsedDetail>();
		m_dictDirectoryLRU = new Dictionary<int, LRUSandBox>();
		m_listInitializedDirectory = new List<int>();
	}

	public void InitializeLRUDirectory(string path, int capacity, int server_time, bool parent_is_lru, bool lru_file_or_directory)
	{
		int id = GetID(path);
		if (id > 0)
		{
			if (DirectoryIsInitialized(id)) return;
			string directoryName = FileDirectoryHandler.GetDirectoryNameFromPath(path);
			LRUSandBox lruSandBox = new LRUSandBox(capacity, lru_file_or_directory);
			bool isExist = FileDirectoryHandler.ExistDirectory(path);
			if (isExist)
			{
				string filesUsedRecordPath = GetFileSystemUsedRecordPath(id, directoryName);
				bool isInitializeLRU = false;
				if (FileDirectoryHandler.ExistFile(filesUsedRecordPath))
				{
					FilesUsedDetail filesUsedDetail = MyXmlSerializer.Deserialize<FilesUsedDetail>(filesUsedRecordPath);
					if (filesUsedDetail != null)
					{
						m_dictFilesUsedDetail.Add(id, filesUsedDetail);
						filesUsedDetail.Clean();
						filesUsedDetail.Sort();
						FilesUsedDetail.FileUsedDetail[] sFileUsedDetail = filesUsedDetail.Details;
						for (int i = 0; i < sFileUsedDetail.Length; i++)
						{
							FilesUsedDetail.FileUsedDetail fileUsedDetail = sFileUsedDetail[i];
							lruSandBox.Add(fileUsedDetail.id, fileUsedDetail.path);
						}
						isInitializeLRU = true;
					}
					else
					{
						RO.LoggerUnused.LogWarning("Files Used record deserialize from .xml fail, path is" + path);
					}
				}
				else
				{
					RO.LoggerUnused.LogWarning ("Files used record .xml file not exists, path is " + path);
				}
				if (!isInitializeLRU)
				{
					string[] childrenFileSystemPath = FileDirectoryHandler.GetChildrenPath(path);
					List<FilesUsedDetail.FileUsedDetail> listFileUsedDetail = new List<FilesUsedDetail.FileUsedDetail>();
					for (int i = 0; i < childrenFileSystemPath.Length; i++)
					{
						string childPath = childrenFileSystemPath[i];
						int idChild = GetID(childPath);
						DateTime serverTime = server_time < 0 ? DateTime.Now : DiskFileManager.ToDateTime(server_time);
						FilesUsedDetail.FileUsedDetail fileUsedDetail = new FilesUsedDetail.FileUsedDetail(idChild, childPath, serverTime);
						listFileUsedDetail.Add(fileUsedDetail);
						lruSandBox.Add(idChild, childPath);
					}
					FilesUsedDetail filesUsedDetial = new FilesUsedDetail(path, listFileUsedDetail.ToArray());
					m_dictFilesUsedDetail.Add(id, filesUsedDetial);
					SaveFileSystemUsedTime ();
				}
			}
			else
			{
				FileDirectoryHandler.CreateDirectory(path);
				FilesUsedDetail filesUsedDetail = new FilesUsedDetail(path);
				m_dictFilesUsedDetail.Add(id, filesUsedDetail);
			}
			m_dictDirectoryLRU.Add(id, lruSandBox);

			if (parent_is_lru) LRUParent(path, server_time);

			m_listInitializedDirectory.Add(id);
		}
	}

	public void InitializeDirectory(string path, int server_time, bool parent_is_lru)
	{
		int id = GetID(path);
		if (DirectoryIsInitialized(id)) return;
		bool isExist = FileDirectoryHandler.ExistDirectory(path);
		if (!isExist)
		{
			FileDirectoryHandler.CreateDirectory(path);
		}
		
		if (parent_is_lru) LRUParent(path, server_time);

		m_listInitializedDirectory.Add(id);
	}

	private void SaveIDer()
	{
		PlayerPrefs.SetInt("FILE_DIRECTORY_ID", m_ider);
	}

	private int GetIDer()
	{
		return PlayerPrefs.GetInt("FILE_DIRECTORY_ID");
	}

	private int GetID(string path)
	{
		if (string.IsNullOrEmpty(path)) return 0;
		int id = PlayerPrefs.GetInt(path, 0);
		if (id > 0) return id;
		id = ++m_ider;
		SaveIDer();
		PlayerPrefs.SetInt(path, id);
		return id;
	}

	private void SaveFileSystemUsedTime()
	{
		if (m_dictFilesUsedDetail != null && m_dictFilesUsedDetail.Count > 0)
		{
			foreach (KeyValuePair<int, FilesUsedDetail> pair in m_dictFilesUsedDetail)
			{
				int id = pair.Key;
				FilesUsedDetail filesUsedDetail = pair.Value;
				string path = filesUsedDetail.Path;
				string directoryName = FileDirectoryHandler.GetDirectoryNameFromPath(path);
				string fileSystemUsedRecordPath = GetFileSystemUsedRecordPath(id, directoryName);
				MyXmlSerializer.Serialize<FilesUsedDetail>(filesUsedDetail, fileSystemUsedRecordPath);
			}
		}
	}

	private string GetFileSystemUsedRecordPath(int id, string directory_name)
	{
		return m_filesUsedRecordRootPath + "/" + id + "_" + directory_name + ".xml";
	}

	public bool SaveFile(string path, byte[] bytes, int server_time)
	{
		bool[] bs = FileDirectoryHandler.WriteFile(path, bytes);
		if (bs != null && bs.Length > 0)
		{
			bool isWrited = bs[1];
			if (isWrited)
			{
				LRUParent(path, server_time);
			}
			return isWrited;
		}
		return false;
	}

	public void SaveFile(string path, byte[] bytes, int server_time, Action<bool> on_complete)
	{
		FileDirectoryHandler.WriteFile(path, bytes, (x, y) => {
			if (y)
			{
				LRUParent(path, server_time);
			}
			if (on_complete != null)
				on_complete(y);
		});
	}

	// require Compatibility Version 14
	public bool SaveTexture2D(string path, Texture2D texture2D, int server_time)
	{
		if (server_time > 0)
		{
			if (texture2D != null) {
				byte[] bytes = texture2D.EncodeToPNG ();
				return SaveFile (path, bytes, server_time);
			}
		}
		return false;
	}

	// require Compatibility Version 14
	public void SaveTexture2DAsync(string path, Texture2D texture2D, int server_time, Action<bool> on_complete)
	{
		bool bCallSave = false;
		if (server_time > 0)
		{
			if (texture2D != null) {
				byte[] bytes = texture2D.EncodeToPNG ();
				SaveFile (path, bytes, server_time, on_complete);
				bCallSave = true;
			}
		}
		if (!bCallSave) {
			if (on_complete != null) {
				on_complete (false);
			}
		}
	}

	public byte[] LoadFile(string path, int server_time)
	{
		byte[] bytes = FileDirectoryHandler.LoadFile(path);
		if (bytes != null)
		{
			LRUParent(path, server_time);
		}
		return bytes;
	}

	// require Compatibility Version 14
	public Texture2D LoadTexture2D(string path, int server_time)
	{
		Texture2D texture2D = Texture2DLoader.Load (path);
		if (texture2D != null) {
			LRUParent (path, server_time);
		}
		return texture2D;
	}

	public void LoadFileAsync(string path, int use_time, Action<byte[]> on_complete)
	{
		FileHelper.LoadFileAsync(path, (x) => {
			LRUParent(path, use_time);
			byte[] bytes = x;
			if (on_complete != null)
			{
				FunctionsCallerInMainThread.ins.Call(() => {
					on_complete(bytes);
				});
			}
		});
	}

	public void LRUParent(string path, int server_time)
	{
		//Logger.Log(string.Format("FUN >>> DiskFileManager:LRUParent, <param>path = {0}</param", path));
		string parentDirectoryPath = FileDirectoryHandler.GetParentDirectoryPath(path);
		if (!string.IsNullOrEmpty(parentDirectoryPath))
		{
			int parentDirectoryID = GetID(parentDirectoryPath);
			if (parentDirectoryID > 0)
			{
				int id = GetID(path);
				if (m_dictFilesUsedDetail.ContainsKey(parentDirectoryID))
				{
					FilesUsedDetail filesUsedDetail = m_dictFilesUsedDetail[parentDirectoryID];
					DateTime serverTime = server_time < 0 ? DateTime.Now : DiskFileManager.ToDateTime(server_time);
					FilesUsedDetail.FileUsedDetail fileUsedDetail = new FilesUsedDetail.FileUsedDetail(id, path, serverTime);
					if (!filesUsedDetail.ExistDetail(id))
					{
						filesUsedDetail.AddDetail(fileUsedDetail);
					}
					else
					{
						filesUsedDetail.UpdateDetail(id, fileUsedDetail);
					}
					SaveFileSystemUsedTime();
				}
				if (m_dictDirectoryLRU.ContainsKey(parentDirectoryID))
				{
					LRUSandBox lruSandBox = m_dictDirectoryLRU[parentDirectoryID];
					if (lruSandBox.Exist(id))
					{
						lruSandBox.MakeActive(id);
					}
					else
					{
						lruSandBox.Add(id, path);
					}
				}
			}
		}
	}
	
	public static DateTime ToDateTime(int to_greenwich_seconds)
	{
		DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
		dt = TimeZone.CurrentTimeZone.ToLocalTime (dt);
		return dt.AddSeconds(to_greenwich_seconds);
	}

	private bool DirectoryIsInitialized(int id)
	{
		return m_listInitializedDirectory.Contains(id);
	}

	public void Reset()
	{
		if (m_dictDirectoryLRU != null) {
			m_dictDirectoryLRU.Clear ();
		}
		if (m_dictFilesUsedDetail != null) {
			m_dictFilesUsedDetail.Clear ();
		}
		if (m_listInitializedDirectory != null) {
			m_listInitializedDirectory.Clear ();
		}
	}
}
