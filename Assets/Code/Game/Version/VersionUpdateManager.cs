using UnityEngine;
using System.Collections.Generic;
using System;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class VersionUpdateManager:MonoBehaviour
	{
		static string _CurrentVersion;
		static string _CurrentServerVersion;
		static string _serverResJsonString;

		UpdateBgmManager _bgmManager = new UpdateBgmManager();

		private AudioSource _bgmCtrl;

		public AudioSource bgmCtrl {
			set{ _bgmCtrl = value; }
		}

		public static string serverResJsonString {
			get {
				return _serverResJsonString;
			}
			set {
				_serverResJsonString = value;
			}
		}

		public static string CurrentVersion {
			get {
				return _CurrentVersion;
			}
			set {
				_CurrentVersion = value;
			}
		}

		public static string CurrentServerVersion {
			get {
				return _CurrentServerVersion;
			}
			set {
				_CurrentServerVersion = value;
			}
		}

		bool _isStarted = false;
		bool _needReCheck = false;
		Action _OnUpdateComplete;
		IVersionUpdateOpt _currentUpdate;
		NewInstallPanel _ui;
		Queue<IVersionUpdateOpt> _updates = new Queue<IVersionUpdateOpt> ();
		Queue<IVersionUpdateOpt> _checks = new Queue<IVersionUpdateOpt> ();

		public bool needReCheck {
			get {
				return _needReCheck;
			}
			set {
				_needReCheck = value;
			}
		}

		public static bool SkipCheckVersion {
			get { 
				LitJson.JsonData elementsData = ServerVersionCheckOpt.OperationJson.data ["elements"];
				if (elementsData.Keys.Contains ("skipVersion")) {
					bool res = false;
					if (bool.TryParse (elementsData ["skipVersion"].ToString (), out res)) {
						return res;
					}
				}
				return false;
			}
		}


		public void SetUI (NewInstallPanel ui)
		{
			_ui = ui;
		}

		public void AddUpdateOperation (IVersionUpdateOpt opt)
		{
			if (_updates.Contains (opt) == false)
				_updates.Enqueue (opt);
		}

		public IVersionUpdateOpt currentUpdate {
			get {
				return _currentUpdate;
			}
			set {
				if (_currentUpdate != null)
					_currentUpdate.Dispose ();
				_currentUpdate = value;
				_currentUpdate.SetManager (this);
				_currentUpdate.SetOnFinish (this.UpdateNext);
			}
		}

		public void StartUpdate (Action UpdateComplete)
		{
			#if RESOURCE_LOAD
			_updates.Clear ();
			#else
			if (_updates == null || _updates.Count == 0) {
				RO.LoggerUnused.LogError ("待更新的操作队列为空，请检查");
				return;
			}
			if (_isStarted)
				return;
			_isStarted = true;
			#endif
			_OnUpdateComplete = UpdateComplete;
			needReCheck = false;
			UpdateNext ();
		}

		void RestartUpdate ()
		{
			#if RESOURCE_LOAD
			#else
			if (_isStarted)
				return;
			_isStarted = true;
			VersionUpdateOperation.DisposeConfigs ();
			#endif
			needReCheck = false;
			_updates = _checks;
			_checks = new Queue<IVersionUpdateOpt> ();
			UpdateNext ();
		}

		void UpdateNext ()
		{
			if (_currentUpdate == null || _currentUpdate.state == VersionUpdateState.DONE) {
				if (_updates.Count > 0) {
					currentUpdate = _updates.Dequeue ();
					_checks.Enqueue (currentUpdate.GetNew ());
					_currentUpdate.Start ();
				} else {
					UpdateDone ();
				}
			}
		}

		void TryReCheck ()
		{
			if (needReCheck) {
				RestartUpdate ();
			}
		}

		void UpdateDone ()
		{
			_currentUpdate = null;
			_isStarted = false;
			if (needReCheck) {
				TryReCheck ();
			} else {
				DestroyBGMs ();
				if (_OnUpdateComplete != null)
					_OnUpdateComplete ();
				_OnUpdateComplete = null;
				_updates = new Queue<IVersionUpdateOpt> ();
				_checks = new Queue<IVersionUpdateOpt> ();
			}
		}

		public void ForceRecheck ()
		{
			_isStarted = false;
			if (_currentUpdate != null) {
				_currentUpdate.state = VersionUpdateState.DONE;
			}
			IVersionUpdateOpt recheck = null;
			while (_updates.Count > 0) {
				recheck = _updates.Dequeue ();
				_checks.Enqueue (recheck.GetNew ());
			}
			RestartUpdate ();
		}

		public void StartCheck ()
		{
			if (_ui != null)
				_ui.gameObject.SetActive (true);
		}

		public void ShowCheck (string check)
		{
//			if (_ui != null) {
//				_ui.ShowCheck (check);
//			}
		}

		public void ShowUpdate (string tip, string progressText)
		{
			if (_ui != null) {
				_ui.ShowUpdate (tip, progressText);
			}
		}

		public void UpdateProgress (float obj)
		{
			if (_ui != null) {
				_ui.Progress = obj;
			}
		}

		public void ShowYesConfirm (string msg, string confirmBtnText, Action confirmHandler)
		{
			_ui.ShowYesConfirm (msg, confirmBtnText, confirmHandler);
		}

		public void ShowConfirm (string confirmMsg, Action confirmHandler, Action cancelHandler, string confirmBtnText = "", string cancelBtnText = "", int cancelBtnBgState = 0)
		{
			_ui.ShowConfirm (confirmMsg, confirmHandler, cancelHandler, confirmBtnText, cancelBtnText, cancelBtnBgState);
		}

		public void ShowError (string content, string tip, Action errorHandler)
		{
			_ui.ShowError (content, tip, errorHandler);
		}

		public void HideConfirm ()
		{
			_ui.HideConfirm ();
		}

		public void HideError ()
		{
			_ui.HideError ();
		}

		public void CreateBGM ()
		{
			_bgmManager.AddBgm (_bgmCtrl);
			_bgmManager.Fork ();
		}

		public void FadeEndBGM()
		{
			_bgmManager.UnFork ();
			_bgmManager.FadeEndBgm ();
		}

		public void DestroyBGMs()
		{
			_bgmManager.DestroyAll ();
		}

		void Update ()
		{
			if (_currentUpdate != null)
				_currentUpdate.Update ();
		}

		internal class UpdateBgmManager
		{
			List<UpdateBgmCtrl> _bgms = new List<UpdateBgmCtrl>();
			bool _isForked;

			public void Fork()
			{
				_isForked = true;
			}

			public void UnFork()
			{
				_isForked = false;
			}

			public void DestroyAll()
			{
				for (int i = 0; i < _bgms.Count; i++) {
					_bgms [i].Destroy ();
				}
				_bgms.Clear ();
				UnFork ();
			}

			public void AddBgm(AudioSource source)
			{
				if (_isForked)
					return;
				UpdateBgmCtrl bgm = new UpdateBgmCtrl (source);
				_bgms.Add (bgm);
				bgm.Start ();
			}

			public void FadeEndBgm()
			{
				for (int i = 0; i < _bgms.Count; i++) {
					if (_bgms [i].isPlaying) {
						_bgms [i].End ();
						_bgms.RemoveAt (i);
						break;
					}
				}
			}
		}

		internal class UpdateBgmCtrl
		{
			AudioSource _source;
			AudioSource _origin;
			// 0: none , 1: playing , 2:ending , 3: destroyed
			int _state = -1;

			public UpdateBgmCtrl (AudioSource source)
			{
				_origin = source;
			}

			public void Start ()
			{
				if (_state < 1) {
					_state = 1;
					GameObject go = GameObject.Instantiate (_origin.gameObject);
					_source = go.GetComponent<AudioSource> ();
					_source.Play ();
				}
			}

			public void End ()
			{
				if (_state == 1) {
					_state = 2;
					TweenVolume t = TweenVolume.Begin (_source.gameObject, 1, 0);
					t.SetOnFinished (() => {
						Destroy ();
					});
				}
			}

			public void Destroy ()
			{
				if (_source != null && _state == 2) {
					_state = 3;
					GameObject.Destroy (_source.gameObject);
				}
				_source = null;
				_origin = null;
			}

			public bool isPlaying { get { return _state == 1; } }

			public bool isEnding { get { return _state == 2; } }
		}
	}
}
// namespace RO
