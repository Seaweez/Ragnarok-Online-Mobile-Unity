using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Unity_IO_LowLevel_Unsafe_AssetLoadingSubsystem : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Unity.IO.LowLevel.Unsafe.AssetLoadingSubsystem");
		addMember(l,0,"Other");
		addMember(l,1,"Texture");
		addMember(l,2,"VirtualTexture");
		addMember(l,3,"Mesh");
		addMember(l,4,"Audio");
		addMember(l,5,"Scripts");
		addMember(l,6,"EntitiesScene");
		addMember(l,7,"EntitiesStreamBinaryReader");
		addMember(l,8,"FileInfo");
		LuaDLL.lua_pop(l, 1);
	}
}
