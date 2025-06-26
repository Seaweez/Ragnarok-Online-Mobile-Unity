using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;
using Ghost.Utils;

namespace RO
{
	public class TerrainProjector : MonoBehaviour 
	{
		void Update()
		{
			Ray ray = new Ray(transform.position+Vector3.up, Vector3.down);
			RaycastHit hit;
			int layerMast = LayerMask.GetMask(Config.Layer.TERRAIN.Key);
			if (Physics.Raycast(ray, out hit, float.PositiveInfinity, layerMast))
			{
				transform.LookAt(hit.point-hit.normal);
			}
		}
	}
} // namespace RO
