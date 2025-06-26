using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_HttpOperationJsonState : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"RO.HttpOperationJsonState");
		addMember(l,0,"OK");
		addMember(l,1,"LackOfParams");
		addMember(l,2,"ErrorGetServerVersion");
		addMember(l,3,"ErrorGetUpdateInfo");
		addMember(l,4,"ErrorClientMuchNewer");
		LuaDLL.lua_pop(l, 1);
	}
}
