using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using System.IO;
using Ghost.Config;
using Ghost.Extensions;

namespace Ghost.EditorTool
{
	public class AssetDependenceLevel {

		public const int LEVEL_MAX = 100;

		private static int DoGetAssetLevel(Object assetObj, string assetPath)
		{
			var assetType = assetObj.GetType();
			if (typeof(MonoScript) == assetType)
			{
				return 0;
			}
			else if (typeof(Shader) == assetType)
			{
				return 0;
			}
			else if (typeof(Texture2D) == assetType)
			{
				return 0;
			}
			else if (typeof(TextAsset) == assetType)
			{
				return 0;
			}
			else if (typeof(AnimationClip) == assetType)
			{
				return 0;
			}
			else if (typeof(AudioClip) == assetType)
			{
				return 0;
			}
			else if (typeof(Font) == assetType)
			{
				return 0;
			}
			else if (typeof(Material) == assetType)
			{
				return 1;
			}
			else if (typeof(UnityEditor.Animations.AnimatorController) == assetType)
			{
				return 1;
			}
			else if (typeof(GameObject) == assetType)
			{
				if (string.Equals(Path.GetExtension(assetPath).TrimStart('.').ToLower(), PathConfig.EXTENSION_FBX))
				{
					return 2;
				}
				else if (string.Equals(Path.GetExtension(assetPath).TrimStart('.').ToLower(), PathConfig.EXTENSION_PREFAB))
				{
					return LEVEL_MAX - 1;
				}
			}
			else if (typeof(Object) == assetType)
			{
				if (string.Equals(Path.GetExtension(assetPath).TrimStart('.').ToLower(), PathConfig.EXTENSION_SCENE))
				{
					return LEVEL_MAX;
				}
			}
			else
			{
				if (string.Equals(Path.GetExtension(assetPath).TrimStart('.').ToLower(), PathConfig.EXTENSION_ASSET))
				{
					return 0;
				}
			}
			Debug.LogError(string.Format("{0}({1})\n asset type is not supported\n", assetPath, assetType));
			return -1;
		}

		public static bool IsBadAssetLevel(int level)
		{
			return 0 > level;
		}

		/// <summary>
		/// Gets the asset level.
		/// </summary>
		/// <returns>The asset level. Use IsBadAssetLevel to check the level returned.</returns>
		/// <param name="path">Path. If path is folder, return the max level of the files in this folder(exclude sub folders).</param>
		/// /// <param name="path">Path.</param>
		public static int GetAssetLevel(Object assetObj, string assetPath)
		{
			if (Directory.Exists(assetPath))
			{
				var files = Directory.GetFiles(assetPath);
				if (files.IsNullOrEmpty())
				{
					Debug.LogWarning(string.Format("{0}\nis a empty folder\n", assetPath));
					return -1;
				}
				var levels = new List<int>();
				foreach(var file in files)
				{
					var obj = AssetDatabase.LoadMainAssetAtPath(file);
					if (null == obj)
					{
						continue;
					}
					levels.Add(DoGetAssetLevel(obj, file));
				}
				if (0 >= levels.Count)
				{
					Debug.LogWarning(string.Format("{0}\nis a empty folder\n", assetPath));
					return -1;
				}
				levels.Sort();
				return levels[levels.Count-1];
			}
			else if (File.Exists(assetPath))
			{
				return DoGetAssetLevel(assetObj, assetPath);
			}
			Debug.LogWarning(string.Format("{0}\nis not a folder or file\n"));
			return -1;
		}

		public static int GetAssetLevel(string assetPath)
		{
			return GetAssetLevel(AssetDatabase.LoadMainAssetAtPath(assetPath), assetPath);
		}

		public static int GetAssetLevel(Object assetObj)
		{
			return GetAssetLevel(assetObj, AssetDatabase.GetAssetPath(assetObj));
		}
		
	}
} // namespace Ghost.EditorTool
