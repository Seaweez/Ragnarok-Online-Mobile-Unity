using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class MyCheckDoublcClick : MonoBehaviour
	{
		public UICamera.VoidDelegate onDoubleClick;
		float clicktime = 0;

		void OnClick ()
		{
			if (clicktime + 0.35f > RealTime.time) {
				DoubleClick ();
			}
			clicktime = RealTime.time;
		}
		
		void DoubleClick ()
		{
			if (onDoubleClick != null) {
				onDoubleClick (gameObject);
			}
		}
	}
} // namespace RO
