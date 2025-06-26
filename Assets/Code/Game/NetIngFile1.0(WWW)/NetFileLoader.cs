using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class WWWDownloader : MonoSingleton<WWWDownloader>
{
	private class CR_Loader
	{
		public enum E_State
		{
			None,
			Ing,
			End,
			Err
		}

		public enum E_Error
		{
			None,
			WWWErr
		}

		private const int UNION_LENGTH = 1024 * 1024;
		private static int m_count;
		public static int Count
		{
			get {return m_count;}
		}

		private int m_id;
		public int ID
		{
			get {return m_id;}
		}
		public string m_url;
		public string m_fileName;
		public string m_path;
		public Action<float> m_actionOnProgress;
		public Action<byte[], string, int, string> m_actionOnComplete;
		public byte[] m_buffer;
		public E_State m_state;
		public float m_progress;
		public E_Error m_err;
		public string m_errMessage;
		public WWW m_www;
		public IEnumerator m_iEnumerator;

		public CR_Loader(string url, Action<float> action_on_progress, Action<byte[], string, int, string> action_on_complete)
		{
			m_id = ++m_count;
			m_url = url;
			string[] urlSplitBySlash = m_url.Split('/');
			if (urlSplitBySlash != null && urlSplitBySlash.Length > 0)
				m_fileName = urlSplitBySlash[urlSplitBySlash.Length - 1];
			m_path = WWWDownloader.ins.ROOT_PATH + m_fileName;
			m_actionOnProgress = action_on_progress;
			m_actionOnComplete = action_on_complete;
			m_buffer = null;
			m_state = E_State.None;
			m_progress = 0;
			m_err = E_Error.None;
			m_errMessage = "";
			m_iEnumerator = IE_Load();
		}

		public IEnumerator IE_Load()
		{
			m_www = new WWW(m_url);
			m_state = E_State.Ing;
			while (!m_www.isDone)
			{
				m_progress = m_www.progress;
				FireOnProgress();
				yield return 0;
			}
			
			if (m_www.error != null)
			{
				m_state = E_State.Err;
				m_progress = m_www.progress;
				m_err = E_Error.WWWErr;
				m_errMessage = m_www.error;
				FireOnComplete();
				yield break;
			}

			byte[] bytes = m_www.bytes;
			FileStream fs;
			if (File.Exists(m_path))
			{
				FileInfo fi = new FileInfo(m_path);
				fs = fi.Open(FileMode.Open, FileAccess.Write);
			}
			else
			{
				fs = File.Create(m_path);
			}
			int indicator = 0;
			while (indicator < bytes.Length)
			{
				int remainSize = bytes.Length - indicator;
				if (remainSize < UNION_LENGTH)
				{
					fs.Write(bytes, indicator, remainSize);
				}
				else
				{
					fs.Write(bytes, indicator, UNION_LENGTH);
				}
				indicator += UNION_LENGTH;
				yield return 0;
			}
			fs.Close();
			fs.Dispose();

			m_state = E_State.End;
			m_progress = 1;
			FireOnProgress();
			FireOnComplete();
			m_www.Dispose();
		}
		
		public void FireOnComplete()
		{
			if (m_actionOnComplete != null) m_actionOnComplete(m_buffer, m_path, (int)m_err, m_errMessage);
		}

		public void FireOnProgress()
		{
			if (m_actionOnProgress != null) m_actionOnProgress(m_progress);
		}

		public void Dispose()
		{
			if (m_www != null)
			{
				m_www.Dispose();
			}
		}
	}

	public class LoaderInfo
	{
		public int id;
		public string url;
		public string fileName;
		public string path;
		public float progress;
	}

	public string ROOT_PATH = Application.persistentDataPath + "/Download/";
	private ListQueue<CR_Loader> m_loaderQueue;

	public void Open()
	{
		bool rootDirectoryIsExist = Directory.Exists(ROOT_PATH);
		if (!rootDirectoryIsExist)
		{
			Directory.CreateDirectory(ROOT_PATH);
		}
		m_loaderQueue = new ListQueue<CR_Loader>();
	}

	/// <summary>
	/// Load file from the specified url
	/// </summary>
	/// <param name="action_on_progress">Action_on_progress<progress>.</param>
	/// <param name="action_on_complete">Action_on_complete<bytes, path, errorCode, errorMessage>.</param>
	public int Load(string url, Action<float> action_on_progress, Action<byte[], string, int, string> action_on_complete)
	{
		CR_Loader loader = new CR_Loader(url, action_on_progress, action_on_complete);
		StartCoroutine(loader.m_iEnumerator);
		m_loaderQueue.Enqueue(loader);
		return loader.ID;
	}

	public void Stop(int id)
	{
		CR_Loader loader = GetLoaderFromID(id);
		if (loader != null && loader.m_state == CR_Loader.E_State.Ing)
		{
			StopCoroutine(loader.m_iEnumerator);
			loader.Dispose();
			m_loaderQueue.Remove(loader);
		}
	}

	void LateUpdate()
	{
//		if (m_loaderQueue.Count > 0)
//		{
//			bool b = true;
//			while (b)
//			{
//				CR_Loader loader = m_loaderQueue.Peek();
//				CR_Loader.E_State state = loader.m_state;
//				if (state == CR_Loader.E_State.End || state == CR_Loader.E_State.Err)
//				{
//					m_loaderQueue.Dequeue();
//					b = m_loaderQueue.Count > 0;
//				}
//				else if (state == CR_Loader.E_State.None)
//				{
//					StartCoroutine(loader.m_iEnumerator);
//					b = false;
//				}
//				else
//				{
//					b = false;
//				}
//			}
//		}
	}

	private CR_Loader GetLoaderFromID(int id)
	{
		CR_Loader loader = null;
		for (int i = 0; i < m_loaderQueue.Count; i++)
		{
			CR_Loader itemLoader = m_loaderQueue[i];
			if (itemLoader.ID == id)
			{
				loader = itemLoader;
				break;
			}
		}
		return loader;
	}

	public LoaderInfo GetLoaderInfoFromID(int id)
	{
		CR_Loader loader = GetLoaderFromID(id);
		if (loader == null) return null;
		LoaderInfo loaderInfo = new LoaderInfo();
		loaderInfo.id = loader.ID;
		loaderInfo.url = loader.m_url;
		loaderInfo.fileName = loader.m_fileName;
		loaderInfo.path = loader.m_path;
		loaderInfo.progress = loader.m_progress;
		return loaderInfo;
	}
}

public class WWWUploader : MonoSingleton<WWWUploader>
{
	public class CR_Loader
	{
		public enum E_State
		{
			None,
			Ing,
			End,
			Err
		}
		
		public enum E_Error
		{
			None,
			FileNoExist,
			WWWErr
		}

		private const int UNION_LENGTH = 1024 * 1024;
		private static int m_count;
		public static int Count
		{
			get {return m_count;}
		}
		
		private int m_id;
		public int ID
		{
			get {return m_id;}
		}
		public string m_url;
		public string m_fileName;
		public string m_path;
		public Action<float> m_actionOnProgress;
		public Action<int, string> m_actionOnComplete;
		public E_State m_state;
		public float m_progress;
		public E_Error m_err;
		public string m_errMessage;
		public WWW m_www;
		public IEnumerator m_iEnumerator;
		
		public CR_Loader(string path, string url, Action<float> action_on_progress, Action<int, string> action_on_complete)
		{
			m_id = ++m_count;
			m_path = Application.dataPath + "/" + path;
			m_url = url;
			string[] pathSplitBySlash = m_path.Split('/');
			if (pathSplitBySlash != null && pathSplitBySlash.Length > 0)
				m_fileName = pathSplitBySlash[pathSplitBySlash.Length - 1];
			m_actionOnProgress = action_on_progress;
			m_actionOnComplete = action_on_complete;
			m_state = E_State.None;
			m_progress = 0;
			m_err = E_Error.None;
			m_errMessage = "";
			m_iEnumerator = IE_Load();
		}
		
		public IEnumerator IE_Load()
		{
			bool isExist = File.Exists(m_path);
			if (!isExist)
			{
				m_state = E_State.Err;
				m_err = E_Error.FileNoExist;
				m_errMessage = "File don't exist in the specified path.";
				FireOnComplete();
				yield break;
			}
			FileInfo fi = new FileInfo(m_path);
			int size = (int)fi.Length;
			FileStream fs = fi.Open(FileMode.Open, FileAccess.Read);
			byte[] bytes = new byte[size];
			int indicator = 0;
			while (indicator < size)
			{
				int remainSize = size - indicator;
				if (remainSize < UNION_LENGTH)
				{
					fs.Read(bytes, indicator, remainSize);
				}
				else
				{
					fs.Read(bytes, indicator, UNION_LENGTH);
				}
				indicator += UNION_LENGTH;
				yield return 0;
			}
			fs.Close();
			fs.Dispose();

			WWWForm wwwForm = new WWWForm();
			wwwForm.AddBinaryData("file", bytes, m_fileName);
			m_www = new WWW(m_url, wwwForm);
			while (!m_www.isDone)
			{
				m_state = E_State.Ing;
				m_progress = m_www.progress;
				FireOnProgress();
				yield return 0;
			}
			if (!string.IsNullOrEmpty(m_www.error))
			{
				m_state = E_State.Err;
				m_progress = m_www.progress;
				m_err = E_Error.WWWErr;
				m_errMessage = m_www.error;
				FireOnComplete();
				yield break;
			}
			m_state = E_State.End;
			m_progress = 1;
			FireOnComplete();
			m_www.Dispose();
		}
		
		public void FireOnComplete()
		{
			if (m_actionOnComplete != null) m_actionOnComplete((int)m_err, m_errMessage);
		}
		
		public void FireOnProgress()
		{
			if (m_actionOnProgress != null) m_actionOnProgress(m_progress);
		}

		public void Dispose()
		{
			if (m_www != null)
			{
				m_www.Dispose();
			}
		}
	}

	public class LoaderInfo
	{
		public int id;
		public string url;
		public string fileName;
		public string path;
		public float progress;
	}

	private ListQueue<CR_Loader> m_loaderQueue;
	
	public void Open()
	{
		m_loaderQueue = new ListQueue<CR_Loader>();
	}
	
	/// <summary>
	/// Load file to the specified url
	/// </summary>
	/// <param name="action_on_progress">Action_on_progress<progress>.</param>
	/// <param name="action_on_complete">Action_on_complete<errorCode, errorMessage>.</param>
	public int Load(string path, string url, Action<float> action_on_progress, Action<int, string> action_on_complete)
	{
		CR_Loader loader = new CR_Loader(path, url, action_on_progress, action_on_complete);
		StartCoroutine(loader.m_iEnumerator);
		//m_loaderQueue.Enqueue(loader);
		return loader.ID;
	}

	public void Stop(int id)
	{
		CR_Loader loader = GetLoaderFromID(id);
		if (loader != null && loader.m_state == CR_Loader.E_State.Ing)
		{
			StopCoroutine(loader.m_iEnumerator);
			loader.Dispose();
			m_loaderQueue.Remove(loader);
		}
	}

	void LateUpdate()
	{
//		if (m_loaderQueue.Count > 0)
//		{
//			bool b = true;
//			while (b)
//			{
//				CR_Loader loader = m_loaderQueue.Peek();
//				CR_Loader.E_State state = loader.m_state;
//				if (state == CR_Loader.E_State.End || state == CR_Loader.E_State.Err)
//				{
//					m_loaderQueue.Dequeue();
//					b = m_loaderQueue.Count > 0;
//				}
//				else if (state == CR_Loader.E_State.None)
//				{
//					StartCoroutine(loader.m_iEnumerator);
//					b = false;
//				}
//				else
//				{
//					b = false;
//				}
//			}
//		}
	}

	public CR_Loader GetLoaderFromID(int id)
	{
		CR_Loader loader = null;
		for (int i = 0; i < m_loaderQueue.Count; i++)
		{
			CR_Loader itemLoader = m_loaderQueue[i];
			if (itemLoader.ID == id)
			{
				loader = itemLoader;
				break;
			}
		}
		return loader;
	}

	public LoaderInfo GetLoaderInfoFromID(int id)
	{
		CR_Loader loader = GetLoaderFromID(id);
		if (loader == null) return null;
		LoaderInfo loaderInfo = new LoaderInfo();
		loaderInfo.id = loader.ID;
		loaderInfo.url = loader.m_url;
		loaderInfo.fileName = loader.m_fileName;
		loaderInfo.path = loader.m_path;
		loaderInfo.progress = loader.m_progress;
		return loaderInfo;
	}
}