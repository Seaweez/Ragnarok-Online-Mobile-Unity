using UnityEngine;
using System.Collections;
using System;
using System.Threading;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class SpeechRecognizer : MonoBehaviour{
		public delegate void VolumeCallback (int volume);
		public VolumeCallback updateVolume;
		private float currentTime = 0.0f;
		private new string name;

		public Action<byte[],float,string> handler{
			set;
			get;
		}

		public void SetName(String audioname)
		{
			name = audioname;
		}

		public void SetResult(string str)
		{
			if (handler != null && name != null) {
				String url = FileDirectoryHandler.GetAbsolutePath(name);

				StartCoroutine (GetWWW (url,delegate(WWW www){
					if(www != null)
					{
						AudioClip clip = www.GetAudioClip();
						
						float time = Mathf.Ceil(clip.length);
						byte[] bytes = www.bytes;
						if(bytes != null && time != 0)
						{
							handler(www.bytes,time,str);
						}
					}
				}));
			}
		}

		public void VolumeChanged(string volume)
		{
			if (updateVolume != null) {
				if(Time.time - currentTime > 1)
				{
					updateVolume(int.Parse(volume));
					currentTime = Time.time;
				}
			}
		}

		private IEnumerator GetWWW(string url , Action<WWW> action)
		{
			RO.LoggerUnused.Log("SpeechRecognizer GetAudio www path : " + "file://" + url);

			WWW www = new WWW("file://"+url);
			yield return www;
			if(string.IsNullOrEmpty(www.error) == false)
			{
				RO.LoggerUnused.Log("SpeechRecognizer GetAudio Did not work"); 
				yield break;
			}

			action(www);

			www.Dispose ();
			www = null;
		}
		
		public void GetAudio(string url,Action<AudioClip> action)
		{
			if (url == null)
				return;

			String urlAbsolute = FileDirectoryHandler.GetAbsolutePath(url);

			StartCoroutine (GetWWW (urlAbsolute,delegate(WWW www){
				if(www != null && action != null)
				{
					AudioClip clip = www.GetAudioClipCompressed();
					action(clip);
				}
			}));
		}
	}
} // namespace RO