using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class RoleRender_Alpha 
	{
		public float from{get;private set;}
		public float to{get;private set;}
		public float current{get;private set;}

		public float duration{get;private set;}
		private float timeElapsed = 0f;

		public bool on
		{
			get
			{
				return running || 1f > current;
			}
		}

		public bool running
		{
			get
			{
				return current != to;
			}
		}

		public RoleRender_Alpha()
		{
			from = 1f;
			to = 1f;
			current = 1f;
		}

		public void SetDuration(float d)
		{
			duration = d;
			if (0 >= duration)
			{
				current = to;
			}
		}

		public void LerpTo(float t, float d)
		{
			if (to != t)
			{
				from = current;
				to = t;
				timeElapsed = 0f;
			}
			SetDuration(d);
		}

		public void LerpTo(float f, float t, float d)
		{
			if (f != t)
			{
				from = f;
				to = t;
				current = f;
				timeElapsed = 0f;
			}
			SetDuration(d);
		}

		public void Update()
		{
			if (0 < duration)
			{
				current = Mathf.Lerp(from, to, timeElapsed/duration);
				timeElapsed += Time.deltaTime;
			}
			else
			{
				current = to;
			}
		}
	}
} // namespace RO
