//#define _XDSDK_LINK_NATIVE_

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using RO;

public class XDSDKOld : MonoBehaviour
{
#if _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	private static extern void _Start();
#else
	private static void _Start(){}
#endif
	public static void Start()
	{
		RO.LoggerUnused.Log("XDSDK:Start");
		XDSDKEventListener xdSDKEventListener = XDSDKEventListener.ins;
		_Start();
	}

#if _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	private static extern void _SetCallback();
#else
	private static void _SetCallback(){}
#endif
	public static void SetCallback()
	{
		RO.LoggerUnused.Log("XDSDK:SetCallback");
		_SetCallback();
	}

#if _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	private static extern void _Initialize(string app_id, string app_key, string secret_key, int orientation);
#else
	private static void _Initialize(string app_id, string app_key, string secret_key, int orientation){}
#endif
	public static void Initialize(string app_id, string app_key, string secret_key, int orientation)
	{
		RO.LoggerUnused.Log("XDSDK:Initialize");
		_Initialize(app_id, app_key, secret_key, orientation);
	}

#if _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	private static extern void _SetCustomLogin();
#else
	private static void _SetCustomLogin(){}
#endif
	public static void SetCustomLogin()
	{
		RO.LoggerUnused.Log("XDSDK:SetCustomLogin");
		_SetCustomLogin();
	}

#if _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	private static extern void _LoginWithWX();
#else
	private static void _LoginWithWX(){}
#endif
	public static void LoginWithWX()
	{
		RO.LoggerUnused.Log("XDSDK:LoginWithWX");
		_LoginWithWX();
	}

#if _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	private static extern void _Login(string key);
#else
	private static void _Login(string key){}
#endif
	public static void Login(string key)
	{
		RO.LoggerUnused.Log("XDSDK:Login");
		_Login(key);
	}

#if _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	private static extern void _Logout(string key);
#else
	private static void _Logout(string key){}
#endif
	public static void Logout(string key)
	{
		RO.LoggerUnused.Log("XDSDK:Logout");
		_Logout(key);
	}

#if _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	private static extern void _EnterPlatform();
#else
	private static void _EnterPlatform(){}
#endif
	public static void EnterPlatform()
	{
		_EnterPlatform();
	}

#if _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	private static extern void _PayForProductWithParams(int price, string s_id, string product_id, string product_name, string role_id, string ext, int product_count, string order_id);
#else
	private static void _PayForProductWithParams(int price, string s_id, string product_id, string product_name, string role_id, string ext, int product_count, string order_id){}
#endif
	public static void PayForProductWithParams(int price, string s_id, string product_id, string product_name, string role_id, string ext, int product_count, string order_id)
	{
		RO.LoggerUnused.Log("XDSDK:PayForProductWithParams");
		if (ext == null)
		{
			ext = "null";
		}
		RO.LoggerUnused.Log("ext = " + ext);
		_PayForProductWithParams(price, s_id, product_id, product_name, role_id, ext, product_count, order_id);
	}

#if _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	private static extern bool _IsLogined();
#else
	private static bool _IsLogined()
	{
		return true;
	}
#endif
	public static bool IsLogined()
	{
		return _IsLogined();
	}

#if _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	private static extern string _GetUserID();
#else
	private static string _GetUserID()
	{
		return "";
	}
#endif
	public static string GetUserID()
	{
		return _GetUserID();
	}

#if _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	private static extern string _GetAccessToken();
#else
	private static string _GetAccessToken()
	{
		return "";
	}
#endif
	public static string GetAccessToken()
	{
		return _GetAccessToken();
	}

#if _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	private static extern bool _IsInstalledWX();
#else
	private static bool _IsInstalledWX()
	{
		return false;
	}
#endif
	public static bool IsInstalledWX()
	{
		return _IsInstalledWX();
	}

#if _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	private static extern void _HideWechat();
#else
	private static void _HideWechat(){}
#endif
	public static void HideWechat()
	{
		_HideWechat();
	}

#if _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	private static extern string _GetOrderID();
#else
	private static string _GetOrderID()
	{
		return "";
	}
#endif
	public static string GetOrderID()
	{
		return _GetOrderID();
	}

#if _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	private static extern void _Log(string str);
#else
	private static void _Log(string str){}
#endif
	public static void Log(string str)
	{
		_Log(str);
	}

#if _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	private static extern void _HideGuest(bool b);
#else
	private static void _HideGuest(bool b){}
#endif
	public static void HideGuest(bool b)
	{
		_HideGuest(b);
	}

	#if _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	private static extern void _OpenRealName();
	#else
	private static void _OpenRealName(){}
	#endif
	public static void OpenRealName()
	{
		_OpenRealName();
	}

	#if _XDSDK_LINK_NATIVE_
	[DllImport("__Internal")]
	private static extern void _HideTapTap();
	#else
	private static void _HideTapTap(){}
	#endif
	public static void HideTapTap()
	{
		_HideTapTap();
	}
}
