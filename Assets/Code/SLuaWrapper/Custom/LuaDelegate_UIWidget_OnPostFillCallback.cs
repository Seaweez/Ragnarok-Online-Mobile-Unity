
using System;
using System.Collections.Generic;
namespace SLua
{
    public partial class LuaDelegation : LuaObject
    {

        static internal void Lua_UIWidget_OnPostFillCallback(LuaFunction ld ,UIWidget a1,int a2,BetterList<UnityEngine.Vector3> a3,BetterList<UnityEngine.Vector2> a4,BetterList<UnityEngine.Color32> a5) {
            IntPtr l = ld.L;
            int error = pushTry(l);

			pushValue(l,a1);
			pushValue(l,a2);
			pushValue(l,a3);
			pushValue(l,a4);
			pushValue(l,a5);
			ld.pcall(5, error);
			LuaDLL.lua_settop(l, error-1);
		}
	}
}
