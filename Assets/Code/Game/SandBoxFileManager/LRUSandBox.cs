using UnityEngine;
using System.Collections;
using Ghost.Utility;
using System.Collections.Generic;
using RO;

public class LRUSandBox
{
	public static int CapacityMin = 1;
	public static int DeterminCapacity(int capacity)
	{
		return Mathf.Max(CapacityMin, capacity);
	}
	
	private List<int> usedTimeline;
	private Dictionary<int, string> filesPath;
	
	private int capacity_ = 0;
	public int capacity
	{
		get
		{
			return capacity_;
		}
		set
		{
			if (value == capacity)
			{
				return;
			}
			capacity_ = value;
			if (0 < capacity)
			{
				var removeCount = usedTimeline.Count - capacity;
				if (0 < removeCount)
				{
					for (int i = 0; i < removeCount; ++i)
					{
						DeleteFromSandBox(usedTimeline[i]);
					}
					usedTimeline.RemoveRange(0, removeCount);
				}
			}
			else
			{
				Clear();
			}
		}
	}

	public int pathsCount
	{
		get
		{
			return filesPath.Count;
		}
	}

	private bool m_fileOrDirectory;

	public LRUSandBox(int capacity, bool lru_file_or_directory)
	{
		capacity = DeterminCapacity(capacity);
		usedTimeline = new List<int>(capacity);
		filesPath = new Dictionary<int, string>(capacity);
		capacity_ = capacity;
		m_fileOrDirectory = lru_file_or_directory;
	}

	public void Add (int id, string path)
	{
		//Logger.Log(string.Format("FUN >>> Add, <param>id = {0}, path = {1}</param>", id, path));
		filesPath.Add(id, path);
		usedTimeline.Add(id);
		if (pathsCount > capacity)
		{
			int lru = usedTimeline[0];
			Delete(lru);
		}
	}

	public bool MakeActive(int id)
	{
		if (id <= 0) return false;
		int index = usedTimeline.IndexOf(id);
		if (index >= 0)
		{
			usedTimeline.RemoveAt(index);
			usedTimeline.Add(id);
			return true;
		}
		return false;
	}

	public bool Delete(int id)
	{
		//Logger.Log(string.Format("FUN >>> LRUSandBox:Delete, <param>id = {0}</param>", id));
		if (DeleteFromSandBox(id))
		{
			usedTimeline.Remove(id);
			return true;
		}
		return false;
	}

	private bool DeleteFromSandBox(int id)
	{
		//Logger.Log(string.Format("FUN >>> LRUSandBox:DeleteFromSandBox, <param>id = {0}</param>", id));
		string path;
		if (filesPath.TryGetValue(id, out path))
		{
			DeleteFile(path);
			filesPath.Remove(id);
			return true;
		}
		return false;
	}

	public void Clear()
	{
		DeleteAllFiles();
		filesPath.Clear();
		usedTimeline.Clear();
	}

	private void DeleteFile(string path)
	{
		//Logger.Log(string.Format("FUN >>> LRUSandBox:DeleteFile, <param>path = {0}</param>", path));
		if (m_fileOrDirectory)
		{
			FileDirectoryHandler.DeleteFile(path);
		}
		else
		{
			FileDirectoryHandler.DeleteDirectory(path);
		}
	}

	private void DeleteAllFiles()
	{
		foreach (var keyValuePair in filesPath)
		{
			DeleteFile(keyValuePair.Value);
		}
	}

	public bool Exist(int id)
	{
		if (id <= 0) return false;
		int index = usedTimeline.IndexOf(id);
		if (index >= 0) return true;
		return false;
	}
}
