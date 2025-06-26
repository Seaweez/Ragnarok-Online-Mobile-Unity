using UnityEngine;
using System.Collections;

public class MouseEvent : FLEventBase
{
		public const string MOUSE_CLICK = "MOUSE_CLICK";
		public const string LONG_PRESS = "LONG_PRESS";
		public const string LONG_PRESS_UP = "LONG_PRESS_UP";

		public MouseEvent ():base(MOUSE_CLICK)
		{
		}

		public override FLEventBase Clone ()
		{
				MouseEvent e = base.Clone () as MouseEvent;
				e.target = this.target;
				return e;
		}
}
