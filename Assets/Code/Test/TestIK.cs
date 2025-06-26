using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class TestIK : MonoBehaviour 
	{
		public bool IKActive = true;
		public Transform targetObject;

		public Animator animator;

		void Reset()
		{
			animator = GetComponent<Animator>();
		}

		void Start()
		{
			if (null == animator)
			{
				animator = GetComponent<Animator>();
			}
		}

		void OnAnimatorIK()
		{
			if (null != animator)
			{
				if (IKActive)
				{
					animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
					if (null != targetObject)
					{
						animator.SetIKPosition(AvatarIKGoal.RightHand, targetObject.position);
					}
				}
				else
				{
					animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
				}
			}
		}
	}
} // namespace RO
