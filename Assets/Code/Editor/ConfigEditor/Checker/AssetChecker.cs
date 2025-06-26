using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using RO;
using UnityEditor.Animations;
using System.IO;
using Ghost.Utils;

namespace EditorTool
{
    public class CheckedGameObject
    {
        public UnityEngine.Object obj = null;
        public bool alreadyChecked = false;
        public string error = null;
        public CheckedGameObject(UnityEngine.Object _Obj)
        {
            obj = _Obj;
            alreadyChecked = false;
        }

        public void AppendError(string newError)
        {
            if (string.IsNullOrEmpty(error))
                error += "\n";
            error += newError;
            alreadyChecked = true;
        }
    }

    public class AssetChecker
	{
		static string _ResourcesPath = "Assets/Resources/";
		static Dictionary<string, CheckedGameObject> _cachedAssets = new Dictionary<string, CheckedGameObject>();

		public static string textureExt = "png";
		public static string materialExt = "mat";
		public static string[] modelExt = new string[]{ "FBX", "prefab" };
		public static string prefabExt = "prefab";
		public static string aniExt = "anim";
		public static string rolePath = "Assets/Art/Model/Role";
		public static string atlasSpriteMissErrorFormat = "{0} = {1} 该图片在图集 {2} 中未找到！";
		public static string fileMissErrorFormat = "\"{0}\"文件不存在！";
		public static string fileMissMRMissErrorFormat = "MainRenderer不存在或者\"{0}\"文件不存在！";
        public AssetChecker ()
		{
		}

		public static T TryLoadAsset<T>(string path) where T : UnityEngine.Object
		{
			T asset = null;
			if(_cachedAssets.ContainsKey(path))
			{
				asset = _cachedAssets[path].obj as T;
			}
			else if (System.IO.File.Exists (path))
			{
				asset = AssetDatabase.LoadAssetAtPath<T>(path);
				if(asset != null)
					_cachedAssets.Add(path, new CheckedGameObject(asset));
			}
			return asset;
		}

		public static void ClearAssetCache()
		{
			_cachedAssets.Clear ();
			Resources.UnloadUnusedAssets ();
			GC.Collect ();
		}

		public static bool CheckAsset<T>(string path) where T : UnityEngine.Object
		{
			bool exist = false;
			if (System.IO.File.Exists (path))
			{
				T asset = TryLoadAsset<T> (path);
				if (asset != null)
				{
					Resources.UnloadAsset(asset);
					return true;
				}
			}
			return exist;
		}

		public static bool CheckAssetFile(string path)
		{
			if (System.IO.File.Exists (path))
				return true;
			return false;
		}

		public static string CheckAssetFileWithMultiExt(string folder, string fname, string[] extArr)
		{
			string path = PathUnity.Combine (folder, fname);

			if (extArr.Length > 0) 
			{
				for (int i = 0; i < extArr.Length; i++) 
				{
					string fullPath = Path.ChangeExtension (path, extArr[i]);
					if(System.IO.File.Exists (fullPath))
						return null;
				}
			}
			return string.Format (fileMissErrorFormat, path);
		}

		public static bool CheckAtlasSprite(UIAtlas atlas, string spriteName)
		{					
			return atlas.GetSprite(spriteName) != null;
		}

		public static UISpriteData GetAtlasSpriteByCfg(string spriteName, string key)
		{
			UISpriteData ret = null;
			string[] atlasPathArr = null;
			if(CfgEditor_UIAtlasConfig.map.TryGetValue(key, out atlasPathArr))
			{
				for(int i = 0; i < atlasPathArr.Length; i++)
				{
					String path = _ResourcesPath + atlasPathArr [i];
					ret = GetAtlasSpriteByPath(spriteName, path);
					if(ret != null)
						return ret;
				}
			}
			return ret;
		}

		private static UISpriteData GetAtlasSpriteByPath(string spriteName, string path)
		{
			UISpriteData ret = null;
			UIAtlas atlas = TryLoadAsset<UIAtlas>(path + ".prefab");
			if(atlas != null)
				ret = atlas.GetSprite(spriteName);
			return ret;
		}

		public static bool CheckAtlasSpriteByCfg(string spriteName, string key)
		{
			return GetAtlasSpriteByCfg(spriteName, key) != null;
		}

		public static bool CheckAtlasSpriteByPath(string spriteName, string path)
		{
			return GetAtlasSpriteByPath(spriteName, path) != null;
		}
	}
}