using UnityEngine;
using System.Collections;
using System;
using System.IO;

[SLua.CustomLuaClassAttribute]
public class DUTask
{
	const float InValid_CheckTimeOut = -1;
	protected int _id;
	protected int _clonedTimes = 0;
	protected float _progress = 0;
	protected bool _progressDirty;
	protected float _speed = 0;
	protected string _url;
	protected string _localPath;
	protected DUError _error;
	protected DUTaskState _state;
	protected Action<int> _completeHandler;
	protected Action<DUError> _errorHandler;
	protected Action<DUTask,float,float> _progressHandler;
	protected float _timeOut = InValid_CheckTimeOut;
	protected float _currentTimeOut = InValid_CheckTimeOut;
	protected string _timeOutMsg;
	protected int m_idForRequestObject;
	protected readonly object obj = new object ();

	public int id {
		get {
			return _id;
		}
	}

	public string url {
		get {
			return _url;
		}
	}

	public string localPath {
		get {
			return _localPath;
		}
	}

	public float progress {
		get {
			return _progress;
		}
	}

	public float speed {
		get {
			return _speed;
		}
	}

	public DUTaskState state {
		get {
			return _state;
		}
	}

	public int clonedTimes {
		get {
			return _clonedTimes;
		}
		set {
			_clonedTimes = value;
		}
	}

	protected DUError _CreateError ()
	{
		if (_error == null) {
			_error = new DUError (_id);
			_state = DUTaskState.Error;
		}
		return _error;
	}

	public DUTask (int ID, string url, string localPath)
	{
		_id = ID;
		_localPath = localPath;
		_url = url;
		_state = DUTaskState.Waiting;
	}

	public void SetCallBacks (Action<int> completeCall, Action<DUError> errorCall, Action<DUTask,float,float> progressCall)
	{
		_completeHandler = completeCall;
		_errorHandler = errorCall;
		_progressHandler = progressCall;
	}

	virtual protected void _CheckTimeOut (float passedTime)
	{
		if (_timeOut != InValid_CheckTimeOut) {
			_currentTimeOut += passedTime;
			if (_currentTimeOut >= _timeOut) {
				_CreateError ().RecordTimeOutError (_timeOutMsg);
				StopCheckTimeOut ();
			}
		}
	}

	virtual public void Running (float passedTime)
	{
		FireProgress (passedTime);
		_CheckTimeOut (passedTime);
	}

	virtual public void Begin ()
	{
		if (_state == DUTaskState.Waiting) {
			_DoBegin ();
		}
	}

	virtual public void Pause ()
	{
		if (_state == DUTaskState.Running) {
			_DoPause ();
		}
	}

	virtual public void Cancel ()
	{
		if (_state == DUTaskState.Running) {
			_DoCancel ();
		}
	}

	virtual public void UnPause ()
	{
		if (_state == DUTaskState.Pause) {
			_DoUnPause ();
		}
	}

	virtual protected void _CreateDirectory ()
	{
		string dir = Path.GetDirectoryName (_localPath);
		if (FileHelper.ExistDirectory (dir) == false) {
			FileHelper.CreateDirectory (dir);
		}
	}

	virtual protected void _SuccessComplete ()
	{
		_state = DUTaskState.Complete;
	}

	virtual protected void _DoBegin ()
	{
		m_idForRequestObject++;
		_state = DUTaskState.Running;
	}

	virtual protected void _DoPause ()
	{
		_state = DUTaskState.Pause;
	}

	virtual protected void _DoUnPause ()
	{
		_state = DUTaskState.Waiting;
	}

	virtual protected void _DoCancel ()
	{
		_state = DUTaskState.Cancel;
	}

	virtual protected void _DoClose ()
	{
		StopCheckTimeOut ();
		_progressDirty = false;
	}

	virtual protected void StopCheckTimeOut ()
	{
		_timeOut = InValid_CheckTimeOut;
		_currentTimeOut = InValid_CheckTimeOut;
		_timeOutMsg = null;
	}

	virtual protected void SetTimeOut (float deltaTime, string msg)
	{
		_timeOut = deltaTime;
		_currentTimeOut = 0;
		_timeOutMsg = msg;
	}

	virtual public void FireSuccess ()
	{
		Action<int> tmp = _completeHandler;
		if (tmp != null) {
			_completeHandler = null;
			tmp (this._id);
		}
	}

	virtual public void FireProgress (float passedTime)
	{
		if (_progressHandler != null) {
			_progressHandler (this, _progress, _speed);
		}
	}

	virtual public void FireError ()
	{
		Action<DUError> tmp = _errorHandler;
		if (tmp != null) {
			_errorHandler = null;
			tmp (this._error);
		}
	}

	virtual public void Close ()
	{
		_DoClose ();
		_completeHandler = null;
		_errorHandler = null;
		_progressHandler = null;
	}

	virtual public DUTask CloneSelf ()
	{
		return null;
	}
}
