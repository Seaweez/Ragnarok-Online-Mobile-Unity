using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace Ghost.Utils
{
	[SLua.CustomLuaClassAttribute]
	public static class GeometryUtils {

		private static float Multiply(float p1x , float p1y, float p2x,float p2y, float p0x,float p0y)
		{
			return ((p1x - p0x) * (p2y - p0y) - (p2x - p0x) * (p1y - p0y));
		}
		
		public static bool PointInRect(Vector3 point,Vector3 leftEnd,Vector3 rightEnd,Vector3 right,Vector3 left)
		{
			float x = point.x;
			float y = point.z;
			
			float v0x = leftEnd.x;
			float v0y = leftEnd.z;
			
			float v1x = rightEnd.x;
			float v1y = rightEnd.z;
			
			float v2x = right.x;
			float v2y = right.z;
			
			float v3x = left.x;
			float v3y = left.z;
			
			return Multiply(x,y, v0x,v0y, v1x,v1y) * Multiply(x,y, v3x,v3y, v2x,v2y) <= 0 
				&& Multiply(x,y, v3x,v3y, v0x,v0y) * Multiply(x,y, v2x,v2y, v1x,v1y) <= 0;
		}

		[SLua.DoNotToLuaAttribute]
		public static void GetForwardRect(Vector3 origin, Quaternion rotation, Vector2 range, out Vector3 left, out Vector3 right, out Vector3 leftEnd, out Vector3 rightEnd)
		{
			var halfWidth = range.x / 2;
			var height = range.y;
			
			left =  origin  + rotation * Vector3.left * halfWidth;
			right =  origin  + rotation * Vector3.right * halfWidth;
			leftEnd = left  + rotation * Vector3.forward * height;
			rightEnd = right  + rotation * Vector3.forward * height;
		}

		[SLua.DoNotToLuaAttribute]
		public static void GetSurroundRect(Vector3 origin, Quaternion rotation, Vector2 range, out Vector3 left, out Vector3 right, out Vector3 leftEnd, out Vector3 rightEnd)
		{
			float halfWidth = range.x / 2;
			float halfHeight = range.y / 2;
			float height = range.y;
			
			left = origin + rotation * new Vector3(-halfWidth, 0, -halfHeight);
			right = origin + rotation * new Vector3(halfWidth, 0, -halfHeight);
			leftEnd = left  + rotation * Vector3.forward * height;
			rightEnd = right  + rotation * Vector3.forward * height;
		}

		[SLua.DoNotToLuaAttribute]
		public static void GetRect(Vector3 p1, Vector3 p2, Quaternion rotation, out Vector3 left, out Vector3 right, out Vector3 leftEnd, out Vector3 rightEnd)
		{
			var center = (p1+p2)/2;
			center.y = p1.y;
			
			GetSurroundRect(center, rotation, new Vector2(Mathf.Abs(p1.x-p2.x), Mathf.Abs(p1.z-p2.z)), 
			                           out left, out right, out leftEnd, out rightEnd);
		}
		
		public static bool PointInRange(Vector3 point, Vector3 origin, float range)
		{
			if (0 >= range)
			{
				return false;
			}
			
			var distance = Vector2.Distance(origin.XZ(), point.XZ());
			return distance <= range;
		}

		public static float DistanceXZ(Vector3 p1, Vector3 p2)
		{
			return Vector2.Distance(p1.XZ(), p2.XZ ());
		}

		public static Vector3 GetCenterPoint(Vector3[] points)
		{
			if (points.IsNullOrEmpty())
			{
				return Vector3.zero;
			}
			if (1 == points.Length)
			{
				return points[0];
			}
			Vector3 min = points[0];
			Vector3 max = points[0];
			for (int i = 1; i < points.Length; ++i)
			{
				min = Vector3.Min(min, points[i]);
				max = Vector3.Min(max, points[i]);
			}
			return new Vector3((max.x-min.x)/2, (max.y-min.y)/2, (max.z-min.z)/2);
		}

		public static Vector3 GetCenterPoint(Component[] objts)
		{
			if (objts.IsNullOrEmpty())
			{
				return Vector3.zero;
			}
			if (1 == objts.Length)
			{
				return objts[0].transform.position;
			}
			Vector3 min = objts[0].transform.position;
			Vector3 max = objts[0].transform.position;
			for (int i = 1; i < objts.Length; ++i)
			{
				var point = objts[i].transform.position;
				min = Vector3.Min(min, point);
				max = Vector3.Min(max, point);
			}
			return new Vector3((max.x-min.x)/2, (max.y-min.y)/2, (max.z-min.z)/2);
		}

		public static bool PositionAlmostEqual(Vector3 p1, Vector3 p2)
		{
			return 0.01f > Vector3.Distance(p1, p2);
		}

		public static bool PositionEqual(Vector3 p1, Vector3 p2)
		{
			return p1.x == p2.x && p1.y == p2.y && p1.z == p2.z;
		}

		public static bool PositionAlmostEqual(Vector2 p1, Vector2 p2)
		{
			return 0.01f > Vector2.Distance(p1, p2);
		}

		public static bool PositionEqual(Vector2 p1, Vector2 p2)
		{
			return p1.x == p2.x && p1.y == p2.y;
		}

		public static bool RotationAlmostEqual(Vector3 a1, Vector3 a2)
		{
			return 0.1f > Vector3.Angle(a1, a2);
		}

		public static bool RotationAlmostEqual(Vector2 a1, Vector2 a2)
		{
			return 0.1f > Vector2.Angle(a1, a2);
		}

		public static bool RotationAlmostEqual(Quaternion q1, Quaternion q2)
		{
			return 0.1f > Quaternion.Angle(q1, q2);
		}

		public static bool RotationEqual(Quaternion q1, Quaternion q2)
		{
			return q1.x == q2.x && q1.y == q2.y && q1.z == q2.z && q1.w == q2.w;
		}

		public static Vector3 Bezier (float t, Vector3 p0, Vector3 p1, Vector3 p2)
		{
			float u = 1.0f - t;
			float tt = t * t;
			float tu = 2 * t * u;
			float uu = u * u;
			Vector3 res = new Vector3 ();
			res.x = uu * p0.x + tu * p1.x + tt * p2.x;
			res.y = uu * p0.y + tu * p1.y + tt * p2.y;
			res.z = uu * p0.z + tu * p1.z + tt * p2.z;
			return res;
		}

		public static float UniformAngle(float a)
		{
//			var oldA = a;
			a = ((int)a % 360) + (a-(int)a);
			if (0 > a)
			{
				a += 360;
			}
			return a;
		}

		public static float UniformAngle180(float a)
		{
			a = UniformAngle(a);
			if (180 < a)
			{
				a -= 360;
			}
			return a;
		}

		public static float DistanceOfPointToVector(Vector3 startPoint, Vector3 endPoint, Vector3 point)
		{
			Vector2 startVe2 = startPoint.XZ();
			Vector2 endVe2 = endPoint.XZ();
			float A = endVe2.y - startVe2.y;
			float B = startVe2.x - endVe2.x;
			float C = endVe2.x * startVe2.y - startVe2.x * endVe2.y;
			float denominator = Mathf.Sqrt(A * A + B * B);
			Vector2 pointVe2 = point.XZ();
			return Mathf.Abs((A * pointVe2.x + B * pointVe2.y + C) / denominator);;
		}

		public static float GetAngleByAxisY(Vector3 src, Vector3 target)
		{
			var direction = target-src;
			return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
		}

	}
}
