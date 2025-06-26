using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	public class Carrier : PointSubject 
	{
		public AudioClip[] audioEffects = null;
		public AudioSource audioSource = null;
		public Animator animator = null;

		public void PlayAudioEffect(int index)
		{
			if (null == audioSource)
			{
				return;
			}
			if (audioEffects.IsNullOrEmpty())
			{
				return;
			}
			if (!audioEffects.CheckIndex(index))
			{
				return;
			}
			var clip = audioEffects[index];
			if (null == clip)
			{
				return;
			}
			AudioHelper.PlayOneShotOn (clip, audioSource);
		}

		public void PlayAction(string name)
		{
			if (null == animator)
			{
				return;
			}
			animator.Play(name, -1, 0);
		}

		public int GetSeatCount()
		{
			return null != connectPoints ? connectPoints.Length : 0;
		}

		public GameObject GetSeat(int seat)
		{
			if (0 > seat)
			{
				return GetFirstValidConnectPoint();
			}
			return GetConnectPoint(seat);
		}

#if OBSOLETE
		public bool GetOn(int seat, RoleAgent role)
		{
			var cp = GetSeat(seat);
			if (null == cp)
			{
				return false;
			}
			
			role.Idle();
			role.transform.parent = cp.transform;
			role.position = cp.transform.position;
			role.rotation = new Quaternion();
			return true;
		}
		
		public bool GetOff(RoleAgent role, Vector3 position)
		{
			role.transform.parent = null;
			role.PlaceTo(position);
			role.RestoreTransform();
			return true;
		}
#endif

		public void ClearPassengers()
		{
			var seats = connectPoints;
			if (seats.IsNullOrEmpty())
			{
				return;
			}
			foreach (var seat in seats)
			{
				if(seat!=null)
				{
					var childCount = seat.transform.childCount;
					for (int i = 0; i < childCount; ++i)
					{
						var child = seat.transform.GetChild(i);
						GameObject.Destroy(child.gameObject);
					}
				}
			}
		}

		protected override void Reset ()
		{
			base.Reset ();
			audioSource = GetComponentInChildren<AudioSource>();
			animator = GetComponent<Animator>();
		}
		
		protected override void Start()
		{
			base.Start();
			if (null == audioSource)
			{
				audioSource = GetComponent<AudioSource>();
			}
			if (null == animator)
			{
				animator = GetComponent<Animator>();
			}
		}

		protected override void OnDestroy ()
		{
			base.OnDestroy ();
			audioEffects = null;
		}
	
	}
} // namespace RO
