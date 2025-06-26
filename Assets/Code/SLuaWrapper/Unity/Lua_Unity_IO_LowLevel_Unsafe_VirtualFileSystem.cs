using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Unity_IO_LowLevel_Unsafe_VirtualFileSystem : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetLocalFileSystemName_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.String a1;
			checkType(l, 1, out a1);
			System.String a2;
			System.UInt64 a3;
			System.UInt64 a4;
			var ret=Unity.IO.LowLevel.Unsafe.VirtualFileSystem.GetLocalFileSystemName(a1,out a2,out a3,out a4);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a2);
			pushValue(l,a3);
			pushValue(l,a4);
			return 5;
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
		getTypeTable(l,"Unity.IO.LowLevel.Unsafe.VirtualFileSystem");
		addMember(l,GetLocalFileSystemName_s);
		createTypeMetatable(l,null, typeof(Unity.IO.LowLevel.Unsafe.VirtualFileSystem));
	}
}
