using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_NPCPoint : LuaObject {
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"RO.NPCPoint");
		createTypeMetatable(l,null, typeof(RO.NPCPoint),typeof(RO.NPCInfo));
	}
}
