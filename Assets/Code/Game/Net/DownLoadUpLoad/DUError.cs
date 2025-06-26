using UnityEngine;
using System.Collections;
using System.Net;

[SLua.CustomLuaClassAttribute]
public class DUError
{
	public const string REQUEST_TIMEOUT = "request time out";
	public const string DOWNLOADSTREAM_TIMEOUT = "download time out";
	public const string UPLOADSTREAM_TIMEOUT = "upload time out";
	protected int _taskID;
	protected HttpStatusCode _statusCode;
	protected WebExceptionStatus _webExceptionCode;
	protected DUErrorType _errorType;
	protected string _errorMsg;

	public DUErrorType errorType {
		get {
			return _errorType;
		}
	}

	public HttpStatusCode statusCode {
		get {
			return _statusCode;
		}
	}

	public string errorMsg {
		get {
			return _errorMsg;
		}
	}

	public int taskID {
		get {
			return _taskID;
		}
	}

	public DUError (int taskID)
	{
		this._taskID = taskID;
		_errorType = DUErrorType.Unknown;
	}

	public void RecordIOError (string errorMsg)
	{
		this._errorMsg = errorMsg;
		_errorType = DUErrorType.IO;
	}

	public void RecordResponseError (HttpStatusCode statusCode,string errorMsg)
	{
		this._errorMsg = errorMsg;
		this._statusCode = statusCode;
		_errorType = DUErrorType.Response;
	}

	public void RecordWebError (WebExceptionStatus webException, string errorMsg)
	{
		this._errorMsg = errorMsg;
		this._webExceptionCode = webException;
		_errorType = DUErrorType.Web;
	}

	public void RecordSystemError (string errorMsg)
	{
		this._errorMsg = errorMsg;
		_errorType = DUErrorType.System;
	}

	public void RecordTimeOutError (string errorMsg)
	{
		this._errorMsg = errorMsg;
		_errorType = DUErrorType.TimeOut;
	}

	public void RecordMD5Error (string errorMsg = null)
	{
		this._errorMsg = string.IsNullOrEmpty (errorMsg) ? "md5 doesnt match" : errorMsg;
		_errorType = DUErrorType.MD5Error;
	}
}

[SLua.CustomLuaClassAttribute]
public enum DUErrorType
{
	System,
	IO,
	Web,
	Response,
	TimeOut,
	MD5Error,
	Unknown
}