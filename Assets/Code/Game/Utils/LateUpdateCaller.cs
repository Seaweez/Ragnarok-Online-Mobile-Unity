using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class LateUpdateCaller : MonoBehaviour 
	{
		public System.Action<LateUpdateCaller> listener;

		void LateUpdate()
		{
			if (null != listener)
			{
				listener(this);
			}
		}
	
	}
} // namespace RO
