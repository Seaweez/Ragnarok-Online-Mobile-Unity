using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Unity_Profiling_LowLevel_Unsafe_ProfilerMarkerData : LuaObject
{
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l)
	{
		try
		{
#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
#else
			Profiler.BeginSample(methodName);
#endif
#endif
			Unity.Profiling.LowLevel.Unsafe.ProfilerMarkerData o;
			o = new Unity.Profiling.LowLevel.Unsafe.ProfilerMarkerData();
			pushValue(l, true);
			pushValue(l, o);
			return 2;
		}
		catch (Exception e)
		{
			return error(l, e);
		}
#if DEBUG
		finally
		{
#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.EndSample();
#else
			Profiler.EndSample();
#endif
		}
#endif
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Type(IntPtr l)
	{
		try
		{
#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
#else
			Profiler.BeginSample(methodName);
#endif
#endif
			Unity.Profiling.LowLevel.Unsafe.ProfilerMarkerData self;
			checkValueType(l, 1, out self);
			pushValue(l, true);
			pushValue(l, self.Type);
			return 2;
		}
		catch (Exception e)
		{
			return error(l, e);
		}
#if DEBUG
		finally
		{
#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.EndSample();
#else
			Profiler.EndSample();
#endif
		}
#endif
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Type(IntPtr l)
	{
		try
		{
#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
#else
			Profiler.BeginSample(methodName);
#endif
#endif
			Unity.Profiling.LowLevel.Unsafe.ProfilerMarkerData self;
			checkValueType(l, 1, out self);
			System.Byte v;
			checkType(l, 2, out v);
			self.Type = v;
			setBack(l, self);
			pushValue(l, true);
			return 1;
		}
		catch (Exception e)
		{
			return error(l, e);
		}
#if DEBUG
		finally
		{
#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.EndSample();
#else
			Profiler.EndSample();
#endif
		}
#endif
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Size(IntPtr l)
	{
		try
		{
#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
#else
			Profiler.BeginSample(methodName);
#endif
#endif
			Unity.Profiling.LowLevel.Unsafe.ProfilerMarkerData self;
			checkValueType(l, 1, out self);
			pushValue(l, true);
			pushValue(l, self.Size);
			return 2;
		}
		catch (Exception e)
		{
			return error(l, e);
		}
#if DEBUG
		finally
		{
#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.EndSample();
#else
			Profiler.EndSample();
#endif
		}
#endif
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Size(IntPtr l)
	{
		try
		{
#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
#else
			Profiler.BeginSample(methodName);
#endif
#endif
			Unity.Profiling.LowLevel.Unsafe.ProfilerMarkerData self;
			checkValueType(l, 1, out self);
			System.UInt32 v;
			checkType(l, 2, out v);
			self.Size = v;
			setBack(l, self);
			pushValue(l, true);
			return 1;
		}
		catch (Exception e)
		{
			return error(l, e);
		}
#if DEBUG
		finally
		{
#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.EndSample();
#else
			Profiler.EndSample();
#endif
		}
#endif
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public unsafe int get_Ptr(IntPtr l)
	{ // Make the method unsafe
		try
		{
#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
#else
			Profiler.BeginSample(methodName);
#endif
#endif

			Unity.Profiling.LowLevel.Unsafe.ProfilerMarkerData self;
			checkValueType(l, 1, out self);

			void* voidPtr = self.Ptr;
			sbyte* sbytePtr = (sbyte*)voidPtr; // Cast to sbyte* (or whatever type you expect)
			sbyte value = *sbytePtr; // Dereference the pointer to get the value

			pushValue(l, true);
			pushValue(l, value); // Now it should be sbyte

			return 2;
		}
		catch (Exception e)
		{
			return error(l, e);
		}
#if DEBUG
		finally
		{
#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.EndSample();
#else
			Profiler.EndSample();
#endif
		}
#endif
	}

	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public unsafe int set_Ptr(IntPtr l)
	{ // Mark the method as unsafe
		try
		{
#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
#else
			Profiler.BeginSample(methodName);
#endif
#endif
			Unity.Profiling.LowLevel.Unsafe.ProfilerMarkerData self;
			checkValueType(l, 1, out self);

			IntPtr temporary;
			checkType(l, 2, out temporary);  // Use IntPtr as the temporary storage

			void* v = (void*)temporary;  // Cast back to void*

			self.Ptr = v;
			setBack(l, self);
			pushValue(l, true);
			return 1;
		}
		catch (Exception e)
		{
			return error(l, e);
		}
#if DEBUG
		finally
		{
#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.EndSample();
#else
			Profiler.EndSample();
#endif
		}
#endif
	}

	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l)
	{
		getTypeTable(l, "Unity.Profiling.LowLevel.Unsafe.ProfilerMarkerData");
		addMember(l, "Type", get_Type, set_Type, true);
		addMember(l, "Size", get_Size, set_Size, true);
		addMember(l, "Ptr", get_Ptr, set_Ptr, true);
		createTypeMetatable(l, constructor, typeof(Unity.Profiling.LowLevel.Unsafe.ProfilerMarkerData), typeof(System.ValueType));
	}
}
