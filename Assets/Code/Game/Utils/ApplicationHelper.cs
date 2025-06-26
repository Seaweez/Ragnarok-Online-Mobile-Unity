using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class ApplicationHelper
	{
		public static string platformFolder {
			get {
				#if UNITY_EDITOR
				return GetPlatformFolderForAssetBundles (EditorUserBuildSettings.activeBuildTarget);
				#else
				return GetPlatformFolderForAssetBundles (Application.platform);
				#endif
			}
		}

		public static string persistentDataPath {
			get {
				return Application.persistentDataPath;
			}
		}
	
		#if UNITY_EDITOR
		[SLua.DoNotToLua]
		public static string GetPlatformFolderForAssetBundles (BuildTarget target)
		{
			switch (target) {
			case BuildTarget.Android:
				return "Android";
			case BuildTarget.iOS:
				return "iOS";
			case BuildTarget.StandaloneWindows:
			case BuildTarget.StandaloneWindows64:
				return "Windows";
			case BuildTarget.StandaloneOSX:
				return "OSX";
			// Add more build targets for your own.
			// If you add more targets, don't forget to add the same platforms to GetPlatformFolderForAssetBundles(RuntimePlatform) function.
			default:
				return null;
			}
		}
		#endif
		
		static string GetPlatformFolderForAssetBundles (RuntimePlatform platform)
		{
			switch (platform) {
			case RuntimePlatform.Android:
				return "Android";
			case RuntimePlatform.IPhonePlayer:
				return "iOS";
			case RuntimePlatform.WindowsPlayer:
				return "Windows";
			case RuntimePlatform.OSXPlayer:
				return "OSX";
			// Add more build platform for your own.
			// If you add more platforms, don't forget to add the same targets to GetPlatformFolderForAssetBundles(BuildTarget) function.
			default:
				return null;
			}
		}

		public static bool AssetBundleLoadMode {
			get {
				#if RESOURCE_LOAD
				return false;
				#else
				return true;
				#endif
			}
		}

		public static string ParseToUTF8URL (string url)
		{
			string result = "";  
//			Regex rx = new Regex ("^[/u4e00-/u9fa5]$");//中文字符unicode范围  
			for (int i = 0; i < url.Length; i++) {  
				if (url [i] >= 0x4e00 && url [i] <= 0x9fa5) {
					result += WWW.EscapeURL (url [i].ToString (), System.Text.Encoding.UTF8);
				} else
					result += url [i].ToString ();
			}  
			return result;
		}

		public static bool lowmemory_registed = false;

		public static void RegisterLowMemoryCall ()
		{
			if (lowmemory_registed) {
				return;
			}
			lowmemory_registed = true;
			Application.lowMemory += OnLowMemoryCall;
		}

		private static void OnLowMemoryCall ()
		{
			MyLuaSrv.Instance.LuaManualGC ();
			System.GC.Collect ();
			ResourceManager.Instance.GC ();
		}
	}
}
 // namespace RO
