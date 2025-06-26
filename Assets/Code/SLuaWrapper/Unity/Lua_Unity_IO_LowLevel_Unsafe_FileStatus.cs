using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Unity_IO_LowLevel_Unsafe_FileStatus : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Unity.IO.LowLevel.Unsafe.FileStatus");
		addMember(l,0,"Closed");
		addMember(l,1,"Pending");
		addMember(l,2,"Open");
		addMember(l,3,"OpenFailed");
		LuaDLL.lua_pop(l, 1);
	}
}
