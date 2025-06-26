using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	public class EffectAnimationPlayer_1 : MonoBehaviour 
	{
		public float intervalMin = 1;
		public float intervalMax = 1;

		public string[] animationNames;

		public Animator animator;

		public AudioClip[] audioEffects = null;
		public AudioSource audioSource = null;

		public void ActionEventPlayAudioEffect(int index)
		{
			if (null == audioSource)
			{
				return;
			}
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
			AudioHelper.PlayOneShotOn (clip, audioSource);
		}

		private void PlayAnimation()
		{
			if (null != animator && !animationNames.IsNullOrEmpty())
			{
				var name = animationNames[Random.Range(0, animationNames.Length)];
				animator.Play(name, -1, 0);
			}
			Schedule();
		}

		private void Schedule()
		{
			Invoke("PlayAnimation", Random.Range(intervalMin, intervalMax));
		}

		void Reset()
		{
			if (null == animator)
			{
				animator = GetComponent<Animator>();
			}
			if (null == audioSource)
			{
				audioSource = GetComponent<AudioSource>();
			}
		}

		void Start()
		{
			if (null == animator)
			{
				animator = GetComponent<Animator>();
			}
			if (null == audioSource)
			{
				audioSource = GetComponent<AudioSource>();
			}

			Schedule();
		}

		void OnEnable()
		{
			Schedule();
		}

		void OnDisable()
		{
			if (IsInvoking())
			{
				CancelInvoke();
			}
		}
	
	}
} // namespace RO
