using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_FunctionSDK : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			FunctionSDK o;
			o=new FunctionSDK();
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
	static public int XDSDKInitialize(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			FunctionSDK self=(FunctionSDK)checkSelf(l);
			System.String a1;
			checkType(l, 2, out a1);
			System.String a2;
			checkType(l, 3, out a2);
			System.String a3;
			checkType(l, 4, out a3);
			System.Int32 a4;
			checkType(l, 5, out a4);
			XDSDKCallback a5;
			checkDelegate(l,6,out a5);
			XDSDKCallback a6;
			checkDelegate(l,7,out a6);
			self.XDSDKInitialize(a1,a2,a3,a4,a5,a6);
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
	static public int AnySDKInitialize(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			FunctionSDK self=(FunctionSDK)checkSelf(l);
			System.String a1;
			checkType(l, 2, out a1);
			System.String a2;
			checkType(l, 3, out a2);
			System.String a3;
			checkType(l, 4, out a3);
			System.String a4;
			checkType(l, 5, out a4);
			XDSDKCallback a5;
			checkDelegate(l,6,out a5);
			XDSDKCallback a6;
			checkDelegate(l,7,out a6);
			self.AnySDKInitialize(a1,a2,a3,a4,a5,a6);
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
	static public int Login(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			FunctionSDK self=(FunctionSDK)checkSelf(l);
			XDSDKCallback a1;
			checkDelegate(l,2,out a1);
			XDSDKCallback a2;
			checkDelegate(l,3,out a2);
			XDSDKCallback a3;
			checkDelegate(l,4,out a3);
			self.Login(a1,a2,a3);
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
	static public int AnySDKLogin(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==5){
				FunctionSDK self=(FunctionSDK)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				XDSDKCallback a2;
				checkDelegate(l,3,out a2);
				XDSDKCallback a3;
				checkDelegate(l,4,out a3);
				XDSDKCallback a4;
				checkDelegate(l,5,out a4);
				self.AnySDKLogin(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(argc==6){
				FunctionSDK self=(FunctionSDK)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				XDSDKCallback a3;
				checkDelegate(l,4,out a3);
				XDSDKCallback a4;
				checkDelegate(l,5,out a4);
				XDSDKCallback a5;
				checkDelegate(l,6,out a5);
				self.AnySDKLogin(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function AnySDKLogin to call");
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
	static public int GetAccessToken(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			FunctionSDK self=(FunctionSDK)checkSelf(l);
			var ret=self.GetAccessToken();
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
	static public int EnterPlatform(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			FunctionSDK self=(FunctionSDK)checkSelf(l);
			self.EnterPlatform();
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
	static public int IsLogined(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			FunctionSDK self=(FunctionSDK)checkSelf(l);
			var ret=self.IsLogined();
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
	static public int ListenLogout(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			FunctionSDK self=(FunctionSDK)checkSelf(l);
			XDSDKCallback a1;
			checkDelegate(l,2,out a1);
			XDSDKCallback a2;
			checkDelegate(l,3,out a2);
			self.ListenLogout(a1,a2);
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
	static public int XDSDKPay(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			FunctionSDK self=(FunctionSDK)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			System.String a2;
			checkType(l, 3, out a2);
			System.String a3;
			checkType(l, 4, out a3);
			System.String a4;
			checkType(l, 5, out a4);
			System.String a5;
			checkType(l, 6, out a5);
			System.String a6;
			checkType(l, 7, out a6);
			System.Int32 a7;
			checkType(l, 8, out a7);
			System.String a8;
			checkType(l, 9, out a8);
			XDSDKCallback a9;
			checkDelegate(l,10,out a9);
			XDSDKCallback a10;
			checkDelegate(l,11,out a10);
			XDSDKCallback a11;
			checkDelegate(l,12,out a11);
			XDSDKCallback a12;
			checkDelegate(l,13,out a12);
			XDSDKCallback a13;
			checkDelegate(l,14,out a13);
			self.XDSDKPay(a1,a2,a3,a4,a5,a6,a7,a8,a9,a10,a11,a12,a13);
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
	static public int AnySDKPay(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			FunctionSDK self=(FunctionSDK)checkSelf(l);
			System.String a1;
			checkType(l, 2, out a1);
			System.String a2;
			checkType(l, 3, out a2);
			System.Single a3;
			checkType(l, 4, out a3);
			System.Int32 a4;
			checkType(l, 5, out a4);
			System.String a5;
			checkType(l, 6, out a5);
			System.String a6;
			checkType(l, 7, out a6);
			System.Int32 a7;
			checkType(l, 8, out a7);
			System.Int32 a8;
			checkType(l, 9, out a8);
			System.String a9;
			checkType(l, 10, out a9);
			System.String a10;
			checkType(l, 11, out a10);
			XDSDKCallback a11;
			checkDelegate(l,12,out a11);
			XDSDKCallback a12;
			checkDelegate(l,13,out a12);
			XDSDKCallback a13;
			checkDelegate(l,14,out a13);
			XDSDKCallback a14;
			checkDelegate(l,15,out a14);
			XDSDKCallback a15;
			checkDelegate(l,16,out a15);
			XDSDKCallback a16;
			checkDelegate(l,17,out a16);
			self.AnySDKPay(a1,a2,a3,a4,a5,a6,a7,a8,a9,a10,a11,a12,a13,a14,a15,a16);
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
	static public int GetChannelID(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			FunctionSDK self=(FunctionSDK)checkSelf(l);
			var ret=self.GetChannelID();
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
	static public int HideWechat(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			FunctionSDK self=(FunctionSDK)checkSelf(l);
			self.HideWechat();
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
	static public int GetOrderID(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			FunctionSDK self=(FunctionSDK)checkSelf(l);
			var ret=self.GetOrderID();
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
	static public int ResetPayState(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			FunctionSDK self=(FunctionSDK)checkSelf(l);
			self.ResetPayState();
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
	static public int get_IsInitialized(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			FunctionSDK self=(FunctionSDK)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsInitialized);
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
	static public int set_IsInitialized(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			FunctionSDK self=(FunctionSDK)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.IsInitialized=v;
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
	static public int get_CurrentType(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			FunctionSDK self=(FunctionSDK)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.CurrentType);
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
	static public int get_Instance(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,FunctionSDK.Instance);
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
		getTypeTable(l,"FunctionSDK");
		addMember(l,XDSDKInitialize);
		addMember(l,AnySDKInitialize);
		addMember(l,Login);
		addMember(l,AnySDKLogin);
		addMember(l,GetAccessToken);
		addMember(l,EnterPlatform);
		addMember(l,IsLogined);
		addMember(l,ListenLogout);
		addMember(l,XDSDKPay);
		addMember(l,AnySDKPay);
		addMember(l,GetChannelID);
		addMember(l,HideWechat);
		addMember(l,GetOrderID);
		addMember(l,ResetPayState);
		addMember(l,"IsInitialized",get_IsInitialized,set_IsInitialized,true);
		addMember(l,"CurrentType",get_CurrentType,null,true);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,constructor, typeof(FunctionSDK),typeof(Singleton<FunctionSDK>));
	}
}
