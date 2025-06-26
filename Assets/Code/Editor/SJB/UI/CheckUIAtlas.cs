using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;
using System;
using System.IO;

namespace EditorTool
{
	public class CheckUIAtlas
	{
		[MenuItem ("RO/检测所有UI图集大小")]
		static public void CheckAtlasSize ()
		{
			UIAtlas[] uis = Search<UIAtlas> ();
			long wholeSize = 0;
			long size = 0;
			Texture t;
			for (int i=0; i<uis.Length; i++) {
				UIAtlas ui = uis [i];
				string s;
				t = ui.texture;
				s = t.width + "x" + t.height;
				size = CalculateTextureSizeBytes (t);
				Debug.Log (string.Format ("图集{0}，{1}，大小{2}", ui.name, s, humanReadableByteCount (size)));
				wholeSize += size;
			}
			Debug.Log (string.Format ("总共{0}个UI图集,size:{1}", uis.Length, humanReadableByteCount (wholeSize)));
//			CheckUITexture ();
		}

		static void CheckUITexture ()
		{
			UITexture[] uits = Search<UITexture> (true);
			List<Texture> ts = new List<Texture> ();
			long wholeSize = 0;
			long size = 0;
			for (int i=0; i<uits.Length; i++) {
				Texture t = uits [i].mainTexture;
				if (t != null) {
					if (ts.Contains (t) == false)
						ts.Add (t);
				}
			}
			foreach (Texture t in ts) {
				string s = t.width + "x" + t.height;
				size = CalculateTextureSizeBytes (t);
				Debug.Log (string.Format ("图{0}，{1}，大小{2}", t, s, humanReadableByteCount (size)));
				wholeSize += size;
			}
			Debug.Log (string.Format ("总共{0}个UITexture,所用图片共{1}个,total size:{2}", uits.Length, ts.Count, humanReadableByteCount (wholeSize)));
		}

		static T[] Search<T> (bool includeChildren = false)
		{
			string[] paths = AssetDatabase.GetAllAssetPaths ();
			List<T> list = new List<T> ();
			
			for (int i = 0; i < paths.Length; ++i) {
				string path = paths [i];
				
				bool valid = false;
				
				if (path.EndsWith ("prefab", System.StringComparison.OrdinalIgnoreCase)) {
					valid = true;
				}
				
				if (!valid)
					continue;
				
				EditorUtility.DisplayProgressBar ("Loading", "Searching assets, please wait...", (float)i / paths.Length);
				UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath (path);
				if (obj == null)
					continue;
				
				if (PrefabUtility.GetPrefabType (obj) == PrefabType.Prefab) {
					T[] t = includeChildren ? ((obj as GameObject).GetComponentsInChildren<T> ()) : ((obj as GameObject).GetComponents<T> ());
					if (t != null) {
						foreach (T s in t) {
							if (!list.Contains (s))
								list.Add (s);
						}
					}
				}
			}
			EditorUtility.ClearProgressBar ();
			return list.ToArray ();
		}

		public static string humanReadableByteCount (long bytes)
		{
			int unit = 1024;
			if (bytes < unit)
				return bytes + " B";
			int exp = (int)(Math.Log (bytes) / Math.Log (unit));
			return String.Format ("{0:F1} {1}B", bytes / Math.Pow (unit, exp), "KMGTPE" [exp - 1]);
		}

		public static int GetBitsPerPixel(TextureFormat format)
		{
			switch (format)
			{
				case TextureFormat.Alpha8: // Alpha-only texture format.
					return 8;
				case TextureFormat.ARGB4444: // A 16 bits/pixel texture format. Texture stores color with an alpha channel.
					return 16;
				case TextureFormat.RGBA4444: // A 16 bits/pixel texture format.
					return 16;
				case TextureFormat.RGB24: // A color texture format.
					return 24;
				case TextureFormat.RGBA32: // Color with an alpha channel texture format.
					return 32;
				case TextureFormat.ARGB32: // Color with an alpha channel texture format.
					return 32;
				case TextureFormat.RGB565: // A 16 bit color texture format.
					return 16;
				case TextureFormat.DXT1: // Compressed color texture format.
					return 4;
				case TextureFormat.DXT5: // Compressed color with alpha channel texture format.
					return 8;
				case TextureFormat.PVRTC_RGB2: // PowerVR (iOS) 2 bits/pixel compressed color texture format.
					return 2;
				case TextureFormat.PVRTC_RGBA2: // PowerVR (iOS) 2 bits/pixel compressed with alpha channel texture format
					return 2;
				case TextureFormat.PVRTC_RGB4: // PowerVR (iOS) 4 bits/pixel compressed color texture format.
					return 4;
				case TextureFormat.PVRTC_RGBA4: // PowerVR (iOS) 4 bits/pixel compressed with alpha channel texture format
					return 4;
				case TextureFormat.ETC_RGB4: // ETC (GLES2.0) 4 bits/pixel compressed RGB texture format.
					return 4;
				case TextureFormat.ETC2_RGBA8: // ETC (GLES2.0) 8 bits/pixel compressed RGB texture format.
					return 8;
				case TextureFormat.BGRA32: // Format returned by iPhone camera
					return 32;
				default:
					return 0;
			}
		}

		public static int CalculateTextureSizeBytes (Texture tTexture)
		{
			int tWidth = tTexture.width;
			int tHeight = tTexture.height;
			if (tTexture is Texture2D) {
				Texture2D tTex2D = tTexture as Texture2D;
				int bitsPerPixel = GetBitsPerPixel (tTex2D.format);
				int mipMapCount = tTex2D.mipmapCount;
				int mipLevel = 1;
				int tSize = 0;
				while (mipLevel<=mipMapCount) {
					tSize += tWidth * tHeight * bitsPerPixel / 8;
					tWidth = tWidth / 2;
					tHeight = tHeight / 2;
					mipLevel++;
				}
				return tSize;
			}
			
			if (tTexture is Cubemap) {
				Cubemap tCubemap = tTexture as Cubemap;
				int bitsPerPixel = GetBitsPerPixel (tCubemap.format);
				return tWidth * tHeight * 6 * bitsPerPixel / 8;
			}
			return 0;
		}

		[MenuItem ("Assets/UI/Atlas依赖检查")]
		static public void CheckAtlasRefrence ()
		{
			var obj = Selection.activeObject;
			if (obj is GameObject) {
				var atlas = ((GameObject)obj).GetComponent<UIAtlas> ();
				var atlasPath = AssetDatabase.GetAssetPath (obj);
				if (atlas != null) {
					Debug.LogFormat (obj, "CheckAtlasReferences: {0}", atlasPath);
//					var res = "";
					var count = 0;
					var ui_prefab_root = Path.Combine (Application.dataPath, "Resources/GUI/v1");
					var appRoot = Application.dataPath;
					string filename = null;
					foreach (string f in Directory.GetFiles(ui_prefab_root, "*.prefab", SearchOption.AllDirectories)) {
						filename = "Assets" + f.Replace (appRoot, "");
						if (isDependedOn (filename, atlasPath)) {
							var refObj = AssetDatabase.LoadAssetAtPath<GameObject> (filename);
//							res += filename + "\n";
							Debug.LogFormat (refObj, "<color=green>{0}</color>", filename);
							count++;
						}
					}
					Debug.Log (string.Format ("atlas:{0} 引用的prefab count:{1}", Path.GetFileNameWithoutExtension (atlasPath), count));
				}
			}
		}

		static bool isDependedOn (string fileName, string checkFile)
		{
			string[] depends = AssetDatabase.GetDependencies (new string[]{fileName});
			if (depends != null) {
				for (int i=0; i<depends.Length; i++) {
					if (depends [i].Contains (checkFile)) {
						return true;
					}
				}
			}
			return false;
		}

		[MenuItem ("Assets/UI/prefab检查atlas")]
		static public void CheckPrefabAtlas ()
		{
			var obj = Selection.activeObject;
			if (obj is GameObject) {
				Component[] sps = GameObjectUtil.Instance.GetAllComponentsInChildren ((GameObject)obj, typeof(UISprite));
				if (sps != null) {
					for (int i=0; i<sps.Length; i++) {
						UISprite sp = (UISprite)sps [i];
						Debug.Log (string.Format ("sprite <color=green>GO</color>:{0} , <color=green>atlas</color>:{1} <color=green>spriteName</color>:{2}", sp.gameObject.name, sp.atlas.name, sp.spriteName));
					}
				}
			}
		}
	}
} // namespace EditorTool
