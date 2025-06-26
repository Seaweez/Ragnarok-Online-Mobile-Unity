using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_CheckUIEventByWidget : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_onDrag(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.CheckUIEventByWidget self=(RO.CheckUIEventByWidget)checkSelf(l);
			UICamera.VectorDelegate v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self.onDrag=v;
			else if(op==1) self.onDrag+=v;
			else if(op==2) self.onDrag-=v;
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
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_onClick(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.CheckUIEventByWidget self=(RO.CheckUIEventByWidget)checkSelf(l);
			UICamera.VoidDelegate v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self.onClick=v;
			else if(op==1) self.onClick+=v;
			else if(op==2) self.onClick-=v;
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
		getTypeTable(l,"RO.CheckUIEventByWidget");
		addMember(l,"onDrag",null,set_onDrag,true);
		addMember(l,"onClick",null,set_onClick,true);
		createTypeMetatable(l,null, typeof(RO.CheckUIEventByWidget),typeof(UnityEngine.MonoBehaviour));
	}
}
