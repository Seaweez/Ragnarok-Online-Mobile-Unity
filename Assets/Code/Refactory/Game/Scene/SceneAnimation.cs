using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class SceneAnimation : LuaGameObject 
	{
		public Animator animator;
		public SpriteFade screenMask;
		public float screenMaskFadeDuration = 1;

		private Coroutine checkEndCoroutine = null;

		private IEnumerator CheckEnd()
		{
			while (true)
			{
				if (!(null != animator && animator.enabled))
				{
					break;
				}
				var currentState = animator.GetCurrentAnimatorStateInfo(0);
				if (!currentState.IsValid())
				{
					break;
				}
				var nextState = animator.GetNextAnimatorStateInfo(0);
				if (1 < currentState.normalizedTime && !nextState.IsValid())
				{
					break;
				}
				
				if (null != screenMask 
				    && SpriteFade.Phase.VISIBLE == screenMask.phase
				    && 1 <= screenMask.alpha)
				{
					break;
				}
				yield return 0;
			}
			DoStop();
		}

		public void DoStop()
		{
			if (null == checkEndCoroutine)
			{
				return;
			}
			checkEndCoroutine = null;
			
			if (null != screenMask)
			{
				screenMask.FadeOut();
			}

			animator.enabled = false;
		}

		public void Play()
		{
			if (null != checkEndCoroutine)
			{
				return;
			}

			animator.enabled = true;
			checkEndCoroutine = StartCoroutine(CheckEnd());
		}
		
		public void Stop()
		{
			if (null == checkEndCoroutine)
			{
				return;
			}

			if (null != screenMask)
			{
				if (SpriteFade.Phase.INVISIBLE == screenMask.phase)
				{
					screenMask.fadeDuration = screenMaskFadeDuration;
					screenMask.FullScreen();
					screenMask.FadeIn();
				}
			}
			else
			{
				DoStop();
			}
		}

		[SLua.DoNotToLuaAttribute]
		public void ActionEvent_ScreenMaskFadeIn(float duration)
		{
			if (null != screenMask)
			{
				screenMask.fadeDuration = duration;
				screenMask.FullScreen();
				screenMask.FadeIn();
			}
		}

		#region behaviour
		protected override void Awake ()
		{
			animator.enabled = false;
			base.Awake ();
		}
		#endregion behaviour
	
	}
} // namespace RO
