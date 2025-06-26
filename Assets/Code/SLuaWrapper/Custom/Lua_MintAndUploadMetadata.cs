using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_MintAndUploadMetadata : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int QueueTransaction(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			MintAndUploadMetadata self=(MintAndUploadMetadata)checkSelf(l);
			System.Func<System.Threading.Tasks.Task> a1;
			checkDelegate(l,2,out a1);
			self.QueueTransaction(a1);
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
	static public int Mint(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			MintAndUploadMetadata self=(MintAndUploadMetadata)checkSelf(l);
			System.String a1;
			checkType(l, 2, out a1);
			System.String a2;
			checkType(l, 3, out a2);
			SLua.LuaFunction a3;
			checkType(l, 4, out a3);
			self.Mint(a1,a2,a3);
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
		getTypeTable(l,"MintAndUploadMetadata");
		addMember(l,QueueTransaction);
		addMember(l,Mint);
		createTypeMetatable(l,null, typeof(MintAndUploadMetadata),typeof(UnityEngine.MonoBehaviour));
	}
}
