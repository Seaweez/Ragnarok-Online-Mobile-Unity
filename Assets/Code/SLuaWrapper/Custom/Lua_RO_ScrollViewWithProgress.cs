using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_ScrollViewWithProgress : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddStateRange(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.ScrollViewWithProgress self=(RO.ScrollViewWithProgress)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			System.Single a2;
			checkType(l, 3, out a2);
			System.Single a3;
			checkType(l, 4, out a3);
			System.Boolean a4;
			checkType(l, 5, out a4);
			self.AddStateRange(a1,a2,a3,a4);
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
	static public int RemoveStateRange(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.ScrollViewWithProgress self=(RO.ScrollViewWithProgress)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			self.RemoveStateRange(a1);
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
	static public int CheckState(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.ScrollViewWithProgress self=(RO.ScrollViewWithProgress)checkSelf(l);
			self.CheckState();
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
	static public int UpdateScrollbars(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.ScrollViewWithProgress self=(RO.ScrollViewWithProgress)checkSelf(l);
			System.Boolean a1;
			checkType(l, 2, out a1);
			self.UpdateScrollbars(a1);
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
	static public int set_onProgress(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.ScrollViewWithProgress self=(RO.ScrollViewWithProgress)checkSelf(l);
			RO.ScrollViewWithProgress.OnProgressNotification v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self.onProgress=v;
			else if(op==1) self.onProgress+=v;
			else if(op==2) self.onProgress-=v;
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
	static public int set_onProgressStateChange(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.ScrollViewWithProgress self=(RO.ScrollViewWithProgress)checkSelf(l);
			RO.ScrollViewWithProgress.OnProgressNotification v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self.onProgressStateChange=v;
			else if(op==1) self.onProgressStateChange+=v;
			else if(op==2) self.onProgressStateChange-=v;
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
	static public int get_progress(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.ScrollViewWithProgress self=(RO.ScrollViewWithProgress)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.progress);
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
	static public int get_state(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.ScrollViewWithProgress self=(RO.ScrollViewWithProgress)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.state);
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
		getTypeTable(l,"RO.ScrollViewWithProgress");
		addMember(l,AddStateRange);
		addMember(l,RemoveStateRange);
		addMember(l,CheckState);
		addMember(l,UpdateScrollbars);
		addMember(l,"onProgress",null,set_onProgress,true);
		addMember(l,"onProgressStateChange",null,set_onProgressStateChange,true);
		addMember(l,"progress",get_progress,null,true);
		addMember(l,"state",get_state,null,true);
		createTypeMetatable(l,null, typeof(RO.ScrollViewWithProgress),typeof(UIScrollView));
	}
}
