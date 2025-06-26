#define _XDSDK_LINK_NATIVE_

using UnityEngine;
using System.Collections;
using System;
using RO;
using xdsdk;
using System.Collections.Generic;

[SLua.CustomLuaClassAttribute]
public class FunctionSDK : Singleton<FunctionSDK>
{
	[SLua.CustomLuaClassAttribute]
	public enum E_SDKType
	{
		None,
		XD,
		Any
	}

#if _XDSDK_LINK_NATIVE_
	private E_SDKType m_currentType = E_SDKType.XD;
#else
	private E_SDKType m_currentType = E_SDKType.None;
#endif
	public E_SDKType CurrentType
	{
		get
		{
			return m_currentType;
		}
	}

	public bool IsInitialized = false;

	public static FunctionSDK Instance
	{
		get
		{
			return ins;
		}
	}

    public void XDSDKInitialize(string app_id, string app_key, string secret_key, int orientation, XDSDKCallback on_initialize_success, XDSDKCallback on_initialize_fail)
	{
		RO.LoggerUnused.Log("FunctionSDK:Initialize");
        FunctionXDSDK.ins.Initialize(app_id, app_key, secret_key, orientation,on_initialize_success,on_initialize_fail);
	}

    public void AnySDKInitialize(string app_key, string app_secret, string private_key, string oauth_login_server, XDSDKCallback on_initialize_success, XDSDKCallback on_initialize_fail)
	{
        
	}

    public void Login(XDSDKCallback on_login_success, XDSDKCallback on_login_fail, XDSDKCallback on_login_cancel)
	{
		RO.LoggerUnused.Log("FunctionSDK:Login");
        FunctionXDSDK.ins.Login(on_login_success, on_login_fail, on_login_cancel);
	}

    public void AnySDKLogin(string server_id, XDSDKCallback on_login_success, XDSDKCallback on_login_fail, XDSDKCallback on_login_cancel)
	{
		if (m_currentType == E_SDKType.Any)
		{
			FunctionAnySDK.ins.Login(server_id, (x) => {
				if (on_login_success != null) on_login_success(x);
			}, (x) => {
				if (on_login_fail != null) on_login_fail(x);
			}, (x) => {
				if (on_login_cancel != null) on_login_cancel(x);
			});
		}
	}

    public void AnySDKLogin(string server_id, string oauth_login_server, XDSDKCallback on_login_success, XDSDKCallback on_login_fail, XDSDKCallback on_login_cancel)
	{
		if (m_currentType == E_SDKType.Any)
		{
			FunctionAnySDK.ins.Login(server_id, oauth_login_server, (x) => {
				if (on_login_success != null) on_login_success(x);
			}, (x) => {
				if (on_login_fail != null) on_login_fail(x);
			}, (x) => {
				if (on_login_cancel != null) on_login_cancel(x);
			});
		}
	}


	public string GetAccessToken()
	{
		return XDSDK.GetAccessToken();
	}

	public void EnterPlatform()
	{
        Debug.Log("@@@EnterPlatform");
        XDSDK.OpenUserCenter();
	}

	public bool IsLogined()
	{
		return XDSDK.IsLoggedIn();
	}

    public void ListenLogout(XDSDKCallback on_logout_success, XDSDKCallback on_logout_fail)
	{
		FunctionXDSDK.ins.ROXDSDKHandlerCallBack._OnLogoutSucceed = on_logout_success;
		FunctionXDSDK.ins.ROXDSDKHandlerCallBack._OnLoginFailed = on_logout_fail;
	}

	public void XDSDKPay(int price,
	                     string s_id,
	                     string product_id,
	                     string product_name,
	                     string role_id,
	                     string ext,
	                     int product_count,
	                     string order_id,
	                     XDSDKCallback on_pay_success,
                         XDSDKCallback on_pay_fail,
                         XDSDKCallback on_pay_timeout,
                         XDSDKCallback on_pay_cancel,
                         XDSDKCallback on_pay_product_illegal)
	{

        FunctionXDSDK.ins.ROXDSDKHandlerCallBack._OnPayCompleted = on_pay_success;
        FunctionXDSDK.ins.ROXDSDKHandlerCallBack._OnPayCanceled = on_pay_cancel;
        FunctionXDSDK.ins.ROXDSDKHandlerCallBack._OnPayFailed = on_pay_fail;

            Dictionary<string, string> info = new Dictionary<string, string>();
            info.Add("OrderId", order_id);
            info.Add("Product_Price", ""+price);
            info.Add("EXT", ext);
            info.Add("Sid", s_id);
            info.Add("Role_Id", role_id);
            info.Add("Product_Id", product_id);
            info.Add("Product_Name", product_name);
            XDSDK.Pay(info);

	}

	public void AnySDKPay(string p_id,
	                      string p_name,
	                      float p_price,
	                      int p_count,
	                      string role_id,
	                      string role_name,
	                      int role_grade,
	                      int role_balance,
	                      string server_id,
	                      string custom_string,
                          XDSDKCallback on_success,
                          XDSDKCallback on_fail,
                          XDSDKCallback on_timeout,
                          XDSDKCallback on_cancel,
                          XDSDKCallback on_product_illegal,
                          XDSDKCallback on_paying)
	{

		
	}

	public string GetChannelID()
	{
        return "";
	}

	public void HideWechat()
	{
        XDSDK.HideWX();
	}

	public string GetOrderID()
	{
        return FunctionXDSDK.ins.GetOrderID();
	}

	public void ResetPayState()
	{

	}
}
