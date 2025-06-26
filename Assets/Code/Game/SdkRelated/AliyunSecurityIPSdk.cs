using UnityEngine;
using System.Net;
using System.Threading;
using System;
using System.Runtime.InteropServices;
using RO;

[SLua.CustomLuaClassAttribute]
public class AliyunSecurityIPSdk {

	private static int hasInitSdk = -1;

	public bool hasFinishGetAliIp = false;
	private Action<string>hander;
	private string ip = null;
	private string groupName;
	private int timeOut = 5;
	private float startTime = 0;
	private float passedTime = 0;

	public bool isOverTime{
		get{
			if(passedTime > timeOut)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}

	public AliyunSecurityIPSdk(Action<string>handler,string groupName,int timeOut)
	{
		this.hander = handler;
		this.timeOut = timeOut;
	}

	public void exeHandler()
	{
		this.hander(this.ip);
	}

	#if UNITY_ANDROID && !UNITY_EDITOR
	
	private static AndroidJavaClass AliJavaObjectAsyc = (AndroidJavaClass)ExternalInterfaces.androidSavePic;
	private static AndroidJavaObject m_androidJavaObject = new AndroidJavaObject("com.aliyun.security.yunceng.android.sdk.YunCeng");

	public static int Android_ALSDK_Init(string appkey)
	{
		if(null == m_androidJavaObject)
		{
			m_androidJavaObject = new AndroidJavaObject("com.aliyun.security.yunceng.android.sdk.YunCeng");
		}

		if(null != m_androidJavaObject)
		{
			return m_androidJavaObject.CallStatic<int>("init",appkey);
		}
		return -1;
	}

	public static string Android_ALSDK_GetNextIPByGroupName(string groupName)
	{
		if(null == m_androidJavaObject)
		{
			m_androidJavaObject = new AndroidJavaObject("com.aliyun.security.yunceng.android.sdk.YunCeng");
		}

		if(null != m_androidJavaObject)
		{
			try
			{				
				var ip = m_androidJavaObject.CallStatic<string>("getNextIpByGroupName",groupName);
				return ip;
			}catch(Exception e)
			{
				ROLogger.Log("Exception:Android_ALSDK_GetNextIPByGroupName!");
			}	
		}
		return "";
	}
	
	public static void Android_ALSDK_GetNextIPByGroupNameAsync(string groupName,string gameObjectName)
	{
		if(null == AliJavaObjectAsyc)
		{
			AliJavaObjectAsyc = new AndroidJavaClass ("com.xindong.RODevelop.UnitySavePicActivity");
		}

		if(null != AliJavaObjectAsyc)
		{
			AliJavaObjectAsyc.CallStatic("getNextIpByGroupName", groupName,gameObjectName);
		}
	}
	#else

	const string LUADLL = "__Internal";
	[DllImport(LUADLL)]
	private static extern string ALSDK_GetNextIPByGroupName(string gName); 
	[DllImport(LUADLL)]
	private static extern int ALSDK_init(string appkey);
	[DllImport(LUADLL)]
	private static extern string GetLanguage();

	#endif
	
	// Use this for initialization
	public static int initALSDK(string appkey)
	{
		if(hasInitSdk!=0)
		{
			#if UNITY_ANDROID && !UNITY_EDITOR
			hasInitSdk = Android_ALSDK_Init(appkey);
			#elif UNITY_IPHONE && !UNITY_EDITOR
			hasInitSdk = ALSDK_init(appkey);
			#endif
		}
		return hasInitSdk;
	}
	
	// Update is called once per frame
	public void getIpByGroupNameAsync() 
	{
		startTime =(int) Time.realtimeSinceStartup;
		#if UNITY_ANDROID && !UNITY_EDITOR
		ip = Android_ALSDK_GetNextIPByGroupName(groupName);
		#elif UNITY_IPHONE && !UNITY_EDITOR 
		ip = ALSDK_GetNextIPByGroupName(groupName);
		#endif
		hasFinishGetAliIp = true;
	}

	public void updatePassedTime()
	{
		var now = Time.realtimeSinceStartup;
		this.passedTime = now - startTime;
	}

	public static string getIpByGroupNameSync(string groupName) 
	{	
		string ip = null;
		#if UNITY_ANDROID && !UNITY_EDITOR
		ip = Android_ALSDK_GetNextIPByGroupName(groupName);
		#elif UNITY_IPHONE && !UNITY_EDITOR 
		ip = ALSDK_GetNextIPByGroupName(groupName);
		#endif
		return ip;
	}

	public static SystemLanguage getLanguage() 
	{	
//		string ip = null;
		SystemLanguage language = Application.systemLanguage;

		#if UNITY_IPHONE && !UNITY_EDITOR
		if (language == SystemLanguage.Thai)
		{
			string name = GetLanguage();
			if (name.StartsWith("th"))
			{
				language = SystemLanguage.Thai;
			}
			else
			{
				language = SystemLanguage.Thai;
			}
		}

		if (language == SystemLanguage.Chinese) {  
				string name = GetLanguage();  
				if (name.StartsWith("zh-Hans")) {  
					language = SystemLanguage.ChineseSimplified;  
				} 
				else{
					language = SystemLanguage.ChineseTraditional;  
				}			
			}
		#endif
		
		return language;
	}
}
