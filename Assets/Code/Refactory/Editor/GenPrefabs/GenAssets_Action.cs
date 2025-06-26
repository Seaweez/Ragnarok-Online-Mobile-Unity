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
		public static readonly string[] RefreshEPActions = {
			"wait",
			"walk",
			"sit_down",
			"die",
			"ride_wait",
			"ride_walk",
			"ride_sit_down",
			"ride_die",
			"choose_wait",
			"choose_wait2"
		};

		public static Dictionary<int, LuaTable> GetActionInfos(out WorkerContainer workerContainer)
		{
			return GetAssetsInfo(out workerContainer, "Action", "Table_ActionAnime");
		}

//		[MenuItem( "RO/GenAssets/Actions", false, 1004)]
//		public static void GenActions()
//		{
//			WorkerContainer workerContainer;
//			var infos = GetActionInfos(out workerContainer);
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
//					Debug.LogFormat("No Actions");
//					return;
//				}
//				
//				try
//				{
//					var roleActions = new List<RoleActionInfo>();
//					var npcActions = new List<RoleActionInfo>();
//					
//					var i = 0;
//					foreach (var key_value in infos)
//					{
//						++i;
//						var ID = key_value.Key;
//						var info = key_value.Value;
//						var name = LuaWorker.GetFieldString(info, "Name");
//						var assetName = string.Format("{0}{1}", ID, name);
//
//						ShowProgress("Actions", i, totalCount, assetName);
//						
//						#region role
//						var asset = new RoleActionInfo();
//						asset.name = name;
//						asset.showWeapon = 0 < LuaWorker.GetFieldInt(info, "PlayerShowWeapon");
//						asset.refreshEP = ArrayUtility.Contains(RefreshEPActions, asset.name);
//						roleActions.Add(asset);
//						#endregion role
//						
//						#region npc
//						asset = new RoleActionInfo();
//						asset.name = name;
//						asset.showWeapon = 0 < LuaWorker.GetFieldInt(info, "NPCShowWeapon");
//						asset.refreshEP = ArrayUtility.Contains(RefreshEPActions, asset.name);
//						npcActions.Add(asset);
//						#endregion npc
//					}
//
//					var path = Path.ChangeExtension(PathUnity.Combine(AssetsFolder_Role, ResourceIDHelper.NAME_ROLE_ACTION_LIST), AssetExtension);
//			
//					var roleList = AssetDatabase.LoadAssetAtPath<RoleActionInfoList>(path);
//					if (null == roleList)
//					{
//						roleList = new RoleActionInfoList();
//						AssetDatabase.CreateAsset (roleList, path);
//					}
//					roleList.actions = roleActions;
//					EditorUtility.SetDirty(roleList);
//
//					path = Path.ChangeExtension(PathUnity.Combine(AssetsFolder_Role, ResourceIDHelper.NAME_NPC_ACTION_LIST), AssetExtension);
//					roleList = AssetDatabase.LoadAssetAtPath<RoleActionInfoList>(path);
//					if (null == roleList)
//					{
//						roleList = new RoleActionInfoList();
//						AssetDatabase.CreateAsset (roleList, path);
//					}
//					roleList.actions = npcActions;
//					EditorUtility.SetDirty(roleList);
//				}
//				finally
//				{
//					EditorUtility.ClearProgressBar();
//					AssetDatabase.SaveAssets();
//				}
//			}
//		}
	}
} // namespace EditorTool
