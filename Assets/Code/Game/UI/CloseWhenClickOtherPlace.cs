using UnityEngine;
using System.Collections.Generic;
using Ghost.Utils;
using System;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class CloseWhenClickOtherPlace : CallBackWhenClickOtherPlace
	{
		public bool isDestroy = false;
		public Action callBack = null;

		protected override void ReInit ()
		{
			base.ReInit ();
			base.call = Call;
		}
		
		void Call ()
		{
			if (isDestroy) {
				GameObject.DestroyImmediate (gameObject);
			} else {
				gameObject.SetActive (false);
			}
			if (callBack != null) {
				callBack ();
			}
		}

		public void ReCalculateBound ()
		{
			ReInit ();
		}

		public void AddTarget (Transform target)
		{
			if (!targets.Contains (target)) {
				targets.Add (target);
				ReInit ();
			}
		}

		public void RemoveTarget (Transform target)
		{
			targets.Remove (target);
			ReInit ();
		}
		
		public void ClearTarget ()
		{
			targets.Clear ();
			ReInit ();
		}
	}
} // namespace RO
