using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_DUTaskState : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"DUTaskState");
		addMember(l,0,"Waiting");
		addMember(l,1,"Running");
		addMember(l,2,"Pause");
		addMember(l,3,"Complete");
		addMember(l,4,"Cancel");
		addMember(l,5,"Error");
		LuaDLL.lua_pop(l, 1);
	}
}
