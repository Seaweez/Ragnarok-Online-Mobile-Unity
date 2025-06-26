using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_Audio_AudioMixerGroup : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_audioMixer(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Audio.AudioMixerGroup self=(UnityEngine.Audio.AudioMixerGroup)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.audioMixer);
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
		getTypeTable(l,"UnityEngine.Audio.AudioMixerGroup");
		addMember(l,"audioMixer",get_audioMixer,null,true);
		createTypeMetatable(l,null, typeof(UnityEngine.Audio.AudioMixerGroup),typeof(UnityEngine.Object));
	}
}
