using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class LeanTweenUtil
	{
		public static LTDescr UIAlpha (UIPanel panel, float from, float to, float time, float delayTime = 0)
		{
			return LeanTween.value (panel.gameObject, delegate(float obj) {
				if (panel != null)
					panel.alpha = obj;
			}, from, to, time).setDelay (delayTime);		
		}

		public static LTDescr moveLocal (GameObject go, Vector3 tPos, float duration)
		{
			return LeanTween.moveLocal (go, tPos, duration);
		}
	}
} // namespace RO
