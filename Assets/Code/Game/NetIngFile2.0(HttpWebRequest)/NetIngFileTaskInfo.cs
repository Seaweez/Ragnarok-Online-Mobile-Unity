using UnityEngine;
using System.Collections;

public class TaskInfo
{
	public enum E_State
	{
		None = HTTPFileLoader.E_State.None,
		Ing = HTTPFileLoader.E_State.Ing,
		Pause = HTTPFileLoader.E_State.Pause,
		End = HTTPFileLoader.E_State.End,
		Err = HTTPFileLoader.E_State.Err,
		Waiting
	}
	
	public int id;
	public string url;
	public float progress;
	public E_State state;
}

public class DownloadTaskInfo : TaskInfo
{
	public string path;
}

public class UploadTaskInfo : TaskInfo
{
	public string fileURL;
}