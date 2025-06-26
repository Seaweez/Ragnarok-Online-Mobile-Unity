using UnityEngine;
using System;
using System.Collections.Generic;
using SLua;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public static class ROLogger
	{
		#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX
		public static bool enable = true;
		#else
		public static bool enable = false;
		#endif

		public static void Log(object msg)
		{
			if (!enable)
			{
				return;
			}
			Debug.Log(msg);
		}
		public static void LogWarning(object msg)
		{
			if (!enable)
			{
				return;
			}
			Debug.LogWarning(msg);
		}
		public static void LogError(object msg)
		{
			if (!enable)
			{
				return;
			}
			UnityEngine.Debug.LogError(msg);
		}

		public static void Log(object msg, UnityEngine.Object context)
		{
			if (!enable)
			{
				return;
			}
			Debug.Log(msg, context);
		}
		public static void LogWarning(object msg, UnityEngine.Object context)
		{
			if (!enable)
			{
				return;
			}
			Debug.LogWarning(msg, context);
		}
		public static void LogError(object msg, UnityEngine.Object context)
		{
			if (!enable)
			{
				return;
			}
			Debug.LogError(msg, context);
		}
		
		public static void LogFormat(string msg, params object[] args)
		{
			if (!enable)
			{
				return;
			}
			UnityEngine.Debug.LogFormat(msg, args);
		}
		public static void LogWarningFormat(string msg, params object[] args)
		{
			if (!enable)
			{
				return;
			}
			UnityEngine.Debug.LogWarningFormat(msg, args);
		}
		public static void LogErrorFormat(string msg, params object[] args)
		{
			if (!enable)
			{
				return;
			}
			UnityEngine.Debug.LogErrorFormat(msg, args);
		}
		
		public static void LogFormat(UnityEngine.Object obj, string msg, params object[] args)
		{
			if (!enable)
			{
				return;
			}
			UnityEngine.Debug.LogFormat(obj, msg, args);
		}
		public static void LogWarningFormat(UnityEngine.Object obj, string msg, params object[] args)
		{
			if (!enable)
			{
				return;
			}
			UnityEngine.Debug.LogWarningFormat(obj, msg, args);
		}
		public static void LogErrorFormat(UnityEngine.Object obj, string msg, params object[] args)
		{
			if (!enable)
			{
				return;
			}
			UnityEngine.Debug.LogErrorFormat(obj, msg, args);
		}
	}
} // namespace RO
