using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_GameObjectForLua : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_onEnable(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectForLua self=(RO.GameObjectForLua)checkSelf(l);
			System.Action<UnityEngine.GameObject> v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self.onEnable=v;
			else if(op==1) self.onEnable+=v;
			else if(op==2) self.onEnable-=v;
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
	static public int set_onDisable(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectForLua self=(RO.GameObjectForLua)checkSelf(l);
			System.Action<UnityEngine.GameObject> v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self.onDisable=v;
			else if(op==1) self.onDisable+=v;
			else if(op==2) self.onDisable-=v;
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
	static public int set_onDestroy(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectForLua self=(RO.GameObjectForLua)checkSelf(l);
			System.Action<UnityEngine.GameObject> v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self.onDestroy=v;
			else if(op==1) self.onDestroy+=v;
			else if(op==2) self.onDestroy-=v;
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
		getTypeTable(l,"RO.GameObjectForLua");
		addMember(l,"onEnable",null,set_onEnable,true);
		addMember(l,"onDisable",null,set_onDisable,true);
		addMember(l,"onDestroy",null,set_onDestroy,true);
		createTypeMetatable(l,null, typeof(RO.GameObjectForLua),typeof(UnityEngine.MonoBehaviour));
	}
}
