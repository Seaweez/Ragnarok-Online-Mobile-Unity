using UnityEngine;
using System.Collections.Generic;
using System;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class SystemTime
	{
		static DateTime orign = new DateTime (1970, 1, 1);

		public static double GetCurrentTimeStamp ()
		{
			DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime (orign);
			DateTime nowTime = DateTime.Now;
			return Math.Round(((nowTime - startTime).TotalMilliseconds),MidpointRounding.AwayFromZero);
		}
	
	}
} // namespace RO
