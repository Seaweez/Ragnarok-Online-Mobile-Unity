using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class LateUpdateDelegate : MonoBehaviour 
	{
		public System.Action<GameObject> listener;

		void LateUpdate()
		{
			if (null != listener)
			{
				listener(gameObject);
			}
		}
	}
} // namespace RO
