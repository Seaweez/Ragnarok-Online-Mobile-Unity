using UnityEngine;
using System.Collections;

namespace RO.Net
{
	/**
	 * Log class
	 */ 
	[SLua.CustomLuaClassAttribute]
	public class NetLog
	{
		// log
		public static void Log(string msg)
		{
			if (NetConnectionManager.Me != null && NetConnectionManager.Me.EnableLog)
				RO.LoggerUnused.Log(msg);
		}

		// log warn
		public static void LogW(string msg)
		{
			NetLog.Log("<color=orange>" + msg + "</color>");
		}
		
		// log error
		public static void LogE(string msg)
		{
			NetLog.Log("<color=red>" + msg + "</color>");
		}
	}
}