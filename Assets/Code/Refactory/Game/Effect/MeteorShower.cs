using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	public class MeteorShower : MonoBehaviour 
	{
		public RealFar realFar;
		public RealFarStreak realFarStreak;
		public Animator animator;
		public string animationName;
		public float animationLength;

		public Vector3 startPosition;
		public Vector3 startSpeed;
		public Vector3 acceleration;
		public float duration;

		public bool running{get;private set;}

		private Vector3 speed;
		private float endTime;
		private bool ending;
	
		public void Launch()
		{
			if (running)
			{
				Shutdown();
			}

			if (null == realFar)
			{
				return;
			}

			if (0 >= duration)
			{
				return;
			}

			if (null != realFarStreak)
			{
				realFarStreak.Clear();
			}
			if (null != animator && 0 < animationLength && !string.IsNullOrEmpty(animationName))
			{
				animator.speed = animationLength / duration;
				animator.Play(animationName, -1, 0);
			}

			position = startPosition;
			speed = startSpeed;
			endTime = Time.time + duration;
			ending = false;

			running = true;
		}

		public void Shutdown()
		{
			if (!running)
			{
				return;
			}
			running = false;
			ending = false;
		}

		private Vector3 position
		{
			get
			{
				return realFar.farPosition;
			}
			set
			{
				realFar.farPosition = value;
			}
		}

		void Reset()
		{
			if (null == realFar)
			{
				realFar = GetComponent<RealFar>();
			}
			if (null == realFarStreak)
			{
				realFarStreak = GetComponent<RealFarStreak>();
			}
		}

		void LateUpdate()
		{
			if (!running)
			{
				return;
			}

			if (ending)
			{
				if (null == realFarStreak || realFarStreak.empty)
				{
					Shutdown();
				}
			}
			else
			{
				position += speed * Time.deltaTime;
				if (Time.time >= endTime)
				{
					ending = true;
					return;
				}
				speed += Time.deltaTime * acceleration;
			}
		}
	}
} // namespace RO
