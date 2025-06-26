using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using EditorTool;

public static class AndroidAutoBuilder
{
	static string GetProjectName ()
	{
		string[] s = Application.dataPath.Split ('/');
		return s [s.Length - 2];
	}

	static string[] GetScenePaths ()
	{
		string[] scenes = new string[EditorBuildSettings.scenes.Length];


		for (int i = 0; i < scenes.Length; i++) {
			scenes [i] = EditorBuildSettings.scenes [i].path;
		}

		return scenes;
	}
//	[MenuItem("RO/Test",false,0)]
	static void PerformAndroidBuild ()
	{
		PrepareBuild ();
		BuildToPath ("ROAndroid");
	}

	public static void PerformAndroidBuildBundleMode ()
	{
		PrepareBuild ();
		BuildToPath ("ROAndroid", false);
	}

	static void PerformiOSBuildSpecify ()
	{
		PrepareBuild ();
		BuildToPath ("VersionROAndroid");
	}

	static void PrepareBuild ()
	{
		PlayerSettings.keystorePass = "KcT1874";

		bool iconWithVersion = true;
		//新版本资源打入安装包
		#if RESOURCE_LOAD
		#else
		List<string> args = CommandArgs.GetCommandArgs ();
		if (args.Count >= 1) {
			bool.TryParse (args [0], out iconWithVersion);
		}
		if (args.Count >= 2) {
			int start = int.Parse (args [1]);
			string serverVersion = "1.0";
			if (args.Count >= 3) {
				serverVersion = args [2];
			}
			BuildBundleEditor.BuildApp (true, start, serverVersion);
		}
		ArchiveUtil.ZipBundlesAndMoveToStream ();
		#endif
		//成path2name.xml
		//生成app icon
		Path2NameEditor.CreateDataAsset ();
		string assetPath = Application.dataPath;
		string shPath = null;
		if (iconWithVersion) {
			shPath = Path.Combine (assetPath, "Art/Public/Texture/General/TestVersion/AutoCreateVer.sh");
		} else {
			shPath = Path.Combine (assetPath, "Art/Public/Texture/General/TestVersion/JustCopyIcon.sh");
		}
		EditorTool.CommandHelper.ExcuteExternalCommandNoLog ("sh", shPath + " " + assetPath);
	
		shPath = Path.Combine (assetPath, "Art/Public/Texture/General/AutoCreateRoundrectang.sh");
		EditorTool.CommandHelper.ExcuteExternalCommandNoLog ("sh", shPath + " " + assetPath);
		AssetDatabase.Refresh ();
	}

	static void BuildToPath (string path, bool resourceMode = true)
	{
		var scenes = new List<string> ();
		foreach (var scene in EditorBuildSettings.scenes) {
			if (scene.enabled) {
				scenes.Add (scene.path);
			}
		}
		
//		EditorUserBuildSettings.SwitchActiveBuildTarget (BuildTarget.iOS);
//		PlayerSettings.apiCompatibilityLevel = ApiCompatibilityLevel.NET_2_0_Subset;
//		PlayerSettings.iOS.scriptCallOptimization = ScriptCallOptimizationLevel.SlowAndSafe;
		string Define = ScriptDefines.Add ("DEBUG_DRAW");
		Define = ScriptDefines.Add ("DEBUG_DRAW;LUA_5_3");
		if (resourceMode)
			Define = ScriptDefines.Add ("RESOURCE_LOAD");
		else
			Define = ScriptDefines.Remove ("RESOURCE_LOAD");
		PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.Android, Define);
		SetBundleVersion (1,0);
//		PlayerSettings.Android.keystoreName = "Test";
		PlayerSettings.Android.keystorePass = "111111";
//		PlayerSettings.Android.keyaliasName = "";
		PlayerSettings.Android.keyaliasPass = "111111";
		PlayerSettings.Android.bundleVersionCode = PlayerSettings.Android.bundleVersionCode + 1;
		BuildPipeline.BuildPlayer (scenes.ToArray (), "../../" + path, BuildTarget.Android, BuildOptions.None); // or None to create new one
	}

	[MenuItem("RO/Test安卓版本号",false,0)]
	static void TestAndroidVersionCode ()
	{
		SetBundleVersion (1,0);
	}

	static void SetBundleVersion (int bigVersion,int secondVersion)
	{
		int newBig = bigVersion;
		int newSecond = 1;
		string old = PlayerSettings.bundleVersion;
		string[] splits = old.Split ('.');
		if (splits.Length > 0) {
			int oldBigVersion = CheckAndGetVersion(splits,0);
			if (bigVersion == oldBigVersion) {
				int oldSecondVersion = CheckAndGetVersion(splits,1);
				if (secondVersion == oldSecondVersion) {
					int oldLastVersion = CheckAndGetVersion(splits,2);
					if(oldLastVersion>0)
					{
						newSecond = oldLastVersion + 1;
					}
				}
			}
		} 
		PlayerSettings.bundleVersion = newBig.ToString () + "." + secondVersion.ToString() + "." + newSecond.ToString();
	}

	static int CheckAndGetVersion(string[] versionCode,int index)
	{
		if (versionCode.Length > index) {
			int version = 0;
			if (int.TryParse (versionCode [index], out version) == false) {
				return -1;
			}
			return version;
		}
		return -1;
	}
}
