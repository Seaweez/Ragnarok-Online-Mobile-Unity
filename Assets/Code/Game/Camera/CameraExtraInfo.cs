using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class CameraExtraInfo : MonoBehaviour 
	{
		public float fieldOfViewExtra = 0f;
		public float fieldOfViewScale = 1f;
	
		public new Camera camera = null;

		void Reset()
		{
			camera = GetComponent<Camera>();
		}

		void Start()
		{
			if (null == camera)
			{
				camera = GetComponent<Camera>();
			}
		}
	}
} // namespace RO
