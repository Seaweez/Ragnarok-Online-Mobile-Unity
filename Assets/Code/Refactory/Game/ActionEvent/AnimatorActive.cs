using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	public class AnimatorActive : MonoBehaviour 
	{
		public Animator[] animators = null;

		public void ActionEvent_Enable(int i)
		{
			if (animators.CheckIndex(i))
			{
				animators[i].enabled = true;
			}
		}

		public void ActionEvent_Disable(int i)
		{
			if (animators.CheckIndex(i))
			{
				animators[i].enabled = false;
			}
		}
	
	}
} // namespace RO
