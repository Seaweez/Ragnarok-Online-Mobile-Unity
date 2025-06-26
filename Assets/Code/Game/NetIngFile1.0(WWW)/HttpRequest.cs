using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using LitJson;
using RO;

[SLua.CustomLuaClassAttribute]
public class HttpRequest : MonoSingleton<HttpRequest>
{
	private class WWWAndCallback
	{
		public WWW www;
		public Action<WWW> onGet;
	}

	public static HttpRequest Instance
	{
		get
		{
			return ins;
		}
	}

	public void HTTPGet(string url, Action<WWW> _onGet)
	{
		WWW _www = new WWW(url);
		StartCoroutine("WaitForHTTPGet", new WWWAndCallback() { www = _www, onGet = _onGet });
	}
	
	public void HTTPGet(WWW _www, Action<WWW> _onGet)
	{
		//StartCoroutine(WaitForRequest(_www, _onGet));
		StartCoroutine("WaitForHTTPGet", new WWWAndCallback() { www = _www, onGet = _onGet });
	}
	
	public void HTTPPost(string url, byte[] data, Dictionary<string, string> headers, Action<WWW> onPost)
	{
		WWW www = new WWW(url, data, headers);
		StartCoroutine(WaitForRequest(www, onPost));
	}

	public void HTTPPost(string url, byte[] data, Action<WWW> onPost)
	{
		WWW www = new WWW(url, data);
		StartCoroutine(WaitForRequest(www, onPost));
	}
	
	IEnumerator WaitForRequest(WWW www, Action<WWW> reqCallback)
	{
		yield return www;
		if (!www.isDone || www.error != null)
		{
			//Logger.Log("[WWW Error]" + www.error);
			if (reqCallback != null) reqCallback(www);
			yield break;
		}
		if (reqCallback != null) reqCallback(www);
	}
	
	IEnumerator WaitForHTTPGet(WWWAndCallback wac)
	{
		yield return wac.www;
		if (wac.onGet != null) wac.onGet(wac.www);
	}
}
