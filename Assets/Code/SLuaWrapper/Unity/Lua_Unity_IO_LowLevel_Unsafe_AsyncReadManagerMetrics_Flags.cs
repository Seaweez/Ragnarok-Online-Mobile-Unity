using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Unity_IO_LowLevel_Unsafe_AsyncReadManagerMetrics_Flags : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics.Flags");
		addMember(l,0,"None");
		addMember(l,1,"ClearOnRead");
		LuaDLL.lua_pop(l, 1);
	}
}
