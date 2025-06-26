using UnityEngine;
using System.Collections;

[SLua.CustomLuaClassAttribute]
public class HttpErrorWraper
{

	private long _errorCode = -1;
	private string _errorMessage = "None";
	private HttpErrorType _errorType = HttpErrorType.None;


	public long ErrorCode{
		get{
			return this._errorCode;
		}
	}

	public string ErrorMessage{
		get{
			return this._errorMessage;
		}
	}

	public HttpErrorType ErrorType{
		get{
			return this._errorType;
		}
	}

	public void RequestTimeout()
	{
		this._errorCode = -1;
		this._errorType = HttpErrorType.Timeout;
		this._errorMessage = "Request Timeout.";
	}

	public void RequestError(long responseCode,string msg)
	{
		this._errorCode = responseCode;
		string errorMsg = "ResponseCode:{0},ResponseMsg:{1}.";
		errorMsg = string.Format(errorMsg,responseCode,msg);
		this._errorMessage = errorMsg;
		this._errorType = HttpErrorType.ErrorResponse;
	}

	[SLua.CustomLuaClassAttribute]
	public enum HttpErrorType
	{
		None,
		NetException,
		Timeout,
		ErrorResponse,
	}
}

