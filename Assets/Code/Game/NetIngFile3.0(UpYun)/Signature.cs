using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using Spine;

public static class Signature
{
	public static string Create(Dictionary<string, object> pParams, string tail)
	{
		string retValue = "";
		if (pParams != null && pParams.Count > 0)
		{
			string str = "";
			foreach (KeyValuePair<string, object> pair in pParams)
			{
				str += pair.Key + pair.Value.ToString();
			}
			str += tail;
			retValue = MyMD5.HashString(str);
		}
		return retValue;
	}

	public static string CreateWithFormAPIValue(Dictionary<string, object> pParams, string form_api_value)
	{
		return Signature.Create(pParams, form_api_value);
	}

	public static string CreateWithToken(Dictionary<string, object> pParams, string token)
	{
		return Signature.Create(pParams, token);
	}

	public static string CreatePolicy(Dictionary<string, object> pParams)
	{
		string retValue = "";
		if (pParams != null && pParams.Count > 0)
		{
			string jsonStr = Json.Serialize(pParams);
			if (!string.IsNullOrEmpty(jsonStr))
			{
				byte[] bytes = Encoding.UTF8.GetBytes(jsonStr);
				retValue = Convert.ToBase64String(bytes);
			}
		}
		return retValue;
	}
}
