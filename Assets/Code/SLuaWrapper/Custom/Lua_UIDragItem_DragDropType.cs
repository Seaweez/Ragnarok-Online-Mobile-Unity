using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UIDragItem_DragDropType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UIDragItem.DragDropType");
		addMember(l,0,"Empty");
		addMember(l,1,"Target");
		addMember(l,2,"Source");
		addMember(l,3,"Both");
		LuaDLL.lua_pop(l, 1);
	}
}
