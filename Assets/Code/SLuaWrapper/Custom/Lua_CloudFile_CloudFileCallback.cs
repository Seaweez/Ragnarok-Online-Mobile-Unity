using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_CloudFile_CloudFileCallback : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			CloudFile.CloudFileCallback o;
			o=new CloudFile.CloudFileCallback();
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
	static public int FireProgress(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			CloudFile.CloudFileCallback self=(CloudFile.CloudFileCallback)checkSelf(l);
			System.Single a1;
			checkType(l, 2, out a1);
			self.FireProgress(a1);
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
	static public int FireSuccess(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			CloudFile.CloudFileCallback self=(CloudFile.CloudFileCallback)checkSelf(l);
			self.FireSuccess();
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
	static public int FireError(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			CloudFile.CloudFileCallback self=(CloudFile.CloudFileCallback)checkSelf(l);
			System.String a1;
			checkType(l, 2, out a1);
			self.FireError(a1);
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
	static public int set__ProgressCallback(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			CloudFile.CloudFileCallback self=(CloudFile.CloudFileCallback)checkSelf(l);
			CloudFile.CloudFileCallback.ProgressCallback v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self._ProgressCallback=v;
			else if(op==1) self._ProgressCallback+=v;
			else if(op==2) self._ProgressCallback-=v;
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
	static public int set__SuccessCallback(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			CloudFile.CloudFileCallback self=(CloudFile.CloudFileCallback)checkSelf(l);
			CloudFile.CloudFileCallback.SuccessCallback v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self._SuccessCallback=v;
			else if(op==1) self._SuccessCallback+=v;
			else if(op==2) self._SuccessCallback-=v;
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
	static public int set__ErrorCallback(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			CloudFile.CloudFileCallback self=(CloudFile.CloudFileCallback)checkSelf(l);
			CloudFile.CloudFileCallback.ErrorCallback v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self._ErrorCallback=v;
			else if(op==1) self._ErrorCallback+=v;
			else if(op==2) self._ErrorCallback-=v;
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
		getTypeTable(l,"CloudFile.CloudFileCallback");
		addMember(l,FireProgress);
		addMember(l,FireSuccess);
		addMember(l,FireError);
		addMember(l,"_ProgressCallback",null,set__ProgressCallback,true);
		addMember(l,"_SuccessCallback",null,set__SuccessCallback,true);
		addMember(l,"_ErrorCallback",null,set__ErrorCallback,true);
		createTypeMetatable(l,constructor, typeof(CloudFile.CloudFileCallback));
	}
}
