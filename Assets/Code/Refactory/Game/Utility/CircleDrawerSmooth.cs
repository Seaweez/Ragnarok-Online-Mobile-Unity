using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class CircleDrawerSmooth : MonoBehaviour 
	{
		public CircleDrawer drawer;
		public float duration = 1;

		public float radius;

		private float velocity = 0f;
		private bool smoothing = false;

		private Coroutine smoothCoroutine = null;

		private void DoSet(float r)
		{
			if (null == drawer)
			{
				return;
			}
			drawer.radius = r;
			drawer.Draw();
		}

		public void Set()
		{
			DoSet(radius);
			smoothing = false;
		}

		public void SmoothSet()
		{
			if (smoothing || null == drawer || drawer.radius == radius)
			{
				return;
			}
			velocity = 0;
			smoothing = true;
			if (null == smoothCoroutine)
			{
				smoothCoroutine = StartCoroutine(StartSmooth());
			}
		}

		private void SmoothStep()
		{
			if (null == drawer)
			{
				return;
			}
			if (0.001f > Mathf.Abs(drawer.radius-radius))
			{
				DoSet(radius);
				smoothing = false;
			}
			else
			{
				DoSet(Mathf.SmoothDamp(drawer.radius, radius, ref velocity, duration));
				if (drawer.radius == radius)
				{
					smoothing = false;
				}
			}
		}

		private IEnumerator StartSmooth()
		{
			while (smoothing)
			{
				SmoothStep();
				yield return null;
			}
			smoothCoroutine = null;
		}
	}
} // namespace RO
