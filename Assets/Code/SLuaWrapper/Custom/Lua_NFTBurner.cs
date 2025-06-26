using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_NFTBurner : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int BurnNFT(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			NFTBurner self=(NFTBurner)checkSelf(l);
			System.String a1;
			checkType(l, 2, out a1);
			System.Int32 a2;
			checkType(l, 3, out a2);
			SLua.LuaFunction a3;
			checkType(l, 4, out a3);
			var ret=self.BurnNFT(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
		#if DEBUG
		finally {
			UnityEngine.Profiling.Profiler.EndSample();
		}
		#endif
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"NFTBurner");
		addMember(l,BurnNFT);
		createTypeMetatable(l,null, typeof(NFTBurner),typeof(UnityEngine.MonoBehaviour));
	}
}
