using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using Ghost.Extensions;
using Ghost.Utils;

namespace EditorTool
{
	public class TextureInfo : ScriptableObject
	{
		public Object[] mipMapFolders;
		public Object[] mipMapIgnoreFolders;
		public Object[] guiTextureTypeFolders;
		public Texture[] mipMapIgnoreTextures;

		private HashSet<string> mipMapPaths;
		private HashSet<string> ignorePaths;
		private HashSet<string> guiTypePaths;
		private HashSet<Texture> ignoreTextures;

		private HashSet<string> _HashSetPaths(Object[] folders)
		{
			var paths = new HashSet<string> ();
			if (!folders.IsNullOrEmpty())
			{
				foreach (var folder in folders)
				{
					var path = AssetDatabase.GetAssetPath(folder);
					if (AssetDatabase.IsValidFolder(path))
					{
						paths.Add(path);
					}
				}
			}
			return paths;
		}

		public void Refresh()
		{
			mipMapPaths = new HashSet<string>();
			if (!mipMapFolders.IsNullOrEmpty())
			{
				foreach (var folder in mipMapFolders)
				{
					var path = AssetDatabase.GetAssetPath(folder);
					if (AssetDatabase.IsValidFolder(path))
					{
						mipMapPaths.Add(path);
					}
				}
			}

			ignorePaths = new HashSet<string>();
			if (!mipMapIgnoreFolders.IsNullOrEmpty())
			{
				foreach (var folder in mipMapIgnoreFolders)
				{
					var path = AssetDatabase.GetAssetPath(folder);
					if (AssetDatabase.IsValidFolder(path))
					{
						ignorePaths.Add(path);
					}
				}
			}

			guiTypePaths = _HashSetPaths (guiTextureTypeFolders);
			
			ignoreTextures = new HashSet<Texture>();
			if (!mipMapIgnoreTextures.IsNullOrEmpty())
			{
				foreach (var t in mipMapIgnoreTextures)
				{
					if (null != t)
					{
						ignoreTextures.Add(t);
					}
				}
			}
		}

		private bool IsInMipMapFolder(string path)
		{
			if (mipMapPaths.IsNullOrEmpty())
			{
				return false;
			}
			foreach (var folder in mipMapPaths)
			{
				if (path.StartsWith(folder))
				{
					return true;
				}
			}
			return false;
		}

		private bool IsIgnored(Texture t, string path)
		{
			if (ignoreTextures.Contains(t))
			{
				Debug.LogFormat(t, "<color=yellow>[MipMap Ignore]: </color>{0}", path);
				return true;
			}
			foreach (var ignorePath in ignorePaths)
			{
				if (path.StartsWith(ignorePath))
				{
					Debug.LogFormat(t, "<color=yellow>[MipMap Ignore]: </color>{0}", path);
					return true;
				}
			}
			return false;
		}

		private bool IsInGUIType(string path)
		{
			return _IsInPath (path, guiTypePaths);
		}

		private bool _IsInPath(string path,HashSet<string> paths)
		{
			if (paths.IsNullOrEmpty())
			{
				return false;
			}
			foreach (var folder in paths)
			{
				if (path.StartsWith(folder))
				{
					return true;
				}
			}
			return false;
		}

		public static string BatchProcessingInfoPath = "Assets/Art/TextureBatchProccessing.asset";

		private static TextureInfo global = null;
		public static TextureInfo Global
		{
			get
			{
				if (null == global)
				{
					global = AssetDatabase.LoadAssetAtPath<TextureInfo>(BatchProcessingInfoPath);
					if (null == global)
					{
						Debug.LogErrorFormat("No TextureBatchProccessing.asset");
					}
					else
					{
						global.Refresh();
					}
				}
				return global;
			}
		}

		public static bool InMipMapFolder(string path)
		{
			return null != Global && Global.IsInMipMapFolder(path);
		}

		public static bool InGUITypeFolder(string path)
		{
			return null != Global && Global.IsInGUIType(path);
		}

		public static bool Ignored(Texture t, string path)
		{
			return null == Global || Global.IsIgnored(t, path);
		}
	}

	[CustomEditor(typeof(TextureInfo))]
	public class TextureInfoEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			if (GUILayout.Button("Refresh"))
			{
				var info = target as TextureInfo;
				info.Refresh();
			}
			EditorGUILayout.Separator();
			base.OnInspectorGUI ();
		}
	}

} // namespace EditorTool
