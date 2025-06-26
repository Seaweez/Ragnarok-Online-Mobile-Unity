using UnityEngine;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;
using RO.Config;
using System.IO;
using Ghost.Utils;

namespace RO
{
	public enum VersionUpdateState
	{
		NONE,
		CHECKING,
		UPDATING,
		DONE,
	}

	public enum VersionCheckResult
	{
		NONE,
		DONT_UPDATE,
		NEED_UPDATE_RES,
		FORCE_UPDATE_APP,
	}

	public interface IVersionUpdateOpt
	{
		VersionUpdateState state{ get; set; }

		void SetManager (VersionUpdateManager manager);

		void SetOnFinish (Action onFinish);

		void Dispose ();

		void Start ();

		void Update ();

		IVersionUpdateOpt GetNew();
	}

	public class VersionUpdateOperation:IVersionUpdateOpt
	{
		static XmlSerializer _serializer;

		public static XmlSerializer serializer {
			get {
				if (_serializer == null)
					_serializer = new XmlSerializer (typeof(BuildBundleConfig));
				return _serializer;
			}
		}

		public VersionUpdateState state { get; set; }

		public VersionCheckResult checkResult { get; private set; }

		public Action OnFinish { get; private set; }

		protected VersionUpdateManager _manager;
		static protected string _saveConfigFilePath;
		static protected string _localAppAssetFilePath;
		static protected BuildBundleConfig _saveVersion;
//		static protected BuildBundleConfig _serverVersion;
		static protected ClientVersionJsonData _remoteClientVersion;
		static protected BuildBundleConfig _appAssetVersion;
		static protected BuildBundleConfig _localAppAssetVersion;
//		protected float _checkingTime = 0;

//		virtual protected float LimitCheckTime{ get { return 3; } }

		public static void DisposeConfigs ()
		{
			_saveVersion = null;
			_remoteClientVersion = null;
			_appAssetVersion = null;
			_localAppAssetVersion = null;
			HttpOperationJson.ResetInstance ();
		}

		#region IVersionUpdateOpt implementation

		public virtual void SetManager (VersionUpdateManager manager)
		{
			_manager = manager;
		}

		public virtual void SetOnFinish (Action onFinish)
		{
			OnFinish = onFinish;
		}

		public virtual void Start ()
		{
			_manager.StartCheck ();
			StartCheck ();
		}

		protected virtual void Finish ()
		{
			state = VersionUpdateState.DONE;
			if (OnFinish != null)
				OnFinish ();
		}

		protected virtual void StartCheck ()
		{
			state = VersionUpdateState.CHECKING;
			_saveConfigFilePath = PathUnity.Combine (PathUnity.Combine (Application.persistentDataPath, ApplicationHelper.platformFolder), ROPathConfig.VersionFileName);
			//_localAppAssetFilePath = PathUnity.Combine (PathUnity.Combine (Application.persistentDataPath, ApplicationHelper.platformFolder), "LocalApp_Versions.xml");
		}

		protected virtual void CompareVersion ()
		{
		}

		protected virtual void StartUpdate ()
		{
		}

		protected virtual void UpdateDone ()
		{
		}

		protected void SaveNewVersion (int version)
		{
			BuildBundleConfig save = _saveVersion;
			if (save == null) {
				save = new BuildBundleConfig();
			}
			if (save != null) {
				save.currentVersion = version;
				save.SaveToFile (_saveConfigFilePath);
				_saveVersion = save;
			}
		}

		protected void SaveLocalStreamVersion(int version)
		{
			BuildBundleConfig localStream = _localAppAssetVersion ?? new BuildBundleConfig();
			localStream.currentVersion = version;
			localStream.SaveToFile(_localAppAssetFilePath);
			_localAppAssetVersion = localStream;
		}

		protected virtual void LoadLocalStreamVersion()
		{
			if (File.Exists (_localAppAssetFilePath))
			{
				if (_localAppAssetVersion == null)
				{
					string text = File.ReadAllText (_localAppAssetFilePath);
					if (string.IsNullOrEmpty (text) == false)
					{
						_localAppAssetVersion = BuildBundleConfig.CreateByStr (text);
					}
				}
			}
		}
		
		protected virtual void LoadStreamVersion ()
		{
			if (_appAssetVersion == null) {
				TextAsset ta = Resources.Load<TextAsset> (ROPathConfig.TrimExtension (ROPathConfig.VersionFileName));
				if (ta != null) {
					_appAssetVersion = BuildBundleConfig.CreateByStr (ta.text);
					RO.LoggerUnused.LogFormat ("App install version:{0}", _appAssetVersion.currentVersion);
				}
			}
		}

		protected virtual void LoadSaveVersion ()
		{
			if (File.Exists (_saveConfigFilePath)) {
				if (_saveVersion == null) {
					string text = File.ReadAllText (_saveConfigFilePath);
					if (string.IsNullOrEmpty (text) == false) {
						_saveVersion = BuildBundleConfig.CreateByStr (text);
						RO.LoggerUnused.LogFormat ("Game cached version:{0}", _saveVersion.currentVersion);
					}
				}
			}
		}

		public virtual void Dispose ()
		{
			_manager = null;
			OnFinish = null;
		}

		public virtual void Update ()
		{
		}

		virtual protected void QuitGame ()
		{
			Application.Quit ();
		}

		virtual public IVersionUpdateOpt GetNew()
		{
			return null;
		}
		#endregion
	}
} // namespace RO
