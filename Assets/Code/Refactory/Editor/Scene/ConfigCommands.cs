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
	public static class ConfigCommands 
	{
		private static float DESTINATION_VALID_RANGE = 5;
		private static string SCENE_INNER_TELEPORT_DATA_NAME = "TeleportData.asset";
		private static int SCENE_INFO_CSV_COL_COUNT = 3;

		private static void BuildTeleportPath(BornPoint[] bps, ExitPoint[] eps, NPCInfo[] nps)
		{
			Dictionary<int, BornPoint> bpsMap = null;
			if (!bps.IsNullOrEmpty())
			{
				bpsMap = new Dictionary<int, BornPoint>();
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
				bps = bpsMap.Values.ToArray();
			}

			if (!eps.IsNullOrEmpty())
			{
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
					if (0 == ep.nextSceneID && null != bpsMap)
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
				eps = epsMap.Values.ToArray();
			}

			if (bps.IsNullOrEmpty() || eps.IsNullOrEmpty())
			{
				return;
			}
			
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
						var info = otherBP
.toBPS.Find(delegate(ToBornPointInfo obj) {
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

				writer.WriteMemberName ("range");
				writer.WriteMemberValue(bp.range);
				
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

			writer.WriteMemberName ("pursue");
			writer.WriteMemberValue(np.pursue);

			writer.WriteMemberName ("pursuetime");
			writer.WriteMemberValue(np.pursueTime);
			
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
		
		private static void WriteSceneRaidInfo(WriterAdapter writer, SceneRaid raid)
		{
			writer.WriteMemberName ("RaidNPCPoints");
			WriteRaidNPCPointsInfo(writer, raid.gameObject);
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

		private static BornPoint[] WriteBornPointsInfo_Config(WriterAdapter writer, System.Action initCallback, GameObject root = null)
		{
			var inited = false;
			
			var bps = null != root ? root.FindComponentsInChildren<BornPoint>() : GameObject.FindObjectsOfType<BornPoint>();
			foreach (var bp in bps)
			{
				if (!bp.gameObject.activeInHierarchy)
				{
					continue;
				}
				if (!inited)
				{
					inited = true;
					initCallback();
					
					writer.WriteArrayStart();
				}
				writer.WriteStructStart();
				
				writer.WriteMemberName ("ID");
				writer.WriteMemberValue(bp.ID);
				
				writer.WriteMemberName ("position");
				ExportUtils.ExportVector(writer, bp.transform.position);

				writer.WriteMemberName ("range");
				writer.WriteMemberValue(bp.range);
				
				writer.WriteStructEnd();
			}
			
			if (inited)
			{
				writer.WriteArrayEnd();
			}
			
			return bps;
		}
		
		private static ExitPoint[] WriteExitPointsInfo_Config(WriterAdapter writer, System.Action initCallback, GameObject root = null)
		{
			var inited = false;
			
			var eps = null != root ? root.FindComponentsInChildren<ExitPoint>() : GameObject.FindObjectsOfType<ExitPoint>();
			foreach (var ep in eps)
			{
				if (!ep.gameObject.activeInHierarchy)
				{
					continue;
				}
				if (!inited)
				{
					inited = true;
					initCallback();
					
					writer.WriteArrayStart();
				}
				writer.WriteStructStart();
				
				writer.WriteMemberName ("ID");
				writer.WriteMemberValue(ep.ID);

				writer.WriteMemberName ("commonEffectID");
				writer.WriteMemberValue(ep.commonEffectID);
				
				writer.WriteMemberName ("position");
				ExportUtils.ExportVector(writer, ep.transform.position);
				
				writer.WriteMemberName ("nextSceneID");
				writer.WriteMemberValue(ep.nextSceneID);

				writer.WriteMemberName ("nextSceneBornPointID");
				writer.WriteMemberValue(ep.nextSceneBornPointID);
				
				writer.WriteMemberName ("type");
				ExportUtils.ExportEnum(writer, ep.type);
				switch (ep.type)
				{
				case AreaTrigger.Type.CIRCLE:
					writer.WriteMemberName ("range");
					writer.WriteMemberValue(ep.range);
					break;
				case AreaTrigger.Type.RECTANGE:
					writer.WriteMemberName ("size");
					ExportUtils.ExportVector(writer, ep.size);
					break;
				}
				
				writer.WriteStructEnd();
			}
			
			if (inited)
			{
				writer.WriteArrayEnd();
			}
			
			return eps;
		}
		
		private static NPCInfo[] WriteNPCPointsInfo_Config(WriterAdapter writer, System.Action initCallback, GameObject root = null)
		{
			var inited = false;
			
			var nps = null != root ? root.FindComponentsInChildren<NPCInfo>() : GameObject.FindObjectsOfType<NPCInfo>();
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
				if (!inited)
				{
					inited = true;
					initCallback();
					
					writer.WriteArrayStart();
				}
				writer.WriteStructStart();

				writer.WriteMemberName ("uniqueID");
				writer.WriteMemberValue(np.UniqueID);
				
				writer.WriteMemberName ("ID");
				writer.WriteMemberValue(np.ID);
				
				writer.WriteMemberName ("position");
				ExportUtils.ExportVector(writer, np.transform.position);

				writer.WriteStructEnd();
			}
			
			if (inited)
			{
				writer.WriteArrayEnd();
			}
			return nps as NPCInfo[];
		}

		private static void WriteSceneBaseInfo_Config(WriterAdapter writer, System.Action initCallback, GameObject root, CameraController.Info cameraInfo = null)
		{
			var inited = false;

			if (null != cameraInfo)
			{
				if (!inited)
				{
					inited = true;
					initCallback();
					writer.WriteStructStart();
				}
				writer.WriteMemberName ("cameraInfo");
				WriteCameraInfo_Config(writer, cameraInfo);
			}

			var bpsInited = false;
			WriteBornPointsInfo_Config(writer, delegate {
				if (!inited)
				{
					inited = true;
					initCallback();
					writer.WriteStructStart();
				}
				if (!bpsInited)
				{
					bpsInited = true;
					writer.WriteMemberName ("bps");
				}
			}, root);

			var epsInited = false;
			WriteExitPointsInfo_Config(writer, delegate {
				if (!inited)
				{
					inited = true;
					initCallback();
					writer.WriteStructStart();
				}
				if (!epsInited)
				{
					epsInited = true;
					writer.WriteMemberName ("eps");
				}
			}, root);
			
			var npsInited = false;
			WriteNPCPointsInfo_Config(writer, delegate {
				if (!inited)
				{
					inited = true;
					initCallback();
					writer.WriteStructStart();
				}
				if (!npsInited)
				{
					npsInited = true;
					writer.WriteMemberName ("nps");
				}
			}, root);

			if (inited)
			{
				writer.WriteStructEnd();
			}
		}

		private static void WriteCameraInfo_Config(WriterAdapter writer, CameraController.Info info)
		{
			writer.WriteStructStart();

			var p = info.focusOffset;
			writer.WriteMemberName ("focusOffset");
			writer.WriteArrayStart();
			writer.WriteMemberValue(p.x);
			writer.WriteMemberValue(p.y);
			writer.WriteMemberValue(p.z);
			writer.WriteArrayEnd();

			p = info.focusViewPort;
			writer.WriteMemberName ("focusViewPort");
			writer.WriteArrayStart();
			writer.WriteMemberValue(p.x);
			writer.WriteMemberValue(p.y);
			writer.WriteMemberValue(p.z);
			writer.WriteArrayEnd();

			p = info.rotation;
			writer.WriteMemberName ("rotation");
			writer.WriteArrayStart();
			writer.WriteMemberValue(p.x);
			writer.WriteMemberValue(p.y);
			writer.WriteMemberValue(p.z);
			writer.WriteArrayEnd();

			writer.WriteMemberName ("fieldOfView");
			writer.WriteMemberValue(info.fieldOfView);

//			public Vector3 cameraPosition = Vector3.zero;
//			public bool cameraPositionLocked = false;

			writer.WriteStructEnd();
		}

		private static RoomPoint[] WriteRoomPointsInfo_Config(WriterAdapter writer, System.Action initCallback, GameObject root = null)
		{
			var inited = false;
			
			var rps = null != root ? root.FindComponentsInChildren<RoomPoint>() : GameObject.FindObjectsOfType<RoomPoint>();
			foreach (var p in rps)
			{
				if (!p.gameObject.activeInHierarchy)
				{
					continue;
				}
				if (!inited)
				{
					inited = true;
					initCallback();
					
					writer.WriteArrayStart();
				}
				writer.WriteStructStart();
				
				writer.WriteMemberName ("position");
				ExportUtils.ExportVector(writer, p.transform.position);
				
				writer.WriteMemberName ("type");
				ExportUtils.ExportEnum(writer, p.type);
				switch (p.type)
				{
				case AreaTrigger.Type.CIRCLE:
					writer.WriteMemberName ("range");
					writer.WriteMemberValue(p.range);
					break;
				case AreaTrigger.Type.RECTANGE:
					writer.WriteMemberName ("size");
					ExportUtils.ExportVector(writer, p.size);
					break;
				}
				
				writer.WriteStructEnd();
			}
			
			if (inited)
			{
				writer.WriteArrayEnd();
			}
			
			return rps;
		}

		private static void WriteScenePublicInfo_Config(WriterAdapter writer, System.Action initCallback, GameObject root)
		{
			var inited = false;
			
			var bpsInited = false;
			WriteRoomPointsInfo_Config(writer, delegate {
				if (!inited)
				{
					inited = true;
					initCallback();
					writer.WriteStructStart();
				}
				if (!bpsInited)
				{
					bpsInited = true;
					writer.WriteMemberName ("rps");
				}
			}, root);
			
			if (inited)
			{
				writer.WriteStructEnd();
			}
		}

		private static void WriteSceneInfo_Config(WriterAdapter writer, GameObject pve, GameObject pvp, GameObject raidRoot, params List<LuaGameObject>[] objlist)
		{
			writer.WriteStructStart();

			#region public
			WriteScenePublicInfo_Config(writer, delegate() {
				writer.WriteMemberName ("Public");
			}, null);
			#endregion public

			#region root
			if (null != pve)
			{
				WriteSceneBaseInfo_Config(writer, delegate() {
					writer.WriteMemberName ("PVE");
				}, pve);
			}
			
			if (null != pvp)
			{
				WriteSceneBaseInfo_Config(writer, delegate() {
					writer.WriteMemberName ("PVP");
				}, pvp);
			}
			#endregion root

			#region raid
			SceneRaid[] raids = null;
			if (null != raidRoot)
			{
				raids = raidRoot.FindComponentsInChildren<SceneRaid>();
			}
			else
			{
				raids = GameObject.FindObjectsOfType<SceneRaid>();
			}
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
					CameraController.Info cameraInfo = raid.cameraInfoEnable ? raid.cameraInfo : null;
					WriteSceneBaseInfo_Config(writer, delegate() {
						writer.WriteMemberName (raid.ID);
					}, raid.gameObject, cameraInfo);
				}
				
				writer.WriteStructEnd();
			}
			#endregion raid

			var guildFlags = objlist [0];
			if (0 < guildFlags.Count)
			{
				writer.WriteMemberName ("GuildFlags");
				writer.WriteStructStart();

				foreach (var obj in guildFlags)
				{
					LuaGameObject lobj = obj as LuaGameObject;

					writer.WriteMemberName(obj.ID);

					writer.WriteStructStart ();

					writer.WriteMemberName ("strongHoldId");
					writer.WriteMemberValue (int.Parse(lobj.GetProperty (0)));

					writer.WriteMemberName ("position");
					ExportUtils.ExportVector(writer, obj.transform.position);

					writer.WriteStructEnd ();
				}

				writer.WriteStructEnd();
			}

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
		
		private static void WriteSceneInfo(WriterAdapter writer, WriterAdapter configWriter, GameObject root, GameObject raidRoot)
		{
			writer.WriteStructStart();
			
			#region root
			GameObject pve;
			GameObject pvp;

			if (null == root)
			{
				pve = null;
				pvp = null;
				var sceneRoot = GameObject.FindObjectOfType<SceneRoot>();
				if (null != sceneRoot)
				{
					if (null != sceneRoot.PVE)
					{
						pve = sceneRoot.PVE;
						pvp = sceneRoot.PVP;
					}
					else
					{
						pve = sceneRoot.gameObject;
					}
				}
				sceneRoot.FindPVEAndPVP();
			}
			else
			{
				DeterminPVEAndPVP(root, out pve, out pvp);
			}

			if (null != pve)
			{
				WriteSceneBaseInfo(writer, pve, false);
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
			SceneRaid[] raids = null;
			if (null != raidRoot)
			{
				raids = raidRoot.FindComponentsInChildren<SceneRaid>();
			}
			else
			{
				raids = GameObject.FindObjectsOfType<SceneRaid>();
			}
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
					WriteSceneRaidInfo(writer, raid);
					
					writer.WriteStructEnd();
				}
				
				writer.WriteStructEnd();
			}
			#endregion raid

			#region LuaGameObject
			var clickableObjs = Object.FindObjectsOfType<LuaGameObjectClickable>();
				
			var guildFlags = new List<LuaGameObject>();
			var guildPhotoFrames = new List<LuaGameObject>();
			var weddingPhotoFrames = new List<LuaGameObject>();

			if (!clickableObjs.IsNullOrEmpty())
			{
				
				foreach (var obj in clickableObjs)
				{
					if (obj.gameObject.activeInHierarchy)
					{
						switch (obj.type)
						{
						case 10: // guild photo frame
							guildPhotoFrames.Add(obj);
							break;
						case 11: // guild flag
							guildFlags.Add(obj);
							break;
						case 12: // wedding photo frame
							weddingPhotoFrames.Add(obj);
							break;
						}
					}
				}
				if (0 < guildFlags.Count)
				{
					writer.WriteMemberName ("GuildFlags");
					writer.WriteStructStart();

					foreach (var obj in guildFlags)
					{
						LuaGameObject lobj = obj as LuaGameObject;

						writer.WriteMemberName(obj.ID);

						writer.WriteStructStart ();

						writer.WriteMemberName ("strongHoldId");
						writer.WriteMemberValue (int.Parse(lobj.GetProperty (0)));

						writer.WriteMemberName ("position");
						ExportUtils.ExportVector(writer, obj.transform.position);

						writer.WriteStructEnd ();
					}

					writer.WriteStructEnd();
				}
				if (0 < guildPhotoFrames.Count)
				{
					writer.WriteMemberName ("GuildPhotoFrames");
					writer.WriteStructStart();

					foreach (var obj in guildPhotoFrames)
					{
						writer.WriteMemberName(obj.ID);
						ExportUtils.ExportVector(writer, obj.transform.position);
					}

					writer.WriteStructEnd();
				}
				if (0 < weddingPhotoFrames.Count)
				{
					writer.WriteMemberName ("WeddingPhotoFrames");
					writer.WriteStructStart();

					foreach (var obj in weddingPhotoFrames)
					{
						writer.WriteMemberName(obj.ID);
						ExportUtils.ExportVector(writer, obj.transform.position);
					}

					writer.WriteStructEnd();
				}
			}
			#endregion LuaGameObject

			writer.WriteStructEnd();

			WriteSceneInfo_Config(configWriter, pve, pvp, raidRoot, guildFlags);
		}
		
		private static StringBuilder GetSceneInfo_Lua(GameObject root, GameObject raidRoot, string configName, out StringBuilder configInfo)
		{
			StringBuilder sb = new StringBuilder ();
			var writer = DataWriter.GetAdapter(new LuaWriter (sb));

			configInfo = new StringBuilder();
			var configWriter = DataWriter.GetAdapter(new LuaWriter (configInfo), configName, true);
			WriteSceneInfo(writer, configWriter, root, raidRoot);
			configInfo.AppendLine();
			configInfo.AppendFormat("return {0}", configName);
			return sb;
		}
		
		private static void ExportToLua(string folder, string configPath, GameObject root = null, GameObject raidRoot = null)
		{
			StringBuilder configInfo;
			var exportStr = GetSceneInfo_Lua(root, raidRoot, Path.GetFileNameWithoutExtension(configPath), out configInfo).ToString();
			

			var file = PathUnity.Combine(folder, "SceneInfo.lua");
			
			FileStream fs = null;
			StreamWriter sw = null;

			FileStream fs1 = null;
			StreamWriter sw1 = null;
			try
			{
				fs = new FileStream(file, File.Exists(file) ? FileMode.Truncate : FileMode.Create);
				sw = new StreamWriter(fs, new UTF8Encoding(false));

				fs1 = new FileStream(configPath, File.Exists(configPath) ? FileMode.Truncate : FileMode.Create);
				sw1 = new StreamWriter(fs1, new UTF8Encoding(false));

				sw.Write(exportStr);
				sw1.Write(configInfo.ToString());
				
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

				if (null != sw1)
				{
					sw1.Close();
				}
				if (null != fs1)
				{
					fs1.Close();
				}
			}
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

		private static void WriteSceneInfo(CSVWriter writter, GameObject root, GameObject raidRoot)
		{
			#region root
			GameObject pve;
			GameObject pvp;

			if (null == root)
			{
				pve = null;
				pvp = null;
				var sceneRoot = GameObject.FindObjectOfType<SceneRoot>();
				if (null != sceneRoot)
				{
					if (null != sceneRoot.PVE)
					{
						pve = sceneRoot.PVE;
						pvp = sceneRoot.PVP;
					}
					else
					{
						pve = sceneRoot.gameObject;
					}
				}
				sceneRoot.FindPVEAndPVP();
			}
			else
			{
				DeterminPVEAndPVP(root, out pve, out pvp);
			}

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
			SceneRaid[] raids = null;
			if (null != raidRoot)
			{
				raids = raidRoot.FindComponentsInChildren<SceneRaid>();
			}
			else
			{
				raids = GameObject.FindObjectsOfType<SceneRaid>();

			}
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

		private static StringBuilder GetSceneInfo_CSV(GameObject root, GameObject raidRoot)
		{
			StringBuilder sb = new StringBuilder ();
			var writter = new CSVWriter(SCENE_INFO_CSV_COL_COUNT, sb);
			WriteSceneInfo(writter, root, raidRoot);
			return sb;
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
		
		public static void ExportScene(NavMeshInfo navMeshInfo)
		{
			//EditorApplication.SaveScene();
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

			var configName = sceneName.StartsWith("Scene") ? sceneName.Insert(5, "_") : sceneName.Insert(0, "Scene_");
			var configPath = PathUnity.Combine(
				PathConfig.DIRECTORY_ASSETS, 
				string.Format ("Resources/Script/Refactory/Config/Scene/{0}.txt", configName));

			ExportToLua(folder, configPath);
			ExportToCSV(folder);
			
			//EditorApplication.SaveScene();
			EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
		}

		[MenuItem("RO/Config/BuildTeleportPath")]
		static void Menu_BuildTeleportPath()
		{
			BuildTeleportPath();
		}

		[MenuItem("RO/Config/ExportScene")]
		static void Menu_ExportScene()
		{
			ExportScene(null);
		}

		[MenuItem("RO/Config/ClearEnviromentSetting")]
		static void Menu_ClearEnviromentSetting()
		{
			EnviromentSetting.ClearAll();
		}

        
		//[MenuItem("Tools/刷新Excel文档")]
		[MenuItem("OverSeasTool/刷新Excel文档")]
		public static void ExportKrExcel()
        {
            //RunCommand(Application.dataPath + "/Code/Refactory/Editor/Scene/oversea/lang/export.php kr tw");
			RunCommand(Application.dataPath + "/../../../oversea/lang/export.php kr tw");
        }

		public static void RunCommand(string param)
		{
			
			string cmd = @"#!/bin/bash
                           # 实属无奈，语言包excel太大，unity调用php读取excel竟然一直卡死，直接terminal调用都没问题
                           "
				           + param;

			File.WriteAllText("/tmp/oversea", cmd);

			CommandHelper.ExcuteExternalCommand("chmod", "+x /tmp/oversea");
			CommandHelper.ExcuteExternalCommand("open", "/tmp/oversea");
            
		}
	
	}
} // namespace EditorTool
