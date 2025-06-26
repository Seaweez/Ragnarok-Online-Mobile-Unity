using UnityEngine;
using System.Collections;
using UnityEditor;
using RO;
using System;
using Ghost.Utils;
using System.IO;
using EditorTool;
using SLua;

public class Excel2Lua : Editor
{
	//	[ MenuItem( "RO/Excel2Lua/CopyToClient" ) ]
	public static void CopyToClient ()
	{
		//		EditorUtils.info = "Copy start...";
		var path = Application.dataPath;
		var sourcePath = PathUnity.Combine (path, "../../../Cehua/Table/luas");
		var targetPath = PathUnity.Combine (path, "Resources/Script/Config");

		DirectoryInfo dirInfo = new DirectoryInfo (targetPath);
		foreach (FileInfo file in dirInfo.GetFiles()) {
			file.Delete (); 
		}

		var files = Directory.GetFiles (sourcePath);
		for (var i = 0; i < files.Length; i++) {
			var file = files [i];
			File.Copy (file, Path.Combine (targetPath, Path.GetFileName (file)), true);
			//			EditorUtils.progress = i / (files.Length - 1);
			//			EditorUtils.showProgress();
		}

		//		EditorUtils.info = "success";
		AssetDatabase.Refresh ();
	}

	//	[ MenuItem( "RO/Excel2Lua/CommitAllLuas(Will Force Add And Commit, Carefully Use!!!)" ) ]
	public static void CommitAllLuas ()
	{
		EditorTool.CommandHelper.ExcutePython ("../../Cehua/Table/Excel2Lua_Commit.py");
	}


	public static byte[] LoadLuaFile (string path)
	{
		return FileHelper.LoadFile (path);
	}

	static protected string s_sLuaRequire = @"
		    return function(path)
			    require(path)
		    end
		    ";
	static protected LuaFunction m_oLuaFunction;
	[MenuItem("RO/测试lua换行符")]
	public static void testLuaWrite()
	{
		LuaSvrForEditor.Me.Dispose();
		LuaSvrForEditor.Me.init();

		m_oLuaFunction = LuaSvrForEditor.Me.DoString(s_sLuaRequire) as LuaFunction;

		//加载优化Lua脚本
		m_oLuaFunction.call("Script/test");

		//执行Lua优化函数
		//		LuaSvrForEditor.Me.DoString("OptConfigFun('" + savePath + "','" + tableName + "','" + OptKey  + "')");
	}


	[ MenuItem ("RO/RebuildExcel(If Excel Changed, Do This)")]
	public static void RebuildLuas ()
	{
		EditorTool.CommandHelper.ExcutePython ("../../Cehua/Table/Excel2Lua.py");
		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh ();
		var valid = checkExcelValid();
		if(!valid)
		{
			return;
		}
		TableConfigMemoryToolWindow.OnlyOptConfig();
		refactoryHUCONG_SecondGen();
	}


	[ MenuItem ("RO/Commands/Update Protoc")]
	public static void updateProtoc ()
	{
		string path = CommandSetting.Instance.protobufCmdPath;
		if (string.IsNullOrEmpty (path)) {
			if (EditorUtility.DisplayDialog ("脚本地址为空", "生成protobuf的脚本地址未设置，前往设置", "GO!")) {
				CommandSetting.Open ();
			}
			return;
		}
		CommandSetting.Instance.CallGenProtobuf ();
		AssetDatabase.SaveAssets ();
	}

	[ MenuItem ("RO/CheckExcelValid")]
	public static bool checkExcelValid ()
	{
		string luasDir = Application.dataPath;
		int index = luasDir.IndexOf("client-refactory");
		string result = luasDir.Remove(index);

		luasDir = PathUnity.Combine(result,"Cehua/Table/lua_server");

		LuaSvrForEditor lf = LuaSvrForEditor.Me;
		lf.Dispose();
		lf.init();
		string[] files = Directory.GetFiles(luasDir);
		var finalResult = true;
		foreach (string fileName in files)
		{
			string excelName = Path.GetFileNameWithoutExtension(fileName);
			string name = excelName.Replace("Table_","");
			if(name == "Table") continue;
			string content = File.ReadAllText(fileName);
			object obj = null;
			try{
				obj = lf.DoString(content);
			}
			catch(Exception e)
			{
				Debug.LogError(e.ToString());
			}
			if(obj == null)
			{
				string promot = string.Format("{0}表配置错误",name);
				EditorUtility.DisplayDialog("错误！！！",promot,"ok");
				Debug.LogError(promot);
				finalResult = false;
			}
		}
		return finalResult;
	}

	[ MenuItem ("RO/Refactory HUCONG")]
	public static void refactoryHUCONG_SecondGen ()
	{
		LuaSvrForEditor lf = LuaSvrForEditor.Me;
		lf.Dispose();
		lf.init();
		string assetPath = Application.dataPath;
		int index = assetPath.IndexOf("client-refactory");
		string result = assetPath.Remove(index);

		string genFilesDir = PathUnity.Combine(result,"Cehua/Lua/SecondaryConfigureGenerator/");
		lf.DoString("CurrentPath = \""+genFilesDir+"\"");
		string cmdFilePath = PathUnity.Combine(genFilesDir,"ReferenceFilesName.txt");
		string content = File.ReadAllText(cmdFilePath);
		lf.DoString(content);

		cmdFilePath = PathUnity.Combine(genFilesDir,"AdventureValueConfGenerator.txt");
		content = File.ReadAllText(cmdFilePath);
		lf.DoString(content);

		cmdFilePath = PathUnity.Combine(genFilesDir,"ItemBeTransformedWayConfGenerator.txt");
		content = File.ReadAllText(cmdFilePath);
		lf.DoString(content);

		cmdFilePath = PathUnity.Combine(genFilesDir,"ItemOriginConfGenerator.txt");
		content = File.ReadAllText(cmdFilePath);
		lf.DoString(content);

		cmdFilePath = PathUnity.Combine(genFilesDir,"QuestRewardConfGenerator.txt");
		content = File.ReadAllText(cmdFilePath);
		lf.DoString(content);

		cmdFilePath = PathUnity.Combine(genFilesDir,"StageBagConfGenerator.txt");
		content = File.ReadAllText(cmdFilePath);
		lf.DoString(content);

		cmdFilePath = PathUnity.Combine(genFilesDir, "EquipComposeConfGenerator.txt");
		content = File.ReadAllText(cmdFilePath);
		lf.DoString(content);

		lf.Dispose();
	}

	public static void doCommonRequire()
	{

	}
}
