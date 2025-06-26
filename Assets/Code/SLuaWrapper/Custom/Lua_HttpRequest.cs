using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_HttpRequest : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int HTTPGet(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(string),typeof(System.Action<UnityEngine.WWW>))){
				HttpRequest self=(HttpRequest)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.Action<UnityEngine.WWW> a2;
				checkDelegate(l,3,out a2);
				self.HTTPGet(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.WWW),typeof(System.Action<UnityEngine.WWW>))){
				HttpRequest self=(HttpRequest)checkSelf(l);
				UnityEngine.WWW a1;
				checkType(l, 2, out a1);
				System.Action<UnityEngine.WWW> a2;
				checkDelegate(l,3,out a2);
				self.HTTPGet(a1,a2);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function HTTPGet to call");
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
	static public int HTTPPost(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==4){
				HttpRequest self=(HttpRequest)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.Byte[] a2;
				checkArray(l, 3, out a2);
				System.Action<UnityEngine.WWW> a3;
				checkDelegate(l,4,out a3);
				self.HTTPPost(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(argc==5){
				HttpRequest self=(HttpRequest)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.Byte[] a2;
				checkArray(l, 3, out a2);
				System.Collections.Generic.Dictionary<System.String,System.String> a3;
				checkType(l, 4, out a3);
				System.Action<UnityEngine.WWW> a4;
				checkDelegate(l,5,out a4);
				self.HTTPPost(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function HTTPPost to call");
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
	static public int get_Instance(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,HttpRequest.Instance);
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
		getTypeTable(l,"HttpRequest");
		addMember(l,HTTPGet);
		addMember(l,HTTPPost);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(HttpRequest),typeof(MonoSingleton<HttpRequest>));
	}
}
