using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_PhotographMode : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"RO.PhotographMode");
		addMember(l,0,"SELFIE");
		addMember(l,1,"PHOTOGRAPHER");
		addMember(l,2,"CAM3D");
		LuaDLL.lua_pop(l, 1);
	}
}
