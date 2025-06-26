using UnityEngine;
using System.Collections;
using RO;

public class XDSDKEventListener : MonoSingleton<XDSDKEventListener>
{
	public bool m_bInitializeSuccess;
	public string m_strInitializeSuccessMsg;
	public void OnReveiveInitializeSuccess(string message)
	{
		RO.LoggerUnused.Log("XDSDKEventListener:OnReveiveInitializeSuccess");
		m_bInitializeSuccess = true;
		m_strInitializeSuccessMsg = message;
	}
	public bool m_bInitializeFail;
	public string m_strInitializeFailMsg;
	public void OnReceiveInitializeFail(string message)
	{
		RO.LoggerUnused.Log("XDSDKEventListener:OnReceiveInitializeFail");
		m_bInitializeFail = true;
		m_strInitializeFailMsg = message;
	}
	public bool m_bLoginSuccess;
	public string m_strLoginSuccessMsg;
	public void OnReceiveLoginSuccess(string message)
	{
		RO.LoggerUnused.Log("XDSDKEventListener:OnReceiveLoginSuccess");
		m_bLoginSuccess = true;
		m_strLoginSuccessMsg = message;
	}
	public bool m_bLoginTimeout;
	public string m_strLoginTimeoutMsg;
	public void OnReceiveLoginTimeout(string message)
	{
		RO.LoggerUnused.Log("XDSDKEventListener:OnReceiveLoginTimeout");
		m_bLoginTimeout = true;
		m_strLoginTimeoutMsg = message;
	}
	public bool m_bLoginNoNeed;
	public string m_strLoginNoNeedMsg;
	public void OnReceiveLoginNoNeed(string message)
	{
		RO.LoggerUnused.Log("XDSDKEventListener:OnReceiveLoginNoNeed");
		m_bLoginNoNeed = true;
		m_strLoginNoNeedMsg = message;
	}
	public bool m_bLoginCancel;
	public string m_strLoginCancelMsg;
	public void OnReceiveLoginCancel(string message)
	{
		RO.LoggerUnused.Log("XDSDKEventListener:OnReceiveLoginCancel");
		m_bLoginCancel = true;
		m_strLoginCancelMsg = message;
	}
	public bool m_bLoginFail;
	public string m_strLoginFailMsg;
	public void OnReceiveLoginFail(string message)
	{
		RO.LoggerUnused.Log("XDSDKEventListener:OnReceiveLoginFail");
		m_bLoginFail = true;
		m_strLoginFailMsg = message;
	}
	public bool m_bLogoutSuccess;
	public string m_strLogoutSuccessMsg;
	public void OnReceiveLogoutSuccess(string message)
	{
		RO.LoggerUnused.Log("XDSDKEventListener:OnReceiveLogoutSuccess");
		m_bLogoutSuccess = true;
		m_strLogoutSuccessMsg = message;
	}
	public bool m_bLogoutFail;
	public string m_strLogoutFailMsg;
	public void OnReceiveLogoutFail(string message)
	{
		RO.LoggerUnused.Log("XDSDKEventListener:OnReceiveLogoutFail");
		m_bLogoutFail = true;
		m_strLogoutFailMsg = message;
	}
	public bool m_bPaySuccess;
	public string m_strPaySuccessMsg;
	public void OnReceivePaySuccess(string message)
	{
		Debug.Log("XDSDKEventListener:OnReceivePaySuccess");
		m_bPaySuccess = true;
		m_strPaySuccessMsg = message;
	}
	public bool m_bPayFail;
	public string m_strPayFailMsg;
	public void OnReceivePayFail(string message)
	{
        Debug.Log("XDSDKEventListener:OnReceivePayFail");
		m_bPayFail = true;
		m_strPayFailMsg = message;
	}
	public bool m_bPayCancel;
	public string m_strPayCancelMsg;
	public void OnReceivePayCancel(string message)
	{
        Debug.Log("XDSDKEventListener:OnReceivePayCancel");
		m_bPayCancel = true;
		m_strPayCancelMsg = message;
	}
	public bool m_bPayTimeout;
	public string m_strPayTimeoutMsg;
	public void OnReceivePayTimeout(string message)
	{
        Debug.Log("XDSDKEventListener:OnReceivePayTimeout");
		m_bPayTimeout = true;
		m_strPayTimeoutMsg = message;
	}
	public bool m_bPayProductIllegal;
	public string m_strPayProductIllegalMsg;
	public void OnReceivePayProductIllegal(string message)
	{
        Debug.Log("XDSDKEventListener:OnReceivePayProductIllegal");
		m_bPayProductIllegal = true;
		m_strPayProductIllegalMsg = message;
	}
	public bool m_bRealNameSuccess;
	public string m_strRealNameSuccessMsg;
	public void OnReceiveRealNameSuccess(string message)
	{
        Debug.Log ("XDSDKEventListener:OnReceiveRealNameSuccess");
		m_bRealNameSuccess = true;
		m_strRealNameSuccessMsg = message;
	}
	public bool m_bRealNameFail;
	public string m_strRealNameFailMsg;
	public void OnReceiveRealNameFail(string message)
	{
        Debug.Log ("XDSDKEventListener:OnReceiveRealNameFail");
		m_bRealNameFail = true;
		m_strRealNameFailMsg = message;
	}
	public void OnReceivePurchaseFromAppStore(string message)
	{
        Debug.Log ("XDSDKEventListener:OnReceivePurchaseFromAppStore");
		FunctionXDSDK.Ins.PurchaseFromAppStore(message);
	}
}
