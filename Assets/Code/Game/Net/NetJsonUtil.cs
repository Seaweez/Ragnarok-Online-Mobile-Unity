using UnityEngine;
using System.Collections;
using LitJson;

// json util
using RO;


public class NetJsonUtil {
	/// <summary>
	/// Gets the json object.
	/// </summary>
	/// <param name="jsonPath">Json path.</param>
	public static JsonData GetJsonObj(string jsonPath)
	{
		var jsonAsset = ResourceManager.Me.SLoad<TextAsset>(jsonPath);
//		var jsonAsset = Resources.Load(jsonPath) as TextAsset;
		var jsonText = jsonAsset.text;
		return JsonMapper.ToObject(jsonText);
	}
}
