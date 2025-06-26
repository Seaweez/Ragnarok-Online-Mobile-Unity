using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using CloudFile;
using System;

[SLua.CustomLuaClassAttribute]
public class DUUpLoadTask : DUTask
{
	protected byte[] m_bytes;
	RequestObjectForUpload m_requestObject;
	long contentSize = 0;

	public DUUpLoadTask (int ID, string url, string localPath) : base (ID, url, localPath)
	{
	}

	string FileExtension {
		get { 
			if (string.IsNullOrEmpty (_localPath) == false) {
				return Path.GetExtension (_localPath);
			}
			return null;
		}
	}

	protected override void _DoPause ()
	{
		base._DoPause ();
		_DoClose ();
	}

	protected override void _DoClose ()
	{
		base._DoClose ();
		if (readStream != null) {
			readStream.Close ();
			readStream = null;
		}
		if (m_requestObject != null) {
			m_requestObject.Release ();
		}
		m_requestObject = null;
		m_lengthOfHaveWrited = 0;
		_recordReadCount = 0;
	}

	protected override void _DoBegin ()
	{
		base._DoBegin ();
		#region(design defect)
		long contentLength = -1;
		if (File.Exists (_localPath)) {
			FileInfo fi = new FileInfo (_localPath);
			contentLength = fi.Length;
		} else {
			_CreateError ().RecordIOError (string.Format ("local has no file {0}", _localPath));
			return;
		}
		#endregion
		if (File.Exists (_localPath)) {
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create (_url);
			request.AllowWriteStreamBuffering = false;
			request.Method = "PUT";
			request.Headers.Add (CloudServer.AUTH_KEY, CloudServer.AUTH_VALUE);
			string md5Value = MyMD5.HashFile (_localPath);
			request.Headers.Add ("Content-MD5", md5Value);
			if (!string.IsNullOrEmpty (FileExtension)) {
				request.ContentType = FileExtension;
			}
			request.ContentLength = contentLength;
			contentSize = request.ContentLength;
			request.KeepAlive = true;
			m_requestObject = new RequestObjectForUpload (m_idForRequestObject);
			m_requestObject.Request = request;
			m_requestObject.Buffer = new byte[DUTaskManager.UpLoadBufferSize];
			try {
//				request.BeginGetRequestStream (new AsyncCallback (UploadBitsRequestCallback), m_requestObject);
				request.BeginGetRequestStream (new AsyncCallback (OnGetRequestStream), m_requestObject);
				SetTimeOut (request.Timeout / 1000.0f, DUError.REQUEST_TIMEOUT);
			} catch (Exception e) {
				HandleError (e);
			}
		}
	}

	// The last BeginWrite cost longer time.
	private long m_lengthOfHaveWrited;

	private void OnResponse (IAsyncResult result)
	{
		if (_state == DUTaskState.Running) {
			RequestObjectForUpload requestObject = (RequestObjectForUpload)result.AsyncState;
			if (requestObject != null && requestObject.ID == m_idForRequestObject) {
				StopCheckTimeOut ();
				try {
					if (requestObject != null) {
						HttpWebRequest request = requestObject.Request;
						HttpWebResponse response = request.EndGetResponse (result) as HttpWebResponse;
						m_requestObject.Response = response;
						Stream responseStream = response.GetResponseStream ();
						m_requestObject.ResponseStream = responseStream;
						if (response.StatusCode == HttpStatusCode.OK) {
							_DoClose ();
							_SuccessComplete ();
						} else {
							_CreateError ().RecordResponseError (response.StatusCode, "ResponseStatusIsntOK");
							_DoClose ();
						}
					}
				} catch (Exception e) {
					HandleError (e);
				}
			}
		}
	}

	private long _recordReadCount = 0;

	public override void FireProgress (float passedTime)
	{
		float delta = m_lengthOfHaveWrited - _recordReadCount;
		_recordReadCount = m_lengthOfHaveWrited;
		_speed = delta / passedTime;
		base.FireProgress (passedTime);
	}

	Stream readStream;

	private void UploadBitsRequestCallback (IAsyncResult result)
	{
		if (_state == DUTaskState.Running) {
			RequestObjectForUpload requestObject = (RequestObjectForUpload)result.AsyncState;
			if (requestObject != null && requestObject.ID == m_idForRequestObject) {
				StopCheckTimeOut ();

				HttpWebRequest request = requestObject.Request;
				Stream requestStream = null;
				try {
					requestStream = request.EndGetRequestStream (result);
					if (requestStream != null) {
						m_requestObject.RequestStream = requestStream;
						readStream = File.OpenRead (_localPath);
						if (readStream != null) {
							byte[] buffer = m_requestObject.Buffer;
							// How many bytes was read from the UploadStream
							int count = 0;
							while (readStream != null && (count = readStream.Read (buffer, 0, buffer.Length)) > 0) {
								// write out the buffer to the wire
								if (requestStream != null && requestStream.CanWrite) {
									requestStream.Write (buffer, 0, count);
								} else {
									return;
								}
								// Make sure that the system sends the buffer
								// update how many bytes are uploaded
								m_lengthOfHaveWrited += count;
								_progress = (float)m_lengthOfHaveWrited / contentSize;
							}
							if (readStream != null) {
								readStream.Close ();
								readStream = null;
							}
						}
						if (_state != DUTaskState.Running) {
							return;
						}
						request.BeginGetResponse (new AsyncCallback (OnResponse), m_requestObject);
						SetTimeOut (request.Timeout / 1000.0f, DUError.REQUEST_TIMEOUT);
					}
				} catch (Exception e) {
					HandleError (e);
				}
			}
		}
//		else if (_state == DUTaskState.Error) {
//			_DoClose ();
//			_CreateError ().RecordSystemError ("state not running");
//		}
	}

	private void HandleError (Exception e)
	{
		if (_state == DUTaskState.Running && m_requestObject != null && m_requestObject.ID == m_idForRequestObject) {
			if (e is WebException) {
				HttpWebResponse hwr = ((WebException)e).Response as HttpWebResponse;
				if (hwr != null) {
					_CreateError ().RecordResponseError (hwr.StatusCode, hwr.StatusDescription);
				} else {
					_CreateError ().RecordWebError (((WebException)e).Status, e.ToString ());
				}
			} else if (e is IOException) {
				_CreateError ().RecordIOError (e.ToString ());
			} else {
				_CreateError ().RecordSystemError (e.ToString ());
			}
			_DoClose ();
			Debug.Log (e);
			throw e;
		}
	}

	private void OnGetRequestStream (IAsyncResult result)
	{
		if (_state == DUTaskState.Running) {
			RequestObjectForUpload requestObject = (RequestObjectForUpload)result.AsyncState;
			if (requestObject != null && requestObject.ID == m_idForRequestObject) {
				StopCheckTimeOut ();

				HttpWebRequest request = requestObject.Request;
				Stream requestStream = null;
				try {
					requestStream = request.EndGetRequestStream (result);
					if (requestStream != null) {
						m_requestObject.RequestStream = requestStream;
						readStream = File.OpenRead (_localPath);
						WriteToRequestStream ();
					}
				} catch (Exception e) {
					HandleError (e);
				}

			}
		}
	}

	private int validSize = 0;

	private void WriteToRequestStream ()
	{
		Stream requestStream = m_requestObject.RequestStream;

		byte[] bytesToWrite = null;
		byte[] InnerBuffer = m_requestObject.Buffer;
		int bytesToWriteLength = 0;
		int bufferOffset = 0;
		if (readStream != null) {
			int bytesRead = 0;
			if (InnerBuffer != null) {
				bytesRead = readStream.Read (InnerBuffer, 0, (int)InnerBuffer.Length);
				if (bytesRead <= 0) {
					if (readStream != null) {
						readStream.Close ();
						readStream = null;
					}
					InnerBuffer = null;
				}
			}
			if (InnerBuffer != null) {
				bytesToWriteLength = bytesRead;
				bytesToWrite = InnerBuffer;
			}
		}
		validSize = bytesToWriteLength;

		try {
			requestStream.BeginWrite (bytesToWrite, bufferOffset, bytesToWriteLength, new AsyncCallback (OnWriteToRequestStream), m_requestObject);
		} catch (Exception e) {
			HandleError (e);
		}
	}

	// The last BeginWrite cost longer time.
	private void OnWriteToRequestStream (IAsyncResult result)
	{
		if (_state == DUTaskState.Running) {
			RequestObjectForUpload requestObject = (RequestObjectForUpload)result.AsyncState;
			if (requestObject != null && requestObject.ID == m_idForRequestObject) {
				StopCheckTimeOut ();

				Stream requestStream = requestObject.RequestStream;
				try {
					requestStream.EndWrite (result);
				} catch (Exception e) {
					HandleError (e);
				}
				m_lengthOfHaveWrited += validSize;
				_progress = (float)m_lengthOfHaveWrited / contentSize;
				if (_progress >= 1) {
					m_requestObject.Request.BeginGetResponse (new AsyncCallback (OnResponse), m_requestObject);
					SetTimeOut (m_requestObject.Request.Timeout / 1000.0f, DUError.REQUEST_TIMEOUT);
				} else {
					WriteToRequestStream ();
				}
			}
		}
	}

	public override DUTask CloneSelf ()
	{
		DUUpLoadTask cloned = new DUUpLoadTask (_id, _url, _localPath);
		cloned._state = _state;
		cloned.SetCallBacks (_completeHandler, _errorHandler, _progressHandler);
		cloned._clonedTimes = _clonedTimes + 1;
		return cloned;
	}
}