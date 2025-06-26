using UnityEngine;
using System.Collections;

namespace Ghost.Utils
{
	[SLua.CustomLuaClassAttribute]
	public static class DebugUtils {

		public static void DrawDoubleCircle(Vector3 origin, Quaternion rotation, float radius1, float radius2, int pieceCount, Color color)
		{
			if (3 > pieceCount)
			{
				return;
			}
			if (0 >= radius1 || 0 >= radius2)
			{
				return;
			}
			
			float pieceAngle = 360.0f / pieceCount;
			
			Vector3 p0_1 = origin + rotation * Vector3.forward * radius1;
			Vector3 p0_2 = origin + rotation * Vector3.forward * radius2;
			Vector3 p1_1 = p0_1;
			Vector3 p1_2 = p0_2;
			for (int i = 0; i < pieceCount-1; ++i)
			{
				var r = Quaternion.Euler(0, pieceAngle*(i+1), 0);
				Vector3 p2_1 = origin + rotation * (r * Vector3.forward * radius1);
				Vector3 p2_2 = origin + rotation * (r * Vector3.forward * radius2);
				Debug.DrawLine(p1_1, p2_1, color);
				Debug.DrawLine(p1_2, p2_2, color);
				Debug.DrawLine(p2_1, p2_2, color);
				
				p1_1 = p2_1;
				p1_2 = p2_2;
			}
			Debug.DrawLine(p0_1, p1_1, color);
			Debug.DrawLine(p0_1, p0_2, color);
		}
		
		public static void DrawCircle(Vector3 origin, Quaternion rotation, float radius, int pieceCount, Color color)
		{
			if (3 > pieceCount)
			{
				return;
			}
			if (0 >= radius)
			{
				return;
			}

			float pieceAngle = 360.0f / pieceCount;

			Vector3 p0 = origin + rotation * Vector3.forward * radius;
			Vector3 p1 = p0;
			for (int i = 0; i < pieceCount-1; ++i)
			{
				var r = Quaternion.Euler(0, pieceAngle*(i+1), 0);
				Vector3 p2 = origin + rotation * (r * Vector3.forward * radius);
				Debug.DrawLine(p1, p2, color);

				p1 = p2;
			}
			Debug.DrawLine(p0, p1, color);
		}

		public static void DrawCircle(Vector3 origin, float radius, int pieceCount, Color color)
		{
			DrawCircle(origin, new Quaternion(), radius, pieceCount, color);
		}

		public static void DrawRect(Vector3 left, Vector3 right, Vector3 leftEnd, Vector3 rightEnd, Color color)
		{
			Debug.DrawLine(left, right, color);
			Debug.DrawLine(left, leftEnd, color);
			Debug.DrawLine(right, rightEnd, color);
			Debug.DrawLine(leftEnd, rightEnd, color);
		}

		public static void DrawBounds(Bounds bounds, Color color)
		{
			var size = bounds.size;

			var points = new Vector3[8];
			points[0] = bounds.center-bounds.extents;
			points[1] = points[0]+new Vector3(size.x, 0, 0);
			points[2] = points[0]+new Vector3(0, size.y, 0);
			points[3] = points[0]+new Vector3(0, 0, size.z);
			points[4] = bounds.center+bounds.extents;
			points[5] = points[4]-new Vector3(size.x, 0, 0);
			points[6] = points[4]-new Vector3(0, size.y, 0);
			points[7] = points[4]-new Vector3(0, 0, size.z);

			Debug.DrawLine(points[0], points[1], color);
			Debug.DrawLine(points[0], points[2], color);
			Debug.DrawLine(points[0], points[3], color);
			Debug.DrawLine(points[4], points[5], color);
			Debug.DrawLine(points[4], points[6], color);
			Debug.DrawLine(points[4], points[7], color);
			Debug.DrawLine(points[1], points[6], color);
			Debug.DrawLine(points[1], points[7], color);
			Debug.DrawLine(points[2], points[5], color);
			Debug.DrawLine(points[2], points[7], color);
			Debug.DrawLine(points[3], points[5], color);
			Debug.DrawLine(points[3], points[6], color);
		}
		
	}
} // namespace Ghost.Utils