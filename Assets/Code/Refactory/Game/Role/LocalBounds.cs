using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	public class LocalBounds : MonoBehaviour 
	{
		public Bounds localBounds;

		public Bounds bounds
		{
			get
			{
				var b = localBounds;
				b.size.Multiply(transform.lossyScale);
				return b;
			}
		}

#if DEBUG_DRAW
		private void DebugDraw(Color color)
		{
			Gizmos.color = color;
			Gizmos.DrawWireCube(transform.position+bounds.center, bounds.size);
		}
		
		void OnDrawGizmos()
		{
			DebugDraw(Color.green);
		}
		
		void OnDrawGizmosSelected()
		{
			DebugDraw(Color.red);
		}
#endif // DEBUG_DRAW
	
	}
} // namespace RO
