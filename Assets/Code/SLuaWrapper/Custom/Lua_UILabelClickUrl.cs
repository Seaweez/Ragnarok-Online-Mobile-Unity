using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UILabelClickUrl : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_callback(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UILabelClickUrl self=(UILabelClickUrl)checkSelf(l);
			UILabelClickUrl.UrlCallback v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self.callback=v;
			else if(op==1) self.callback+=v;
			else if(op==2) self.callback-=v;
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
		getTypeTable(l,"UILabelClickUrl");
		addMember(l,"callback",null,set_callback,true);
		createTypeMetatable(l,null, typeof(UILabelClickUrl),typeof(UnityEngine.MonoBehaviour));
	}
}
