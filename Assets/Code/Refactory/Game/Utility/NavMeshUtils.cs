using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;
using Ghost.Utils;
using RO;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class NavMeshPathAgent
	{
		public UnityEngine.AI.NavMeshPath path{get;private set;}
		public bool calcPending{get;private set;}
		public Vector3[] corners{get;private set;}
		public bool cornersValid{get;private set;}

		public bool complete{get{return UnityEngine.AI.NavMeshPathStatus.PathComplete == path.status;}}
		public bool completePartial{get{return UnityEngine.AI.NavMeshPathStatus.PathPartial == path.status;}}
		public bool invalid{get{return UnityEngine.AI.NavMeshPathStatus.PathInvalid == path.status;}}

		public bool idle{get;private set;}

		public NavMeshPathAgent()
		{
			path = new UnityEngine.AI.NavMeshPath();
			calcPending = false;
			cornersValid = false;

			idle = true;
		}

		public void Clear()
		{
			path.ClearCorners();
			calcPending = false;
			cornersValid = false;
			idle = true;
		}
		
		public bool Calc(Vector3 p1, Vector3 p2, out float x, out float y, out float z)
		{
			if (!idle)
			{
				Clear();
			}
			else
			{
				idle = false;
			}

			var ret = false;
			if (NavMeshUtils.CalcPath(p1, p2, path))
			{
				calcPending = true;
				Update();
				if (cornersValid)
				{
					p2 = corners[corners.Length-1];
					ret = true;
				}
			}
			else
			{
				calcPending = false;
			}
			x = p2.x;
			y = p2.y;
			z = p2.z;
			return ret;
		}

		public bool GetCorner(int i, out float x, out float y, out float z)
		{
			Update();
			if (cornersValid && corners.CheckIndex(i))
			{
				var c = corners[i];
				x = c.x;
				y = c.y;
				z = c.z;
				return true;
			}
			else
			{
				x = 0;
				y = 0;
				z = 0;
			}
			return false;
		}
		
		public void Update()
		{
			if (idle)
			{
				return;
			}
			if (calcPending)
			{
				switch (path.status)
				{
				case UnityEngine.AI.NavMeshPathStatus.PathComplete:
				case UnityEngine.AI.NavMeshPathStatus.PathPartial:
					calcPending = false;
					corners = path.corners;
					cornersValid = true;
					break;
				case UnityEngine.AI.NavMeshPathStatus.PathInvalid:
					calcPending = false;
					break;
				}
			}
		}
	}

	[SLua.CustomLuaClassAttribute]
	public static class NavMeshUtils 
	{
		public static float DefaultSampleRange = 1f;
		public static float MaxSampleRange = 100f;

		private static UnityEngine.AI.NavMeshPath tempPath = new UnityEngine.AI.NavMeshPath();

		public static Mesh NavMesh2Mesh()
		{
			var info = UnityEngine.AI.NavMesh.CalculateTriangulation();
			if (info.vertices.IsNullOrEmpty())
			{
				return null;
			}
			var mesh = new Mesh();
			mesh.vertices = info.vertices;
			mesh.triangles = info.indices;
			return mesh;
		}

		public static bool CalcPath(Vector3 p1, Vector3 p2, UnityEngine.AI.NavMeshPath path)
		{
			if (UnityEngine.AI.NavMesh.CalculatePath(p1, p2, Config.NavMeshArea.MASK_ALL, path))
			{
				return true;
			}
			return false;
		}
		
		public static void SampleTransform(Transform transform)
		{
			transform.position = SamplePosition(transform.position, DefaultSampleRange);
		}
		
		public static void SampleTransformWithRange(Transform transform, float range)
		{
			transform.position = SamplePosition(transform.position, range);
		}

		public static bool SamplePosition(Vector3 p, out float x, out float y, out float z)
		{
			Vector3 ret;
			if (SamplePosition(p, out ret))
			{
				x = ret.x;
				y = ret.y;
				z = ret.z;
				return true;
			}
			else
			{
				x = p.x;
				y = p.y;
				z = p.z;
				return false;
			}
		}

		public static bool SamplePositionWithRange(Vector3 p, float range, out float x, out float y, out float z)
		{
			Vector3 ret;
			if (SamplePosition(p, range, out ret))
			{
				x = ret.x;
				y = ret.y;
				z = ret.z;
				return true;
			}
			else
			{
				x = p.x;
				y = p.y;
				z = p.z;
				return false;
			}
		}

		public static bool RaycastDirection(Vector3 p, Vector3 dir, float distance, out float x, out float y, out float z)
		{
			var target = p + dir * distance;
			UnityEngine.AI.NavMeshHit hit;
			if (UnityEngine.AI.NavMesh.Raycast(p, target, out hit, Config.NavMeshArea.MASK_ALL))
			{
				target = hit.position;
				x = target.x;
				y = target.y;
				z = target.z;
				return !GeometryUtils.PositionEqual(p, target);
			}
			x = p.x;
			y = p.y;
			z = p.z;
			return false;
		}

		public static bool SampleDirection(Vector3 p, Vector3 dir, out float x, out float y, out float z)
		{
			var target = p+dir.normalized;
			UnityEngine.AI.NavMeshHit hit;
			if (UnityEngine.AI.NavMesh.SamplePosition(target, out hit, Vector3.Distance(p, target), Config.NavMeshArea.MASK_ALL))
			{
				tempPath.ClearCorners();
				if (UnityEngine.AI.NavMesh.CalculatePath(p, hit.position, Config.NavMeshArea.MASK_ALL, tempPath))
				{
					var corners = tempPath.corners;
					if (!corners.IsNullOrEmpty())
					{
						if (1 < corners.Length)
						{
							target = corners[1];
							x = target.x;
							y = target.y;
							z = target.z;
							return true;
						}
					}
				}
			}

			x = target.x;
			y = target.y;
			z = target.z;
			return false;
		}

		[SLua.DoNotToLuaAttribute]
		public static Vector3 SamplePosition(Vector3 p)
		{
			return SamplePosition(p, DefaultSampleRange);
		}

		[SLua.DoNotToLuaAttribute]
		public static bool SamplePosition(Vector3 p, out Vector3 ret)
		{
			return SamplePosition(p, DefaultSampleRange, out ret);
		}

		[SLua.DoNotToLuaAttribute]
		public static Vector3 SamplePosition(Vector3 p, float range)
		{
			Vector3 ret;
			SamplePosition(p, range, out ret);
			return ret;
		}

		[SLua.DoNotToLuaAttribute]
		public static bool SamplePosition(Vector3 p, float range, out Vector3 ret)
		{
			UnityEngine.AI.NavMeshHit hit;
			if (UnityEngine.AI.NavMesh.SamplePosition(p, out hit, range, Config.NavMeshArea.MASK_ALL))
			{
				ret = hit.position;
				return true;
			}
			else
			{
				Vector3 newP;
				if (RaycastDownPosition(p, out newP))
				{
					if (UnityEngine.AI.NavMesh.SamplePosition(newP, out hit, range, Config.NavMeshArea.MASK_ALL))
					{
						ret = hit.position;
						return true;
					}
					else if (range < MaxSampleRange)
					{
						if (UnityEngine.AI.NavMesh.SamplePosition(newP, out hit, MaxSampleRange, Config.NavMeshArea.MASK_ALL))
						{
							ret = hit.position;
							return true;
						}
					}
				}
				else if (range < MaxSampleRange)
				{
					if (UnityEngine.AI.NavMesh.SamplePosition(p, out hit, MaxSampleRange, Config.NavMeshArea.MASK_ALL))
					{
						ret = hit.position;
						return true;
					}
				}
			}
			ret = p;
			return false;
		}

		[SLua.DoNotToLuaAttribute]
		public static Vector3 RaycastDownPosition(Vector3 p)
		{
			Vector3 ret;
			RaycastDownPosition(p, out ret);
			return ret;
		}

		[SLua.DoNotToLuaAttribute]
		public static bool RaycastDownPosition(Vector3 p, out Vector3 ret)
		{
			var testP = p;
			testP.y = Mathf.Max(testP.y, 1000f);
			Ray ray = new Ray(testP, Vector3.down);
			
			RaycastHit rayHit;
			if (Physics.Raycast (ray, out rayHit, float.PositiveInfinity, LayerMask.GetMask (Config.Layer.TERRAIN.Key))) 
			{
				ret = rayHit.point;
				return true;
			}
			ret = p;
			return false;
		}

		public static bool CanArrived(Vector3 p1, Vector3 p2, float invalidRange)
		{
			return CanArrived(p1, p2, invalidRange, true);
		}
		public static bool CanArrived(Vector3 p1, Vector3 p2, float invalidRange, bool adjust)
		{
			UnityEngine.AI.NavMeshPath path;
			return CanArrived(p1, p2, invalidRange, adjust, out path);
		}
		public static bool CanArrived(Vector3 p1, Vector3 p2, float invalidRange, bool adjust, out UnityEngine.AI.NavMeshPath path)
		{
			if (adjust)
			{
				p2 = SamplePosition(p2);
			}
			path = new UnityEngine.AI.NavMeshPath();
			if (!UnityEngine.AI.NavMesh.CalculatePath(p1, p2, RO.Config.NavMeshArea.MASK_ALL, path))
			{
				return false;
			}
			var corners = path.corners;
			if (corners.IsNullOrEmpty())
			{
				return false;
			}
			return Vector3.Distance(corners[corners.Length-1], p2) < invalidRange;
		}
		public static float GetPathDistance(UnityEngine.AI.NavMeshPath path)
		{
			var corners = path.corners;
			if (corners.IsNullOrEmpty())
			{
				return 0;
			}
			float distance = 0;
			var p1 = corners[0];
			for (int i = 1; i < corners.Length; ++i)
			{
				var p2 = corners[i];
				distance += Vector3.Distance(p1, p2);
				p1 = p2;
			}
			return distance;
		}
	
	}
} // namespace RO
