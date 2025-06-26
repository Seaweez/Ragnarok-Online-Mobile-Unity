

using System;

namespace SLua
{

	/// <summary>
	/// A bridge between UnityEngine.Debug.LogXXX and standalone.LogXXX
	/// </summary>
	internal class SluaLogger
	{
		public static void Log(string msg)
		{
#if !SLUA_STANDALONE
			UnityEngine.Debug.Log(msg);
#else
            Console.WriteLine(msg);
#endif
		}
		public static void LogError(string msg)
		{
#if !SLUA_STANDALONE
			UnityEngine.Debug.LogError(msg);
#else
            Console.WriteLine(msg);
#endif
		}

		public static void LogWarning(string msg)
		{
#if !SLUA_STANDALONE
			UnityEngine.Debug.LogWarning(msg);
#else
            Console.WriteLine(msg);
#endif
		}
	}


}

namespace RO
{
	public static class LoggerUnused
	{
		public static void Log(object msg)
		{
			if (!MyLuaSrv.EnablePrint)
			{
				return;
			}
			UnityEngine.Debug.Log(msg);
		}
		public static void LogWarning(object msg)
		{
			if (!MyLuaSrv.EnablePrint)
			{
				return;
			}
			UnityEngine.Debug.LogWarning(msg);
		}
		public static void LogWarning(object msg, UnityEngine.Object context)
		{
			if (!MyLuaSrv.EnablePrint)
			{
				return;
			}
			UnityEngine.Debug.LogWarning(msg, context);
		}
		public static void LogError(object msg)
		{
			if (!MyLuaSrv.EnablePrint)
			{
				return;
			}
			UnityEngine.Debug.LogError(msg);
		}

		public static void LogFormat(string msg, params object[] args)
		{
			if (!MyLuaSrv.EnablePrint)
			{
				return;
			}
			UnityEngine.Debug.LogFormat(msg, args);
		}
		public static void LogWarningFormat(string msg, params object[] args)
		{
			if (!MyLuaSrv.EnablePrint)
			{
				return;
			}
			UnityEngine.Debug.LogWarningFormat(msg, args);
		}
		public static void LogErrorFormat(string msg, params object[] args)
		{
			if (!MyLuaSrv.EnablePrint)
			{
				return;
			}
			UnityEngine.Debug.LogErrorFormat(msg, args);
		}

		public static void LogFormat(UnityEngine.Object obj, string msg, params object[] args)
		{
			if (!MyLuaSrv.EnablePrint)
			{
				return;
			}
			UnityEngine.Debug.LogFormat(obj, msg, args);
		}
		public static void LogWarningFormat(UnityEngine.Object obj, string msg, params object[] args)
		{
			if (!MyLuaSrv.EnablePrint)
			{
				return;
			}
			UnityEngine.Debug.LogWarningFormat(obj, msg, args);
		}
		public static void LogErrorFormat(UnityEngine.Object obj, string msg, params object[] args)
		{
			if (!MyLuaSrv.EnablePrint)
			{
				return;
			}
			UnityEngine.Debug.LogErrorFormat(obj, msg, args);
		}
	}
}