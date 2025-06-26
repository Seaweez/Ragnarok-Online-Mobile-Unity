using UnityEngine;
using System.Collections;
using System;
using LitJson;

public class FunctionAnySDK : Singleton<FunctionAnySDK>
{
//	private static FunctionAnySDK instance;
//	public static FunctionAnySDK Instance
//	{
//		get
//		{
//			if (instance == null)
//				instance = new FunctionAnySDK();
//			return instance;
//		}
//	}

	private AnySDKCallbacks m_anySDKCallbacks;
	public AnySDKCallbacks Callbacks
	{
		get {return m_anySDKCallbacks;}
	}

	public void Launch()
	{
		m_anySDKCallbacks = new AnySDKCallbacks();
	}

	public void Initialize(string app_key, string app_secret, string private_key, string oauth_login_server, AnySDKCallbacks.AnySDKCallback on_success, AnySDKCallbacks.AnySDKCallback on_fail)
	{
		AnySDK.Instance.Initialize(app_key, app_secret, private_key, oauth_login_server);
		m_anySDKCallbacks.Register(AnySDKCallbacks.E_CallbackCommand.InitializeSuccess, on_success);
		m_anySDKCallbacks.Register(AnySDKCallbacks.E_CallbackCommand.InitializeFail, on_fail);
	}

	public void UserListen()
	{
		AnySDK.Instance.UserListen();
	}

	public void PayListen()
	{
		AnySDK.Instance.PayListen();
	}

	public void ListenLogout(AnySDKCallbacks.AnySDKCallback on_logout_success, AnySDKCallbacks.AnySDKCallback on_logout_fail)
	{
		RegisterLogoutCallbacks(on_logout_success, on_logout_fail);
	}

	public void Login(AnySDKCallbacks.AnySDKCallback on_success, AnySDKCallbacks.AnySDKCallback on_fail, AnySDKCallbacks.AnySDKCallback on_cancel)
	{
		AnySDK.Instance.Login();
		RegisterLoginCallbacks((x) => {
			Debug.Log(x);
			JsonData data = JsonMapper.ToObject(x);
			if (data != null)
			{
				JsonData statusFieldValue = data["status"];
				if (statusFieldValue != null && statusFieldValue.IsInt)
				{
					int status = (int)statusFieldValue;
					if (status == 9)
					{
						JsonData anyDataFieldValue = data["anydata"];
						if (anyDataFieldValue != null)
						{
							string anyData = anyDataFieldValue.ToJson();
							VerificationOfLogin.ins.CachedAnyDataForAnySDKLogin = anyData;
							JsonData accessTokenFieldValue = anyDataFieldValue["access_token"];
							if (accessTokenFieldValue != null)
							{
								string accessToken = accessTokenFieldValue.ToJson();
								VerificationOfLogin.ins.CachedAccessToken = accessToken;
							}
						}
					}
				}
			}
			string accessToken1 = VerificationOfLogin.ins.CachedAccessToken;
			RO.LoggerUnused.Log("accessToken is " + (string.IsNullOrEmpty(accessToken1) ? "null or empty" : accessToken1));
			if (on_success != null)
			{
				on_success(x);
			}
		}, (x) => {
			if (on_fail != null)
			{
				on_fail(x);
			}
		}, (x) => {
			if (on_cancel != null)
			{
				on_cancel(x);
			}
		});
	}

	public void Login(string server_id, AnySDKCallbacks.AnySDKCallback on_success, AnySDKCallbacks.AnySDKCallback on_fail, AnySDKCallbacks.AnySDKCallback on_cancel)
	{
		AnySDK.Instance.Login(server_id);
		RegisterLoginCallbacks((x) => {
			JsonData data = JsonMapper.ToObject(x);
			if (data != null)
			{
				JsonData statusFieldValue = data["status"];
				if (statusFieldValue != null && statusFieldValue.IsInt)
				{
					int status = (int)statusFieldValue;
					if (status == 9)
					{
						JsonData anyDataFieldValue = data["anydata"];
						if (anyDataFieldValue != null)
						{
							string anyData = anyDataFieldValue.ToJson();
							VerificationOfLogin.ins.CachedAnyDataForAnySDKLogin = anyData;
							JsonData accessTokenFieldValue = anyDataFieldValue["access_token"];
							if (accessTokenFieldValue != null)
							{
								string accessToken = accessTokenFieldValue.ToJson();
								VerificationOfLogin.ins.CachedAccessToken = accessToken;
							}
						}
					}
				}
			}
			string accessToken1 = VerificationOfLogin.ins.CachedAccessToken;
			RO.LoggerUnused.Log("accessToken is " + (string.IsNullOrEmpty(accessToken1) ? "null or empty" : accessToken1));
			if (on_success != null)
			{
				on_success(x);
			}
		}, (x) => {
			if (on_fail != null)
			{
				on_fail(x);
			}
		}, (x) => {
			if (on_cancel != null)
			{
				on_cancel(x);
			}
		});
	}

	public void Login(string server_id, string oauth_login_server, AnySDKCallbacks.AnySDKCallback on_success, AnySDKCallbacks.AnySDKCallback on_fail, AnySDKCallbacks.AnySDKCallback on_cancel)
	{
		AnySDK.Instance.Login(server_id, oauth_login_server);
		RegisterLoginCallbacks((x) => {
			Debug.Log("x = " + x);
			JsonData data = JsonMapper.ToObject(x);
			if (data != null)
			{
				JsonData statusFieldValue = data["status"];
				if (statusFieldValue != null && statusFieldValue.IsInt)
				{
					int status = (int)statusFieldValue;
					if (status == 9)
					{
						JsonData anyDataFieldValue = data["anydata"];
						if (anyDataFieldValue != null)
						{
							string anyData = anyDataFieldValue.ToJson();
							VerificationOfLogin.ins.CachedAnyDataForAnySDKLogin = anyData;
							JsonData accessTokenFieldValue = anyDataFieldValue["access_token"];
							if (accessTokenFieldValue != null)
							{
								string accessToken = accessTokenFieldValue.ToJson();
								VerificationOfLogin.ins.CachedAccessToken = accessToken;
							}
						}
					}
				}
			}
			string accessToken1 = VerificationOfLogin.ins.CachedAccessToken;
			RO.LoggerUnused.Log("accessToken is " + (string.IsNullOrEmpty(accessToken1) ? "null or empty" : accessToken1));
			if (on_success != null)
			{
				on_success(x);
			}
		}, (x) => {
			if (on_fail != null)
			{
				on_fail(x);
			}
		}, (x) => {
			if (on_cancel != null)
			{
				on_cancel(x);
			}
		});
	}

	private void RegisterLoginCallbacks(AnySDKCallbacks.AnySDKCallback on_success, AnySDKCallbacks.AnySDKCallback on_fail, AnySDKCallbacks.AnySDKCallback on_cancel)
	{
		m_anySDKCallbacks.Register(AnySDKCallbacks.E_CallbackCommand.LoginSuccess, on_success);
		m_anySDKCallbacks.Register(AnySDKCallbacks.E_CallbackCommand.LoginFail, on_fail);
		m_anySDKCallbacks.Register(AnySDKCallbacks.E_CallbackCommand.LoginNetError, on_fail);
		m_anySDKCallbacks.Register(AnySDKCallbacks.E_CallbackCommand.LoginCancel, on_cancel);
	}

	private void RegisterLogoutCallbacks(AnySDKCallbacks.AnySDKCallback on_success, AnySDKCallbacks.AnySDKCallback on_fail)
	{
		m_anySDKCallbacks.Register(AnySDKCallbacks.E_CallbackCommand.LogoutSuccess, on_success);
		m_anySDKCallbacks.Register(AnySDKCallbacks.E_CallbackCommand.LogoutFail, on_fail);
	}

	private void RegisterPayCallbacks(AnySDKCallbacks.AnySDKCallback on_success, AnySDKCallbacks.AnySDKCallback on_fail, AnySDKCallbacks.AnySDKCallback on_timeout, AnySDKCallbacks.AnySDKCallback on_cancel, AnySDKCallbacks.AnySDKCallback on_product_illegal, AnySDKCallbacks.AnySDKCallback on_paying)
	{
		m_anySDKCallbacks.Register(AnySDKCallbacks.E_CallbackCommand.PaySuccess, on_success);
		m_anySDKCallbacks.Register(AnySDKCallbacks.E_CallbackCommand.PayFail, on_fail);
		m_anySDKCallbacks.Register(AnySDKCallbacks.E_CallbackCommand.PayTimeout, on_timeout);
		m_anySDKCallbacks.Register(AnySDKCallbacks.E_CallbackCommand.PayCancel, on_cancel);
		m_anySDKCallbacks.Register(AnySDKCallbacks.E_CallbackCommand.PayProductIllegal, on_product_illegal);
		m_anySDKCallbacks.Register(AnySDKCallbacks.E_CallbackCommand.PayPaying, on_paying);
	}

	public void Logout()
	{
		AnySDK.Instance.Logout();
	}

	public bool IsLogined()
	{
		return AnySDK.Instance.IsLogined();
	}

	public string GetUserID()
	{
		return AnySDK.Instance.GetUserID();
	}

	public void EnterPlatform()
	{
		AnySDK.Instance.EnterPlatform();
	}

	public void ShowToolBar()
	{
		AnySDK.Instance.ShowToolBar();
	}

	public void HideToolBar()
	{
		AnySDK.Instance.HideToolBar();
	}

	public void ChangeAccount()
	{
		AnySDK.Instance.ChangeAccount();
	}

	public void Exit()
	{
		AnySDK.Instance.Exit();
	}

	public void Pause()
	{
		AnySDK.Instance.Pause();
	}

	public void PayForProduct(string p_id,
	                          string p_name,
	                          float p_price,
	                          int p_count,
	                          string role_id,
	                          string role_name,
	                          int role_grade,
	                          int role_balance,
	                          string server_id,
	                          string custom_string,
	                          AnySDKCallbacks.AnySDKCallback on_success,
	                          AnySDKCallbacks.AnySDKCallback on_fail,
	                          AnySDKCallbacks.AnySDKCallback on_timeout,
	                          AnySDKCallbacks.AnySDKCallback on_cancel,
	                          AnySDKCallbacks.AnySDKCallback on_product_illegal,
	                          AnySDKCallbacks.AnySDKCallback on_paying)
	{
		AnySDK.Instance.PayForProduct(p_id, p_name, p_price, p_count, role_id, role_name, role_grade, role_balance, server_id, custom_string);
		RegisterPayCallbacks(on_success, on_fail, on_timeout, on_cancel, on_product_illegal, on_paying);
	}

	public string GetChannelID()
	{
		return AnySDK.Instance.GetChannelID();
	}

	public string GetOrderID(string plugin_id)
	{
		return AnySDK.Instance.GetOrderID (plugin_id);
	}

	public void ResetPayState()
	{
		AnySDK.Instance.ResetPayState ();
	}
}
