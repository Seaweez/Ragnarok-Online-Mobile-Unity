using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class LookAtCamera : MonoBehaviour 
	{
		private Transform selfTransform = null;

		private void Look()
		{
			if (null != selfTransform && null != Camera.main)
			{
				selfTransform.LookAt(Camera.main.transform.position);
			}
		}

		void Start()
		{
			selfTransform = transform;
		}

		void LateUpdate()
		{
			Look();
		}
	}
} // namespace RO
