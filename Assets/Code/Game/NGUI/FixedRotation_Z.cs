using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class FixedRotation_Z : MonoBehaviour
	{
		public float value = 0;

		void LateUpdate ()
		{
			Vector3 rotationVector3 = transform.rotation.eulerAngles;
			if (rotationVector3.z != value) {
				transform.rotation = Quaternion.Euler (new Vector3 (rotationVector3.x, rotationVector3.y, value));
			}
		}
	}
} // namespace RO
