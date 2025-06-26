using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_E_TyrantdbUserSex : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"E_TyrantdbUserSex");
		addMember(l,0,"Male");
		addMember(l,1,"Female");
		addMember(l,2,"Unknown");
		LuaDLL.lua_pop(l, 1);
	}
}
