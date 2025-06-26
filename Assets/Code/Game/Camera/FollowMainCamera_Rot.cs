using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class FollowMainCamera_Rot : MonoBehaviour
	{
		void LateUpdate ()
		{
			if (CameraController.Me) {
				transform.rotation = CameraController.Me.cameraRotation;
			}
		}
	}
} // namespace RO
