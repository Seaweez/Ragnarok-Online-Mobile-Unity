using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class UpdateDelegate : MonoBehaviour 
	{
		public System.Action<GameObject> listener;

		void Update()
		{
			if (null != listener)
			{
				listener(gameObject);
			}
		}
	}
} // namespace RO
