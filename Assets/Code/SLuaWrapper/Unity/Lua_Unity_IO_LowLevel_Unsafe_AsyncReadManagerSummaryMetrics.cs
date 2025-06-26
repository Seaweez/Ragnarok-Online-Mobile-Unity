using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Unity_IO_LowLevel_Unsafe_AsyncReadManagerSummaryMetrics : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics o;
			o=new Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics();
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
	static public int get_TotalBytesRead(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.TotalBytesRead);
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
	static public int get_AverageBandwidthMBPerSecond(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AverageBandwidthMBPerSecond);
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
	static public int get_AverageReadSizeInBytes(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AverageReadSizeInBytes);
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
	static public int get_AverageWaitTimeMicroseconds(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AverageWaitTimeMicroseconds);
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
	static public int get_AverageReadTimeMicroseconds(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AverageReadTimeMicroseconds);
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
	static public int get_AverageTotalRequestTimeMicroseconds(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AverageTotalRequestTimeMicroseconds);
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
	static public int get_AverageThroughputMBPerSecond(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AverageThroughputMBPerSecond);
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
	static public int get_LongestWaitTimeMicroseconds(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LongestWaitTimeMicroseconds);
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
	static public int get_LongestReadTimeMicroseconds(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LongestReadTimeMicroseconds);
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
	static public int get_LongestReadAssetType(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LongestReadAssetType);
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
	static public int get_LongestWaitAssetType(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LongestWaitAssetType);
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
	static public int get_LongestReadSubsystem(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.LongestReadSubsystem);
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
	static public int get_LongestWaitSubsystem(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.LongestWaitSubsystem);
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
	static public int get_NumberOfInProgressRequests(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.NumberOfInProgressRequests);
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
	static public int get_NumberOfCompletedRequests(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.NumberOfCompletedRequests);
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
	static public int get_NumberOfFailedRequests(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.NumberOfFailedRequests);
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
	static public int get_NumberOfWaitingRequests(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.NumberOfWaitingRequests);
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
	static public int get_NumberOfCanceledRequests(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.NumberOfCanceledRequests);
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
	static public int get_TotalNumberOfRequests(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.TotalNumberOfRequests);
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
	static public int get_NumberOfCachedReads(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.NumberOfCachedReads);
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
	static public int get_NumberOfAsyncReads(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.NumberOfAsyncReads);
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
	static public int get_NumberOfSyncReads(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.NumberOfSyncReads);
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
		getTypeTable(l,"Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics");
		addMember(l,"TotalBytesRead",get_TotalBytesRead,null,true);
		addMember(l,"AverageBandwidthMBPerSecond",get_AverageBandwidthMBPerSecond,null,true);
		addMember(l,"AverageReadSizeInBytes",get_AverageReadSizeInBytes,null,true);
		addMember(l,"AverageWaitTimeMicroseconds",get_AverageWaitTimeMicroseconds,null,true);
		addMember(l,"AverageReadTimeMicroseconds",get_AverageReadTimeMicroseconds,null,true);
		addMember(l,"AverageTotalRequestTimeMicroseconds",get_AverageTotalRequestTimeMicroseconds,null,true);
		addMember(l,"AverageThroughputMBPerSecond",get_AverageThroughputMBPerSecond,null,true);
		addMember(l,"LongestWaitTimeMicroseconds",get_LongestWaitTimeMicroseconds,null,true);
		addMember(l,"LongestReadTimeMicroseconds",get_LongestReadTimeMicroseconds,null,true);
		addMember(l,"LongestReadAssetType",get_LongestReadAssetType,null,true);
		addMember(l,"LongestWaitAssetType",get_LongestWaitAssetType,null,true);
		addMember(l,"LongestReadSubsystem",get_LongestReadSubsystem,null,true);
		addMember(l,"LongestWaitSubsystem",get_LongestWaitSubsystem,null,true);
		addMember(l,"NumberOfInProgressRequests",get_NumberOfInProgressRequests,null,true);
		addMember(l,"NumberOfCompletedRequests",get_NumberOfCompletedRequests,null,true);
		addMember(l,"NumberOfFailedRequests",get_NumberOfFailedRequests,null,true);
		addMember(l,"NumberOfWaitingRequests",get_NumberOfWaitingRequests,null,true);
		addMember(l,"NumberOfCanceledRequests",get_NumberOfCanceledRequests,null,true);
		addMember(l,"TotalNumberOfRequests",get_TotalNumberOfRequests,null,true);
		addMember(l,"NumberOfCachedReads",get_NumberOfCachedReads,null,true);
		addMember(l,"NumberOfAsyncReads",get_NumberOfAsyncReads,null,true);
		addMember(l,"NumberOfSyncReads",get_NumberOfSyncReads,null,true);
		createTypeMetatable(l,constructor, typeof(Unity.IO.LowLevel.Unsafe.AsyncReadManagerSummaryMetrics));
	}
}
