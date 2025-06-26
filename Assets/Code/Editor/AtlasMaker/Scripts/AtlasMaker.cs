using System;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Collections.Generic;

namespace EditorTool
{
	public class AtlasMaker
	{
		public AtlasMaker ()
		{
		}

		public static void MakeAtlas (AtlasData data)
		{
			List<Texture> _TexList = CollectTexture (data);
			Make (data, _TexList);
		}

		public static List<Texture> CollectTexture (AtlasData data)
		{
			EditorUtility.DisplayProgressBar ("Collect Texture", "please wait...", 0);

			List<Texture> _TexList = new List<Texture> ();
			if (data.resourcePathList.Count > 0) {
				for (int i = 0; i < data.resourcePathList.Count; i++) {
					string[] pngs = Directory.GetFiles (data.resourcePathList [i], "*.png");
					for (int k = 0; k < pngs.Length; k++) {
						Texture tex = AssetDatabase.LoadAssetAtPath (pngs [k], typeof(Texture)) as Texture;
						if (tex != null)
							_TexList.Add (tex);

						EditorUtility.DisplayProgressBar ("Collect Texture", "please wait...", ((i * k) / (float)(data.resourcePathList.Count * pngs.Length)));
					}
				}
				if (_TexList.Count <= 0) {
					AtlasInspectorWindow.window.ShowNotification ("目录下没有图片任何！");
					return null;
				}
				return _TexList;
			} else {
				AtlasInspectorWindow.window.ShowNotification ("请选择图片");
				return null;
			}
			EditorUtility.ClearProgressBar ();
		}

		private static void Make (AtlasData data, List<Texture> texList)
		{
			if (!string.IsNullOrEmpty (data.atlasPath)) {
				if (_TryCreateDirector (data.targetPfbPath)
				    || _TryCreateDirector (data.targetMatPath)
				    || _TryCreateDirector (data.targetTexPath)) {
					AssetDatabase.SaveAssets ();
					AssetDatabase.Refresh ();
				}

				CreateOrUpdateAtlas (data, texList);

				Platform (data.targetTexPath, (int)TextureImporterFormat.RGBA32, "Default", data.maxSize);
				Platform (data.targetTexPath, (int)TextureImporterFormat.RGBA32, "Standalone", data.maxSize);
				Platform (data.targetTexPath, data.androidFormat, "Android", data.maxSize);
				Platform (data.targetTexPath, data.iosFormat, "iPhone", data.maxSize);

				AtlasInspectorWindow.window.ShowNotification ("图集制作成功！");
				AssetDatabase.SaveAssets ();
				AssetDatabase.Refresh ();
			} else
				AtlasInspectorWindow.window.ShowNotification ("导出目录为空！");
			Resources.UnloadUnusedAssets ();
		}

		static bool _TryCreateDirector (string path)
		{
			path = Path.GetDirectoryName (path);
			DirectoryInfo directory = new DirectoryInfo (path);
			if (!directory.Exists) {
				Directory.CreateDirectory (path);
				return true;
			}
			return false;
		}

		static void CreateOrUpdateAtlas (AtlasData data, List<Texture> texList)
		{
			// atlas Material
			Material mat = AssetDatabase.LoadAssetAtPath (data.targetMatPath, typeof(Material)) as Material;
			if (mat == null) {
				Shader shader = Shader.Find ("Unlit/Transparent Colored");
				mat = new Material (shader);
				AssetDatabase.CreateAsset (mat, data.targetMatPath);
				AssetDatabase.Refresh (ImportAssetOptions.ForceSynchronousImport);
				mat = AssetDatabase.LoadAssetAtPath (data.targetMatPath, typeof(Material)) as Material;
			}

			// atlas prefab
			GameObject atlasGO = AssetDatabase.LoadAssetAtPath (data.targetPfbPath, typeof(GameObject)) as GameObject;
			UIAtlas atlas = null;
			if (atlasGO == null) {
				UnityEngine.Object prefab = PrefabUtility.CreateEmptyPrefab (data.targetPfbPath);

				atlasGO = new GameObject (data.atlasName);
				atlasGO.AddComponent<UIAtlas> ().spriteMaterial = mat;
				PrefabUtility.ReplacePrefab (atlasGO, prefab);
				GameObject.DestroyImmediate (atlasGO);

				AssetDatabase.SaveAssets ();
				AssetDatabase.Refresh (ImportAssetOptions.ForceSynchronousImport);

				atlasGO = AssetDatabase.LoadAssetAtPath (data.targetPfbPath, typeof(GameObject)) as GameObject;
				atlas = atlasGO.GetComponent<UIAtlas> ();
			} else {
				atlas = atlasGO.GetComponent<UIAtlas> ();
			}

			// atlas texture
			if (atlas.texture != null) {
				string texPath = AssetDatabase.GetAssetPath (atlas.texture.GetInstanceID ());
				if (texPath != data.targetTexPath) {
					System.IO.File.Delete (texPath);
					AssetDatabase.SaveAssets ();
					AssetDatabase.Refresh (ImportAssetOptions.ForceSynchronousImport);

					// refresh
					atlasGO = AssetDatabase.LoadAssetAtPath (data.targetPfbPath, typeof(GameObject)) as GameObject;
					atlas = atlasGO.GetComponent<UIAtlas> ();
				}
			}

			NGUISettings.atlas = atlas;

			try {
				if (data.forceSquare) {
					NGUISettings.unityPacking = false;
					NGUISettings.forceSquareAtlas = true;

					UIAtlasMaker.UpdateAtlas (texList, false, data.targetTexPath, data.maxSize);
				} else {
					NGUISettings.unityPacking = true;
					UIAtlasMaker.UpdateAtlas (texList, false, data.targetTexPath, data.maxSize);
				}
			} catch (System.Exception ex) {
				Debug.Log (string.Format ("<color=red>{0}</color>", ex.ToString ()));
			}
		}


		static void RevertToARGB32 (string path)
		{
			TextureImporter texImp = TextureImporter.GetAtPath (path) as TextureImporter;
			if (texImp != null) {
				texImp.ClearPlatformTextureSettings ("Android");
				texImp.ClearPlatformTextureSettings ("iPhone");

				TextureImporterSettings tis = new TextureImporterSettings ();
				TextureImporterPlatformSettings tps = new TextureImporterPlatformSettings ();
				tps.format = TextureImporterFormat.ARGB32;
				texImp.SetPlatformTextureSettings (tps);
				texImp.SetTextureSettings (tis);
			}
		}

		public static void  Platform (string path, int teximporter, string platform, int maxSize)
		{
			TextureImporter textureImporter = TextureImporter.GetAtPath (path) as TextureImporter;
			if (textureImporter) {
				TextureImporterPlatformSettings platSettings = textureImporter.GetPlatformTextureSettings (platform);
				if (teximporter == (int)TextureImporterFormat.AutomaticCompressed) {
					if (platform == "Android") {
						platSettings.format = TextureImporterFormat.ETC2_RGBA8;
					} else if (platform == "iPhone") {
						platSettings.format = TextureImporterFormat.PVRTC_RGBA2;
					}
				} else {
					platSettings.format = (TextureImporterFormat)teximporter;
				}
				platSettings.overridden = true;
				platSettings.maxTextureSize = maxSize;
				textureImporter.SetPlatformTextureSettings (platSettings);
				textureImporter.SaveAndReimport ();
			}
		}
	}
}

