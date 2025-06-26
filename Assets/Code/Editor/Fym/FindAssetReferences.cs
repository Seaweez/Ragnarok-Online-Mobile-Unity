using UnityEditor;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindAssetReferences {

	#if UNITY_EDITOR_OSX

	[MenuItem("RO/Fym/查找资源引用", false, 2000)]
	private static void FindProjectReferences()
	{
		string appDataPath = Application.dataPath;
		string output = "";
		string selectedAssetPath = AssetDatabase.GetAssetPath (Selection.activeObject);
		List<string> references = new List<string>();

		string guid = AssetDatabase.AssetPathToGUID (selectedAssetPath);

		var psi = new System.Diagnostics.ProcessStartInfo();
		psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;
		psi.FileName = "/usr/bin/mdfind";
		psi.Arguments = "-onlyin " + Application.dataPath + " " + guid;
		psi.UseShellExecute = false;
		psi.RedirectStandardOutput = true;
		psi.RedirectStandardError = true;

		System.Diagnostics.Process process = new System.Diagnostics.Process();
		process.StartInfo = psi;

		process.OutputDataReceived += (sender, e) => {
			if(string.IsNullOrEmpty(e.Data))
				return;

			string relativePath = "Assets" + e.Data.Replace(appDataPath, "");

			// skip the meta file of whatever we have selected
			if(relativePath == selectedAssetPath + ".meta")
				return;

			references.Add(relativePath);

		};
		process.ErrorDataReceived += (sender, e) => {
			if(string.IsNullOrEmpty(e.Data))
				return;

			output += "Error: " + e.Data + "\n";
		};
		process.Start();
		process.BeginOutputReadLine();
		process.BeginErrorReadLine();

		process.WaitForExit(2000);

		foreach(var file in references){
			output += file + "\n";
			Debug.Log(file, AssetDatabase.LoadMainAssetAtPath(file));
		}

		Debug.LogWarning(references.Count + " references found for object " + Selection.activeObject.name + "\n\n" + output);
	}

	#endif
}
//public class Ce : MonoBehaviour {
//	[MenuItem("Assets/Check Prefab Use ?")]  
//	private static void OnSearchForReferences()  
//	{  
//		//确保鼠标右键选择的是一个Prefab  
//		if(Selection.gameObjects.Length != 1)  
//		{  
//			return;  
//		}  
//
//		//遍历所有游戏场景  
//		foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes)  
//		{  
//			if(scene.enabled)  
//			{  
//				//打开场景  
//				EditorApplication.OpenScene(scene.path);  
//				//获取场景中的所有游戏对象  
//				GameObject []gos = (GameObject[])FindObjectsOfType(typeof(GameObject));  
//				foreach(GameObject go  in gos)  
//				{  
//					//判断GameObject是否为一个Prefab的引用  
//					if(PrefabUtility.GetPrefabType(go)  == PrefabType.PrefabInstance)  
//					{  
//						UnityEngine.Object parentObject = EditorUtility.GetPrefabParent(go);   
//						string path = AssetDatabase.GetAssetPath(parentObject);  
//						//判断GameObject的Prefab是否和右键选择的Prefab是同一路径。  
//						if(path == AssetDatabase.GetAssetPath(Selection.activeGameObject))  
//						{  
//							//输出场景名，以及Prefab引用的路径  
//							Debug.Log(scene.path  + "  " + GetGameObjectPath(go));  
//						}  
//					}  
//				}  
//			}  
//		}  
//	}  
//	public static string GetGameObjectPath(GameObject obj)  
//	{  
//		string path = "/" + obj.name;  
//		while (obj.transform.parent != null)  
//		{  
//			obj = obj.transform.parent.gameObject;  
//			path = "/" + obj.name + path;  
//		}  
//		return path;  
//	}  
//}
//