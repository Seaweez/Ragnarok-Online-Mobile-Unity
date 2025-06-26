using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class EffectHandle : MonoBehaviour 
	{
		public static List<EffectHandle> tempEffectList = new List<EffectHandle>();

		[System.Flags, SLua.CustomLuaClassAttribute]
		public enum EffectType
		{
			NONE = 0,
			PARTICLE_SYSTEM = 1 << 0,
			ANIMATOR = 1 << 1,
			LOGIC = 1 << 2,
			ALL = PARTICLE_SYSTEM | ANIMATOR | LOGIC
		}

		public int epID{get;set;}
		
		public EffectType ignoreEffectType = EffectType.NONE;
		public ParticleSystem[] particles = null;
		public Animator[] animators = null;
		public EffectLogic[] logics = null;

		private bool _componentsDisable = false; 
		public bool componentsDisable
		{
			get
			{
				return _componentsDisable;
			}
			set
			{
				if (value == componentsDisable)
				{
					return;
				}
				_componentsDisable = value;
				ApplyComponentsEnable();
			}
		}
		private void ApplyComponentsEnable()
		{
			var enable = !componentsDisable;
			if (!animators.IsNullOrEmpty())
			{
				for (int i = 0; i < animators.Length; ++i)
				{
					animators[i].enabled = enable;
				}
			}
			if (!particles.IsNullOrEmpty())
			{
				for (int i = 0; i < particles.Length; ++i)
				{
					if (enabled)
					{
						particles[i].Play();
					}
					else
					{
						particles[i].Stop();
						particles[i].Clear();
					}
				}
			}
			if (!logics.IsNullOrEmpty())
			{
				for (int i = 0; i < logics.Length; ++i)
				{
					logics[i].enabled = enable;
				}
			}
		}

		public bool running {get;private set;}
		
		public static bool CheckEffectType(EffectType test, EffectType type)
		{
			return type == (test & type);
		}

		public void Restore()
		{
			if (!particles.IsNullOrEmpty())
			{
				for (int i = 0; i < particles.Length; ++i) 
				{
					var particle = particles[i];
					particle.Stop ();
					particle.Clear ();
					particle.Play ();
				}
			}
		}

		public void SetPlaybackSpeed(float speed)
		{
			if (!particles.IsNullOrEmpty())
			{
				for (int i = 0; i < particles.Length; ++i) 
				{
					var particle = particles[i];
					var main = particle.main;
					main.simulationSpeed = speed;
				}
			}
			if (!animators.IsNullOrEmpty())
			{
				for (int i = 0; i < animators.Length; ++i) 
				{
					var a = animators[i];
					a.speed = speed;
				}
			}
		}

		#region behaviour
		protected virtual void Awake ()
		{
//			particles = GetComponentsInChildren<ParticleSystem> ();
//			animators = GetComponentsInChildren<Animator> ();
//			logics = GetComponentsInChildren<EffectLogic> ();
		}

		protected virtual void OnEnable()
		{
			running = true;
		}

		protected virtual void OnDisable()
		{
			running = false;
		}
		
		protected virtual void LateUpdate ()
		{
			if (!running)
			{
				return;
			}

			EffectType completedEffects = EffectType.NONE;
			
			// particles
			if (CheckEffectType(ignoreEffectType, EffectType.PARTICLE_SYSTEM) || particles.IsNullOrEmpty ()) 
			{
				completedEffects |= EffectType.PARTICLE_SYSTEM;
			} 
			else 
			{
				int completedCount = 0;
				for (int i = 0; i < particles.Length; ++i) 
				{
					var particle = particles[i];
					if (particle.main.loop || !particle.isPlaying) 
					{
						++completedCount;
					}
				}
				if (particles.Length == completedCount) 
				{
					completedEffects |= EffectType.PARTICLE_SYSTEM;
				}
			}
			
			// animators
			if (CheckEffectType(ignoreEffectType, EffectType.ANIMATOR) || animators.IsNullOrEmpty ())
			{
				completedEffects |= EffectType.ANIMATOR;
			} 
			else 
			{
				int completedCount = 0;
				for (int i = 0; i < animators.Length; ++i) 
				{
					var animator = animators[i];
					if (null == animator.runtimeAnimatorController 
					    || (1 < animator.GetCurrentAnimatorStateInfo (0).normalizedTime && !animator.GetNextAnimatorStateInfo (0).IsValid ())) 
					{
						++completedCount;
					}
				}
				if (animators.Length == completedCount) 
				{
					completedEffects |= EffectType.ANIMATOR;
				}
			}
			
			// logics
			if (CheckEffectType(ignoreEffectType, EffectType.LOGIC) || logics.IsNullOrEmpty ()) 
			{
				completedEffects |= EffectType.LOGIC;
			} 
			else 
			{
				int completedCount = 0;
				foreach (var logic in logics) 
				{
					if (!logic.running) 
					{
						++completedCount;
					}
				}
				if (logics.Length == completedCount) {
					completedEffects |= EffectType.LOGIC;
				}
			}
			
			if (EffectType.ALL == completedEffects) 
			{
				running = false;
			}
		}
		#endregion behaviour
	
	}
} // namespace RO
