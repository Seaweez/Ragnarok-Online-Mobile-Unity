using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class AnimatorEventHelper : MonoBehaviour 
	{
		public Animator animator = null;
		public float fadeDuration = 0.3f;

		public void ActionEvent_CrossFade(string name)
		{
			if (null == animator)
			{
				return;
			}
			animator.CrossFade(name, fadeDuration, -1, 0);
		}
	
	}
} // namespace RO
