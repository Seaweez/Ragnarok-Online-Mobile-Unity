using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class RectObject : MonoBehaviour 
	{
		public Rect rect = new Rect();

		public Vector2 offset
		{
			get
			{
				return rect.center;
			}
			set
			{
				rect.center = value;
			}
		}
		public Vector2 size
		{
			get
			{
				return rect.size;
			}
			set
			{
				rect.size = value;
			}
		}

		public bool Contains(Vector3 p)
		{
			return rect.Contains(transform.InverseTransformPoint(p).XZ ());
		}

		public void PlaceTo(Vector3 p, float angleY)
		{
			transform.position = p;
			transform.eulerAngles = new Vector3(0, angleY, 0);
		}

		#region behaviour
		private void DebugDraw(Color color)
		{
			var p1 = transform.TransformPoint(new Vector3(rect.xMin, 0, rect.yMin));
			var p2 = transform.TransformPoint(new Vector3(rect.xMax, 0, rect.yMin));
			var p3 = transform.TransformPoint(new Vector3(rect.xMax, 0, rect.yMax));
			var p4 = transform.TransformPoint(new Vector3(rect.xMin, 0, rect.yMax));
			Debug.DrawLine(p1, p2, color);
			Debug.DrawLine(p2, p3, color);
			Debug.DrawLine(p3, p4, color);
			Debug.DrawLine(p4, p1, color);
		}
		
		void OnDrawGizmos()
		{
			DebugDraw(Color.green);
		}
		
		void OnDrawGizmosSelected()
		{
			DebugDraw(Color.red);
		}
		#endregion behaviour
	}
} // namespace RO
