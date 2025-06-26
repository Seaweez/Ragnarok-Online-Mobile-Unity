using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UISoundVolume : LuaObject {
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"UISoundVolume");
		createTypeMetatable(l,null, typeof(UISoundVolume),typeof(UnityEngine.MonoBehaviour));
	}
}
