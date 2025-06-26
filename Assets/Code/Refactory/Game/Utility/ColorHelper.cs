using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class ColorHelper
	{
		public static bool TryParseHtmlString (string str, out float r, out float g, out float b, out float a)
		{
			Color color;
			if (ColorUtility.TryParseHtmlString(str, out color))
			{
				r = color.r;
				g = color.g;
				b = color.b;
				a = color.a;
				return true;
			}
			r = 0;
			g = 0;
			b = 0;
			a = 0;
			return false;
		}
	}
} // namespace RO
