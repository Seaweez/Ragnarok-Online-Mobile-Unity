using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using RO;
using RO.Config;
using SLua;
using Ghost.Utils;
using Ghost.Extensions;

namespace EditorTool
{
	public static partial class GenAssets 
	{
		public static readonly string[] NeatralNPCTypes = {
			"NPC",
			"GatherNPC",
			"SealNPC",
		};
		public static Dictionary<int,LuaTable> kLuaDataInfo=new Dictionary<int, LuaTable>();

		public static Dictionary<int, LuaTable> GetNPCInfos(out WorkerContainer workerContainer)
		{
			return GetAssetsInfo(out workerContainer, "NPC", "Table_Npc", "Table_Monster");
		}
		public static readonly Dictionary<string,string> kArrayCheck=new Dictionary<string,string>()
		{
			{"Body","Body"},
			{"Hair","Hair"},
			{"LeftHand","Weapon"},
			{"RightHand","Weapon"},
			{"Head","Head"},
			{"Wing","Wing"},
			{"Face","Face"},
			{"Tail","Tail"},
			{"Eye","Eye"},
			{"Mount","Mount"},
		};
		public static Dictionary<int,LuaTable> GetNpcInfos(out WorkerContainer WorkerContainer)
		{
			return GetAssetsInfo(out WorkerContainer,  "NPC", "Table_Npc");
		}
		public static Dictionary<int,LuaTable> kNpcTableData = new Dictionary<int, LuaTable> ();

		[MenuItem( "RO/CheckAssets/CheckMonsterNpcTable", false, 11)]
		public static void CheckMonsterNpc()
		{
			WorkerContainer workerContainer;
			var infos = GetNPCInfos(out workerContainer);
			kNpcTableData = GetNpcInfos(out workerContainer);
			var kLogError = new Dictionary<string, string>();
			using(workerContainer)
			{
				if (null == infos)
				{
					return;
				}

				var totalCount = infos.Count;
				if (0 == totalCount)
				{
					Debug.LogError("Get Monster. Npc table failed.");
					return;
				}

				try
				{
					var i = 0;
					foreach (var itemValue in infos)
					{
						++i;
						var ID = itemValue.Key;
						var info = itemValue.Value;
						var name = LuaWorker.GetFieldString(info, "NameZh");
						if (string.IsNullOrEmpty(name))
						{
							name = LuaWorker.GetFieldString(info, "NameEn");
						}
						foreach(var key in kArrayCheck.Keys)
						{
							var fileName = LuaWorker.GetFieldString(info, key);
							if(fileName=="0"||null==fileName)
								continue;
							if(null!=fileName)	
							{
								string sPreFilePath="Assets/Resources/Role/";
								var sFilePath=sPreFilePath+kArrayCheck[key];
								if(Directory.Exists(sFilePath))
								{
									string[] files = Directory.GetFiles(sFilePath);
									bool result=false;
									for(int h=0;h<files.Length;h++)
									{
										string fl=Path.GetFileNameWithoutExtension(files[h]);
										if(fl==fileName)
										{
											result=true;
										}
									}
									if(!result)
									{
										string _tableName=sTableName(ID);
										string logK=string.Format("{0}={1}",key,fileName);
										string logV=string.Format("Table_{0},ID={1}, Name={2} ",_tableName,ID,name);
										if(!kLogError.ContainsKey(logK))
										{
											kLogError.Add(logK,logV);
										}
										else
										{
											kLogError[logK] += "\n"+logV;
										}
										//Debug.LogError("表key值:"+ID.ToString() + "   所填：" +key +"="+ fileName);//不在unity目录："+sFilePath+"中。");
									}
								}
							}
						}
						ShowProgress("Checking Table_Monster.txt. Table_Npc.txt", i, totalCount, ID.ToString());
					}
				}
				finally
				{
					EditorUtility.ClearProgressBar();
					foreach (var i in kLogError) 
					{
						Debug.LogFormat ("<color=red>{0}</color>{1}", i.Key+"\n", i.Value);
					}
				}
			}
		}


		[MenuItem( "RO/CheckAssets/CheckHeadIcon", false, 11)]
		public static void CheckHead()
		{
			WorkerContainer workerContainer;
			var infos = GetNPCInfos(out workerContainer);
			kNpcTableData = GetNpcInfos(out workerContainer);
			var kLogError = new Dictionary<string, string>();
			using(workerContainer)
			{
				if (null == infos)
				{
					return;
				}

				var totalCount = infos.Count;
				if (0 == totalCount)
				{
					Debug.LogError("Get Monster. Npc table failed.");
					return;
				}

				try
				{
					var i = 0;
					foreach (var itemValue in infos)
					{
						++i;
						var ID = itemValue.Key;
						var info = itemValue.Value;
						var name = LuaWorker.GetFieldString(info, "Icon");
						if(string.IsNullOrEmpty(name))
							continue;
						string sAtlasPath="Assets/Resources/GUI/atlas/preferb/";
						if(Directory.Exists(sAtlasPath))
						{
							string[] resourceFiles = Directory.GetFiles(sAtlasPath);
							bool checkResult=false;
							for(int h=0;h<resourceFiles.Length;h++)
							{
								string fl=Path.GetFileNameWithoutExtension(resourceFiles[h]);
								if(fl.Contains("face_") && !fl.Contains(".meta"))
								{
									UIAtlas targetAtlas = AssetDatabase.LoadAssetAtPath<UIAtlas>(sAtlasPath+fl);
									if(null!=targetAtlas)
									{
										foreach(UISpriteData item in targetAtlas.spriteList)
										{
											if(item.name==name)
											{
												checkResult=true;
												break;
											}
										}
									}
								}
							}
							if(!checkResult)
							{
								string _tableName=sTableName(ID);
								string logK=string.Format("Icon={0}",name);
								string logV=string.Format("Table_{0}, ID={1} ",_tableName,ID);
								if(!kLogError.ContainsKey(logK))
								{
									kLogError.Add(logK,logV);
								}
								else
								{
									kLogError[logK] += "\n"+logV;
								}
							}
						}
						else
						{
							Debug.LogError("头像资源目录改了？之前的目录是："+sAtlasPath);
							return;
						}
						ShowProgress("Checking Head Icon", i, totalCount, ID.ToString());
					}
				}
				finally
				{
					EditorUtility.ClearProgressBar();
					foreach (var i in kLogError) 
					{
						Debug.LogFormat ("<color=red>{0}</color>{1}", i.Key+"\n", i.Value);
					}
				}
			}
		}

		public static string sTableName(int id)
		{
			foreach (var i in kNpcTableData.Keys) 
			{
				if (id == i) 
				{
					return "NPC";
				}
			}
			return "Monster";
		}

//		[MenuItem( "RO/GenAssets/NPCs", false, 11)]
//		public static void GenNPCs()
//		{
//			WorkerContainer workerContainer;
//			var infos = GetNPCInfos(out workerContainer);
//			using(workerContainer)
//			{
//				if (null == infos)
//				{
//					return;
//				}
//
//				var totalCount = infos.Count;
//				if (0 == totalCount)
//				{
//					Debug.LogFormat("No NPCs");
//					return;
//				}
//
//				RoleAgent assetModel = null;
//				try
//				{
//					var prefab = AssetDatabase.LoadAssetAtPath<RoleAgent>(Path.ChangeExtension(PathUnity.Combine(AssetsFolder_Role, "RoleAgent"), PrefabExtension));
//					if (null == prefab)
//					{
//						Debug.LogErrorFormat("assetModel is null");
//						return;
//					}
//					else
//					{
//						assetModel = GameObject.Instantiate<RoleAgent>(prefab);
//						assetModel.gameObject.hideFlags = HideFlags.HideAndDontSave;
//					}
//
//					WorkerContainer hairWorkerContainer;
//					var hairInfos = GetHairInfos(out hairWorkerContainer);
//					if (!hairWorkerContainer.workers.IsNullOrEmpty())
//					{
//						ArrayUtility.AddRange(ref workerContainer.workers, hairWorkerContainer.workers);
//					}
//
//					WorkerContainer bodyWorkerContainer;
//					var bodyInfos = GetBodyInfos(out bodyWorkerContainer);
//					if (!bodyWorkerContainer.workers.IsNullOrEmpty())
//					{
//						ArrayUtility.AddRange(ref workerContainer.workers, bodyWorkerContainer.workers);
//					}
//					
//					var unhandledAssets = GetUnhandledAssets<RoleAgent>(AssetsFolder_NPC, "prefab");
//
//					var actionList = AssetDatabase.LoadAssetAtPath<RoleActionInfoList>(Path.ChangeExtension(PathUnity.Combine(AssetsFolder_Role, ResourceIDHelper.NAME_NPC_ACTION_LIST), AssetExtension));
//					
//					var i = 0;
//					foreach (var key_value in infos)
//					{
//						++i;
//						var ID = key_value.Key;
//						var info = key_value.Value;
//						var name = LuaWorker.GetFieldString(info, "NameEn");
//						var assetName = ID.ToString();//string.Format("{0}{1}", ID, name);
//						
//						ShowProgress("NPCs", i, totalCount, assetName);
//						
//						RoleAgent asset;
//						if (unhandledAssets.TryGetValue(assetName, out asset))
//						{
//							if (null == asset)
//							{
//								continue;
//							}
//							unhandledAssets.Remove(assetName);
//						}
//						else
//						{
//							var path = Path.ChangeExtension(PathUnity.Combine(AssetsFolder_NPC, assetName), PrefabExtension);
//
//							var newPrefab = PrefabUtility.CreatePrefab (path, assetModel.gameObject);
//							asset = newPrefab.GetComponent<RoleAgent>();
//							asset.name = assetName;
//							Debug.LogFormat("<color=green>New Created: </color>{0}", asset.name);
//						}
//						var assetObj = asset.gameObject;
//
//						var data = asset.data;			
//						data.ID = ID;
//						if (ArrayUtility.Contains(NeatralNPCTypes, LuaWorker.GetFieldString(info, "Type")))
//						{
//							data.camp = RoleInfo.Camp.NEUTRAL;
//						}
//						else
//						{
//							data.camp = RoleInfo.Camp.ENEMY;
//						}
//						data.moveActionScale = LuaWorker.GetFieldFloat(info, "MoveSpdRate");
//						data.outLine = 0 != LuaWorker.GetFieldInt(info, "Stroke");
//						data.toon = 0 == LuaWorker.GetFieldInt(info, "NoToon");
//						data.blendMode = (CombineMeshesWorker.BlendMode)LuaWorker.GetFieldInt(info, "BlendMode");
//						data.actionList = actionList;
//						var accessRange = LuaWorker.GetFieldInt(info, "AccessRange");
//						if (0 < accessRange)
//						{
//							data.accessRange = accessRange;
//						}
//
//						var moveType = LuaWorker.GetFieldInt(info, "move");
//						if (1 == moveType)
//						{
//							if (null != asset.shadow)
//							{
//								GameObject.DestroyImmediate(asset.shadow.gameObject, true);
//								asset.shadow = null;
//							}
//						}
//
//						var avatar = data.avatar;
//						avatar.body.ID = LuaWorker.GetFieldInt(info, "Body");
//						avatar.hair.ID = LuaWorker.GetFieldInt(info, "Hair");
//						avatar.leftWeapon.ID = LuaWorker.GetFieldInt(info, "LeftHand");
//						avatar.rightWeapon.ID = LuaWorker.GetFieldInt(info, "RightHand");
//						avatar.wing.ID = LuaWorker.GetFieldInt(info, "Wing");
//						avatar.mount.ID = LuaWorker.GetFieldInt(info, "Mount");
//						avatar.accessories.ID = LuaWorker.GetFieldInt(info, "Head");
//						avatar.face.ID = LuaWorker.GetFieldInt(info, "Face");
//						avatar.tail.ID = LuaWorker.GetFieldInt(info, "Tail");
//						var hairColorIndex = LuaWorker.GetFieldInt(info, "HeadDefaultColor");
//						if (0 == hairColorIndex)
//						{
//							avatar.hair.maskColor = GetDefaultColor(hairInfos, avatar.hair.ID);
//						}
//						else 
//						{
//							avatar.hair.maskColor = GetColor(hairInfos, avatar.hair.ID, hairColorIndex);
//						}
//						var bodyColorIndex = LuaWorker.GetFieldInt(info, "BodyDefaultColor");
//						if (0 == bodyColorIndex)
//						{
//							avatar.body.maskColor = GetDefaultColor(bodyInfos, avatar.body.ID);
//						}
//						else 
//						{
//							avatar.body.maskColor = GetColor(bodyInfos, avatar.body.ID, bodyColorIndex);
//						}
//						avatar.body.offset.y = LuaWorker.GetFieldFloat(info, "FloatHeight");
//
//						if (null != asset)
//						{
//							EditorUtility.SetDirty(asset);
//						}
//						else if (null != assetObj)
//						{
//							EditorUtility.SetDirty(assetObj);
//						}
//					}
//
//					if (!workerContainer.debug)
//					{
//						foreach (var asset in unhandledAssets.Values)
//						{
//							if (null != asset)
//							{
//								Debug.LogFormat("<color=red>Deleted: </color>{0}", asset.name);
//								AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(asset));
//							}
//						}
//					}
//				}
//				finally
//				{
//					if (null != assetModel)
//					{
//						GameObject.DestroyImmediate(assetModel.gameObject);
//					}
//					EditorUtility.ClearProgressBar();
//					AssetDatabase.SaveAssets();
//				}
//			}
//		}
	}
} // namespace EditorTool
