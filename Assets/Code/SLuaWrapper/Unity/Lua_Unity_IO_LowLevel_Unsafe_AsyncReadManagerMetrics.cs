using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Unity_IO_LowLevel_Unsafe_AsyncReadManagerMetrics : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsEnabled_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			var ret=Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics.IsEnabled();
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
	static public int ClearCompletedMetrics_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics.ClearCompletedMetrics();
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
	static public int GetMetrics_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics.Flags a1;
				checkEnum(l,1,out a1);
				var ret=Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics.GetMetrics(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters),typeof(Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics.Flags))){
				Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters a1;
				checkType(l, 1, out a1);
				Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics.Flags a2;
				checkEnum(l,2,out a2);
				var ret=Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics.GetMetrics(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(List<Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric>),typeof(Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics.Flags))){
				System.Collections.Generic.List<Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric> a1;
				checkType(l, 1, out a1);
				Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics.Flags a2;
				checkEnum(l,2,out a2);
				Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics.GetMetrics(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(argc==3){
				System.Collections.Generic.List<Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric> a1;
				checkType(l, 1, out a1);
				Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters a2;
				checkType(l, 2, out a2);
				Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics.Flags a3;
				checkEnum(l,3,out a3);
				Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics.GetMetrics(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function GetMetrics to call");
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
	static public int StartCollectingMetrics_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics.StartCollectingMetrics();
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
	static public int StopCollectingMetrics_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics.StopCollectingMetrics();
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
	static public int GetCurrentSummaryMetrics_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics.Flags a1;
				checkEnum(l,1,out a1);
				var ret=Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics.GetCurrentSummaryMetrics(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters a1;
				checkType(l, 1, out a1);
				Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics.Flags a2;
				checkEnum(l,2,out a2);
				var ret=Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics.GetCurrentSummaryMetrics(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function GetCurrentSummaryMetrics to call");
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
	static public int GetSummaryOfMetrics_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric[]))){
				Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric[] a1;
				checkArray(l, 1, out a1);
				var ret=Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics.GetSummaryOfMetrics(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(List<Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric>))){
				System.Collections.Generic.List<Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric> a1;
				checkType(l, 1, out a1);
				var ret=Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics.GetSummaryOfMetrics(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric[]),typeof(Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters))){
				Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric[] a1;
				checkArray(l, 1, out a1);
				Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters a2;
				checkType(l, 2, out a2);
				var ret=Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics.GetSummaryOfMetrics(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(List<Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric>),typeof(Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters))){
				System.Collections.Generic.List<Unity.IO.LowLevel.Unsafe.AsyncReadManagerRequestMetric> a1;
				checkType(l, 1, out a1);
				Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters a2;
				checkType(l, 2, out a2);
				var ret=Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics.GetSummaryOfMetrics(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function GetSummaryOfMetrics to call");
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
	static public int GetTotalSizeOfNonASRMReadsBytes_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Boolean a1;
			checkType(l, 1, out a1);
			var ret=Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics.GetTotalSizeOfNonASRMReadsBytes(a1);
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics");
		addMember(l,IsEnabled_s);
		addMember(l,ClearCompletedMetrics_s);
		addMember(l,GetMetrics_s);
		addMember(l,StartCollectingMetrics_s);
		addMember(l,StopCollectingMetrics_s);
		addMember(l,GetCurrentSummaryMetrics_s);
		addMember(l,GetSummaryOfMetrics_s);
		addMember(l,GetTotalSizeOfNonASRMReadsBytes_s);
		createTypeMetatable(l,null, typeof(Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics));
	}
}
