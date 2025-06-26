using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_LookAtCamera : LuaObject {
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"RO.LookAtCamera");
		createTypeMetatable(l,null, typeof(RO.LookAtCamera),typeof(UnityEngine.MonoBehaviour));
	}
}
