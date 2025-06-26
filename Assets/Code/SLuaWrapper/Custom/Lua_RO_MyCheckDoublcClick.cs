using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_MyCheckDoublcClick : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_onDoubleClick(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.MyCheckDoublcClick self=(RO.MyCheckDoublcClick)checkSelf(l);
			UICamera.VoidDelegate v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self.onDoubleClick=v;
			else if(op==1) self.onDoubleClick+=v;
			else if(op==2) self.onDoubleClick-=v;
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
		getTypeTable(l,"RO.MyCheckDoublcClick");
		addMember(l,"onDoubleClick",null,set_onDoubleClick,true);
		createTypeMetatable(l,null, typeof(RO.MyCheckDoublcClick),typeof(UnityEngine.MonoBehaviour));
	}
}
