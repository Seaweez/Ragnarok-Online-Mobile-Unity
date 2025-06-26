using UnityEngine;
using System.Collections;
using System.Net;
using System;
using System.IO;
using CloudFile;

[SLua.CustomLuaClassAttribute]
public class DUDownLoadTask : DUTask
{
	public DUDownLoadTask (int ID, string url, string localPath) : base (ID, url, localPath)
	{
	}

	RequestObjectForDownload m_requestObject;

	protected void _DeleteLocalPath ()
	{
		if (File.Exists (_localPath)) {
			File.Delete (_localPath);
		}
	}

	protected override void _DoBegin ()
	{
		base._DoBegin ();
		_CreateDirectory ();
		_DeleteLocalPath ();
		HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (_url);
		request.Method = "GET";
		request.Headers.Add (UpYunServer.AUTH_KEY, UpYunServer.AUTH_VALUE);
		//			request.Headers.Set("Date", DateTime.Now.ToString("r"));
		m_requestObject = new RequestObjectForDownload (m_idForRequestObject);
		m_requestObject.Request = request;
		m_requestObject.Buffer = new byte[DUTaskManager.DownLoadBufferSize];
		Debug.LogFormat ("{0} _DoBegin {1}", _id, m_idForRequestObject);
		try {
			request.BeginGetResponse (new AsyncCallback (OnResponse), m_requestObject);
			SetTimeOut (request.Timeout / 1000.0f, DUError.REQUEST_TIMEOUT);
			// start count time out
		} catch (Exception e) {
			HandleError (e);
		}
	}

	protected override void _DoPause ()
	{
		base._DoPause ();
//		_DoClose ();
	}

	bool _isClosed = false;

	protected override void _DoClose ()
	{
		base._DoClose ();
		if (m_requestObject != null) {
			_isClosed = true;
			Debug.LogFormat ("{0} doclose", _id);
			m_requestObject.Release ();
		}
		m_md5FromServer = null;
		m_requestObject = null;
		m_lengthOfHaveRead = 0;
		_recordReadCount = 0;
	}

	private string m_md5FromServer;

	protected void OnResponse (IAsyncResult result)
	{
		if (_isClosed) {
			Debug.LogFormat ("{0} closed but OnResponse,state:{1}", _id, _state);
		}
		if (_state == DUTaskState.Running) {
			RequestObjectForDownload requestObject = (RequestObjectForDownload)result.AsyncState;
			if (requestObject != null && requestObject.ID == m_idForRequestObject) {
				// end count time out
				StopCheckTimeOut ();
				try {
					if (m_requestObject != null) {
						HttpWebRequest request = requestObject.Request;
						if (request.HaveResponse == false) {
							Debug.LogFormat ("{0} Have no Response", _id);
							_CreateError ().RecordSystemError ("Have no Response");
							_DoClose ();
							return;
						}
						HttpWebResponse response = request.EndGetResponse (result) as HttpWebResponse;
						requestObject.Response = response;
						if (response.StatusCode == HttpStatusCode.OK) {
							m_md5FromServer = response.Headers ["ETag"];
							if (!string.IsNullOrEmpty (m_md5FromServer)) {
								m_md5FromServer = m_md5FromServer.Replace ("\"", "");
							}
							Stream stream = response.GetResponseStream ();
							requestObject.ResponseStream = stream;
							ReadFromResponseStream ();
						} else {
							_CreateError ().RecordResponseError (response.StatusCode,"ResponseStatusIsntOK");
							_DoClose ();
						}
					}
				} catch (Exception e) {
					HandleError (e);
				}
			}
		} else {
			_DoClose ();
		}
			
	}

	protected void ReadFromResponseStream ()
	{
		if (_state == DUTaskState.Running && m_requestObject != null && m_requestObject.ID == m_idForRequestObject) {
			byte[] buffer = m_requestObject.Buffer;
			Stream stream = m_requestObject.ResponseStream;
			try {
				stream.BeginRead (buffer, 0, buffer.Length, new AsyncCallback (OnReadFromStream), m_requestObject);
				SetTimeOut (stream.ReadTimeout / 1000.0f, DUError.DOWNLOADSTREAM_TIMEOUT);
				// start count time out
			} catch (Exception e) {
				HandleError (e);
			}
		}
	}

	void HandleError (Exception e)
	{
		if (_state == DUTaskState.Running && m_requestObject != null && m_requestObject.ID == m_idForRequestObject) {
			if (e is WebException) {
				HttpWebResponse hwr = ((WebException)e).Response as HttpWebResponse;
				if (hwr != null) {
					_CreateError ().RecordResponseError (hwr.StatusCode,hwr.StatusDescription);
				} else {
					_CreateError ().RecordWebError (((WebException)e).Status, e.ToString ());
				}
			} else if (e is IOException) {
				_CreateError ().RecordIOError (e.ToString ());
			} else {
				_CreateError ().RecordSystemError (e.ToString ());
			}
			_DoClose ();
			Debug.LogFormat ("{0} HandleError", _id);
			Debug.Log (e);
			throw e;
		}
	}

	private int m_lengthOfHaveRead;

	private void OnReadFromStream (IAsyncResult result)
	{
		if (_state == DUTaskState.Running) {
			RequestObjectForDownload requestObject = (RequestObjectForDownload)result.AsyncState;
			StopCheckTimeOut ();
			if (requestObject.ID == m_idForRequestObject) {
				try {
					int readBytesLength = requestObject.ResponseStream.EndRead (result);
					if (readBytesLength > 0) {
						FileHelper.AppendBytes (_localPath, m_requestObject.Buffer, readBytesLength);
						m_lengthOfHaveRead += readBytesLength;
						_progress = (float)m_lengthOfHaveRead / m_requestObject.ContentLength;
						ReadFromResponseStream ();
					} else {
						bool successed = false;
						if (string.IsNullOrEmpty (m_md5FromServer)) {
							successed = true;
						} else {
							string fileMD5 = MyMD5.HashFile (_localPath);
							if (string.Equals (fileMD5, m_md5FromServer)) {
								successed = true;
							} else {
								_CreateError ().RecordMD5Error ();
								Debug.LogFormat ("file:{0}, server:{1}.", fileMD5, m_md5FromServer);
							}
						}
						_DoClose ();
						if (successed) {
							_SuccessComplete ();
						}
					}
				} catch (Exception e) {
					HandleError (e);
				}
			}
		}
	}

	private int _recordReadCount = 0;

	public override void FireProgress (float passedTime)
	{
		float delta = m_lengthOfHaveRead - _recordReadCount;
		_recordReadCount = m_lengthOfHaveRead;
		_speed = delta / passedTime;
		base.FireProgress (passedTime);
	}

	public override DUTask CloneSelf ()
	{
		DUDownLoadTask cloned = new DUDownLoadTask (_id, _url, _localPath);
		cloned._state = _state;
		cloned.SetCallBacks (_completeHandler, _errorHandler, _progressHandler);
		cloned._clonedTimes = _clonedTimes + 1;
		return cloned;
	}
}
