using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class AudioManager : SingleTonGO<AudioManager>
	{
		public AudioListener listener = null;
		public AudioSource bgMusic = null;
		[Range(0,1)]
		public float
			fadeOutEnd = 0.2f;
		[Range(0,1)]
		public float
			fadeInStart = 0.5f;
//		public float fadeOutDuration = 1;
		public float fadeInDuration = 1;

		public static float volume = 1;

		private Transform listenerFolow = null;

		public static AudioManager Instance {
			get {
				return Me;
			}
		}

		public override bool forceResetMe {
			get {
				return true;
			}
		}
		
		public GameObject monoGameObject
		{
			get
			{
				return gameObject;
			}
		}

		public void SetListenerFollow(Transform t)
		{
			listenerFolow = t;
		}

		void Start ()
		{
			if (null == listener) {
				listener = GetComponentInChildren<AudioListener> ();
			}
			if (null == bgMusic) {
				bgMusic = GetComponentInChildren<AudioSource> ();
			}
			StartFadeIn ();
		}

		protected override void OnDestroy ()
		{
			CancelFadeIn ();
			base.OnDestroy ();
		}

		void LateUpdate()
		{
			if (null != listenerFolow)
			{
				listener.transform.position = listenerFolow.position;
			}
		}
	
		void CancelFadeIn ()
		{
			if (bgMusic != null) {
				LeanTween.cancel (this.gameObject);
				bgMusic.volume = volume;
			}
		}

		void StartFadeIn ()
		{
			if (bgMusic != null) {
				bgMusic.volume = fadeInStart;
				LeanTween.value (this.gameObject, Fade, fadeInStart * volume, volume, fadeInDuration).setDestroyOnComplete (true).setOnComplete (CancelFadeIn);
			}
		}

		void Fade (float val)
		{
			if (bgMusic != null)
				bgMusic.volume = val;
		}
	}
} // namespace RO
