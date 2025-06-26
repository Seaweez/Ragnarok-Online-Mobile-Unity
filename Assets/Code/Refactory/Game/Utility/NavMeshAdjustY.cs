using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;
using Ghost.Utils;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public static class NavMeshAdjustY 
	{
		public static float DefaultSampleRange = 1f;

		public static void Adjust(Transform transform)
		{
			transform.position = Adjust(transform.position, DefaultSampleRange);
		}
		public static void Adjust(Transform transform, float range)
		{
			transform.position = Adjust(transform.position, range);
		}

		//world position
		public static Vector3 Adjust(Vector3 orignPos)
		{
			return Adjust(orignPos, DefaultSampleRange);
		}
		public static Vector3 Adjust(Vector3 orignPos, float range)
		{
			var samplePosition = SamplePosition(orignPos, range);
			if (GeometryUtils.PositionAlmostEqual(orignPos.XZ (), samplePosition.XZ ()))
			{
				orignPos.y = samplePosition.y;
			}
			return orignPos;
		}

		public static void SamplePosition(Transform transform)
		{
			transform.position = SamplePosition(transform.position, DefaultSampleRange);
		}

		public static void SamplePosition(Transform transform, float range)
		{
			transform.position = SamplePosition(transform.position, range);
		}

		public static Vector3 SamplePosition(Vector3 p)
		{
			return SamplePosition(p, DefaultSampleRange);
		}

		public static Vector3 SamplePosition(Vector3 p, float range)
		{
			UnityEngine.AI.NavMeshHit hit;
			if (UnityEngine.AI.NavMesh.SamplePosition(p, out hit, range, Config.NavMeshArea.MASK_ALL))
			{
				return hit.position;
			}
			else
			{
				return RaycastDownPosition(p);
			}
		}

		public static Vector3 RaycastDownPosition(Vector3 p)
		{
			var testP = p;
			testP.y = Mathf.Max(testP.y, 1000f);
			Ray ray = new Ray(testP, Vector3.down);
			
			RaycastHit rayHit;
			if (Physics.Raycast (ray, out rayHit, float.PositiveInfinity, LayerMask.GetMask (Config.Layer.TERRAIN.Key))) 
			{
				return rayHit.point;
			}
			return p;
		}
	}
} // namespace RO
