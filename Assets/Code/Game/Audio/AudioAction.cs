using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	public class AudioAction : MonoBehaviour 
	{
		public AudioClip[] audioEffects = null;
		public AudioSource audioSource = null;

		public void ActionEventPlayAudioEffect(int index)
		{
			if (audioEffects.IsNullOrEmpty())
			{
				return;
			}
			if (!audioEffects.CheckIndex(index))
			{
				return;
			}
			var clip = audioEffects[index];
			if (null == clip)
			{
				return;
			}
			if (null != audioSource)
			{
				AudioHelper.PlayOneShotOn (clip, audioSource);
			}
			else
			{
				AudioHelper.PlayOneShotAt (clip, transform.position);
			}
		}

		void Awake()
		{
			if (null == audioSource)
			{
				audioSource = GetComponent<AudioSource>();
			}
		}
	
	}
} // namespace RO
