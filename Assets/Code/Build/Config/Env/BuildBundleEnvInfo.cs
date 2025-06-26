using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using Ghost.Utils;

namespace RO.Config
{
	[SLua.CustomLuaClassAttribute]
	public static class BuildBundleEnvInfo
	{
		public static string Env {
			get {
				//alvin
				return "Alpha";
//				#if ENV_DEV
//					return "Dev";
//				#elif ENV_ALPHA
//					return "Alpha";
//				#elif ENV_STUDIO
//					return "Studio";
//				#else
//				return "";
//				#endif
			}
		}

		public static void SetEnv (string env, string sdk = "None")
		{
			string path = Path.Combine (Application.dataPath, "Resources/ChannelEnv.xml");
			AppEnvConfig config = AppEnvConfig.CreateByFile (path);
			if (config == null) {
				config = new AppEnvConfig ();
			}
			if (config.channelEnv != env || config.sdk != sdk) {
				config.channelEnv = env;
				config.sdk = sdk;
				config.SaveToFile (path);
			}
		}

		public static string CdnWithEnv {
			get {
				string url = "http://api.pinidea.online/res/";
				url = PathUnity.Combine (url, Env);
				url = PathUnity.Combine (url, ApplicationHelper.platformFolder + "/");
				return url;
			}
		}
	}
} // namespace RO.Config
