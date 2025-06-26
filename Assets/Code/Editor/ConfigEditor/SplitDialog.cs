using System;
using UnityEditor;
using UnityEngine;
using EditorTool;
using SLua;
using System.IO;

namespace EditorTool
{

	public class SplitDialog : Singleton<SplitDialog>
	{
		static protected string s_sLuaRequire = @"
		    return function(path)
			    require(path)
		    end
		    ";

		protected LuaFunction m_oLuaFunction;

		public string Quest_Config_Path = Application.dataPath +  "/../../../Cehua/Table/lua_server/";
		static string SplitDialog_Lua_Path = Application.dataPath + "/../../../client-export/Config_DialogSplit.lua";


		public static byte[] LoadLuaFile (string path)
		{	
			return FileHelper.LoadFile (path);
		}

		[ContextMenu ("TestSplitDialog")]
		public void OptFile ()
		{
			LuaState luaState = new LuaState ();
            luaState.loaderDelegate = LoadLuaFile;

			string quest_filenames = "";

			DirectoryInfo pathDir = new DirectoryInfo (Quest_Config_Path);
			FileInfo [] goFileInfo = pathDir.GetFiles ();
			for (int i=0; i<goFileInfo.Length; i++) {
				if (goFileInfo [i] == null) 
					continue;
				string fileName = goFileInfo [i].Name;

				if (fileName.StartsWith ("Table_Quest_") || fileName == "Table_Quest.txt") {
					fileName = fileName.Substring(0, fileName.Length-4);
					quest_filenames = quest_filenames + "'" + fileName + "', ";
				}
			}

			string luastr_quest_filenames = string.Format (
				"runtimePlatform = '{0}';\nquest_filenames = {1}{2}{3};", 
				Application.platform.ToString (),
				"{", 
				quest_filenames,
				"}");

			Debug.Log(luastr_quest_filenames);
			luaState.doString (luastr_quest_filenames);

			luaState.doFile (SplitDialog_Lua_Path);

			luaState.Dispose();
			luaState = null;

			AssetDatabase.Refresh ();
		}
	}
}

