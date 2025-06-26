using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_LuaWWW : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int StartLoad(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.LuaWWW self=(RO.LuaWWW)checkSelf(l);
			System.String a1;
			checkType(l, 2, out a1);
			System.Action<UnityEngine.WWW> a2;
			checkDelegate(l,3,out a2);
			System.Action<UnityEngine.WWW> a3;
			checkDelegate(l,4,out a3);
			self.StartLoad(a1,a2,a3);
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
	static public int WWWLoad_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				System.String a1;
				checkType(l, 1, out a1);
				System.Action<UnityEngine.WWW> a2;
				checkDelegate(l,2,out a2);
				RO.LuaWWW.WWWLoad(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(argc==3){
				System.String a1;
				checkType(l, 1, out a1);
				System.Action<UnityEngine.WWW> a2;
				checkDelegate(l,2,out a2);
				System.Action<UnityEngine.WWW> a3;
				checkDelegate(l,3,out a3);
				RO.LuaWWW.WWWLoad(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function WWWLoad to call");
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
	static public int set_loadedHandler(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.LuaWWW self=(RO.LuaWWW)checkSelf(l);
			System.Action<UnityEngine.WWW> v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self.loadedHandler=v;
			else if(op==1) self.loadedHandler+=v;
			else if(op==2) self.loadedHandler-=v;
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
	static public int set_loadingHandler(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.LuaWWW self=(RO.LuaWWW)checkSelf(l);
			System.Action<UnityEngine.WWW> v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self.loadingHandler=v;
			else if(op==1) self.loadingHandler+=v;
			else if(op==2) self.loadingHandler-=v;
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
		getTypeTable(l,"RO.LuaWWW");
		addMember(l,StartLoad);
		addMember(l,WWWLoad_s);
		addMember(l,"loadedHandler",null,set_loadedHandler,true);
		addMember(l,"loadingHandler",null,set_loadingHandler,true);
		createTypeMetatable(l,null, typeof(RO.LuaWWW),typeof(UnityEngine.MonoBehaviour));
	}
}
