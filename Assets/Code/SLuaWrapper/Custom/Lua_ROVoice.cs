using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ROVoice : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			ROVoice o;
			o=new ROVoice();
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
	static public int ChatSDKInit_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.String a2;
			checkType(l, 2, out a2);
			System.Int32 a3;
			checkType(l, 3, out a3);
			ROVoice.CallBack a4;
			checkDelegate(l,4,out a4);
			ROVoice.ChatSDKInit(a1,a2,a3,a4);
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
	static public int ChatSDKLogin_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.String a1;
			checkType(l, 1, out a1);
			System.String a2;
			checkType(l, 2, out a2);
			ROVoice.CallBack a3;
			checkDelegate(l,3,out a3);
			ROVoice.ChatSDKLogin(a1,a2,a3);
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
	static public int Logout_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			ROVoice.CallBack a1;
			checkDelegate(l,1,out a1);
			ROVoice.Logout(a1);
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
	static public int ChatMic_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Boolean a1;
			checkType(l, 1, out a1);
			ROVoice.CallBack a2;
			checkDelegate(l,2,out a2);
			ROVoice.ChatMic(a1,a2);
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
	static public int SetPausePlayRealAudio_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Boolean a1;
			checkType(l, 1, out a1);
			ROVoice.SetPausePlayRealAudio(a1);
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
	static public int AudioToolsPlayAudio_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			ROVoice.CallBack a1;
			checkDelegate(l,1,out a1);
			ROVoice.AudioToolsPlayAudio(a1);
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
	static public int AudioToolsStartRecord_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			ROVoice.CallBack a1;
			checkDelegate(l,1,out a1);
			ROVoice.CallBack a2;
			checkDelegate(l,2,out a2);
			ROVoice.CallBack a3;
			checkDelegate(l,3,out a3);
			ROVoice.CallBack a4;
			checkDelegate(l,4,out a4);
			ROVoice.AudioToolsStartRecord(a1,a2,a3,a4);
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
	static public int AudioToolsStopRecord_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			ROVoice.AudioToolsStopRecord();
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
	static public int AudioToolsPlayOnlineAudio_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			ROVoice.CallBack a1;
			checkDelegate(l,1,out a1);
			ROVoice.AudioToolsPlayOnlineAudio(a1);
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
	static public int ReceiveTextMessageNotify_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			ROVoice.CallBack a1;
			checkDelegate(l,1,out a1);
			ROVoice.CallBack a2;
			checkDelegate(l,2,out a2);
			ROVoice.CallBack a3;
			checkDelegate(l,3,out a3);
			ROVoice.ReceiveTextMessageNotify(a1,a2,a3);
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
	static public int LoginNotify_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			ROVoice.CallBack a1;
			checkDelegate(l,1,out a1);
			ROVoice.LoginNotify(a1);
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
	static public int LogoutNotify_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			ROVoice.CallBack a1;
			checkDelegate(l,1,out a1);
			ROVoice.LogoutNotify(a1);
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
	static public int set__SendRealTimeVoiceMessageErrorNotify(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			XDSDKCallback v;
			int op=checkDelegate(l,2,out v);
			if(op==0) ROVoice._SendRealTimeVoiceMessageErrorNotify=v;
			else if(op==1) ROVoice._SendRealTimeVoiceMessageErrorNotify+=v;
			else if(op==2) ROVoice._SendRealTimeVoiceMessageErrorNotify-=v;
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
	static public int set__ReceiveRealTimeVoiceMessageNofify(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			XDSDKCallback v;
			int op=checkDelegate(l,2,out v);
			if(op==0) ROVoice._ReceiveRealTimeVoiceMessageNofify=v;
			else if(op==1) ROVoice._ReceiveRealTimeVoiceMessageNofify+=v;
			else if(op==2) ROVoice._ReceiveRealTimeVoiceMessageNofify-=v;
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
	static public int set__ReceiveTextMessageNotify(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			XDSDKCallback v;
			int op=checkDelegate(l,2,out v);
			if(op==0) ROVoice._ReceiveTextMessageNotify=v;
			else if(op==1) ROVoice._ReceiveTextMessageNotify+=v;
			else if(op==2) ROVoice._ReceiveTextMessageNotify-=v;
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
	static public int set__RecorderMeteringPeakPowerNotify(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			XDSDKCallback v;
			int op=checkDelegate(l,2,out v);
			if(op==0) ROVoice._RecorderMeteringPeakPowerNotify=v;
			else if(op==1) ROVoice._RecorderMeteringPeakPowerNotify+=v;
			else if(op==2) ROVoice._RecorderMeteringPeakPowerNotify-=v;
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
	static public int set__PlayMeteringPeakPowerNotify(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			XDSDKCallback v;
			int op=checkDelegate(l,2,out v);
			if(op==0) ROVoice._PlayMeteringPeakPowerNotify=v;
			else if(op==1) ROVoice._PlayMeteringPeakPowerNotify+=v;
			else if(op==2) ROVoice._PlayMeteringPeakPowerNotify-=v;
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
	static public int set__AudioToolsRecorderMeteringPeakPowerNotify(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			XDSDKCallback v;
			int op=checkDelegate(l,2,out v);
			if(op==0) ROVoice._AudioToolsRecorderMeteringPeakPowerNotify=v;
			else if(op==1) ROVoice._AudioToolsRecorderMeteringPeakPowerNotify+=v;
			else if(op==2) ROVoice._AudioToolsRecorderMeteringPeakPowerNotify-=v;
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
	static public int set__AudioToolsPlayMeteringPeakPowerNotify(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			XDSDKCallback v;
			int op=checkDelegate(l,2,out v);
			if(op==0) ROVoice._AudioToolsPlayMeteringPeakPowerNotify=v;
			else if(op==1) ROVoice._AudioToolsPlayMeteringPeakPowerNotify+=v;
			else if(op==2) ROVoice._AudioToolsPlayMeteringPeakPowerNotify-=v;
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
	static public int set__MicStateNotify(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			XDSDKCallback v;
			int op=checkDelegate(l,2,out v);
			if(op==0) ROVoice._MicStateNotify=v;
			else if(op==1) ROVoice._MicStateNotify+=v;
			else if(op==2) ROVoice._MicStateNotify-=v;
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
	static public int set__onConnectFail(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			XDSDKCallback v;
			int op=checkDelegate(l,2,out v);
			if(op==0) ROVoice._onConnectFail=v;
			else if(op==1) ROVoice._onConnectFail+=v;
			else if(op==2) ROVoice._onConnectFail-=v;
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
	static public int set__onReconnectSuccess(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			XDSDKCallback v;
			int op=checkDelegate(l,2,out v);
			if(op==0) ROVoice._onReconnectSuccess=v;
			else if(op==1) ROVoice._onReconnectSuccess+=v;
			else if(op==2) ROVoice._onReconnectSuccess-=v;
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
	static public int set__LoginNotify(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			XDSDKCallback v;
			int op=checkDelegate(l,2,out v);
			if(op==0) ROVoice._LoginNotify=v;
			else if(op==1) ROVoice._LoginNotify+=v;
			else if(op==2) ROVoice._LoginNotify-=v;
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
	static public int set__LogoutNotify(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			XDSDKCallback v;
			int op=checkDelegate(l,2,out v);
			if(op==0) ROVoice._LogoutNotify=v;
			else if(op==1) ROVoice._LogoutNotify+=v;
			else if(op==2) ROVoice._LogoutNotify-=v;
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
		getTypeTable(l,"ROVoice");
		addMember(l,ChatSDKInit_s);
		addMember(l,ChatSDKLogin_s);
		addMember(l,Logout_s);
		addMember(l,ChatMic_s);
		addMember(l,SetPausePlayRealAudio_s);
		addMember(l,AudioToolsPlayAudio_s);
		addMember(l,AudioToolsStartRecord_s);
		addMember(l,AudioToolsStopRecord_s);
		addMember(l,AudioToolsPlayOnlineAudio_s);
		addMember(l,ReceiveTextMessageNotify_s);
		addMember(l,LoginNotify_s);
		addMember(l,LogoutNotify_s);
		addMember(l,"_SendRealTimeVoiceMessageErrorNotify",null,set__SendRealTimeVoiceMessageErrorNotify,false);
		addMember(l,"_ReceiveRealTimeVoiceMessageNofify",null,set__ReceiveRealTimeVoiceMessageNofify,false);
		addMember(l,"_ReceiveTextMessageNotify",null,set__ReceiveTextMessageNotify,false);
		addMember(l,"_RecorderMeteringPeakPowerNotify",null,set__RecorderMeteringPeakPowerNotify,false);
		addMember(l,"_PlayMeteringPeakPowerNotify",null,set__PlayMeteringPeakPowerNotify,false);
		addMember(l,"_AudioToolsRecorderMeteringPeakPowerNotify",null,set__AudioToolsRecorderMeteringPeakPowerNotify,false);
		addMember(l,"_AudioToolsPlayMeteringPeakPowerNotify",null,set__AudioToolsPlayMeteringPeakPowerNotify,false);
		addMember(l,"_MicStateNotify",null,set__MicStateNotify,false);
		addMember(l,"_onConnectFail",null,set__onConnectFail,false);
		addMember(l,"_onReconnectSuccess",null,set__onReconnectSuccess,false);
		addMember(l,"_LoginNotify",null,set__LoginNotify,false);
		addMember(l,"_LogoutNotify",null,set__LogoutNotify,false);
		createTypeMetatable(l,constructor, typeof(ROVoice));
	}
}
