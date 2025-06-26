using UnityEngine;
using System.Collections;

[SLua.CustomLuaClassAttribute]
public enum E_TyrantdbUserType
{
	Anonymous = FunctionTyrantdb.E_UserType.Anonymous,
	Registered = FunctionTyrantdb.E_UserType.Registered
}

[SLua.CustomLuaClassAttribute]
public enum E_TyrantdbUserSex
{
	Male = FunctionTyrantdb.E_UserSex.Male,
	Female = FunctionTyrantdb.E_UserSex.Female,
	Unknown = FunctionTyrantdb.E_UserSex.Unknown
}

[SLua.CustomLuaClassAttribute]
public class FunctionTyrantdb : Singleton<FunctionTyrantdb>
{
	public static FunctionTyrantdb Instance
	{
		get
		{
			return ins;
		}
	}

	public enum E_UserType
	{
		Anonymous,
		Registered
	}
	public enum E_UserSex
	{
		Male,
		Female,
		Unknown
	}

	public void Initialize(string app_id, string channel, string version, bool request_permission)
	{
		Tyrantdb.Initialize(app_id, channel, version, request_permission);
	}

	public void Resume()
	{
		Tyrantdb.Resume();
	}

	public void Stop()
	{
		Tyrantdb.Stop();
	}

	public void SetUser(string user_id, E_UserType user_type, E_UserSex user_sex, int user_age, string user_name)
	{
		Tyrantdb.SetUser(user_id, (int)user_type, (int)user_sex, user_age, user_name);
	}

	public void SetLevel(int level)
	{
		Tyrantdb.SetLevel(level);
	}

	public void SetServer(string server_id)
	{
		Tyrantdb.SetServer(server_id);
	}

	public void Charge(string order_id, string product_name, int amount, string currency_type, int virtual_currency_amount, string pay_way)
	{
		Tyrantdb.Charge(order_id, product_name, amount, currency_type, virtual_currency_amount, pay_way);
	}

	public void ChargeSuccess(string order_id)
	{
		Tyrantdb.ChargeSuccess(order_id);
	}

	public void ChargeFail(string order_id, string fail_reason)
	{
		Tyrantdb.ChargeFail(order_id, fail_reason);
	}

	public void ChargeSuccess(string order_id, string product_name, int amount, string currency_type, int virtual_currency_amount, string pay_way)
	{
		Tyrantdb.ChargeSuccess(order_id, product_name, amount, currency_type, virtual_currency_amount, pay_way);
	}

	public void OnCreateRole(string str)
	{
		Tyrantdb.OnCreateRole (str);
	}

	public void ChargeTo3rd(string order_id, int amount, string currency_type, string payment)
	{
		Tyrantdb.ChargeTo3rd (order_id, amount, currency_type, payment);
	}

	public string GetAppVersion()
	{
		return Tyrantdb.GetAppVersion ();
	}
}
