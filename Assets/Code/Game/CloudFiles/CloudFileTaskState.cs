using UnityEngine;
using System.Collections;

namespace CloudFile
{
	[SLua.CustomLuaClassAttribute]
	public enum E_TaskState
	{
		None,
		Progress,
		Interrupt,
		Error,
		Complete
	}
}