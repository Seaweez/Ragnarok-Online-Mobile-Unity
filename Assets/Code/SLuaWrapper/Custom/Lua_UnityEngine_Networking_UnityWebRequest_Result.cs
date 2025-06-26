using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_Networking_UnityWebRequest_Result : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UnityEngine.Networking.UnityWebRequest.Result");
		addMember(l,0,"InProgress");
		addMember(l,1,"Success");
		addMember(l,2,"ConnectionError");
		addMember(l,3,"ProtocolError");
		addMember(l,4,"DataProcessingError");
		LuaDLL.lua_pop(l, 1);
	}
}
