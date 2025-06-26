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
		public static string GetMountModlePath(LuaTable info)
		{
			var path = PathUnity.Combine(LuaWorker.GetFieldString(info, "ModelDir"), LuaWorker.GetFieldString(info, "ModelName"));
			path.TrimStart('/');
			return path;
		}

		public static Dictionary<int, LuaTable> GetMountInfos(out WorkerContainer workerContainer)
		{
			return GetAssetsInfo_1(out workerContainer, "Mount", GetMountModlePath, "Table_Mount");
		}

		[MenuItem( "RO/GenAssets/Mounts", false, 104)]
		public static void GenMounts()
		{
			WorkerContainer workerContainer;
			var infos = GetMountInfos(out workerContainer);
			using(workerContainer)
			{
				if (null == infos)
				{
					return;
				}

				var totalCount = infos.Count;
				if (0 == totalCount)
				{
					Debug.LogFormat("No Mounts");
					return;
				}

				var assetInfo = AssetInfo.GetInstance("Mount");
				try
				{
					assetInfo.Begin();

					WorkerContainer actionWorkerContainer;
					var actionInfos = GetActionInfos(out actionWorkerContainer);
					if (!actionWorkerContainer.workers.IsNullOrEmpty())
					{
						ArrayUtility.AddRange(ref workerContainer.workers, actionWorkerContainer.workers);
					}

					var unhandledAssets = GetUnhandledAssets<GameObject>(AssetsFolder_Mount, "prefab");

					var audioSourceModel = AssetDatabase.LoadAssetAtPath<AudioSource>(Path.ChangeExtension(AssetsPath_RoleComplete, PrefabExtension));
					
					var i = 0;
					foreach (var key_value in infos)
					{
						++i;
						var ID = key_value.Key;
						var info = key_value.Value;
						LuaWorker.GetFieldString(info, "NameEn");
						var assetName = ID.ToString();
						
						ShowProgress("Mounts", i, totalCount, assetName);
						
						// 1.
						var srcFolder = PathUnity.Combine(AssetsSrcFolder_Role, LuaWorker.GetFieldString(info, "ModelDir"));
						var srcName = LuaWorker.GetFieldString(info, "ModelName");
						var clipFolders = new string[]{LuaWorker.GetFieldString(info, "Anime")};
						if (!string.IsNullOrEmpty(clipFolders[0]))
						{
							clipFolders[0] = PathUnity.Combine(srcFolder, clipFolders[0]);
						}
						else
						{
							clipFolders[0] = srcFolder;
						}
						var targetPath = Path.ChangeExtension(PathUnity.Combine(AssetsFolder_Mount, assetName), PrefabExtension);
						
						bool needAudioSource;
						var asset = GenRolePrefabsAndControllerAndSE(
							assetInfo,
							srcFolder,
							srcName, 
							clipFolders, 
							targetPath, 
							info,
							actionInfos, 
							"MountActionB", 
							RoleAnimatorDefaultState,
							out needAudioSource);
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
//						SetMaterial(asset, srcFolder, matName);
						
						var rolePart = asset.GetComponent<RolePartMount>();
						if (null == rolePart)
						{
							rolePart = asset.AddComponent<RolePartMount>();
						}
						rolePart.audioEffects = new string[]{
							LuaWorker.GetFieldString(info, "StandSE"),
							LuaWorker.GetFieldString(info, "WalkSE"),
							LuaWorker.GetFieldString(info, "AttackSE"),
							LuaWorker.GetFieldString(info, "BeHitSE"),
							LuaWorker.GetFieldString(info, "DieSE")
						};
						
						if (needAudioSource)
						{
							rolePart.audioSource = SetAudioSource(asset, audioSourceModel);
						}
						else
						{
							var audioSource = asset.GetComponent<AudioSource>();
							if (null != audioSource)
							{
								Component.DestroyImmediate(audioSource, true);
							}
							rolePart.audioSource = null;
						}

						InitRolePart(rolePart, srcFolder, srcName, matName);
						
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
