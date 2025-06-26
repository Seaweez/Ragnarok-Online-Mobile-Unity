using UnityEngine;
using System.Collections.Generic;
using System;

using UnityEngine.Networking;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class HttpWWWRequestOrder
	{
		private string _url;
		private WWWForm _formData;
		private bool _useCookie;
		private bool _autoDecodeJson;
		private Action<HttpWWWResponse> _successCall;
		private Action<HttpWWWRequestOrder> _overTimeCall;
		private Action<HttpWWWRequestOrder> _errorCall;
		private float _overTime;
		private float _passedTime = 0;
		private WWW _www;
		private UnityWebRequest _request;
		private int _responseCode = 0;
		public HttpErrorWraper errorWraper;
		public string orderError;
		//被抛弃的请求
		private bool _isAbandoned;

		public string url {
			get {
				return _url;
			}
		}

		public int responseCode {
			get
			{
				return _responseCode;
			}

		}

		public bool started {
			get {
				return _www != null || _request != null;
			}
		}

		public WWW www {
			get {
				return _www;
			}
			set {
				if (_www != value) {
					if (value != null) {
						if (value.url != _url)
							return;
					}
					_www = value;
				}
			}
		}

		public UnityWebRequest request {
			get {
				return _request;
			}
			set {
				if (_request != value) {
					if (value != null) {
						if (!value.url.Contains(_url))
							return;
					}
					_request = value;
				}
			}
		}

		public bool isDone {
			set;
			get;
		}

		public WWWForm formData {
			get {
				return _formData;
			}
		}

		public bool useCookie {
			get {
				return _useCookie;
			}
		}

		public bool autoDecodeJson {
			get {
				return _autoDecodeJson;
			}
		}

		public float passedTime {
			get {
				return _passedTime;
			}
			set {
				_passedTime = value;
			}
		}

		public bool isAbandoned {
			get {
				return _isAbandoned;
			}
			set {
				_isAbandoned = value;
			}
		}

		public HttpWWWRequestOrder (string url, float overTime, WWWForm formData, bool useCookie, bool autoDecodeJson)
		{
			this._url = url;
			this._overTime = overTime;
			this._formData = formData;
			this._useCookie = useCookie;
			this._autoDecodeJson = autoDecodeJson;
		}

		public void SetCallBacks (Action<HttpWWWResponse> successCall, Action<HttpWWWRequestOrder> overTimeCall, Action<HttpWWWRequestOrder> errorCall)
		{
			this._successCall = successCall;
			this._overTimeCall = overTimeCall;
			this._errorCall = errorCall;
		}

		public bool IsOverTime {
			get {
				if (_overTime > 0 && started) {
					return _passedTime >= _overTime;
				}
				return false;
			}
		}

		public void ErrorCall ()
		{
			if (_errorCall != null) {
				_errorCall (this);
			}
		}

		public void OverTimeCall ()
		{
			RequestTimeout();
			if (_overTimeCall != null) {
				_overTimeCall (this);
			}
		}

		public void SuccessCall (HttpWWWResponse rp)
		{
			if (_successCall != null) {
				_successCall (rp);
			}
		}

		public void Dispose ()
		{
			//RO.Logger.Log ("www request order dispose");
			this._successCall = null;
			this._overTimeCall = null;
			this._errorCall = null;
			if (_www != null) {
				_www.Dispose ();
				_www = null;
			}

			if (_request != null) {
				_request.Dispose ();
				_request = null;
			}
		}

		public void RequestTimeout()
		{
			this.errorWraper = new HttpErrorWraper();
			this.errorWraper.RequestTimeout();
		}

		public void RequestError()
		{
			this.errorWraper = new HttpErrorWraper();
			if (_www != null) {
				var code = getResponseCode(_www);
				this.errorWraper.RequestError(code,_www.error);
			}
			else if(_request != null){
				this.errorWraper.RequestError(_request.responseCode,_request.error);
			}
		}

		public void Cancel ()
		{
			this.isAbandoned = true;
			this._successCall = null;
			this._overTimeCall = null;
			this._errorCall = null;
		}

		public HttpWWWRequestOrder CloneSelf ()
		{
			HttpWWWRequestOrder clone = new HttpWWWRequestOrder (this._url, this._overTime, this._formData, this._useCookie, this._autoDecodeJson);
			clone.SetCallBacks (this._successCall, this._overTimeCall, this._errorCall);

			return clone;
		}

		public static HttpWWWRequestOrder CloneIt (HttpWWWRequestOrder source)
		{
			return source.CloneSelf ();
		}

		private static int getResponseCode(WWW request) {
			int ret = 0;
			if (request == null) {
				return ret;
			}
			if (request.responseHeaders == null) {
				return ret;
			}
			else {
				if (!request.responseHeaders.ContainsKey("STATUS")) {
					return ret;
				}
				else {
					ret = parseResponseCode(request.responseHeaders["STATUS"]);
					return ret;
				}
			}
		}

		private static int parseResponseCode(string statusLine) {
			int ret = 0;
			string[] components = statusLine.Split(' ');
			if (components.Length < 3) {
				return ret;
			}
			else {
				if (!int.TryParse(components[1], out ret)) {
					return ret;
				}
			}

			return ret;
		}
	}
} // namespace RO
