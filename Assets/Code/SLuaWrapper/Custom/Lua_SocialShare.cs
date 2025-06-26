using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_SocialShare : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Initialize(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			SocialShare self=(SocialShare)checkSelf(l);
			System.String a1;
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
			self.Initialize(a1,a2,a3,a4,a5,a6);
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
	static public int ShareText(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				SocialShare self=(SocialShare)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				self.ShareText(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(argc==4){
				SocialShare self=(SocialShare)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				cn.sharesdk.unity3d.PlatformType a3;
				checkEnum(l,4,out a3);
				self.ShareText(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(argc==6){
				SocialShare self=(SocialShare)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				System.Action<System.String> a3;
				checkDelegate(l,4,out a3);
				System.Action<System.Int32,System.String> a4;
				checkDelegate(l,5,out a4);
				System.Action a5;
				checkDelegate(l,6,out a5);
				self.ShareText(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(argc==7){
				SocialShare self=(SocialShare)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				cn.sharesdk.unity3d.PlatformType a3;
				checkEnum(l,4,out a3);
				System.Action<System.String> a4;
				checkDelegate(l,5,out a4);
				System.Action<System.Int32,System.String> a5;
				checkDelegate(l,6,out a5);
				System.Action a6;
				checkDelegate(l,7,out a6);
				self.ShareText(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function ShareText to call");
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
	static public int ShareImage(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==4){
				SocialShare self=(SocialShare)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				System.String a3;
				checkType(l, 4, out a3);
				self.ShareImage(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(argc==5){
				SocialShare self=(SocialShare)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				System.String a3;
				checkType(l, 4, out a3);
				cn.sharesdk.unity3d.PlatformType a4;
				checkEnum(l,5,out a4);
				self.ShareImage(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(argc==7){
				SocialShare self=(SocialShare)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				System.String a3;
				checkType(l, 4, out a3);
				System.Action<System.String> a4;
				checkDelegate(l,5,out a4);
				System.Action<System.Int32,System.String> a5;
				checkDelegate(l,6,out a5);
				System.Action a6;
				checkDelegate(l,7,out a6);
				self.ShareImage(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				return 1;
			}
			else if(argc==8){
				SocialShare self=(SocialShare)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				System.String a3;
				checkType(l, 4, out a3);
				cn.sharesdk.unity3d.PlatformType a4;
				checkEnum(l,5,out a4);
				System.Action<System.String> a5;
				checkDelegate(l,6,out a5);
				System.Action<System.Int32,System.String> a6;
				checkDelegate(l,7,out a6);
				System.Action a7;
				checkDelegate(l,8,out a7);
				self.ShareImage(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function ShareImage to call");
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
	static public int WXShareImage(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			SocialShare self=(SocialShare)checkSelf(l);
			System.String a1;
			checkType(l, 2, out a1);
			System.Int32 a2;
			checkType(l, 3, out a2);
			var ret=self.WXShareImage(a1,a2);
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
	static public int ShareInternetImage(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==5){
				SocialShare self=(SocialShare)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				System.String a3;
				checkType(l, 4, out a3);
				cn.sharesdk.unity3d.PlatformType a4;
				checkEnum(l,5,out a4);
				self.ShareInternetImage(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(argc==8){
				SocialShare self=(SocialShare)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				System.String a3;
				checkType(l, 4, out a3);
				cn.sharesdk.unity3d.PlatformType a4;
				checkEnum(l,5,out a4);
				System.Action<System.String> a5;
				checkDelegate(l,6,out a5);
				System.Action<System.Int32,System.String> a6;
				checkDelegate(l,7,out a6);
				System.Action a7;
				checkDelegate(l,8,out a7);
				self.ShareInternetImage(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function ShareInternetImage to call");
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
	static public int ShareHyperlink(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				SocialShare self=(SocialShare)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				self.ShareHyperlink(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(argc==4){
				SocialShare self=(SocialShare)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				cn.sharesdk.unity3d.PlatformType a3;
				checkEnum(l,4,out a3);
				self.ShareHyperlink(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(argc==6){
				SocialShare self=(SocialShare)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				System.Action<System.String> a3;
				checkDelegate(l,4,out a3);
				System.Action<System.Int32,System.String> a4;
				checkDelegate(l,5,out a4);
				System.Action a5;
				checkDelegate(l,6,out a5);
				self.ShareHyperlink(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(argc==7){
				SocialShare self=(SocialShare)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				cn.sharesdk.unity3d.PlatformType a3;
				checkEnum(l,4,out a3);
				System.Action<System.String> a4;
				checkDelegate(l,5,out a4);
				System.Action<System.Int32,System.String> a5;
				checkDelegate(l,6,out a5);
				System.Action a6;
				checkDelegate(l,7,out a6);
				self.ShareHyperlink(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function ShareHyperlink to call");
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
	static public int ShareResultHandler(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			SocialShare self=(SocialShare)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			cn.sharesdk.unity3d.ResponseState a2;
			checkEnum(l,3,out a2);
			cn.sharesdk.unity3d.PlatformType a3;
			checkEnum(l,4,out a3);
			System.Collections.Hashtable a4;
			checkType(l, 5, out a4);
			self.ShareResultHandler(a1,a2,a3,a4);
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
	static public int IsClientValid(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			SocialShare self=(SocialShare)checkSelf(l);
			cn.sharesdk.unity3d.PlatformType a1;
			checkEnum(l,2,out a1);
			var ret=self.IsClientValid(a1);
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
	static public int CancelAuthorize(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			SocialShare self=(SocialShare)checkSelf(l);
			cn.sharesdk.unity3d.PlatformType a1;
			checkEnum(l,2,out a1);
			self.CancelAuthorize(a1);
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
	static public int CreateShareContent_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			var ret=SocialShare.CreateShareContent();
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
	static public int get_shareSDK(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			SocialShare self=(SocialShare)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.shareSDK);
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
	static public int set_shareSDK(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			SocialShare self=(SocialShare)checkSelf(l);
			cn.sharesdk.unity3d.ShareSDK v;
			checkType(l,2,out v);
			self.shareSDK=v;
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
	static public int get_AndroidWxShareFenZi(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,SocialShare.AndroidWxShareFenZi);
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
	static public int set_AndroidWxShareFenZi(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Single v;
			checkType(l,2,out v);
			SocialShare.AndroidWxShareFenZi=v;
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
			pushValue(l,SocialShare.Instance);
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
		getTypeTable(l,"SocialShare");
		addMember(l,Initialize);
		addMember(l,ShareText);
		addMember(l,ShareImage);
		addMember(l,WXShareImage);
		addMember(l,ShareInternetImage);
		addMember(l,ShareHyperlink);
		addMember(l,ShareResultHandler);
		addMember(l,IsClientValid);
		addMember(l,CancelAuthorize);
		addMember(l,CreateShareContent_s);
		addMember(l,"shareSDK",get_shareSDK,set_shareSDK,true);
		addMember(l,"AndroidWxShareFenZi",get_AndroidWxShareFenZi,set_AndroidWxShareFenZi,false);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(SocialShare),typeof(MonoSingleton<SocialShare>));
	}
}
