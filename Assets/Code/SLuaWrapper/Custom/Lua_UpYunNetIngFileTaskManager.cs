using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UpYunNetIngFileTaskManager : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Open(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UpYunNetIngFileTaskManager self=(UpYunNetIngFileTaskManager)checkSelf(l);
			self.Open();
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
	static public int Close(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UpYunNetIngFileTaskManager self=(UpYunNetIngFileTaskManager)checkSelf(l);
			self.Close();
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
	static public int Download(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UpYunNetIngFileTaskManager self=(UpYunNetIngFileTaskManager)checkSelf(l);
			System.String a1;
			checkType(l, 2, out a1);
			System.String a2;
			checkType(l, 3, out a2);
			System.Action a3;
			checkDelegate(l,4,out a3);
			System.Action<System.Single> a4;
			checkDelegate(l,5,out a4);
			System.Action a5;
			checkDelegate(l,6,out a5);
			System.Action<System.Int32,System.Int32,System.String> a6;
			checkDelegate(l,7,out a6);
			System.String[] a7;
			checkArray(l, 8, out a7);
			var ret=self.Download(a1,a2,a3,a4,a5,a6,a7);
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
	static public int Upload(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==8){
				UpYunNetIngFileTaskManager self=(UpYunNetIngFileTaskManager)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				System.Action a3;
				checkDelegate(l,4,out a3);
				System.Action<System.Single> a4;
				checkDelegate(l,5,out a4);
				System.Action a5;
				checkDelegate(l,6,out a5);
				System.Action<System.Int32,System.Int32,System.String> a6;
				checkDelegate(l,7,out a6);
				System.String[] a7;
				checkArray(l, 8, out a7);
				var ret=self.Upload(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==9){
				UpYunNetIngFileTaskManager self=(UpYunNetIngFileTaskManager)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				System.String a3;
				checkType(l, 4, out a3);
				System.Action a4;
				checkDelegate(l,5,out a4);
				System.Action<System.Single> a5;
				checkDelegate(l,6,out a5);
				System.Action a6;
				checkDelegate(l,7,out a6);
				System.Action<System.Int32,System.Int32,System.String> a7;
				checkDelegate(l,8,out a7);
				System.String[] a8;
				checkArray(l, 9, out a8);
				var ret=self.Upload(a1,a2,a3,a4,a5,a6,a7,a8);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function Upload to call");
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
	static public int GetDownloadTaskInfoFromID(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UpYunNetIngFileTaskManager self=(UpYunNetIngFileTaskManager)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			var ret=self.GetDownloadTaskInfoFromID(a1);
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
	static public int GetUploadTaskInfoFromID(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UpYunNetIngFileTaskManager self=(UpYunNetIngFileTaskManager)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			var ret=self.GetUploadTaskInfoFromID(a1);
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
	static public int GetBlocksUploadTaskInfoFromID(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UpYunNetIngFileTaskManager self=(UpYunNetIngFileTaskManager)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			var ret=self.GetBlocksUploadTaskInfoFromID(a1);
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
	static public int PauseTaskFromID(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UpYunNetIngFileTaskManager self=(UpYunNetIngFileTaskManager)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			self.PauseTaskFromID(a1);
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
	static public int ContinueTaskFromID(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UpYunNetIngFileTaskManager self=(UpYunNetIngFileTaskManager)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			self.ContinueTaskFromID(a1);
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
	static public int RemoveDownloadTaskFromID(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UpYunNetIngFileTaskManager self=(UpYunNetIngFileTaskManager)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			self.RemoveDownloadTaskFromID(a1);
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
	static public int RemoveUploadTaskFromID(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UpYunNetIngFileTaskManager self=(UpYunNetIngFileTaskManager)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			self.RemoveUploadTaskFromID(a1);
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
	static public int RemoveBlocksUploadTaskFromID(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UpYunNetIngFileTaskManager self=(UpYunNetIngFileTaskManager)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			self.RemoveBlocksUploadTaskFromID(a1);
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
	static public int ArrayToDictionary_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.String[] a1;
			checkArray(l, 1, out a1);
			var ret=UpYunNetIngFileTaskManager.ArrayToDictionary(a1);
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
	static public int get_Ins(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,UpYunNetIngFileTaskManager.Ins);
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
	static public int get_TasksInfo(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UpYunNetIngFileTaskManager self=(UpYunNetIngFileTaskManager)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.TasksInfo);
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
		getTypeTable(l,"UpYunNetIngFileTaskManager");
		addMember(l,Open);
		addMember(l,Close);
		addMember(l,Download);
		addMember(l,Upload);
		addMember(l,GetDownloadTaskInfoFromID);
		addMember(l,GetUploadTaskInfoFromID);
		addMember(l,GetBlocksUploadTaskInfoFromID);
		addMember(l,PauseTaskFromID);
		addMember(l,ContinueTaskFromID);
		addMember(l,RemoveDownloadTaskFromID);
		addMember(l,RemoveUploadTaskFromID);
		addMember(l,RemoveBlocksUploadTaskFromID);
		addMember(l,ArrayToDictionary_s);
		addMember(l,"Ins",get_Ins,null,false);
		addMember(l,"TasksInfo",get_TasksInfo,null,true);
		createTypeMetatable(l,null, typeof(UpYunNetIngFileTaskManager),typeof(MonoSingleton<UpYunNetIngFileTaskManager>));
	}
}
