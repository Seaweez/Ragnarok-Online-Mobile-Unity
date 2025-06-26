using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_NavMeshAdjustY : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Adjust_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(UnityEngine.Transform))){
				UnityEngine.Transform a1;
				checkType(l, 1, out a1);
				RO.NavMeshAdjustY.Adjust(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Vector3))){
				UnityEngine.Vector3 a1;
				checkType(l, 1, out a1);
				var ret=RO.NavMeshAdjustY.Adjust(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Transform),typeof(float))){
				UnityEngine.Transform a1;
				checkType(l, 1, out a1);
				System.Single a2;
				checkType(l, 2, out a2);
				RO.NavMeshAdjustY.Adjust(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Vector3),typeof(float))){
				UnityEngine.Vector3 a1;
				checkType(l, 1, out a1);
				System.Single a2;
				checkType(l, 2, out a2);
				var ret=RO.NavMeshAdjustY.Adjust(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function Adjust to call");
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
	static public int SamplePosition_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(UnityEngine.Transform))){
				UnityEngine.Transform a1;
				checkType(l, 1, out a1);
				RO.NavMeshAdjustY.SamplePosition(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Vector3))){
				UnityEngine.Vector3 a1;
				checkType(l, 1, out a1);
				var ret=RO.NavMeshAdjustY.SamplePosition(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Transform),typeof(float))){
				UnityEngine.Transform a1;
				checkType(l, 1, out a1);
				System.Single a2;
				checkType(l, 2, out a2);
				RO.NavMeshAdjustY.SamplePosition(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Vector3),typeof(float))){
				UnityEngine.Vector3 a1;
				checkType(l, 1, out a1);
				System.Single a2;
				checkType(l, 2, out a2);
				var ret=RO.NavMeshAdjustY.SamplePosition(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SamplePosition to call");
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
	static public int RaycastDownPosition_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Vector3 a1;
			checkType(l, 1, out a1);
			var ret=RO.NavMeshAdjustY.RaycastDownPosition(a1);
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
	static public int get_DefaultSampleRange(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,RO.NavMeshAdjustY.DefaultSampleRange);
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
	static public int set_DefaultSampleRange(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Single v;
			checkType(l,2,out v);
			RO.NavMeshAdjustY.DefaultSampleRange=v;
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
		getTypeTable(l,"RO.NavMeshAdjustY");
		addMember(l,Adjust_s);
		addMember(l,SamplePosition_s);
		addMember(l,RaycastDownPosition_s);
		addMember(l,"DefaultSampleRange",get_DefaultSampleRange,set_DefaultSampleRange,false);
		createTypeMetatable(l,null, typeof(RO.NavMeshAdjustY));
	}
}
