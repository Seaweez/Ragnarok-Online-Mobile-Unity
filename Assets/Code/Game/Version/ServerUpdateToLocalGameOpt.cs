using UnityEngine;
using System.Collections.Generic;
using System.IO;
using RO.Config;
using System;

namespace RO
{
	public class ServerUpdateToLocalGameOpt : ServerVersionCheckOpt
	{
		const float CheckDuration = 3.0f;
		int failureCount = 0;
		string unzipTargetPath;
		Queue<ClientVersionJsonData.AssetsUpdateInfo> _updateInfos;
		List<SingleUpdate> _checkUpdates;
		ClientVersionJsonData.AssetsUpdateInfo _currentDownload;
		SingleUpdate _currentUpdate;
		int _totalUpdateNum;
		long _totalSize = 0;
		int _currentIndex;
		UnZipFile unzip;
		byte[] contents;
		string _downloadedFilePath;
		int _loadID;
		string _md5Checkfile;

		public override IVersionUpdateOpt GetNew ()
		{
			return new ServerUpdateToLocalGameOpt ();
		}

		NetworkReachability currentApplicationInternet {
			get {
				return Application.internetReachability;
			}
		}

		protected override void CompareVersion ()
		{
			_loadID = -1;
			_currentUpdate = null;
			_md5Checkfile = Path.Combine (Application.persistentDataPath, ExtractFilesCheck.DOWNLOAD_ABZIPS_FILE_NAME_PREFIX);
			List<ClientVersionJsonData.AssetsUpdateInfo> infos = _remoteClientVersion.infos;
			if (VersionUpdateManager.SkipCheckVersion) {
				infos = null;
			}
			if (infos != null && infos.Count > 0) {
				_manager.CreateBGM ();
				_totalUpdateNum = infos.Count;
				_updateInfos = new Queue<ClientVersionJsonData.AssetsUpdateInfo> ();
				_checkUpdates = new List<SingleUpdate> ();
				_totalSize = 0;
				ClientVersionJsonData.AssetsUpdateInfo info = null;
				for (int i = 0; i < _totalUpdateNum; i++) {
					info = infos [i];
					_totalSize += info.size;
					_updateInfos.Enqueue (info);
					SingleUpdate checkUpdate = new SingleUpdate ();
					checkUpdate.info = info;
					checkUpdate.state = SingleUpdate.WAIT;
					_checkUpdates.Add (checkUpdate);
				}
				CheckWifiNetAndStartUpdate ();
			} else {
				Finish ();
			}
		}

		void ResetAutoCheck ()
		{
		}

		void CheckWifiNetAndStartUpdate ()
		{
			ResetAutoCheck ();
			WifiCheck (WifiValidHandle, 3);
		}

		protected void WifiValidHandle (bool v)
		{
			//判断到是wifi情况，自动开始更新，无需玩家确认
			if (v) {
				_manager.HideConfirm ();
				StartUpdate ();
			} else if (_totalSize > 0) {
				if (InternetIsValid ()) {
					_manager.ShowYesConfirm (string.Format (ROWords.NOWIFIENV_UPDATE_INFO, ClientVersionJsonData.humanReadableByteCount (_totalSize)), ROWords.DOWNLOAD_UPDATE, StartUpdate);
					_manager.UpdateProgress (0);
				}
			} else {
				_manager.HideConfirm ();
				StartUpdate ();
			}
		}

		protected override void StartUpdate ()
		{
			ResetAutoCheck ();
			unzipTargetPath = ROPathConfig.PersistentDirectory;
			_manager.HideConfirm ();
			DownLoadRes ();
		}

		protected override void UpdateDone ()
		{
			_manager.FadeEndBGM ();
			Finish ();
		}

		protected override void Finish ()
		{
			RO.LoggerUnused.Log ("step3: check and update server assets--done!!");
			if (VersionUpdateManager.SkipCheckVersion) {
				VersionUpdateManager.CurrentVersion = LocalVersion.currentVersion.ToString ();
			} else {
				VersionUpdateManager.CurrentVersion = _remoteClientVersion.clientVersion;
			}
			base.Finish ();
		}

		void DownLoadRes ()
		{
			if (_currentUpdate == null || (_currentUpdate.info == _currentDownload && _currentUpdate.state == SingleUpdate.DONE)) {
				if (_updateInfos.Count > 0) {
					unzip = GetUnZipFile ();
					_currentDownload = _updateInfos.Dequeue ();
					_currentIndex = _totalUpdateNum - _updateInfos.Count;
					_currentUpdate = _checkUpdates.Find ((c) => {
						return c.info == _currentDownload;
					});
					StartDownLoad ();
				} else
					UpdateDone ();
			}
		}

		void CheckValidDiskAndStartDownload ()
		{
			if (DeviceInfo.GetSizeOfValidMemory () >= _currentDownload.size) {
				StartDownLoad ();
			} else {
				string humanReadable = ClientVersionJsonData.humanReadableByteCount (_currentDownload.size);
				_manager.ShowYesConfirm (string.Format (ROWords.DOWNLOAD_HASNOSPACE, humanReadable), ROWords.QUIT_GAME, QuitGame);
			}
		}

		void StartDownLoad ()
		{
			if (_loadID != -1) {
				NetIngFileTaskManager.Ins.RemoveTaskFromID (_loadID);
			}
			_loadID = -1;
			_manager.ShowUpdate ("", ROWords.DOWNLOADING + "(" + _currentIndex.ToString () + "/" + _totalUpdateNum.ToString () + ")...{0:N0}%");
			ResetAutoCheck ();
			if (_currentDownload != null) {
				_manager.needReCheck = true;
				_loadID = NetIngFileTaskManager.Ins.Download (_currentDownload.url, false, null, LoadingHandler, LoadedResHandler, ErrorHandler);
				//				LuaWWW.WWWLoad (_currentDownload.url, LoadingHandler, LoadedResHandler);
			}
		}

		void LoadingHandler (float progress)
		{
			_manager.UpdateProgress (progress);
		}

		void ErrorHandler (int arg1, int errorCode, string errorMsg)
		{
			_manager.ShowUpdate (ROWords.POOR_NET, ROWords.TRY_RECONNET);
			_manager.UpdateProgress (0);
			Debug.LogFormat ("DownLoad failed:{0}", errorMsg);
			if (arg1 == (int)HTTPFileDownloader.E_Error.MD5Inconsistent) {
				//md5 check failed
				_manager.ShowError (ROWords.DOWNLOAD_FILE_MD5_ERROR, ROWords.CLICK_CONFIRM_REDOWNLOAD, TryRedownLoad);
				return;
			}
			failureCount++;
			if (failureCount >= 5) {
				failureCount = 0;
				BuglyAgent.PrintLog (LogSeverity.LogError, errorMsg);
				string msg = null;
				switch (errorCode) {
				case 2:
					_manager.ShowUpdate (ROWords.DOWNLOAD_OVERTIME, ROWords.TRY_RECONNET);
					_manager.UpdateProgress (0);
					break;
				case 404:
					msg = string.Format (ROWords.DOWNLOAD_FILE_404_ERROR, _saveVersion != null ? _saveVersion.currentVersion.ToString () : "null");
					_manager.ShowError (msg, ROWords.TRYING_FIX, TryRedownLoad);
					return;
				case 405:
					msg = string.Format (ROWords.DOWNLOAD_FILE_405_ERROR, _saveVersion != null ? _saveVersion.currentVersion.ToString () : "null");
					_manager.ShowError (msg, ROWords.TRYING_FIX, TryRedownLoad);
					return;
				default:
					msg = string.Format (ROWords.DOWNLOAD_FILE_DEFAULT_ERROR, errorCode.ToString (), errorMsg);
					_manager.ShowError (msg, ROWords.TRYING_FIX, TryRedownLoad);
					return;
				}
			}
			TryRedownLoad ();
		}

		void CancelCurrentTryReDownload ()
		{
			NetIngFileTaskManager.Ins.RemoveTaskFromID (_loadID);
			TryRedownLoad ();
		}

		void TryRedownLoad ()
		{
			ResetAutoCheck ();
			WifiCheck (WifiValidReDownloadHandle, 3);
		}

		protected void WifiValidReDownloadHandle (bool v)
		{
			//判断到是wifi情况，自动开始更新，无需玩家确认
			if (v) {
				StartDownLoad ();
				_manager.HideConfirm ();
			} else if (_totalSize > 0) {
				ConfirmCarrierDownload ();
			}
		}

		void ConfirmCarrierDownload ()
		{
			if (InternetIsValid ()) {
				_manager.ShowYesConfirm (string.Format (ROWords.NOWIFIENV_UPDATE_INFO, ClientVersionJsonData.humanReadableByteCount (_totalSize)), ROWords.DOWNLOAD_UPDATE, StartDownLoad);
				_manager.UpdateProgress (0);
			} else {
				_retryTimes++;
				if (_retryTimes >= 10) {
					_retryTimes = 0;
					#if !UNITY_EDITOR && (UNITY_IOS || UNITY_IPHONE)
					_manager.ShowConfirm("网络连接异常\n请检查你的网络设置, 稍后再次尝试！",TryRedownLoad,SwitchToSetting,"重试","网络设置",1);
					#else
					_manager.ShowYesConfirm (ROWords.ERROR_NET, ROWords.CONFIRM, TryRedownLoad);
					#endif
					return;
				}
			}
		}

		void LoadedResHandler (string downLoadedFilePath)
		{
			if (!string.IsNullOrEmpty (_currentDownload.md5) && _currentDownload.md5 != MyMD5.HashFile (downLoadedFilePath)) {
				ErrorHandler ((int)HTTPFileDownloader.E_Error.MD5Inconsistent, 0, "md5 check failed");
				return;
			}
			//检查剩余空间
			long res = lzip.getFileInfo (downLoadedFilePath, Application.persistentDataPath);
			if (DeviceInfo.GetSizeOfValidMemory () <= res) {
				_manager.ShowYesConfirm (ROWords.DOWNLOAD_UPZIP_NOSPACE, ROWords.QUIT_GAME, QuitGame);
				return;
			}
			_totalSize -= _currentDownload.size;
			_downloadedFilePath = downLoadedFilePath;
			_currentUpdate.state = SingleUpdate.DOWNLOADED;
			_manager.ShowUpdate (ROWords.UNZIPING_DONOT_LEAVE, ROWords.UNZIPPING + "(" + _currentIndex.ToString () + "/" + _totalUpdateNum.ToString () + ")...{0:N0}%");
			Debug.LogFormat ("开始解压{0}, download url:{1}", _downloadedFilePath, _currentDownload.url);
			unzip.StartUnZipFile (downLoadedFilePath, unzipTargetPath, _currentDownload.clientVersion, UpdateProgress, CheckFilesAndContinue, UnZipFailedCall);
		}

		void UpdateProgress (float p)
		{
			_manager.UpdateProgress (p);
		}

		void UnzipComplete (UnZipFile.UnZipFilesInfo unzipFileInfo)
		{
			if (File.Exists (_downloadedFilePath)) {
				File.Delete (_downloadedFilePath);
			}
			ROFileUtils.FileDelete (_md5Checkfile);
			int version = int.Parse ((string)unzipFileInfo.param);
			Debug.LogFormat ("解压完毕{0}, 设置版本号:{1}", _downloadedFilePath, version);
			SaveNewVersion (version);
			_currentUpdate.state = SingleUpdate.DONE;
			CheckWifiNetAndStartUpdate ();
		}

		void UnZipFailedCall (int res, object param)
		{
			ROFileUtils.FileDelete (_md5Checkfile);
			if (string.IsNullOrEmpty (_downloadedFilePath) == false) {
				unzip.StartUnZipFile (_downloadedFilePath, unzipTargetPath, param, UpdateProgress, CheckFilesAndContinue, UnZipFailedCall);
			}
		}

		UnZipFile GetUnZipFile ()
		{
			if (UnZipFile.Instance == null) {
				new GameObject ("UnZipFile").AddComponent<UnZipFile> ();
			}
			return UnZipFile.Instance;
		}

		void CheckFilesAndContinue (UnZipFile.UnZipFilesInfo unzipFileInfo)
		{
			int res = 0;
			ABZipMD5Infos info = ExtractFilesCheck.ParseZipMD5InfosByFile (_md5Checkfile, ref res);
			if (res < 0) {
				_manager.ShowYesConfirm (ROWords.ZIPFILE_VERIFYMD5_ERROR, ROWords.RETRY, () => {
					UnZipFailedCall (res, unzipFileInfo.param);
				});
				return;
			}
			if (unzipFileInfo.CheckExtractFiles (info) == false) {
				_manager.ShowYesConfirm (ROWords.ZIPFILE_VERIFYMD5_ERROR, ROWords.RETRY, () => {
					UnZipFailedCall (res, unzipFileInfo.param);
				});
				return;
			}
			_currentUpdate.state = SingleUpdate.UNZIPPED;
			UnzipComplete (unzipFileInfo);
		}

		public class SingleUpdate
		{
			public ClientVersionJsonData.AssetsUpdateInfo info;

			public const int WAIT = 0;
			public const int DOWNLOADED = 1;
			public const int UNZIPPED = 2;
			public const int DONE = 3;
			/**
			 * 0 wait
			 * 1 downloaded
			 * 2 unzipped
			 * 3 done
			 **/
			public int state;
		}
	}
}
// namespace RO
