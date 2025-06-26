using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_TouchEventType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"RO.TouchEventType");
		addMember(l,0,"BEGIN");
		addMember(l,1,"MOVE");
		addMember(l,2,"HOLD");
		addMember(l,3,"END");
		LuaDLL.lua_pop(l, 1);
	}
}
