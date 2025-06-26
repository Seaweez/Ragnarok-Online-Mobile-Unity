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
		public static string GetEquipModlePath(LuaTable info)
		{
			var path = PathUnity.Combine(LuaWorker.GetFieldString(info, "ModelDir"), LuaWorker.GetFieldString(info, "Model"));
			path.TrimStart('/');
			return path;
		}

		public static Dictionary<int, LuaTable> GetEquipInfos(out WorkerContainer workerContainer)
		{
			return GetAssetsInfo_1(out workerContainer, "Equip", GetEquipModlePath, "Table_Equip", "Table_EquipFake");
		}

		public static Dictionary<int, LuaTable> GetWeaponTypeInfos(out WorkerContainer workerContainer)
		{
			return GetAssetsInfo(out workerContainer, "WeaponType", "Table_WeaponType");
		}

		public enum EquipType
		{
			Head = 0,
			Face = 1,
			Weapon = 2,
			Wing = 3,
			Tail = 4,
			Mouth = 5,
		};
		public static readonly string[] EquipAssetFolders;

		[MenuItem( "RO/GenAssets/Equips", false, 105)]
		public static void GenEquips()
		{
			WorkerContainer workerContainer;
			var infos = GetEquipInfos(out workerContainer);
			using(workerContainer)
			{
				if (null == infos)
				{
					return;
				}

				var totalCount = infos.Count;
				if (0 == totalCount)
				{
					Debug.LogFormat("No Equips");
					return;
				}

				var assetInfo = AssetInfo.GetInstance("Equip");
				try
				{
					assetInfo.Begin();

					WorkerContainer weaponTypeWorkerContainer;
					var weaponTypeInfos = GetWeaponTypeInfos(out weaponTypeWorkerContainer);
					if (!weaponTypeWorkerContainer.workers.IsNullOrEmpty())
					{
						ArrayUtility.AddRange(ref workerContainer.workers, weaponTypeWorkerContainer.workers);
					}

					var weaponTypes = new HashSet<string>();
					if (null != weaponTypeInfos)
					{
						foreach (var info in weaponTypeInfos.Values)
						{
							var weaponType = LuaWorker.GetFieldString(info, "NameEn");
							if (!string.IsNullOrEmpty(weaponType))
							{
								weaponTypes.Add(weaponType);
							}
						}
					}

					var unhandledAssetsArray = GetUnhandledAssets<GameObject>(EquipAssetFolders, "prefab");

					var i = 0;
					foreach (var key_value in infos)
					{
						++i;
						var ID = key_value.Key;
						var info = key_value.Value;
						LuaWorker.GetFieldString(info, "NameEn");
						var assetName = ID.ToString();
						
						ShowProgress("Equips", i, totalCount, assetName);

						// 1.
						var equipType = LuaWorker.GetFieldString(info, "Type");
						var equipIndex = -1;
						if (weaponTypes.Contains(equipType))
						{
							equipIndex = (int)EquipType.Weapon;
						}
						else
						{
							var equitTypeNames = System.Enum.GetNames(typeof(EquipType));
							for (int j = 0; j < equitTypeNames.Length; ++j)
							{
								if (string.Equals(equitTypeNames[j], equipType))
								{
									equipIndex = System.Convert.ToInt32(System.Enum.GetValues(typeof(EquipType)).GetValue(j));
									break;
								}
							}
						}
						if (0 > equipIndex)
						{
							continue;
						}

						// 2.
						var unhandledAssets = unhandledAssetsArray[equipIndex];
						var targetFolder = EquipAssetFolders[equipIndex];

						var srcFolder = PathUnity.Combine(AssetsSrcFolder_Role, LuaWorker.GetFieldString(info, "ModelDir"));
						var srcName = LuaWorker.GetFieldString(info, "Model");
						var clipFolders = new string[]{LuaWorker.GetFieldString(info, "Animation")};
						if (!string.IsNullOrEmpty(clipFolders[0]))
						{
							clipFolders[0] = PathUnity.Combine(srcFolder, clipFolders[0]);
						}
						else
						{
							clipFolders[0] = srcFolder;
						}
						var targetPath = Path.ChangeExtension(PathUnity.Combine(targetFolder, assetName), PrefabExtension);
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

						// 3.
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

						#region refactory
						var rolePart = asset.GetComponent<RolePart>();
						if (null == rolePart)
						{
							rolePart = asset.AddComponent<RolePart>();
						}
						InitRolePart(rolePart, srcFolder, srcName, matName);
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
						foreach (var unhandledAssets in unhandledAssetsArray)
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
