using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class PlayActive : MonoBehaviour
	{
		public AnimationOrTween.Trigger trigger = AnimationOrTween.Trigger.OnClick;
		public GameObject Target;
		public bool StartState;

		void Start ()
		{
			if (Target != null)
				Target.SetActive (StartState);
		}

		void OnClick ()
		{
			if (trigger == AnimationOrTween.Trigger.OnClick) {
				if (Target != null) {
					StartState = Target.activeInHierarchy;
					StartState = !StartState;
					Target.SetActive (StartState);
				}
			}
		}

		void OnPress (bool press)
		{
			if (trigger == AnimationOrTween.Trigger.OnPress) {
				if (Target != null) {
					Target.SetActive (press);
				}
			}
		}
	
	}
} // namespace RO
