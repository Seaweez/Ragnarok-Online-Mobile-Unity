using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UIOrthoCamera : LuaObject {
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"UIOrthoCamera");
		createTypeMetatable(l,null, typeof(UIOrthoCamera),typeof(UnityEngine.MonoBehaviour));
	}
}
