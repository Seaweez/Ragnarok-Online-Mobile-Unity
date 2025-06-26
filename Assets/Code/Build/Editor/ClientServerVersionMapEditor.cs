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
	public static class ClientServerVersionMapEditor
	{
		static ClientServerVersionMap _config;

		static string _Env;

		public static string Env {
			set{ _Env = value; }
			get {
				if (_Env == null) {
					_Env = AppEnvConfig.Instance.channelEnv;
				}
				return _Env;
			}
		}

		static string FilePath {
			get {
				string fileName = PathUnity.Combine (Env, ApplicationHelper.platformFolder + "_ClientMapMaxServer.xml");
				return PathUnity.Combine ("Assets/", BundleLoaderStrategy.EditorRoot) + fileName;
			}
		}

		public static ClientServerVersionMap config {
			get {
				if (_config == null) {
					_config = ClientServerVersionMap.CreateByFile (FilePath);
					if (_config == null) {
						_config = new ClientServerVersionMap ();
					}
				}
				return _config;
			}
			set {
				_config = value;
			}
		}

		[MenuItem ("AssetBundle/ClientServer")]
		public static void TestSerRes ()
		{
			SetClientMaxServerVersion (1, "2.0.0", false);
			SetClientMaxServerVersion (2, "0", true);
			SetClientMaxServerVersion (3, "2.0.5", false);
			SetClientMaxServerVersion (4, "2.0.11", true, true);
			config = null;
		}

		public static void CmdSetClientMaxServerVersion ()
		{
			List<string> args = CommandArgs.GetCommandArgs ();
			if (args.Count >= 3) {
				int clientCode = int.Parse (args [0]);
				string maxServer = args [1];
				bool enabled = bool.Parse (args [2]);
				SetClientMaxServerVersion (clientCode, maxServer, enabled, true);
			} else
				throw new Exception ("参数少于4个！");
		}

		public static void SetClientMaxServerVersion (int clientVersion, string serverVersion, bool enabled, bool save = false)
		{
			config.ModifyInfo (clientVersion, serverVersion, enabled);
			if (save) {
				config.SaveToFile (FilePath);
				AssetDatabase.Refresh ();
			}
		}
	}
}
 // namespace EditorTool