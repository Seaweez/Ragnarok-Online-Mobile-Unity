using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_ROPushReceiver : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get__Instance(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,RO.ROPushReceiver._Instance);
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
	static public int set__Instance(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.ROPushReceiver v;
			checkType(l,2,out v);
			RO.ROPushReceiver._Instance=v;
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
	static public int set__OnReceiveNotification(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.ROPushReceiver self=(RO.ROPushReceiver)checkSelf(l);
			XDSDKCallback v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self._OnReceiveNotification=v;
			else if(op==1) self._OnReceiveNotification+=v;
			else if(op==2) self._OnReceiveNotification-=v;
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
	static public int set__OnReceiveMessage(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.ROPushReceiver self=(RO.ROPushReceiver)checkSelf(l);
			XDSDKCallback v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self._OnReceiveMessage=v;
			else if(op==1) self._OnReceiveMessage+=v;
			else if(op==2) self._OnReceiveMessage-=v;
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
	static public int set__OnOpenNotification(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.ROPushReceiver self=(RO.ROPushReceiver)checkSelf(l);
			XDSDKCallback v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self._OnOpenNotification=v;
			else if(op==1) self._OnOpenNotification+=v;
			else if(op==2) self._OnOpenNotification-=v;
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
	static public int set__OnJPushTagOperateResult(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.ROPushReceiver self=(RO.ROPushReceiver)checkSelf(l);
			XDSDKCallback v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self._OnJPushTagOperateResult=v;
			else if(op==1) self._OnJPushTagOperateResult+=v;
			else if(op==2) self._OnJPushTagOperateResult-=v;
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
	static public int set__OnJPushAliasOperateResult(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.ROPushReceiver self=(RO.ROPushReceiver)checkSelf(l);
			XDSDKCallback v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self._OnJPushAliasOperateResult=v;
			else if(op==1) self._OnJPushAliasOperateResult+=v;
			else if(op==2) self._OnJPushAliasOperateResult-=v;
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
	static public int get_Instance(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,RO.ROPushReceiver.Instance);
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
		getTypeTable(l,"RO.ROPushReceiver");
		addMember(l,"_Instance",get__Instance,set__Instance,false);
		addMember(l,"_OnReceiveNotification",null,set__OnReceiveNotification,true);
		addMember(l,"_OnReceiveMessage",null,set__OnReceiveMessage,true);
		addMember(l,"_OnOpenNotification",null,set__OnOpenNotification,true);
		addMember(l,"_OnJPushTagOperateResult",null,set__OnJPushTagOperateResult,true);
		addMember(l,"_OnJPushAliasOperateResult",null,set__OnJPushAliasOperateResult,true);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(RO.ROPushReceiver),typeof(UnityEngine.MonoBehaviour));
	}
}
