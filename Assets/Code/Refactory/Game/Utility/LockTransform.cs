using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class LockTransform : MonoBehaviour 
	{
		public bool lockRotation = false;
		private Quaternion rotation = Quaternion.identity;

		void LateUpdate()
		{
			if (lockRotation)
			{
				transform.rotation = rotation;
			}
		}

		void Start()
		{
			rotation = transform.rotation;
		}
	}
} // namespace RO
