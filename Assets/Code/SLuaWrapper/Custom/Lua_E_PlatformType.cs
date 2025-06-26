using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_E_PlatformType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"E_PlatformType");
		addMember(l,1,"Sina");
		addMember(l,22,"Wechat");
		addMember(l,23,"WechatMoments");
		addMember(l,24,"QQ");
		LuaDLL.lua_pop(l, 1);
	}
}
