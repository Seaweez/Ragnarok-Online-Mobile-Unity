using UnityEngine;
using System.Collections.Generic;
using System;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class GameObjectForLua : MonoBehaviour
	{
		public Action<GameObject> onEnable;
		public Action<GameObject> onDisable;
		public Action<GameObject> onDestroy;
//		public Action<GameObject> OnDestroy;
	
		void OnEnable ()
		{
			if (onEnable != null) {
				onEnable (gameObject);
			}
		}

		void OnDisable ()
		{
			if (onDisable != null) {
				onDisable (gameObject);
			}
		}

		void OnDestroy ()
		{
			if (onDestroy != null) {
				onDestroy (gameObject);
			}
		}
	}
} // namespace RO
