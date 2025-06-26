using UnityEngine;
using System.Collections;
using System;

namespace RO.Test
{
	public class LuaLuancherMonitor : MonoBehaviour
	{
		public Action<GameObject> onEnable;
		public Action<GameObject> onDisable;
		public Action<GameObject> onDestroy;

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

}
