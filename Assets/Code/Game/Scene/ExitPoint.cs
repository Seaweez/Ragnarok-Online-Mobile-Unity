using UnityEngine;
using System.Collections.Generic;
using Ghost.Utils;
using Ghost.Extensions;
using Ghost.Attribute;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class ExitPoint : AreaTrigger 
	{
		[SerializeField, SetProperty("ID")]
		private int ID_ = 0;
		public int ID
		{
			get
			{
				return ID_;
			}
			set
			{
				ID_ = value;
				gameObject.name = string.Format("ep_{0}", ID);
			}
		}

		public string mapIcon = "map_portal";

		public int nextSceneID = 0;
		public int nextSceneBornPointID = 0;
//		public int nextSceneType = 0;
//		public int raidID = 0;

		public bool visible = true;
		public int questID = 0;
		public bool Private = false;

		public int commonEffectID = 16;

		public List<ToBornPointInfo> from = null;
		public BornPoint to = null;
		public List<ToBornPointTeleportPathInfo> teleportPaths = null;

		private Dictionary<int, ToBornPointTeleportPathInfo> teleportPathMap = null;

		public override void OnRoleEnter(Transform t)
		{
		}

		public void BuildTeleportTo(BornPoint bp)
		{
			var visitedEPS = new HashSet<ExitPoint>();
			DoBuildTeleportTo(bp, visitedEPS);
		}

		private float DoBuildTeleportTo(BornPoint bp, HashSet<ExitPoint> visitedEPS)
		{
			visitedEPS.Add(this);
			if (Private)
			{
				// ignore private ep
				return -1;
			}

			if (null == to || 0 != nextSceneID)
			{
				// no teleport
				return -1;
			}

			if (null != teleportPaths)
			{
				var teleportInfo = teleportPaths.Find(delegate(ToBornPointTeleportPathInfo obj) {
					return obj.bp == bp;
				});
				if (null != teleportInfo)
				{
					// already built
					return teleportInfo.totalCost;
				}
			}

			if (to == bp)
			{
				// direct teleport
				if (null == teleportPaths)
				{
					teleportPaths = new List<ToBornPointTeleportPathInfo>();
				}
				teleportPaths.Add(new ToBornPointTeleportPathInfo(bp, null, 1));
				return 1;
			}

			if (!from.IsNullOrEmpty())
			{
				var info = from.Find(delegate(ToBornPointInfo obj) {
					return obj.bp == bp;
				});
				if (null != info)
				{
					// direct arrive, no need to teleport
					return -1;
				}
			}

			float cost = -1;

			var minCost = float.PositiveInfinity;
			ExitPoint nextEP = null;
			
			if (!to.to.IsNullOrEmpty())
			{
				// next can arrive eps
				foreach (var epInfo in to.to)
				{
					if (visitedEPS.Contains(epInfo.ep))
					{
						continue;
					}
					var nextCost = epInfo.ep.DoBuildTeleportTo(bp, visitedEPS);
					if (0 > nextCost)
					{
						continue;
					}
					nextCost += epInfo.cost;
					if (minCost > nextCost)
					{
						minCost = nextCost;
						nextEP = epInfo.ep;
					}
				}
			}
			if (!to.toBPS.IsNullOrEmpty())
			{
				foreach (var bpInfo in to.toBPS)
				{
					if (bpInfo.bp == bp)
					{
						if (minCost > bpInfo.cost)
						{
							cost = bpInfo.cost + 1;
							nextEP = null;
						}
						break;
					}
				}
			}
			
			if (null != nextEP)
			{
				cost = minCost + 1;
			}
			else if (0 > cost)
			{
				return -1;
			}

			if (null == teleportPaths)
			{
				teleportPaths = new List<ToBornPointTeleportPathInfo>();
			}
			teleportPaths.Add(new ToBornPointTeleportPathInfo(bp, nextEP, cost));
			return cost;
		}

		public ToBornPointTeleportPathInfo TryGetTeleportPathTo(int bpID)
		{
			if (null == teleportPathMap)
			{
				return null;
			}
			ToBornPointTeleportPathInfo path;
			if (!teleportPathMap.TryGetValue(bpID, out path))
			{
				return null;
			}
			return path;
		}

		void Start()
		{
#if UNITY_EDITOR
#else
			GameObject.Destroy(gameObject);
#endif
		}

		void OnDestroy()
		{
#if OBSOLETE
			if (null != ExitPointManager.Me)
			{
				ExitPointManager.Me.Remove(this);
			}
#endif
		}

#if DEBUG_DRAW
		protected override void DebugDraw(Color color)
		{
			base.DebugDraw(color);
			Gizmos.DrawIcon(transform.position, "ep.png", true);
		}
#endif // DEBUG_DRAW
	}

} // namespace RO
