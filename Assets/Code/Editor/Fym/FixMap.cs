using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class FixMap : EditorWindow {

	// Use this for initialization

	[MenuItem("RO/Fym/修复地图模糊(使用后必须使用SVN-Revert)",false,0)]
	static void FixMapTexture ()
	{
		EditorWindow.GetWindow<FixMap>(false, "FixMapTool", true);	
	}

	void OnGUI() 
	{
		EditorGUILayout.BeginVertical();
		GUILayout.Label("1.点击开始修复按钮，会卡顿好一会");
		GUILayout.Label("2.等待Unity可以操作后，使用SVN恢复文件“Revert”所有文件");
		GUILayout.Label("3.SVN完成后点击unity，等待Unity加载后就成功修复了。");
		GUILayout.Label("强调！修复过程会卡顿，稍等片刻即可，修复成功后请马上使用SVN Revert（恢复）文件，如若误提交文件，后果自负。");
		GUILayout.Label("强调！修复过程会卡顿，稍等片刻即可，修复成功后请马上使用SVN Revert（恢复）文件，如若误提交文件，后果自负。");
		GUILayout.Label("强调！修复过程会卡顿，稍等片刻即可，修复成功后请马上使用SVN Revert（恢复）文件，如若误提交文件，后果自负。");
		Debug.Log("强调！修复过程会卡顿，稍等片刻即可，修复成功后请马上使用SVN Revert（恢复）文件，如若误提交文件，后果自负。");
		Debug.Log("强调！修复过程会卡顿，稍等片刻即可，修复成功后请马上使用SVN Revert（恢复）文件，如若误提交文件，后果自负。");
		Debug.Log("强调！修复过程会卡顿，稍等片刻即可，修复成功后请马上使用SVN Revert（恢复）文件，如若误提交文件，后果自负。");
		if (GUILayout.Button("我已经知晓，开始修复"))
		{
			GetSceneTextrue();
			GetLightMap();
		}
		//一键修复地图模糊  光照贴图工具
		//checkFolder = EditorGUILayout.ObjectField("Check Folder", checkFolder, typeof(Object), false);

	
		EditorGUILayout.EndVertical();
	}
	void GetSceneTextrue()
	{
		var path = "Assets/Art/Public/Texture/Scene/v1";
		var guids = AssetDatabase.FindAssets("t:Texture", new string[]{path});
		foreach (var guid in guids)
		{
			var obj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(Texture)) as Texture;
			TextureImporter modelTexImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(obj)) as TextureImporter;
			modelTexImporter.textureType = TextureImporterType.Default;
			modelTexImporter.wrapMode = TextureWrapMode.Repeat;
			modelTexImporter.filterMode = FilterMode.Bilinear;
			modelTexImporter.SaveAndReimport();
		}
	}
	void GetLightMap()
	{
		var path = "Assets/Scene";
		var guids = AssetDatabase.FindAssets("t:Texture", new string[]{path});
		foreach (var guid in guids)
		{
			var obj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(Texture)) as Texture;
			TextureImporter modelTexImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(obj)) as TextureImporter;
			modelTexImporter.textureType = TextureImporterType.Lightmap;
			modelTexImporter.filterMode = FilterMode.Bilinear;
			modelTexImporter.SaveAndReimport();
		}
	}
}


