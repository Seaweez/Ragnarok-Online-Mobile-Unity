using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Unity_Profiling_LowLevel_Unsafe_ProfilerUnsafeUtility : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public unsafe int CreateCategory_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Char* a1;
			checkType(l, 1, out a1);
			System.Int32 a2;
			checkType(l, 2, out a2);
			Unity.Profiling.ProfilerCategoryColor a3;
			checkEnum(l,3,out a3);
			var ret=Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.CreateCategory(a1,a2,a3);
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
	static public unsafe int GetCategoryByName_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Char* a1;
			checkType(l, 1, out a1);
			System.Int32 a2;
			checkType(l, 2, out a2);
			var ret=Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.GetCategoryByName(a1,a2);
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
	static public int GetCategoryDescription_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.UInt16 a1;
			checkType(l, 1, out a1);
			var ret=Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.GetCategoryDescription(a1);
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
	static public unsafe int CreateMarker_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==4){
				System.String a1;
				checkType(l, 1, out a1);
				System.UInt16 a2;
				checkType(l, 2, out a2);
				Unity.Profiling.LowLevel.MarkerFlags a3;
				checkEnum(l,3,out a3);
				System.Int32 a4;
				checkType(l, 4, out a4);
				var ret=Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.CreateMarker(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==5){
				System.Char* a1;
				checkType(l, 1, out a1);
				System.Int32 a2;
				checkType(l, 2, out a2);
				System.UInt16 a3;
				checkType(l, 3, out a3);
				Unity.Profiling.LowLevel.MarkerFlags a4;
				checkEnum(l,4,out a4);
				System.Int32 a5;
				checkType(l, 5, out a5);
				var ret=Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.CreateMarker(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function CreateMarker to call");
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
	static public unsafe int SetMarkerMetadata_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==5){
				System.IntPtr a1;
				checkType(l, 1, out a1);
				System.Int32 a2;
				checkType(l, 2, out a2);
				System.String a3;
				checkType(l, 3, out a3);
				System.Byte a4;
				checkType(l, 4, out a4);
				System.Byte a5;
				checkType(l, 5, out a5);
				Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.SetMarkerMetadata(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(argc==6){
				System.IntPtr a1;
				checkType(l, 1, out a1);
				System.Int32 a2;
				checkType(l, 2, out a2);
				System.Char* a3;
				checkType(l, 3, out a3);
				System.Int32 a4;
				checkType(l, 4, out a4);
				System.Byte a5;
				checkType(l, 5, out a5);
				System.Byte a6;
				checkType(l, 6, out a6);
				Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.SetMarkerMetadata(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetMarkerMetadata to call");
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
	static public int BeginSample_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.IntPtr a1;
			checkType(l, 1, out a1);
			Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.BeginSample(a1);
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
	static public unsafe int BeginSampleWithMetadata_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.IntPtr a1;
			checkType(l, 1, out a1);
			System.Int32 a2;
			checkType(l, 2, out a2);
			IntPtr a3; // ใช้ IntPtr แทน void*
			checkType(l, 3, out a3); // ตั้งสมมุติฐานว่า checkType มีรูปแบบที่รับ IntPtr
			Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.BeginSampleWithMetadata(a1, a2, a3.ToPointer()); // ใช้ ToPointer ถ้า BeginSampleWithMetadata ต้องการ void*
			pushValue(l, true);
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
	static public int EndSample_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.IntPtr a1;
			checkType(l, 1, out a1);
			Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.EndSample(a1);
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
	static public unsafe int SingleSampleWithMetadata_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			IntPtr a1;
			checkType(l, 1, out a1);
			Int32 a2;
			checkType(l, 2, out a2);
			IntPtr a3; // ใช้ IntPtr แทน void*
			checkType(l, 3, out a3); // ตั้งสมมุติฐานว่า checkType มีรูปแบบที่รับ IntPtr
			Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.SingleSampleWithMetadata(a1, a2, a3.ToPointer()); // ใช้ ToPointer() ถ้า SingleSampleWithMetadata ต้องการ void*
			pushValue(l, true);
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
	static public unsafe int CreateCounterValue_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==8){
				System.IntPtr a1;
				System.String a2;
				checkType(l, 2, out a2);
				System.UInt16 a3;
				checkType(l, 3, out a3);
				Unity.Profiling.LowLevel.MarkerFlags a4;
				checkEnum(l,4,out a4);
				System.Byte a5;
				checkType(l, 5, out a5);
				System.Byte a6;
				checkType(l, 6, out a6);
				System.Int32 a7;
				checkType(l, 7, out a7);
				Unity.Profiling.ProfilerCounterOptions a8;
				checkEnum(l,8,out a8);
				var ret=Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.CreateCounterValue(out a1,a2,a3,a4,a5,a6,a7,a8);
				pushValue(l,true);
				pushValue(l,ret);
				pushValue(l,a1);
				return 3;
			}
			else if(argc==9){
				System.IntPtr a1;
				System.Char* a2;
				checkType(l, 2, out a2);
				System.Int32 a3;
				checkType(l, 3, out a3);
				System.UInt16 a4;
				checkType(l, 4, out a4);
				Unity.Profiling.LowLevel.MarkerFlags a5;
				checkEnum(l,5,out a5);
				System.Byte a6;
				checkType(l, 6, out a6);
				System.Byte a7;
				checkType(l, 7, out a7);
				System.Int32 a8;
				checkType(l, 8, out a8);
				Unity.Profiling.ProfilerCounterOptions a9;
				checkEnum(l,9,out a9);
				var ret=Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.CreateCounterValue(out a1,a2,a3,a4,a5,a6,a7,a8,a9);
				pushValue(l,true);
				pushValue(l,ret);
				pushValue(l,a1);
				return 3;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function CreateCounterValue to call");
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
	static public unsafe int FlushCounterValue_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			IntPtr a1;
			checkType(l, 1, out a1);  // Assuming checkType has an overload for IntPtr
			Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.FlushCounterValue((void*)a1);
			pushValue(l, true);
			return 1;
		}
		catch (Exception e) {
			return error(l, e);
		}
		#if DEBUG
		finally {
			UnityEngine.Profiling.Profiler.EndSample();
		}
		#endif
	}


	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CreateFlow_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.UInt16 a1;
			checkType(l, 1, out a1);
			var ret=Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.CreateFlow(a1);
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
	static public int FlowEvent_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.UInt32 a1;
			checkType(l, 1, out a1);
			Unity.Profiling.ProfilerFlowEventType a2;
			checkEnum(l,2,out a2);
			Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.FlowEvent(a1,a2);
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
	static public int get_CategoryRender(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.CategoryRender);
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
	static public int get_CategoryScripts(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.CategoryScripts);
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
	static public int get_CategoryGUI(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.CategoryGUI);
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
	static public int get_CategoryPhysics(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.CategoryPhysics);
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
	static public int get_CategoryAnimation(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.CategoryAnimation);
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
	static public int get_CategoryAi(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.CategoryAi);
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
	static public int get_CategoryAudio(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.CategoryAudio);
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
	static public int get_CategoryVideo(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.CategoryVideo);
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
	static public int get_CategoryParticles(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.CategoryParticles);
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
	static public int get_CategoryLighting(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.CategoryLighting);
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
	static public int get_CategoryNetwork(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.CategoryNetwork);
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
	static public int get_CategoryLoading(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.CategoryLoading);
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
	static public int get_CategoryOther(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.CategoryOther);
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
	static public int get_CategoryVr(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.CategoryVr);
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
	static public int get_CategoryAllocation(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.CategoryAllocation);
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
	static public int get_CategoryInternal(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.CategoryInternal);
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
	static public int get_CategoryFileIO(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.CategoryFileIO);
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
	static public int get_CategoryInput(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.CategoryInput);
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
	static public int get_CategoryVirtualTexturing(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.CategoryVirtualTexturing);
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
	static public int get_Timestamp(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.Timestamp);
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
	static public int get_TimestampToNanosecondsConversionRatio(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility.TimestampToNanosecondsConversionRatio);
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
		getTypeTable(l,"Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility");
		addMember(l,CreateCategory_s);
		addMember(l,GetCategoryByName_s);
		addMember(l,GetCategoryDescription_s);
		addMember(l,CreateMarker_s);
		addMember(l,SetMarkerMetadata_s);
		addMember(l,BeginSample_s);
		addMember(l,BeginSampleWithMetadata_s);
		addMember(l,EndSample_s);
		addMember(l,SingleSampleWithMetadata_s);
		addMember(l,CreateCounterValue_s);
		addMember(l,FlushCounterValue_s);
		addMember(l,CreateFlow_s);
		addMember(l,FlowEvent_s);
		addMember(l,"CategoryRender",get_CategoryRender,null,false);
		addMember(l,"CategoryScripts",get_CategoryScripts,null,false);
		addMember(l,"CategoryGUI",get_CategoryGUI,null,false);
		addMember(l,"CategoryPhysics",get_CategoryPhysics,null,false);
		addMember(l,"CategoryAnimation",get_CategoryAnimation,null,false);
		addMember(l,"CategoryAi",get_CategoryAi,null,false);
		addMember(l,"CategoryAudio",get_CategoryAudio,null,false);
		addMember(l,"CategoryVideo",get_CategoryVideo,null,false);
		addMember(l,"CategoryParticles",get_CategoryParticles,null,false);
		addMember(l,"CategoryLighting",get_CategoryLighting,null,false);
		addMember(l,"CategoryNetwork",get_CategoryNetwork,null,false);
		addMember(l,"CategoryLoading",get_CategoryLoading,null,false);
		addMember(l,"CategoryOther",get_CategoryOther,null,false);
		addMember(l,"CategoryVr",get_CategoryVr,null,false);
		addMember(l,"CategoryAllocation",get_CategoryAllocation,null,false);
		addMember(l,"CategoryInternal",get_CategoryInternal,null,false);
		addMember(l,"CategoryFileIO",get_CategoryFileIO,null,false);
		addMember(l,"CategoryInput",get_CategoryInput,null,false);
		addMember(l,"CategoryVirtualTexturing",get_CategoryVirtualTexturing,null,false);
		addMember(l,"Timestamp",get_Timestamp,null,false);
		addMember(l,"TimestampToNanosecondsConversionRatio",get_TimestampToNanosecondsConversionRatio,null,false);
		createTypeMetatable(l,null, typeof(Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility));
	}
}
