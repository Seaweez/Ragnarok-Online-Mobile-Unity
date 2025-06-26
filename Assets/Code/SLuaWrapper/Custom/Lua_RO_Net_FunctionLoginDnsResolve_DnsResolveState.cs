using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_Net_FunctionLoginDnsResolve_DnsResolveState : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"RO.Net.FunctionLoginDnsResolve.DnsResolveState");
		addMember(l,0,"None");
		addMember(l,1,"Finish");
		addMember(l,2,"Resolving");
		addMember(l,3,"Failure");
		LuaDLL.lua_pop(l, 1);
	}
}
