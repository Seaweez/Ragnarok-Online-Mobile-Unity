using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_Rendering_BatchRendererGroup : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.BatchRendererGroup o;
			UnityEngine.Rendering.BatchRendererGroup.OnPerformCulling a1;
			checkDelegate(l,2,out a1);
			o=new UnityEngine.Rendering.BatchRendererGroup(a1);
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
	static public int Dispose(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.BatchRendererGroup self=(UnityEngine.Rendering.BatchRendererGroup)checkSelf(l);
			self.Dispose();
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
	static public int AddBatch(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.BatchRendererGroup self=(UnityEngine.Rendering.BatchRendererGroup)checkSelf(l);
			UnityEngine.Mesh a1;
			checkType(l, 2, out a1);
			System.Int32 a2;
			checkType(l, 3, out a2);
			UnityEngine.Material a3;
			checkType(l, 4, out a3);
			System.Int32 a4;
			checkType(l, 5, out a4);
			UnityEngine.Rendering.ShadowCastingMode a5;
			checkEnum(l,6,out a5);
			System.Boolean a6;
			checkType(l, 7, out a6);
			System.Boolean a7;
			checkType(l, 8, out a7);
			UnityEngine.Bounds a8;
			checkValueType(l, 9, out a8);
			System.Int32 a9;
			checkType(l, 10, out a9);
			UnityEngine.MaterialPropertyBlock a10;
			checkType(l, 11, out a10);
			UnityEngine.GameObject a11;
			checkType(l, 12, out a11);
			System.UInt64 a12;
			checkType(l, 13, out a12);
			System.UInt32 a13;
			checkType(l, 14, out a13);
			var ret=self.AddBatch(a1,a2,a3,a4,a5,a6,a7,a8,a9,a10,a11,a12,a13);
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
	static public int SetBatchFlags(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.BatchRendererGroup self=(UnityEngine.Rendering.BatchRendererGroup)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			System.UInt64 a2;
			checkType(l, 3, out a2);
			self.SetBatchFlags(a1,a2);
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
	static public int SetBatchPropertyMetadata(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.BatchRendererGroup self=(UnityEngine.Rendering.BatchRendererGroup)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			Unity.Collections.NativeArray<System.Int32> a2;
			checkValueType(l, 3, out a2);
			Unity.Collections.NativeArray<System.Int32> a3;
			checkValueType(l, 4, out a3);
			self.SetBatchPropertyMetadata(a1,a2,a3);
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
	static public int SetInstancingData(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.BatchRendererGroup self=(UnityEngine.Rendering.BatchRendererGroup)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			System.Int32 a2;
			checkType(l, 3, out a2);
			UnityEngine.MaterialPropertyBlock a3;
			checkType(l, 4, out a3);
			self.SetInstancingData(a1,a2,a3);
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
	static public int GetBatchMatrices(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.BatchRendererGroup self=(UnityEngine.Rendering.BatchRendererGroup)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			var ret=self.GetBatchMatrices(a1);
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
	static public int GetBatchScalarArrayInt(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int),typeof(string))){
				UnityEngine.Rendering.BatchRendererGroup self=(UnityEngine.Rendering.BatchRendererGroup)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				var ret=self.GetBatchScalarArrayInt(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int))){
				UnityEngine.Rendering.BatchRendererGroup self=(UnityEngine.Rendering.BatchRendererGroup)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				var ret=self.GetBatchScalarArrayInt(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function GetBatchScalarArrayInt to call");
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
	static public int GetBatchScalarArray(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int),typeof(string))){
				UnityEngine.Rendering.BatchRendererGroup self=(UnityEngine.Rendering.BatchRendererGroup)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				var ret=self.GetBatchScalarArray(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int))){
				UnityEngine.Rendering.BatchRendererGroup self=(UnityEngine.Rendering.BatchRendererGroup)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				var ret=self.GetBatchScalarArray(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function GetBatchScalarArray to call");
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
	static public int GetBatchVectorArrayInt(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int),typeof(string))){
				UnityEngine.Rendering.BatchRendererGroup self=(UnityEngine.Rendering.BatchRendererGroup)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				var ret=self.GetBatchVectorArrayInt(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int))){
				UnityEngine.Rendering.BatchRendererGroup self=(UnityEngine.Rendering.BatchRendererGroup)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				var ret=self.GetBatchVectorArrayInt(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function GetBatchVectorArrayInt to call");
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
	static public int GetBatchVectorArray(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int),typeof(string))){
				UnityEngine.Rendering.BatchRendererGroup self=(UnityEngine.Rendering.BatchRendererGroup)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				var ret=self.GetBatchVectorArray(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int))){
				UnityEngine.Rendering.BatchRendererGroup self=(UnityEngine.Rendering.BatchRendererGroup)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				var ret=self.GetBatchVectorArray(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function GetBatchVectorArray to call");
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
	static public int GetBatchMatrixArray(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int),typeof(string))){
				UnityEngine.Rendering.BatchRendererGroup self=(UnityEngine.Rendering.BatchRendererGroup)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				var ret=self.GetBatchMatrixArray(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int))){
				UnityEngine.Rendering.BatchRendererGroup self=(UnityEngine.Rendering.BatchRendererGroup)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				var ret=self.GetBatchMatrixArray(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function GetBatchMatrixArray to call");
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
	static public int SetBatchBounds(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.BatchRendererGroup self=(UnityEngine.Rendering.BatchRendererGroup)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			UnityEngine.Bounds a2;
			checkValueType(l, 3, out a2);
			self.SetBatchBounds(a1,a2);
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
	static public int GetNumBatches(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.BatchRendererGroup self=(UnityEngine.Rendering.BatchRendererGroup)checkSelf(l);
			var ret=self.GetNumBatches();
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
	static public int RemoveBatch(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.BatchRendererGroup self=(UnityEngine.Rendering.BatchRendererGroup)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			self.RemoveBatch(a1);
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
	static public int EnableVisibleIndicesYArray(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.BatchRendererGroup self=(UnityEngine.Rendering.BatchRendererGroup)checkSelf(l);
			System.Boolean a1;
			checkType(l, 2, out a1);
			self.EnableVisibleIndicesYArray(a1);
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
		getTypeTable(l,"UnityEngine.Rendering.BatchRendererGroup");
		addMember(l,Dispose);
		addMember(l,AddBatch);
		addMember(l,SetBatchFlags);
		addMember(l,SetBatchPropertyMetadata);
		addMember(l,SetInstancingData);
		addMember(l,GetBatchMatrices);
		addMember(l,GetBatchScalarArrayInt);
		addMember(l,GetBatchScalarArray);
		addMember(l,GetBatchVectorArrayInt);
		addMember(l,GetBatchVectorArray);
		addMember(l,GetBatchMatrixArray);
		addMember(l,SetBatchBounds);
		addMember(l,GetNumBatches);
		addMember(l,RemoveBatch);
		addMember(l,EnableVisibleIndicesYArray);
		createTypeMetatable(l,constructor, typeof(UnityEngine.Rendering.BatchRendererGroup));
	}
}
