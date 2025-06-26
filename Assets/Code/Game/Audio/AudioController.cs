using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class AudioController 
	{
		public AudioSource audioSource;
		public AudioClip clip;

		public string name = null;
		
		public AudioController(AudioSource source, AudioClip c)
		{
			audioSource = source;
			clip = c;
		}
		
		public void Stop()
		{
			if (null != audioSource && audioSource.clip == clip && audioSource.isPlaying)
			{
				audioSource.Stop();
			}
		}
	}
} // namespace RO
