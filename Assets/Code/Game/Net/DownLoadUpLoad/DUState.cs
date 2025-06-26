using UnityEngine;
using System.Collections;

[SLua.CustomLuaClassAttribute]
public enum DUTaskState
{
	Waiting,
	Running,
	Pause,
	Complete,
	Cancel,
	Error
}

[SLua.CustomLuaClassAttribute]
public class DUTaskType
{
	public readonly static int NormalDownLoad = 1;
	public readonly static int BlockDownLoad = 2;
	public readonly static int DOWNLOAD = NormalDownLoad | BlockDownLoad;

	public readonly static int NormalUpLoad = 8;
	public readonly static int BlockUpLoad = 16;
	public readonly static int UPLOAD = NormalUpLoad | BlockUpLoad;
}