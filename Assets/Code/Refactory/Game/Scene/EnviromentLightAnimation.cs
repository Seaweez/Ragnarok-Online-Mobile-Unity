using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class EnviromentLightAnimation : EnviromentAnimationBase 
	{
		public Color lightColor = Color.white;
		public float lightScale = 1;
		private bool prevEnable = false;

		protected override void Update ()
		{
			if (prevEnable != enable)
			{
				prevEnable = enable;
				if (null != LuaLuancher.Me)
				{
					LuaLuancher.Me.Call("SetWeatherAnimationEnable", enable);
				}
			}
			base.Update ();
		}
		
		public override void Set()
		{
			if (null != LuaLuancher.Me)
			{
				LuaLuancher.Me.Call("SetWeatherInfo", lightColor.r, lightColor.g, lightColor.b, lightColor.a, lightScale);
			}
		}
	}
} // namespace RO
