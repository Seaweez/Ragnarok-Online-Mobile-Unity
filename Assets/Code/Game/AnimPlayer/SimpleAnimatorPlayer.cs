using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class SimpleAnimatorPlayer : MonoBehaviour
	{
		public AnimatorHelper animatorHelper{ get; private set; }

		void Awake ()
		{
			Animator animator = GetComponent<Animator> ();
			if (null != animator) {
				animatorHelper = new AnimatorHelper (animator);
				animatorHelper.defaultStateName = Config.RoleAction.WAIT;
			}
		}
	
		public void FireEventByName (string name)
		{
			if (animatorHelper != null) {
//				print (name);
				animatorHelper.NotifyActionEvent (0, name);
			}
		}

		public void FireEventById (int id)
		{
			if (animatorHelper != null) {
				animatorHelper.NotifyActionEvent (id);
			}
		}

		void Update()
		{
			if (null != animatorHelper)
			{
				animatorHelper.Update();
			}
		}
	}
} // namespace RO
