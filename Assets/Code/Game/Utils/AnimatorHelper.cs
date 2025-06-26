using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public static class AnimatorExtensions
	{
		public static bool IsValid(this AnimatorStateInfo info)
		{
			return 0 != info.fullPathHash;
		}
	}

	[SLua.CustomLuaClassAttribute]
	public class AnimatorHelper 
	{
		[SLua.CustomLuaClassAttribute]
		public class CrossFadeParam
		{
			public string stateName = string.Empty;
			public float crossFadeDuration = 0f;
			public float speed = 1f;
			public bool reset = false;
			public int loopCount = 1;
			public float normalizedTime = 0f;

			public CrossFadeParam()
			{
			}
			public CrossFadeParam(string sn, float cfd, int lc = 1, float s = 1, bool r = false, float nt = 0f)
			{
				Set (sn, cfd, lc, s, r, nt);
			}

			public bool valid
			{
				get
				{
					return !string.IsNullOrEmpty(stateName);
				}
			}

			public void Set(string sn, float cfd, int lc = 1, float s = 1, bool r = false, float nt = 0f)
			{
				stateName = sn;
				crossFadeDuration = cfd;
				speed = s;
				reset = r;
				loopCount = lc;
				normalizedTime = nt;
			}
		}

		public delegate void RawListener(Animator animator);
		public delegate void StateChangedListener(AnimatorStateInfo oldState, AnimatorStateInfo newState);
		public delegate void LoopCountChangedListener(AnimatorStateInfo state, int oldLoopCount, int newLoopCount);
		public delegate void ActionEventListener(AnimatorStateInfo state, int eventID, object eventData);

		public RawListener rawListener;
		public StateChangedListener stateChangedListener;
		public LoopCountChangedListener loopCountChangedListener;
		public ActionEventListener actionEventListener;

		public string defaultStateName{get;set;}
		public string forceDefaultStateName{get;set;}

		private AnimatorStateInfo currentState_;
		private CrossFadeParam nextActionParam = new CrossFadeParam();

		private float speed = 1f;
		private bool forceSpeed = false;

		private Animator animator{get;set;}
		private Animator[] animators{get;set;}

		public AnimatorHelper(Animator a, Animator[] ats = null)
		{
			animator = a;
			currentState_ = animator.GetCurrentAnimatorStateInfo(0);

			if (ats.IsNullOrEmpty())
			{
				RefreshSubAnimators();
			}
			else
			{
				RefreshSubAnimators(ats);
			}
		}


		public AnimatorStateInfo currentState
		{
			get
			{
				return currentState_;
			}
		}

		public bool HasState(string stateName)
		{
			var stateNameHash = Animator.StringToHash(stateName);
			return null != animator && animator.HasState(0, stateNameHash);
		}

		public void RefreshSubAnimators(GameObject[] externals = null)
		{
			animators = animator.GetComponentsInChildren<Animator>();
			if (!externals.IsNullOrEmpty())
			{
				var allAnimators = new List<Animator>();
				allAnimators.AddRange(animators);
				
				foreach (var ext in externals)
				{
					var extAnimators = ext.GetComponentsInChildren<Animator>();
					if (!extAnimators.IsNullOrEmpty())
					{
						foreach (var extAnimator in extAnimators)
						{
							if (allAnimators.Contains(extAnimator))
							{
								continue;
							}
							allAnimators.Add(extAnimator);
						}
					}
				}
				animators = allAnimators.ToArray();
			}
		}

		public void RefreshSubAnimators(Animator[] newAnimators)
		{
			animators = newAnimators;
		}

		private void DoCrossFade(CrossFadeParam param)
		{
			var playSpeed = forceSpeed ? speed : param.speed;
			if (0 == playSpeed)
			{
				return;
			}
			foreach (var a in animators)
			{
				if (null == a)
				{
					continue;
				}
				if (0 >= a.layerCount)
				{
					continue;
				}
				var stateName = param.stateName;
				var stateNameHash = Animator.StringToHash(stateName);
				if (!a.HasState(0, stateNameHash))
				{
					stateName = defaultStateName;
					stateNameHash = Animator.StringToHash(stateName);
					if (!a.HasState(0, stateNameHash))
					{
						stateName = forceDefaultStateName;
						stateNameHash = Animator.StringToHash(stateName);
						if (!a.HasState(0, stateNameHash))
						{
							continue;
						}
					}
				}
				a.speed = playSpeed;
				var currentState = a.GetCurrentAnimatorStateInfo(0);
				var nextState = a.GetNextAnimatorStateInfo(0);
				if (param.reset 
				    || (!currentState.IsValid() || currentState.shortNameHash != stateNameHash) 
				    || (nextState.IsValid() && nextState.shortNameHash != stateNameHash))
				{
					if (0 < param.crossFadeDuration)
					{
						a.CrossFade(stateName, param.crossFadeDuration, -1, param.normalizedTime);
					}
					else
					{
						a.Play(stateName, -1, param.normalizedTime);
					}
				}
			}
		}

		public void CrossFade(CrossFadeParam param)
		{
			nextActionParam = param;
		}

		public void CrossFade(string stateName, float crossFadeDuration, float speed = 1, bool reset = false)
		{
			nextActionParam.Set(stateName, crossFadeDuration, 1, speed, reset);
		}

		public void CrossFade(string stateName, float crossFadeDuration, float speed, bool reset, float normalizedTime)
		{
			nextActionParam.Set(stateName, crossFadeDuration, 1, speed, reset, normalizedTime);
		}
		
		public void CrossFadeForce(string stateName, float crossFadeDuration, float speed = 1)
		{
			CrossFade (stateName, crossFadeDuration, speed, true);
		}

		public void CrossFadeForce(string stateName, float crossFadeDuration, float speed, float normalizedTime)
		{
			CrossFade (stateName, crossFadeDuration, speed, true, normalizedTime);
		}

		public void Play(CrossFadeParam param)
		{
			param.crossFadeDuration = 0;
			CrossFade(param);
		}

		public void Play(string stateName, float speed = 1, bool reset = false)
		{
			CrossFade(stateName, 0, speed, reset);
		}

		public void Play(string stateName, float speed, bool reset, float normalizedTime)
		{
			CrossFade(stateName, 0, speed, reset, normalizedTime);
		}

		public void PlayForce(string stateName, float speed = 1)
		{
			Play (stateName, speed, true);
		}

		public void PlayForce(string stateName, float speed, float normalizedTime)
		{
			Play (stateName, speed, true, normalizedTime);
		}

		private void DoSetSpeed(float s)
		{
			foreach (var a in animators)
			{
				a.speed = s;
			}
			speed = s;
		}

		public void SetSpeed(float s)
		{
			DoSetSpeed(s);
			forceSpeed = true;
		}

		public void ClearSpeed()
		{
			if (!forceSpeed)
			{
				return;
			}
			SetSpeed(1);
			forceSpeed = false;
		}

		public void Update()
		{
			if (nextActionParam.valid)
			{
				DoCrossFade(nextActionParam);
				nextActionParam.stateName = string.Empty;
			}

			if (null != rawListener)
			{
				rawListener(animator);
			}
			var newState = animator.GetCurrentAnimatorStateInfo(0);
			if (currentState_.fullPathHash != newState.fullPathHash)
			{
				if (null != stateChangedListener)
				{
					stateChangedListener(currentState_, newState);
				}
			}
			else 
			{
				if (null != loopCountChangedListener)
				{
					var oldLoopCount = (int)currentState_.normalizedTime;
					var newLoopCount = (int)newState.normalizedTime;
					if (oldLoopCount != newLoopCount)
					{
						loopCountChangedListener(currentState_, oldLoopCount, newLoopCount);
					}
				}
			}
			currentState_ = newState;
		}

		public void NotifyActionEvent(int eventID, object eventData = null)
		{
			if (null != actionEventListener)
			{
				actionEventListener(currentState_, eventID, eventData);
			}
		}
	
	}
} // namespace RO
