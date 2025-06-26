using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
public class ModelCheck : EditorWindow {
	
	float width;
	float height;
	float zaxis;
	float lineSpacing;
	float columnSpacing;
	
	
	
	int i_alllinenumb=0;
	int i_linenumb=0;
	int i_test =0;
	
	
	
	
	
	
	Object checkFolder = null;
	string[] directoryEntries;  
	GameObject[] a=null;
	
	ModelCheckGizmos model;
	
	
	
	
	[MenuItem("RO/Fym/ModeleCheck",false,0)]
	static void ShowWindow ()
	{       
		EditorWindow.GetWindow<ModelCheck>(false, "ModelCheckTool", true);	
	}
	
	void OnGUI() 
	{
		EditorGUILayout.BeginVertical();
		checkFolder = EditorGUILayout.ObjectField("Check Folder", checkFolder, typeof(Object), false);
		
		SetHW();
		if (GUILayout.Button("Run"))
		{//OnDrawGizmosSelected();
			if(a!=null)
			{
				for(int i = 0 ; i<a.Length;i++)
				{
					DestroyImmediate(a[i]);
				}
			}
			if(checkFolder!=null)
			{
				
				GetFilerInstant();
			}else
			{
				Debug.Log("目录为空，请设置目录.");
			}
			i_test=0;
			i_linenumb = 0;
		}
		if (GUILayout.Button("Delete"))
		{
			if(checkFolder!=null)
			{
				if(a!=null)
				{
					for(int i = 0 ; i<a.Length;i++)
					{
						DestroyImmediate(a[i]);
					}
				}
			}
		}
		EditorGUILayout.EndVertical();
	}
	//获得文件并实例化
	void GetFilerInstant()
	{
		var path = AssetDatabase.GetAssetPath(checkFolder);
		
		var guids = AssetDatabase.FindAssets("t:GameObject", new string[]{path});
		float w =lineSpacing;
		float h =columnSpacing;
		
		
		a = new GameObject[guids.Length];
		foreach (var guid in guids)
		{
			
			if(i_linenumb<i_alllinenumb)
			{
				var obj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(GameObject)) as GameObject;
				a[i_test] = Instantiate(obj, new Vector3(w, h, 0), Quaternion.identity) as GameObject;
				a[i_test].AddComponent<ModelCheckGizmos>().SetGizmos(width,height,zaxis);
				i_test++;
				i_linenumb++;
				w+=lineSpacing;
			}
			else
			{
				w=lineSpacing;
				h+=columnSpacing;
				i_linenumb = 0;
			}
		}
		//var path = AssetDatabase.GetAssetPath(checkFolder);
		//DirectoryInfo direction = new DirectoryInfo(path);
		//FileInfo[] files = direction.GetFiles("*",SearchOption.AllDirectories);  
		//		for(int i=0;i<files.Length;i++)
		//		{  
		//			if (files[i].Name.EndsWith(".prefab"))
		//			{  
		//				GameObject a =(GameObject)files[i].;
		//
		//				// Instantiate(prefab, new Vector3(i * 2.0f, 0, 0), Quaternion.identity);
		//				Debug.Log(directoryEntries[i]); 
		//			}  
		//		}
		
		//		var path = AssetDatabase.GetAssetPath(checkFolder);
		//		directoryEntries = Directory.GetFileSystemEntries(path);
		//		for(int i = 0; i < directoryEntries.Length ; i ++)
		//		{
		//			if(directoryEntries[i].EndsWith(".prefab"))
		//			{
		//				string p = directoryEntries[i];  
		//				Debug.Log(directoryEntries[i]);
		//			}
		//		}
		
	}
	//设置盒子宽高
	void SetHW()
	{
		if(width>-1)
		{
			width = EditorGUILayout.FloatField("包围盒宽度:", width);
		}
		else
		{
			width=1;
			width = EditorGUILayout.FloatField("包围盒宽度:", width);
		}
		if(height>-1)
		{
			height = EditorGUILayout.FloatField("包围盒高度:", height);
		}
		else
		{
			height=1;
			height = EditorGUILayout.FloatField("包围盒高度", height);
		}
		if(zaxis>-1)
		{
			zaxis = EditorGUILayout.FloatField("包围盒深度:", zaxis);
		}
		else
		{
			zaxis=0;
			zaxis = EditorGUILayout.FloatField("包围盒深度", zaxis);
		}
		if(lineSpacing>-1)
		{
			lineSpacing = EditorGUILayout.FloatField("行距:", lineSpacing);
		}
		else
		{
			lineSpacing=0;
			lineSpacing= EditorGUILayout.FloatField("行距:", lineSpacing);
		}
		if(columnSpacing>0)
		{
			columnSpacing= EditorGUILayout.FloatField("列距:", columnSpacing);
		}
		else
		{
			columnSpacing=1;
			columnSpacing = EditorGUILayout.FloatField("列距:", columnSpacing);
		}
		
		
		
		
		if(i_alllinenumb>0)
		{
			i_alllinenumb= EditorGUILayout.IntField("一行多少个:", i_alllinenumb);
		}
		else
		{
			i_alllinenumb=1;
			i_alllinenumb = EditorGUILayout.IntField("一行多少个:", i_alllinenumb);
		}
	}
}
