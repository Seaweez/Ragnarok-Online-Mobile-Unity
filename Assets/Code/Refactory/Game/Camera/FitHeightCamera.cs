using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class FitHeightCamera : MonoBehaviour 
	{
		public Vector2 ratio = new Vector2(16, 9);

		#region behaviour
		void Awake ()
		{
			var aspect = ratio.y / ratio.x;
			var allCameras = Camera.allCameras;
			for (int i = 0; i < allCameras.Length; ++i)
			{
				var c = allCameras[i];
				var screenAspect = 1/c.aspect; //Screen.height / Screen.width;
				if (screenAspect <= aspect)
				{
					return;
				}
				var heightScale = aspect * (1/screenAspect);
				c.rect = new Rect(0,(1-heightScale)/2,1,heightScale);
			}
		}

		void OnDestroy()
		{
			var allCameras = Camera.allCameras;
			for (int i = 0; i < allCameras.Length; ++i)
			{
				allCameras[i].rect = new Rect(0, 0, 1, 1);
			}
		}
		#endregion behaviour
	}
} // namespace RO
