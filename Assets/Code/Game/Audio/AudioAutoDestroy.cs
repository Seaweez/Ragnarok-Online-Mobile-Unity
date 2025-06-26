using UnityEngine;
using System.Collections.Generic;
using System;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class AudioAutoDestroy : MonoBehaviour
	{
		public ResourceID resID = null;
		public string resPath = null;
		public AudioSource source = null;
		public Action OnFinish;

		public void Destroy ()
		{
			if (OnFinish != null)
				OnFinish ();
			if (null != resID) 
			{
				GameObjPool.Me.RAddEx (gameObject, resID, Config.Pool.NAME_AUDIO, true);
			} 
			else if (!string.IsNullOrEmpty(resPath))
			{
				GameObjPool.Me.AddEx (gameObject, resPath, Config.Pool.NAME_AUDIO, true);
			}
			else 
			{
				GameObject.Destroy (gameObject);
			}
//			Component.Destroy (this);
		}

		void Start ()
		{
			if (null == source) {
				source = GetComponent<AudioSource> ();
			}
		}

		void LateUpdate ()
		{
			if (null == source) {
				Destroy ();
				return;
			}

			if (!source.isPlaying) {
				Destroy ();
				return;
			}
		}
	
	}
} // namespace RO
