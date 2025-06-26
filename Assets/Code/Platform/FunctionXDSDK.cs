using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using xdsdk;


public delegate void XDSDKCallback(string message);

[SLua.CustomLuaClassAttribute]
public class FunctionXDSDK : MonoSingleton<FunctionXDSDK>
{
	public static FunctionXDSDK Ins
	{
		get {
			return ins;
		}
	}

	// iOS
	private bool m_isLaunching;
	private bool m_isInitialized = false;

    public ROXDSDKHandler ROXDSDKHandlerCallBack
    {
        get
        {
            if(_ROXDSDKHandlerCallBack == null)
            {
                _ROXDSDKHandlerCallBack = new ROXDSDKHandler();
            }
            return _ROXDSDKHandlerCallBack;
        }

    }
    private ROXDSDKHandler _ROXDSDKHandlerCallBack;

	public bool IsInitialized
	{
		get
		{
			return m_isInitialized;
		}
	}

	public void Initialize(string app_id, string app_key, string secret_key, int orientation, XDSDKCallback on_launch_success, XDSDKCallback on_launch_fail)
	{
        gameObject.AddComponent<XDSDKListener>();
        XDSDK.SetCallback(ROXDSDKHandlerCallBack);
        XDSDK.InitSDK(app_id, orientation, "", "", false);
        ROXDSDKHandlerCallBack._OnInitSucceed = on_launch_success;
        ROXDSDKHandlerCallBack._OnInitFailed = on_launch_fail;
	}

	// code version 25
	public void InitializeTypeTwo(string app_id, string app_key, string secret_key, int orientation,bool enableTapdb, XDSDKCallback on_launch_success, XDSDKCallback on_launch_fail)
	{
		gameObject.AddComponent<XDSDKListener>();
		XDSDK.SetCallback(ROXDSDKHandlerCallBack);
		XDSDK.InitSDK(app_id, orientation, "", "", enableTapdb);
		ROXDSDKHandlerCallBack._OnInitSucceed = on_launch_success;
		ROXDSDKHandlerCallBack._OnInitFailed = on_launch_fail;
	}

	public void SetCustomLogin()
	{

	}

    public void Login(XDSDKCallback on_login_success, XDSDKCallback on_login_fail, XDSDKCallback on_login_cancel)
    {
		XDSDK.SetLoginEntries (new string[]{"WX_LOGIN","XD_LOGIN","GUEST_LOGIN","QQ_LOGIN"});
        ROXDSDKHandlerCallBack._OnLoginSucceed = on_login_success;
        ROXDSDKHandlerCallBack._OnLoginFailed = on_login_fail;
        ROXDSDKHandlerCallBack._OnLoginCanceled = on_login_cancel;
        XDSDK.Login();
    }


	public void LoginWithWeiXin(XDSDKCallback on_login_success, XDSDKCallback on_login_fail)
	{

	}

	// android only success
	public void Logout(XDSDKCallback on_logout_success, XDSDKCallback on_logout_fail)
	{
        ROXDSDKHandlerCallBack._OnLoginSucceed = on_logout_success;
        ROXDSDKHandlerCallBack._OnLoginFailed = on_logout_fail;
        XDSDK.Logout();
	}

	private bool m_bIsListeningLogout;
	public void ListenLogout(XDSDKCallback on_logout_success, XDSDKCallback on_logout_fail)
	{

	}

	public void ListenGuestBind(XDSDKCallback on_guest_bind_success)
	{
        
	}

	public void EnterPlatform()
	{
        Debug.Log("OpenUserCenter");
        XDSDK.OpenUserCenter();
	}
	// android
	public void OpenUserCenter()
	{
        Debug.Log("OpenUserCenter");
		XDSDK.OpenUserCenter ();
	}

	public bool IsInstalledWeiXin()
	{
        return false;
	}

	public string GetUserID()
	{
        return "";
	}
	
	public string GetAccessToken()
	{
        return XDSDK.GetAccessToken();
	}
	
	public bool IsLogined()
	{
        return XDSDK.IsLoggedIn();
	}

	// iOS
	public void Pay(int price,
	                string s_id,
	                string product_id,
	                string product_name,
	                string role_id,
	                string ext,
	                int product_count,
					string order_id,
	                XDSDKCallback on_pay_success,
	                XDSDKCallback on_pay_fail,
	                XDSDKCallback on_pay_cancel,
	                XDSDKCallback on_pay_timeout,
	                XDSDKCallback on_pay_product_illegal)
	{
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

	// android
	public bool Pay(
		string product_name,
		string product_id,
		string product_price,
		string s_id,
		string role_id,
		string order_id,
		string ext,
		XDSDKCallback on_pay_complete,
		XDSDKCallback on_pay_fail,
		XDSDKCallback on_pay_cancel)
	{
		if (Application.platform == RuntimePlatform.Android) {
			Dictionary<string, string> info = new Dictionary<string,string>();
			info.Add("OrderId", order_id);
			info.Add("Product_Price", product_price);
			info.Add("EXT", ext);
			info.Add("Sid", s_id);
			info.Add("Role_Id", role_id);
			info.Add("Product_Id", product_id);
			info.Add("Product_Name", product_name);
			return xdsdk.XDSDK.Pay (info);
		}
		return false;
	}

	public void HideWechat()
	{
		XDSDK.HideWX ();
	}

	public string GetOrderID()
	{
        return "";
	}

    public void Log(string str)
    {
        
    }

	public void HideGuest(bool b)
	{
        XDSDK.HideGuest ();
	}

	public void HideQQ()
	{
        XDSDK.HideQQ ();
	}

	public void ShowVC()
	{
		XDSDK.ShowVC ();
	}

	public void SetQQWeb()
	{
        XDSDK.SetQQWeb ();
	}

	public void SetWXWeb()
	{
		XDSDK.SetWXWeb ();
	}

	public void Exit()
	{
		XDSDK.Exit ();
	}

	public void OpenRealName(XDSDKCallback on_real_name_success, XDSDKCallback on_real_name_fail)
	{

        ROXDSDKHandlerCallBack._OnRealNameSucceed = on_real_name_success;
        ROXDSDKHandlerCallBack._OnRealNameFailed = on_real_name_fail;
        XDSDK.OpenRealName ();
	}

	public void ListenRealName(XDSDKCallback on_real_name_success, XDSDKCallback on_real_name_fail)
	{

	}

	public string m_purchaseFromAppStore;
	public void ClearPurchaseFromAppStore()
	{
		m_purchaseFromAppStore = null;
	}
	public XDSDKCallback m_callbackAppStorePurchase;
	public void PurchaseFromAppStore(string purchaseFromAppStore)
	{
		m_purchaseFromAppStore = purchaseFromAppStore;
		if (m_callbackAppStorePurchase != null) {
			m_callbackAppStorePurchase (m_purchaseFromAppStore);
		}
	}
	public void SetCallbackAppStorePurchase(XDSDKCallback callback)
	{
		m_callbackAppStorePurchase = callback;
		if (!string.IsNullOrEmpty (m_purchaseFromAppStore)&&m_callbackAppStorePurchase!=null) {
			m_callbackAppStorePurchase (m_purchaseFromAppStore);
		}
	}
}

public class ROXDSDKHandler : xdsdk.XDCallback
{

    public XDSDKCallback _OnInitSucceed;
    public XDSDKCallback _OnInitFailed;
    public XDSDKCallback _OnLoginSucceed;
    public XDSDKCallback _OnLoginFailed;
    public XDSDKCallback _OnLoginCanceled;
    public XDSDKCallback _OnGuestBindSucceed;
    public XDSDKCallback _OnLogoutSucceed;
    public XDSDKCallback _OnPayCompleted;
    public XDSDKCallback _OnPayFailed;
    public XDSDKCallback _OnPayCanceled;
    public XDSDKCallback _OnExitConfirm;
    public XDSDKCallback _OnExitCancel;
    public XDSDKCallback _OnWXShareSucceed;
    public XDSDKCallback _OnWXShareFailed;
    public XDSDKCallback _OnRealNameSucceed;
    public XDSDKCallback _OnRealNameFailed;
 

    public override void OnInitSucceed()
    {
       
		if (_OnInitSucceed != null) {
			FunctionSDK.ins.IsInitialized = true;
			_OnInitSucceed ("");
		} else {
			Debug.Log("ROXDSDKHandler _OnInitSucceed == null");
		}
    }

    public override void OnInitFailed(string msg)
    {
		if (_OnInitFailed != null) {
			_OnInitFailed (msg);
		} else {
			Debug.Log("ROXDSDKHandler _OnInitFailed == null");
		}
    }

    public override void OnLoginSucceed(string token)
    {
		if (_OnLoginSucceed != null) {
			_OnLoginSucceed (token);
		} else {
			Debug.Log("ROXDSDKHandler OnLoginSucceed == null");
		}
    }


    public override void OnLoginFailed(string msg)
    {
		if (_OnLoginFailed != null) {
			_OnLoginFailed (msg);
		} else {
			Debug.Log("ROXDSDKHandler _OnLoginFailed == null");
		}
    }

    public override void OnLoginCanceled()
    {
		if (_OnLoginCanceled != null) {
			_OnLoginCanceled ("");
		} else {
			Debug.Log("ROXDSDKHandler _OnLoginCanceled == null");
		}
    }

    public override void OnGuestBindSucceed(string token)
    {
		if (_OnGuestBindSucceed != null) {
			_OnGuestBindSucceed (token);
		} else {
			Debug.Log("ROXDSDKHandler _OnGuestBindSucceed == null");
		}
    }

    public override void OnLogoutSucceed()
    {
		if (_OnLogoutSucceed != null) {
			_OnLogoutSucceed ("");
		} else {
			Debug.Log("ROXDSDKHandler _OnLogoutSucceed == null");
		}
    }

    public override void OnPayCompleted()
    {
		if (_OnPayCompleted != null) {
			_OnPayCompleted ("");
		} else {
			Debug.Log("ROXDSDKHandler _OnPayCompleted == null");
		}
    }

    public override void OnPayFailed(string msg)
    {
		if (_OnPayFailed != null) {
			_OnPayFailed (msg);
		} else {
			Debug.Log("ROXDSDKHandler _OnPayFailed == null");
		}

    }

    public override void OnPayCanceled()
    {
		if (_OnPayCanceled != null) {
			_OnPayCanceled ("");
		} else {
			Debug.Log("ROXDSDKHandler _OnPayCanceled == null");
		}
    }

    public override void OnExitConfirm()
    {
		if (_OnExitConfirm != null) {

			_OnExitConfirm ("");
		} else {
			Debug.Log("ROXDSDKHandler _OnExitConfirm == null");
		}
    }

    public override void OnExitCancel()
    {
		if (_OnExitCancel != null) {
			_OnExitCancel ("");
		} else {
			Debug.Log("ROXDSDKHandler _OnExitCancel == null");
		}
    }

    public override void OnWXShareSucceed()
    {
		if (_OnWXShareSucceed != null) {
			_OnWXShareSucceed ("");
		} else {
			Debug.Log("ROXDSDKHandler _OnWXShareSucceed == null");
		}
    }

    public override void OnWXShareFailed(string msg)
    {
		if (_OnWXShareFailed != null) {
			_OnWXShareFailed (msg);
		} else {
			Debug.Log("ROXDSDKHandler _OnWXShareFailed == null");
		}
    }

    public override void OnRealNameSucceed()
    {
		if (_OnRealNameSucceed != null) {
			_OnRealNameSucceed ("");
		} else {
			Debug.Log("ROXDSDKHandler _OnRealNameSucceed == null");
		}
    }


    public override void OnRealNameFailed(string msg)
    {
		if (_OnRealNameFailed != null) {
			_OnRealNameFailed (msg);
		} else {
			Debug.Log("ROXDSDKHandler _OnRealNameFailed == null");
		}

    }
}
