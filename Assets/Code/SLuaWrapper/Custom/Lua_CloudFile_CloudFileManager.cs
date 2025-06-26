using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_CloudFile_CloudFileManager : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			CloudFile.CloudFileManager o;
			o=new CloudFile.CloudFileManager();
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
	static public int Open(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			CloudFile.CloudFileManager self=(CloudFile.CloudFileManager)checkSelf(l);
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
	static public int Download(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==5){
				CloudFile.CloudFileManager self=(CloudFile.CloudFileManager)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				System.Boolean a3;
				checkType(l, 4, out a3);
				System.String[] a4;
				checkArray(l, 5, out a4);
				var ret=self.Download(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==8){
				CloudFile.CloudFileManager self=(CloudFile.CloudFileManager)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				System.Boolean a3;
				checkType(l, 4, out a3);
				CloudFile.CloudFileCallback.ProgressCallback a4;
				checkDelegate(l,5,out a4);
				CloudFile.CloudFileCallback.SuccessCallback a5;
				checkDelegate(l,6,out a5);
				CloudFile.CloudFileCallback.ErrorCallback a6;
				checkDelegate(l,7,out a6);
				System.String[] a7;
				checkArray(l, 8, out a7);
				var ret=self.Download(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function Download to call");
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
	static public int NormalUpload(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==4){
				CloudFile.CloudFileManager self=(CloudFile.CloudFileManager)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				System.String[] a3;
				checkArray(l, 4, out a3);
				var ret=self.NormalUpload(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==7){
				CloudFile.CloudFileManager self=(CloudFile.CloudFileManager)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				CloudFile.CloudFileCallback.ProgressCallback a3;
				checkDelegate(l,4,out a3);
				CloudFile.CloudFileCallback.SuccessCallback a4;
				checkDelegate(l,5,out a4);
				CloudFile.CloudFileCallback.ErrorCallback a5;
				checkDelegate(l,6,out a5);
				System.String[] a6;
				checkArray(l, 7, out a6);
				var ret=self.NormalUpload(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function NormalUpload to call");
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
	static public int FormUpload(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==6){
				CloudFile.CloudFileManager self=(CloudFile.CloudFileManager)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				System.String a3;
				checkType(l, 4, out a3);
				System.String a4;
				checkType(l, 5, out a4);
				System.String[] a5;
				checkArray(l, 6, out a5);
				var ret=self.FormUpload(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==9){
				CloudFile.CloudFileManager self=(CloudFile.CloudFileManager)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				System.String a3;
				checkType(l, 4, out a3);
				System.String a4;
				checkType(l, 5, out a4);
				CloudFile.CloudFileCallback.ProgressCallback a5;
				checkDelegate(l,6,out a5);
				CloudFile.CloudFileCallback.SuccessCallback a6;
				checkDelegate(l,7,out a6);
				CloudFile.CloudFileCallback.ErrorCallback a7;
				checkDelegate(l,8,out a7);
				System.String[] a8;
				checkArray(l, 9, out a8);
				var ret=self.FormUpload(a1,a2,a3,a4,a5,a6,a7,a8);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function FormUpload to call");
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
	static public int StopTask(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			CloudFile.CloudFileManager self=(CloudFile.CloudFileManager)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			self.StopTask(a1);
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
	static public int RestartTask(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			CloudFile.CloudFileManager self=(CloudFile.CloudFileManager)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			self.RestartTask(a1);
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
			CloudFile.CloudFileManager self=(CloudFile.CloudFileManager)checkSelf(l);
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
	static public int ArrayToDictionary_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.String[] a1;
			checkArray(l, 1, out a1);
			var ret=CloudFile.CloudFileManager.ArrayToDictionary(a1);
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
			pushValue(l,CloudFile.CloudFileManager.Ins);
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
	static public int get__TaskRecordCenter(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			CloudFile.CloudFileManager self=(CloudFile.CloudFileManager)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self._TaskRecordCenter);
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
	static public int get__CloudFileCallbacks(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			CloudFile.CloudFileManager self=(CloudFile.CloudFileManager)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self._CloudFileCallbacks);
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
		getTypeTable(l,"CloudFile.CloudFileManager");
		addMember(l,Open);
		addMember(l,Download);
		addMember(l,NormalUpload);
		addMember(l,FormUpload);
		addMember(l,StopTask);
		addMember(l,RestartTask);
		addMember(l,Close);
		addMember(l,ArrayToDictionary_s);
		addMember(l,"Ins",get_Ins,null,false);
		addMember(l,"_TaskRecordCenter",get__TaskRecordCenter,null,true);
		addMember(l,"_CloudFileCallbacks",get__CloudFileCallbacks,null,true);
		createTypeMetatable(l,constructor, typeof(CloudFile.CloudFileManager),typeof(Singleton<CloudFile.CloudFileManager>));
	}
}
