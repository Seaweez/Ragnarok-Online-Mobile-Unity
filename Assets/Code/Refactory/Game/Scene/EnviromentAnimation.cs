using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	public class EnviromentAnimationBase : MonoBehaviour
	{
		public int priority = 0;

		public bool enable = false;

		protected virtual void Update()
		{
			if (!enable)
			{
				return;
			}
			Set();
		}

		public virtual void Get()
		{
		}

		public virtual void Set()
		{
		}
	}
} // namespace RO
