using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using Ghost.Config;
using Ghost.Extensions;
using Ghost.Utils;
using RO;

namespace EditorTool
{
	public class ImportModelEditor : AssetPostprocessor {

		private static List<string> importedAssets = new List<string>();
		private static List<string> importedAssets1 = new List<string>();

		private static readonly Vector3 ACTION_DEFAULT_ERROR = new Vector3(8f,0.5f,8f);
		private static Dictionary<string, Vector3> actionErrors;
		static ImportModelEditor()
		{
			actionErrors = new Dictionary<string, Vector3>();
//			actionErrors.Add("attack_wait", new Vector3(8f,0.5f,8f));
		}

		private static Vector3 GetActionError(string action)
		{
			Vector3 error;
			if (!actionErrors.TryGetValue(action, out error))
			{
				error = ACTION_DEFAULT_ERROR;
			}
			return error;
		}

		private void ImportModelStep1(GameObject input)
		{
			var assetDirectory = Path.GetDirectoryName(assetPath);
			var materialsDirectory = PathUnity.Combine(assetDirectory, PathConfig.DIRECTORY_MATERIALS);
			if (!Directory.Exists(materialsDirectory))
			{
				return;
			}

			var files = Directory.GetFiles(materialsDirectory, StringUtils.ConnectToString("*.", PathConfig.EXTENSION_MATERIAL), SearchOption.AllDirectories);
			if (!files.IsNullOrEmpty())
			{
				foreach (var file in files)
				{
					var oldMateriaFile = PathUnity.Combine(assetDirectory, Path.GetFileName(file));
					if (File.Exists(oldMateriaFile))
					{
						AssetDatabase.DeleteAsset(oldMateriaFile);
					}
				}
			}

			importedAssets.Add(assetPath);
			var importer = assetImporter as ModelImporter;
			importer.SaveAndReimport();
//			AssetDatabase.ImportAsset(assetPath);
		}

		private void ImportModelStep2(GameObject input)
		{
			var assetDirectory = Path.GetDirectoryName(assetPath);
			var materialsDirectory = PathUnity.Combine(assetDirectory, PathConfig.DIRECTORY_MATERIALS);
			if (!Directory.Exists(materialsDirectory))
			{
				return;
			}

			var files = Directory.GetFiles(materialsDirectory, StringUtils.ConnectToString("*.", PathConfig.EXTENSION_MATERIAL), SearchOption.AllDirectories);
			if (!files.IsNullOrEmpty())
			{
				foreach (var file in files)
				{
					var material = AssetDatabase.LoadAssetAtPath(file, typeof(Material)) as Material;
					if (null != material)
					{
						var testFiles = Directory.GetFiles(assetDirectory, StringUtils.ConnectToString(Path.GetFileNameWithoutExtension(file), ".*"));
						foreach (var testFile in testFiles)
						{
							var texture = AssetDatabase.LoadAssetAtPath(testFile, typeof(Texture)) as Texture;
							if (null != texture)
							{
								material.mainTexture = texture;
								break;
							}
						}
					}

					var newFile = PathUnity.Combine(assetDirectory, Path.GetFileName(file));
					AssetDatabase.MoveAsset(file, newFile);
				}
			}
			
			importedAssets.Remove(assetPath);
			Directory.Delete(materialsDirectory, true);

			AssetDatabase.Refresh();
		}

		private void OptimizeModel(GameObject obj)
		{
			// ****** can't use this optimization, because the avatar can't match all input bones

			var importer = assetImporter as ModelImporter;
			//importer.tangentImportMode = ModelImporterTangentSpaceMode.None;
			importer.importTangents = ModelImporterTangents.None;
			importer.optimizeMesh = true;
			importer.optimizeGameObjects = true;

			var pointNames = new HashSet<string>();
			var points = GameObjectHelper.GetPoints(obj, "EP_");
			if (!points.IsNullOrEmpty())
			{
				foreach (var p in points)
				{
					if (null != p)
					{
						pointNames.Add(p.name);
					}
				}
			}
			points = GameObjectHelper.GetPoints(obj, "CP_");
			if (!points.IsNullOrEmpty())
			{
				foreach (var p in points)
				{
					if (null != p)
					{
						pointNames.Add(p.name);
					}
				}
			}
			var pointInfos = GameObjectHelper.GetPointsWithID(obj, "ECP_");
			if (!pointInfos.IsNullOrEmpty())
			{
				foreach (var info in pointInfos)
				{
					pointNames.Add(info.transform.name);
				}
			}

			importer.extraExposedTransformPaths = pointNames.ToArray();
			importer.SaveAndReimport();
		}

		void OnPreprocessModel () 
		{
			if (assetPath.StartsWith(PathUnity.Combine(PathConfig.DIRECTORY_ASSETS, PathConfig.DIRECTORY_STANDARD_ASSETS)))
			{
				return;
			}
			var importer = assetImporter as ModelImporter;
			importer.globalScale = 1;
			importer.materialName = ModelImporterMaterialName.BasedOnTextureName;
			importer.materialSearch = ModelImporterMaterialSearch.Everywhere;

			if (assetPath.Contains("@"))
			{
				// animation
				importer.animationCompression = ModelImporterAnimationCompression.Optimal;

//				var error = ACTION_DEFAULT_ERROR;
//				var match = Regex.Match(assetPath, @"@[\w\W]*\.");
//				if (match.Success)
//				{
//					var actionName = match.Value.TrimStart('@').TrimEnd('.');
//					error = GetActionError(actionName);
//				}
//				importer.animationPositionError = error.x;
//				importer.animationRotationError = error.y;
//				importer.animationScaleError = error.z;
			}
		}

		void OnPostprocessModel( GameObject input )
		{
			if (assetPath.StartsWith(PathUnity.Combine(PathConfig.DIRECTORY_ASSETS, PathConfig.DIRECTORY_STANDARD_ASSETS)))
			{
				return;
			}
			if (assetPath.Contains("@"))
			{
				// animation
				var assetDirectory = Path.GetDirectoryName(assetPath);

				var materialsDirectory = PathUnity.Combine(assetDirectory, PathConfig.DIRECTORY_MATERIALS);
				if (Directory.Exists(materialsDirectory))
				{
					Directory.Delete(materialsDirectory, true);
				}

				AssetDatabase.Refresh();
			}
			else
			{
				if (!importedAssets1.Remove(assetPath))
				{
					if (null != AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Art/AutoOptimizeModel.txt"))
					{
						importedAssets1.Add(assetPath);
						OptimizeModel(input);
						return;
					}
				}
				if (!importedAssets.Contains(assetPath))
				{
					ImportModelStep1(input);
//					if (!importedAssets.Contains(assetPath))
//					{
//						AssetInfo.Instance.AddReimportedAsset(assetPath);
//					}
				}
				else
				{
					ImportModelStep2(input);
//					AssetInfo.Instance.AddReimportedAsset(assetPath);
				}
			}
		}

		void OnPreprocessTexture () 
		{
			if (TextureInfo.InMipMapFolder(assetPath))
			{
				var textureImporter  = (TextureImporter) assetImporter;
				TextureCommands.DoEnableMipMap(null, textureImporter, false);
			}

			if (TextureInfo.InGUITypeFolder (assetPath)) {
				var textureImporter  = (TextureImporter) assetImporter;
				TextureCommands.DoChangeTextureTypeToGUI(null, textureImporter, false);
			}

//			if (assetPath.StartsWith("Assets/Art/"))
//			{
//				var textureImporter  = (TextureImporter) assetImporter;
//				if (assetPath.StartsWith("Assets/Art/Public/Texture/GUI/"))
//				{
//					textureImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
//				}
//				else
//				{
//					textureImporter.textureFormat = TextureImporterFormat.AutomaticCompressed;
//				}
//			}
		}
		
	}
} // namespace EditorTool
