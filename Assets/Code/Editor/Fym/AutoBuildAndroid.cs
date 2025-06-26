using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
public class AutoBuildAndroid : MonoBehaviour {
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
	[MenuItem("RO/Fym/安卓一键导出打包/打包",false,0)]
	static void PerformiOSBuild ()
	{
		string[] scenes = {"Assets/DevelopTest/TestModel/TestModel.unity"};
		#if UNITY_5_0 
			EditorUserBuildSettings.SwitchActiveBuildTarget (BuildTarget.Android);
			PlayerSettings.apiCompatibilityLevel = ApiCompatibilityLevel.NET_2_0_Subset;
			PlayerSettings.iOS.scriptCallOptimization = ScriptCallOptimizationLevel.SlowAndSafe;
			PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.Android, "DEBUG_DRAW;RESOURCE_LOAD");
			BuildPipeline.BuildPlayer (scenes, "RO.apk",BuildTarget.Android, BuildOptions.None); // or None to create new one
		#elif UNITY_2017
			EditorUserBuildSettings.SwitchActiveBuildTarget (BuildTargetGroup.Android,BuildTarget.Android);
			PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android,ApiCompatibilityLevel.NET_2_0_Subset) ;//= ApiCompatibilityLevel.NET_2_0_Subset;
			PlayerSettings.iOS.scriptCallOptimization = ScriptCallOptimizationLevel.SlowAndSafe;
			PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.iOS, "DEBUG_DRAW;RESOURCE_LOAD");
			BuildPipeline.BuildPlayer (scenes, "../../../RO_ios",
			BuildTarget.iOS, BuildOptions.AcceptExternalModificationsToPlayer); // or None to create new one
		#else
			EditorUserBuildSettings.SwitchActiveBuildTarget (BuildTarget.iOS);
			PlayerSettings.apiCompatibilityLevel = ApiCompatibilityLevel.NET_2_0_Subset;
			PlayerSettings.iOS.scriptCallOptimization = ScriptCallOptimizationLevel.SlowAndSafe;
			PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.iOS, "DEBUG_DRAW;RESOURCE_LOAD");
			BuildPipeline.BuildPlayer (scenes, "../../../RO_ios",
			BuildTarget.iOS, BuildOptions.AcceptExternalModificationsToPlayer); // or None to create new one
		#endif
	}
}

