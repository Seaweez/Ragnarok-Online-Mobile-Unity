using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	[RequireComponent(typeof(AudioSource))]
	public class AudioPlayer : MonoBehaviour
	{
		public string[] audioEffects = null;
		public AudioSource audioSource;

		void OnEnable ()
		{
			if (null == audioSource) {
				audioSource = this.gameObject.GetComponent<AudioSource> ();
			}
		}
	
		public void ActionEventPlayAudioEffectLoop (int index)
		{
			AudioClip clip = ValidClip (index);
			if (clip != null) {
				audioSource.loop = true;
				AudioHelper.PlayOn (clip, audioSource);
			}
		}

		public void ActionEventPlayAudioEffectOneShot (int index)
		{
			AudioClip clip = ValidClip (index);
			if (clip != null) {
				audioSource.loop = false;
				AudioHelper.PlayOneShotOn (clip, audioSource);
			}
		}

		AudioClip ValidClip (int audioClipIndex)
		{
			if (null == audioSource) {
				return null;
			}
			if (audioEffects.IsNullOrEmpty ()) {
				return null;
			}
			if (!audioEffects.CheckIndex (audioClipIndex)) {
				return null;
			}
			string clipName = audioEffects [audioClipIndex];
			if (string.IsNullOrEmpty(clipName)) {
				return null;
			}
			var clip = ResourceManager.Instance.SLoad<AudioClip> (ResourcePathHelper.IDSE(clipName));
			if (clip == null) {
				return null;
			}
			return clip;
		}

		public void ActionEventPlayMultiOneShot (int index)
		{
			AudioClip clip = ValidClip (index);
			if (clip != null) {
				AudioSource source = AudioHelper.PlayOneShotAt (clip, this.transform.position);
				source.transform.SetParent (this.transform);
			}
		}

		//停止当前播放的音效
		public void ActionEventStopCurrentAudio ()
		{
			if (null == audioSource || null == audioSource.clip) {
				return;
			}
			audioSource.Stop ();
		}

		//暂停当前播放的音效
		public void ActionEventPauseCurrentAudio ()
		{
			if (null == audioSource || null == audioSource.clip) {
				return;
			}
			audioSource.Pause ();
		}
	}
} // namespace RO
