using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class AnimatorPlayer : MonoBehaviour 
	{
		public Animator[] animators;

		[SLua.CustomLuaClassAttribute]
		public class Params
		{
			public string stateName = null;
			public int layer = 0;
			public float normalizedTime = 0;
			public float speed = 1;
		}
	
		public void Play(Params p)
		{
			if (animators.IsNullOrEmpty())
			{
				return;
			}
			foreach (var a in animators)
			{
				var stateName = p.stateName;
				var stateNameHash = Animator.StringToHash(stateName);
				if (!a.HasState(p.layer, stateNameHash))
				{
					stateNameHash = a.GetCurrentAnimatorStateInfo(p.layer).shortNameHash;
				}
				a.speed = p.speed;
				a.Play(stateNameHash, p.layer, p.normalizedTime);
			}
		}
	}
} // namespace RO
