using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UICenterOnClick : LuaObject {
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"UICenterOnClick");
		createTypeMetatable(l,null, typeof(UICenterOnClick),typeof(UnityEngine.MonoBehaviour));
	}
}
