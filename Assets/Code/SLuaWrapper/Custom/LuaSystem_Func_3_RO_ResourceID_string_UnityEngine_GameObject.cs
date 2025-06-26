
using System;
using System.Collections.Generic;
namespace SLua
{
    public partial class LuaDelegation : LuaObject
    {

        static internal UnityEngine.GameObject Lua_System_Func_3_RO_ResourceID_string_UnityEngine_GameObject(LuaFunction ld ,RO.ResourceID a1,string a2) {
            IntPtr l = ld.L;
            int error = pushTry(l);

			pushValue(l,a1);
			pushValue(l,a2);
			ld.pcall(2, error);
			UnityEngine.GameObject ret;
			checkType(l,error+1,out ret);
			LuaDLL.lua_settop(l, error-1);
			return ret;
		}
	}
}
