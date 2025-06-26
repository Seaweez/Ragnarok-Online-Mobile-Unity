using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UpYunTaskInfo_E_State : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UpYunTaskInfo.E_State");
		addMember(l,0,"None");
		addMember(l,1,"Ing");
		addMember(l,2,"Pause");
		addMember(l,3,"End");
		addMember(l,4,"Err");
		addMember(l,5,"Waiting");
		LuaDLL.lua_pop(l, 1);
	}
}
