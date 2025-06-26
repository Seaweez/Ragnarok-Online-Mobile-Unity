using UnityEngine;
using System.Collections;
using RO;
using System;

[SLua.CustomLuaClassAttribute]
public class DUWTaskManager : SingleTonGO<DUWTaskManager>
{
	DUWDownLoadTaskCenter _downloaderTasks = new DUWDownLoadTaskCenter ();
	DUWUpLoadTaskCenter _uploaderTasks = new DUWUpLoadTaskCenter ();

	public static DUWTaskManager Instance {
		get{ return Me; }
	}

	public GameObject monoGameObject {
		get{ return this.gameObject; }
	}

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

	public DUWDownLoadTask FindDownLoadTaskByID (int task_id)
	{
		return _downloaderTasks.FindTaskByID (task_id);
	}

	public DUWUpLoadTask FindUpLoadTaskByID (int task_id)
	{
		return _uploaderTasks.FindTaskByID (task_id);
	}

	#endregion

	#region 通过本地文件路径，获取上传/下载Task

	public DUWDownLoadTask FindDownLoadTaskByLocalpath (string localpath)
	{
		return _downloaderTasks.FindTaskByLocalpath (localpath);
	}

	public DUWUpLoadTask FindUpLoadTaskByLocalpath (string localpath)
	{
		return _uploaderTasks.FindTaskByLocalpath (localpath);
	}

	#endregion

	public DUWDownLoadTask DownLoad (string url, string path, bool continueDownLoad)
	{
		DUTask downLoad = FindDownLoadTaskByLocalpath (path);
		if (downLoad != null) {
			//已有上传task在使用
			//TODO 后续版本中如有强烈的此类需求，可以先创建一个状态为BUSY的下载任务
			return null;
		}
		return _downloaderTasks.CreateTask (url, path, continueDownLoad);
	}

	public DUWDownLoadTask DownLoadWithCall (string url, string path, bool continueDownLoad,
	                                        Action<int> completeCall, Action<DUError> errorCall, Action<DUTask,float,float> progressCall)
	{
		DUWDownLoadTask task = DownLoad (url, path, continueDownLoad);
		if (task != null) {
			task.SetCallBacks (completeCall, errorCall, progressCall);
		}
		return task;
	}

	//强制取消同个path的下载，重新以本次任务做新下载
	public DUWDownLoadTask ForceDownLoadWithCall (string url, string path,
	                                             Action<int> completeCall, Action<DUError> errorCall, Action<DUTask,float,float> progressCall)
	{
		DUWDownLoadTask dl = FindDownLoadTaskByLocalpath (path);
		if (dl != null) {
			_downloaderTasks.CancelTask (dl);
		}
		return DownLoadWithCall (url, path, false, completeCall, errorCall, progressCall);
	}

	public DUWUpLoadTask UpLoad (string url, string path, bool continueDownLoad)
	{
		DUWUpLoadTask upLoad = FindUpLoadTaskByLocalpath (path);
		if (upLoad != null) {
			//已有下载task在使用
			//TODO 后续版本中如有强烈的此类需求，可以先创建一个状态为BUSY的下载任务
			return null;
		}
		return _uploaderTasks.CreateTask (url, path, continueDownLoad);
	}

	public DUWUpLoadTask UpLoadWithCall (string url, string path, bool continueDownLoad,
	                                    Action<int> completeCall, Action<DUError> errorCall, Action<DUTask,float,float> progressCall)
	{
		DUWUpLoadTask task = UpLoad (url, path, continueDownLoad);
		if (task != null) {
			task.SetCallBacks (completeCall, errorCall, progressCall);
		}
		return task;
	}

	//强制取消同个path的上传，重新以本次任务做新上传
	public DUWUpLoadTask ForceUpLoadWithCall (string url, string path,
	                                         Action<int> completeCall, Action<DUError> errorCall, Action<DUTask,float,float> progressCall)
	{
		DUWUpLoadTask ul = FindUpLoadTaskByLocalpath (path);
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
