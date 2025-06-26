using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_NetState : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"NetState");
		addMember(l,0,"Disconnect");
		addMember(l,1,"Connecting");
		addMember(l,2,"Connect");
		addMember(l,3,"Error");
		addMember(l,4,"Sending");
		addMember(l,5,"Disconnecting");
		addMember(l,6,"ConnectFailure");
		addMember(l,7,"Timeout");
		LuaDLL.lua_pop(l, 1);
	}
}
