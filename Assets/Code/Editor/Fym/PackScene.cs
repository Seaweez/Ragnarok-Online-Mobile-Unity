using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;
public class PackScene
{
		/// <summary>
		/// 定义场景产生预设的路径
		/// </summary>
		/// 
		static string path1 = "/Prefabs/";
	
	[MenuItem("RO/Fym/创建无关联的预设",false,0)]
	static void Excute()
	{
		var path2 = Path.GetDirectoryName(EditorSceneManager.GetActiveScene().path);
		string prefabsPath = (path2+path1);
		if (!Directory.Exists(prefabsPath))
		{
			Directory.CreateDirectory(prefabsPath);
		}
		//循环遍历产生预设。
		GameObject [] obj_Prefab = new GameObject[Selection.objects.Length];
		for (int i = 0, max = obj_Prefab.Length; i < max; i++)
		{
			obj_Prefab[i] = (GameObject)Selection.objects[i];
			PrefabUtility.CreatePrefab(prefabsPath+obj_Prefab[i].name+".prefab",obj_Prefab[i],ReplacePrefabOptions.ReplaceNameBased);
		}
	}
}
