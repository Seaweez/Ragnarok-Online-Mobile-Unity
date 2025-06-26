using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
public class FindMissingScripts : EditorWindow 
{
	static int go_count = 0, components_count = 0, missing_count = 0;


	Object checkFolder = null;
	string[] directoryEntries;  
	//GameObject[] a=null;


	[MenuItem("RO/Fym/检查丢失的脚本",false,0)]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(FindMissingScripts));
	}

	public void OnGUI()
	{

		EditorGUILayout.BeginVertical();
		checkFolder = EditorGUILayout.ObjectField("Check Folder", checkFolder, typeof(Object), false);


		if (GUILayout.Button("查找丢失文件"))
		{
			if(checkFolder!=null)
			{
				FindInSelected();
			}
			else
			{
				Debug.Log("目录为空，请设置目录.");
			}
		}




		EditorGUILayout.EndVertical();
	}
	void FindInSelected()
	{
		go_count = 0;
		components_count = 0;
		missing_count = 0;

		var path = AssetDatabase.GetAssetPath(checkFolder);
		var guids = AssetDatabase.FindAssets("t:GameObject", new string[]{path});
	//	a = new GameObject[guids.Length];
		foreach (var guid in guids)
		{
			var obj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(GameObject)) as GameObject;
			FindInGO(obj);
		}

		Debug.Log(string.Format("搜索了 {0} 对象, {1} 组件, 发现 {2} 丢失", go_count, components_count, missing_count));
	}


	private static void FindInGO(GameObject g)
	{
		go_count++;
		Component[] components = g.GetComponents<Component>();
		for (int i = 0; i < components.Length; i++)
		{
			components_count++;
			if (components[i] == null)
			{
				missing_count++;
				string s = g.name;
				Transform t = g.transform;
				while (t.parent != null) 
				{
					s = t.parent.name +"/"+s;
					t = t.parent;
				}
				Object.DestroyImmediate(components[i]);
				Debug.Log (s + " has an empty script attached in position: " + i, g);
			}
		}
		// Now recurse through each child GO (if there are any):
		foreach (Transform childT in g.transform)
		{
			//Debug.Log("Searching " + childT.name  + " " );
			FindInGO(childT.gameObject);
		}
	}




















	










}