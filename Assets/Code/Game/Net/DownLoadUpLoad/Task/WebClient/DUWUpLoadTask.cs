using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System;

[SLua.CustomLuaClassAttribute]
public class DUWUpLoadTask : DUTask
{
	DUWebClient _webClient;
	FileStream _readStream;
	Stream _writeStream;
	byte[] _buffer;
	long _contentLength = -1;

	protected void _DeleteLocalPath ()
	{
		if (File.Exists (_localPath)) {
			File.Delete (_localPath);
		}
	}

	public DUWUpLoadTask (int ID, string url, string localPath) : base (ID, url, localPath)
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

	protected override void _DoBegin ()
	{
		base._DoBegin ();
		if (File.Exists (_localPath)) {
			FileInfo fi = new FileInfo (_localPath);
			_contentLength = fi.Length;
		} else {
			_CreateError ().RecordIOError (string.Format ("local has no file {0}", _localPath));
			return;
		}

		WebHeaderCollection header = new WebHeaderCollection ();
		header.Add (CloudServer.AUTH_KEY, CloudServer.AUTH_VALUE);
		string md5Value = MyMD5.HashFile (_localPath);
		header.Add ("Content-MD5", md5Value);
		_buffer = new byte[8192];
		_CreateClient (header, false, _contentLength);

		Uri uri = new Uri (url);
//		_webClient.UploadFileAsync (uri, localPath);
//		Debug.LogFormat ("_webClient.OpenWriteAsync {0} {1}", _id, _clonedTimes);
		_webClient.OpenWriteAsync (uri);
	}

	protected override void _DoPause ()
	{
		base._DoPause ();
		_DoClose ();
	}

	protected override void _DoClose ()
	{
		base._DoClose ();
		CloseReadStream ();
		if (_webClient != null) {
			_RemoveCallBackToClient ();
			if (_webClient.request != null) {
				_webClient.request.KeepAlive = false;
			}
			_webClient.CancelAsync ();
			_webClient.Dispose ();
			_webClient = null;
		}
		_buffer = null;
		_writeStream = null;
		_sends = 0;
		_recordSends = 0;
	}

	void _CreateClient (WebHeaderCollection header, bool useProxy, long contentLength)
	{
		_webClient = new DUWebClient (header, useProxy, contentLength);
		_AddCallBackToClient ();
	}

	void _AddCallBackToClient ()
	{
		if (_webClient != null) {
			_webClient.OpenWriteCompleted -= _OpenWriteGetStream;
			_webClient.OpenWriteCompleted += _OpenWriteGetStream;
		}
	}

	void _RemoveCallBackToClient ()
	{
		if (_webClient != null) {
			_webClient.OpenWriteCompleted -= _OpenWriteGetStream;
		}
	}

	private long _sends = 0;
	private long _recordSends = 0;

	void _OpenWriteGetStream (object sender, OpenWriteCompletedEventArgs e)
	{
		if (e.Cancelled) {
			_DoClose ();
			return;
		}
		if (e.Error != null) {
			HandleError (e.Error);
			return;
		}
		_writeStream = e.Result;
		if (_writeStream != null) {
//		SyncWriteStream ();
			ASyncWriteSream ();
		}
	}

	void ASyncWriteSream ()
	{
		try {
//			Debug.LogFormat ("File.OpenRead {0} {1}", _id, _clonedTimes);
			_readStream = File.OpenRead (_localPath);
			if (_readStream != null) {
				WriteBytes (_writeStream);
			}
		} catch (Exception e) {
			HandleError (e);
		}
	}

	void SyncWriteStream ()
	{
		try {
			_readStream = File.OpenRead (_localPath);
			if (_readStream != null) {
				// How many bytes was read from the UploadStream
				int count = 0;
				while (_readStream != null && (count = _readStream.Read (_buffer, 0, _buffer.Length)) > 0) {
					//							Debug.Log(_state);
					// write out the buffer to the wire
					if (_writeStream != null && _writeStream.CanWrite) {
						_writeStream.Write (_buffer, 0, count);
					} else {
						return;
					}
					// Make sure that the system sends the buffer
					// update how many bytes are uploaded
					_UploadProgressChanged (count);
				}
			}
			if (_state != DUTaskState.Running) {
				return;
			}
			SendResponseForSure ();
		} catch (Exception ee) {
			HandleError (ee);
		}
	}

	void SendResponseForSure ()
	{
		if (_webClient != null && _webClient.request != null) {
			_webClient.request.BeginGetResponse (new AsyncCallback (OnResponse), _webClient.request);
		}
	}

	bool WriteBytes (Stream WriteStream)
	{
		byte[] bytesToWrite = null;
		int bytesToWriteLength = 0;
		int bufferOffset = 0;

		int bytesRead = 0;
		if (_buffer != null) {
			bytesRead = _readStream.Read (_buffer, 0, (int)_buffer.Length);
			if (bytesRead <= 0) {
				_readStream.Close ();
				_buffer = null;
			}
		}
		if (_buffer != null) {
			bytesToWriteLength = bytesRead;
			bytesToWrite = _buffer;
		} else {
			return true; // completed
		}

		_UploadProgressChanged (bytesToWriteLength);
		WriteStream.BeginWrite (bytesToWrite, bufferOffset, bytesToWriteLength, new AsyncCallback (UploadBitsWriteCallback), this);

		return false; // not complete
	}

	static private void UploadBitsWriteCallback (IAsyncResult result)
	{
		DUWUpLoadTask state = (DUWUpLoadTask)result.AsyncState;
		Stream stream = (Stream)state._writeStream;

		bool completed = false;

		try {
			if (stream != null) {
				stream.EndWrite (result);
				completed = state.WriteBytes (stream);
			}
			if (completed) {
				state.SendResponseForSure ();
			}
		} catch (Exception e) {
			state.HandleError (e);
		} 
	}

	private void OnResponse (IAsyncResult result)
	{
		HttpWebRequest request = (HttpWebRequest)result.AsyncState;
		try {
			HttpWebResponse response = request.EndGetResponse (result) as HttpWebResponse;
			if (response.StatusCode == HttpStatusCode.OK) {
				_DoClose ();
				_SuccessComplete ();
			} else {
				_CreateError ().RecordResponseError (response.StatusCode, "ResponseStatusIsntOK");
				_DoClose ();
			}
			response.Close ();
		} catch (Exception e) {
			HandleError (e);
		}
	}

	void _UploadProgressChanged (long writeBytesCount)
	{
		_sends += writeBytesCount;
		int ProgressPercentage = (int)(100 * _sends / _contentLength);
		_progress = 0.01f * ProgressPercentage;
		var delta = writeBytesCount;
		_recordSends += delta;
	}

	public override void FireProgress (float passedTime)
	{
		_speed = _recordSends / passedTime;
		_recordSends = 0;
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
			}
		} else if (e is IOException) {
			_CreateError ().RecordIOError (e.ToString ());
		} else {
			_CreateError ().RecordSystemError (e.ToString ());
		}
		_DoClose ();
//		Debug.LogFormat ("{0} HandleError", _id);
//		Debug.Log (e);
		throw e;
	}

	void CloseReadStream ()
	{
		if (_readStream != null) {
//			Debug.LogFormat ("CloseReadStream {0} {1}", _id, _clonedTimes);
			_readStream.Close ();
			_readStream = null;
		}
	}

	public override DUTask CloneSelf ()
	{
		DUWUpLoadTask cloned = new DUWUpLoadTask (_id, _url, _localPath);
		cloned._state = _state;
		cloned.SetCallBacks (_completeHandler, _errorHandler, _progressHandler);
		cloned._clonedTimes = _clonedTimes + 1;
		return cloned;
	}
}
