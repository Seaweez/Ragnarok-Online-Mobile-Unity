using UnityEngine;
using System.Collections;
using System;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public static class DateTimeHelper
	{
		public const float MINUTES_AN_HOUR = 60;
		public const float SECONDS_A_MINUTE = 60;
		public const float HOURS_A_DAY = 24;
		public const float SECONDS_AN_HOUR = MINUTES_AN_HOUR * SECONDS_A_MINUTE;
		public const float MINUTES_A_DAY = HOURS_A_DAY * MINUTES_AN_HOUR;
		public const float SECONDS_A_DAY = HOURS_A_DAY * SECONDS_AN_HOUR;

		public static bool IsCurTimeInRegion (string startStrTime, string endStrTime)
		{
			bool b = false;
			if (string.IsNullOrEmpty (startStrTime) || string.IsNullOrEmpty (endStrTime)) {
				RO.LoggerUnused.LogError ("some args is null");
				return b;
			}
			DateTime curTime = DateTime.Now;
			DateTime startTime = GetDateTimeByStr (startStrTime);
			DateTime endTime = GetDateTimeByStr (endStrTime);
			if (curTime > startTime && curTime < endTime) {
				b = true;
			}
			return b;
		}

		public static float GetTimeProgressInDay ()
		{
			var now = DateTime.Now;
			var secondsInDay = ((now.Hour * MINUTES_AN_HOUR + now.Minute) * SECONDS_A_MINUTE + now.Second);
			return GetTimeProgressInDay (secondsInDay);
		}

		public static float GetTimeProgressInDay (float secondsInDay)
		{
			return secondsInDay / SECONDS_A_DAY;
		}

		public static long DateTimeToTimeStamp (DateTime dateTime)
		{
			var start = new DateTime (1970, 1, 1, 0, 0, 0, dateTime.Kind);
			return Convert.ToInt64 ((dateTime - start).TotalSeconds);
		}

		public static string FormatTimeTick (long secendStamp, string format)
		{
			var dt = new DateTime (1970, 1, 1, 0, 0, 0);
			dt = dt.AddSeconds (secendStamp);
			return dt.ToString (format);
		}

		public static bool IsTimeInRegionByDeltaDay (string startStrTime, long secendStamp, int deltaDay)
		{
			startStrTime = startStrTime + "-00-00-0";
			var dt = GetDateTimeByStr (startStrTime);
			dt = dt.AddSeconds (deltaDay * 24 * 3600);
			return DateTimeToTimeStamp (dt) >= secendStamp;
		}

		public static DateTime SysNow ()
		{
			return DateTime.Now;
		}

		private static DateTime GetDateTimeByStr (string str)
		{
			string[] strArr = str.Split ('-');
			if (strArr.Length == 6) {
				int[] arr = new int[6];
				for (int i=0; i<strArr.Length; ++i) {
					arr [i] = int.Parse (strArr [i]);
				}
				DateTime dt = new DateTime (arr [0], arr [1], arr [2], arr [3], arr [4], arr [5]);
				return dt;
			} else {
				RO.LoggerUnused.LogError ("GetDateTimeByStr str format error!");
				return new DateTime ();
			}
		}
	}
}
