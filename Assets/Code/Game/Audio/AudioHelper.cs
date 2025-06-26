using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public static class AudioHelper
	{
		public static float volume = 1;

		private static AudioSource GetAudioSource (string resPath)
		{
			AudioSource source = null;
			var go = GameObjPool.Me.GetEx (resPath, Config.Pool.NAME_AUDIO, null, true);
			if (null != go) {
				source = go.GetComponent<AudioSource> ();
			}
			if (null == source) {
				GameObject.Destroy (go);

				var prefab = ResourceManager.Me.SLoad<GameObject> (resPath);
				if (null == prefab) {
					return null;
				}
				go = GameObject.Instantiate (prefab) as GameObject;
				source = go.GetComponent<AudioSource> ();
				if (null == source) {
					GameObject.Destroy (go);
				}
			}
			if (null != source) {
				source.volume = volume;
			}
			return source;
		}

		private static AudioSource PlayOneShotAt (AudioClip clip, Vector3 position, string sourceResPath)
		{
			var source = GetAudioSource (sourceResPath);
			if (null == source) {
				return null;
			}
			source.transform.position = position;
			source.clip = clip;
			source.Play ();
			source.loop = false;
			
			var autoDestroy = source.GetComponent<AudioAutoDestroy> ();
			if (null == autoDestroy) {
				autoDestroy = source.gameObject.AddComponent<AudioAutoDestroy> ();
				autoDestroy.source = source;
			}
			autoDestroy.resPath = sourceResPath;
			
			return source;
		}

		public static AudioSource PlayOneShot2DAt (AudioClip clip, Vector3 position)
		{
			return PlayOneShotAt (clip, position, ResourcePathHelper.IDAudioOneShot_2D ());
		}

		public static AudioSource PlayOneShot2DAt (ResourceID resID, Vector3 position)
		{
			var clip = ResourceManager.Me.Load<AudioClip> (resID);
			if (null == clip) {
				return null;
			}
			return PlayOneShot2DAt (clip, position);
		}

		public static AudioSource SPlayOneShot2DAt (string resPath, Vector3 position)
		{
			var clip = ResourceManager.Me.SLoad<AudioClip> (resPath);
			if (null == clip) {
				return null;
			}
			return PlayOneShot2DAt (clip, position);
		}

		public static AudioSource PlayOneShot2D (AudioClip clip)
		{
			return PlayOneShot2DAt (clip, Vector3.zero);
		}

		public static AudioSource PlayOneShot2D (ResourceID resID)
		{
			var clip = ResourceManager.Me.Load<AudioClip> (resID);
			if (null == clip) {
				return null;
			}
			return PlayOneShot2D (clip);
		}
		public static AudioSource SPlayOneShot2D (string resPath)
		{
			var clip = ResourceManager.Me.SLoad<AudioClip> (resPath);
			if (null == clip) {
				return null;
			}
			return PlayOneShot2D (clip);
		}

		public static AudioSource PlayOneShotAt (AudioClip clip, Vector3 position)
		{
			return PlayOneShotAt (clip, position, ResourcePathHelper.IDAudioOneShot ());
		}

		public static AudioSource PlayOneShotAt (ResourceID resID, Vector3 position)
		{
			var clip = ResourceManager.Me.Load<AudioClip> (resID);
			if (null == clip) {
				return null;
			}
			return PlayOneShotAt (clip, position);
		}

		public static AudioSource SPlayOneShotAt (string resPath, Vector3 position)
		{
			var clip = ResourceManager.Me.SLoad<AudioClip> (resPath);
			if (null == clip) {
				return null;
			}
			return PlayOneShotAt (clip, position);
		}

		public static AudioSource PlayOneShotOn (AudioClip clip, GameObject obj)
		{
			var source = obj.GetComponentInChildren<AudioSource> ();
			if (null == source) {
				return null;
			}
			if (null != source) {
				source.volume = volume;
			}
			source.PlayOneShot (clip);
			return source;
		}

		public static AudioSource PlayOneShotOn (ResourceID resID, GameObject obj)
		{
			var clip = ResourceManager.Me.Load<AudioClip> (resID);
			if (null == clip) {
				return null;
			}
			return PlayOneShotOn (clip, obj);
		}

		public static AudioSource SPlayOneShotOn (string resPath, GameObject obj)
		{
			var clip = ResourceManager.Me.SLoad<AudioClip> (resPath);
			if (null == clip) {
				return null;
			}
			return PlayOneShotOn (clip, obj);
		}

		public static AudioSource PlayOneShotOn (AudioClip clip, AudioSource source)
		{
			if (null != source) {
				source.volume = volume;
			}
			source.PlayOneShot (clip);
			return source;
		}

		public static AudioSource PlayOneShotOn (ResourceID resID, AudioSource source)
		{
			var clip = ResourceManager.Me.Load<AudioClip> (resID);
			if (null == clip) {
				return null;
			}
			return PlayOneShotOn (clip, source);
		}

		public static AudioSource SPlayOneShotOn (string resPath, AudioSource source)
		{
			var clip = ResourceManager.Me.SLoad<AudioClip> (resPath);
			if (null == clip) {
				return null;
			}
			return PlayOneShotOn (clip, source);
		}

		public static AudioController PlayOn (AudioClip clip, GameObject obj)
		{
			var source = obj.GetComponentInChildren<AudioSource> ();
			if (null == source) {
				return null;
			}
			if (null != source) {
				source.volume = volume;
			}
			source.clip = clip;
			source.Play ();
			return new AudioController (source, clip);
		}

		public static AudioController PlayOn (ResourceID resID, GameObject obj)
		{
			var clip = ResourceManager.Me.Load<AudioClip> (resID);
			if (null == clip) {
				return null;
			}
			return PlayOn (clip, obj);
		}

		public static AudioController SPlayOn (string resPath, GameObject obj)
		{
			var clip = ResourceManager.Me.SLoad<AudioClip> (resPath);
			if (null == clip) {
				return null;
			}
			return PlayOn (clip, obj);
		}

		public static AudioController PlayOn (AudioClip clip, AudioSource source)
		{
			if (null != source) {
				source.volume = volume;
			}
			source.clip = clip;
			source.Play ();
			return new AudioController (source, clip);
		}

		public static AudioController PlayOn (ResourceID resID, AudioSource source)
		{
			var clip = ResourceManager.Me.Load<AudioClip> (resID);
			if (null == clip) {
				return null;
			}
			return PlayOn (clip, source);
		}

		public static AudioController SPlayOn (string resPath, AudioSource source)
		{
			var clip = ResourceManager.Me.SLoad<AudioClip> (resPath);
			if (null == clip) {
				return null;
			}
			return PlayOn (clip, source);
		}

		public static AudioClip GetAudioClipByWavByte (byte[] rawData, string name)
		{
			WAVHelper wav = new WAVHelper (rawData);
			RO.LoggerUnused.Log (wav);
			AudioClip audioClip = AudioClip.Create (name, wav.SampleCount, 1, wav.Frequency, false);
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      
			
			audioClip.SetData (wav.LeftChannel, 0);

			return audioClip;
		}

		public static AudioClip GetAudioClipByPcmByte (byte[] rawData, string name)
		{
			PCMHelper pcm = new PCMHelper (rawData);
			RO.LoggerUnused.Log (pcm);
			AudioClip audioClip = AudioClip.Create (name, pcm.SampleCount, 1, pcm.Frequency, false);
			audioClip.SetData (pcm.LeftChannel, 0);
			
			return audioClip;
		}
	}
} // namespace RO
