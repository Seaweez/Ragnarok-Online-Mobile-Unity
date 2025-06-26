using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Text;
using UnityEngine.Networking;
using LitJson;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class HttpWWWRequest : SingleTonGO<HttpWWWRequest>
	{
		static Dictionary<string,string> _HEADER = new Dictionary<string, string> ();
		public float overTime = 10.0f;
		float _currentTime;
		//最多同时开启的WWW数量,0为无限制
		int _wwwQueueMaxNum = 0;
		List<HttpWWWRequestOrder> _waitingList = new List<HttpWWWRequestOrder> ();
		List<HttpWWWRequestOrder> _requestingList = new List<HttpWWWRequestOrder> ();
		protected override void Awake ()
		{
			base.Awake ();
			_currentTime = Time.realtimeSinceStartup;
		}

		public static HttpWWWRequest Instance {
			get {
				return Me;
			}
		}

		public GameObject monoGameObject {
			get {
				return gameObject;
			}
		}

		public int wwwQueueMaxNum {
			get {
				return _wwwQueueMaxNum;
			}
			set {
				_wwwQueueMaxNum = value;
			}
		}

		public bool RequestListNotFull {
			get {
				return _requestingList.Count < wwwQueueMaxNum || wwwQueueMaxNum == 0;
			}
		}

		public int CurrentFitCount {
			get {
				if (wwwQueueMaxNum == 0) {
					return _waitingList.Count;
				}
				return Mathf.Max (0, wwwQueueMaxNum - _requestingList.Count);
			}
		}

		public bool Request (string serverroot, string operation, WWWForm data, float overTime, bool useCookie, bool autoDecodeJson, Action<HttpWWWResponse> successCall, Action<HttpWWWRequestOrder> overTimeCall, Action<HttpWWWRequestOrder> errorCall)
		{
			string url = serverroot + operation;
			HttpWWWRequestOrder order = new HttpWWWRequestOrder (url, overTime, data, useCookie, autoDecodeJson);
			order.SetCallBacks (successCall, overTimeCall, errorCall);
			RequestByOrder (order);
			return true;
		}

		public bool RequestByOrder (HttpWWWRequestOrder order)
		{
			TryRequestRightNow (order);
			return true;
		}

		public void RestartQuestByOrder (HttpWWWRequestOrder order, bool forceRightNow)
		{
			HttpWWWRequestOrder restartOrder = order.CloneSelf ();
			if (forceRightNow) {
				DoRequest (restartOrder);
			} else {
				RequestByOrder (restartOrder);
			}
		}

		void TryRequestRightNow (HttpWWWRequestOrder order)
		{
			if (RequestListNotFull) {
				DoRequest (order);
			} else {
				AddToWait (order);
			}
		}

		void DoRequest (HttpWWWRequestOrder order)
		{
			StartCoroutine (PostByUnityWebRequest (order));
		}

		void AddToRequesting (HttpWWWRequestOrder order)
		{
			if (_requestingList.Contains (order) == false) {
				_requestingList.Add (order);
				RemoveWaiting (order);
			}
		}

		void RemoveRequest (HttpWWWRequestOrder order)
		{
			_requestingList.Remove (order);
			RemoveWaiting (order);
		}

		void AbandonRequest (HttpWWWRequestOrder order)
		{
			//RO.Logger.Log ("AbandonRequest "+order.url);
			RemoveRequest (order);
			order.isAbandoned = true;
		}

		void AddToWait (HttpWWWRequestOrder order)
		{
			if (_waitingList.Contains (order) == false)
				_waitingList.Add (order);
		}

		void RemoveWaiting (HttpWWWRequestOrder order)
		{
			_waitingList.Remove (order);
		}


		IEnumerator PostByUnityWebRequest (HttpWWWRequestOrder order)
		{
			if (order != null) {
				UnityWebRequest request = null;				
//				ROLogger.Log ("url: " + order.url);
				if (order.formData != null) {
//					ROLogger.Log("WWWForm Data :" + Encoding.Default.GetString (order.formData.data));
					if (order.useCookie) {
						//request = new WWW (order.url, order.formData.data, _HEADER);
						//request = UnityWebRequest.SerializeSimpleForm (_HEADER);
						//UnityWebRequest.SerializeSimpleForm (_HEADER);
						request = UnityWebRequest.Post(order.url,order.formData);
					} else {
						request = UnityWebRequest.Post(order.url,order.formData);
					}
				} else {
					if (order.useCookie) {
						request = UnityWebRequest.Post(order.url,_HEADER);
					} else {
						request = UnityWebRequest.Get(order.url);
					}
				}
	
				order.request = request;

				order.passedTime = 0;

				AddToRequesting (order);

				yield return request.SendWebRequest();

				if (order != null) {
					HttpWWWResponse rp = new HttpWWWResponse ();
					order.isDone = request.isDone;
					var errorMsg = GetErrorMsg(request);
					if (null == errorMsg && order.isAbandoned == false) {
						rp.resString = request.downloadHandler.text;
						if (request.downloadHandler.text == string.Empty) {
//							ROLogger.LogWarning ("http request answer is Empty");
						}
						//RO.Logger.Log ("Res is:" + rp.resString);
						if (order.autoDecodeJson)
							rp.resData = Process (request, false);
						order.SuccessCall (rp);
					} else {						
						rp.wwwError = errorMsg;
						order.orderError = errorMsg;
						order.RequestError();
						if (rp.wwwError != null && rp.wwwError != "") {
//							ROLogger.Log("www error:" + rp.wwwError);
							order.ErrorCall ();
						} else {
//							ROLogger.Log("收到已超时的请求 " + request.url);
						}
					}
					order.Dispose ();
				} else {
					request.Dispose ();
					request = null;
				}
			} else
				yield break;
		}

		string GetErrorMsg(UnityWebRequest request)
		{
			if(request.responseCode != 200)
			{	
				var error = request.error == null?"error is null;": request.error;
				if(null == request.error && null != request.downloadHandler)
				{
					var text = request.downloadHandler.text;
					error = error + (text == null?";text is null":text);
				}
				return error;
			}
			return null;
		}

		IEnumerator Post (HttpWWWRequestOrder order)
		{
			if (order != null) {
				WWW www;

				RO.LoggerUnused.Log ("url: " + order.url);
				if (order.formData != null) {
					RO.LoggerUnused.Log ("WWWForm Data :" + Encoding.Default.GetString (order.formData.data));
					if (order.useCookie) {
						www = new WWW (order.url, order.formData.data, _HEADER);
					} else {
						www = new WWW (order.url, order.formData);
					}
				} else {
					if (order.useCookie) {
						WWWForm form = new WWWForm ();
						form.AddField ("a", "a");//不允许空表单，随便加点内容
						www = new WWW (order.url, form.data, _HEADER);
					} else {
						www = new WWW (order.url);
					}
				}

				order.www = www;

				order.passedTime = 0;

				AddToRequesting (order);

				yield return www;

				if (order != null) {
					HttpWWWResponse rp = new HttpWWWResponse ();
					order.isDone = www.isDone;
					if (string.IsNullOrEmpty (www.error) && order.isAbandoned == false) {
						if (order.useCookie && www.responseHeaders.ContainsKey ("SET-COOKIE")) {
							if (_HEADER.ContainsKey ("Cookie")) {
								_HEADER ["Cookie"] = www.responseHeaders ["SET-COOKIE"].Split (';') [0];
							} else {
								_HEADER.Add ("Cookie", www.responseHeaders ["SET-COOKIE"].Split (';') [0]);
							}
						}
						rp.resString = www.text;
						if (www.text == string.Empty) {
							RO.LoggerUnused.LogWarning ("http request answer is Empty");
						}
						//RO.Logger.Log ("Res is:" + rp.resString);
						if (order.autoDecodeJson)
							rp.resData = Process (www, false);
						order.SuccessCall (rp);
					} else {
						order.RequestError();
						rp.wwwError = www.error;
						if (rp.wwwError != null && rp.wwwError != "") {
							RO.LoggerUnused.Log ("www error:" + rp.wwwError);
							order.ErrorCall ();
						} else {
							RO.LoggerUnused.Log ("收到已超时的请求 " + www.url);
						}
					}
					order.Dispose ();
				} else {
					www.Dispose ();
					www = null;
				}
			} else
				yield break;
		}

		public JsonData Process (WWW www, bool bUTF8)
		{
			if (!www.isDone)
				return null;

			if (www.bytes.Length == 0)
				return null;

			//string encoding = string.Empty;
			//return JsonMapper.ToObject(www.text);
			try {
				if (bUTF8)
					return JsonMapper.ToObject (Encoding.UTF8.GetString (www.bytes, 0, www.bytes.Length));
				else {
					return JsonMapper.ToObject (www.text);
				}
			} catch (System.Exception ex) {
				//BuglyAgent.PrintLog (LogSeverity.LogError, www.text);
				//BuglyAgent.PrintLog (LogSeverity.LogError, ex.ToString ());
				RO.LoggerUnused.Log ("res:" + www.text);
				RO.LoggerUnused.LogWarning (ex.ToString ());
				return null;
			}      
		}

		public JsonData Process (UnityWebRequest request, bool bUTF8)
		{
			if (!request.isDone)
				return null;

			if (request.isNetworkError || request.downloadHandler == null || request.downloadHandler.data == null)
				return null;

			//string encoding = string.Empty;
			//return JsonMapper.ToObject(www.text);
			try {
				if (bUTF8){
					var data = request.downloadHandler.data;
					return JsonMapper.ToObject (Encoding.UTF8.GetString (data, 0, data.Length));
				}					
				else {
					return JsonMapper.ToObject (request.downloadHandler.text);
				}
			} catch (System.Exception ex) {
				//BuglyAgent.PrintLog (LogSeverity.LogError, request.downloadHandler.text);
				//BuglyAgent.PrintLog (LogSeverity.LogError, ex.ToString ());
				RO.LoggerUnused.Log ("res:" + request.downloadHandler.text);
				RO.LoggerUnused.LogWarning (ex.ToString ());
				return null;
			}      
		}

		void LateUpdate ()
		{
			float passed = (Time.realtimeSinceStartup - _currentTime);
			_currentTime = Time.realtimeSinceStartup;
			if (_requestingList.Count > 0) {
				HttpWWWRequestOrder order;
				//遍历检查是否有www已接到返回
				for (int i = _requestingList.Count - 1; i >= 0; i--) {
					order = _requestingList [i];
					if (order.isDone) {
						RemoveRequest (order);
					} else {
						order.passedTime += passed;
						if (order.IsOverTime) {
							AbandonRequest (order);
							order.OverTimeCall ();
						}
					}
				}
			}
			//检查是否需要将waiting添加至requesting
			if (_waitingList.Count > 0 && RequestListNotFull) {
				for (int i = _waitingList.Count - 1; i >= 0; i--) {
					DoRequest (_waitingList [i]);
					if (RequestListNotFull == false)
						break;
				}
			}
		}

		public void Clear ()
		{
			CancelAllOrders (_waitingList);
			CancelAllOrders (_requestingList);
		}

		void CancelAllOrders (List<HttpWWWRequestOrder> orders)
		{
			for (int i=0; i<orders.Count; i++) {
				orders [i].Cancel ();
			}
			orders.Clear ();
		}

		public void DelayTestRequest (HttpWWWRequestOrder order, float delay)
		{
			AddToRequesting (order);
			LeanTween.delayedCall (delay, () => {
				DoRequest (order);
			});
		}
	}
} // namespace RO
