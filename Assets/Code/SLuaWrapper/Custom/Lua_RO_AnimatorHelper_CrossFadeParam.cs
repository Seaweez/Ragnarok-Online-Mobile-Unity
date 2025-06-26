using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_AnimatorHelper_CrossFadeParam : LuaObject {
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
			RO.AnimatorHelper.CrossFadeParam o;
			if(argc==1){
				o=new RO.AnimatorHelper.CrossFadeParam();
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==7){
				System.String a1;
				checkType(l, 2, out a1);
				System.Single a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Single a4;
				checkType(l, 5, out a4);
				System.Boolean a5;
				checkType(l, 6, out a5);
				System.Single a6;
				checkType(l, 7, out a6);
				o=new RO.AnimatorHelper.CrossFadeParam(a1,a2,a3,a4,a5,a6);
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
	static public int Set(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper.CrossFadeParam self=(RO.AnimatorHelper.CrossFadeParam)checkSelf(l);
			System.String a1;
			checkType(l, 2, out a1);
			System.Single a2;
			checkType(l, 3, out a2);
			System.Int32 a3;
			checkType(l, 4, out a3);
			System.Single a4;
			checkType(l, 5, out a4);
			System.Boolean a5;
			checkType(l, 6, out a5);
			System.Single a6;
			checkType(l, 7, out a6);
			self.Set(a1,a2,a3,a4,a5,a6);
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
	static public int get_stateName(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper.CrossFadeParam self=(RO.AnimatorHelper.CrossFadeParam)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.stateName);
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
	static public int set_stateName(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper.CrossFadeParam self=(RO.AnimatorHelper.CrossFadeParam)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.stateName=v;
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
	static public int get_crossFadeDuration(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper.CrossFadeParam self=(RO.AnimatorHelper.CrossFadeParam)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.crossFadeDuration);
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
	static public int set_crossFadeDuration(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper.CrossFadeParam self=(RO.AnimatorHelper.CrossFadeParam)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.crossFadeDuration=v;
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
	static public int get_speed(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper.CrossFadeParam self=(RO.AnimatorHelper.CrossFadeParam)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.speed);
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
	static public int set_speed(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper.CrossFadeParam self=(RO.AnimatorHelper.CrossFadeParam)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.speed=v;
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
	static public int get_reset(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper.CrossFadeParam self=(RO.AnimatorHelper.CrossFadeParam)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.reset);
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
	static public int set_reset(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper.CrossFadeParam self=(RO.AnimatorHelper.CrossFadeParam)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.reset=v;
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
	static public int get_loopCount(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper.CrossFadeParam self=(RO.AnimatorHelper.CrossFadeParam)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.loopCount);
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
	static public int set_loopCount(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper.CrossFadeParam self=(RO.AnimatorHelper.CrossFadeParam)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.loopCount=v;
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
	static public int get_normalizedTime(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper.CrossFadeParam self=(RO.AnimatorHelper.CrossFadeParam)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.normalizedTime);
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
	static public int set_normalizedTime(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper.CrossFadeParam self=(RO.AnimatorHelper.CrossFadeParam)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.normalizedTime=v;
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
	static public int get_valid(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper.CrossFadeParam self=(RO.AnimatorHelper.CrossFadeParam)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.valid);
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
		getTypeTable(l,"RO.AnimatorHelper.CrossFadeParam");
		addMember(l,Set);
		addMember(l,"stateName",get_stateName,set_stateName,true);
		addMember(l,"crossFadeDuration",get_crossFadeDuration,set_crossFadeDuration,true);
		addMember(l,"speed",get_speed,set_speed,true);
		addMember(l,"reset",get_reset,set_reset,true);
		addMember(l,"loopCount",get_loopCount,set_loopCount,true);
		addMember(l,"normalizedTime",get_normalizedTime,set_normalizedTime,true);
		addMember(l,"valid",get_valid,null,true);
		createTypeMetatable(l,constructor, typeof(RO.AnimatorHelper.CrossFadeParam));
	}
}
