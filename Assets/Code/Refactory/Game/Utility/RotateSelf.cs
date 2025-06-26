using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class RotateSelf : MonoBehaviour
	{
		public Space space = Space.Self;
		public float rotateSpeed;
		public Vector3 axis;

		void Update ()
		{
			transform.Rotate (axis, rotateSpeed * Time.deltaTime, space);
		}
	}
} // namespace RO
