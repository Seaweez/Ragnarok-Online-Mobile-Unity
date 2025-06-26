using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public struct FolderInfo
{
	public string FolderName;
	public Object[] datas;
}

public class AssetManager
{
	public static Object[] getAllAssets (string path)
	{
		List<Object> result = new List<Object> ();
		DirectoryInfo pathDir = new DirectoryInfo (path);
		FileInfo [] goFileInfo = pathDir.GetFiles ();
		for (int i=0; i<goFileInfo.Length; i++) {
			if (goFileInfo [i] == null) 
				continue;
			string goFilePath = goFileInfo [i].FullName;
			int sIndex = Mathf.Max (goFilePath.IndexOf ("Assets"), 0);
			string goAssetPath = goFilePath.Substring (sIndex, goFilePath.Length - sIndex);
			
			Object o = AssetDatabase.LoadAssetAtPath (goAssetPath, typeof(Object));
			if (o == null)
				continue;
			
			result.Add (o);
		}
		
		DirectoryInfo[] Directorys = pathDir.GetDirectories ();
		foreach (DirectoryInfo Dir in Directorys) {
			result.AddRange (getAllAssets (path + "/" + Dir.Name));
		}
		
		return result.ToArray ();
	}

	public static List<FolderInfo> getAssetsByFolder (string path)
	{
		List<FolderInfo> results = new List<FolderInfo> ();

		DirectoryInfo dirs = new DirectoryInfo (path);
		DirectoryInfo[] childs = dirs.GetDirectories ();
//
		FolderInfo folderData;
//		folderData.FolderName = dirs.Name;
//		folderData.datas = getAllAssets (dirs.FullName);
//		results.Add (folderData);

//		FileInfo[] childs = dirs.GetFiles ();
//
//		for (int i = 0; i<dirs.; i++) {
//			//folderData.FolderName = childs [i].Name;
//			folderData.datas = getAllAssets (dirs.FullName);
//			results.Add (folderData);
//		}
		if(childs.Length!=0)
		{
			for (int i = 0; i<childs.Length; i++) {
				folderData.FolderName = childs [i].Name;
				folderData.datas = getAllAssets (dirs.FullName);
				results.Add (folderData);
			}
		}
		else
		{
			folderData.FolderName = dirs.Name;
			folderData.datas = getAllAssets (dirs.FullName);
			results.Add (folderData);
		}
		return results;
	}

	public static bool deletaAsset (Object data)
	{
		if (data == null)
			return false;

		string path = AssetDatabase.GetAssetPath (data);
		return AssetDatabase.DeleteAsset (path);
	}

	public static Object copyAsset (Object data)
	{
		if (data == null)
			return null;

		string path = AssetDatabase.GetAssetPath (data);
		string newpath = AssetDatabase.GenerateUniqueAssetPath (path);
		
		AssetDatabase.CopyAsset (path, newpath);
		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh ();

		return AssetDatabase.LoadAssetAtPath (newpath, typeof(Object));
	}

	public static string renameAsset (Object data, string newName)
	{
		string path = AssetDatabase.GetAssetPath (data);
		string rename = AssetDatabase.RenameAsset (path, newName);
		AssetDatabase.Refresh ();
		return rename;
	}
}
