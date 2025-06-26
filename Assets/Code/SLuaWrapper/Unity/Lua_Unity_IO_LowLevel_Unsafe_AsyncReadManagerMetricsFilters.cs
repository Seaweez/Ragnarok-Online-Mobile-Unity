using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Unity_IO_LowLevel_Unsafe_AsyncReadManagerMetricsFilters : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters o;
			if(argc==1){
				o=new Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters();
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(System.UInt64))){
				System.UInt64 a1;
				checkType(l, 2, out a1);
				o=new Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(Unity.IO.LowLevel.Unsafe.ProcessingState))){
				Unity.IO.LowLevel.Unsafe.ProcessingState a1;
				checkEnum(l,2,out a1);
				o=new Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(Unity.IO.LowLevel.Unsafe.FileReadType))){
				Unity.IO.LowLevel.Unsafe.FileReadType a1;
				checkEnum(l,2,out a1);
				o=new Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(Unity.IO.LowLevel.Unsafe.Priority))){
				Unity.IO.LowLevel.Unsafe.Priority a1;
				checkEnum(l,2,out a1);
				o=new Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(Unity.IO.LowLevel.Unsafe.AssetLoadingSubsystem))){
				Unity.IO.LowLevel.Unsafe.AssetLoadingSubsystem a1;
				checkEnum(l,2,out a1);
				o=new Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(System.UInt64[]))){
				System.UInt64[] a1;
				checkArray(l, 2, out a1);
				o=new Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(Unity.IO.LowLevel.Unsafe.ProcessingState[]))){
				Unity.IO.LowLevel.Unsafe.ProcessingState[] a1;
				checkArray(l, 2, out a1);
				o=new Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(Unity.IO.LowLevel.Unsafe.FileReadType[]))){
				Unity.IO.LowLevel.Unsafe.FileReadType[] a1;
				checkArray(l, 2, out a1);
				o=new Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(Unity.IO.LowLevel.Unsafe.Priority[]))){
				Unity.IO.LowLevel.Unsafe.Priority[] a1;
				checkArray(l, 2, out a1);
				o=new Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(Unity.IO.LowLevel.Unsafe.AssetLoadingSubsystem[]))){
				Unity.IO.LowLevel.Unsafe.AssetLoadingSubsystem[] a1;
				checkArray(l, 2, out a1);
				o=new Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==6){
				System.UInt64[] a1;
				checkArray(l, 2, out a1);
				Unity.IO.LowLevel.Unsafe.ProcessingState[] a2;
				checkArray(l, 3, out a2);
				Unity.IO.LowLevel.Unsafe.FileReadType[] a3;
				checkArray(l, 4, out a3);
				Unity.IO.LowLevel.Unsafe.Priority[] a4;
				checkArray(l, 5, out a4);
				Unity.IO.LowLevel.Unsafe.AssetLoadingSubsystem[] a5;
				checkArray(l, 6, out a5);
				o=new Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			return error(l,"New object failed.");
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
	static public int SetTypeIDFilter(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(System.UInt64[]))){
				Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters)checkSelf(l);
				System.UInt64[] a1;
				checkArray(l, 2, out a1);
				self.SetTypeIDFilter(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(System.UInt64))){
				Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters)checkSelf(l);
				System.UInt64 a1;
				checkType(l, 2, out a1);
				self.SetTypeIDFilter(a1);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetTypeIDFilter to call");
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
	static public int SetStateFilter(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(Unity.IO.LowLevel.Unsafe.ProcessingState[]))){
				Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters)checkSelf(l);
				Unity.IO.LowLevel.Unsafe.ProcessingState[] a1;
				checkArray(l, 2, out a1);
				self.SetStateFilter(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(Unity.IO.LowLevel.Unsafe.ProcessingState))){
				Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters)checkSelf(l);
				Unity.IO.LowLevel.Unsafe.ProcessingState a1;
				checkEnum(l,2,out a1);
				self.SetStateFilter(a1);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetStateFilter to call");
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
	static public int SetReadTypeFilter(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(Unity.IO.LowLevel.Unsafe.FileReadType[]))){
				Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters)checkSelf(l);
				Unity.IO.LowLevel.Unsafe.FileReadType[] a1;
				checkArray(l, 2, out a1);
				self.SetReadTypeFilter(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(Unity.IO.LowLevel.Unsafe.FileReadType))){
				Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters)checkSelf(l);
				Unity.IO.LowLevel.Unsafe.FileReadType a1;
				checkEnum(l,2,out a1);
				self.SetReadTypeFilter(a1);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetReadTypeFilter to call");
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
	static public int SetPriorityFilter(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(Unity.IO.LowLevel.Unsafe.Priority[]))){
				Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters)checkSelf(l);
				Unity.IO.LowLevel.Unsafe.Priority[] a1;
				checkArray(l, 2, out a1);
				self.SetPriorityFilter(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(Unity.IO.LowLevel.Unsafe.Priority))){
				Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters)checkSelf(l);
				Unity.IO.LowLevel.Unsafe.Priority a1;
				checkEnum(l,2,out a1);
				self.SetPriorityFilter(a1);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetPriorityFilter to call");
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
	static public int SetSubsystemFilter(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(Unity.IO.LowLevel.Unsafe.AssetLoadingSubsystem[]))){
				Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters)checkSelf(l);
				Unity.IO.LowLevel.Unsafe.AssetLoadingSubsystem[] a1;
				checkArray(l, 2, out a1);
				self.SetSubsystemFilter(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(Unity.IO.LowLevel.Unsafe.AssetLoadingSubsystem))){
				Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters)checkSelf(l);
				Unity.IO.LowLevel.Unsafe.AssetLoadingSubsystem a1;
				checkEnum(l,2,out a1);
				self.SetSubsystemFilter(a1);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetSubsystemFilter to call");
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
	static public int RemoveTypeIDFilter(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters)checkSelf(l);
			self.RemoveTypeIDFilter();
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
	static public int RemoveStateFilter(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters)checkSelf(l);
			self.RemoveStateFilter();
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
	static public int RemoveReadTypeFilter(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters)checkSelf(l);
			self.RemoveReadTypeFilter();
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
	static public int RemovePriorityFilter(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters)checkSelf(l);
			self.RemovePriorityFilter();
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
	static public int RemoveSubsystemFilter(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters)checkSelf(l);
			self.RemoveSubsystemFilter();
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
	static public int ClearFilters(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters self=(Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters)checkSelf(l);
			self.ClearFilters();
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
		getTypeTable(l,"Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters");
		addMember(l,SetTypeIDFilter);
		addMember(l,SetStateFilter);
		addMember(l,SetReadTypeFilter);
		addMember(l,SetPriorityFilter);
		addMember(l,SetSubsystemFilter);
		addMember(l,RemoveTypeIDFilter);
		addMember(l,RemoveStateFilter);
		addMember(l,RemoveReadTypeFilter);
		addMember(l,RemovePriorityFilter);
		addMember(l,RemoveSubsystemFilter);
		addMember(l,ClearFilters);
		createTypeMetatable(l,constructor, typeof(Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetricsFilters));
	}
}
