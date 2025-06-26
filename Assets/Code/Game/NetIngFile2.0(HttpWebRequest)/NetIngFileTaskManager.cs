using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;

[SLua.CustomLuaClassAttribute]
public sealed class NetIngFileTaskManager : RO.SingleTonGO<NetIngFileTaskManager>
{
	public static NetIngFileTaskManager Ins
	{
		get
		{
			return Me;
		}
	}

	private List<TaskInfo> m_taskInfoList;
	List<HTTPFileLoader> m_loaderList;
	private ListQueue<HTTPFileLoader> m_loaderQueue;
	private int m_ticketCount;

	public string RootDirectoryPath
	{
		get
		{
			string path = "Download";
			if (!FileDirectoryHandler.ExistDirectory(path))
			{
				FileDirectoryHandler.CreateDirectory(path);
			}
			return FileDirectoryHandler.GetAbsolutePath(path);
		}
	}

	protected override void Awake ()
	{
		Initialize();
		base.Awake ();
	}

	private void Initialize()
	{
		m_taskInfoList = new List<TaskInfo>();
		m_loaderList = new List<HTTPFileLoader>();
		m_loaderQueue = new ListQueue<HTTPFileLoader>();
		m_ticketCount = ServicePointManager.DefaultConnectionLimit;
	}

	public void Close()
	{
		if (m_loaderList != null && m_loaderList.Count > 0)
		{
			for (int i = 0; i < m_loaderList.Count; i++)
			{
				HTTPFileLoader loader = m_loaderList[i];
				loader.Stop();
				loader = null;
			}
		}
		m_loaderList.Clear();
	}

	public int Download(
		string url,
		bool check_md5,
		Action action_on_start,
		Action<float> action_on_progress,
		Action<string> action_on_complete,
		Action<int, int, string> action_on_error,
		string[] custom_headers = null)
	{
		HTTPFileDownloader loader = new HTTPFileDownloader(url, ArrayToDictionary(custom_headers), () => {
			if (action_on_start != null)
			{
				action_on_start();
			}
		}, (x) => {
			if (action_on_progress != null)
			{
				action_on_progress(x);
			}
		}, (x) => {
			m_ticketCount++;

			if (action_on_complete != null)
			{
				action_on_complete(x);
			}
		}, (x, y, z) => {
			m_ticketCount++;

			if (action_on_error != null)
			{
				action_on_error(x, y, z);
			}
		});
		loader.BCheckMD5 = check_md5;

		DownloadTaskInfo info = new DownloadTaskInfo();
		info.id = loader.ID;
		info.url = loader.URL;
		info.path = loader.Path;
		m_taskInfoList.Add(info);

		if (m_ticketCount > 0)
		{
			m_loaderList.Add(loader);
			loader.Start();
			m_ticketCount--;

			info.state = (TaskInfo.E_State)loader.State;
		}
		else
		{
			m_loaderQueue.Add(loader);

			info.state = TaskInfo.E_State.Waiting;
		}
		return loader.ID;
	}

	public int Upload(string path, string url, string content_type, Action action_on_start, Action<float> action_on_progress, Action<string> action_on_complete, Action<int, int, string> action_on_error, string[] custom_headers = null)
	{
		HTTPFileLoader loader = new HTTPFileUploader(path, url, ArrayToDictionary(custom_headers), content_type, () => {
			if (action_on_start != null)
			{
				action_on_start();
			}
		}, (x) => {
			if (action_on_progress != null)
			{
				action_on_progress(x);
			}
		}, (x) => {
			if (action_on_complete != null)
			{
				action_on_complete(x);
			}
		}, (x, y, z) => {
			if (action_on_error != null)
			{
				action_on_error(x, y, z);
			}
		});

		UploadTaskInfo info = new UploadTaskInfo();
		info.id = loader.ID;
		info.url = loader.URL;
		m_taskInfoList.Add(info);

		if (m_ticketCount > 0)
		{
			m_loaderList.Add(loader);
			loader.Start();
			m_ticketCount--;

			info.state = (TaskInfo.E_State)loader.State;
		}
		else
		{
			m_loaderQueue.Add(loader);

			info.state = TaskInfo.E_State.Waiting;
		}
		return loader.ID;
	}

	private DownloadTaskInfo GetDownloadTaskInfoFromID(int id)
	{
		bool isWaiting = false;
		HTTPFileDownloader loader = m_loaderList.Find(x => x.ID == id) as HTTPFileDownloader;
		if (loader == null)
		{
			loader = m_loaderQueue.Find(x => x.ID == id) as HTTPFileDownloader;
			if (loader == null)
			{
				return null;
			}
			isWaiting = true;
		}
		DownloadTaskInfo info = m_taskInfoList.Find(x => x.id == id) as DownloadTaskInfo;
		if (info == null)
		{
			info = new DownloadTaskInfo();
			info.id = loader.ID;
			info.url = loader.URL;
			info.path = loader.Path;
		}
		info.progress = loader.Progress;
		info.state = isWaiting ? TaskInfo.E_State.Waiting : (TaskInfo.E_State)loader.State;
		return info;
	}

	private UploadTaskInfo GetUploadTaskInfoFromID(int id)
	{
		bool isWaiting = false;
		HTTPFileUploader loader = m_loaderList.Find(x => x.ID == id) as HTTPFileUploader;
		if (loader == null)
		{
			loader = m_loaderQueue.Find(x => x.ID == id) as HTTPFileUploader;
			if (loader == null)
			{
				return null;
			}
			isWaiting = true;
		}
		UploadTaskInfo info = m_taskInfoList.Find(x => x.id == id) as UploadTaskInfo;
		if (info == null)
		{
			info = new UploadTaskInfo();
			info.id = loader.ID;
			info.url = loader.URL;
		}
		info.progress = loader.Progress;
		info.state = isWaiting ? TaskInfo.E_State.Waiting : (TaskInfo.E_State)loader.State;
		info.fileURL = loader.FileURL;
		return info;
	}

	public void PauseTaskFromID(int id)
	{
		HTTPFileLoader loader = m_loaderList.Find(x => x.ID == id);
		if (loader != null)
		{
			loader.Pause();
		}
	}

	public void ContinueTaskFromID(int id)
	{
		HTTPFileLoader loader = m_loaderList.Find(x => x.ID == id);
		if (loader != null)
		{
			loader.Continue();
		}
	}

	public void RemoveTaskFromID(int id)
	{
		HTTPFileLoader loader = m_loaderList.Find(x => x.ID == id);
		if (loader != null)
		{
			loader.Stop();
			m_loaderList.Remove(loader);
			return;
		}
		loader = m_loaderQueue.Find(x => x.ID == id);
		if (loader != null)
		{
			loader.Stop();
			m_loaderQueue.Remove(loader);
		}
	}

	void Update()
	{
		if (m_ticketCount > 0 && m_loaderQueue.Count > 0)
		{
			HTTPFileLoader loader = m_loaderQueue.Peek();
			HTTPFileLoader.E_State state = loader.State;
			if (state == HTTPFileLoader.E_State.None)
			{
				loader.Start();
				m_ticketCount--;
				m_loaderQueue.Dequeue();
			}
		}

		if (m_loaderList != null && m_loaderList.Count > 0) {
			for (int i = m_loaderList.Count - 1; i >= 0; --i) {
				HTTPFileLoader loader = m_loaderList[i];
				if (loader != null) {
					if (loader.m_bS) {
						loader.FireOnStart ();
						loader.m_bS = false;
					}
					if (loader.m_bP) {
						loader.FireOnProgress ();
						loader.m_bP = false;
					}
					if (loader.m_bC) {
						loader.FireOnComplete ();
						loader.m_bC = false;
					}
					if (loader.m_bE) {
						loader.FireOnError ();
						loader.m_bE = false;
					}
					if (loader.State == HTTPFileLoader.E_State.End || loader.State == HTTPFileLoader.E_State.Err)
					{
						m_loaderList.Remove(loader);
						loader = null;
					}
				}
			}
		}

//		if (Application.internetReachability == NetworkReachability.NotReachable)
//		{
//			if (m_loaderList != null && m_loaderList.Count > 0) {
//				for (int i = m_loaderList.Count - 1; i >= 0; --i) {
//					HTTPFileLoader loader = m_loaderList[i];
//					if (loader.State == HTTPFileLoader.E_State.Ing || loader.State == HTTPFileLoader.E_State.Pause) {
//						loader.Stop();
//						loader.FireOnError();
//					}
//				}
//			}
//			Close();
//		}
	}

	protected override void OnDestroy ()
	{
		Close();
		base.OnDestroy ();
	}

	public static Dictionary<string, string> ArrayToDictionary(string[] arr)
	{
		if (arr != null && arr.Length > 0) {
			Dictionary<string, string> retValue = new Dictionary<string, string> ();
			int i = 0;
			while (i < arr.Length) {
				string value = null;
				if (i + 1 < arr.Length) {
					value = arr [i + 1];
				}
				retValue.Add (arr [i], value);
				i = i + 2;
			}
			return retValue;
		}
		return null;
	}
}