using System;
using UnityEngine;

public class PayPalLogger
{
    public static bool _ENABLED = true;

    public static void Log(string debugString)
    {
        if (_ENABLED)
        {
            Debug.Log("<b>PayPal API: </b>" + debugString);
        }
        
    }

    public static void LogFormat(string format, object[] args)
    {
        if (_ENABLED)
        {
            Debug.LogFormat("<b>PayPal API: </b>" + format, args);
        }      
    }


    public static void LogError(string debugString)
    {
        if (_ENABLED)
        {
            Debug.LogError("<b>PayPal API: </b>" + debugString);
        }

    }

    public static void LogErrorFormat(string format, object[] args)
    {
        if (_ENABLED)
        {
            Debug.LogErrorFormat("<b>PayPal API: </b>" + format, args);
        }
    }

}
