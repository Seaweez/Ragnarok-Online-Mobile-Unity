using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DUDownLoadTaskCenter :DUTaskCenter<DUDownLoadTask>
{
	protected override DUDownLoadTask _CreateTask (int id, string url, string localPath)
	{
		return new DUDownLoadTask (id, url, localPath);
	}
}

public class DUUpLoadTaskCenter :DUTaskCenter<DUUpLoadTask>
{
	protected override DUUpLoadTask _CreateTask (int id, string url, string localPath)
	{
		return new DUUpLoadTask (id, url, localPath);
	}
}

public class DUWDownLoadTaskCenter :DUTaskCenter<DUWDownLoadTask>
{
	protected override DUWDownLoadTask _CreateTask (int id, string url, string localPath)
	{
		return new DUWDownLoadTask (id, url, localPath);
	}
}

public class DUWUpLoadTaskCenter :DUTaskCenter<DUWUpLoadTask>
{
	protected override DUWUpLoadTask _CreateTask (int id, string url, string localPath)
	{
		return new DUWUpLoadTask (id, url, localPath);
	}
}

public class DUTaskCenter<T> where T:DUTask
{
	static int _ID = 0;
	float _currentTime;
	//最大并发任务数量
	protected int _maxConcurrentNum = 5;

	public int maxConcurrentNum {
		get {
			return _maxConcurrentNum;
		}
		set {
			_maxConcurrentNum = value;
		}
	}

	protected List<T> _waitingList = new List<T> ();
	protected List<T> _processingList = new List<T> ();
	protected List<T> _completeTasks = new List<T> ();
	protected List<T> _errorTasks = new List<T> ();
	protected List<T> _tmpRunningTasks = new List<T> ();

	public bool ProcessListNotFull {
		get {
			return _processingList.Count < _maxConcurrentNum || _maxConcurrentNum == 0;
		}
	}

	#region 查找，通过ID或者本地文件名

	virtual protected T _FindInByID (List<T> list, int task_id)
	{
		for (int i = 0; i < list.Count; i++) {
			if (list [i].id == task_id) {
				return list [i];
			}
		}
		return null;
	}

	virtual protected T _FindInByLocalPath (List<T> list, string localpath)
	{
		for (int i = 0; i < list.Count; i++) {
			if (list [i].localPath == localpath) {
				return list [i];
			}
		}
		return null;
	}

	virtual public T FindTaskByID (int task_id)
	{
		T res = _FindInByID (_processingList, task_id);
		if (res == null) {
			res = _FindInByID (_waitingList, task_id);
		}
		return res;
	}

	virtual public T FindTaskByLocalpath (string localpath)
	{
		T res = _FindInByLocalPath (_processingList, localpath);
		if (res == null) {
			res = _FindInByLocalPath (_waitingList, localpath);
		}
		return res;
	}

	#endregion

	virtual public bool PauseTaskByID (int task_id)
	{
		T task = FindTaskByID (task_id);
		if (task != null) {
			if (task.state == DUTaskState.Running) {
				var t = (T)task.CloneSelf ();
				task.Pause ();
				t.Pause ();
				_processingList.Remove (task);
				_waitingList.Remove (task);
				_AddToWait (t);
			}
			return true;
		}
		return false;
	}

	virtual public bool CancelTaskByID (int task_id)
	{
		T task = FindTaskByID (task_id);
		if (task != null) {
			return CancelTask (task);
		}
		return false;
	}

	virtual public bool CancelTask (T task)
	{
		if (_processingList.Remove (task)) {
			task.Cancel ();
			task.Close ();
			return true;
		} else if (_waitingList.Remove (task)) {
			task.Cancel ();
			task.Close ();
			return true;
		} 
		return false;
	}

	virtual protected T _CreateTask (int id, string url, string localPath)
	{
		return null;
	}

	virtual public T CreateTask (string url, string localpath, bool continueLoad)
	{
		T res = FindTaskByLocalpath (localpath);
		if (res == null) {
			res = _CreateTask (++_ID, url, localpath);
			_AddToWait (res);
		} else if (res.state == DUTaskState.Pause) {
			_UnPause (res);
		}
		return res;
	}

	virtual public T StartTaskByID (int task_id)
	{
		T res = FindTaskByID (task_id);
		if (res != null) {
			_UnPause (res);
		}
		return res;
	}

	virtual protected void _UnPause (T task)
	{
		task.UnPause ();
	}

	virtual protected void _DoStartTask (T task)
	{
		if (_processingList.Contains (task) == false) {
			_processingList.Add (task);
			_RemoveWaiting (task);
			task.Begin ();
		}
	}

	protected void _AddToWait (T task)
	{
		if (_waitingList.Contains (task) == false)
			_waitingList.Add (task);
	}

	protected void _RemoveWaiting (T task)
	{
		_waitingList.Remove (task);
	}

	virtual public void CloseAll ()
	{
		_ID = 0;
		for (int i = _waitingList.Count - 1; i >= 0; i--) {
			CancelTask (_waitingList [i]);
		}

		for (int i = _processingList.Count - 1; i >= 0; i--) {
			CancelTask (_processingList [i]);
		}

		_waitingList.Clear ();
		_processingList.Clear ();
	}

	virtual public void Update ()
	{
		float passed = (Time.realtimeSinceStartup - _currentTime);
		_currentTime = Time.realtimeSinceStartup;
		if (_processingList.Count > 0) {
			T task;
			for (int i = _processingList.Count - 1; i >= 0; i--) {
				task = _processingList [i];
				switch (task.state) {
				case DUTaskState.Complete:
					_completeTasks.Add (task);
					_processingList.Remove (task);
					break;
				case DUTaskState.Error:
					_errorTasks.Add (task);
					_processingList.Remove (task);
					break;
				case DUTaskState.Running:
					_tmpRunningTasks.Add (task);
					break;
				}
			}
		}

		//handle complete
		HandleDirtyTaskListAndClear (_completeTasks, _SuccessCompleteTask);

		//handle error
		HandleDirtyTaskListAndClear (_errorTasks, _ErrorCompleteTask);

		//
		HandleDirtyTaskListAndClear<float> (_tmpRunningTasks, _RunningTask, passed);

		//检查是否需要将waiting添加至requesting
		if (_waitingList.Count > 0 && ProcessListNotFull) {
			T task;
			for (int i = 0; i < _waitingList.Count; i++) {
				task = _waitingList [i];
				if (task.state != DUTaskState.Pause) {
					_DoStartTask (task);
					i--;
				}
				if (ProcessListNotFull == false)
					break;
			}
		}
	}

	//成功时结束任务
	protected void _SuccessCompleteTask (T task)
	{
		_processingList.Remove (task);
		task.FireSuccess ();
		task.Close ();
	}

	//失败时结束任务
	protected void _ErrorCompleteTask (T task)
	{
		_processingList.Remove (task);
		task.FireError ();
		task.Close ();
	}

	//成功时结束任务
	protected void _RunningTask (T task, float passed)
	{
		task.Running (passed);
	}

	protected void HandleDirtyTaskListAndClear (List<T> list, System.Action<T> call)
	{
		if (list.Count > 0) {
			for (int i = 0; i < list.Count; i++) {
				call (list [i]);
			}
			list.Clear ();
		}
	}

	protected void HandleDirtyTaskListAndClear<V> (List<T> list, System.Action<T,V> call, V param)
	{
		if (list.Count > 0) {
			for (int i = 0; i < list.Count; i++) {
				call (list [i], param);
			}
			list.Clear ();
		}
	}
}
