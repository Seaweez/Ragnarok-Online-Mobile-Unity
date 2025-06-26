using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_AreaTrigger_Type : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"RO.AreaTrigger.Type");
		addMember(l,0,"CIRCLE");
		addMember(l,1,"RECTANGE");
		LuaDLL.lua_pop(l, 1);
	}
}
