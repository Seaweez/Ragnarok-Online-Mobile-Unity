using UnityEngine;
using System.Collections;
using game;
using System.Collections.Generic;
using System;
using RO;

public class AnySDKEventListener : MonoBehaviour
{
	public static AnySDKEventListener ins
	{
		get
		{
			GameObject go = new GameObject("AnySDKEventListener");
			DontDestroyOnLoad(go);
			return go.AddComponent<AnySDKEventListener>();
		}
	}

	public void UserExternalCall(string msg)
	{
		RO.LoggerUnused.Log("UserExternalCall(" + msg + ")");
		Dictionary<string,string> dic = GameUtil.stringToDictionary(msg);
		int code = Convert.ToInt32(dic["code"]);
		string result = dic["msg"];
		switch(code)
		{
			case (int)UserActionResultCode.kInitSuccess://初始化SDK成功回调
			{
				RO.LoggerUnused.Log("init success, " + result);
				FunctionAnySDK.ins.Callbacks.Call(AnySDKCallbacks.E_CallbackCommand.InitializeSuccess, result);
			}
				break;
			case (int)UserActionResultCode.kInitFail://初始化SDK失败回调
			{
				RO.LoggerUnused.Log("init fail, " + result);
				FunctionAnySDK.ins.Callbacks.Call(AnySDKCallbacks.E_CallbackCommand.InitializeFail, result);
			}
				break;
			case (int)UserActionResultCode.kLoginSuccess://登陆成功回调
			{
				RO.LoggerUnused.Log("login success, " + result);
				FunctionAnySDK.ins.Callbacks.Call(AnySDKCallbacks.E_CallbackCommand.LoginSuccess, result);
			}
				break;
			case (int)UserActionResultCode.kLoginNetworkError://登陆网络出错回调
			{
				RO.LoggerUnused.Log("login network error, " + result);
				FunctionAnySDK.ins.Callbacks.Call(AnySDKCallbacks.E_CallbackCommand.LoginNetError, result);
			}
				break;
			case (int)UserActionResultCode.kLoginCancel://登陆取消回调
			{
				RO.LoggerUnused.Log("login cancel, " + result);
				FunctionAnySDK.ins.Callbacks.Call(AnySDKCallbacks.E_CallbackCommand.LoginCancel, result);
			}
				break;
			case (int)UserActionResultCode.kLoginFail://登陆失败回调
			{
				RO.LoggerUnused.Log("login fail, " + result);
				FunctionAnySDK.ins.Callbacks.Call(AnySDKCallbacks.E_CallbackCommand.LoginFail, result);
			}
				break;
			case (int)UserActionResultCode.kLogoutSuccess://登出成功回调
			{
				Debug.Log ("logout success, " + result);
				RO.LoggerUnused.Log("logout success, " + result);
				FunctionAnySDK.ins.Callbacks.Call(AnySDKCallbacks.E_CallbackCommand.LogoutSuccess, result);
			}
				break;
			case (int)UserActionResultCode.kLogoutFail://登出失败回调
			{
				Debug.Log ("logout fail, " + result);
				RO.LoggerUnused.Log("logout fail, " + result);
				FunctionAnySDK.ins.Callbacks.Call(AnySDKCallbacks.E_CallbackCommand.LogoutFail, result);
			}
				break;
			case (int)UserActionResultCode.kPlatformEnter://平台中心进入回调
				break;
			case (int)UserActionResultCode.kPlatformBack://平台中心退出回调
				break;
			case (int)UserActionResultCode.kPausePage://暂停界面回调
				break;
			case (int)UserActionResultCode.kExitPage://退出游戏回调
				break;
			case (int)UserActionResultCode.kAntiAddictionQuery://防沉迷查询回调
				break;
			case (int)UserActionResultCode.kRealNameRegister://实名注册回调
				break;
			case (int)UserActionResultCode.kAccountSwitchSuccess://切换账号成功回调
				break;
			case (int)UserActionResultCode.kAccountSwitchFail://切换账号成功回调
				break;
			default:
				break;
		}
	}

	public void OnReceivePayResult(string msg)
	{
		RO.LoggerUnused.Log("OnReceivePayResult(" + msg + ")");
		Dictionary<string, string> dict = GameUtil.stringToDictionary(msg);
		int code = Convert.ToInt32(dict["code"]);
		string result = dict["msg"];
		switch(code)
		{
			case (int)PayResultCode.kPaySuccess://支付成功回调
			{
				RO.LoggerUnused.Log("pay success, " + result);
				FunctionAnySDK.ins.Callbacks.Call(AnySDKCallbacks.E_CallbackCommand.PaySuccess, result);
			}
				break;
			case (int)PayResultCode.kPayFail://支付失败回调
			{
				RO.LoggerUnused.Log("pay fail, " + result);
				FunctionAnySDK.ins.Callbacks.Call(AnySDKCallbacks.E_CallbackCommand.PayFail, result);
			}
				break;
			case (int)PayResultCode.kPayCancel://支付取消回调
			{
				RO.LoggerUnused.Log("pay cancel, " + result);
				FunctionAnySDK.ins.Callbacks.Call(AnySDKCallbacks.E_CallbackCommand.PayCancel, result);
			}
				break;
			case (int)PayResultCode.kPayNetworkError://支付超时回调
			{
				RO.LoggerUnused.Log("pay timeout, " + result);
				FunctionAnySDK.ins.Callbacks.Call(AnySDKCallbacks.E_CallbackCommand.PayTimeout, result);
			}
				break;
			case (int)PayResultCode.kPayProductionInforIncomplete://支付信息不完整
			{
				RO.LoggerUnused.Log("pay product illegal, " + result);
				FunctionAnySDK.ins.Callbacks.Call(AnySDKCallbacks.E_CallbackCommand.PayProductIllegal, result);
			}
				break;
			case (int)PayResultCode.kPayNowPaying:
			{
				RO.LoggerUnused.Log("pay paying, " + result);
				FunctionAnySDK.ins.Callbacks.Call(AnySDKCallbacks.E_CallbackCommand.PayPaying, result);
			}
				break;
			default:
				break;
		}
	}
}
