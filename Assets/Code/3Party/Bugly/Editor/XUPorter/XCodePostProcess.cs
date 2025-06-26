using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RO;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.XCodeEditor;
#endif
using System.IO;

public static class XCodePostProcess
{

	#if UNITY_EDITOR
	[PostProcessBuild (999)]
	public static void OnPostProcessBuild (BuildTarget target, string pathToBuiltProject)
	{
#if UNITY_5
		if (target != BuildTarget.iOS) {
#else
        	if (target != BuildTarget.iOS) {
#endif
			Debug.LogWarning ("Target is not iPhone. XCodePostProcess will not run");
			return;
		}

		//得到xcode工程的路径
		Path.GetFullPath (pathToBuiltProject);
		ROXCodePostManager xcodeManager = new ROXCodePostManager (pathToBuiltProject);
		xcodeManager.ProcessAll ();

#if UNITY_IOS
        if (target == BuildTarget.iOS)
        {
            string projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
            UnityEditor.iOS.Xcode.PBXProject pbxProject = new UnityEditor.iOS.Xcode.PBXProject();
            pbxProject.ReadFromFile(projectPath);
            string customtarget = pbxProject.GetUnityMainTargetGuid();
            pbxProject.SetBuildProperty(customtarget, "ENABLE_BITCODE", "NO");
            pbxProject.WriteToFile(projectPath);
        }
#endif
	}

	private static void CopyDirectory (string sourcePath, string destPath)
	{
		if (!Directory.Exists (destPath)) {
			Directory.CreateDirectory (destPath);
		}
			
		foreach (string file in Directory.GetFiles(sourcePath)) {
			string dest = Path.Combine (destPath, Path.GetFileName (file));
			File.Copy (file, dest);
		}
			
		foreach (string folder in Directory.GetDirectories(sourcePath)) {
			string dest = Path.Combine (destPath, Path.GetFileName (folder));
			CopyDirectory (folder, dest);
		}
	}
	#endif
		
	public static void Log (string message)
	{
		UnityEngine.Debug.Log ("PostProcess: " + message);
	}
}
