using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_VectorHelper : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.VectorHelper o;
			o=new RO.VectorHelper();
			pushValue(l,true);
			pushValue(l,o);
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
	static public int GetAngleByAxisY_s(IntPtr l) {
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
			var ret=RO.VectorHelper.GetAngleByAxisY(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
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
	static public int Vector2SmoothDamp_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==8){
				UnityEngine.Vector2 a1;
				checkType(l, 1, out a1);
				UnityEngine.Vector2 a2;
				checkType(l, 2, out a2);
				UnityEngine.Vector2 a3;
				checkType(l, 3, out a3);
				System.Single a4;
				checkType(l, 4, out a4);
				System.Single a5;
				System.Single a6;
				System.Single a7;
				System.Single a8;
				RO.VectorHelper.Vector2SmoothDamp(a1,a2,a3,a4,out a5,out a6,out a7,out a8);
				pushValue(l,true);
				pushValue(l,a5);
				pushValue(l,a6);
				pushValue(l,a7);
				pushValue(l,a8);
				return 5;
			}
			else if(argc==9){
				UnityEngine.Vector2 a1;
				checkType(l, 1, out a1);
				UnityEngine.Vector2 a2;
				checkType(l, 2, out a2);
				UnityEngine.Vector2 a3;
				checkType(l, 3, out a3);
				System.Single a4;
				checkType(l, 4, out a4);
				System.Single a5;
				checkType(l, 5, out a5);
				System.Single a6;
				System.Single a7;
				System.Single a8;
				System.Single a9;
				RO.VectorHelper.Vector2SmoothDamp(a1,a2,a3,a4,a5,out a6,out a7,out a8,out a9);
				pushValue(l,true);
				pushValue(l,a6);
				pushValue(l,a7);
				pushValue(l,a8);
				pushValue(l,a9);
				return 5;
			}
			else if(argc==10){
				UnityEngine.Vector2 a1;
				checkType(l, 1, out a1);
				UnityEngine.Vector2 a2;
				checkType(l, 2, out a2);
				UnityEngine.Vector2 a3;
				checkType(l, 3, out a3);
				System.Single a4;
				checkType(l, 4, out a4);
				System.Single a5;
				checkType(l, 5, out a5);
				System.Single a6;
				checkType(l, 6, out a6);
				System.Single a7;
				System.Single a8;
				System.Single a9;
				System.Single a10;
				RO.VectorHelper.Vector2SmoothDamp(a1,a2,a3,a4,a5,a6,out a7,out a8,out a9,out a10);
				pushValue(l,true);
				pushValue(l,a7);
				pushValue(l,a8);
				pushValue(l,a9);
				pushValue(l,a10);
				return 5;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function Vector2SmoothDamp to call");
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
	static public int Vector3SmoothDamp_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==10){
				UnityEngine.Vector3 a1;
				checkType(l, 1, out a1);
				UnityEngine.Vector3 a2;
				checkType(l, 2, out a2);
				UnityEngine.Vector3 a3;
				checkType(l, 3, out a3);
				System.Single a4;
				checkType(l, 4, out a4);
				System.Single a5;
				System.Single a6;
				System.Single a7;
				System.Single a8;
				System.Single a9;
				System.Single a10;
				RO.VectorHelper.Vector3SmoothDamp(a1,a2,a3,a4,out a5,out a6,out a7,out a8,out a9,out a10);
				pushValue(l,true);
				pushValue(l,a5);
				pushValue(l,a6);
				pushValue(l,a7);
				pushValue(l,a8);
				pushValue(l,a9);
				pushValue(l,a10);
				return 7;
			}
			else if(argc==11){
				UnityEngine.Vector3 a1;
				checkType(l, 1, out a1);
				UnityEngine.Vector3 a2;
				checkType(l, 2, out a2);
				UnityEngine.Vector3 a3;
				checkType(l, 3, out a3);
				System.Single a4;
				checkType(l, 4, out a4);
				System.Single a5;
				checkType(l, 5, out a5);
				System.Single a6;
				System.Single a7;
				System.Single a8;
				System.Single a9;
				System.Single a10;
				System.Single a11;
				RO.VectorHelper.Vector3SmoothDamp(a1,a2,a3,a4,a5,out a6,out a7,out a8,out a9,out a10,out a11);
				pushValue(l,true);
				pushValue(l,a6);
				pushValue(l,a7);
				pushValue(l,a8);
				pushValue(l,a9);
				pushValue(l,a10);
				pushValue(l,a11);
				return 7;
			}
			else if(argc==12){
				UnityEngine.Vector3 a1;
				checkType(l, 1, out a1);
				UnityEngine.Vector3 a2;
				checkType(l, 2, out a2);
				UnityEngine.Vector3 a3;
				checkType(l, 3, out a3);
				System.Single a4;
				checkType(l, 4, out a4);
				System.Single a5;
				checkType(l, 5, out a5);
				System.Single a6;
				checkType(l, 6, out a6);
				System.Single a7;
				System.Single a8;
				System.Single a9;
				System.Single a10;
				System.Single a11;
				System.Single a12;
				RO.VectorHelper.Vector3SmoothDamp(a1,a2,a3,a4,a5,a6,out a7,out a8,out a9,out a10,out a11,out a12);
				pushValue(l,true);
				pushValue(l,a7);
				pushValue(l,a8);
				pushValue(l,a9);
				pushValue(l,a10);
				pushValue(l,a11);
				pushValue(l,a12);
				return 7;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function Vector3SmoothDamp to call");
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
	static public int Vector3Project_s(IntPtr l) {
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
			System.Single a3;
			System.Single a4;
			System.Single a5;
			RO.VectorHelper.Vector3Project(a1,a2,out a3,out a4,out a5);
			pushValue(l,true);
			pushValue(l,a3);
			pushValue(l,a4);
			pushValue(l,a5);
			return 4;
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
	static public int Vector3ProjectOnPlane_s(IntPtr l) {
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
			System.Single a3;
			System.Single a4;
			System.Single a5;
			RO.VectorHelper.Vector3ProjectOnPlane(a1,a2,out a3,out a4,out a5);
			pushValue(l,true);
			pushValue(l,a3);
			pushValue(l,a4);
			pushValue(l,a5);
			return 4;
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
	static public int Vector3Slerp_s(IntPtr l) {
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
			System.Single a3;
			checkType(l, 3, out a3);
			System.Single a4;
			System.Single a5;
			System.Single a6;
			RO.VectorHelper.Vector3Slerp(a1,a2,a3,out a4,out a5,out a6);
			pushValue(l,true);
			pushValue(l,a4);
			pushValue(l,a5);
			pushValue(l,a6);
			return 4;
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
	static public int Vector3SlerpUnclamped_s(IntPtr l) {
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
			System.Single a3;
			checkType(l, 3, out a3);
			System.Single a4;
			System.Single a5;
			System.Single a6;
			RO.VectorHelper.Vector3SlerpUnclamped(a1,a2,a3,out a4,out a5,out a6);
			pushValue(l,true);
			pushValue(l,a4);
			pushValue(l,a5);
			pushValue(l,a6);
			return 4;
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
	static public int Vector3RotateTowards_s(IntPtr l) {
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
			System.Single a3;
			checkType(l, 3, out a3);
			System.Single a4;
			checkType(l, 4, out a4);
			System.Single a5;
			System.Single a6;
			System.Single a7;
			RO.VectorHelper.Vector3RotateTowards(a1,a2,a3,a4,out a5,out a6,out a7);
			pushValue(l,true);
			pushValue(l,a5);
			pushValue(l,a6);
			pushValue(l,a7);
			return 4;
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
	static public int QuaternionSetFromToRotation_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Quaternion a1;
			checkType(l, 1, out a1);
			UnityEngine.Vector3 a2;
			checkType(l, 2, out a2);
			UnityEngine.Vector3 a3;
			checkType(l, 3, out a3);
			System.Single a4;
			System.Single a5;
			System.Single a6;
			System.Single a7;
			RO.VectorHelper.QuaternionSetFromToRotation(a1,a2,a3,out a4,out a5,out a6,out a7);
			pushValue(l,true);
			pushValue(l,a4);
			pushValue(l,a5);
			pushValue(l,a6);
			pushValue(l,a7);
			return 5;
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
	static public int QuaternionSetLookRotation_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Quaternion a1;
			checkType(l, 1, out a1);
			UnityEngine.Vector3 a2;
			checkType(l, 2, out a2);
			UnityEngine.Vector3 a3;
			checkType(l, 3, out a3);
			System.Single a4;
			System.Single a5;
			System.Single a6;
			System.Single a7;
			RO.VectorHelper.QuaternionSetLookRotation(a1,a2,a3,out a4,out a5,out a6,out a7);
			pushValue(l,true);
			pushValue(l,a4);
			pushValue(l,a5);
			pushValue(l,a6);
			pushValue(l,a7);
			return 5;
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
	static public int QuaternionEuler_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Single a1;
			checkType(l, 1, out a1);
			System.Single a2;
			checkType(l, 2, out a2);
			System.Single a3;
			checkType(l, 3, out a3);
			System.Single a4;
			System.Single a5;
			System.Single a6;
			System.Single a7;
			RO.VectorHelper.QuaternionEuler(a1,a2,a3,out a4,out a5,out a6,out a7);
			pushValue(l,true);
			pushValue(l,a4);
			pushValue(l,a5);
			pushValue(l,a6);
			pushValue(l,a7);
			return 5;
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
	static public int QuaternionSlerp_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Quaternion a1;
			checkType(l, 1, out a1);
			UnityEngine.Quaternion a2;
			checkType(l, 2, out a2);
			System.Single a3;
			checkType(l, 3, out a3);
			System.Single a4;
			System.Single a5;
			System.Single a6;
			System.Single a7;
			RO.VectorHelper.QuaternionSlerp(a1,a2,a3,out a4,out a5,out a6,out a7);
			pushValue(l,true);
			pushValue(l,a4);
			pushValue(l,a5);
			pushValue(l,a6);
			pushValue(l,a7);
			return 5;
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
	static public int QuaternionSlerpUnclamped_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Quaternion a1;
			checkType(l, 1, out a1);
			UnityEngine.Quaternion a2;
			checkType(l, 2, out a2);
			System.Single a3;
			checkType(l, 3, out a3);
			System.Single a4;
			System.Single a5;
			System.Single a6;
			System.Single a7;
			RO.VectorHelper.QuaternionSlerpUnclamped(a1,a2,a3,out a4,out a5,out a6,out a7);
			pushValue(l,true);
			pushValue(l,a4);
			pushValue(l,a5);
			pushValue(l,a6);
			pushValue(l,a7);
			return 5;
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
	static public int QuaternionGetEulerAngles_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Quaternion a1;
			checkType(l, 1, out a1);
			System.Single a2;
			System.Single a3;
			System.Single a4;
			RO.VectorHelper.QuaternionGetEulerAngles(a1,out a2,out a3,out a4);
			pushValue(l,true);
			pushValue(l,a2);
			pushValue(l,a3);
			pushValue(l,a4);
			return 4;
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
	static public int QuaternionSetEulerAngles_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Quaternion a1;
			checkType(l, 1, out a1);
			UnityEngine.Vector3 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			System.Single a4;
			System.Single a5;
			System.Single a6;
			RO.VectorHelper.QuaternionSetEulerAngles(a1,a2,out a3,out a4,out a5,out a6);
			pushValue(l,true);
			pushValue(l,a3);
			pushValue(l,a4);
			pushValue(l,a5);
			pushValue(l,a6);
			return 5;
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
	static public int QuaternionRotateTowards_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Quaternion a1;
			checkType(l, 1, out a1);
			UnityEngine.Quaternion a2;
			checkType(l, 2, out a2);
			System.Single a3;
			checkType(l, 3, out a3);
			System.Single a4;
			System.Single a5;
			System.Single a6;
			System.Single a7;
			RO.VectorHelper.QuaternionRotateTowards(a1,a2,a3,out a4,out a5,out a6,out a7);
			pushValue(l,true);
			pushValue(l,a4);
			pushValue(l,a5);
			pushValue(l,a6);
			pushValue(l,a7);
			return 5;
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
	static public int QuaternionAngleAxis_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Single a1;
			checkType(l, 1, out a1);
			UnityEngine.Vector3 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			System.Single a4;
			System.Single a5;
			System.Single a6;
			RO.VectorHelper.QuaternionAngleAxis(a1,a2,out a3,out a4,out a5,out a6);
			pushValue(l,true);
			pushValue(l,a3);
			pushValue(l,a4);
			pushValue(l,a5);
			pushValue(l,a6);
			return 5;
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
	static public int QuaternionToAngleAxis_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Quaternion a1;
			checkType(l, 1, out a1);
			System.Single a2;
			System.Single a3;
			System.Single a4;
			System.Single a5;
			RO.VectorHelper.QuaternionToAngleAxis(a1,out a2,out a3,out a4,out a5);
			pushValue(l,true);
			pushValue(l,a2);
			pushValue(l,a3);
			pushValue(l,a4);
			pushValue(l,a5);
			return 5;
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
	static public int QuaternionInverse_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Quaternion a1;
			checkType(l, 1, out a1);
			System.Single a2;
			System.Single a3;
			System.Single a4;
			System.Single a5;
			RO.VectorHelper.QuaternionInverse(a1,out a2,out a3,out a4,out a5);
			pushValue(l,true);
			pushValue(l,a2);
			pushValue(l,a3);
			pushValue(l,a4);
			pushValue(l,a5);
			return 5;
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
		getTypeTable(l,"RO.VectorHelper");
		addMember(l,GetAngleByAxisY_s);
		addMember(l,Vector2SmoothDamp_s);
		addMember(l,Vector3SmoothDamp_s);
		addMember(l,Vector3Project_s);
		addMember(l,Vector3ProjectOnPlane_s);
		addMember(l,Vector3Slerp_s);
		addMember(l,Vector3SlerpUnclamped_s);
		addMember(l,Vector3RotateTowards_s);
		addMember(l,QuaternionSetFromToRotation_s);
		addMember(l,QuaternionSetLookRotation_s);
		addMember(l,QuaternionEuler_s);
		addMember(l,QuaternionSlerp_s);
		addMember(l,QuaternionSlerpUnclamped_s);
		addMember(l,QuaternionGetEulerAngles_s);
		addMember(l,QuaternionSetEulerAngles_s);
		addMember(l,QuaternionRotateTowards_s);
		addMember(l,QuaternionAngleAxis_s);
		addMember(l,QuaternionToAngleAxis_s);
		addMember(l,QuaternionInverse_s);
		createTypeMetatable(l,constructor, typeof(RO.VectorHelper));
	}
}
