using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Unity_IO_LowLevel_Unsafe_Priority : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Unity.IO.LowLevel.Unsafe.Priority");
		addMember(l,0,"PriorityLow");
		addMember(l,1,"PriorityHigh");
		LuaDLL.lua_pop(l, 1);
	}
}
