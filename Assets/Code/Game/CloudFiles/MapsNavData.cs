using UnityEngine;
using System.Collections;
using System;

[SLua.CustomLuaClassAttribute]
public class MapsNavData : ScriptableObject
{
	public static MapsNavData ins;
	public int originMapID;
	public string originMapName;
	public int targetMapID;
	public string targetMapName;
	public string path;
	public int confMemorySize;

	void Awake()
	{
		ins = this;
	}

	void Destroy()
	{
		originMapName = null;
		targetMapName = null;
		path = null;
	}

	public Action<bool> callbackSetData = MapsNavCallback.Ins.callbackSetData;
	public void FireCallbackSetData(bool is_only_confmemorysize)
	{
		if (callbackSetData != null) {
			callbackSetData (is_only_confmemorysize);
		}
	}
}

[SLua.CustomLuaClassAttribute]
public class MapsNavCallback : Singleton<MapsNavCallback>
{
	public static MapsNavCallback Ins
	{
		get
		{
			return ins;
		}
	}

	public Action<bool> callbackSetData;
}