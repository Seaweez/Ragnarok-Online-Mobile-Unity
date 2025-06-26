using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_EffectHandle_EffectType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"RO.EffectHandle.EffectType");
		addMember(l,0,"NONE");
		addMember(l,1,"PARTICLE_SYSTEM");
		addMember(l,2,"ANIMATOR");
		addMember(l,4,"LOGIC");
		addMember(l,7,"ALL");
		LuaDLL.lua_pop(l, 1);
	}
}
