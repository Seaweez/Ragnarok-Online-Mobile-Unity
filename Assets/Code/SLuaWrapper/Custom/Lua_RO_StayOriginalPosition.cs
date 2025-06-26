using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_StayOriginalPosition : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetPosition(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.StayOriginalPosition self=(RO.StayOriginalPosition)checkSelf(l);
			UnityEngine.Vector3 a1;
			checkType(l, 2, out a1);
			self.SetPosition(a1);
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
		getTypeTable(l,"RO.StayOriginalPosition");
		addMember(l,SetPosition);
		createTypeMetatable(l,null, typeof(RO.StayOriginalPosition),typeof(UnityEngine.MonoBehaviour));
	}
}
