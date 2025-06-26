using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UISlider : LuaObject {
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"UISlider");
		createTypeMetatable(l,null, typeof(UISlider),typeof(UIProgressBar));
	}
}
