using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class RoleRender_ChangeColor
	{
		public Color from{get;private set;}
		public Color to{get;private set;}
		public Color current{get;private set;}

		public float duration{get;private set;}
		private float timeElapsed = 0f;

		public bool on
		{
			get
			{
				return running || 0f < current.a;
			}
		}

		public bool running
		{
			get
			{
				return current.r != to.r || current.g != to.g || current.b != to.b || current.a != to.a;
			}
		}

		public RoleRender_ChangeColor()
		{
			from = new Color(1,1,1,0);
			to = new Color(1,1,1,0);
			current = new Color(1,1,1,0);
		}
		
		public void SetDuration(float d)
		{
			duration = d;
			if (0 >= duration)
			{
				current = to;
			}
		}

		public void LerpTo(Color t, float d)
		{
			if (to != t)
			{
				from = current;
				to = t;
				timeElapsed = 0f;
			}
			SetDuration(d);
		}

		public void LerpFromTo(Color f, Color t, float d)
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
				current = Color.Lerp(from, to, timeElapsed/duration);
				timeElapsed += Time.deltaTime;
			}
			else
			{
				current = to;
			}
		}
	}
} // namespace RO
