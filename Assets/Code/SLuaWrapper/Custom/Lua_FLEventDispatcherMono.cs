using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_FLEventDispatcherMono : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int passEvent(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			FLEventDispatcherMono self=(FLEventDispatcherMono)checkSelf(l);
			FLEventBase a1;
			checkType(l, 2, out a1);
			System.String a2;
			checkType(l, 3, out a2);
			self.passEvent(a1,a2);
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
	static public int DispatchEvent(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			FLEventDispatcherMono self=(FLEventDispatcherMono)checkSelf(l);
			FLEventBase a1;
			checkType(l, 2, out a1);
			System.String a2;
			checkType(l, 3, out a2);
			self.DispatchEvent(a1,a2);
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
	static public int AddEventListener(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			FLEventDispatcherMono self=(FLEventDispatcherMono)checkSelf(l);
			System.String a1;
			checkType(l, 2, out a1);
			FLEventBase.FLEventHandler a2;
			checkDelegate(l,3,out a2);
			self.AddEventListener(a1,a2);
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
	static public int RemoveEventListener(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			FLEventDispatcherMono self=(FLEventDispatcherMono)checkSelf(l);
			System.String a1;
			checkType(l, 2, out a1);
			FLEventBase.FLEventHandler a2;
			checkDelegate(l,3,out a2);
			self.RemoveEventListener(a1,a2);
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
		getTypeTable(l,"FLEventDispatcherMono");
		addMember(l,passEvent);
		addMember(l,DispatchEvent);
		addMember(l,AddEventListener);
		addMember(l,RemoveEventListener);
		createTypeMetatable(l,null, typeof(FLEventDispatcherMono),typeof(BaseBehaviour));
	}
}
