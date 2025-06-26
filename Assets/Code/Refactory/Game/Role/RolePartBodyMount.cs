using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class RolePartBodyMount : RolePart 
	{
		public AudioSource audioSource;
		public string[] audioEffects;
		private Dictionary<string, AudioClip> seClips;
	
		private AudioClip GetSE(string name)
		{
			if (null != seClips)
			{
				AudioClip clip;
				if (seClips.TryGetValue(name, out clip))
				{
					return clip;
				}
			}
			var seClip = ResourceManager.Loader.SLoad<AudioClip>(ResourcePathHelper.IDSE(name));
			if (null != seClip)
			{
				if (null == seClips)
				{
					seClips = new Dictionary<string, AudioClip>();
				}
				seClips.Add(name, seClip);
			}
			return seClip;
		}

		#region action event
		[SLua.DoNotToLuaAttribute]
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
			var clipPath = audioEffects[index];
			if (string.IsNullOrEmpty(clipPath))
			{
				return;
			}
			var clip = GetSE(clipPath);
			if (null == clip)
			{
				return;
			}
			AudioHelper.PlayOneShotOn (clip, audioSource);
		}
		
		[SLua.DoNotToLuaAttribute]
		public void ActionEventPlaySE(string clipPath)
		{
			if (null == audioSource)
			{
				return;
			}
			
			var clip = GetSE(clipPath);
			if (null == clip)
			{
				return;
			}
			AudioHelper.PlayOneShotOn (clip, audioSource);
		}

		[SLua.DoNotToLuaAttribute]
		public void ActionEventPlayEffectAt(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return;
			}
			string path = name;
			Vector3 epPosition;
			
			Transform ep = null;
			var i1 = name.IndexOf('[');
			if (0 <= i1)
			{
				path = name.Substring(0, i1);
				var i2 = name.IndexOf(']', i1);
				var start = i1+1;
				var len = i2-start;
				if (0 < len)
				{
					var n = int.Parse(name.Substring(start, len));
					ep = GetEP(n);
				}
			}
			if (null == ep)
			{
				epPosition = transform.position;
			}
			else
			{
				epPosition = ep.position;
			}
			if (null != LuaLuancher.Me)
			{
				LuaLuancher.Me.Call("PlayEffect_OneShotAt", path, epPosition.x, epPosition.y, epPosition.z);
			}
		}
		#endregion action event
	}
} // namespace RO
