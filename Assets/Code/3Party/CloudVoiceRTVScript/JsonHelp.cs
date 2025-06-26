using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using CloudVoiceVideoTroops;
using LitJson;


namespace CloudVoiceVideoTroops
{
public class JsonHelp {
		public static string getStringValue(JsonData data, string keyName)
		{
			if (null == data || null == keyName) {
				return "";			
			}

			string value = "";
			try{
				value = (string)data[keyName];
			}
			catch(Exception e)
			{
				//Debug.Log("Exception = "+ e.ToString());
				Debug.Log("keyName = " + keyName +", Exception = "+ e.ToString());
			}
			finally
			{
				//Debug.Log("finally = ================");
			}
			
			return value;
		}
		
		public static bool getBoolValue(JsonData data, string keyName)
		{
			if (null == data || null == keyName) {
				return false;			
			}

			bool value = false;
			int valueInt = 0;
			try{
				value = (bool)data[keyName];
			}
			catch(Exception e)
			{
				Debug.Log("keyName = " + keyName +", Exception = "+ e.ToString());
				
				int iValue  = (int)data[keyName];
				if(0 == iValue)
				{
					value = false;
				}
				else
				{
					value = true;
				}
				
			}
			finally
			{
				//Debug.Log("finally = ================");
			}
			
			return value;
		}


		public static int getIntValue(JsonData data, string keyName)
		{
			int value = 0;

			try{
				value = (int)data[keyName];
			}
			catch(Exception e)
			{
				//Debug.Log("Exception = "+ e.ToString());
			}
			finally
			{
				//Debug.Log("finally = ================");
			}

			return value;
		}
        public static double getDoubleValue(JsonData data, string keyName)
        {
            if (null == data || null == keyName)
            {
                return 0.0d;
            }

            double value = 0.0d;
            try
            {
                value = (double)data[keyName];
            }
            catch (Exception e)
            {
                //Debug.Log("Exception = "+ e.ToString());
                Debug.Log("keyName = " + keyName + ", Exception = " + e.ToString());
            }
            finally
            {
                //Debug.Log("finally = ================");
            }

            return value;
        }
		/*
		public static Int16 getInt16Value(JsonData data, string keyName)
		{
			Int16 value = 0;
			
			try{
				value = (Int16)data[keyName];
			}
			catch(Exception e)
			{
				//Debug.Log("Exception = "+ e.ToString());
			}
			finally
			{
				//Debug.Log("finally = ================");
			}
			
			return value;
		}

		public static Int32 getInt32Value(JsonData data, string keyName)
		{
			Int32 value = 0;
			
			try{
				value = (Int32)data[keyName];
			}
			catch(Exception e)
			{
				//Debug.Log("Exception = "+ e.ToString());
			}
			finally
			{
				//Debug.Log("finally = ================");
			}
			
			return value;
		}

		public static Int64 getInt64Value(JsonData data, string keyName)
		{
			Int64 value = 0;
			
			try{
				value = (Int64)data[keyName];
			}
			catch(Exception e)
			{
				//Debug.Log("Exception = "+ e.ToString());
			}
			finally
			{
				//Debug.Log("finally = ================");
			}
			
			return value;
		}
		*/

 }
}