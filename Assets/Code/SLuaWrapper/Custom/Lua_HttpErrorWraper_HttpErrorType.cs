using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_HttpErrorWraper_HttpErrorType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"HttpErrorWraper.HttpErrorType");
		addMember(l,0,"None");
		addMember(l,1,"NetException");
		addMember(l,2,"Timeout");
		addMember(l,3,"ErrorResponse");
		LuaDLL.lua_pop(l, 1);
	}
}
