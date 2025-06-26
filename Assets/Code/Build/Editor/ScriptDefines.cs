using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace EditorTool
{
	public static class ScriptDefines
	{
		static BuildTargetGroup grp {
			get {
				switch (EditorUserBuildSettings.activeBuildTarget) {
				case BuildTarget.iOS:
					return BuildTargetGroup.iOS;
				case BuildTarget.Android:
					return BuildTargetGroup.Android;
				default:
					return BuildTargetGroup.iOS;
				}
			}
		}

		public static bool Contains (string define, BuildTargetGroup group = BuildTargetGroup.Unknown)
		{
			group = group == BuildTargetGroup.Unknown ? grp : group;
			string currentDefine = PlayerSettings.GetScriptingDefineSymbolsForGroup (group);
			return currentDefine.Contains (define);
		}

		/// <summary>
		/// 给buildsettings里的script define symbol增加一个define
		/// </summary>
		public static string Add (string define, BuildTargetGroup group = BuildTargetGroup.Unknown)
		{
			group = group == BuildTargetGroup.Unknown ? grp : group;
			string currentDefine = PlayerSettings.GetScriptingDefineSymbolsForGroup (group);
			if (currentDefine.Contains (define) == false)
				currentDefine += ";" + define;
			return currentDefine;
		}

		/// <summary>
		/// 给buildsettings里的script define symbol移除一个define
		/// </summary>
		public static string Remove (string define, BuildTargetGroup group = BuildTargetGroup.Unknown)
		{
			group = group == BuildTargetGroup.Unknown ? grp : group;
			string currentDefine = PlayerSettings.GetScriptingDefineSymbolsForGroup (group);
			if (currentDefine.Contains (define))
				currentDefine = currentDefine.Replace (define, "");
			return currentDefine;
		}

		/// <summary>
		/// 给buildsettings里的script define symbol替换一个define
		/// </summary>
		public static string Replace (string find, string newReplace, BuildTargetGroup group = BuildTargetGroup.Unknown)
		{
			group = group == BuildTargetGroup.Unknown ? grp : group;
			string currentDefine = PlayerSettings.GetScriptingDefineSymbolsForGroup (group);
			if (currentDefine.Contains (find))
				currentDefine = currentDefine.Replace (find, newReplace);
			return currentDefine;
		}

		/// <summary>
		/// 给buildsettings里的script define symbol通过部分字符串替换一个define
		/// </summary>
		public static string ReplaceByPart (string findPart, string newReplace, BuildTargetGroup group = BuildTargetGroup.Unknown)
		{
			group = group == BuildTargetGroup.Unknown ? grp : group;
			string currentDefine = PlayerSettings.GetScriptingDefineSymbolsForGroup (group);
			string[] defines = currentDefine.Split (';');
			string define = "";
			bool found = false;
			for (int i=0; i<defines.Length; i++) {
				define = defines [i];
				if (define.Contains (findPart)) {
					found = true;
					currentDefine = currentDefine.Replace (define, newReplace);
				}
			}
			if (!found)
				return Add (newReplace, group);
			return currentDefine;
		}

		public static void ExcuteMethodSetEnv ()
		{
			List<string> args = CommandArgs.GetCommandArgs ();
			if (args.Count > 0) {
				string sdk = "None";
				bool skipCheckVersion = false;
				if (args.Count > 1) {
					sdk = args [1];
					if (args.Count > 2)
						bool.TryParse(args [2],out skipCheckVersion);
				}

				RO.Config.BuildBundleEnvInfo.SetEnv (args [0], sdk);
				string Define = ScriptDefines.ReplaceByPart ("_LINK_NATIVE", string.Format ("_{0}_LINK_NATIVE_", sdk));
				PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.iOS, Define);
				if(skipCheckVersion)
					Define = ScriptDefines.Add("TestApp");
				else
					Define = ScriptDefines.Remove("TestApp");
				PlayerSettings.SetScriptingDefineSymbolsForGroup (grp, Define);
			}
		}

		public static void ExcuteMethodSetInternalEnv ()
		{
			List<string> args = CommandArgs.GetCommandArgs ();
			if (args.Count > 0) {
				ScriptDefines.Add(args[0],BuildTargetGroup.iOS);
			}
		}


		public static void ExcuteMethodSetFastEnv ()
		{
			string Define = ScriptDefines.Add("LUA_FASTPACKING");
		
			PlayerSettings.SetScriptingDefineSymbolsForGroup (grp, Define);
		}

		public static void SetNewZipEnv ()
		{
			List<string> args = CommandArgs.GetCommandArgs ();
			bool newZipEnv = false;
			if (args.Count > 0) {
				bool.TryParse(args[0],out newZipEnv);
			}
			string define;
			if(newZipEnv)
			{
				define = ScriptDefines.Add("ARCHIVE_AB");
			}else
			{
				define = ScriptDefines.Remove("ARCHIVE_AB");
			}
			PlayerSettings.SetScriptingDefineSymbolsForGroup (grp, define);
		}

		public static void SwitchToDev ()
		{
			PlayerSettings.SetScriptingDefineSymbolsForGroup (grp, ReplaceByPart ("ENV_", "ENV_DEV"));
		}

		public static void SwitchToAlpha ()
		{
			PlayerSettings.SetScriptingDefineSymbolsForGroup (grp, ReplaceByPart ("ENV_", "ENV_ALPHA"));
		}

		public static void SwitchToStudio ()
		{
			PlayerSettings.SetScriptingDefineSymbolsForGroup (grp, ReplaceByPart ("ENV_", "ENV_STUDIO"));
		}
	}
} // namespace EditorTool
