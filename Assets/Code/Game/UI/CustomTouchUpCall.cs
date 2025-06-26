using UnityEngine;
using System.Collections.Generic;
using System;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class CustomTouchUpCall : CallBackWhenClickOtherPlace
	{
		public new Action call = null;
		public Func<bool> check = null;

		void Update ()
		{
			if (Input.GetMouseButtonDown (0)) {
				catchPoses [0] = Input.mousePosition;
			} else if (Input.GetMouseButtonUp (0)) {
				Vector3 pos;
				if (catchPoses.TryGetValue (0, out pos)) {
					if (!CheckPos (pos) && !excute) {
						excute = excuteOnce;
						catchPoses.Clear ();
						if (check == null || !check ()) {
							if (call != null) {
								call ();
							}
						}
					}
				}
			}
			excute = false;
		}
	}
} // namespace RO
