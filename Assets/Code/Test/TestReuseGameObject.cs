using UnityEngine;
using System.Collections.Generic;

namespace RO.Test
{
	public class TestReuseGameObject : MonoBehaviour 
	{
		public Transform pool;

		public Transform obj;

		public bool inPool = false;
		public bool outPool = false;

		void Update()
		{
			if (null == pool || null == obj)
			{
				return;
			}
			if (inPool)
			{
				inPool = false;
				// 1.
				obj.gameObject.SetActive(false);
				// 2.
				obj.parent = pool;
			}
			else if (outPool)
			{
				outPool = false;
				// 1.
				obj.parent = null;
				obj.position = transform.position;
				// 2.
				obj.gameObject.SetActive(true);
			}
		}
	
	}
} // namespace RO.Test
