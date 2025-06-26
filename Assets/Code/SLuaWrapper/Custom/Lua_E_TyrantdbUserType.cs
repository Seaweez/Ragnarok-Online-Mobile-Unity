using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_E_TyrantdbUserType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"E_TyrantdbUserType");
		addMember(l,0,"Anonymous");
		addMember(l,1,"Registered");
		LuaDLL.lua_pop(l, 1);
	}
}
