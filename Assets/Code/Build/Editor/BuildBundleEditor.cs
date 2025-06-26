using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO.Config;
using System.Xml.Serialization;
using System.IO;
using RO;
using System;
using Ghost.Utils;

namespace EditorTool
{
	public static class BuildBundleEditor
	{
		static BuildBundleConfig _config;
		static XmlSerializer _serializer;
		static string _Env;

		public static string Env {
			set{ _Env = value;}
			get {
				if (_Env == null) {
					_Env = AppEnvConfig.Instance.channelEnv;
					//						BuildBundleEnvInfo.Env
				}
				return _Env;
			}
		}
		
		public static XmlSerializer serializer {
			get {
				if (_serializer == null)
					_serializer = new XmlSerializer (typeof(BuildBundleConfig));
				return _serializer;
			}
		}
		
		public static BuildBundleConfig config {
			get {
				if (_config == null) {
					if (File.Exists (PathHelper.GetVersionConfigPath (true, Env))) {
						TextAsset ta = AssetDatabase.LoadAssetAtPath<TextAsset> (PathHelper.GetVersionConfigPath (false, Env));
						StringReader sr = new StringReader (ta.text);
						_config = (BuildBundleConfig)serializer.Deserialize (sr);
						sr.Close ();
					} else
						_config = new BuildBundleConfig ();
				}
				return _config;
			}
			set {
				_config = value;
			}
		}

		[MenuItem( "AssetBundle/测试Env&Sdk")]
		public static void TestSetEnv ()
		{
			string sdk = "None";
			BuildBundleEnvInfo.SetEnv ("Develop", sdk);
			string Define = ScriptDefines.ReplaceByPart ("_LINK_NATIVE", string.Format ("_{0}_LINK_NATIVE_", sdk));
			PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.iOS, Define);
		}
		
		[MenuItem( "AssetBundle/测试版本号")]
		public static void TestSerRes ()
		{
			SetResVersion (100, 120, 0, "1.0", false, false);
			SetResVersion (121, 150, 0, "1.0", false, false);
			SetResVersion (121, 150, 0, "1.0", false, false);
			SetResVersion (121, 150, 0, "1.0", false, false);
			BuildApp (false);
			BuildApp (false);
			SetResVersion (121, 150, 0, "1.0", false, false);
			SetResVersion (121, 150, 0, "1.1", false, false);
			SetResVersion (121, 150, 0, "1.1", false, false);
			BuildApp (true, 0, "1.2");
			config = null;
		}
		
		public static void BuildApp (bool save, int version = 0, string serverversion = "1.0")
		{
			BuildBundleInfo latest = null;
			int latestVer = (version != 0 ? version : config.LastInfoNextVersion);
			Debug.Log (config.infos.Count);
			if (config.infos.Count > 0) {
				latest = config.LastInfo;
				version = (version != 0 ? version : latest.endV);
				SetResVersion (version, version, latestVer, serverversion, true, false);
			}
			SetCurrentVersion (latestVer, save);
		}
		
		public static void CmdSetResVersion ()
		{
			List<string> args = CommandArgs.GetCommandArgs ();
			if (args.Count >= 5) {
				int start = int.Parse (args [0]);
				int end = int.Parse (args [1]);
				int version = int.Parse (args [2]);
				bool forceUpdate = bool.Parse (args [3]);
				bool isCurrent = bool.Parse (args [4]);
				if (args.Count > 5) {
					Env = args [5];
				}
				string serverVersion = "";
				if (args.Count > 6) {
					serverVersion = args [6];
				}
				
				SetResVersion (start, end, version, serverVersion, forceUpdate, !isCurrent);
				if (isCurrent)
					SetCurrentVersion (version, true);
			} else
				throw new Exception ("参数少于4个！");
		}

		public static void CmdSaveCurrentVersionToPersistentDataPath()
		{
			List<string> args = CommandArgs.GetCommandArgs ();
			if (args.Count >= 1) {
				int current = int.Parse (args [0]);
				SaveCurrentVersionToPersistentDataPath(current);
			}
		}
		
		public static void SetResVersion (int start, int end, int version, string serverVersion, bool forceUpdate, bool save)
		{
			if (version == 0) {
				version = config.LastInfoNextVersion;
			}
			config.AddInfo (start, end, version, serverVersion, forceUpdate);
			if (save)
				SaveVersionsXML ();
		}
		
		public static void CmdSetCurrentVersion ()
		{
			List<string> args = CommandArgs.GetCommandArgs ();
			if (args.Count >= 1) {
				int current = int.Parse (args [0]);
				SetCurrentVersion (current);
			} else
				throw new Exception ("参数少于1个！");
		}
		
		public static void SetCurrentVersion (int current, bool save = true)
		{
			int latest = 999999;
			if (config.LastInfo != null) {
				latest = config.LastInfo.version;
			}
			config.currentVersion = Mathf.Min (current, latest);
			if (save)
				SaveVersionsXML ();
		}
		
		static void SaveVersionsXML ()
		{
			//			PathHelper
			//使用 holder 建立名为 dataHolder.asset 的资源
			string fileName = PathHelper.GetVersionConfigPath (true, Env);
			string folder = Path.GetDirectoryName (fileName);
			if (Directory.Exists (folder) == false)
				Directory.CreateDirectory (folder);
			TextWriter writer = new StreamWriter (fileName);
			serializer.Serialize (writer, config);
			writer.Close ();
			SaveCurrentVersionToPersistentDataPath (config.currentVersion);
			AssetDatabase.Refresh ();
		}

		public static void SaveCurrentVersionToPersistentDataPath (int version)
		{
			BuildBundleConfig config = new BuildBundleConfig ();
			config.currentVersion = version;
			string folder = PathUnity.Combine (PathUnity.Combine (Application.dataPath, BundleLoaderStrategy.EditorRoot), ApplicationHelper.platformFolder);
			string fileName = PathUnity.Combine (folder, ROPathConfig.VersionFileName);
			config.SaveToFile (fileName);
		}
	}
} // namespace EditorTool