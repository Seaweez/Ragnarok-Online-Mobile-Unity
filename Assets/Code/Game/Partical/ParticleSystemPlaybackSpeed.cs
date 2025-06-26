using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	public class ParticleSystemPlaybackSpeed : MonoBehaviour 
	{
		public ParticleSystem[] particaleSystems = null;

		public float playbackSpeed = 1f;

		void Update()
		{
			if (!particaleSystems.IsNullOrEmpty())
			{
				foreach (var ps in particaleSystems)
				{
					var main = ps.main;
					main.simulationSpeed = playbackSpeed;
				}
			}
		}
	}
} // namespace RO
