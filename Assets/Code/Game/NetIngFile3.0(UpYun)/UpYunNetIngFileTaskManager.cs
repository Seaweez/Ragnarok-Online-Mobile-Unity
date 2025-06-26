using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using RO;

[SLua.CustomLuaClassAttribute]
public class UpYunNetIngFileTaskManager : MonoSingleton<UpYunNetIngFileTaskManager>
{
	public static UpYunNetIngFileTaskManager Ins {
		get { return ins;}
	}

	private List<UpYunTaskInfo> m_taskInfoList;

	public UpYunTaskInfo[] TasksInfo {
		get { return m_taskInfoList.ToArray ();}
	}

	private List<UpYunHTTPFileLoader> m_loaderList;
	private ListQueue<UpYunHTTPFileLoader> m_loaderQueue;
	private int m_ticketCount;

	public void Open()
	{
		m_taskInfoList = new List<UpYunTaskInfo> ();
		m_loaderList = new List<UpYunHTTPFileLoader> ();
		m_loaderQueue = new ListQueue<UpYunHTTPFileLoader> ();
		m_ticketCount = 10;
	}
	
	public void Close()
	{
		if (m_loaderList != null && m_loaderList.Count > 0) {
			for (int i = 0; i < m_loaderList.Count; i++) {
				UpYunHTTPFileLoader loader = m_loaderList [i];
				loader.Stop ();
				loader = null;
			}
			m_loaderList.Clear ();
		}
	}

	public int Download(string server_path, string local_path, Action action_on_start, Action<float> action_on_progress, Action action_on_complete, Action<int, int, string> action_on_error, string[] custom_headers = null)
	{
		UpYunHTTPFileDownloader loader = new UpYunHTTPFileDownloader (server_path, local_path, ArrayToDictionary(custom_headers), () => {
			if (action_on_start != null) {
				action_on_start ();
			}
		}, (x) => {
			if (action_on_progress != null) {
				action_on_progress (x);
			}
		}, () => {
			m_ticketCount++;
			
			if (action_on_complete != null) {
				action_on_complete ();
			}
		}, (x, y, z) => {
			m_ticketCount++;
			
			if (action_on_error != null) {
				action_on_error (x, y, z);
			}
		});
		
		UpYunDownloadTaskInfo info = new UpYunDownloadTaskInfo ();
		info.id = loader.ID;
		info.path = loader.Path;
		m_taskInfoList.Add (info);
		
		if (m_ticketCount > 0) {
			m_loaderList.Add (loader);
			loader.Start ();
			m_ticketCount--;
			
			info.state = (UpYunTaskInfo.E_State)loader.State;
		} else {
			m_loaderQueue.Add (loader);
			
			info.state = UpYunTaskInfo.E_State.Waiting;
		}
		return loader.ID;
	}
	
	public int Upload (string path, string url, Action action_on_start, Action<float> action_on_progress, Action action_on_complete, Action<int, int, string> action_on_error, string[] custom_headers = null)
	{
		UpYunHTTPFileUploader loader = new UpYunHTTPFileUploader (path, url, ArrayToDictionary(custom_headers), () => {
			if (action_on_start != null) {
				action_on_start ();
			}
		}, (x) => {
			m_ticketCount++;
			if (action_on_progress != null) {
				action_on_progress (x);
			}
		}, () => {
			m_ticketCount++;
			if (action_on_complete != null) {
				action_on_complete ();
			}
		}, (x, y, z) => {
			if (action_on_error != null) {
				action_on_error (x, y, z);
			}
		});
		
		UpYunUploadTaskInfo info = new UpYunUploadTaskInfo ();
		info.id = loader.ID;
		info.path = loader.Path;
		m_taskInfoList.Add (info);
		
		if (m_ticketCount > 0) {
			m_loaderList.Add (loader);
			loader.Start ();
			m_ticketCount--;
			
			info.state = (UpYunTaskInfo.E_State)loader.State;
		} else {
			m_loaderQueue.Add (loader);
			
			info.state = UpYunTaskInfo.E_State.Waiting;
		}
		return loader.ID;
	}

	public int Upload(string path, string signature, string policy, Action action_on_start, Action<float> action_on_progress, Action action_on_complete, Action<int, int, string> action_on_error, string[] custom_headers = null)
	{
		UpYunHTTPFileBlocksUploader loader = new UpYunHTTPFileBlocksUploader(path, ArrayToDictionary(custom_headers), signature, policy, () => {
			if (action_on_start != null) {
				action_on_start ();
			}
		}, (x) => {
			if (action_on_progress != null) {
				action_on_progress (x);
			}
		}, () => {
			m_ticketCount++;
			if (action_on_complete != null) {
				action_on_complete ();
			}
		}, (x, y, z) => {
			m_ticketCount++;
			if (action_on_error != null) {
				action_on_error (x, y, z);
			}
		});

		UpYunBlocksUploadTaskInfo info = new UpYunBlocksUploadTaskInfo ();
		info.id = loader.ID;
		info.path = loader.Path;
		m_taskInfoList.Add (info);
		
		if (m_ticketCount > 0) {
			m_loaderList.Add(loader);
			loader.Initialize();
			if (loader.State != UpYunHTTPFileLoader.E_State.Err)
			{
				loader.Start();
				m_ticketCount--;
			}
			info.state = (UpYunTaskInfo.E_State)loader.State;
		}
		else
		{
			m_loaderQueue.Add (loader);
			info.state = UpYunTaskInfo.E_State.Waiting;
		}
		return loader.ID;
	}
	
	public UpYunDownloadTaskInfo GetDownloadTaskInfoFromID(int id)
	{
		bool isWaiting = false;
		UpYunHTTPFileDownloader loader = m_loaderList.Find (x => x.ID == id) as UpYunHTTPFileDownloader;
		if (loader == null) {
			loader = m_loaderQueue.Find (x => x.ID == id) as UpYunHTTPFileDownloader;
			if (loader == null) {
				return null;
			}
			isWaiting = true;
		}
		UpYunDownloadTaskInfo info = m_taskInfoList.Find (x => x.id == id) as UpYunDownloadTaskInfo;
		if (info == null) {
			info = new UpYunDownloadTaskInfo ();
			info.id = loader.ID;
			info.path = loader.Path;
		}
		info.progress = loader.Progress;
		info.state = isWaiting ? UpYunTaskInfo.E_State.Waiting : (UpYunTaskInfo.E_State)loader.State;
		return info;
	}
	
	public UpYunUploadTaskInfo GetUploadTaskInfoFromID(int id)
	{
		bool isWaiting = false;
		UpYunHTTPFileUploader loader = m_loaderList.Find (x => x.ID == id) as UpYunHTTPFileUploader;
		if (loader == null) {
			loader = m_loaderQueue.Find (x => x.ID == id) as UpYunHTTPFileUploader;
			if (loader == null) {
				return null;
			}
			isWaiting = true;
		}
		UpYunUploadTaskInfo info = m_taskInfoList.Find (x => x.id == id) as UpYunUploadTaskInfo;
		if (info == null) {
			info = new UpYunUploadTaskInfo ();
			info.id = loader.ID;
		}
		info.progress = loader.Progress;
		info.state = isWaiting ? UpYunTaskInfo.E_State.Waiting : (UpYunTaskInfo.E_State)loader.State;
		return info;
	}

	public UpYunBlocksUploadTaskInfo GetBlocksUploadTaskInfoFromID(int id)
	{
		bool isWaiting = false;
		UpYunHTTPFileBlocksUploader loader = m_loaderList.Find (x => x.ID == id) as UpYunHTTPFileBlocksUploader;
		if (loader == null) {
			loader = m_loaderQueue.Find (x => x.ID == id) as UpYunHTTPFileBlocksUploader;
			if (loader == null) {
				return null;
			}
			isWaiting = true;
		}
		UpYunBlocksUploadTaskInfo info = m_taskInfoList.Find (x => x.id == id) as UpYunBlocksUploadTaskInfo;
		if (info == null) {
			info = new UpYunBlocksUploadTaskInfo ();
			info.id = loader.ID;
		}
		info.progress = loader.Progress;
		info.state = isWaiting ? UpYunTaskInfo.E_State.Waiting : (UpYunTaskInfo.E_State)loader.State;
		return info;
	}
	
	public void PauseTaskFromID(int id)
	{
		UpYunHTTPFileLoader loader = m_loaderList.Find (x => x.ID == id);
		if (loader != null) {
			loader.Pause ();
		}
	}
	
	public void ContinueTaskFromID(int id)
	{
		UpYunHTTPFileLoader loader = m_loaderList.Find (x => x.ID == id);
		if (loader != null) {
			loader.Continue ();
		}
	}

	public void RemoveDownloadTaskFromID(int id)
	{
		UpYunHTTPFileLoader loader = m_loaderList.Find (x => x.ID == id);
		if (loader != null && loader is UpYunHTTPFileDownloader) {
			loader.Stop ();
			m_loaderList.Remove (loader);
			loader = null;
			m_ticketCount++;
			return;
		}
		loader = m_loaderQueue.Find (x => x.ID == id);
		if (loader != null && loader is UpYunHTTPFileDownloader) {
			m_loaderQueue.Remove (loader);
			loader = null;
		}
	}

	public void RemoveUploadTaskFromID(int id)
	{
		UpYunHTTPFileLoader loader = m_loaderList.Find (x => x.ID == id);
		if (loader != null && loader is UpYunHTTPFileUploader) {
			loader.Stop ();
			m_loaderList.Remove (loader);
			loader = null;
			m_ticketCount++;
			return;
		}
		loader = m_loaderQueue.Find (x => x.ID == id);
		if (loader != null && loader is UpYunHTTPFileUploader) {
			m_loaderQueue.Remove (loader);
			loader = null;
		}
	}

	public void RemoveBlocksUploadTaskFromID(int id)
	{
		UpYunHTTPFileLoader loader = m_loaderList.Find (x => x.ID == id);
		if (loader != null && loader is UpYunHTTPFileBlocksUploader) {
			loader.Stop ();
			m_loaderList.Remove (loader);
			loader = null;
			m_ticketCount++;
			return;
		}
		loader = m_loaderQueue.Find (x => x.ID == id);
		if (loader != null && loader is UpYunHTTPFileBlocksUploader) {
			m_loaderQueue.Remove (loader);
			loader = null;
		}
	}
	
	void Update()
	{
		if (m_ticketCount > 0 && m_loaderQueue.Count > 0) {
			UpYunHTTPFileLoader loader = m_loaderQueue.Peek ();
			UpYunHTTPFileLoader.E_State state = loader.State;
			if (state == UpYunHTTPFileLoader.E_State.None) {
				loader.Start ();
				m_ticketCount--;
				m_loaderQueue.Dequeue ();
			}
		}

		if (m_loaderList != null && m_loaderList.Count > 0) {
			for (int i = m_loaderList.Count - 1; i >= 0; --i) {
				UpYunHTTPFileLoader loader = m_loaderList [i];
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
					if (loader.State == UpYunHTTPFileLoader.E_State.End || loader.State == UpYunHTTPFileLoader.E_State.Err)
					{
						m_loaderList.Remove(loader);
						loader = null;
					}
				}
			}
		}
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