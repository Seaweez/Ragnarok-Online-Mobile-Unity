using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_JPush_JsonHelper : LuaObject {
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"JPush.JsonHelper");
		createTypeMetatable(l,null, typeof(JPush.JsonHelper));
	}
}
