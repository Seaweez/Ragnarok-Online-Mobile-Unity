using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_Net_FunctionLoginChooseSocket_SocketConnectState : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"RO.Net.FunctionLoginChooseSocket.SocketConnectState");
		addMember(l,0,"None");
		addMember(l,1,"Finish");
		addMember(l,2,"Connecting");
		addMember(l,3,"Failure");
		LuaDLL.lua_pop(l, 1);
	}
}
