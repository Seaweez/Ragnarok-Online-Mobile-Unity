//#define _TYRANTDB_LINK_NATIVE_

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public class Tyrantdb
{
#if UNITY_ANDROID && !UNITY_EDITOR
	private static AndroidJavaObject m_currentActivity;
	public static AndroidJavaObject CurrentActivity
	{
		get
		{
			if (m_currentActivity == null)
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				m_currentActivity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			}
			return m_currentActivity;
		}
	}

	//private AndroidJavaClass m_cTyrantdbGameTracker = new AndroidJavaClass("com.xindong.tyrantdb.TyrantdbGameTracker");
	private static AndroidJavaObject m_androidJavaObject = new AndroidJavaObject("com.xindong.tyrantdb.TyrantdbGameTracker");
#endif

#if _TYRANTDB_LINK_NATIVE_ || _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	extern private static void _InitializeForTyrantdb(string app_id, string channel, string version);
#else
	private static void _InitializeForTyrantdb(string app_id, string channel, string version){}
#endif
	public static void Initialize(string app_id, string channel, string version, bool request_permission)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			m_androidJavaObject.CallStatic("init", CurrentActivity, app_id, channel, version, request_permission);
#endif
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_InitializeForTyrantdb(app_id, channel, version);
		}
	}
	
	public static void Resume()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			m_androidJavaObject.CallStatic("onResume", CurrentActivity);
#endif
		}
	}
	
	public static void Stop()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			m_androidJavaObject.CallStatic("onStop", CurrentActivity);
#endif
		}
	}
	
#if _TYRANTDB_LINK_NATIVE_ || _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	extern private static void _SetUserForTyrantdb(string user_id, int user_type, int user_sex, int user_age, string user_name);
#else
	private static void _SetUserForTyrantdb(string user_id, int user_type, int user_sex, int user_age, string user_name){}
#endif
	public static void SetUser(string user_id, int user_type, int user_sex, int user_age, string user_name)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			m_androidJavaObject.CallStatic("setUserDeprecated", user_id, user_type, user_sex, user_age, user_name);
#endif
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_SetUserForTyrantdb(user_id, user_type, user_sex, user_age, user_name);
		}
	}

#if _TYRANTDB_LINK_NATIVE_ || _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	extern private static void _SetLevelForTyrantdb(int level);
#else
	private static void _SetLevelForTyrantdb(int level){}
#endif
	public static void SetLevel(int level)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			m_androidJavaObject.CallStatic("setLevel", level);
#endif
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_SetLevelForTyrantdb(level);
		}
	}

#if _TYRANTDB_LINK_NATIVE_ || _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	extern private static void _SetServerForTyrantdb(string server_id);
#else
	private static void _SetServerForTyrantdb(string server_id){}
#endif
	public static void SetServer(string server_id)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			m_androidJavaObject.CallStatic("setServer", server_id);
#endif
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_SetServerForTyrantdb(server_id);
		}
	}

#if _TYRANTDB_LINK_NATIVE_ || _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	extern private static void _OnChargeRequestForTyrantdb(string order_id, string product_name, int amount, string currency_type, int virtual_currency_amount, string pay_way);
#else
	private static void _OnChargeRequestForTyrantdb(string order_id, string product_name, int amount, string currency_type, int virtual_currency_amount, string pay_way){}
#endif
	public static void Charge(string order_id, string product_name, int amount, string currency_type, int virtual_currency_amount, string pay_way)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			m_androidJavaObject.CallStatic("onChargeRequest", order_id, product_name, amount, currency_type, virtual_currency_amount, pay_way);
#endif
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_OnChargeRequestForTyrantdb(order_id, product_name, amount, currency_type, virtual_currency_amount, pay_way);
		}
	}

#if _TYRANTDB_LINK_NATIVE_ || _XDSDK_LINK_NATIVE_	
	[DllImport("__Internal")]
	extern private static void _OnChargeSuccessForTyrantdb(string order_id);
#else
	private static void _OnChargeSuccessForTyrantdb(string order_id){}
#endif
	public static void ChargeSuccess(string order_id)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			m_androidJavaObject.CallStatic("onChargeSuccess", order_id);
#endif
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_OnChargeSuccessForTyrantdb(order_id);
		}
	}

#if _TYRANTDB_LINK_NATIVE_ || _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	extern private static void _OnChargeFailForTyrantdb(string order_id, string reason);
#else
	private static void _OnChargeFailForTyrantdb(string order_id, string reason){}
#endif
	public static void ChargeFail(string order_id, string fail_reason)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			m_androidJavaObject.CallStatic("onChargeFail", order_id, fail_reason);
#endif
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_OnChargeFailForTyrantdb(order_id, fail_reason);
		}
	}

#if _TYRANTDB_LINK_NATIVE_ || _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	extern private static void _OnChargeOnlySuccessForTyrantdb(string order_id, string product_name, int amount, string currency_type, int virtual_currency_amount, string payment);
#else
	private static void _OnChargeOnlySuccessForTyrantdb(string order_id, string product_name, int amount, string currency_type, int virtual_currency_amount, string payment){}
#endif
	public static void ChargeSuccess(string order_id, string product_name, int amount, string currency_type, int virtual_currency_amount, string pay_way)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			m_androidJavaObject.CallStatic("onChargeOnlySuccess", order_id, product_name, amount, currency_type, virtual_currency_amount, pay_way);
#endif
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_OnChargeOnlySuccessForTyrantdb(order_id, product_name, amount, currency_type, virtual_currency_amount, pay_way);
		}
	}

#if _TYRANTDB_LINK_NATIVE_ || _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	extern private static void _OnCreateRole(string str);
#else
	private static void _OnCreateRole(string str){}
#endif
	public static void OnCreateRole(string str)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_OnCreateRole (str);
		}
	}

#if _TYRANTDB_LINK_NATIVE_ || _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	extern private static void _ChargeTo3rd(string order_id, int amount, string currency_type, string payment);
#else
	private static void _ChargeTo3rd(string order_id, int amount, string currency_type, string payment){}
#endif
	public static void ChargeTo3rd(string order_id, int amount, string currency_type, string payment)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			m_androidJavaObject.CallStatic("chargeTo3rd", order_id, (long)amount, currency_type, payment);
#endif
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_ChargeTo3rd(order_id, amount, currency_type, payment);
		}
	}

	#if _TYRANTDB_LINK_NATIVE_ || _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	extern private static string _GetAppVersion();
	#else
	private static string _GetAppVersion()
	{
		return "";
	}
	#endif
	public static string GetAppVersion()
	{
		string retValue = "";
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			retValue = _GetAppVersion();
		}
		return retValue;
	}
}
