using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UIInputOnGUI : LuaObject {
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"UIInputOnGUI");
		createTypeMetatable(l,null, typeof(UIInputOnGUI),typeof(UnityEngine.MonoBehaviour));
	}
}
