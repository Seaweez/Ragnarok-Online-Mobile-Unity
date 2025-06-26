using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Unity_IO_LowLevel_Unsafe_AsyncReadManager : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public unsafe int Read_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				Unity.IO.LowLevel.Unsafe.FileHandle a1;
				checkValueType(l, 1, out a1);
				Unity.IO.LowLevel.Unsafe.ReadCommandArray a2;
				checkValueType(l, 2, out a2);
				var ret=Unity.IO.LowLevel.Unsafe.AsyncReadManager.Read(in a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				pushValue(l,a1);
				return 3;
			}
			else if(argc==6){
				System.String a1;
				checkType(l, 1, out a1);

				// Handle the pointer type manually instead of using checkValueType
				Unity.IO.LowLevel.Unsafe.ReadCommand* a2 = (Unity.IO.LowLevel.Unsafe.ReadCommand*)LuaDLL.lua_touserdata(l, 2);

				System.UInt32 a3;
				checkType(l, 3, out a3);
				System.String a4;
				checkType(l, 4, out a4);
				System.UInt64 a5;
				checkType(l, 5, out a5);
				Unity.IO.LowLevel.Unsafe.AssetLoadingSubsystem a6;
				checkEnum(l, 6, out a6);
				var ret = Unity.IO.LowLevel.Unsafe.AsyncReadManager.Read(a1, a2, a3, a4, a5, a6);
				pushValue(l, true);
				pushValue(l, ret);
				return 2;
			}

			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function Read to call");
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
	static public unsafe int GetFileInfo_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.String a1;
			checkType(l, 1, out a1);
        
			// Don't use checkValueType for the pointer; retrieve it manually.
			Unity.IO.LowLevel.Unsafe.FileInfoResult* a2 = (Unity.IO.LowLevel.Unsafe.FileInfoResult*) LuaDLL.lua_touserdata(l, 2);

			var ret=Unity.IO.LowLevel.Unsafe.AsyncReadManager.GetFileInfo(a1, a2);
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
	static public unsafe int ReadDeferred_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			Unity.IO.LowLevel.Unsafe.FileHandle a1;
			checkValueType(l, 1, out a1);

			// Directly fetch the ReadCommandArray* from Lua stack
			if (LuaDLL.lua_type(l, 2) != LuaTypes.LUA_TUSERDATA) {
				// Handle error
				throw new Exception("Expected a user data at parameter 2.");
			}
			Unity.IO.LowLevel.Unsafe.ReadCommandArray* a2 = (Unity.IO.LowLevel.Unsafe.ReadCommandArray*)LuaDLL.lua_touserdata(l, 2);

			Unity.Jobs.JobHandle a3;
			checkValueType(l, 3, out a3);

			var ret = Unity.IO.LowLevel.Unsafe.AsyncReadManager.ReadDeferred(in a1, a2, a3);
        
			pushValue(l, true);
			pushValue(l, ret);
			pushValue(l, a1);

			return 3;
		}
		catch (Exception e) {
			// Handle the exception
			pushValue(l, false);
			pushValue(l, e.Message);
			return 2;
		}
	}

	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int OpenFileAsync_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.String a1;
			checkType(l, 1, out a1);
			var ret=Unity.IO.LowLevel.Unsafe.AsyncReadManager.OpenFileAsync(a1);
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
	static public int CloseCachedFileAsync_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.String a1;
			checkType(l, 1, out a1);
			Unity.Jobs.JobHandle a2;
			checkValueType(l, 2, out a2);
			var ret=Unity.IO.LowLevel.Unsafe.AsyncReadManager.CloseCachedFileAsync(a1,a2);
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"Unity.IO.LowLevel.Unsafe.AsyncReadManager");
		addMember(l,Read_s);
		addMember(l,GetFileInfo_s);
		addMember(l,ReadDeferred_s);
		addMember(l,OpenFileAsync_s);
		addMember(l,CloseCachedFileAsync_s);
		createTypeMetatable(l,null, typeof(Unity.IO.LowLevel.Unsafe.AsyncReadManager));
	}
}
