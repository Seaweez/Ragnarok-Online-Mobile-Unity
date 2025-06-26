using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RenderHeads_Media_AVProVideo_MediaPlayer_FileLocation : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"RenderHeads.Media.AVProVideo.MediaPlayer.FileLocation");
		addMember(l,0,"AbsolutePathOrURL");
		addMember(l,1,"RelativeToProjectFolder");
		addMember(l,2,"RelativeToStreamingAssetsFolder");
		addMember(l,3,"RelativeToDataFolder");
		addMember(l,4,"RelativeToPeristentDataFolder");
		LuaDLL.lua_pop(l, 1);
	}
}
