using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class EffectLogic : MonoBehaviour 
	{
		public bool running{get; protected set;}

		protected virtual void Replay()
		{
			running = false;
		}

		void OnEnable()
		{
			Replay();
		}
	}
} // namespace RO
