using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_DUErrorType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"DUErrorType");
		addMember(l,0,"System");
		addMember(l,1,"IO");
		addMember(l,2,"Web");
		addMember(l,3,"Response");
		addMember(l,4,"TimeOut");
		addMember(l,5,"MD5Error");
		addMember(l,6,"Unknown");
		LuaDLL.lua_pop(l, 1);
	}
}
