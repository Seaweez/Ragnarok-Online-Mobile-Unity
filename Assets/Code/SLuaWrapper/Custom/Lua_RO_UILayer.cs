using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_UILayer : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"RO.UILayer");
		addMember(l,0,"Scene");
		addMember(l,1,"BottomMost");
		addMember(l,2,"Bottom");
		addMember(l,3,"Lower");
		addMember(l,4,"Normal");
		addMember(l,5,"Upper");
		addMember(l,6,"Top");
		addMember(l,7,"TopMost");
		addMember(l,8,"Screen");
		addMember(l,9,"LayerCount");
		LuaDLL.lua_pop(l, 1);
	}
}
