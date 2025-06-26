using UnityEngine;
using System.Collections;
using RO;
using System.Collections.Generic;
using System.IO;

namespace UnityEditor.XCodeEditor
{
	public class ROOptionalXCodePost : IROXCodePostProcess
	{
		XCProject _project;

		public ROOptionalXCodePost (XCProject p)
		{
			_project = p;
		}

		#region IROXCodePostProcess implementation

		public void Process ()
		{
			_HandleConfigSDK ();
			_HandleJPush ();
		}

		public void Release ()
		{
			_project = null;
		}

		#endregion

		void _HandleConfigSDK ()
		{
			string sdk = "None";
			if (AppEnvConfig.Instance != null) {
				sdk = AppEnvConfig.Instance.sdk;
			}
			List<string> files = FilterModsBySDK (sdk);
			if (files != null) {
				foreach (string file in files) {
					UnityEngine.Debug.Log ("ProjMod File: " + file);
					_project.ApplyMod (file);
				}
			}
		}

		void _HandleJPush ()
		{
			bool needJPush = false;
			BuildParams param = BuildParams.Instance;
			if (param != null) {
				needJPush = param.Get_JPush_Enable;
			}
			ModifyAppControllerClass pushClass = new ModifyAppControllerClass (_project.projectRootPath + "/Classes/UnityAppController.h",
				                                     _project.projectRootPath + "/Classes/UnityAppController.mm");

			ModifyAppControllerClass Preprocessor = new ModifyAppControllerClass (_project.projectRootPath + "/Classes/Preprocessor.h",
				_project.projectRootPath + "/Classes/Preprocessor.mm");

			Preprocessor.OpenPush ();

			if (needJPush) {
				XCodeCapabilities capability = new XCodeCapabilities (_project);
				capability.AddPushNotifications ();
				capability.AddBackgroundModes (BackgroundModesOptions.RemoteNotifications);
				capability.Release ();

				pushClass.MergeJPush (param.Get_JPush_Appkey, param.Get_JPush_IsRelease);

				_project.overwriteBuildSetting ("CODE_SIGN_ENTITLEMENTS", "Unity-iPhone/ro_prod.entitlements", "Release");
				_project.overwriteBuildSetting ("CODE_SIGN_ENTITLEMENTS", "Unity-iPhone/ro_dev.entitlements", "Debug");

				string devEntitlementsPath = Path.Combine (_project.projectRootPath, "Unity-iPhone/ro_dev.entitlements");
				ROEntitlements plist = new ROEntitlements (devEntitlementsPath);
				plist.CreateFile ();
				Hashtable hash = new Hashtable ();
				hash.Add ("aps-environment", "development");
				plist.Process (hash);
				_project.AddFile (devEntitlementsPath);

				string proEntitlementsPath = Path.Combine (_project.projectRootPath, "Unity-iPhone/ro_prod.entitlements");
				plist = new ROEntitlements (proEntitlementsPath);
				plist.CreateFile ();
				hash = new Hashtable ();
				hash.Add ("aps-environment", "production");
				plist.Process (hash);
				_project.AddFile (proEntitlementsPath);
			} else {
				pushClass.MergeWithoutJPush ();
			}
		}

		public static List<string> FilterModsBySDK (string sdkConfigFileName)
		{
			string[] sdkConfigs = Directory.GetFiles (Application.dataPath, "*.sdkmods", SearchOption.AllDirectories);
			if (sdkConfigs != null) {
				foreach (string sdkFileName in sdkConfigs) {
					if (Path.GetFileNameWithoutExtension (sdkFileName) == sdkConfigFileName) {
						sdkConfigFileName = sdkFileName;
						break;
					}
				}
			}
			FileInfo projectFileInfo = new FileInfo (sdkConfigFileName);
			if (!projectFileInfo.Exists) {
				Debug.LogWarning ("File does not exist.");
				return null;
			}
			string contents = projectFileInfo.OpenText ().ReadToEnd ();
			Debug.Log (contents);
			Hashtable mods = (Hashtable)XUPorterJSON.MiniJSON.jsonDecode (contents);
			ArrayList modsNames = (ArrayList)mods ["mods"];
			string rootpath = Application.dataPath.Replace ("Assets", "");
			// find all .projmods from root of project
			string[] files = Directory.GetFiles (rootpath, "*.projmods", SearchOption.AllDirectories);
			if (modsNames != null) {
				List<string> listmods = new List<string> ();
				string fileName = "";
				foreach (string file in files) {
					fileName = Path.GetFileNameWithoutExtension (file);
					if (modsNames.Contains (fileName)) {
						listmods.Add (file);
					}
				}
				return listmods;
			}
			return null;
		}
	}


}