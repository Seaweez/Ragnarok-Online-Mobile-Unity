using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Unity_Profiling_LowLevel_Unsafe_ProfilerUnsafeUtility_TimestampConversionRatio : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.TimestampConversionRatio o;
			o=new Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.TimestampConversionRatio();
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
	static public int get_Numerator(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.TimestampConversionRatio self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Numerator);
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
	static public int set_Numerator(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.TimestampConversionRatio self;
			checkValueType(l,1,out self);
			System.Int64 v;
			checkType(l,2,out v);
			self.Numerator=v;
			setBack(l,self);
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
	static public int get_Denominator(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.TimestampConversionRatio self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Denominator);
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
	static public int set_Denominator(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.TimestampConversionRatio self;
			checkValueType(l,1,out self);
			System.Int64 v;
			checkType(l,2,out v);
			self.Denominator=v;
			setBack(l,self);
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
		getTypeTable(l,"Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.TimestampConversionRatio");
		addMember(l,"Numerator",get_Numerator,set_Numerator,true);
		addMember(l,"Denominator",get_Denominator,set_Denominator,true);
		createTypeMetatable(l,constructor, typeof(Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.TimestampConversionRatio),typeof(System.ValueType));
	}
}
