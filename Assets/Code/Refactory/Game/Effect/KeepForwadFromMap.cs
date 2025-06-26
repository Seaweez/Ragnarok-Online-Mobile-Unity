using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class KeepForwadFromMap : MonoBehaviour 
	{
		void LateUpdate()
		{
			var origin = Vector3.zero;
			if (null != Map2DManager.Me)
			{
				var map = Map2DManager.Me.GetMap2D();
				if (null != map)
				{
					origin = map.transform.position;
				}
			}
			var direction = transform.position-origin;
			var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
			var eular = transform.eulerAngles;
			eular.y = targetAngle;
			transform.eulerAngles = eular;
		}
	}
} // namespace RO
