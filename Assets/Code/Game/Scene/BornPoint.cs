using UnityEngine;
using System.Collections.Generic;
using Ghost.Utils;
using Ghost.Attribute;
using Ghost.Extensions;

namespace RO
{
	[System.Serializable, SLua.CustomLuaClassAttribute]
	public class ToExitPointInfo
	{
		public ExitPoint ep;
		public float cost;
		
		public ToExitPointInfo(ExitPoint e, float c)
		{
			ep = e;
			cost = c;
		}
	}

	[System.Serializable, SLua.CustomLuaClassAttribute]
	public class ToBornPointInfo
	{
		public BornPoint bp;
		public float cost;
		
		public ToBornPointInfo(BornPoint b, float c)
		{
			bp = b;
			cost = c;
		}
	}

	[System.Serializable, SLua.CustomLuaClassAttribute]
	public class ToExitPointTeleportPathInfo
	{
		public ExitPoint ep;
		public BornPoint bp;
		public ExitPoint nextEP; // be null if can direct arrive ep
		public float totalCost;
		
		public ToExitPointTeleportPathInfo(ExitPoint e, BornPoint b, ExitPoint ne, float c)
		{
			ep = e;
			bp = b;
			nextEP = ne;
			totalCost = c;
		}
	}

	[System.Serializable, SLua.CustomLuaClassAttribute]
	public class ToBornPointTeleportPathInfo
	{
		public BornPoint bp;
		public ExitPoint nextEP; // be null if "to" can arrive bp
		public float totalCost;
		
		public ToBornPointTeleportPathInfo(BornPoint b, ExitPoint e, float c)
		{
			bp = b;
			nextEP = e;
			totalCost = c;
		}
	}

	[SLua.CustomLuaClassAttribute]
	public class BornPoint : MonoBehaviour 
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
				gameObject.name = string.Format("bp_{0}", ID);
			}
		}
		public float range = 0;

		public Vector3 position
		{
			get
			{
				return transform.position;
			}
		}

		public List<ExitPoint> from = null;
		public List<ToExitPointInfo> to = null;
		public List<ToBornPointInfo> toBPS = null;
		public List<ToExitPointTeleportPathInfo> teleportPaths = null;
		
		private Dictionary<int, ToExitPointTeleportPathInfo> teleportPathMap = null;

		public void BuildTeleportTo(ExitPoint ep)
		{
			if (!teleportPaths.IsNullOrEmpty())
			{
				var teleportInfo = teleportPaths.Find(delegate(ToExitPointTeleportPathInfo obj) {
					return obj.ep == ep;
				});
				if (null != teleportInfo)
				{
					// already built
					return;
				}
			}

			if (!to.IsNullOrEmpty())
			{
				var toInfo = to.Find(delegate(ToExitPointInfo obj) {
					return obj.ep == ep;
				});
				if (null != toInfo)
				{
					// can arrive
					if (null == teleportPaths)
					{
						teleportPaths = new List<ToExitPointTeleportPathInfo>();
					}
					teleportPaths.Add(new ToExitPointTeleportPathInfo(ep, null, null, toInfo.cost));
					return;
				}

				if (!ep.from.IsNullOrEmpty())
				{
					var minCost = float.PositiveInfinity;
					ExitPoint startEP = null;
					BornPoint endBP = null;
					foreach (var bpInfo in ep.from)
					{
						foreach (var epInfo in to)
						{
							if (!epInfo.ep.teleportPaths.IsNullOrEmpty())
							{
								var teleportInfo = epInfo.ep.teleportPaths.Find(delegate(ToBornPointTeleportPathInfo obj) {
									return obj.bp == bpInfo.bp;
								});
								if (null != teleportInfo)
								{
									var cost = bpInfo.cost + teleportInfo.totalCost;
									if (minCost > cost)
									{
										minCost = cost;
										startEP = epInfo.ep;
										endBP = bpInfo.bp;
									}
								}
							}
						}
					}

					if (null != startEP)
					{
						if (null == teleportPaths)
						{
							teleportPaths = new List<ToExitPointTeleportPathInfo>();
						}
						teleportPaths.Add(new ToExitPointTeleportPathInfo(ep, endBP, startEP, minCost));
					}
				}
			}
		}

		public ToExitPointTeleportPathInfo TryGetTeleportPathTo(int epID)
		{
			if (null == teleportPathMap)
			{
				return null;
			}
			ToExitPointTeleportPathInfo path;
			if (!teleportPathMap.TryGetValue(epID, out path))
			{
				return null;
			}
			return path;
		}
		
		#if OBSOLETE
		void Start()
		{
			if (!(null != BornPointManager.Me && BornPointManager.Me.Add(this)))
			{
				GameObject.Destroy(gameObject);
			}
			else
			{
				/*** no need teleport paths, they are all in lua table "MapTeleport" ***/
//				if (!teleportPaths.IsNullOrEmpty())
//				{
//					teleportPathMap = new Dictionary<int, ToExitPointTeleportPathInfo>(teleportPaths.Count);
//					foreach (var path in teleportPaths)
//					{
//						teleportPathMap.Add(path.ep.ID, path);
//					}
//				}
				teleportPaths = null;
			}
		}

		void OnDestroy()
		{
			if (null != BornPointManager.Me)
			{
				BornPointManager.Me.Remove(this);
			}
		}
		#endif

#if DEBUG_DRAW
		private void DebugDraw(Color color)
		{
			DebugUtils.DrawCircle(transform.position, Quaternion.identity, 0.5f, 30, color);
			if (0 < range)
			{
				DebugUtils.DrawCircle(transform.position, Quaternion.identity, range, 30, Color.blue);
			}
			if (null != from)
			{
				Gizmos.color = Color.yellow;
				foreach(var ep in from)
				{
					Gizmos.DrawLine(ep.position, position);
				}
			}

			if (null != to)
			{
				Gizmos.color = Color.green;
				foreach(var epInfo in to)
				{
					Gizmos.DrawLine(epInfo.ep.position, position);
				}
			}

			Gizmos.DrawIcon(transform.position, "bp.png", true);
		}

		void OnDrawGizmos()
		{
			DebugDraw(Color.green);
		}
		
		void OnDrawGizmosSelected()
		{
			DebugDraw(Color.red);
		}
#endif // DEBUG_DRAW

	}

} // namespace RO
