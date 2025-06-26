using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using SLua;
using Ghost.Utils;
using Ghost.Extensions;
using RO;
using RO.Config;

namespace EditorTool
{
	public static partial class GenAssets 
	{
		public static string GetStagePartModlePath(LuaTable info)
		{
			var path = PathUnity.Combine(LuaWorker.GetFieldString(info, "ModelDir"), LuaWorker.GetFieldString(info, "ModelName"));
			path.TrimStart('/');
			return path;
		}

		public static Dictionary<int, LuaTable> GetStagePartObjectInfos(out WorkerContainer workerContainer)
		{
			return GetAssetsInfo_1(out workerContainer, "StagePart", GetStagePartModlePath, "Table_StageParts");
		}

		[MenuItem( "RO/GenAssets/StageParts", false, 1004)]
		public static void GenStagePartObjects()
		{
			WorkerContainer workerContainer;
			var infos = GetStagePartObjectInfos(out workerContainer);
			using(workerContainer)
			{
				if (null == infos)
				{
					return;
				}

				var totalCount = infos.Count;
				if (0 == totalCount)
				{
					Debug.LogFormat("No StagePart");
					return;
				}

				var assetInfo = AssetInfo.GetInstance("StagePart");
				try
				{
					assetInfo.Begin();

					var unhandledAssets = GetUnhandledAssets<GameObject>(AssetsFolder_Stage, "prefab");

					var i = 0;
					foreach (var key_value in infos)
					{
						++i;
						var ID = key_value.Key;
						var info = key_value.Value;
						LuaWorker.GetFieldString(info, "NameEn");
						var assetName = ID.ToString();

						ShowProgress("StagePart", i, totalCount, assetName);

						// 1.
						var srcFolder = PathUnity.Combine(AssetsSrcFolder_Model, LuaWorker.GetFieldString(info, "ModelDir"));
						var srcName = LuaWorker.GetFieldString(info, "ModelName");
						var clipFolders = new string[]{srcFolder}; 
						if (!Directory.Exists(AssetsFolder_Stage))
						{
							Directory.CreateDirectory(AssetsFolder_Stage);
						}
						var targetPath = Path.ChangeExtension(PathUnity.Combine(AssetsFolder_Stage, assetName), PrefabExtension);

						var asset = GenPrefabsAndController(
							assetInfo,
							srcFolder, 
							srcName, 
							clipFolders, 
							targetPath, 
							info,
							null, null, 
							new string[]{"wait"});
						if (null == asset)
						{
							continue;
						}
						var assetObj = asset.gameObject;

						// 2.
						if (!unhandledAssets.Remove(assetName))
						{
							Debug.LogFormat("<color=green>New Created: </color>{0}", asset.name);
						}

						asset.name = assetName;
						var matName = LuaWorker.GetFieldString(info, "Texture");
						if (string.IsNullOrEmpty(matName))
						{
							matName = srcName;
						}
						SetMaterial(asset, srcFolder, matName);

						var animeName = LuaWorker.GetFieldString(info,"Anime");
						if(animeName != "")
						{
							var animatorPlayer = asset.GetComponent<SimpleAnimatorPlayer>();
							if (null == animatorPlayer)
							{
								animatorPlayer = asset.AddComponent<SimpleAnimatorPlayer>();
							}
						}

						if (null != asset)
						{
							EditorUtility.SetDirty(asset);
						}
						else if (null != assetObj)
						{
							EditorUtility.SetDirty(assetObj);
						}
					}

					if (!workerContainer.debug)
					{
						foreach (var asset in unhandledAssets.Values)
						{
							if (null != asset)
							{
								Debug.LogFormat("<color=red>Deleted: </color>{0}", asset.name);
								AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(asset));
							}
						}
					}
				}
				finally
				{
					try
					{
						assetInfo.End();
					}
					catch(System.Exception)
					{
					}
					EditorUtility.ClearProgressBar();
					AssetDatabase.SaveAssets();
				}
			}
		}
	}
} // namespace EditorTool
