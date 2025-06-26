using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_InputGesture : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_zoomInAction(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.InputGesture self=(RO.InputGesture)checkSelf(l);
			System.Action v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self.zoomInAction=v;
			else if(op==1) self.zoomInAction+=v;
			else if(op==2) self.zoomInAction-=v;
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
	static public int set_zoomOutAction(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.InputGesture self=(RO.InputGesture)checkSelf(l);
			System.Action v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self.zoomOutAction=v;
			else if(op==1) self.zoomOutAction+=v;
			else if(op==2) self.zoomOutAction-=v;
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
		getTypeTable(l,"RO.InputGesture");
		addMember(l,"zoomInAction",null,set_zoomInAction,true);
		addMember(l,"zoomOutAction",null,set_zoomOutAction,true);
		createTypeMetatable(l,null, typeof(RO.InputGesture),typeof(UnityEngine.MonoBehaviour));
	}
}
