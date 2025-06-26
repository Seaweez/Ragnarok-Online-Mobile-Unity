using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[ExecuteInEditMode]
	public class RealFar : MonoBehaviour 
	{
		public Vector3 farPosition;

		public new Camera camera;
		public float cameraDistance = 50f;

		public Vector3 TranslatePoint(Vector3 p)
		{
			if (null != transform.parent)
			{
				p = transform.parent.TransformPoint(p);
			}
			if (null == camera)
			{
				return p;
			}
			var dir = (p - camera.transform.position);
			return camera.transform.position + dir/dir.magnitude * cameraDistance * (180-camera.fieldOfView);
		}

		void Start()
		{
			if (null == camera)
			{
				camera = Camera.main;
			}
		}

		void LateUpdate()
		{
			transform.position = TranslatePoint(farPosition);
		}
	
	}
} // namespace RO
