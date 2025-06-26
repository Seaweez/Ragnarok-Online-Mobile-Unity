using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_RolePart_BlendMode : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"RO.RolePart.BlendMode");
		addMember(l,0,"Opaque");
		addMember(l,1,"Transparent");
		LuaDLL.lua_pop(l, 1);
	}
}
