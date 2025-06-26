using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;
using System.IO;
using Ghost.Utils;

namespace EditorTool
{
	public class AssetManagerConfigEditor
	{
		static AssetManageConfig _config;

		public static string filePath {
			get {
				return PathUnity.Combine (Application.dataPath, "Resources/AssetManageConfig/AssetManageConfig.xml");
			}
		}

		[MenuItem("AssetBundle/导出资源加载管理配置")]
		public static void SetAssetManagerInfos ()
		{
			ReCreateConfig ();
			SaveVersionsXML ();
		}

		public static void ReCreateConfig ()
		{
			config.cachePoolMaxNum = 35;
			//Art
			config.AddInfo (1, "Art/Model/Bus", AssetManageMode.UnLoadImmediately, 0, AssetManageMode.UnLoadImmediately, 0, AssetEncryptMode.None);
			config.AddInfo (2, "Art/Model/Effect", AssetManageMode.Custom, 0, AssetManageMode.AutoUnloadNoDependsCachePool, 0, AssetEncryptMode.None);
			config.AddInfo (3, "Art/Model/Item", AssetManageMode.NeverUnLoad, 0, AssetManageMode.NeverUnLoad, 0, AssetEncryptMode.None);
			config.AddInfo (4, "Art/Model/Role", AssetManageMode.Custom, 0, AssetManageMode.AutoUnloadNoDependsCachePool, 0, AssetEncryptMode.None);
			config.AddInfo (5, "Art/Model/Scene", AssetManageMode.Custom, 0, AssetManageMode.Custom, 0, AssetEncryptMode.None);
			AssetManageInfo artPublics = config.AddInfo (6, "Art/Public", AssetManageMode.LRU, 0, AssetManageMode.Custom, 0, AssetEncryptMode.None);
			AssetManageInfo guisub = artPublics.AddSubInfo (7, "Art/Public/Texture/GUI", AssetManageMode.Custom, 0, AssetManageMode.Custom, 0, AssetEncryptMode.None);
			guisub.AddSubInfo (8, "Art/Public/Texture/GUI/atlas/Login", AssetManageMode.Custom, 0, AssetManageMode.AutoUnloadNoDepends, 0, AssetEncryptMode.None);
			artPublics.AddSubInfo (9, "Art/Public/Texture/Spine", AssetManageMode.NeverUnLoad, 0, AssetManageMode.NeverUnLoad, 0, AssetEncryptMode.None);

			//resource
			config.AddInfo (101, "Resources/Public/Audio/BGM", AssetManageMode.UnLoadImmediately, 0, AssetManageMode.UnLoadImmediately, 0, AssetEncryptMode.None);
			config.AddInfo (102, "Resources/Public/Audio/SE", AssetManageMode.LRU, 15, AssetManageMode.LRU, 50, AssetEncryptMode.None);
			config.AddInfo (103, "Resources/Public/Effect", AssetManageMode.NeverUnLoad, 0, AssetManageMode.LRU, 40, AssetEncryptMode.None);
			config.AddInfo (104, "Resources/Public/Emoji", AssetManageMode.NeverUnLoad, 0, AssetManageMode.UnLoadImmediately, 0, AssetEncryptMode.None);
			config.AddInfo (105, "Resources/Public/Item", AssetManageMode.NeverUnLoad, 0, AssetManageMode.UnLoadImmediately, 0, AssetEncryptMode.None);
			config.AddInfo (106, "Resources/Public/Material", AssetManageMode.NeverUnLoad, 0, AssetManageMode.NeverUnLoad, 0, AssetEncryptMode.None);
			config.AddInfo (107, "Resources/Public/Shader", AssetManageMode.NeverUnLoad, 0, AssetManageMode.NeverUnLoad, 0, AssetEncryptMode.None);
			config.AddInfo (108, "Resources/Public/SpineEffect", AssetManageMode.NeverUnLoad, 0, AssetManageMode.UnLoadImmediately, 0, AssetEncryptMode.None);

			config.AddInfo (109, "Resources/Role", AssetManageMode.Custom, 0, AssetManageMode.ResourceAutoUnloadNoDepends, 0, AssetEncryptMode.None);
			config.AddInfo (110, "Resources/NPC", AssetManageMode.Custom, 20, AssetManageMode.UnLoadImmediately, 20, AssetEncryptMode.None);
			config.AddInfo (111, "Resources/Bus", AssetManageMode.UnLoadImmediately, 0, AssetManageMode.UnLoadImmediately, 0, AssetEncryptMode.None);
			config.AddInfo (112, "Resources/GUI/atlas", AssetManageMode.NeverUnLoad, 0, AssetManageMode.NeverUnLoad, 0, AssetEncryptMode.None);
			config.AddInfo (113, "Resources/GUI/pic", AssetManageMode.UnLoadImmediately, 0, AssetManageMode.UnLoadImmediately, 0, AssetEncryptMode.None);
			config.AddInfo (114, "Resources/GUI/v1", AssetManageMode.NeverUnLoad, 0, AssetManageMode.UnLoadImmediately, 0, AssetEncryptMode.None);

			config.AddInfo (115, "Resources/Skill", AssetManageMode.NeverUnLoad, 0, AssetManageMode.UnLoadImmediately, 0, AssetEncryptMode.None);
			AssetManageInfo luaScript = config.AddInfo (116, "Resources/Script", AssetManageMode.Custom, 0, AssetManageMode.UnLoadImmediately, 0, AssetEncryptMode.None);
			luaScript.AddSubInfo (117,"Resources/Script/FrameWork", AssetManageMode.NeverUnLoad, 0, AssetManageMode.UnLoadImmediately, 0, AssetEncryptMode.None);

            luaScript = config.AddInfo(118, "Resources/Script2", AssetManageMode.Custom, 0, AssetManageMode.UnLoadImmediately, 0, AssetEncryptMode.None);
            luaScript.AddSubInfo(119, "Resources/Script2/FrameWork", AssetManageMode.NeverUnLoad, 0, AssetManageMode.UnLoadImmediately, 0, AssetEncryptMode.None);

            //developScene
            config.AddInfo (201, "DevelopScene/Terrains", AssetManageMode.Custom, 0, AssetManageMode.Custom, 0, AssetEncryptMode.None);
		}

		public static AssetManageConfig config {
			get {
				if (_config == null) {
					_config = new AssetManageConfig ();
				}
				return _config;
			}
			set {
				_config = value;
			}
		}

		static void SaveVersionsXML ()
		{
			string fileName = filePath;
			string folder = Path.GetDirectoryName (fileName);
			if (Directory.Exists (folder) == false)
				Directory.CreateDirectory (folder);
			config.SaveToFile (fileName);
			AssetDatabase.Refresh ();
		}
	}
} // namespace EditorTool
