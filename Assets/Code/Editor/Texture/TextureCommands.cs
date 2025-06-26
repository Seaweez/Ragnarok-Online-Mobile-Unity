using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Ghost.Utils;
using Ghost.Extensions;

namespace EditorTool
{
	public static class TextureCommands 
	{
		public static void DoEnableMipMap(Texture t, TextureImporter importer, bool reimport )
		{
			if (null != importer)
			{
				if (importer.mipmapEnabled)
				{
					return;
				}
				if (TextureInfo.Ignored(t, importer.assetPath))
				{
					return;
				}
				importer.mipmapEnabled = true;
				if (null != t)
				{
					Debug.LogFormat(t, "[Enable MipMap]\n{0}", importer.assetPath);
				}
				else
				{
					Debug.LogFormat("[Enable MipMap]\n{0}", importer.assetPath);
				}
				if (reimport)
				{
					importer.SaveAndReimport();
				}
			}
		}

		public static void DoEnableMipMap(Texture t, TextureImporter importer)
		{
			DoEnableMipMap(t, importer, true);
		}

		public static void DoDisableMipMap(Texture t, TextureImporter importer, bool reimport)
		{
			if (null != importer)
			{
				if (!importer.mipmapEnabled)
				{
					return;
				}
				importer.mipmapEnabled = false;
				if (null != t)
				{
					Debug.LogFormat(t, "[Disable MipMap]\n{0}", importer.assetPath);
				}
				else
				{
					Debug.LogFormat("[Disable MipMap]\n{0}", importer.assetPath);
				}
				if (reimport)
				{
					importer.SaveAndReimport();
				}
			}
		}

		public static void DoDisableMipMap(Texture t, TextureImporter importer)
		{
			DoDisableMipMap(t, importer, true);
		}

		public static void DoChangeTextureType(Texture t, TextureImporter importer,TextureImporterType type,bool reimport)
		{
			if (null != importer)
			{
				if (importer.textureType == type)
				{
					return;
				}
				importer.textureType = type;
				if (null != t)
				{
					Debug.LogFormat(t, "[Change TextureType]  {1}\n{0}", importer.assetPath,type);
				}
				else
				{
					Debug.LogFormat("[Change TextureType]  {1}\n{0}", importer.assetPath,type);
				}
				if (reimport)
				{
					importer.SaveAndReimport();
				}
			}
		}

		public static void DoChangeTextureType(Texture t, TextureImporter importer,TextureImporterType type)
		{
			DoChangeTextureType (t, importer, type , true);
		}

		public static void DoChangeTextureTypeToGUI(Texture t,TextureImporter importer)
		{
			DoChangeTextureTypeToGUI (t, importer, true);
		}

		public static void DoChangeTextureTypeToGUI(Texture t,TextureImporter importer,bool reimport)
		{
			DoChangeTextureType (t, importer, TextureImporterType.GUI, reimport);
		}


		public static void DoTruColor2Compressed(Texture t, TextureImporter importer)
		{
			if (null != importer)
			{
				if (TextureImporterCompression.Uncompressed != importer.textureCompression)
				{
					return;
				}
				importer.textureCompression = TextureImporterCompression.Compressed;
				
				if (null != t)
				{
					Debug.LogFormat(t, "[TruColor -> Compressed]\n{0}", importer.assetPath);
				}
				else
				{
					Debug.LogFormat("[TruColor -> Compressed]\n{0}", importer.assetPath);
				}
				importer.SaveAndReimport();
			}
		}

		public static void DoCompressed2TruColor(Texture t, TextureImporter importer)
		{
			if (null != importer)
			{
				if (TextureImporterCompression.Compressed != importer.textureCompression)
				{
					return;
				}
				importer.textureCompression = TextureImporterCompression.Uncompressed;
				
				if (null != t)
				{
					Debug.LogFormat(t, "[Compressed -> TruColor]\n{0}", importer.assetPath);
				}
				else
				{
					Debug.LogFormat("[Compressed -> TruColor]\n{0}", importer.assetPath);
				}
				importer.SaveAndReimport();
			}
		}

		public static void DoCheck(Texture t, TextureImporter importer)
		{
			if (null == t)
			{
				return;
			}
			if (null != importer)
			{
				bool hasAlpha = importer.DoesSourceTextureHaveAlpha();
				var hasAlphaStr = hasAlpha ? "<color=red>{0}</color>" : "<color=green>{0}</color>";
				var formatStr = (TextureImporterCompression.Compressed == importer.textureCompression) ? "<color=green>{0}</color>" : "<color=red>{0}</color>";
				var mipMapStr = importer.mipmapEnabled ? "<color=yellow>{0}</color>" : "<color=blue>{0}</color>";
				var texelSize = new Vector2(t.width, t.height);
				var texelSizeStr = (256 <= texelSize.x || 256 <= texelSize.y) ? "<color=yellow>{0} in {1}</color>" : "<color=green>{0} in {1}</color>";
				Debug.LogFormat(t, "[Check] hasAlpha={1} format={2}, mipmap={3}, texelSize={4}\n{0}", 
				                importer.assetPath, 
				                string.Format(hasAlphaStr, hasAlpha),
				                string.Format(formatStr, importer.textureCompression), 
				                string.Format(mipMapStr, importer.mipmapEnabled),
				                string.Format(texelSizeStr, texelSize, importer.maxTextureSize));
			}
		}

		public static void DoSplitAlpha(Texture t, TextureImporter importer)
		{
			if (null == t || !importer.DoesSourceTextureHaveAlpha() || !importer.isReadable)
			{
				return;
			}
			var tex = t as Texture2D;
			if (null == tex)
			{
				return;
			}

			Texture2D texRGB = new Texture2D(tex.width, tex.height, TextureFormat.RGB24, importer.mipmapEnabled);
			Texture2D texAlpha = new Texture2D(tex.width, tex.height, TextureFormat.RGB24, importer.mipmapEnabled);

			var colors = tex.GetPixels();
			texRGB.SetPixels(colors);

			var alphaColors = new Color[colors.Length];
			for (int i = 0; colors.Length > i; ++i)
			{
				alphaColors[i].r = colors[i].a;
				alphaColors[i].g = colors[i].a;
				alphaColors[i].b = colors[i].a;
			}
			texAlpha.SetPixels(alphaColors);
			
			texRGB.Apply();
			texAlpha.Apply();

			var fullPath = importer.assetPath;
			Path.GetFileNameWithoutExtension(fullPath);
			var folder = Path.GetDirectoryName(fullPath);

			var name = tex.name;
			texRGB.name = name + "_rgb";
			texAlpha.name = name + "_a";

			var path = Path.ChangeExtension(PathUnity.Combine(folder, texRGB.name), "png");
			File.WriteAllBytes(path, texRGB.EncodeToPNG());
//			AssetDatabase.CreateAsset(texRGB, path);

			path = Path.ChangeExtension(PathUnity.Combine(folder, texAlpha.name), "png");
			File.WriteAllBytes(path, texAlpha.EncodeToPNG());
			AssetDatabase.Refresh ();
//			AssetDatabase.CreateAsset(texAlpha, path);
			Debug.LogFormat(t, "[SplitAlpha]\n{0}", importer.assetPath);
		}

		public static void DoTexture(Object selectionObj, System.Action<Texture, TextureImporter> doFunc)
		{
			var path = AssetDatabase.GetAssetPath(selectionObj);
			if (AssetDatabase.IsValidFolder(path))
			{
				var filterString = StringUtils.ConnectToString("t:", typeof(Texture).Name);
				var guids = AssetDatabase.FindAssets(filterString, new string[]{path}); 
				
				if (guids.IsNullOrEmpty())
				{
					return;
				}

				var totalCount = guids.Length;
				try
				{
					int i = 0;
					foreach (var guid in guids)
					{
						var objPath = AssetDatabase.GUIDToAssetPath(guid);
						var obj = AssetDatabase.LoadAssetAtPath<Texture>(objPath);

						++i;
						EditorUtility.DisplayProgressBar(
							string.Format("Proccessing Folder: {0}", path), 
							string.Format("{0}/{1}, {2}", i, totalCount, obj.name), 
							(float)i/totalCount);
						doFunc(obj, TextureImporter.GetAtPath(objPath) as TextureImporter);
					}
				}
				finally
				{
					EditorUtility.ClearProgressBar();
				}

			}
			else
			{
				doFunc(selectionObj as Texture, TextureImporter.GetAtPath(path) as TextureImporter);
			}
		}

		public static void DoTextures(System.Action<Texture, TextureImporter> doFunc)
		{
			if (Selection.objects.IsNullOrEmpty())
			{
				return;
			}
			foreach (var obj in Selection.objects)
			{
				DoTexture(obj, doFunc);
			}
			AssetDatabase.SaveAssets();
			Debug.LogFormat("[DoTextures Finished]");
		}

		[MenuItem("Assets/Texture/Check")]
		static void Check()
		{
			DoTextures(DoCheck);
		}

		[MenuItem("Assets/Texture/EnableMipMap")]
		static void EnableMipMap()
		{
			DoTextures(DoEnableMipMap);
		}

		[MenuItem("Assets/Texture/DisableMipMap")]
		static void DisableMipMap()
		{
			DoTextures(DoDisableMipMap);
		}

		[MenuItem("Assets/Texture/TruColor->Compressed")]
		static void TruColor2Compressed()
		{
			DoTextures(DoTruColor2Compressed);
		}

		[MenuItem("Assets/Texture/Compressed->TruColor")]
		static void Compressed2TruColor()
		{
			DoTextures(DoCompressed2TruColor);
		}
		
		[MenuItem("Assets/Texture/SplitAlpha")]
		static void SplitAlpha()
		{
			DoTextures(DoSplitAlpha);
		}

		#region batch processing


		[MenuItem("RO/Texture/CreateBatchProcessingInfo")]
		static void CreateBatchProcessingInfo()
		{
			var info = AssetDatabase.LoadAssetAtPath<TextureInfo>(TextureInfo.BatchProcessingInfoPath);
			if (null == info)
			{
				info = ScriptableObject.CreateInstance<TextureInfo>();
				AssetDatabase.CreateAsset(info, TextureInfo.BatchProcessingInfoPath);
			}
			Selection.activeObject = info;
		}

		[MenuItem("RO/Texture/BatchProcess")]
		static void BatchProcess()
		{
			BatchProcess_MipMap();
			BatchProcess_GUI ();
		}

		public static void BatchProcess_MipMap()
		{
			if (null == TextureInfo.Global || TextureInfo.Global.mipMapFolders.IsNullOrEmpty())
			{
				return;
			}
			foreach (var folder in TextureInfo.Global.mipMapFolders)
			{
				DoTexture(folder, DoEnableMipMap);
			}
		}

		[MenuItem("RO/Texture/BatchProcess_GUI")]
		static void BatchProcessGUI()
		{
			BatchProcess_GUI();
		}

		public static void BatchProcess_GUI()
		{
			if (null == TextureInfo.Global || TextureInfo.Global.guiTextureTypeFolders.IsNullOrEmpty())
			{
				return;
			}
			foreach (var folder in TextureInfo.Global.guiTextureTypeFolders)
			{
				DoTexture(folder, DoChangeTextureTypeToGUI);
			}
		}

		#endregion batch processing
	}
} // namespace EditorTool
