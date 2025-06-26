using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_AppStateMonitor : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_applicationQuitHandler(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AppStateMonitor self=(RO.AppStateMonitor)checkSelf(l);
			System.Action v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self.applicationQuitHandler=v;
			else if(op==1) self.applicationQuitHandler+=v;
			else if(op==2) self.applicationQuitHandler-=v;
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
	static public int set_applicationFocusHandler(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AppStateMonitor self=(RO.AppStateMonitor)checkSelf(l);
			System.Action<System.Boolean> v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self.applicationFocusHandler=v;
			else if(op==1) self.applicationFocusHandler+=v;
			else if(op==2) self.applicationFocusHandler-=v;
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
	static public int set_applicationPauseHandler(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AppStateMonitor self=(RO.AppStateMonitor)checkSelf(l);
			System.Action<System.Boolean> v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self.applicationPauseHandler=v;
			else if(op==1) self.applicationPauseHandler+=v;
			else if(op==2) self.applicationPauseHandler-=v;
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
	static public int get_Instance(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,RO.AppStateMonitor.Instance);
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
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_monoGameObject(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AppStateMonitor self=(RO.AppStateMonitor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.monoGameObject);
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
		getTypeTable(l,"RO.AppStateMonitor");
		addMember(l,"applicationQuitHandler",null,set_applicationQuitHandler,true);
		addMember(l,"applicationFocusHandler",null,set_applicationFocusHandler,true);
		addMember(l,"applicationPauseHandler",null,set_applicationPauseHandler,true);
		addMember(l,"Instance",get_Instance,null,false);
		addMember(l,"monoGameObject",get_monoGameObject,null,true);
		createTypeMetatable(l,null, typeof(RO.AppStateMonitor),typeof(RO.SingleTonGO<RO.AppStateMonitor>));
	}
}
