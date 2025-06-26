using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Unity_IO_LowLevel_Unsafe_ProcessingState : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Unity.IO.LowLevel.Unsafe.ProcessingState");
		addMember(l,0,"Unknown");
		addMember(l,1,"InQueue");
		addMember(l,2,"Reading");
		addMember(l,3,"Completed");
		addMember(l,4,"Failed");
		addMember(l,5,"Canceled");
		LuaDLL.lua_pop(l, 1);
	}
}
