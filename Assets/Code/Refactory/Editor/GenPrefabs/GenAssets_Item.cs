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
		public static string GetItemModlePath(LuaTable info)
		{
			var path = PathUnity.Combine(LuaWorker.GetFieldString(info, "ModelDir"), LuaWorker.GetFieldString(info, "ModelName"));
			path.TrimStart('/');
			return path;
		}

		public static Dictionary<int, LuaTable> GetItemObjectInfos(out WorkerContainer workerContainer)
		{
			return GetAssetsInfo_1(out workerContainer, "ItemObject", GetItemModlePath, "Table_ItemObject");
		}

		[MenuItem( "RO/GenAssets/ItemObjects", false, 1003)]
		public static void GenItemObjects()
		{
			WorkerContainer workerContainer;
			var infos = GetItemObjectInfos(out workerContainer);
			using(workerContainer)
			{
				if (null == infos)
				{
					return;
				}

				var totalCount = infos.Count;
				if (0 == totalCount)
				{
					Debug.LogFormat("No ItemObjects");
					return;
				}

				var assetInfo = AssetInfo.GetInstance("ItemObject");
				try
				{
					assetInfo.Begin();

					var unhandledAssets = GetUnhandledAssets<GameObject>(AssetsFolder_Item, "prefab");

					var i = 0;
					foreach (var key_value in infos)
					{
						++i;
						var ID = key_value.Key;
						var info = key_value.Value;
						LuaWorker.GetFieldString(info, "NameEn");
						var assetName = ID.ToString();
						
						ShowProgress("ItemObjects", i, totalCount, assetName);

						// 1.
						var srcFolder = PathUnity.Combine(AssetsSrcFolder_Model, LuaWorker.GetFieldString(info, "ModelDir"));
						var srcName = LuaWorker.GetFieldString(info, "ModelName");
						var clipFolders = new string[]{srcFolder}; 
						var targetPath = Path.ChangeExtension(PathUnity.Combine(AssetsFolder_Item, assetName), PrefabExtension);
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

						var animatorPlayer = asset.GetComponent<SimpleAnimatorPlayer>();
						if (null == animatorPlayer)
						{
							animatorPlayer = asset.AddComponent<SimpleAnimatorPlayer>();
						}
						if (1 == LuaWorker.GetFieldInt(info, "EffectPoint"))
						{
							var pointSubject = asset.GetComponent<PointSubject>();
							if (null == pointSubject)
							{
								pointSubject = asset.AddComponent<PointSubject>();
							}
						}

						var shadow = asset.FindGameObjectInChildren(delegate(GameObject obj){
							return string.Equals(obj.name, "Shadow");
						});
						if (null == shadow)
						{
							var shadowAsset = AssetDatabase.LoadAssetAtPath<GameObject>(Path.ChangeExtension(PathUnity.Combine(AssetsFolder_Public, "Shadow"), PrefabExtension));
							if (null != shadowAsset)
							{
								var item = PrefabUtility.InstantiatePrefab(asset) as GameObject;
								shadow = PrefabUtility.InstantiatePrefab(shadowAsset) as GameObject;

								var shadowTransform = shadow.transform;
								shadowTransform.parent = item.transform;
//								shadowTransform.localPosition = Vector3.zero;
//								shadowTransform.localRotation = Quaternion.identity;
//								shadowTransform.localScale = Vector3.one;

								var shadowSprite = shadow.GetComponent<SpriteRenderer>();
								if (null != shadowSprite)
								{
									ModelUtils.AdjustSprite(item, shadowSprite, 1.2f);
								}

								PrefabUtility.ReplacePrefab(item, asset, ReplacePrefabOptions.Default);
								GameObject.DestroyImmediate(item);
								GameObject.DestroyImmediate(shadow);
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
