using UnityEngine;
using System.Collections.Generic;
using RO.Config;
using System;
using LitJson;
using System.Text.RegularExpressions;

namespace RO
{
	public class ServerVersionCheckOpt : VersionUpdateOperation
	{
		protected int _retryTimes = 0;
		protected int _errorTimes = 0;
		protected string _cdnAddress;
		protected HttpWWWSeveralRequests _requestRemoteClientVersion;
        protected LocalizationManager _localizationManager;
        protected float _timeOut = 5.0f;
		const float _MAX_TIMEOUT_ = 15.0f;

		public bool InternetIsValid ()
		{
			return Application.internetReachability != NetworkReachability.NotReachable;
		}

		public bool IsWifi ()
		{
			return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
		}

		protected override void StartCheck()
		{
			base.StartCheck();
			_retryTimes = 0;
			_errorTimes = 0;
			//_cdnAddress = BuildBundleEnvInfo.CdnWithEnv;
			_cdnAddress = "http://api.pinidea.online/res/" + ApplicationHelper.platformFolder + "/";
			// Print _cdnAddress to Unity Console
			Debug.Log("_cdnAddress: " + _cdnAddress);


			_manager.ShowUpdate("", ROWords.CHECKING_REMOTE_VERSION);
			_manager.UpdateProgress(0);
			// Load installed version number
			LoadStreamVersion();
			// Load cached version number
			LoadSaveVersion();
			// Download server version
			LoadServerResVersion();
           // _localizationManager.Runscript();


        }


		virtual protected void LoadServerResVersion ()
		{
			if (_remoteClientVersion == null) {
				TryGetRemoteClientVersion ();
			} else {
				CompareVersion ();
			}
		}

		protected void SwitchToSetting ()
		{
			ExternalInterfaces.OpenAppSet ();
			TryGetRemoteClientVersion ();
		}

		void GetRemoteFailedCallBack (HttpWWWRequestOrder oder)
		{
			if (oder.IsOverTime) {
				_timeOut += UnityEngine.Random.Range (3f, 5f);
				_timeOut = Mathf.Min (_timeOut, _MAX_TIMEOUT_);
			}
			ReTryGetRemoteClientVersion (oder);
		}

		void ReTryGetRemoteClientVersion (HttpWWWRequestOrder order)
		{
			_manager.ShowUpdate (ROWords.POOR_NET, ROWords.CHECKING_REMOTE_VERSION);
			_manager.UpdateProgress (0);
			if (InternetIsValid () == false) {
				_retryTimes++;
				if (_retryTimes >= 3) {
					_retryTimes = 0;
					#if !UNITY_EDITOR && (UNITY_IOS || UNITY_IPHONE)
					_manager.ShowConfirm("网络连接异常\n请检查你的网络设置, 稍后再次尝试！",TryGetRemoteClientVersion,SwitchToSetting,"重试","网络设置",1);
					#else
					_manager.ShowYesConfirm (ROWords.ERROR_NET, ROWords.CONFIRM, TryGetRemoteClientVersion);
					#endif
				} else {
					TryGetRemoteClientVersion ();
				}
				return;
			}
			_errorTimes++;
			if (_errorTimes >= 8) {
				_errorTimes = 0;
				if (order.IsOverTime) {
					_ShowErrorMsg (ROWords.SERVER_VISIT_OVERTIME, TryGetRemoteClientVersion);
				} else {
					WWW www = order.www;
					if (www != null) {
						string error = order.www.error;
						if (error.Contains ("Couldn't")) {
							if (error.Contains ("resolve")) {
								_ShowErrorMsg (ROWords.SERVER_VISIT_ERROR, TryGetRemoteClientVersion);
							}
						} else {
							_ShowErrorMsg (ROWords.UNKNOWN_ERROR, TryGetRemoteClientVersion);
						}
					} else if (order.request != null) {
						HttpErrorWraper error = order.errorWraper;
						long errorCode = order.request.responseCode;
						if (error != null) {
							_ShowErrorMsg (error.ErrorMessage, TryGetRemoteClientVersion);
						} else if (errorCode != (long)System.Net.HttpStatusCode.OK) {
							if (errorCode == (long)System.Net.HttpStatusCode.NotFound) {
								_ShowErrorMsg (ROWords.SERVER_VISIT_ERROR, TryGetRemoteClientVersion);
							} else {
								_ShowErrorMsg (ROWords.UNKNOWN_ERROR, TryGetRemoteClientVersion);
							}
						} else {
							_ShowErrorMsg (ROWords.UNKNOWN_ERROR, TryGetRemoteClientVersion);
						}
					} else {
						_ShowErrorMsg (ROWords.UNKNOWN_ERROR, TryGetRemoteClientVersion);
					}
				}
			} else {
				TryGetRemoteClientVersion ();
			}
		}

		void _ShowErrorMsg (string msg, Action call, string tip = null, string format = null)
		{
			if (string.IsNullOrEmpty (format)) {
				format = ROWords.FEED_BACK;
			}
			if (string.IsNullOrEmpty (tip)) {
				tip = ROWords.TRYING_FIX;
			}
			_manager.ShowError (string.Format (format, msg), tip, call);
		}

		void TryGetRemoteClientVersion ()
		{
			if (this._requestRemoteClientVersion != null) {
				this._requestRemoteClientVersion.Dispose ();
				this._requestRemoteClientVersion = null;
			}
			this._requestRemoteClientVersion = new HttpWWWSeveralRequests ();
			WWWForm operation = OperationInfo;
			JsonData data = OperationJson.data ["urls"];
			if (data.IsArray) {
				for (int i = 0; i < data.Count; i++) {
					string url = data [i].ToString ();
					HttpWWWRequestOrder order = new HttpWWWRequestOrder (url + ":90/versionwindow.php?", _timeOut, operation, false, true);
					this._requestRemoteClientVersion.AddOrder (order);
				}
				this._requestRemoteClientVersion.SetCallBacks (OnResposeRemoteClientVersion, GetRemoteFailedCallBack);
				this._requestRemoteClientVersion.StartRequest ();
			}

		}

		virtual protected void OnResposeRemoteClientVersion (HttpWWWResponse response)
		{
			if (this._requestRemoteClientVersion != null) {
				this._requestRemoteClientVersion.Dispose ();
				this._requestRemoteClientVersion = null;
			}
			if (!string.IsNullOrEmpty (response.wwwError)) {
				Debug.LogError (response.wwwError);
				return;
			}
			_remoteClientVersion = new ClientVersionJsonData (response.resData);
			VersionUpdateManager.CurrentServerVersion = _remoteClientVersion.serverVersion;
			VersionUpdateManager.serverResJsonString = response.resString;
			Debug.Log (response.resString);
			if (response.Status != (int)HttpOperationJsonState.OK) {
				_manager.ShowError (_remoteClientVersion.GetValue ("errorstr"), _remoteClientVersion.GetValue ("errortip"), RestartCheckUpdate);
				return;
			}
			if (_remoteClientVersion.notifyCode > 0) {
				_manager.ShowYesConfirm (_remoteClientVersion.notify, ROWords.CONFIRM, CompareVersion);
				return;
			}
			CompareVersion ();
		}

		void RestartCheckUpdate ()
		{
			_manager.ForceRecheck ();
		}

		virtual protected void ForceUpdateGame (string content = null)
		{
			if (string.IsNullOrEmpty (content)) {
				content = ROWords.NEED_REINSTALL_NEW;
			}
			_manager.ShowYesConfirm (content, ROWords.GOTO, () => {
				List<ClientVersionJsonData.AssetsUpdateInfo> infos = _remoteClientVersion.infos;
				if (infos != null && infos.Count > 0) {
					string appUrl = infos [0].url;
					if (string.IsNullOrEmpty (appUrl)) {
						string chinese = WWW.EscapeURL ("-企业版", System.Text.Encoding.UTF8);
						Application.OpenURL ("http://api.pinidea.online/?dir=RO" + chinese);
					} else {  
						Application.OpenURL (ApplicationHelper.ParseToUTF8URL (appUrl));
					}
					QuitGame ();
				}
			});
		}

		override protected void QuitGame ()
		{
			Application.Quit ();
		}

		public static BuildBundleConfig LocalVersion {
			get { 
				if (_saveVersion == null) {
					// cached local version is null, then return install version which is contained in resources 
					return _appAssetVersion;
				} else {
					// pick up version config which the currentVersion is newer
					return _saveVersion.currentVersion >= _appAssetVersion.currentVersion ? _saveVersion : _appAssetVersion;
				}
			}
		}

		public static HttpOperationJson OperationJson {
			get { 
				if (_saveVersion == null) {
					// cached local version is null, then return install version which is contained in resources 
					return HttpOperationJson.ReadFromResourceFolder ();
				} else {
					// pick up version config which the currentVersion is newer
					return _saveVersion.currentVersion >= _appAssetVersion.currentVersion ? HttpOperationJson.Instance : HttpOperationJson.ReadFromResourceFolder ();
				}
			}
		}

		public static WWWForm OperationInfo {
			get {
				// /version?branch=$branch&phonename=$deviceName&phoneplat=iOS(or Android)&client=$version&plat=xd
				Debug.Log("AppEnvConfig.Instance: " + (AppEnvConfig.Instance != null));
				Debug.Log("LocalVersion: " + (LocalVersion != null));
				Debug.Log("OperationJson.data: " + (OperationJson.data != null));
				List<OperationInfoStruct> elements = new List<OperationInfoStruct> ();
				elements.Add (new OperationInfoStruct ("branch", AppEnvConfig.Instance.channelEnv));
				elements.Add (new OperationInfoStruct ("phonename", DeviceInfo.GetModel ()));
				elements.Add (new OperationInfoStruct ("phoneplat", ApplicationHelper.platformFolder));
				elements.Add (new OperationInfoStruct ("client", LocalVersion.currentVersion.ToString ()));
				elements.Add (new OperationInfoStruct ("clientCode", CompatibilityVersion.version.ToString ()));
				elements.Add (new OperationInfoStruct ("memory", DeviceInfo.GetSizeOfRAM ().ToString ()));
				elements.Add (new OperationInfoStruct ("totalDriveSize", (DeviceInfo.GetSizeOfMemory () / 1024).ToString ()));
				elements.Add (new OperationInfoStruct ("cpuName", DeviceInfo.GetCPUName ().ToString ()));
				elements.Add (new OperationInfoStruct ("screenWidth", DeviceInfo.GetScreenWidth ().ToString ()));
				elements.Add (new OperationInfoStruct ("screenHeight", DeviceInfo.GetScreenHeight ().ToString ()));
				elements.Add (new OperationInfoStruct ("gpuName", DeviceInfo.GetGPUName ().ToString ()));
				elements.Add (new OperationInfoStruct ("gpuType", DeviceInfo.GetGPUType ().ToString ()));
				//				elements.Add (new OperationInfoStruct ("client", "51100"));
				JsonData elementsData = OperationJson.data ["elements"];
				if (elementsData != null) {
					foreach (string key in elementsData.Keys) {
						elements.Add (new OperationInfoStruct (key, elementsData [key].ToString ()));
					}
				}
				WWWForm form = new WWWForm ();
				for (int i = 0; i < elements.Count; i++) {
					if (string.IsNullOrEmpty (elements [i].value) == false) {
						form.AddField (elements [i].key, WWW.EscapeURL (elements [i].value));
					}
				}
				return form;
			}
		}

		Action<bool> _wifiCheckCall;
		float _wifiCheckTimeOut;

		protected void WifiCheck (Action<bool> call, float timeout)
		{
			if (IsWifi ()) {
				_wifiCheckCall = null;
				call (true);
			} else {
				_wifiCheckCall = call;
				_wifiCheckTimeOut = Time.unscaledTime + timeout;
			}
		}

		void WifiCallBack ()
		{
			Action<bool> call = _wifiCheckCall;
			_wifiCheckCall = null;
			if (call != null) {
				call (IsWifi ());
			}
		}

		public override void Update ()
		{
			base.Update ();
			if (_wifiCheckCall != null) {
				if (!IsWifi ()) {
					if (Time.unscaledTime >= _wifiCheckTimeOut) {
						WifiCallBack ();
					}
				} else
					WifiCallBack ();

			}
		}

		struct OperationInfoStruct
		{
			public string key;
			public string value;

			public OperationInfoStruct (string k, string v)
			{
				this.key = k;
				this.value = v;
			}
		}

	}

	public class CheckNeedUpdateAppOpt:ServerVersionCheckOpt
	{
		protected override void CompareVersion ()
		{
			if (!VersionUpdateManager.SkipCheckVersion && _remoteClientVersion.forceUpdateApp) {
				ForceUpdateGame ();
				return;
			}
			Finish ();
		}

		protected override void Finish ()
		{
			Debug.Log ("step1: Check need update app --done!!");
			base.Finish ();
		}

		public override IVersionUpdateOpt GetNew ()
		{
			return new CheckNeedUpdateAppOpt ();
		}
	}
}
// namespace RO
