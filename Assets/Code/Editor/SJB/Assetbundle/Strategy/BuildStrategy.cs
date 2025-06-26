using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
using EditorTool;
using System.Text;

public enum AssetType
{
	UnKnown,
	Prefab,
	Asset,
	Text,
	Img,
	Mat,
	Shader,
	Csharp,
}

public static class BuildMenu
{
	const string BuildAllMenu = "AssetBundle/AssetbundleName settings/All (must be executed once after SVN update)";
	const string BuildScript = "AssetBundle/AssetbundleName settings/lua script";
	const string BuildResource = "AssetBundle/AssetbundleName settings/resource";
	const string BuildUIShader = "AssetBundle/AssetbundleName settings/UI shader";
	const string BuildScene = "AssetBundle/AssetbundleName settings/scene (build settings)";

	const string BuildSimpleAllMenu = "AssetBundle/AssetbundleName settings/Reduce all (must be executed once after SVN update)";
	const string BuildSimpleResource = "AssetBundle/AssetbundleName settings/Reduce resource";
	const string BuildSimpleUIShader = "AssetBundle/AssetbundleName settings/Reduce UI shader";
	const string BuildSimpleScene = "AssetBundle/AssetbundleName settings/Reduce scene (build settings)";

	const string CheckLoadWay = "AssetBundle/Load AssetBundle mode";
	const string CheckBuildSettingScenes = "AssetBundle/Check/BuildSettingScene";
	const string LoadWaySavePref = "SimulateAssetBundleMode";

	static int m_SimulateAssetBundleInEditor = -1;

	public static bool SimulateAssetBundleInEditor {
		get {
			if (m_SimulateAssetBundleInEditor == -1) {
				m_SimulateAssetBundleInEditor = EditorPrefs.GetBool (LoadWaySavePref, false) ? 1 : 0;
				SimulateAssetBundleInEditor = (m_SimulateAssetBundleInEditor == 1);
			}
				
			return m_SimulateAssetBundleInEditor != 0;
		}
		set {
			int newValue = value ? 1 : 0;
			if (newValue != m_SimulateAssetBundleInEditor) {
				m_SimulateAssetBundleInEditor = newValue;
				EditorPrefs.SetBool (LoadWaySavePref, value);
			}
			BuildTargetGroup grp = BuildTargetGroup.iOS;
			if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
				grp = BuildTargetGroup.Android;
			// if (value) {
			// 	PlayerSettings.SetScriptingDefineSymbolsForGroup (grp, ScriptDefines.Remove ("RESOURCE_LOAD"));
			// } else {
			// 	PlayerSettings.SetScriptingDefineSymbolsForGroup (grp, ScriptDefines.Add ("RESOURCE_LOAD"));
			// }
		}
	}

	[MenuItem (CheckLoadWay)]
	public static void ToggleSimulateAssetBundle ()
	{
		SimulateAssetBundleInEditor = !SimulateAssetBundleInEditor;
		AssetDatabase.SaveAssets ();
	}

	[MenuItem (CheckLoadWay, true)]
	public static bool ToggleSimulateAssetBundleValidate ()
	{
		Menu.SetChecked (CheckLoadWay, SimulateAssetBundleInEditor);
		return true;
	}

    #region 设置资源包名称
    [MenuItem (BuildAllMenu, false, 1)]
	public static void BuildAll ()
	{
		AssetManagerConfigEditor.SetAssetManagerInfos ();
	    BuildStrategy.IsSimplePacking = false;
        BuildStrategy.BuildAll ();
	}

	[MenuItem (BuildScript, false, 2)]
	public static void BuildScripts ()
	{
	    BuildStrategy.IsSimplePacking = false;
        BuildStrategy.BuildScripts ();
	}

	[MenuItem (BuildResource, false, 3)]
	public static void BuildResources ()
	{
	    BuildStrategy.IsSimplePacking = false;
        BuildStrategy.BuildResources ();
	}

	[MenuItem (BuildUIShader, false, 4)]
	public static void BuildUIShaders ()
	{
	    BuildStrategy.IsSimplePacking = false;
        BuildStrategy.BuildUIShaders ();
	}

	[MenuItem (BuildScene, false, 5)]
	public static void BuildScenes ()
	{
	    BuildStrategy.IsSimplePacking = false;
        BuildStrategy.BuildScenes ();
	}
    #endregion

    #region 设置精简资源包名称
    [MenuItem(BuildSimpleAllMenu, false, 51)]
    public static void BuildSimpleAll()
    {
        BuildStrategy.IsSimplePacking = true;
        BuildStrategy.BuildAll();
    }

    [MenuItem(BuildSimpleResource, false, 53)]
    public static void BuildSimpleResources()
    {
        BuildStrategy.IsSimplePacking = true;
        BuildStrategy.BuildResources();
    }

    [MenuItem(BuildSimpleUIShader, false, 54)]
    public static void BuildSimpleUIShaders()
    {
        BuildStrategy.IsSimplePacking = true;
        BuildStrategy.BuildUIShaders();
    }

    [MenuItem(BuildSimpleScene, false, 55)]
    public static void BuildSimpleScenes()
    {
        BuildStrategy.IsSimplePacking = true;
        BuildStrategy.BuildScenes();
    }
    #endregion

    [MenuItem (CheckBuildSettingScenes)]
	public static void Check_BuildSettingScenes ()
	{
		List<string> tmp = new List<string> ();
		BuildStrategy.CheckBuildSettingScenes (ref tmp, false);
	}

    [MenuItem("AssetBundle/清除所有bundleName")]
    public static void ClearAllBundleName()
    {
        var clearPaths = new[] { "Art", "Debug", "Resources", "Scene" };
	    
	    //遍历所有file
	    EditorUtility.DisplayProgressBar("Get Bundle Name", "Traverse Folders To Get Files", 0);
	    var fileInfos = new List<FileInfo>();
        foreach (var clearPath in clearPaths)
        {
            BuildStrategy.GetAllFileInfos(fileInfos, new DirectoryInfo(Application.dataPath + "/" + clearPath));
        }
	    EditorUtility.DisplayProgressBar("Get Bundle Name", "Traverse Folders To Get Files", 1);

	    //清除BundleName
	    for (int i = 0; i < fileInfos.Count; i++)
	    {
		    var fileInfo = fileInfos[i];
		    var filePath = fileInfo.FullName.Replace("\\", "/").Replace(Application.dataPath + "/", "Assets/");
		    var importer = AssetImporter.GetAtPath(filePath);
		    if (null != importer)
		    {
			    importer.assetBundleName = "";
		    }
		    EditorUtility.DisplayProgressBar("Clear Bundle Name", string.Format("Clear Bundle Name: {0}", fileInfo.FullName), (float)(i + 1) / fileInfos.Count);
	    }
	    AssetDatabase.Refresh();
	    EditorUtility.ClearProgressBar();
	    
        Debug.Log("清除所有bundleName完成");
    }
}

public static class BuildStrategy
{
	static readonly string MapLuaPath = "Script/Config_Map_FuBen/Table_Map";
	const string ABExtension = ".unity3d";
	static SDictionary<string,string> RealUnUseBundles = new SDictionary<string, string> ();
	const string Scene_CombineBundle_Split = "___";
    public static bool IsSimplePacking = false;

	public static void BuildAll ()
	{
		System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch ();
		stopWatch.Start();

		RealUnUseBundles.Clear ();
		var bundleNames = AssetDatabase.GetAllAssetBundleNames ();
		foreach (var bundleName in bundleNames)
		{
			RealUnUseBundles[bundleName] = bundleName;
		}
		Path2NameEditor.CreateDataAsset ();
        InternalBuildScripts ();
        BuildUIShaders ();
        BuildResources ();
		var errorCount = 0;
        if (!InternalBuildScenes ()) {
			errorCount++;
			EditorApplication.Exit (1);
		}
		if (errorCount == 0) {
			ClearUnUseName ();
		}
		AssetDatabase.Refresh ();
		
		stopWatch.Stop();
		Debug.Log(string.Format("标记所有BundleName耗时{0}秒", stopWatch.Elapsed.TotalMilliseconds / 1000));
	}

	static void ClearUnUseName ()
	{
		AssetImporter importer = null;
		string[] realPaths = null;
		foreach (KeyValuePair<string,string> kvp in RealUnUseBundles) {
			realPaths = AssetDatabase.GetAssetPathsFromAssetBundle (kvp.Key);
			if (realPaths != null && realPaths.Length > 0) {
				//remove their bundle name
				for (int i = 0; i < realPaths.Length; i++) {
					importer = AssetImporter.GetAtPath (realPaths [i]);
					if (importer != null) {
						importer.assetBundleName = null;
					}
					Debug.LogFormat ("无用Bundle资源:{0} bundleName:{1}", realPaths [i], kvp.Key);
				}
			}
		}
		RealUnUseBundles.Clear ();
	}

	[MenuItem ("Assets/打包选中文件\\文件夹")]
	public static void BuildSelect ()
	{
		Path2NameEditor.CreateDataAsset ();
		UnityEngine.Object[] selects = Selection.objects;
		if (selects != null && selects.Length > 0) {
			for (int i = 0; i < selects.Length; i++) {
				UnityEngine.Object selected = selects [i];
				if (selected != null) {
					string path = AssetDatabase.GetAssetPath (selected);
					System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch ();  
					sw.Start ();
					string extension = Path.GetExtension (path);
					if (extension == ".unity")
						BuildScene (path);
					else
						ROScanAssets.ScanDependencies (path, null, new List<string> (){ "Resources/Script" }, HandleResourcesAndDepends);
				
					Build ();
			
					sw.Stop ();  
					TimeSpan ts2 = sw.Elapsed;
					Debug.Log (string.Format ("打包耗时{0}秒", ts2.TotalMilliseconds / 1000));
				}
			}
//			AutoBuildAssetBundles.BuildAssetBundles ();
		}
	}

	[MenuItem ("Assets/清除bundleName")]
	public static void ClearSelect ()
	{
		UnityEngine.Object selected = Selection.activeObject;
		if (selected != null) {
			string path = AssetDatabase.GetAssetPath (selected);
			System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch ();  
			sw.Start ();
			ROScanAssets.ScanDependencies (path, null, null, HandleClearSelect);
			AssetDatabase.RemoveUnusedAssetBundleNames ();
			AssetDatabase.Refresh ();
			sw.Stop ();
			TimeSpan ts2 = sw.Elapsed;
			Debug.Log (string.Format ("耗时{0}秒", ts2.TotalMilliseconds / 1000));
		}
	}

	public static void BuildScripts ()
	{
		Path2NameEditor.CreateDataAsset ();
		InternalBuildScripts ();
	    AssetDatabase.Refresh();
    }

	public static void BuildUIShaders ()
	{
		System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
		stopWatch.Start();
		
		ROScanAssets.ScanDependencies ("Resources/Public/Shader/UI", new List<string> (){ ".shader" }, new List<string> (){ "Resources/Script" }, HandleResourcesAndDepends);
		
		stopWatch.Stop();
		Debug.Log(string.Format("标记UIShaders BundleName耗时{0}秒", stopWatch.Elapsed.TotalMilliseconds / 1000));
	}

	static void InternalBuildScripts ()
	{
#if LUA_FASTPACKING || LUA_STREAMINGASSETS

#else
	    System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
	    stopWatch.Start();
        LuaEncrypt.EncryptTD();
	    ROScanAssets.ScanDependencies("Resources/Script2", null, null, HandleScriptAndItsDepends);
        stopWatch.Stop();
	    Debug.Log(string.Format("标记Scripts BundleName耗时{0}秒", stopWatch.Elapsed.TotalMilliseconds / 1000));
#endif
    }

    public static void BuildScenes ()
	{
		InternalBuildScenes();
	    AssetDatabase.Refresh();
	}

	static void Build ()
	{
//			AutoBuildAssetBundles.BuildAssetBundles ();
//			AssetDatabase.RemoveUnusedAssetBundleNames ();
		AssetDatabase.Refresh ();
	}

	public static bool CheckBuildSettingScenes (ref List<string> scenes, bool skipCheck)
	{
	    if (IsSimplePacking)
	    {
            scenes = new List<string>()
            {
                "Assets/Scene/GameStart/GameEntrance.unity",
                "Assets/Scene/CharacterSelect/CharacterSelect.unity",
                "Assets/Scene/Prontera/SceneProntera.unity",
                "Assets/Scene/prt_1/Sceneprt_1.unity",
                "Assets/Scene/LoadScene/LoadScene.unity",
                "Assets/Scene/CharacterChoose/CharacterChoose.unity"
            };
        }
	    else
	    {
	        List<string> sceneExcepts = new List<string>() {
	            "Scene/testformonster",
	            "Scene/testmap1",
	            "Scene/Launch"
	        };

	        scenes = GetLevelsFromBuildSettings().FindAll(p => {
	            foreach (var except in sceneExcepts)
	            {
	                if (p.Contains(except))
	                {
	                    return false;
	                }
	            }
	            return true;
	        });

	        List<string> errorScenes = new List<string>();
	        if (!CheckConfigMapHasIn(scenes, sceneExcepts, ref errorScenes))
	        {
	            if (!skipCheck)
	            {
	                StringBuilder sb = new StringBuilder();
	                foreach (var errorScene in errorScenes)
	                {
	                    sb.Append(errorScene + " | ");
	                }
	                Debug.LogErrorFormat("these scenes:\n {0} \nhave not in buildsettings", sb.ToString());
	                return false;
	            }
	        }
        }

        return true;
	}

	static bool InternalBuildScenes ()
	{
		System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
		stopWatch.Start();
		
		List<string> scenes = new List<string> ();
		string assetPath = Application.dataPath;
		bool isTrunk = assetPath.Contains ("client-trunk") || assetPath.Contains ("Develop/Develop");
		if (!CheckBuildSettingScenes (ref scenes, isTrunk) && !isTrunk) {
			return false;
		}
		ROScanAssets.ScanDependciesByFiles (scenes, null, true, HandleSceneAndItsDepends);
		
		stopWatch.Stop();
		Debug.Log(string.Format("标记Scenes BundleName耗时{0}秒", stopWatch.Elapsed.TotalMilliseconds / 1000));
		
		return true;
	}

	public static void BuildResources ()
	{
		System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
		stopWatch.Start();
		
	    if (IsSimplePacking)
	    {
	        ROScanAssets.ScanDependencies("Resources/", new List<string>
	        {
	            ".prefab"
	        }, new List<string>
	        {
	            "Resources/Script",
	            "Resources/Role",
                "Resources/Public/Effect"
            }, HandleResourcesAndDepends);

            ROScanAssets.ScanDependencies("Resources/", new List<string>
	        {
	            ".asset",
	            ".xml",
	            ".json",
	            ".wav",
	            ".ogg",
	            ".mp3"
	        }, new List<string>
            {
                "Resources/Script",
                "Resources/Public/Audio"
            }, HandleResourcesAndDepends);

	        ROScanAssets.ScanDependencies("Resources/Public/Material", new List<string>
	        {
	            ".mat"
	        }, new List<string> { "Resources/Script" }, HandleResourcesAndDepends);

	        ROScanAssets.ScanDependencies("Resources/Public/Shader", new List<string>
	        {
	            ".asset",
	            ".mat",
	            ".shader",
	            ".cginc"
	        }, new List<string> { "Resources/Script" }, HandleResourcesAndDepends);

	        ROScanAssets.ScanDependencies("Resources/GUI/pic", new List<string>
	        {
	            ".png",
	            ".jpg",
	            ".renderTexture"
	        }, null, HandleResourcesAndDepends);

	        ROScanAssets.ScanDependencies("Resources/Enviroment", new List<string>
	        {
	            ".png",
	            ".jpg"
	        }, null, HandleResourcesAndDepends);

            //精简角色资源
	        List<string> rolePrefabs = new List<string>
	        {
                "Assets/Resources/Role/Body/10001.prefab",
	            "Assets/Resources/Role/Body/10009.prefab",
	            "Assets/Resources/Role/Body/40008.prefab",
	            "Assets/Resources/Role/Body/30.prefab",
	            "Assets/Resources/Role/Body/38.prefab",
	            "Assets/Resources/Role/Body/45.prefab",
	            "Assets/Resources/Role/Body/14.prefab",
	            "Assets/Resources/Role/Body/5.prefab",
	            "Assets/Resources/Role/Body/21.prefab",
	            "Assets/Resources/Role/Body/29.prefab",
	            "Assets/Resources/Role/Body/37.prefab",
	            "Assets/Resources/Role/Body/46.prefab",
	            "Assets/Resources/Role/Body/13.prefab",
	            "Assets/Resources/Role/Body/6.prefab",
	            "Assets/Resources/Role/Body/22.prefab",
	            "Assets/Resources/Role/Body/1.prefab",
	            "Assets/Resources/Role/Body/2.prefab",

                "Assets/Resources/Role/Weapon/41223.prefab",
	            "Assets/Resources/Role/Weapon/400058.prefab",
	            "Assets/Resources/Role/Weapon/40605.prefab",
	            "Assets/Resources/Role/Weapon/400061.prefab",
                "Assets/Resources/Role/Weapon/400060.prefab",
	            "Assets/Resources/Role/Weapon/41223.prefab",
	            "Assets/Resources/Role/Weapon/400059.prefab",
	            "Assets/Resources/Role/Weapon/40605.prefab",
	            "Assets/Resources/Role/Weapon/40010.prefab",
	            "Assets/Resources/Role/Weapon/40901.prefab",
	            "Assets/Resources/Role/Weapon/40301.prefab",

                "Assets/Resources/Role/Eye/1.prefab",
	            "Assets/Resources/Role/Eye/2.prefab",

                "Assets/Resources/Role/Hair/12.prefab",
	            "Assets/Resources/Role/Hair/15.prefab",
	            "Assets/Resources/Role/Hair/3.prefab",
	            "Assets/Resources/Role/Hair/14.prefab",
	            "Assets/Resources/Role/Hair/2.prefab",
	            "Assets/Resources/Role/Hair/8.prefab",
	            "Assets/Resources/Role/Hair/3.prefab",
	            "Assets/Resources/Role/Hair/6.prefab",
	            "Assets/Resources/Role/Hair/12.prefab",
	            "Assets/Resources/Role/Hair/8.prefab",
	            "Assets/Resources/Role/Hair/11.prefab",
	            "Assets/Resources/Role/Hair/14.prefab",
	            "Assets/Resources/Role/Hair/998.prefab",
	            "Assets/Resources/Role/Hair/999.prefab",
		        
		        "Assets/Resources/Public/Effect/UI/31HlightBox.prefab",
		        "Assets/Resources/Public/Effect/UI/40WorldMapUnlock.prefab",
		        "Assets/Resources/Public/Effect/UI/41map_icon_task_red.prefab",
		        "Assets/Resources/Public/Effect/UI/43map_icon_task_greed.prefab"
            };
	        ROScanAssets.ScanDependciesByFiles(rolePrefabs, null, true, HandleResourcesAndDepends);
	    }
	    else
	    {
	        ROScanAssets.ScanDependencies("Resources/", new List<string>
	        {
	            ".asset",
	            ".prefab",
	            ".xml",
	            ".json",
	            ".wav",
	            ".ogg",
	            ".mp3"
	        }, new List<string> { "Resources/Script" }, HandleResourcesAndDepends);

	        ROScanAssets.ScanDependencies("Resources/Public/Material", new List<string>
	        {
	            ".mat"
	        }, new List<string> { "Resources/Script" }, HandleResourcesAndDepends);

	        ROScanAssets.ScanDependencies("Resources/Public/Shader", new List<string>
	        {
	            ".asset",
	            ".mat",
	            ".shader",
	            ".cginc"
	        }, new List<string> { "Resources/Script" }, HandleResourcesAndDepends);

	        ROScanAssets.ScanDependencies("Resources/GUI/pic", new List<string>
	        {
	            ".png",
	            ".jpg",
                ".tga",
                ".renderTexture"
	        }, null, HandleResourcesAndDepends);

	        ROScanAssets.ScanDependencies("Resources/Enviroment", new List<string>
	        {
	            ".png",
	            ".jpg",
                ".tga"
	        }, null, HandleResourcesAndDepends);
        }
		
		stopWatch.Stop();
		Debug.Log(string.Format("标记Resources BundleName耗时{0}秒", stopWatch.Elapsed.TotalMilliseconds / 1000));
	}

	static void BuildScene (string path)
	{
		ROScanAssets.ScanDependencies (path, new List<string> (){ ".unity" }, new List<string> () {
			"Scene/testformonster",
			"Scene/testmap1"
		}, HandleSceneAndItsDepends);
	}

	static void HandleClearSelect (string path)
	{
		if (GetAssetType (Path.GetExtension (path)) == AssetType.Csharp)
			return;
		AssetImporter importer = AssetImporter.GetAtPath (path);
		importer.assetBundleName = null;
	}

	static void HandleShaderAndDepends (string path)
	{
		if (CommonSetBundleName ("Assets/Code/3Party/T4M/Shaders", path) == false) {
			HandleResourcesAndDepends (path);
		}
	}

	static void HandleResourcesAndDepends (string path)
	{
		SetAssetBundleName (path);
		Debug.Log (path);
	}

	static void HandleSceneAndItsDepends (string path)
	{
		string extension = Path.GetExtension (path);
		AssetType dependAssetType = GetAssetType (path);
		if (extension == ".unity") {
			if (path.Contains ("Config/")) {
				AssetImporter imp = AssetImporter.GetAtPath (path);
				imp.assetBundleName = null;
				return;
			}
			//是场景
			string bundleName = null;
			int index = Path.GetFileNameWithoutExtension (path).IndexOf (Scene_CombineBundle_Split);
			if (index > 0) {
				bundleName = TrimPath (path);
				index = bundleName.IndexOf (Scene_CombineBundle_Split);
				bundleName = bundleName.Remove (index, bundleName.Length - index);
				bundleName = InternalSetBundleName (path, TrimPath, null, bundleName);
			} else {
				bundleName = InternalSetBundleName (path, TrimPath);
			}
			string[] assetsPath = AssetDatabase.GetAssetPathsFromAssetBundle (bundleName);
			for (int i = 0; i < assetsPath.Length; i++) {
				string assetPath = assetsPath [i];
				extension = Path.GetExtension (assetPath);
				if (extension == ".unity" && path != assetPath) {
					if (assetPath.IndexOf (Scene_CombineBundle_Split) > 0 || path.IndexOf (Scene_CombineBundle_Split) > 0) {
						continue;
					}
					AssetImporter importer = AssetImporter.GetAtPath (assetPath);
					importer.assetBundleName = null;
					Debug.LogFormat ("{0}不合法,不该设置bundleName", assetPath);
				}
			}
		} else if (dependAssetType != AssetType.Asset) {
			SetDevelopSceneBundleName (path);
			if (path.StartsWith ("Assets/Scene") == false || dependAssetType == AssetType.Prefab) {
				//scene以外的资源
				SetAssetBundleName (path);
				if (path.Contains ("Config/")) {
					SetSceneBundleName (path, null);
				}
			}
		}
//			Debug.Log (path);
	}

	static void HandleScriptAndItsDepends (string path)
	{
		AssetImporter importer = AssetImporter.GetAtPath (path);

#if LUA_FASTPACKING || LUA_STREAMINGASSETS
        string sEndWith = ".txt";
#else
        string sEndWith = ".bytes";
#endif

        if (path.EndsWith (sEndWith))
			InternalSetBundleNameByRule (path, TrimAssets, importer);
		else
			HandleClearSelect (path);
	}

	static void SetAssetBundleName (string path, AssetImporter importer = null)
	{
		SetArtBundleName (path, importer);
		SetResourcesBundleName (path, importer);
		SetEngineBundleName (path, importer);
	}

	static void SetArtBundleName (string path, AssetImporter importer = null)
	{
		if (path.StartsWith ("Assets/Art")) {
			InternalSetArtBundleName (path, TrimAssets, importer);
		}
	}

	static void SetResourcesBundleName (string path, AssetImporter importer = null)
	{
		if (path.StartsWith ("Assets/Resources")) {
			if (path.Contains ("Resources/Script") == false) {
				InternalSetResourceBundleName (path, TrimAssets, importer);
			}
		}
	}

	static void SetEngineBundleName (string path, AssetImporter importer = null)
	{
		if (path.StartsWith ("Assets/Engine")) {
			InternalSetBundleName (path, TrimPath, importer);
		}
	}

	static void SetSceneBundleName (string path, AssetImporter importer = null, string fix = null)
	{
		if (path.StartsWith ("Assets/Scene")) {
			InternalSetBundleNameByRule (path, TrimPath, importer, null, fix);
		}
	}

	static void SetDevelopSceneBundleName (string path, AssetImporter importer = null)
	{
		if (path.StartsWith ("Assets/DevelopScene/Terrains/Texture")) {
			InternalSetResourceBundleName (path, TrimPath, importer);
		}
	}

	static bool CommonSetBundleName (string startWith, string path, AssetImporter importer = null)
	{
		if (string.IsNullOrEmpty (startWith) == false) {
			if (path.StartsWith (startWith)) {
				InternalSetResourceBundleName (path, TrimPath, importer);
				return true;
			}
		} else {
			Debug.LogError ("cena CommonSetBundleName has no starwith");
		}
		return false;
	}

	//	static string[] AnimExtension = new string[]{".anim"};
	//	static string[] TextureExtension = new string[]{".png",".bmp",".tga",".mat"};
	//		static string[] TextureExtension = new string[]{".png",".bmp",".tga",".mat"};
	//	static string[] ModelExtension = new string[]{".FBX",".fbx"};
	static string[] ShaderExtension = new string[] {
		".shader",
		".cginc",
		".mat",
		".prefab",
		".controller"
	};

	static void InternalSetResourceBundleName (string path, Func<string,string> modifyPath, AssetImporter importer = null)
	{
		string bundleName = null;
		if (InternalCheckSet (path, ShaderExtension, "", out bundleName)) {
			InternalSetBundleNameByRule (path, modifyPath, importer);
//			InternalSetBundleName (path, modifyPath, importer, modifyPath (bundleName));
		} else
			InternalSetBundleName (path, modifyPath, importer);
	}

	static void InternalSetArtBundleName (string path, Func<string,string> modifyPath, AssetImporter importer = null)
	{
//			string bundleName = null;
//			if (InternalCheckSet (path, AnimExtension, "_Anims", out bundleName)) {
//				InternalSetBundleName (path, modifyPath, importer, modifyPath (bundleName));
//			} else if (InternalCheckSet (path, TextureExtension, "_Texs", out bundleName)) {
//				InternalSetBundleName (path, modifyPath, importer, modifyPath (bundleName));
//			} else if (InternalCheckSet (path, ModelExtension, "_Models", out bundleName)) {
//				InternalSetBundleName (path, modifyPath, importer, modifyPath (bundleName));
//			} else
		InternalSetBundleNameByRule (path, modifyPath, importer);
	}

	static bool InternalCheckSet (string path, string[] endWiths, string fixName, out string bundleName)
	{
		bool isEndWith = false;
		foreach (string endwith in endWiths) {
			if (path.EndsWith (endwith)) {
				isEndWith = true;
				break;
			}
		}
		if (isEndWith) {
			string[] directories = Path.GetDirectoryName (path).Split (Path.DirectorySeparatorChar);
			bundleName = Path.GetDirectoryName (path) + "/" + directories [directories.Length - 1] + fixName;
			return true;
		}
		bundleName = null;
		return false;
	}

	static void InternalSetBundleNameByRule (string path, Func<string,string> modifyPath, AssetImporter importer = null, string bundleName = null, string fixString = null)
	{
		if (importer == null)
			importer = AssetImporter.GetAtPath (path);
		if (string.IsNullOrEmpty (bundleName))
			bundleName = modifyPath (path);
		string extension = Path.GetExtension (bundleName);
		if (string.IsNullOrEmpty (extension) == false) {
			bundleName = bundleName.Replace (extension, "");
		}
		BuildRule rule = BundleBuildRule.MatchRule (path);
		if (rule != null) {
//				if(rule.rule != EBuildRule.SelfName)
			bundleName = rule.GetBundleName (bundleName);
		}
		if (string.IsNullOrEmpty (fixString) == false) {
			bundleName = bundleName + fixString;
		}
		SetBundleName (importer, bundleName + ABExtension);
	}

	static string InternalSetBundleName (string path, Func<string,string> modifyPath, AssetImporter importer = null, string bundleName = null)
	{
		if (importer == null)
			importer = AssetImporter.GetAtPath (path);
		if (string.IsNullOrEmpty (bundleName))
			bundleName = modifyPath (path);
//			if (bundleName.StartsWith ("Resources")) {
		string extension = Path.GetExtension (bundleName);
		if (string.IsNullOrEmpty (extension) == false) {
			bundleName = bundleName.Replace (extension, "");
		}
//			} else
//				bundleName = bundleName.Replace (".", "-");
//			Debug.Log ("bundlename:"+bundleName);
//			Debug.Log (bundleName.Replace (".",""));
//			Debug.Log (System.IO.Path.DirectorySeparatorChar);
		return SetBundleName (importer, bundleName + ABExtension);
	}

    public static void ClearAllBundleName(DirectoryInfo dir)
    {
        var allFile = Array.FindAll(dir.GetFiles(), info => !info.FullName.EndsWith(".meta") && !info.FullName.EndsWith(".cs"));
        for (int index = 0; index < allFile.Length; index++)
        {
            var fileInfo = allFile[index];
            var filePath = fileInfo.FullName.Replace("\\", "/").Replace(Application.dataPath + "/", "Assets/");
            AssetImporter importer = AssetImporter.GetAtPath(filePath);
            if (null != importer)
            {
                importer.assetBundleName = "";
            }
	        EditorUtility.DisplayProgressBar("Clear Bundle Name", string.Format("Clear Bundle Name: {0}", fileInfo.FullName), (float)(index + 1) / allFile.Length);
        }

        DirectoryInfo[] allDir = dir.GetDirectories();
        foreach (DirectoryInfo d in allDir)
        {
            ClearAllBundleName(d);
        }
    }

	public static void GetAllFileInfos(List<FileInfo> fileInfos, DirectoryInfo dir)
	{
		var allFile = Array.FindAll(dir.GetFiles(), info => !info.FullName.EndsWith(".meta") && !info.FullName.EndsWith(".cs"));
		foreach (var file in allFile)
		{
			fileInfos.Add(file);
		}

		DirectoryInfo[] allDir = dir.GetDirectories();
		foreach (DirectoryInfo d in allDir)
		{
			GetAllFileInfos(fileInfos, d);
		}
	}

    static string SetBundleName (AssetImporter importer, string name)
	{
		importer.assetBundleName = name;
		RealUnUseBundles.Remove (importer.assetBundleName);
		return importer.assetBundleName;
	}

	static string TrimPath (string path)
	{
		return path.Replace (Path.GetExtension (path), "").Replace ("Assets/", "");
	}

	static string TrimExtension (string path)
	{
		return path.Replace (Path.GetExtension (path), "");
	}

	static string TrimAssets (string path)
	{
		return path.Replace ("Assets/", "");
	}

	static AssetType GetAssetType (string extension)
	{
		if (extension.Contains (Path.DirectorySeparatorChar.ToString ()))
			extension = Path.GetExtension (extension);
		switch (extension) {
		case ".prefab":
			return AssetType.Prefab;
		case ".shader":
			return AssetType.Shader;
		case ".cs":
			return AssetType.Csharp;
		case ".asset":
			return AssetType.Asset;
		}
		return AssetType.UnKnown;
	}

	static List<string> GetLevelsFromBuildSettings ()
	{
		List<string> levels = new List<string> ();
		for (int i = 0; i < EditorBuildSettings.scenes.Length; ++i) {
			if (EditorBuildSettings.scenes [i].enabled)
				levels.Add (EditorBuildSettings.scenes [i].path);
		}
			
		return levels;
	}

	static SDictionary<string,bool> GetLuaConfigMapName ()
	{
		SDictionary<string,bool> maps = new SDictionary<string, bool> ();
		try {
			var tAsset = (TextAsset)Resources.Load (MapLuaPath);
			if (tAsset != null) {
				var text = tAsset.text;
				var table = LuaSvrForEditor.Me.DoString (text) as SLua.LuaTable;
				foreach (var t in table) {
					SLua.LuaTable config = t.value as SLua.LuaTable;
					if (config != null) {
						string name = config ["NameEn"] as string;
						if (!string.IsNullOrEmpty (name)) {
							maps ["Scene" + name] = true;
						}
					}
				}
			}
			return maps;
		} catch (System.Exception e) {
			Debug.LogException (e);
		}
		return maps;
	}

	static bool CheckConfigMapHasIn (List<string> buildSettings, List<string> excepts, ref List<string> errorScenes)
	{
		errorScenes.Clear ();
		SDictionary<string,bool> configs = GetLuaConfigMapName ();
		SDictionary<string,bool> settings = new SDictionary<string, bool> ();
		buildSettings.ForEach ((name) => {
			settings [Path.GetFileNameWithoutExtension (name)] = true;
		});
		SDictionary<string,bool> exceptsMap = new SDictionary<string, bool> ();
		foreach (var e in excepts) {
			exceptsMap ["Scene" + Path.GetFileNameWithoutExtension (e)] = true;
		}
		foreach (KeyValuePair<string,bool> kvp in configs) {
			if (!settings.ContainsKey (kvp.Key) && !exceptsMap.ContainsKey(kvp.Key)) {
				errorScenes.Add (kvp.Key);
			}
		}
		return errorScenes.Count == 0;
	}
}
