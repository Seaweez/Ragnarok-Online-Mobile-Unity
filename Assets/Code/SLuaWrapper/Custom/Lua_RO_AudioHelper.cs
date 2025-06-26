using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_AudioHelper : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PlayOneShot2DAt_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(UnityEngine.AudioClip),typeof(UnityEngine.Vector3))){
				UnityEngine.AudioClip a1;
				checkType(l, 1, out a1);
				UnityEngine.Vector3 a2;
				checkType(l, 2, out a2);
				var ret=RO.AudioHelper.PlayOneShot2DAt(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(RO.ResourceID),typeof(UnityEngine.Vector3))){
				RO.ResourceID a1;
				checkType(l, 1, out a1);
				UnityEngine.Vector3 a2;
				checkType(l, 2, out a2);
				var ret=RO.AudioHelper.PlayOneShot2DAt(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function PlayOneShot2DAt to call");
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
	static public int SPlayOneShot2DAt_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.String a1;
			checkType(l, 1, out a1);
			UnityEngine.Vector3 a2;
			checkType(l, 2, out a2);
			var ret=RO.AudioHelper.SPlayOneShot2DAt(a1,a2);
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
	static public int PlayOneShot2D_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(UnityEngine.AudioClip))){
				UnityEngine.AudioClip a1;
				checkType(l, 1, out a1);
				var ret=RO.AudioHelper.PlayOneShot2D(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(RO.ResourceID))){
				RO.ResourceID a1;
				checkType(l, 1, out a1);
				var ret=RO.AudioHelper.PlayOneShot2D(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function PlayOneShot2D to call");
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
	static public int SPlayOneShot2D_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.String a1;
			checkType(l, 1, out a1);
			var ret=RO.AudioHelper.SPlayOneShot2D(a1);
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
	static public int PlayOneShotAt_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(UnityEngine.AudioClip),typeof(UnityEngine.Vector3))){
				UnityEngine.AudioClip a1;
				checkType(l, 1, out a1);
				UnityEngine.Vector3 a2;
				checkType(l, 2, out a2);
				var ret=RO.AudioHelper.PlayOneShotAt(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(RO.ResourceID),typeof(UnityEngine.Vector3))){
				RO.ResourceID a1;
				checkType(l, 1, out a1);
				UnityEngine.Vector3 a2;
				checkType(l, 2, out a2);
				var ret=RO.AudioHelper.PlayOneShotAt(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function PlayOneShotAt to call");
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
	static public int SPlayOneShotAt_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.String a1;
			checkType(l, 1, out a1);
			UnityEngine.Vector3 a2;
			checkType(l, 2, out a2);
			var ret=RO.AudioHelper.SPlayOneShotAt(a1,a2);
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
	static public int PlayOneShotOn_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(UnityEngine.AudioClip),typeof(UnityEngine.GameObject))){
				UnityEngine.AudioClip a1;
				checkType(l, 1, out a1);
				UnityEngine.GameObject a2;
				checkType(l, 2, out a2);
				var ret=RO.AudioHelper.PlayOneShotOn(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(RO.ResourceID),typeof(UnityEngine.GameObject))){
				RO.ResourceID a1;
				checkType(l, 1, out a1);
				UnityEngine.GameObject a2;
				checkType(l, 2, out a2);
				var ret=RO.AudioHelper.PlayOneShotOn(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.AudioClip),typeof(UnityEngine.AudioSource))){
				UnityEngine.AudioClip a1;
				checkType(l, 1, out a1);
				UnityEngine.AudioSource a2;
				checkType(l, 2, out a2);
				var ret=RO.AudioHelper.PlayOneShotOn(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(RO.ResourceID),typeof(UnityEngine.AudioSource))){
				RO.ResourceID a1;
				checkType(l, 1, out a1);
				UnityEngine.AudioSource a2;
				checkType(l, 2, out a2);
				var ret=RO.AudioHelper.PlayOneShotOn(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function PlayOneShotOn to call");
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
	static public int SPlayOneShotOn_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(string),typeof(UnityEngine.GameObject))){
				System.String a1;
				checkType(l, 1, out a1);
				UnityEngine.GameObject a2;
				checkType(l, 2, out a2);
				var ret=RO.AudioHelper.SPlayOneShotOn(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(UnityEngine.AudioSource))){
				System.String a1;
				checkType(l, 1, out a1);
				UnityEngine.AudioSource a2;
				checkType(l, 2, out a2);
				var ret=RO.AudioHelper.SPlayOneShotOn(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SPlayOneShotOn to call");
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
	static public int PlayOn_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(UnityEngine.AudioClip),typeof(UnityEngine.GameObject))){
				UnityEngine.AudioClip a1;
				checkType(l, 1, out a1);
				UnityEngine.GameObject a2;
				checkType(l, 2, out a2);
				var ret=RO.AudioHelper.PlayOn(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(RO.ResourceID),typeof(UnityEngine.GameObject))){
				RO.ResourceID a1;
				checkType(l, 1, out a1);
				UnityEngine.GameObject a2;
				checkType(l, 2, out a2);
				var ret=RO.AudioHelper.PlayOn(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.AudioClip),typeof(UnityEngine.AudioSource))){
				UnityEngine.AudioClip a1;
				checkType(l, 1, out a1);
				UnityEngine.AudioSource a2;
				checkType(l, 2, out a2);
				var ret=RO.AudioHelper.PlayOn(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(RO.ResourceID),typeof(UnityEngine.AudioSource))){
				RO.ResourceID a1;
				checkType(l, 1, out a1);
				UnityEngine.AudioSource a2;
				checkType(l, 2, out a2);
				var ret=RO.AudioHelper.PlayOn(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function PlayOn to call");
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
	static public int SPlayOn_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(string),typeof(UnityEngine.GameObject))){
				System.String a1;
				checkType(l, 1, out a1);
				UnityEngine.GameObject a2;
				checkType(l, 2, out a2);
				var ret=RO.AudioHelper.SPlayOn(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(UnityEngine.AudioSource))){
				System.String a1;
				checkType(l, 1, out a1);
				UnityEngine.AudioSource a2;
				checkType(l, 2, out a2);
				var ret=RO.AudioHelper.SPlayOn(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SPlayOn to call");
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
	static public int GetAudioClipByWavByte_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Byte[] a1;
			checkArray(l, 1, out a1);
			System.String a2;
			checkType(l, 2, out a2);
			var ret=RO.AudioHelper.GetAudioClipByWavByte(a1,a2);
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
	static public int GetAudioClipByPcmByte_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Byte[] a1;
			checkArray(l, 1, out a1);
			System.String a2;
			checkType(l, 2, out a2);
			var ret=RO.AudioHelper.GetAudioClipByPcmByte(a1,a2);
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
	static public int get_volume(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,RO.AudioHelper.volume);
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
	static public int set_volume(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Single v;
			checkType(l,2,out v);
			RO.AudioHelper.volume=v;
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
		getTypeTable(l,"RO.AudioHelper");
		addMember(l,PlayOneShot2DAt_s);
		addMember(l,SPlayOneShot2DAt_s);
		addMember(l,PlayOneShot2D_s);
		addMember(l,SPlayOneShot2D_s);
		addMember(l,PlayOneShotAt_s);
		addMember(l,SPlayOneShotAt_s);
		addMember(l,PlayOneShotOn_s);
		addMember(l,SPlayOneShotOn_s);
		addMember(l,PlayOn_s);
		addMember(l,SPlayOn_s);
		addMember(l,GetAudioClipByWavByte_s);
		addMember(l,GetAudioClipByPcmByte_s);
		addMember(l,"volume",get_volume,set_volume,false);
		createTypeMetatable(l,null, typeof(RO.AudioHelper));
	}
}
