using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class StayOriginalPosition : MonoBehaviour
	{
		Vector3 pos = Vector3.zero;

		void Start ()
		{
			pos = transform.position;
		}

		public void SetPosition (Vector3 oriPos)
		{
			pos = oriPos;

			transform.position = pos;
		}

		void LateUpdate ()
		{
			transform.position = pos;
		}
	}
} // namespace RO
