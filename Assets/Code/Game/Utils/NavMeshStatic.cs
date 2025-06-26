using UnityEngine;
using System.Collections.Generic;
using Ghost.Utils;

namespace RO
{
	public class NavMeshStatic : MonoBehaviour 
	{
		public bool editoOnly = true;
		public int navMeshArea = 0;

		void Awake()
		{
			if (editoOnly)
			{
				GameObject.Destroy(gameObject);
			}
		}

		void OnDrawGizmos()
		{
			var r = GetComponent<Renderer>();
			if (null != r)
			{
				DebugUtils.DrawBounds(r.bounds, Color.blue);
			}
		}

		void OnDrawGizmosSelected()
		{
			var r = GetComponent<Renderer>();
			if (null != r)
			{
				DebugUtils.DrawBounds(r.bounds, Color.red);
			}
		}
	
	}
} // namespace RO
