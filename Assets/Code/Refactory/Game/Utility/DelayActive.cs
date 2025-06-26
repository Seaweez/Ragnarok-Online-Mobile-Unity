using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class DelayActive : MonoBehaviour 
	{
		public float delayTime = 0f;
	
		void Awake()
		{
			gameObject.SetActive(false);
			Invoke("Active", delayTime);
		}

		private void Active()
		{
			gameObject.SetActive(true);
		}

	}
} // namespace RO
