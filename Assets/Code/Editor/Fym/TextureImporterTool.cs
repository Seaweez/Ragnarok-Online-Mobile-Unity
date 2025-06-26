using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using Ghost.Extensions;
using Ghost.Utils;
using System.IO;

public class TextureImporterTool : EditorWindow {

	private int scaleBack = 1;
	private Texture textureModel = null;//模版变量
	private Object folder = null;

	private string[] scaleBackStrings = {
		"1",
		"1／2",
		"1／4",
		"1／8",
	};
	private int[] scaleBacks = {
		1,
		2,
		4,
		8
	};

	/// <summary>
	/// 创建、显示窗体
	/// </summary>
	[MenuItem("Assets/批量设置纹理", false, 1)]
	private static void Init()
	{    
		TextureImporterTool window = (TextureImporterTool)EditorWindow.GetWindow(typeof(TextureImporterTool), true, "TextureImportSetting");

		var selectedObjs = Selection.GetFiltered(typeof(Object), SelectionMode.TopLevel);
		if (!selectedObjs.IsNullOrEmpty())//IsNullOrEmpty 即判断是否为Null也判断是否为空
		{
			var path = AssetDatabase.GetAssetPath(selectedObjs[0]);//获得当前选中的目标所在目录
			if (AssetDatabase.IsValidFolder(path))//判断是否为有效文件夹
			{
				window.folder = Selection.activeObject;
			}
			else if (!string.IsNullOrEmpty(path))
			{
				var folderPath = Path.GetDirectoryName(path);//得到目录名称
				if (AssetDatabase.IsValidFolder(folderPath))
				{
					window.folder = AssetDatabase.LoadAssetAtPath(folderPath, typeof(Object));//获得路径
				}
			}
		}

		window.Show();
	}
	
	/// <summary>
	/// 显示窗体里面的内容
	/// </summary>
	private void OnGUI()
	{
		//不用BeginHorizontal();使用自动排版
		folder = EditorGUILayout.ObjectField("当前路径 :",folder,typeof(Object),false);////给当前面板赋值
		if (!AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(folder)))//异常处理,保证程序健壮
		{
			folder = null;
		}
		textureModel = EditorGUILayout.ObjectField("模版图片 :",textureModel,typeof(Texture),false) as Texture;//使用as 对目标进行转换类型，非强制转换
		GUILayout.Label("将会批量处理该图片所在目录的所有图片！");
		scaleBack = EditorGUILayout.IntPopup("尺寸缩放 :", scaleBack, scaleBackStrings, scaleBacks);//IntPopup下拉菜单

		if (GUILayout.Button("应用"))
			LoopSetTexture();
	}
	
	/// <summary>
	/// 获取贴图设置
	/// </summary>
	/// 
	public Texture[] GetTextures()
	{
		if (null == folder)
		{
			return null;
		}

		var filterString = StringUtils.ConnectToString("t:", typeof(Texture).Name);//ConnectToString；；链接字符串
		var guids = AssetDatabase.FindAssets(filterString, new string[]{AssetDatabase.GetAssetPath(folder)});//FinAssets查找资源 

		if (guids.IsNullOrEmpty())
		{
			return null;
		}

		var textures = new List<Texture>();
		foreach (var guid in guids)
		{
			var texture = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(Texture)) as Texture;
			if (null != texture)
			{
				textures.Add(texture);
			}
		}

		return textures.ToArray();
	}
	
	/// <summary>
	/// 循环设置选择的贴图
	/// </summary>
	private void LoopSetTexture()
	{
		if (null == textureModel)
		{
			Debug.LogError("没有模版图片");
			return;
		}
		if (null == folder)
		{
			Debug.LogError("没有当前路径");
			return;
		}

		var textures = GetTextures();
		if (textures.IsNullOrEmpty())
		{
			Debug.Log("没有图片");
			return;
		}

		TextureImporter modelTexImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(textureModel)) as TextureImporter;
		TextureImporterSettings modelSettings = new TextureImporterSettings();
		modelTexImporter.ReadTextureSettings(modelSettings);

		foreach (var texture in textures)
		{
			string path = AssetDatabase.GetAssetPath(texture);
			TextureImporter texImporter = AssetImporter.GetAtPath(path) as TextureImporter;
			if(texture.width%2!=0||texture.height%2!=0)
			{
				Debug.Log("Texture Error,Width and height Error--------> : "+texture.name);
			}
			var texSize = System.Math.Max(texture.width, texture.height);

			TextureImporterPlatformSettings tps = new TextureImporterPlatformSettings();
			tps.maxTextureSize = texSize / scaleBack;
			texImporter.SetPlatformTextureSettings(tps);  

			texImporter.SetTextureSettings(modelSettings);
			texImporter.SaveAndReimport();
		}
	}
}
