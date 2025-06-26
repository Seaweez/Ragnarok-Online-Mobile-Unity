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
		public static Dictionary<int, LuaTable> GetEffectForECPInfos(out WorkerContainer workerContainer)
		{
			return GetAssetsInfo(out workerContainer, "Effect", "Table_EffectECP");
		}

		[MenuItem( "RO/GenAssets/Effects(for ECP)", false, 1005)]
		public static void GenEffectsForECP()
		{
			WorkerContainer workerContainer;
			var infos = GetEffectForECPInfos(out workerContainer);
			using(workerContainer)
			{
				if (null == infos)
				{
					return;
				}

				var totalCount = infos.Count;
				if (0 == totalCount)
				{
					Debug.LogFormat("No Effects");
					return;
				}
				
				try
				{
					var effectInfos = new EffectInfo[totalCount];
					
					var i = 0;
					foreach (var key_value in infos)
					{
						++i;
						var ID = key_value.Key;
						var info = key_value.Value;
						var name = LuaWorker.GetFieldString(info, "NameEn");
						var assetName = string.Format("{0}{1}", ID, name);
						
						ShowProgress("Effects", i, totalCount, assetName);

						EffectInfo effectInfo = new EffectInfo();
						effectInfo.ID = (int)ID;
						effectInfo.path = LuaWorker.GetFieldString(info, "Path");
						effectInfos[i-1] = effectInfo;
					}

					var path = Path.ChangeExtension(PathUnity.Combine(AssetsFolder_Effect, "EffectDictionary"), AssetExtension);
			
					var effectDictionary = AssetDatabase.LoadAssetAtPath<EffectDictionary>(path);
					if (null == effectDictionary)
					{
						effectDictionary = new EffectDictionary();
						AssetDatabase.CreateAsset (effectDictionary, path);
					}
					effectDictionary.effectInfos = effectInfos;
					EditorUtility.SetDirty(effectDictionary);
				}
				finally
				{
					EditorUtility.ClearProgressBar();
					AssetDatabase.SaveAssets();
				}
			}
		}

		[MenuItem( "RO/GenAssets/Effects(fsor EffectHandle)", false, 1006)]
		public static void GenEffectsForEffectHandle()
		{
			try
			{
				var unhandledAssets = GetAllAssets<GameObject>(AssetsFolder_Effect, "prefab");
				var totalCount = unhandledAssets.Count;

				var i = 0;
				foreach (var assetObj in unhandledAssets)
				{
					++i;
					ShowProgress("Effects", i, totalCount, assetObj.name);

					var asset = assetObj.GetComponent<EffectHandle>();
					if (null == asset)
					{
						asset = assetObj.AddComponent<EffectHandle>();
					}

					asset.particles = GameObjectHelper.FindComponentsInChildren<ParticleSystem>(assetObj);
					asset.animators = GameObjectHelper.FindComponentsInChildren<Animator>(assetObj);
					asset.logics = GameObjectHelper.FindComponentsInChildren<EffectLogic>(assetObj);
					var renderers = GameObjectHelper.FindComponentsInChildren<ParticleSystem>(assetObj);
					if (!renderers.IsNullOrEmpty())
					{
						foreach (var r in renderers)
						{
							OptimizationUtils.OptiRenderer(r.GetComponent<Renderer>());
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
			}
			finally
			{
				EditorUtility.ClearProgressBar();
				AssetDatabase.SaveAssets();
			}
		}

		[MenuItem( "RO/GenAssets/Effects(for OrderInLayer)", false, 1007)]
		public static void GenEffectsForOrderInLayer()
		{
			try
			{
				var unhandledAssets = GetAllAssets<GameObject>(AssetsFolder_Effect, "prefab");
				var totalCount = unhandledAssets.Count*2;
				var materialMapOrder = new Dictionary<Material, int>();
				var orderUsed = new HashSet<int>();
				var newOrderAssets = new List<GameObject>();

				var i = 0;
				// 1.
				foreach (var assetObj in unhandledAssets)
				{
					++i;
					ShowProgress("Effects", i, totalCount, assetObj.name);

					var assetPath = AssetDatabase.GetAssetPath(assetObj);
					if (assetPath.StartsWith(AssetsFolder_UIEffect))
					{
						// ignore UI Effect
						Debug.LogFormat(assetObj, "<color=yellow>Ignore UI Effect</color>\n{0}", assetPath);
						continue;
					}

					bool changed = false;
					bool newOrder = false;
					var renders = assetObj.FindComponentsInChildren<ParticleSystemRenderer>();
					if (!renders.IsNullOrEmpty())
					{
						foreach (var r in renders)
						{
							var mat = r.sharedMaterial;
							if (null != mat)
							{
								var oldSortingOrder = r.sortingOrder;
								int order;
								if (materialMapOrder.TryGetValue(mat, out order))
								{
									r.sortingOrder = order;
									if (r.sortingOrder != oldSortingOrder)
									{
										changed = true;
										EditorUtility.SetDirty(r);
									}
								}
								else
								{
									order = r.sortingOrder;
									if (0 <= order)
									{
										if (0 == order || orderUsed.Contains(order))
										{
											// new order
											newOrder = true;
											r.sortingOrder = 0;
										}
										else
										{
											orderUsed.Add(order);
											materialMapOrder.Add(mat, order);
										}
									}
									else
									{
										// ignore
									}
								}
							}
						}
						if (newOrder)
						{
							newOrderAssets.Add(assetObj);
						}
						if (changed && null != assetObj)
						{
							EditorUtility.SetDirty(assetObj);
							Debug.LogFormat(assetObj, "<color=red>Order Changed!</color>\n{0}", assetPath);
						}
					}
				}
				int nextOrder = 1;
				// 2.
				foreach (var assetObj in newOrderAssets)
				{
					++i;
					ShowProgress("Effects", i, totalCount, assetObj.name);

					var renders = assetObj.FindComponentsInChildren<ParticleSystemRenderer>();
					if (!renders.IsNullOrEmpty())
					{
						foreach (var r in renders)
						{
							var mat = r.sharedMaterial;
							if (null != mat && 0 == r.sortingOrder)
							{
								while (orderUsed.Contains(nextOrder))
								{
									++nextOrder;
								}
								r.sortingOrder = nextOrder;
								++nextOrder;
								EditorUtility.SetDirty(r);
							}
						}
						if (null != assetObj)
						{
							EditorUtility.SetDirty(assetObj);
							Debug.LogFormat(assetObj, "<color=red>Order Changed!</color>\n{0}", AssetDatabase.GetAssetPath(assetObj));
						}
					}
				}
			}
			finally
			{
				EditorUtility.ClearProgressBar();
				AssetDatabase.SaveAssets();
			}
		}

		[MenuItem( "RO/GenAssets/SetUIEffect(OrderInLayer=0)", false, 1008)]
		public static void SetUIEffectOrderInLayerZero()
		{
			try
			{
				var unhandledAssets = GetAllAssets<GameObject>(AssetsFolder_UIEffect, "prefab");
				var totalCount = unhandledAssets.Count;

				var i = 0;
				// 1.
				foreach (var assetObj in unhandledAssets)
				{
					++i;
					ShowProgress("Effects", i, totalCount, assetObj.name);

					var changed = false;
					var renders = assetObj.FindComponentsInChildren<ParticleSystemRenderer>();
					if (!renders.IsNullOrEmpty())
					{
						foreach (var r in renders)
						{
							if (0 != r.sortingOrder)
							{
								r.sortingOrder = 0;
								EditorUtility.SetDirty(r);
								changed = true;
							}
						}
					}
					if (changed && null != assetObj)
					{
						EditorUtility.SetDirty(assetObj);
						Debug.LogFormat(assetObj, "<color=red>Order Changed!</color>\n{0}", AssetDatabase.GetAssetPath(assetObj));
					}
				}
			}
			finally
			{
				EditorUtility.ClearProgressBar();
				AssetDatabase.SaveAssets();
			}
		}
	}
} // namespace EditorTool
