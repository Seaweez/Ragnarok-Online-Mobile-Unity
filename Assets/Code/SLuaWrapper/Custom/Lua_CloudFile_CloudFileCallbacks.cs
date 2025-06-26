using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_CloudFile_CloudFileCallbacks : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			CloudFile.CloudFileCallbacks o;
			o=new CloudFile.CloudFileCallbacks();
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
	static public int RegisterCallback(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			CloudFile.CloudFileCallbacks self=(CloudFile.CloudFileCallbacks)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			CloudFile.CloudFileCallback.ProgressCallback a2;
			checkDelegate(l,3,out a2);
			CloudFile.CloudFileCallback.SuccessCallback a3;
			checkDelegate(l,4,out a3);
			CloudFile.CloudFileCallback.ErrorCallback a4;
			checkDelegate(l,5,out a4);
			self.RegisterCallback(a1,a2,a3,a4);
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
	static public int FireProgress(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			CloudFile.CloudFileCallbacks self=(CloudFile.CloudFileCallbacks)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			System.Single a2;
			checkType(l, 3, out a2);
			self.FireProgress(a1,a2);
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
			CloudFile.CloudFileCallbacks self=(CloudFile.CloudFileCallbacks)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			self.FireSuccess(a1);
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
			CloudFile.CloudFileCallbacks self=(CloudFile.CloudFileCallbacks)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			System.String a2;
			checkType(l, 3, out a2);
			self.FireError(a1,a2);
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
	static public int UnregisterCallback(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			CloudFile.CloudFileCallbacks self=(CloudFile.CloudFileCallbacks)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			self.UnregisterCallback(a1);
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
		getTypeTable(l,"CloudFile.CloudFileCallbacks");
		addMember(l,RegisterCallback);
		addMember(l,FireProgress);
		addMember(l,FireSuccess);
		addMember(l,FireError);
		addMember(l,UnregisterCallback);
		createTypeMetatable(l,constructor, typeof(CloudFile.CloudFileCallbacks));
	}
}
