using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class InvisibleDisable : MonoBehaviour 
	{
		public GameObject obj = null;

		void Start()
		{
			if (null != obj)
			{
				obj.SetActive(false);
			}
		}

		void OnBecameVisible()
		{
			if (null != obj)
			{
				obj.SetActive(true);
			}
		}
		void OnBecameInvisible()
		{
			if (null != obj)
			{
				obj.SetActive(false);
			}
		}
	
	}
} // namespace RO
