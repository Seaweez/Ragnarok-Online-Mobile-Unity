using UnityEngine;
using System.Collections;

[SLua.CustomLuaClassAttribute]
public class UpYunTaskInfo
{
	[SLua.CustomLuaClassAttribute]
	public enum E_State
	{
		None = UpYunHTTPFileLoader.E_State.None,
		Ing = UpYunHTTPFileLoader.E_State.Ing,
		Pause = UpYunHTTPFileLoader.E_State.Pause,
		End = UpYunHTTPFileLoader.E_State.End,
		Err = UpYunHTTPFileLoader.E_State.Err,
		Waiting
	}
	
	public int id;
	public float progress;
	public E_State state;
	public string path;
}

public class UpYunDownloadTaskInfo : UpYunTaskInfo
{

}

public class UpYunUploadTaskInfo : UpYunTaskInfo
{

}

public class UpYunBlocksUploadTaskInfo : UpYunTaskInfo
{

}