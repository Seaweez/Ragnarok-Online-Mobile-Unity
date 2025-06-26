using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_CloudFile_E_TaskState : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"CloudFile.E_TaskState");
		addMember(l,0,"None");
		addMember(l,1,"Progress");
		addMember(l,2,"Interrupt");
		addMember(l,3,"Error");
		addMember(l,4,"Complete");
		LuaDLL.lua_pop(l, 1);
	}
}
