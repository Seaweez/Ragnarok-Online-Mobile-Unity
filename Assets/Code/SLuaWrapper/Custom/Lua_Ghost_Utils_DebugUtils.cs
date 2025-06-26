using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ghost_Utils_DebugUtils : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int DrawDoubleCircle_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Vector3 a1;
			checkType(l, 1, out a1);
			UnityEngine.Quaternion a2;
			checkType(l, 2, out a2);
			System.Single a3;
			checkType(l, 3, out a3);
			System.Single a4;
			checkType(l, 4, out a4);
			System.Int32 a5;
			checkType(l, 5, out a5);
			UnityEngine.Color a6;
			checkType(l, 6, out a6);
			Ghost.Utils.DebugUtils.DrawDoubleCircle(a1,a2,a3,a4,a5,a6);
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
	static public int DrawCircle_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==4){
				UnityEngine.Vector3 a1;
				checkType(l, 1, out a1);
				System.Single a2;
				checkType(l, 2, out a2);
				System.Int32 a3;
				checkType(l, 3, out a3);
				UnityEngine.Color a4;
				checkType(l, 4, out a4);
				Ghost.Utils.DebugUtils.DrawCircle(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(argc==5){
				UnityEngine.Vector3 a1;
				checkType(l, 1, out a1);
				UnityEngine.Quaternion a2;
				checkType(l, 2, out a2);
				System.Single a3;
				checkType(l, 3, out a3);
				System.Int32 a4;
				checkType(l, 4, out a4);
				UnityEngine.Color a5;
				checkType(l, 5, out a5);
				Ghost.Utils.DebugUtils.DrawCircle(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function DrawCircle to call");
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
	static public int DrawRect_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Vector3 a1;
			checkType(l, 1, out a1);
			UnityEngine.Vector3 a2;
			checkType(l, 2, out a2);
			UnityEngine.Vector3 a3;
			checkType(l, 3, out a3);
			UnityEngine.Vector3 a4;
			checkType(l, 4, out a4);
			UnityEngine.Color a5;
			checkType(l, 5, out a5);
			Ghost.Utils.DebugUtils.DrawRect(a1,a2,a3,a4,a5);
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
	static public int DrawBounds_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Bounds a1;
			checkValueType(l, 1, out a1);
			UnityEngine.Color a2;
			checkType(l, 2, out a2);
			Ghost.Utils.DebugUtils.DrawBounds(a1,a2);
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
		getTypeTable(l,"Ghost.Utils.DebugUtils");
		addMember(l,DrawDoubleCircle_s);
		addMember(l,DrawCircle_s);
		addMember(l,DrawRect_s);
		addMember(l,DrawBounds_s);
		createTypeMetatable(l,null, typeof(Ghost.Utils.DebugUtils));
	}
}
