using UnityEngine;
using System.Collections.Generic;
using Ghost.Utils;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class AreaTrigger : MonoBehaviour 
	{
		[SLua.CustomLuaClassAttribute]
		public enum Type
		{
			CIRCLE = 0,
			RECTANGE = 1
		}

		public Type type = Type.CIRCLE;

		[SerializeField, HideInInspector]
		public float range = 1.3f;
		[SerializeField, HideInInspector]
		public float innerRange = 0f;
		[SerializeField, HideInInspector]
		public Vector2 size = Vector2.one;

#if DEBUG_DRAW
		[SLua.DoNotToLuaAttribute]
		public bool ddPlayerIn{get;set;}
#endif // DEBUG_DRAW

		public bool invalidate{get;private set;}

		public Vector3 position
		{
			get
			{
				return transform.position;
			}
		}

		public bool Check(Transform t)
		{
			if (invalidate)
			{
				return false;
			}
			switch (type)
			{
			case AreaTrigger.Type.CIRCLE:
				if (0 < innerRange && GeometryUtils.PointInRange(t.position, position, innerRange))
				{
					return false;
				}
				return GeometryUtils.PointInRange(t.position, position, range);
			case AreaTrigger.Type.RECTANGE:
			{
				if (0 < size.x && 0 < size.y)
				{
					Vector3 left,right,leftEnd,rightEnd;
					GeometryUtils.GetSurroundRect(position, transform.rotation, size, out left,out right,out leftEnd,out rightEnd);
					return GeometryUtils.PointInRect(t.position, leftEnd, rightEnd, right, left);
				}
				else
				{
					return false;
				}
			}
			}
			return false;
		}

		public virtual void OnRoleEnter(Transform t)
		{

		}

		public virtual void OnRoleExit(Transform t)
		{
			
		}

		public bool TryGetDistanceInRectX1(Vector3 p, out float d1)
		{
			float d2;
			return TryGetDistanceInRectX(p, out d1, out d2, 1);
		}

		public bool TryGetDistanceInRectX2(Vector3 p, out float d2)
		{
			float d1;
			return TryGetDistanceInRectX(p, out d1, out d2, 2);
		}

		public bool TryGetDistanceInRectX(Vector3 p, out float d1, out float d2)
		{
			return TryGetDistanceInRectX(p, out d1, out d2, 3);
		}

		private bool TryGetDistanceInRectX(Vector3 p, out float d1, out float d2, int mask)
		{
			if (AreaTrigger.Type.RECTANGE != type)
			{
				d1 = 0;
				d2 = 0;
				return false;
			}
			Vector3 left,right,leftEnd,rightEnd;
			GeometryUtils.GetSurroundRect(position, transform.rotation, size, out left,out right,out leftEnd,out rightEnd);
			if (0 != (mask & 1))
			{
				d1 = GeometryUtils.DistanceOfPointToVector(left, leftEnd, p);
			}
			else
			{
				d1 = 0;
			}
			if (0 != (mask & 2))
			{
				d2 = GeometryUtils.DistanceOfPointToVector(right, rightEnd, p);
			}
			else
			{
				d2 = 0;
			}
			return true;
		}

		public bool TryGetDistanceInRectZ1(Vector3 p, out float d1)
		{
			float d2;
			return TryGetDistanceInRectZ(p, out d1, out d2, 1);
		}
		
		public bool TryGetDistanceInRectZ2(Vector3 p, out float d2)
		{
			float d1;
			return TryGetDistanceInRectZ(p, out d1, out d2, 2);
		}
		
		public bool TryGetDistanceInRectZ(Vector3 p, out float d1, out float d2)
		{
			return TryGetDistanceInRectZ(p, out d1, out d2, 3);
		}

		private bool TryGetDistanceInRectZ(Vector3 p, out float d1, out float d2, int mask)
		{
			if (AreaTrigger.Type.RECTANGE != type)
			{
				d1 = 0;
				d2 = 0;
				return false;
			}
			Vector3 left,right,leftEnd,rightEnd;
			GeometryUtils.GetSurroundRect(position, transform.rotation, size, out left,out right,out leftEnd,out rightEnd);
			if (0 != (mask & 1))
			{
				d1 = GeometryUtils.DistanceOfPointToVector(left, right, p);
			}
			else
			{
				d1 = 0;
			}
			if (0 != (mask & 2))
			{
				d2 = GeometryUtils.DistanceOfPointToVector(leftEnd, rightEnd, p);
			}
			else
			{
				d2 = 0;
			}
			return true;
		}

		void OnEnable()
		{
			invalidate = false;
		}

		void OnDisable()
		{
			invalidate = true;
		}
		
#if DEBUG_DRAW
		protected virtual void DebugDraw(Color color)
		{
			switch (type)
			{
			case Type.CIRCLE:
				if (0 < innerRange)
				{
					DebugUtils.DrawDoubleCircle(transform.position, new Quaternion(), innerRange, range, 30, color);
				}
				else
				{
					DebugUtils.DrawCircle(transform.position, new Quaternion(), range, 30, color);
				}
				break;
			case Type.RECTANGE:
			{
				Vector3 left,right,leftEnd,rightEnd;
				GeometryUtils.GetSurroundRect(position, transform.rotation, size, out left,out right,out leftEnd,out rightEnd);
				DebugUtils.DrawRect(left, leftEnd, right, rightEnd, color);
			}
				break;
			}
		}

		void OnDrawGizmos()
		{
			DebugDraw(ddPlayerIn ? Color.red : Color.blue);
		}
		
		void OnDrawGizmosSelected()
		{
			DebugDraw(Color.red);
		}
#endif // DEBUG_DRAW

	}
} // namespace RO
