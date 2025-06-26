using UnityEngine;
using System.Collections.Generic;
using System.IO;
using RO.Config;

namespace RO
{
	//游戏与本地缓存中的版本比较
	public class AppUpdateToLocalGameOpt : VersionUpdateOperation
	{
		int index = 0;
		int totalCount = 0;
		ABZipMD5Infos _installMD5Info;

		public override IVersionUpdateOpt GetNew ()
		{
			return new AppUpdateToLocalGameOpt ();
		}

		protected override void StartCheck ()
		{
			base.StartCheck ();
			_manager.ShowUpdate ("", ROWords.CHECKING_LOCAL_VERSION);
			_manager.UpdateProgress (0);
			RO.LoggerUnused.Log (_saveConfigFilePath);
			//加载游戏内部版本号
			LoadStreamVersion ();
			//加载本地缓存版本号
			LoadSaveVersion ();
#if ARCHIVE_AB
			//加载本地安装包版本
			LoadLocalStreamVersion();
#endif
			//对比
			CompareVersion ();
		}

		protected override void CompareVersion ()
		{
			if (NeedUnzipStream)
			{
#if ARCHIVE_AB
				string persistentPath = Application.persistentDataPath + "/" + ApplicationHelper.platformFolder;
				if (Directory.Exists(persistentPath))
				{
					Directory.Delete(persistentPath, true);
				}
				Directory.CreateDirectory(persistentPath);

				//保存安装包版本号到本地
				_saveVersion = null;
				LoadSaveVersion ();
				SaveNewVersion (_appAssetVersion.currentVersion);
				SaveLocalStreamVersion (_appAssetVersion.currentVersion);
				
				//找不到bug，加此句可以解决bug
				DisposeConfigs();
				
				//调用结束的回调函数
				Finish();
#else
				_manager.CreateBGM ();
				//Check valid disk size
				CheckValidToStartUnzip ();
#endif
			}
			else
			{
				Finish ();
			}
		}

		void CheckValidToStartUnzip ()
		{
			long valid = DeviceInfo.GetSizeOfValidMemory ();
			if (valid >= InstallAssetZips.Instance.unzipSize) {
				StartUpdate ();
			} else {
				string humanReadable = ClientVersionJsonData.humanReadableByteCount (InstallAssetZips.Instance.unzipSize);
				_manager.ShowYesConfirm (string.Format (ROWords.HARDWARE_NOTENOUGH, humanReadable), ROWords.QUIT_GAME, QuitGame);
			}
		}

		private bool NeedUnzipStream {
			get {
				if (_saveVersion == null)
					return true;
#if ARCHIVE_AB
                if (_localAppAssetVersion == null)
                    return true;

				if (_localAppAssetVersion.currentVersion != _appAssetVersion.currentVersion)
					return true;
#endif
                if (_saveVersion.currentVersion < _appAssetVersion.currentVersion)
					return true;
				return false;
			}
		}

		protected override void StartUpdate ()
		{
			totalCount = InstallAssetZips.Instance.names.Count;
			string unzipTargetPath = ROPathConfig.PersistentDirectory;
			_installMD5Info = ExtractFilesCheck.ReadABZipMD5InfosInResource ();
			SingleDone ();
		}

		void ShowMsg ()
		{
			_manager.ShowUpdate (string.Format (ROWords.UNZIPING_FILES_PROGRESS, Mathf.Min (index + 1, totalCount), totalCount), ROWords.UNZIPING_PROGRESS);
		}

		void SingleDone ()
		{
			if (index < totalCount) {
				System.GC.Collect ();
				string zipName = InstallAssetZips.Instance.names [index].zipName;
				string streamPath = Path.Combine (Application.streamingAssetsPath, zipName);
				string unzipTargetPath = ROPathConfig.PersistentDirectory;
				ShowMsg ();
				#if UNITY_ANDROID
				#if UNITY_EDITOR
					streamPath = "file:///" + streamPath;
				#endif
				UnZipFile.Me.StartUnZipFileAndroid (streamPath, unzipTargetPath,null, _manager.UpdateProgress, CheckFilesAndContinue,UnzipFailed);
				#else
				UnZipFile.Me.StartUnZipFile (streamPath, unzipTargetPath, null, _manager.UpdateProgress, CheckFilesAndContinue, UnzipFailed);
				#endif
				index += 1;
			} else {
				UpdateDone ();
			}
		}

		void CheckFilesAndContinue (UnZipFile.UnZipFilesInfo unzipFileInfo)
		{
			if (!unzipFileInfo.CheckExtractFiles (_installMD5Info)) {
				_manager.ShowYesConfirm (ROWords.ZIPFILE_VERIFYMD5_ERROR, ROWords.RETRY, ReUnZipCurrent);
				return;
			}
			SingleDone ();
		}

		void UnzipFailed (int res, object param)
		{
			switch (res) {
			case -1:
				_manager.ShowYesConfirm (ROWords.ZIPFILE_BROKEN_ERROR, ROWords.QUIT_GAME, QuitGame);
				break;
			case -2:
				_manager.ShowConfirm ("unzip error code:-2", ReUnZipCurrent, QuitGame, ROWords.RETRY, ROWords.QUIT_GAME);
				break;
			case -3:
				_manager.ShowConfirm (ROWords.ZIPFILE_UNZIP_ERROR, ReUnZipCurrent, QuitGame, ROWords.RETRY, ROWords.QUIT_GAME);
				break;
			case UnZipFile.ERROR_HAS_NO_FILE:
				_manager.ShowYesConfirm (ROWords.ZIPFILE_NOTFOUND_ERROR, ROWords.QUIT_GAME, QuitGame);
				break;
			case UnZipFile.ERROR_STILL_UNZIPPING:
				_manager.ShowYesConfirm (ROWords.ZIPFILE_DUPLICATE_UNZIPPING_ERROR, ROWords.QUIT_GAME, QuitGame);
				break;
			case UnZipFile.ERROR_READ_ZIPINFO:
				_manager.ShowConfirm (ROWords.ZIPFILE_ZIPINFO_ERROR, ReUnZipCurrent, QuitGame, ROWords.RETRY, ROWords.QUIT_GAME);
				break;
			default:
				_manager.ShowConfirm (ROWords.ZIPFILE_UNKNOWN_ERROR, ReUnZipCurrent, QuitGame, ROWords.RETRY, ROWords.QUIT_GAME);
				break;
			}
		}

		void ReUnZipCurrent ()
		{
			//index backward
			index = Mathf.Max (0, index - 1);
			SingleDone ();
		}

		protected override void UpdateDone ()
		{
			HttpOperationJson.ResetInstance ();
//			HttpOperationJson.ReadFromResourceFolder ().SaveToPersistentDataPath ();
			_saveVersion = null;
			LoadSaveVersion ();
			SaveNewVersion (_appAssetVersion.currentVersion);
//			_appAssetVersion.SaveToFile (_saveConfigFilePath);
//			_saveVersion = _appAssetVersion;
			Finish ();
		}

		protected override void Finish ()
		{
			RO.LoggerUnused.Log ("step2: app update to local--done!!");
			base.Finish ();
		}
	}
}
// namespace RO
