using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_FunctionSDK_E_SDKType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"FunctionSDK.E_SDKType");
		addMember(l,0,"None");
		addMember(l,1,"XD");
		addMember(l,2,"Any");
		LuaDLL.lua_pop(l, 1);
	}
}
