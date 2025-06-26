using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UIWidgetContainer : LuaObject {
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"UIWidgetContainer");
		createTypeMetatable(l,null, typeof(UIWidgetContainer),typeof(UnityEngine.MonoBehaviour));
	}
}
