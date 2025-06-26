using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class FollowPointSubject : MonoBehaviour 
	{
		public PointSubject subject = null;
		public int followEP = -1;
		public Vector3 followOffset = Vector3.zero;
		
		public System.Action<FollowPointSubject> lostFollowListener = null;
		
		public void UpdatePosition()
		{
			if (null == subject)
			{
				return;
			}
			var follow = subject.GetEffectPoint(followEP);
			if (null == follow)
			{
				follow = subject.gameObject;
			}
			transform.position = follow.transform.position + followOffset;
		}
		
		void LateUpdate()
		{
			if (null != subject)
			{
				UpdatePosition();
			}
			else 
			{
				subject = null;
				if (null != lostFollowListener)
				{
					lostFollowListener(this);
				}
			}
		}
		
//		void OnDisable()
//		{
//			subject = null;
//			lostFollowListener = null;
//		}

		void OnDestroy()
		{
			subject = null;
			lostFollowListener = null;
		}
	
	}
} // namespace RO
