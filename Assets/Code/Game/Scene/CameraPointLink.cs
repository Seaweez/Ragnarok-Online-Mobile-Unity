using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;
using Ghost.Utils;

namespace RO
{
	public class CameraPointLink : MonoBehaviour 
	{
		public CameraPointLinkInfo[] links = null;
	
		void Start()
		{
			if (!(null != CameraPointManager.Me && CameraPointManager.Me.AddLink(this)))
			{
				GameObject.Destroy(gameObject);
			}
		}
		
		void OnDestroy()
		{
			if (null != CameraPointManager.Me)
			{
				CameraPointManager.Me.RemoveLink(this);
			}
		}

#if DEBUG_DRAW
		void DebugDraw_Link(CameraPointLinkInfo link)
		{
			Vector3 left,right,leftEnd,rightEnd;
			GeometryUtils.GetRect(link.cp1.position, link.cp2.position, Quaternion.Euler(0,link.angle,0), out left,out right,out leftEnd,out rightEnd);
			var halfExpand = link.expand/2;
			left.x -= halfExpand.x;
			leftEnd.x -= halfExpand.x;
			right.x += halfExpand.x;
			rightEnd.x += halfExpand.x;
			
			left.z -= halfExpand.z;
			right.z -= halfExpand.z;
			leftEnd.z += halfExpand.z;
			rightEnd.z += halfExpand.z;
			
			var oldColor = Gizmos.color;
			if (link.weightOnX)
			{
				Gizmos.color = Color.red;
			}
			Debug.DrawLine(left, right, Gizmos.color);
			Debug.DrawLine(leftEnd, rightEnd, Gizmos.color);
			
			if (link.weightOnZ)
			{
				Gizmos.color = Color.red;
			}
			else
			{
				Gizmos.color = oldColor;
			}
			Debug.DrawLine(right, rightEnd, Gizmos.color);
			Debug.DrawLine(left, leftEnd, Gizmos.color);
		}
		
		void DebugDraw(Color color)
		{
			if (!links.IsNullOrEmpty())
			{
				foreach (var l in links)
				{
					if (null == l.tempCP && l.valid)
					{
						Gizmos.color = color;
						DebugDraw_Link(l);
					}
				}
			}
		}
		
		void OnDrawGizmos()
		{
			DebugDraw(new Color(0f, 0.5f, 0f));
		}
		
		void OnDrawGizmosSelected()
		{
			DebugDraw(Color.green);
		}
#endif // DEBUG_DRAW
	}
} // namespace RO
