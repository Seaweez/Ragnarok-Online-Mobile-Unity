using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using EditorTool;
using System;

public static class IOSAutoBuilder
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

	[MenuItem ("RO/TestShell", false, 0)]
	static void TestShell ()
	{
		string shPath = null;
		string assetPath = Application.dataPath;
		shPath = Path.Combine (assetPath, "Art/Public/Texture/General/TestVersion/JustCopyIcon.sh");
		EditorTool.CommandHelper.ExcuteExternalCommandNoLog ("sh", shPath + " " + assetPath);
		AssetDatabase.Refresh ();
	}

	//	[MenuItem("RO/Test",false,0)]
	static void PerformiOSBuild ()
	{
		PrepareBuild ();
		BuildToPath ("ROIOS");
	}

	public static void PerformiOSBuildBundleMode ()
	{
		PrepareBuild ();
		BuildToPath ("ROIOS", false);
	}

	static void PerformiOSBuildSpecify ()
	{
		PrepareBuild ();
		BuildToPath ("VersionROIOS");
	}

	static void PrepareBuild ()
	{
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
		string Define = ScriptDefines.Remove ("DEBUG_DRAW");
		Define = ScriptDefines.Add ("DEBUG_DRAW;LUA_5_3");
		Define = ScriptDefines.Remove ("DEBUG_DRAW");
		if (resourceMode)
			Define = ScriptDefines.Add ("RESOURCE_LOAD");
		PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.iOS, Define);
		PlayerSettings.iOS.buildNumber = (Convert.ToInt32 (PlayerSettings.iOS.buildNumber) + 1).ToString ();
		BuildPipeline.BuildPlayer (scenes.ToArray (), GetXcodeProjFolder (path), BuildTarget.iOS, BuildOptions.None); // or None to create new one
		EditorUserBuildSettings.development = false;
		EditorUserBuildSettings.connectProfiler = false;
	}

	[MenuItem("AssetBundle/Test Only BuildXcode")]
	static void OnlyBuildXcode ()
	{
		BuildToPath ("ROIOS", false);
	}

	[MenuItem("AssetBundle/Test BuildXcode")]
	static void TestOnlyBuildXcode ()
	{
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
		AssetDatabase.Refresh ();
		BuildToPath ("ROIOS", false);
	}

	static string GetXcodeProjFolder (string path)
	{
		string unityProjPath = Application.dataPath;
		List<string> pathSperate = new List<string> (unityProjPath.Split (Path.DirectorySeparatorChar));
		pathSperate.Remove ("Assets");
		if (pathSperate.Count > 2) {
			pathSperate.RemoveAt (pathSperate.Count - 1);
			pathSperate.RemoveAt (pathSperate.Count - 1);
		}
		string folder = "/";
		for (int i = 0; i < pathSperate.Count; i++) {
			folder = Path.Combine (folder, pathSperate [i]);
		}
		folder = Path.Combine (folder, path);
		Debug.Log ("XcodeProj: " + folder);
		return folder;
	}
}
