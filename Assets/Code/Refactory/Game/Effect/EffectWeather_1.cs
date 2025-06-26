using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class EffectWeather_1 : MonoBehaviour 
	{
		public Camera weatherCamera;
		public Vector3 weatherViewPort = new Vector3(0.5f, 0.5f, 6f);

		void Reset()
		{
			if (null == weatherCamera)
			{
				weatherCamera = Camera.main;
			}
		}

		void Start()
		{
			if (null == weatherCamera)
			{
				weatherCamera = Camera.main;
			}
		}
	
		void LateUpdate () 
		{
			if (null != weatherCamera)
			{
				transform.position = weatherCamera.ViewportToWorldPoint(weatherViewPort);
			}
		}
	}
} // namespace RO
