using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System;

[SLua.CustomLuaClassAttribute]
public class DUWDownLoadTask : DUTask
{
	DUWebClient _webClient;

	protected void _DeleteLocalPath ()
	{
		if (File.Exists (_localPath)) {
			File.Delete (_localPath);
		}
	}

	public DUWDownLoadTask (int ID, string url, string localPath) : base (ID, url, localPath)
	{
	}

	protected override void _DoBegin ()
	{
		base._DoBegin ();

		_CreateDirectory ();
		_DeleteLocalPath ();

		_CreateClient (false);

		Uri uri = new Uri (url);
		_webClient.DownloadFileAsync (uri, localPath);
	}

	protected override void _DoPause ()
	{
		base._DoPause ();
		_DoClose ();
	}

	protected override void _DoClose ()
	{
		base._DoClose ();
		if (_webClient != null) {
			_RemoveCallBackToClient ();
			if (_webClient.request != null) {
				_webClient.request.KeepAlive = false;
			}
			_webClient.CancelAsync ();
			_webClient.Dispose ();
			_webClient = null;
		}
		_received = 0;
		_recordReceived = 0;
	}

	protected override void _DoCancel ()
	{
		base._DoCancel ();
		_DeleteLocalPath ();
	}

	void _CreateClient (bool useProxy)
	{
		_webClient = new DUWebClient (null, useProxy);
		_AddCallBackToClient ();
	}

	void _AddCallBackToClient ()
	{
		if (_webClient != null) {
			_webClient.DownloadFileCompleted -= DownLoadFileComplete;
			_webClient.DownloadFileCompleted += DownLoadFileComplete;
			_webClient.DownloadProgressChanged -= DownloadProgressChanged;
			_webClient.DownloadProgressChanged += DownloadProgressChanged;
		}
	}

	void _RemoveCallBackToClient ()
	{
		if (_webClient != null) {
			_webClient.DownloadFileCompleted -= DownLoadFileComplete;
			_webClient.DownloadProgressChanged -= DownloadProgressChanged;
		}
	}

	void DownLoadFileComplete (object sender, System.ComponentModel.AsyncCompletedEventArgs e)
	{
		if (e.Cancelled) {
			_DoClose ();
			return;
		}
		if (e.Error != null) {
			HandleError (e.Error);
			return;
		}
		if (_webClient != null) {
			if (!string.IsNullOrEmpty (_webClient.Md5FromServer)) {
				string fileMD5 = MyMD5.HashFile (_localPath);
				if (!string.Equals (fileMD5, _webClient.Md5FromServer)) {
					_CreateError ().RecordMD5Error ();
					return;
				}
			}
		}
		_SuccessComplete ();
	}

	private long _received = 0;
	private long _recordReceived = 0;

	void DownloadProgressChanged (object sender, DownloadProgressChangedEventArgs e)
	{
		_progress = 0.01f * e.ProgressPercentage;
		var delta = e.BytesReceived - _received;
		_received = e.BytesReceived;
		_recordReceived += delta;
	}

	public override void FireProgress (float passedTime)
	{
		_speed = _recordReceived / passedTime;
		_recordReceived = 0;
		base.FireProgress (passedTime);
	}

	void HandleError (Exception e)
	{
		if (e is WebException) {
			WebException web = (WebException)e;
			HttpWebResponse hwr = web.Response as HttpWebResponse;
			if (hwr != null) {
				_CreateError ().RecordResponseError (hwr.StatusCode, hwr.StatusDescription);
			} else {
				_CreateError ().RecordWebError (web.Status, web.Message);
//				_CreateError ().RecordSystemError (e.ToString ());
			}
		} else if (e is IOException) {
			_CreateError ().RecordIOError (e.ToString ());
		} else {
			_CreateError ().RecordSystemError (e.ToString ());
		}
		FileHelper.DeleteFile (_localPath);
		_DoClose ();
//		Debug.LogFormat ("{0} HandleError", _id);
//		Debug.Log (e);
		throw e;
	}

	public override DUTask CloneSelf ()
	{
		DUWDownLoadTask cloned = new DUWDownLoadTask (_id, _url, _localPath);
		cloned._state = _state;
		cloned.SetCallBacks (_completeHandler, _errorHandler, _progressHandler);
		cloned._clonedTimes = _clonedTimes + 1;
		return cloned;
	}
}
