using UnityEngine;
using System.Collections;
using game;
using System.Collections.Generic;
using RO;

public class AnySDK
{
	private static AnySDK instance;
	public static AnySDK Instance
	{
		get
		{
			if (instance == null)
				instance = new AnySDK();
			return instance;
		}
	}

	public void UserListen()
	{
		RO.LoggerUnused.Log("AnySDK:UserListen");
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			GameUser.getInstance().setListener(AnySDKEventListener.ins, "UserExternalCall");
		}
	}

	public void PayListen()
	{
		RO.LoggerUnused.Log("AnySDK:PayListen");
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			GameIAP.getInstance().setListener(AnySDKEventListener.ins, "OnReceivePayResult");
		}
	}

	public void Initialize(string app_key, string app_secret, string private_key, string oauth_login_server)
	{
		RO.LoggerUnused.Log("AnySDK:Initialize");
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			Game.getInstance().init(app_key, app_secret, private_key, oauth_login_server);
		}
	}

	public void Login()
	{
		RO.LoggerUnused.Log("AnySDK:Login");
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			GameUser.getInstance().login();
		}
	}

	public void Login(string server_id)
	{
		RO.LoggerUnused.Log("AnySDK:Login");
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			GameUser.getInstance().login(server_id);
		}
	}

	public void Login(string server_id, string oauth_login_server)
	{
		RO.LoggerUnused.Log("AnySDK:Login");
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			GameUser.getInstance().login(server_id, oauth_login_server);
		}
	}

	public void Logout()
	{
		RO.LoggerUnused.Log("AnySDK:Logout");
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			GameUser.getInstance().callFuncWithParam("logout");
		}
	}

	public bool IsLogined()
	{
		RO.LoggerUnused.Log("AnySDK:IsLogined");
		bool retValue = false;
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			retValue = GameUser.getInstance().isLogined();
		}
		return retValue;
	}

	public string GetUserID()
	{
		RO.LoggerUnused.Log("AnySDK:GetUserID");
		string retValue = "";
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			retValue = GameUser.getInstance().getUserID();
		}
		return retValue;
	}

	public void EnterPlatform()
	{
		RO.LoggerUnused.Log("AnySDK:EnterPlatform");
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			GameUser.getInstance().callFuncWithParam("enterPlatform");
		}
	}

	public void ShowToolBar()
	{
		RO.LoggerUnused.Log("AnySDK:ShowToolBar");
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			GameParam param = new GameParam((int)ToolBarPlace.kToolBarBottomLeft);
			GameUser.getInstance().callFuncWithParam("showToolBar", param);
		}
	}

	public void HideToolBar()
	{
		RO.LoggerUnused.Log("AnySDK:HideToolBar");
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			GameUser.getInstance().callFuncWithParam("hideToolBar");
		}
	}

	public void ChangeAccount()
	{
		RO.LoggerUnused.Log("AnySDK:ChangeAccount");
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			GameUser.getInstance().callFuncWithParam("changeAccount");
		}
	}

	public void Exit()
	{
		RO.LoggerUnused.Log("AnySDK:Exit");
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			GameUser.getInstance().callFuncWithParam("exit");
		}
	}

	public void Pause()
	{
		RO.LoggerUnused.Log("AnySDK:Pause");
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			GameUser.getInstance().callFuncWithParam("pause");
		}
	}

	public void PayForProduct(string p_id, string p_name, float p_price, int p_count, string role_id, string role_name, int role_grade, int role_balance, string server_id, string custom_string)
	{
		RO.LoggerUnused.Log("AnySDK:PayForProduct");
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			Dictionary<string,string> mProductInfo = new Dictionary<string,string>();
			mProductInfo["Product_Price"]= p_price.ToString();
			mProductInfo["Product_Id"]= p_id;
			mProductInfo["Product_Name"]= p_name;
			mProductInfo["Server_Id"]= server_id;
			mProductInfo["Product_Count"]= p_count.ToString();
			mProductInfo["Role_Id"]= role_id;
			mProductInfo["Role_Name"]= role_name;
			mProductInfo["Role_Grade"]= role_grade.ToString();
			mProductInfo["Role_Balance"]= role_balance.ToString();
			mProductInfo["EXT"] = custom_string;
			List<string> idArrayList =  GameIAP.getInstance().getPluginId();
			if (idArrayList.Count == 1) {
				GameIAP.getInstance().payForProduct(mProductInfo);
			}
		}
	}

	public string GetOrderID(string plugin_id)
	{
		RO.LoggerUnused.Log("AnySDK:GetOrderID");
		string retValue = "";
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
			retValue = GameIAP.getInstance ().getOrderId (plugin_id);
		}
		return retValue;
	}

	public string GetChannelID()
	{
		RO.LoggerUnused.Log("AnySDK:GetChannelID");
		string retValue = "";
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			retValue = Game.getInstance().getChannelId();
		}
		return retValue;
	}

	public void ResetPayState()
	{
		RO.LoggerUnused.Log("AnySDK:ResetPayState");
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			GameIAP.getInstance ().resetPayState ();
		}
	}
}
