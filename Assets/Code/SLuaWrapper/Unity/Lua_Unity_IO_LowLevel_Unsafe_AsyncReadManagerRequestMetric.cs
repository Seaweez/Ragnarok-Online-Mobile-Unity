using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Unity_IO_LowLevel_Unsafe_AsyncReadManagerRequestMetric : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric o;
			o=new Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric();
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
	static public int get_AssetName(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.AssetName);
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
	static public int get_FileName(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.FileName);
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
	static public int get_OffsetBytes(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.OffsetBytes);
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
	static public int get_SizeBytes(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.SizeBytes);
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
	static public int get_AssetTypeId(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.AssetTypeId);
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
	static public int get_CurrentBytesRead(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.CurrentBytesRead);
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
	static public int get_BatchReadCount(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.BatchReadCount);
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
	static public int get_IsBatchRead(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.IsBatchRead);
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
	static public int get_State(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushEnum(l,(int)self.State);
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
	static public int get_ReadType(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushEnum(l,(int)self.ReadType);
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
	static public int get_PriorityLevel(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushEnum(l,(int)self.PriorityLevel);
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
	static public int get_Subsystem(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushEnum(l,(int)self.Subsystem);
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
	static public int get_RequestTimeMicroseconds(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.RequestTimeMicroseconds);
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
	static public int get_TimeInQueueMicroseconds(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.TimeInQueueMicroseconds);
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
	static public int get_TotalTimeMicroseconds(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.TotalTimeMicroseconds);
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
		getTypeTable(l,"Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric");
		addMember(l,"AssetName",get_AssetName,null,true);
		addMember(l,"FileName",get_FileName,null,true);
		addMember(l,"OffsetBytes",get_OffsetBytes,null,true);
		addMember(l,"SizeBytes",get_SizeBytes,null,true);
		addMember(l,"AssetTypeId",get_AssetTypeId,null,true);
		addMember(l,"CurrentBytesRead",get_CurrentBytesRead,null,true);
		addMember(l,"BatchReadCount",get_BatchReadCount,null,true);
		addMember(l,"IsBatchRead",get_IsBatchRead,null,true);
		addMember(l,"State",get_State,null,true);
		addMember(l,"ReadType",get_ReadType,null,true);
		addMember(l,"PriorityLevel",get_PriorityLevel,null,true);
		addMember(l,"Subsystem",get_Subsystem,null,true);
		addMember(l,"RequestTimeMicroseconds",get_RequestTimeMicroseconds,null,true);
		addMember(l,"TimeInQueueMicroseconds",get_TimeInQueueMicroseconds,null,true);
		addMember(l,"TotalTimeMicroseconds",get_TotalTimeMicroseconds,null,true);
		createTypeMetatable(l,constructor, typeof(Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric),typeof(System.ValueType));
	}
}
