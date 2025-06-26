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
		public static string GetEyeModlePath(LuaTable info)
		{
			var path = PathUnity.Combine(LuaWorker.GetFieldString(info, "ModelDir"), LuaWorker.GetFieldString(info, "Model"));
			path.TrimStart('/');
			return path;
		}

		public static Dictionary<int, LuaTable> GetEyeInfos(out WorkerContainer workerContainer)
		{
			return GetAssetsInfo_1(out workerContainer, "Eye", GetEyeModlePath, "Table_Eye");
		}

		[MenuItem( "RO/GenAssets/Eyes", false, 103)]
		public static void GenEyes()
		{
			WorkerContainer workerContainer;
			var infos = GetEyeInfos(out workerContainer);
			using(workerContainer)
			{
				if (null == infos)
				{
					return;
				}

				var totalCount = infos.Count;
				if (0 == totalCount)
				{
					Debug.LogFormat("No Eyes");
					return;
				}

				var assetInfo = AssetInfo.GetInstance("Eye");
				try
				{
					assetInfo.Begin();

					var unhandledAssets = GetUnhandledAssets<GameObject>(AssetsFolder_Eye, "prefab");

					var i = 0;
					foreach (var key_value in infos)
					{
						++i;
						var ID = key_value.Key;
						var info = key_value.Value;
						LuaWorker.GetFieldString(info, "NameEn");
						var assetName = ID.ToString();
						
						ShowProgress("Eyes", i, totalCount, assetName);

						// 1.
						var srcFolder = PathUnity.Combine(AssetsSrcFolder_Role, LuaWorker.GetFieldString(info, "ModelDir"));
						var srcName = LuaWorker.GetFieldString(info, "Model");
						var clipFolders = new string[]{srcFolder}; 
						var publicClipFolder = LuaWorker.GetFieldString(info, "PublicAnime");
						if (!string.IsNullOrEmpty(publicClipFolder))
						{
							ArrayUtility.Add(ref clipFolders, PathUnity.Combine(AssetsSrcFolder_ActionEye, publicClipFolder));
						}
						var targetPath = Path.ChangeExtension(PathUnity.Combine(AssetsFolder_Eye, assetName), PrefabExtension);
						var asset = GenRolePrefabsAndController(
							assetInfo,
							srcFolder, 
							srcName, 
							clipFolders, 
							targetPath, 
							info,
							null, null, 
							RoleAnimatorDefaultState);
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
//						var matNumber = LuaWorker.GetFieldInt(info, "DefaultColor");
//						var defaultMatName = string.Format("{0}_{1}", matName, matNumber);
						
						#region refactory
//						var rolePart = asset.GetComponent<RolePartHair>();
//						if (null == rolePart)
//						{
//							rolePart = asset.AddComponent<RolePartHair>();
//						}
						var rolePart = asset.GetComponent<RolePart>();
						if (null == rolePart)
						{
							rolePart = asset.AddComponent<RolePart>();
						}
						InitRolePart(rolePart, srcFolder, srcName, matName/*, defaultMatName*/);
						#endregion refactory
						
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
