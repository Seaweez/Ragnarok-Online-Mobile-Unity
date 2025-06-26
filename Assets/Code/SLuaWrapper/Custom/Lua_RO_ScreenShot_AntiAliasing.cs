using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_ScreenShot_AntiAliasing : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"RO.ScreenShot.AntiAliasing");
		addMember(l,0,"None");
		addMember(l,2,"Samples2");
		addMember(l,4,"Samples4");
		addMember(l,8,"Samples8");
		LuaDLL.lua_pop(l, 1);
	}
}
