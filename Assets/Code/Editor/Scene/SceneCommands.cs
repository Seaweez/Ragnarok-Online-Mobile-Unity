using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Ghost.Config;
using Ghost.Utils;
using Ghost.Extensions;
using LitJson;
using RO;
using UnityEditor.SceneManagement;

namespace EditorTool
{
	public static class SceneCommands 
	{
		private static float DESTINATION_VALID_RANGE = 5;
		private static string SCENE_INNER_TELEPORT_DATA_NAME = "TeleportData.asset";
		private static int SCENE_INFO_CSV_COL_COUNT = 3;

		private static void BuildTeleportPath(BornPoint[] bps, ExitPoint[] eps, NPCInfo[] nps)
		{
			if (bps.IsNullOrEmpty())
			{
				return;
			}
			if (eps.IsNullOrEmpty())
			{
				return;
			}
			var bpsMap = new Dictionary<int, BornPoint>();
			foreach (var bp in bps)
			{
				if (!bp.gameObject.activeInHierarchy)
				{
					continue;
				}
				bp.from = null;
				bp.to = null;
				bp.toBPS = null;
				bp.teleportPaths = null;
				bpsMap.Add(bp.ID, bp);
			}

			// teleport from and to
			var epsMap = new Dictionary<int, ExitPoint>();
			foreach (var ep in eps)
			{
				if (!ep.gameObject.activeInHierarchy)
				{
					continue;
				}
				ep.from = null;
				ep.to = null;
				ep.teleportPaths = null;
				epsMap.Add(ep.ID, ep);
				if (0 == ep.nextSceneID)
				{
					BornPoint bp;
					if (bpsMap.TryGetValue(ep.nextSceneBornPointID, out bp))
					{
						if (null == bp.from)
						{
							bp.from = new List<ExitPoint>();
						}
						bp.from.Add(ep);
						ep.to = bp;
					}
				}
			}

			bps = bpsMap.Values.ToArray();
			eps = epsMap.Values.ToArray();

			// arrive to ep and bp
			foreach (var bp in bps)
			{
				var to = new List<ToExitPointInfo>();
				foreach (var ep in eps)
				{
					UnityEngine.AI.NavMeshPath path;
					if (NavMeshUtils.CanArrived(bp.position, ep.position, DESTINATION_VALID_RANGE, true, out path))
					{
						var cost = NavMeshUtils.GetPathDistance(path);
						to.Add(new ToExitPointInfo(ep, cost));
						if (null == ep.from)
						{
							ep.from = new List<ToBornPointInfo>();
						}
						ep.from.Add(new ToBornPointInfo(bp, cost));
					}
				}
				if (0 < to.Count)
				{
					bp.to = to;
				}

				foreach (var otherBP in bps)
				{
					if (bp == otherBP)
					{
						continue;
					}
					if (null != otherBP.toBPS)
					{
						var info = otherBP.toBPS.Find(delegate(ToBornPointInfo obj) {
							return obj.bp == bp;
						});
						if (null != info)
						{
							continue;
						}
					}
					UnityEngine.AI.NavMeshPath path;
					if (NavMeshUtils.CanArrived(bp.position, otherBP.position, DESTINATION_VALID_RANGE, true, out path))
					{
						var cost = NavMeshUtils.GetPathDistance(path);

						if (null == bp.toBPS)
						{
							bp.toBPS = new List<ToBornPointInfo>();
						}
						bp.toBPS.Add(new ToBornPointInfo(otherBP, cost));

						if (null == otherBP.toBPS)
						{
							otherBP.toBPS = new List<ToBornPointInfo>();
						}
						otherBP.toBPS.Add(new ToBornPointInfo(bp, cost));
					}
				}
			}

			#region inner teleport path
			foreach (var ep in eps)
			{
				if (0 != ep.nextSceneID)
				{
					continue;
				}
				foreach (var bp in bps)
				{
					ep.BuildTeleportTo(bp);
				}
			}
			#endregion inner teleport path

			#region outter teleport path
			foreach (var bp in bps)
			{
				foreach (var ep in eps)
				{
					if (0 == ep.nextSceneID)
					{
						continue;
					}
					bp.BuildTeleportTo(ep);
				}
			}
			#endregion outter teleport path

			#region NPC
//			foreach (var np in nps)
//			{
//				np.canArrivedBornPoints = null;
//				if (!np.calcCanArrivedBornPoints)
//				{
//					continue;
//				}
//
//				var canArrivedBornPoints = new List<BornPoint>();
//				foreach (var bp in bps)
//				{
//					NavMeshPath path;
//					if (NavMeshUtils.CanArrived(bp.position, np.position, DESTINATION_VALID_RANGE, true, out path))
//					{
//						canArrivedBornPoints.Add(bp);
//						continue;
//					}
//					var teleportPath = NavMeshUtils.GetTeleportPath(bpsMap, epsMap, bp.position, np.position, DESTINATION_VALID_RANGE);
//					if (null != teleportPath && 0 < teleportPath.Count)
//					{
//						canArrivedBornPoints.Add(bp);
//						continue;
//					}
//				}
//				if (0 < canArrivedBornPoints.Count)
//				{
//					np.canArrivedBornPoints = canArrivedBornPoints.ToArray();
//				}
//			}
			#endregion NPC
		}
		
		private static SceneInnerPlayModeTeleportData BuildTeleportPath(GameObject root)
		{
			var bps = null != root ? root.GetComponentsInChildren<BornPoint>() : GameObject.FindObjectsOfType<BornPoint>();
			var eps = null != root ? root.GetComponentsInChildren<ExitPoint>() : GameObject.FindObjectsOfType<ExitPoint>();
//			var nps = null != root ? root.GetComponentsInChildren<NPCInfo>() : GameObject.FindObjectsOfType<NPCInfo>();
			BuildTeleportPath(bps, eps, null);

			var toBPDatas = new List<SceneInnerToBornPointTeleportData>();
			if (!eps.IsNullOrEmpty())
			{
				foreach (var ep in eps)
				{
					if (!ep.teleportPaths.IsNullOrEmpty())
					{
						foreach (var path in ep.teleportPaths)
						{
							var data = new SceneInnerToBornPointTeleportData();
							data.sourceID = ep.ID;
							data.targetID = path.bp.ID;
							data.totalCost = path.totalCost;

							if (null != path.nextEP)
							{
								data.nextEPID = path.nextEP.ID;
							}
							toBPDatas.Add(data);
						}
					}
				}
			}

			var toEPDatas = new List<SceneInnerToExitPointTeleportData>();
			if (!bps.IsNullOrEmpty())
			{
				foreach (var bp in bps)
				{
					if (!bp.teleportPaths.IsNullOrEmpty())
					{
						foreach (var path in bp.teleportPaths)
						{
							var data = new SceneInnerToExitPointTeleportData();
							data.sourceID = bp.ID;
							data.targetID = path.ep.ID;
							data.totalCost = path.totalCost;

							if (null != path.nextEP)
							{
								data.startEPID = path.nextEP.ID;
							}
							if (null != path.bp)
							{
								data.endBPID = path.bp.ID;
							}
							toEPDatas.Add(data);
						}
					}
				}
			}

			if (0 < toBPDatas.Count || 0 < toEPDatas.Count)
			{
				var data = new SceneInnerPlayModeTeleportData();
				if (0 < toBPDatas.Count)
				{
					data.toBornPoints = toBPDatas.ToArray();
				}
				if (0 < toEPDatas.Count)
				{
					data.toExitPoints = toEPDatas.ToArray();
				}
				return data;
			}
			return null;
		}

		public static void BuildTeleportPath()
		{
			var sceneFolder = Path.GetDirectoryName(EditorSceneManager.GetActiveScene().path);
			PathUnity.Combine(sceneFolder, SCENE_INNER_TELEPORT_DATA_NAME);

			var data = ScriptableObject.CreateInstance<SceneInnerTeleportData>();

			#region root
			var sceneRoot = GameObject.FindObjectOfType<SceneRoot>();
			GameObject root = null;
			if (null != sceneRoot)
			{
				root = null != sceneRoot.PVE ? sceneRoot.PVE : sceneRoot.gameObject;
			}
			data.pve = BuildTeleportPath(root);

			if (null != sceneRoot && null != sceneRoot.PVP)
			{
				root = sceneRoot.PVP;
				data.pvp = BuildTeleportPath(root);
			}
			else
			{
				data.pvp = null;
			}
			#endregion root
			
			#region raid
			var raidDatas = new List<SceneInnerPlayModeTeleportData>();

			var raids = GameObject.FindObjectsOfType<SceneRaid>();
			if (!raids.IsNullOrEmpty())
			{
				foreach (var raid in raids)
				{
					root = raid.gameObject;
					var raidData = BuildTeleportPath(root);
					if (null != raidData)
					{
						raidData.ID = raid.ID;
						raidDatas.Add(raidData);
					}
				}
			}

			data.raids = !raidDatas.IsNullOrEmpty() ? raidDatas.ToArray() : null;
			#endregion raid

			#region export
			var sceneName = Path.GetFileNameWithoutExtension(EditorSceneManager.GetActiveScene().name);
			string folder = PathUnity.Combine(RO.Config.Export.SCENE_DIRECTORY, sceneName);
			if (!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
			}

			var file = PathUnity.Combine(folder, "TeleportInfo.lua");
			
			FileStream fs = null;
			StreamWriter sw = null;
			try
			{
				fs = new FileStream(file, File.Exists(file) ? FileMode.Truncate : FileMode.Create);
				sw = new StreamWriter(fs, new UTF8Encoding(false));
				
				sw.Write(GetTeleportInfo_Lua(data).ToString());
				
				Debug.Log("Export TeleportInfo Lua Success");
			}
			catch (System.Exception e)
			{
				Debug.LogError("Export TeleportInfo Lua Error: "+e.Message);
			}
			finally
			{
				if (null != sw)
				{
					sw.Close();
				}
				if (null != fs)
				{
					fs.Close();
				}
			}
			#endregion export
		}

		private static void WriteEP2BPTeleportInfo(WriterAdapter writer, SceneInnerToBornPointTeleportData[] datas)
		{
			var dic = new Dictionary<int, List<SceneInnerToBornPointTeleportData>>();
			foreach (var data in datas)
			{
				List<SceneInnerToBornPointTeleportData> list;
				if (!dic.TryGetValue(data.sourceID, out list))
				{
					list = new List<SceneInnerToBornPointTeleportData>();
					dic.Add(data.sourceID, list);
				}
				list.Add(data);
			}

			writer.WriteStructStart();
			foreach (var key_value in dic)
			{
				writer.WriteMemberName(key_value.Key);
				writer.WriteStructStart();
				foreach (var data in key_value.Value)
				{
					writer.WriteMemberName(data.targetID);
					writer.WriteStructStart();
					if (0 <= data.nextEPID)
					{
						writer.WriteMemberName("nextEP");
						writer.WriteMemberValue(data.nextEPID);
					}
					writer.WriteMemberName("totalCost");
					writer.WriteMemberValue(data.totalCost);
					writer.WriteStructEnd();
				}
				writer.WriteStructEnd();
			}
			writer.WriteStructEnd();
		}

		private static void WriteBP2EPTeleportInfo(WriterAdapter writer, SceneInnerToExitPointTeleportData[] datas)
		{
			var dic = new Dictionary<int, List<SceneInnerToExitPointTeleportData>>();
			foreach (var data in datas)
			{
				List<SceneInnerToExitPointTeleportData> list;
				if (!dic.TryGetValue(data.sourceID, out list))
				{
					list = new List<SceneInnerToExitPointTeleportData>();
					dic.Add(data.sourceID, list);
				}
				list.Add(data);
			}
			
			writer.WriteStructStart();
			foreach (var key_value in dic)
			{
				writer.WriteMemberName(key_value.Key);
				writer.WriteStructStart();
				foreach (var data in key_value.Value)
				{
					writer.WriteMemberName(data.targetID);
					writer.WriteStructStart();
					if (0 <= data.startEPID)
					{
						writer.WriteMemberName("startEP");
						writer.WriteMemberValue(data.startEPID);
					}
					if (0 <= data.endBPID)
					{
						writer.WriteMemberName("endBP");
						writer.WriteMemberValue(data.endBPID);
					}
					writer.WriteMemberName("totalCost");
					writer.WriteMemberValue(data.totalCost);
					writer.WriteStructEnd();
				}
				writer.WriteStructEnd();
			}
			writer.WriteStructEnd();
		}

		private static void WritePlayModeTeleportInfo(WriterAdapter writer, SceneInnerPlayModeTeleportData data)
		{
			writer.WriteStructStart();
			if (0 <= data.ID)
			{
				writer.WriteMemberName("ID");
				writer.WriteMemberValue(data.ID);
			}
			if (!data.toBornPoints.IsNullOrEmpty())
			{
				writer.WriteMemberName("inner");
				WriteEP2BPTeleportInfo(writer, data.toBornPoints);
			}
			if (!data.toExitPoints.IsNullOrEmpty())
			{
				writer.WriteMemberName("outter");
				WriteBP2EPTeleportInfo(writer, data.toExitPoints);
			}
			writer.WriteStructEnd();
		}

		private static StringBuilder GetTeleportInfo_Lua(SceneInnerTeleportData data)
		{
			StringBuilder sb = new StringBuilder ();
			var writer = DataWriter.GetAdapter(new LuaWriter (sb));

			writer.WriteStructStart();

			if (null != data.pve)
			{
				writer.WriteMemberName("PVE");
				WritePlayModeTeleportInfo(writer, data.pve);
			}

			if (null != data.pvp)
			{
				writer.WriteMemberName("PVP");
				WritePlayModeTeleportInfo(writer, data.pvp);
			}

			if (!data.raids.IsNullOrEmpty())
			{
				writer.WriteMemberName("Raids");

				writer.WriteArrayStart();
				foreach (var raidData in data.raids)
				{
					WritePlayModeTeleportInfo(writer, raidData);
				}
				writer.WriteArrayEnd();
			}

			writer.WriteStructEnd();

			return sb;
		}

		public static void ClearTerrainLayer()
		{
			var objs = GameObject.FindObjectsOfType(typeof(Transform));
			if (objs.IsNullOrEmpty())
			{
				return;
			}
			foreach (var obj in objs)
			{
				var t = obj as Transform;
				if (t.gameObject.layer == RO.Config.Layer.TERRAIN.Value)
				{
					t.gameObject.layer = RO.Config.Layer.STATIC_OBJECT.Value;
				}
			}
		}

		private static BornPoint[] WriteBornPointsInfo(WriterAdapter writer, GameObject root = null)
		{
			writer.WriteArrayStart();

			var bps = null != root ? root.FindComponentsInChildren<BornPoint>() : GameObject.FindObjectsOfType<BornPoint>();
			foreach (var bp in bps)
			{
				if (!bp.gameObject.activeInHierarchy)
				{
					continue;
				}
				writer.WriteStructStart();

				writer.WriteMemberName ("ID");
				writer.WriteMemberValue(bp.ID);

				var position = bp.transform.position;
				writer.WriteMemberName ("position");
				writer.WriteArrayStart();
				writer.WriteMemberValue(position.x);
				writer.WriteMemberValue(position.y);
				writer.WriteMemberValue(position.z);
				writer.WriteArrayEnd();

				writer.WriteStructEnd();
			}

			writer.WriteArrayEnd();

			return bps;
		}

		private static ExitPoint[] WriteExitPointsInfo(WriterAdapter writer, GameObject root = null)
		{
			writer.WriteArrayStart();
			
			var eps = null != root ? root.FindComponentsInChildren<ExitPoint>() : GameObject.FindObjectsOfType<ExitPoint>();
			foreach (var ep in eps)
			{
				if (!ep.gameObject.activeInHierarchy)
				{
					continue;
				}
				writer.WriteStructStart();
				
				writer.WriteMemberName ("ID");
				writer.WriteMemberValue(ep.ID);
				
				var position = ep.transform.position;
				writer.WriteMemberName ("position");
				writer.WriteArrayStart();
				writer.WriteMemberValue(position.x);
				writer.WriteMemberValue(position.y);
				writer.WriteMemberValue(position.z);
				writer.WriteArrayEnd();

//				writer.WriteMemberName ("next_scene_type");
//				writer.WriteMemberValue(ep.nextSceneType);

				writer.WriteMemberName ("next_scene_ID");
				writer.WriteMemberValue(ep.nextSceneID);
				writer.WriteMemberName ("next_scene_born_point_ID");
				writer.WriteMemberValue(ep.nextSceneBornPointID);

//				switch (ep.nextSceneType)
//				{
//				case 1:
//					writer.WriteMemberName ("raid_ID");
//					writer.WriteMemberValue(ep.raidID);
//					break;
//				}

				writer.WriteMemberName ("type");
				writer.WriteMemberValue(System.Convert.ToInt32(ep.type));
				switch (ep.type)
				{
				case AreaTrigger.Type.CIRCLE:
					writer.WriteMemberName ("range");
					writer.WriteMemberValue(ep.range);
					break;
				case AreaTrigger.Type.RECTANGE:
					writer.WriteMemberName ("size");
					writer.WriteArrayStart();
					writer.WriteMemberValue(ep.size.x);
					writer.WriteMemberValue(ep.size.y);
					writer.WriteArrayEnd();
					break;
				}

				writer.WriteMemberName ("visible");
				writer.WriteMemberValue(ep.visible ? 1 : 0);
				
				writer.WriteMemberName ("questid");
				writer.WriteMemberValue(ep.questID);

				writer.WriteMemberName ("privategear");
				writer.WriteMemberValue(ep.Private ? 1 : 0);

				writer.WriteStructEnd();
			}
			
			writer.WriteArrayEnd();

			return eps;
		}

		private static void WriteNPCInfo(WriterAdapter writer, NPCInfo np)
		{
			writer.WriteMemberName ("uniqueid");
			writer.WriteMemberValue(np.UniqueID);

			writer.WriteMemberName ("id");
			writer.WriteMemberValue(np.ID);
			
			var position = np.transform.position;
			writer.WriteMemberName ("pos");
			writer.WriteArrayStart();
			writer.WriteMemberValue(position.x);
			writer.WriteMemberValue(position.y);
			writer.WriteMemberValue(position.z);
			writer.WriteArrayEnd();
			
			var rotation = np.transform.rotation.eulerAngles;
			writer.WriteMemberName ("dir");
			writer.WriteMemberValue(rotation.y);
			
			writer.WriteMemberName ("reborn");
			writer.WriteMemberValue(np.rebornTime);
			
			writer.WriteMemberName ("range");
			writer.WriteMemberValue(np.bornRange);
			
			writer.WriteMemberName ("territory");
			writer.WriteMemberValue(np.territory);

			writer.WriteMemberName ("search");
			writer.WriteMemberValue(np.searchRange);
			
			writer.WriteMemberName ("scale");
			writer.WriteArrayStart();
			writer.WriteMemberValue(np.scaleMin);
			writer.WriteMemberValue(np.scaleMax);
			writer.WriteArrayEnd();
			
			writer.WriteMemberName ("num");
			writer.WriteMemberValue(np.count);

#if OBSOLETE
			writer.WriteMemberName ("camp");
			writer.WriteMemberValue(np.camp.ToString());
#endif
			
			writer.WriteMemberName ("behavior");
			writer.WriteMemberValue(System.Convert.ToInt32(np.behaviours));

			writer.WriteMemberName ("ai");
			writer.WriteMemberValue(np.ai);

			writer.WriteMemberName ("level");
			writer.WriteMemberValue(np.level);

			writer.WriteMemberName ("life");
			writer.WriteMemberValue(np.life);

			writer.WriteMemberName ("orgstate");
			writer.WriteMemberValue(np.originState);

			writer.WriteMemberName ("privategear");
			writer.WriteMemberValue(np.Private ? 1 : 0);

			writer.WriteMemberName ("waitaction");
			writer.WriteMemberValue(np.waiAction);

			writer.WriteMemberName ("mapicon");
			writer.WriteMemberValue(np.mapIcon);

			writer.WriteMemberName ("ignorenavmesh");
			writer.WriteMemberValue(np.ignoreNavmesh ? 1 : 0);
		}

		private static NPCInfo[] WriteNPCPointsInfo(WriterAdapter writer, GameObject root = null)
		{
			writer.WriteArrayStart();
			
			var nps = null != root ? root.FindComponentsInChildren<NPCPoint>() : GameObject.FindObjectsOfType<NPCPoint>();
			foreach (var np in nps)
			{
				if (!np.gameObject.activeInHierarchy)
				{
					continue;
				}
				if (np.local)
				{
					continue;
				}
				writer.WriteStructStart();
				WriteNPCInfo(writer, np);
				writer.WriteStructEnd();
			}
			
			writer.WriteArrayEnd();
			return nps as NPCInfo[];
		}

		private static NPCInfo[] WriteNPCPointsInfo(CSVWriter writter, GameObject root = null)
		{
			var nps = null != root ? root.FindComponentsInChildren<NPCPoint>() : GameObject.FindObjectsOfType<NPCPoint>();
			if (nps.IsNullOrEmpty())
			{
				return null;
			}
			writter.WriteNewRow(new object[]{"[NPC]"});
			writter.WriteNewRow(new object[]{"ID", "UniqueID", "Count"});
			foreach (var np in nps)
			{
				if (!np.gameObject.activeInHierarchy)
				{
					continue;
				}
				if (np.local)
				{
					continue;
				}
				writter.WriteNewRow(new object[]{np.ID, np.UniqueID, np.count});
			}
			return nps as NPCInfo[];
		}

		private static NPCInfo[] WriteRaidNPCPointsInfo(WriterAdapter writer, GameObject root)
		{
			if (null == root)
			{
				return null;
			}

			writer.WriteArrayStart();
			
			var nps = root.FindComponentsInChildren<RaidNPCPoint>();
			foreach (var np in nps)
			{
				if (!np.gameObject.activeInHierarchy)
				{
					continue;
				}
				if (np.local)
				{
					continue;
				}
				writer.WriteStructStart();
				WriteNPCInfo(writer, np);
				writer.WriteStructEnd();
			}
			
			writer.WriteArrayEnd();
			return nps as NPCInfo[];
		}

		private static NPCInfo[] WriteRaidNPCPointsInfo(CSVWriter writter, GameObject root)
		{
			if (null == root)
			{
				return null;
			}
			var nps = root.FindComponentsInChildren<RaidNPCPoint>();
			if (nps.IsNullOrEmpty())
			{
				return null;
			}
			foreach (var np in nps)
			{
				if (!np.gameObject.activeInHierarchy)
				{
					continue;
				}
				if (np.local)
				{
					continue;
				}
				writter.WriteNewRow(new object[]{np.ID, np.UniqueID});
			}
			return nps as NPCInfo[];
		}

		private static void WriteSceneBaseInfo(WriterAdapter writer, GameObject root, bool withTeleportPath = true)
		{
			writer.WriteMemberName ("BornPoints");
			var bps = WriteBornPointsInfo(writer, root);
			
			writer.WriteMemberName ("ExitPoints");
			var eps = WriteExitPointsInfo(writer, root);
			
			writer.WriteMemberName ("NPCPoints");
			var nps = WriteNPCPointsInfo(writer, root);

			if (withTeleportPath)
			{
				BuildTeleportPath(bps, eps, nps);
			}
		}

		private static void WriteSceneRaidInfo(WriterAdapter writer, GameObject root)
		{
			writer.WriteMemberName ("RaidNPCPoints");
			WriteRaidNPCPointsInfo(writer, root);
		}

		private static void WriteSceneInfo(WriterAdapter writer)
		{
			writer.WriteStructStart();

			#region root
			var sceneRoot = GameObject.FindObjectOfType<SceneRoot>();
			sceneRoot.FindPVEAndPVP();
			GameObject root = null;
			if (null != sceneRoot)
			{
				root = null != sceneRoot.PVE ? sceneRoot.PVE : sceneRoot.gameObject;
			}
			WriteSceneBaseInfo(writer, root);

			if (null != sceneRoot && null != sceneRoot.PVP)
			{
				writer.WriteMemberName ("PVP");
				writer.WriteStructStart();
				WriteSceneBaseInfo(writer, sceneRoot.PVP, false);
				writer.WriteStructEnd();
			}
			#endregion root

			#region raid
			var raids = GameObject.FindObjectsOfType<SceneRaid>();
			if (!raids.IsNullOrEmpty())
			{
				writer.WriteMemberName ("Raids");
				writer.WriteStructStart();

				foreach (var raid in raids)
				{
					if (!raid.gameObject.activeInHierarchy)
					{
						continue;
					}
					writer.WriteMemberName (raid.ID);
					writer.WriteStructStart();

					var raidRoot = raid.gameObject;
					WriteSceneBaseInfo(writer, raidRoot, false);
					WriteSceneRaidInfo(writer, raidRoot);

					writer.WriteStructEnd();
				}

				writer.WriteStructEnd();
			}
			#endregion raid
			
			writer.WriteStructEnd();
		}

		private static void DeterminPVEAndPVP(GameObject root, out GameObject pveGO, out GameObject pvpGO)
		{
			GameObject mabePVE = null;
			GameObject mabePVP = null;
			GameObject pve = null;
			GameObject pvp = null;
			if (null != root)
			{
				root.FindGameObjectInChildren(delegate(GameObject go){
					if (!go.activeInHierarchy)
					{
						return false;
					}
					if (null == pve)
					{
						if (RO.Config.Tag.PVE == go.tag)
						{
							pve = go;
							return null != pvp;
						}
						else if (string.Equals("PVE", go.name))
						{
							mabePVE = go;
						}
					}
					if (null == pvp)
					{
						if (RO.Config.Tag.PVP == go.tag)
						{
							pvp = go;
							return null != pve;
						}
						else if (string.Equals("PVP", go.name))
						{
							mabePVP = go;
						}
					}
					return false;
				});
			}
			if (null == pve && null != mabePVE)
			{
				pve = mabePVE;
			}
			if (null == pvp && null != mabePVP)
			{
				pvp = mabePVP;
			}

			if (null == pve)
			{
				pve = root;
			}

			pveGO = pve;
			pvpGO = pvp;
		}

		private static void WriteSceneInfo(WriterAdapter writer, GameObject root, GameObject raidRoot)
		{
			writer.WriteStructStart();
			
			#region root
			GameObject pve;
			GameObject pvp;
			DeterminPVEAndPVP(root, out pve, out pvp);
			if (null != pve)
			{
				WriteSceneBaseInfo(writer, pve);
			}
			
			if (null != pvp)
			{
				writer.WriteMemberName ("PVP");
				writer.WriteStructStart();
				WriteSceneBaseInfo(writer, pvp, false);
				writer.WriteStructEnd();
			}
			#endregion root
			
			#region raid
			var raids = raidRoot.FindComponentsInChildren<SceneRaid>();
			if (!raids.IsNullOrEmpty())
			{
				writer.WriteMemberName ("Raids");
				writer.WriteStructStart();
				
				foreach (var raid in raids)
				{
					if (!raid.gameObject.activeInHierarchy)
					{
						continue;
					}
					writer.WriteMemberName (raid.ID);
					writer.WriteStructStart();
					
					var go = raid.gameObject;
					WriteSceneBaseInfo(writer, go);
					WriteSceneRaidInfo(writer, go);
					
					writer.WriteStructEnd();
				}
				
				writer.WriteStructEnd();
			}
			#endregion raid
			
			writer.WriteStructEnd();
		}
		
		private static void WriteSceneInfo(CSVWriter writter)
		{
			#region root
			var sceneRoot = GameObject.FindObjectOfType<SceneRoot>();
			sceneRoot.FindPVEAndPVP();
			GameObject pve = null;
			if (null != sceneRoot)
			{
				pve = null != sceneRoot.PVE ? sceneRoot.PVE : sceneRoot.gameObject;
			}
			writter.WriteNewRow(new object[]{"[PVE]"});
			WriteNPCPointsInfo(writter, pve);
			
			if (null != sceneRoot && null != sceneRoot.PVP)
			{
				writter.WriteNewRow(new object[]{"[PVP]"});
				WriteNPCPointsInfo(writter, sceneRoot.PVP);
			}
			#endregion root
			
			#region raid
			var raids = GameObject.FindObjectsOfType<SceneRaid>();
			if (!raids.IsNullOrEmpty())
			{
				foreach (var raid in raids)
				{
					if (!raid.gameObject.activeInHierarchy)
					{
						continue;
					}
					writter.WriteNewRow(new object[]{"[Raid]", raid.ID});
					WriteRaidNPCPointsInfo(writter, raid.gameObject);
				}
			}
			#endregion raid
		}
		
		private static void WriteSceneInfo(CSVWriter writter, GameObject root, GameObject raidRoot)
		{
			#region root
			GameObject pve;
			GameObject pvp;
			DeterminPVEAndPVP(root, out pve, out pvp);
			if (null != pve)
			{
				writter.WriteNewRow(new object[]{"[PVE]"});
				WriteNPCPointsInfo(writter, pve);
			}
			if (null != pvp)
			{
				writter.WriteNewRow(new object[]{"[PVP]"});
				WriteNPCPointsInfo(writter, pvp);
			}
			#endregion root
			
			#region raid
			var raids = raidRoot.FindComponentsInChildren<SceneRaid>();
			if (!raids.IsNullOrEmpty())
			{
				foreach (var raid in raids)
				{
					if (!raid.gameObject.activeInHierarchy)
					{
						continue;
					}
					writter.WriteNewRow(new object[]{"[Raid]", raid.ID});
					WriteRaidNPCPointsInfo(writter, raid.gameObject);
				}
			}
			#endregion raid
		}

		private static StringBuilder GetSceneInfo_Json()
		{
			StringBuilder sb = new StringBuilder ();
			var writer = DataWriter.GetAdapter(new JsonWriter (sb));
			WriteSceneInfo(writer);
			return sb;
		}

		private static StringBuilder GetSceneInfo_Lua(GameObject root, GameObject raidRoot)
		{
			StringBuilder sb = new StringBuilder ();
			var writer = DataWriter.GetAdapter(new LuaWriter (sb));
			if (null != root || null != raidRoot)
			{
				WriteSceneInfo(writer, root, raidRoot);
			}
			else
			{
				WriteSceneInfo(writer);
			}
			return sb;
		}

		private static StringBuilder GetSceneInfo_CSV(GameObject root, GameObject raidRoot)
		{
			StringBuilder sb = new StringBuilder ();
			var writter = new CSVWriter(SCENE_INFO_CSV_COL_COUNT, sb);
			if (null != root || null != raidRoot)
			{
				WriteSceneInfo(writter, root, raidRoot);
			}
			else
			{
				WriteSceneInfo(writter);
			}
			return sb;
		}

		private static void ExportToJson(string folder)
		{
			var file = PathUnity.Combine(folder, "SceneInfo.json");
			
			FileStream fs = null;
			StreamWriter sw = null;
			try
			{
				fs = new FileStream(file, File.Exists(file) ? FileMode.Truncate : FileMode.Create);
				sw = new StreamWriter(fs, /*Encoding.UTF8*/new UTF8Encoding(false));
				
				sw.Write(GetSceneInfo_Json().ToString());
				
				Debug.Log("Export SceneInfo Json Success");
			}
			catch (System.Exception e)
			{
				Debug.LogErrorFormat("Export SceneInfo Json Error: {0}\n{1}", e.Message, e.StackTrace);
			}
			finally
			{
				if (null != sw)
				{
					sw.Close();
				}
				if (null != fs)
				{
					fs.Close();
				}
			}
		}

		private static void ExportToLua(string folder, GameObject root = null, GameObject raidRoot = null)
		{
			var file = PathUnity.Combine(folder, "SceneInfo.lua");
			
			FileStream fs = null;
			StreamWriter sw = null;
			try
			{
				fs = new FileStream(file, File.Exists(file) ? FileMode.Truncate : FileMode.Create);
				sw = new StreamWriter(fs, new UTF8Encoding(false));
				
				sw.Write(GetSceneInfo_Lua(root, raidRoot).ToString());
				
				Debug.Log("Export SceneInfo Lua Success");
			}
			catch (System.Exception e)
			{
				Debug.LogErrorFormat("Export SceneInfo Lua Error: {0}\n{1}", e.Message, e.StackTrace);
			}
			finally
			{
				if (null != sw)
				{
					sw.Close();
				}
				if (null != fs)
				{
					fs.Close();
				}
			}
		}

		private static void ExportToCSV(string folder, GameObject root = null, GameObject raidRoot = null)
		{
			var file = PathUnity.Combine(folder, "SceneInfo.csv");
			
			FileStream fs = null;
			StreamWriter sw = null;
			try
			{
				fs = new FileStream(file, File.Exists(file) ? FileMode.Truncate : FileMode.Create);
				sw = new StreamWriter(fs, new UTF8Encoding(false));
				
				sw.Write(GetSceneInfo_CSV(root, raidRoot).ToString());
				
				Debug.Log("Export SceneInfo CSV Success");
			}
			catch (System.Exception e)
			{
				Debug.LogErrorFormat("Export SceneInfo CSV Error: {0}\n{1}", e.Message, e.StackTrace);
			}
			finally
			{
				if (null != sw)
				{
					sw.Close();
				}
				if (null != fs)
				{
					fs.Close();
				}
			}
		}

		public static void DoExport(NavMeshInfo navMeshInfo)
		{

			EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

			// nav mesh
			NavMeshCommands.DoExport(navMeshInfo);

			// info
			var sceneName = Path.GetFileNameWithoutExtension(EditorSceneManager.GetActiveScene().name);
			string folder = PathUnity.Combine(RO.Config.Export.SCENE_DIRECTORY, sceneName);
			if (!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
			}

			ExportToJson(folder);
			ExportToLua(folder);
			ExportToCSV(folder);
			
			EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
		}

		[MenuItem("RO/Scene/ClearLightMapping")]
		static void Menu_ClearLightMapping()
		{
			Lightmapping.Clear();
		}

		[MenuItem("RO/Scene/BuildTeleportPath")]
		static void Menu_BuildTeleportPath()
		{
			BuildTeleportPath();
		}

//		[MenuItem("RO/Scene/ClearTerrainLayer")]
//		static void Menu_ClearTerrainLayer()
//		{
//			ClearTerrainLayer();
//		}
		
	}
} // namespace EditorTool
