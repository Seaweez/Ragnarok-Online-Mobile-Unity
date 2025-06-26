using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ghost.Extensions
{
	public static class VectorExtensions {

		public static Vector2 XZ(this Vector3 p)
		{
			return new Vector2(p.x, p.z);
		}

		public static Vector2 Multiply(this Vector2 p, Vector2 other)
		{
			return new Vector2(p.x*other.x, p.y*other.y);
		}

		public static Vector2 Divide(this Vector2 p, Vector2 other)
		{
			return new Vector2(p.x/other.x, p.y/other.y);
		}

		public static Vector3 Multiply(this Vector3 p, Vector3 other)
		{
			return new Vector3(p.x*other.x, p.y*other.y, p.z*other.z);
		}
		
		public static Vector3 Divide(this Vector3 p, Vector3 other)
		{
			return new Vector3(p.x/other.x, p.y/other.y, p.z/other.z);
		}
		
	}
} // namespace Ghost.Extensions
