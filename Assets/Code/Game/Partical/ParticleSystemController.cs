using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	public class ParticleSystemController : MonoBehaviour 
	{
		public ParticleSystem[] particaleSystems = null;
	
		public void SetPlaybackSpeed(float speed)
		{
			if (!particaleSystems.IsNullOrEmpty())
			{
				foreach (var ps in particaleSystems)
				{
					var main = ps.main;
       				main.simulationSpeed = speed;
				}
			}
		}

	}
} // namespace RO
