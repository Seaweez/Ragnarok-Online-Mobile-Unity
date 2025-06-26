using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace CloudVoiceVideoTroops
{

    public class CloudVoiceLogPrint
	{
		private static int s_logLevel = (int)logLevel.LOG_LEVEL_DEBUG;

	    public static void setLogLevel(int loglevel)
		{
            s_logLevel = loglevel;
		}

		public static void DebugLog (string function,string msg)
		{
            LogPrintf(function, msg, (int)logLevel.LOG_LEVEL_DEBUG);
		}

        public static void InfoLog(string function, string msg)
		{
            LogPrintf(function, msg, (int)logLevel.LOG_LEVEL_INFO);
		}

        public static void ErrorLog(string function, string msg)
		{
            LogPrintf(function, msg, (int)logLevel.LOG_LEVEL_ERROR);
		}

        private static void LogPrintf(string yv_function, string yv_msg, int loglevel = (int)logLevel.LOG_LEVEL_DEBUG)
		{
            if (s_logLevel == (int)logLevel.LOG_LEVEL_OFF)
			{
				return;
			}
            if (loglevel <= s_logLevel) 
			{
                if (loglevel == (int)logLevel.LOG_LEVEL_DEBUG)
				{
                    //Debug.Log(string.Format("###CloudVoiceLogPrint [Debug]###{0},logMsg:{1}", yv_function, yv_msg));
				}
                if (loglevel == (int)logLevel.LOG_LEVEL_INFO)
				{
                    //Debug.Log(string.Format("###CloudVoiceLogPrint [Info]###{0},logMsg:{1}", yv_function, yv_msg));
				}
                if (loglevel == (int)logLevel.LOG_LEVEL_ERROR)
				{
                    //Debug.Log(string.Format("###CloudVoiceLogPrint [Error]###{0},logMsg:{1}", yv_function, yv_msg));
				}
			}
		}
	}
    public enum logLevel
    {
        LOG_LEVEL_OFF = 0,  //0：关闭日志
        LOG_LEVEL_DEBUG = 1, //1：Debug默认该级别
        LOG_LEVEL_INFO = 2,   //2:info
        LOG_LEVEL_ERROR = 3   //3:error
    }
}
