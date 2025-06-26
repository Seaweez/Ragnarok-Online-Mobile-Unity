using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Unity_IO_LowLevel_Unsafe_FileState : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Unity.IO.LowLevel.Unsafe.FileState");
		addMember(l,0,"Absent");
		addMember(l,1,"Exists");
		LuaDLL.lua_pop(l, 1);
	}
}
