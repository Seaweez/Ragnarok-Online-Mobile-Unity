using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class UIPlayTweenExtension : UIPlayTween
	{
		public bool IsInit = false;

		public new void Play (bool forward)
		{
			if (!IsInit) {
				GameObject go = (tweenTarget == null) ? gameObject : tweenTarget;
				UITweener[] mTweens = includeChildren ? go.GetComponentsInChildren<UITweener> () : go.GetComponents<UITweener> ();
				int tweenFactor = forward ? 1 : 0;
				foreach (UITweener tween in mTweens) {
					tween.Sample (tweenFactor, true);
				}
			} else {
				base.Play (forward);
			}
		}
	}
} // namespace RO
