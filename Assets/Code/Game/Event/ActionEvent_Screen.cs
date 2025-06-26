using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class ActionEvent_Screen : MonoBehaviour 
	{
		public SpriteFade screenMask = null;

		public void ActionEvent_ScreenMaskFadeIn(float duration)
		{
			if (null != screenMask)
			{
				screenMask.fadeDuration = duration;
				screenMask.FullScreen();
				screenMask.FadeIn();
			}
		}

		public void ActionEvent_ScreenMaskFadeOut(float duration)
		{
			if (null != screenMask)
			{
				screenMask.FadeOut();
			}
		}
	}
} // namespace RO
