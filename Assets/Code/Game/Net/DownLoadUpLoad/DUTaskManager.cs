using UnityEngine;
using System.Collections;
using RO;
using System;

[SLua.CustomLuaClassAttribute]
public class DUTaskManager : SingleTonGO<DUTaskManager>
{
	static int _DOWNLOADER_BUFFER_SIZE = 1024 * 4;
	static int _UPLOADER_BUFFER_SIZE = 1024 * 80;
	DUDownLoadTaskCenter _downloaderTasks = new DUDownLoadTaskCenter ();
	DUUpLoadTaskCenter _uploaderTasks = new DUUpLoadTaskCenter ();

	public static DUTaskManager Instance {
		get{ return Me; }
	}

	public GameObject monoGameObject {
		get{ return this.gameObject; }
	}

	#region 设置上传、下载buffer大小

	public static int DownLoadBufferSize {
		set { 
			_DOWNLOADER_BUFFER_SIZE = Mathf.Clamp (value, 1024 * 2, 1024 * 1000);
		}
		get {
			return _DOWNLOADER_BUFFER_SIZE;
		}
	}

	public static int UpLoadBufferSize {
		set { 
			_UPLOADER_BUFFER_SIZE = Mathf.Clamp (value, 1024 * 80, 1024 * 1000);
		}
		get {
			return _UPLOADER_BUFFER_SIZE;
		}
	}

	#endregion

	#region 设置并发上限

	public int DownLoadConcurrentMax {
		set { 
			_downloaderTasks.maxConcurrentNum = value;
		}
		get {
			return _downloaderTasks.maxConcurrentNum;
		}
	}

	public int UpLoadConcurrentMax {
		set { 
			_uploaderTasks.maxConcurrentNum = value;
		}
		get {
			return _uploaderTasks.maxConcurrentNum;
		}
	}

	#endregion

	#region 通过ID获取Task

	public DUTask FindTaskByID (int task_id)
	{
		DUTask res = FindDownLoadTaskByID (task_id);
		if (res == null) {
			res = FindUpLoadTaskByID (task_id);
		}
		return res;
	}

	public DUDownLoadTask FindDownLoadTaskByID (int task_id)
	{
		return _downloaderTasks.FindTaskByID (task_id);
	}

	public DUUpLoadTask FindUpLoadTaskByID (int task_id)
	{
		return _uploaderTasks.FindTaskByID (task_id);
	}

	#endregion

	#region 通过本地文件路径，获取上传/下载Task

	public DUDownLoadTask FindDownLoadTaskByLocalpath (string localpath)
	{
		return _downloaderTasks.FindTaskByLocalpath (localpath);
	}

	public DUUpLoadTask FindUpLoadTaskByLocalpath (string localpath)
	{
		return _uploaderTasks.FindTaskByLocalpath (localpath);
	}

	#endregion

	public DUDownLoadTask DownLoad (string url, string path, bool continueDownLoad)
	{
		DUTask download = FindDownLoadTaskByLocalpath (path);
		if (download != null) {
			//已有上传task在使用
			//TODO 后续版本中如有强烈的此类需求，可以先创建一个状态为BUSY的下载任务
			return null;
		}
		return _downloaderTasks.CreateTask (url, path, continueDownLoad);
	}

	public DUDownLoadTask DownLoadWithCall (string url, string path, bool continueDownLoad,
	                                        Action<int> completeCall, Action<DUError> errorCall, Action<DUTask,float,float> progressCall)
	{
		DUDownLoadTask task = DownLoad (url, path, continueDownLoad);
		if (task != null) {
			task.SetCallBacks (completeCall, errorCall, progressCall);
		}
		return task;
	}

	public DUDownLoadTask ForceDownLoadWithCall (string url, string path,
	                                             Action<int> completeCall, Action<DUError> errorCall, Action<DUTask,float,float> progressCall)
	{
		DUDownLoadTask dl = FindDownLoadTaskByLocalpath (path);
		if (dl != null) {
			_downloaderTasks.CancelTask (dl);
		}
		return DownLoadWithCall (url, path, false, completeCall, errorCall, progressCall);
	}

	public DUUpLoadTask UpLoad (string url, string path, bool continueDownLoad)
	{
		DUTask upload = FindDownLoadTaskByLocalpath (path);
		if (upload != null) {
			//已有下载task在使用
			//TODO 后续版本中如有强烈的此类需求，可以先创建一个状态为BUSY的下载任务
			return null;
		}
		return _uploaderTasks.CreateTask (url, path, continueDownLoad);
	}

	public DUUpLoadTask UpLoadWithCall (string url, string path, bool continueDownLoad,
	                                    Action<int> completeCall, Action<DUError> errorCall, Action<DUTask,float,float> progressCall)
	{
		DUUpLoadTask task = UpLoad (url, path, continueDownLoad);
		if (task != null) {
			task.SetCallBacks (completeCall, errorCall, progressCall);
		}
		return task;
	}

	public DUUpLoadTask ForceUpLoadWithCall (string url, string path,
	                                         Action<int> completeCall, Action<DUError> errorCall, Action<DUTask,float,float> progressCall)
	{
		DUUpLoadTask ul = FindUpLoadTaskByLocalpath (path);
		if (ul != null) {
			_uploaderTasks.CancelTask (ul);
		}
		return UpLoadWithCall (url, path, false, completeCall, errorCall, progressCall);
	}

	//通过ID暂停Task
	public bool PauseTaskByID (int task_id)
	{
		if (_downloaderTasks.PauseTaskByID (task_id) || _uploaderTasks.PauseTaskByID (task_id)) {
			return true;
		}
		return false;
	}

	//通过ID重新开始Task（用于恢复暂停的TASK)
	public void StartTaskByID (int task_id)
	{
		_downloaderTasks.StartTaskByID (task_id);
		_uploaderTasks.StartTaskByID (task_id);
	}

	//通过ID取消TASK
	public bool CancelTaskByID (int task_id)
	{
		if (_downloaderTasks.CancelTaskByID (task_id) || _uploaderTasks.CancelTaskByID (task_id)) {
			return true;
		}
		return false;
	}

	//重置（关闭所有TASK）
	public void Reset ()
	{
		_downloaderTasks.CloseAll ();
		_uploaderTasks.CloseAll ();
	}

	protected void LateUpdate ()
	{
		_downloaderTasks.Update ();
		_uploaderTasks.Update ();
	}

	protected override void OnDestroy ()
	{
		base.OnDestroy ();
		Reset ();
	}
}
