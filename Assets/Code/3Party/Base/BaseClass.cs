using UnityEngine;

public class BaseClass
{

		public static void print (object msg)
		{
#if UNITY_EDITOR
			Debuger.Log(msg + "\n" + StackTraceUtility.ExtractStackTrace ());
#endif
		}
	
		public static void log (object msg)
		{
//		UnityEngine.Debug.Log (msg);
		}
	
		public static void logOnScene (string msg)
		{
//		NGUIDebug.Log (msg);
		}
}

public class Debuger
{
    public static void Log(string msg)
    {
        
    }

    public static bool EnableLog ;
    public static void LogError (string msg)
    {

    }
}