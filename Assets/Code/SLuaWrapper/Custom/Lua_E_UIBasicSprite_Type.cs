using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_E_UIBasicSprite_Type : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"E_UIBasicSprite_Type");
		addMember(l,0,"Simple");
		addMember(l,1,"Sliced");
		addMember(l,2,"Tiled");
		addMember(l,3,"Filled");
		addMember(l,4,"Advanced");
		LuaDLL.lua_pop(l, 1);
	}
}
