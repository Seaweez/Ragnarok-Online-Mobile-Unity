using UnityEngine;
using System.Collections;

[SLua.CustomLuaClassAttribute]
public class VerificationOfLogin : Singleton<VerificationOfLogin>
{
	public static VerificationOfLogin Instance
	{
		get
		{
			return ins;
		}
	}

	private string m_cachedAccessToken;
	public string CachedAccessToken
	{
		get
		{
			return m_cachedAccessToken;
		}
		set
		{
			m_cachedAccessToken = value;
		}
	}

	private string m_cachedAnyDataForAnySDKLogin;
	public string CachedAnyDataForAnySDKLogin
	{
		get
		{
			return m_cachedAnyDataForAnySDKLogin;
		}
		set
		{
			m_cachedAnyDataForAnySDKLogin = value;
		}
	}
}
