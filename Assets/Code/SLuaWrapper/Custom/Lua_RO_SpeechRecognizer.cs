using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_SpeechRecognizer : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetName(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.SpeechRecognizer self=(RO.SpeechRecognizer)checkSelf(l);
			System.String a1;
			checkType(l, 2, out a1);
			self.SetName(a1);
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
	static public int SetResult(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.SpeechRecognizer self=(RO.SpeechRecognizer)checkSelf(l);
			System.String a1;
			checkType(l, 2, out a1);
			self.SetResult(a1);
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
	static public int VolumeChanged(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.SpeechRecognizer self=(RO.SpeechRecognizer)checkSelf(l);
			System.String a1;
			checkType(l, 2, out a1);
			self.VolumeChanged(a1);
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
	static public int GetAudio(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.SpeechRecognizer self=(RO.SpeechRecognizer)checkSelf(l);
			System.String a1;
			checkType(l, 2, out a1);
			System.Action<UnityEngine.AudioClip> a2;
			checkDelegate(l,3,out a2);
			self.GetAudio(a1,a2);
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
	static public int set_updateVolume(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.SpeechRecognizer self=(RO.SpeechRecognizer)checkSelf(l);
			RO.SpeechRecognizer.VolumeCallback v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self.updateVolume=v;
			else if(op==1) self.updateVolume+=v;
			else if(op==2) self.updateVolume-=v;
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
	static public int set_handler(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.SpeechRecognizer self=(RO.SpeechRecognizer)checkSelf(l);
			System.Action<System.Byte[],System.Single,System.String> v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self.handler=v;
			else if(op==1) self.handler+=v;
			else if(op==2) self.handler-=v;
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
		getTypeTable(l,"RO.SpeechRecognizer");
		addMember(l,SetName);
		addMember(l,SetResult);
		addMember(l,VolumeChanged);
		addMember(l,GetAudio);
		addMember(l,"updateVolume",null,set_updateVolume,true);
		addMember(l,"handler",null,set_handler,true);
		createTypeMetatable(l,null, typeof(RO.SpeechRecognizer),typeof(UnityEngine.MonoBehaviour));
	}
}
