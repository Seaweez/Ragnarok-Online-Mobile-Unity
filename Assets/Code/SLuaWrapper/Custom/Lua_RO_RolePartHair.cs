using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_RolePartHair : LuaObject {
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"RO.RolePartHair");
		createTypeMetatable(l,null, typeof(RO.RolePartHair),typeof(RO.RolePart));
	}
}
