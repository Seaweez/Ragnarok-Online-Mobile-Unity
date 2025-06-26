using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class ActionSequence
	{
		private Queue<AnimatorHelper.CrossFadeParam> sequence = null;
		private AnimatorHelper.CrossFadeParam currentParam_ = null;

		private AnimatorHelper.CrossFadeParam currentParam {
			get {
				return currentParam_;
			}
			set {
				if (value == currentParam) {
					return;
				}
				currentParam_ = value;
				ApplyCurrent ();
			}
		}

		private AnimatorHelper helper_ = null;

		private AnimatorHelper helper {
			get {
				return helper_;
			}
			set {
				if (value == helper) {
					return;
				}
				if (null != helper) {
					helper.loopCountChangedListener -= OnLoopCountChanged;
				}
				helper_ = value;
				if (null != helper) {
					helper.loopCountChangedListener += OnLoopCountChanged;
					ApplyCurrent ();
				}
			}
		}

		private void ApplyCurrent ()
		{
			if (null != helper && null != currentParam) {
				helper.CrossFade (currentParam.stateName, currentParam.crossFadeDuration, currentParam.speed, currentParam.reset);
			}
		}

		private void Next ()
		{
			if (null != sequence) {
				if (0 < sequence.Count) {
					currentParam = sequence.Dequeue ();
					if (0 < sequence.Count) {
						return;
					}
				}
			}
			End ();
		}

		public void StartArray (AnimatorHelper h, AnimatorHelper.CrossFadeParam[] actionParams)
		{
			if (null == h || actionParams.IsNullOrEmpty ()) {
				End ();
				return;
			}
			helper = h;
			sequence = new Queue<AnimatorHelper.CrossFadeParam> ();
			foreach (var actionParam in actionParams) {
				sequence.Enqueue (actionParam);
			}
			Next ();
		}

		[SLua.DoNotToLuaAttribute]
		public void Start (AnimatorHelper h, params AnimatorHelper.CrossFadeParam[] actionParams)
		{
			StartArray (h, actionParams);
		}

		public void StartForLua (AnimatorHelper h, params object[] actionParams)
		{
			AnimatorHelper.CrossFadeParam[] tempA = new AnimatorHelper.CrossFadeParam[actionParams.Length];
			for (int i=0; i<actionParams.Length; i++) {
				tempA [i] = actionParams [i] as AnimatorHelper.CrossFadeParam;
			}
			StartArray (h, tempA);
		}

		public void End ()
		{
			helper = null;
			sequence = null;
			currentParam = null;
		}

		private void OnLoopCountChanged (AnimatorStateInfo state, int oldLoopCount, int newLoopCount)
		{
			if (null != currentParam && state.IsName (currentParam.stateName)) {
				if (currentParam.loopCount <= newLoopCount) {
					Next ();
				}
			}
		}

	}
} // namespace RO
