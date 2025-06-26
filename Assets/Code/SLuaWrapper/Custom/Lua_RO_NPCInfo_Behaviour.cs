using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_NPCInfo_Behaviour : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"RO.NPCInfo.Behaviour");
		addMember(l,0,"NONE");
		addMember(l,1,"MOVEABLE");
		addMember(l,2,"ATTACK_BACK");
		addMember(l,4,"OUT_RANGE_BACK");
		addMember(l,8,"PICK_UP");
		addMember(l,16,"ASSIST_ATTACK");
		addMember(l,32,"SELECT_TARGET");
		addMember(l,64,"PASSIVITY_SELECT_TARGET");
		addMember(l,128,"DAMANGE_ALWAYS_1");
		addMember(l,256,"CAST");
		addMember(l,512,"GEAR");
		addMember(l,1024,"SKILL_NONSELECTABLE");
		addMember(l,2048,"GHOST");
		addMember(l,4096,"DEMON");
		addMember(l,8192,"FLY");
		addMember(l,16384,"STEAL_CAMERA");
		addMember(l,32768,"NAUGHTY");
		addMember(l,65536,"ALERT");
		addMember(l,131072,"EXPEL");
		LuaDLL.lua_pop(l, 1);
	}
}
