using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_LateUpdateCaller : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_listener(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.LateUpdateCaller self=(RO.LateUpdateCaller)checkSelf(l);
			System.Action<RO.LateUpdateCaller> v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self.listener=v;
			else if(op==1) self.listener+=v;
			else if(op==2) self.listener-=v;
			pushValue(l,true);
			return 1;
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
		getTypeTable(l,"RO.LateUpdateCaller");
		addMember(l,"listener",null,set_listener,true);
		createTypeMetatable(l,null, typeof(RO.LateUpdateCaller),typeof(UnityEngine.MonoBehaviour));
	}
}
