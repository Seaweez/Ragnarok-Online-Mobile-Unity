using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_Rendering_CommandBuffer : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer o;
			o=new UnityEngine.Rendering.CommandBuffer();
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
	static public int ConvertTexture(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Rendering.RenderTargetIdentifier a2;
				checkValueType(l, 3, out a2);
				self.ConvertTexture(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(argc==5){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier a1;
				checkValueType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.Rendering.RenderTargetIdentifier a3;
				checkValueType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				self.ConvertTexture(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function ConvertTexture to call");
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
	static public int WaitAllAsyncReadbackRequests(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			self.WaitAllAsyncReadbackRequests();
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
	static public int RequestAsyncReadback(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.ComputeBuffer),typeof(System.Action<UnityEngine.Rendering.AsyncGPUReadbackRequest>))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeBuffer a1;
				checkType(l, 2, out a1);
				System.Action<UnityEngine.Rendering.AsyncGPUReadbackRequest> a2;
				checkDelegate(l,3,out a2);
				self.RequestAsyncReadback(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.GraphicsBuffer),typeof(System.Action<UnityEngine.Rendering.AsyncGPUReadbackRequest>))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.GraphicsBuffer a1;
				checkType(l, 2, out a1);
				System.Action<UnityEngine.Rendering.AsyncGPUReadbackRequest> a2;
				checkDelegate(l,3,out a2);
				self.RequestAsyncReadback(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Texture),typeof(System.Action<UnityEngine.Rendering.AsyncGPUReadbackRequest>))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Texture a1;
				checkType(l, 2, out a1);
				System.Action<UnityEngine.Rendering.AsyncGPUReadbackRequest> a2;
				checkDelegate(l,3,out a2);
				self.RequestAsyncReadback(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(argc==4){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Texture a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Action<UnityEngine.Rendering.AsyncGPUReadbackRequest> a3;
				checkDelegate(l,4,out a3);
				self.RequestAsyncReadback(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeBuffer),typeof(int),typeof(int),typeof(System.Action<UnityEngine.Rendering.AsyncGPUReadbackRequest>))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeBuffer a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Action<UnityEngine.Rendering.AsyncGPUReadbackRequest> a4;
				checkDelegate(l,5,out a4);
				self.RequestAsyncReadback(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.GraphicsBuffer),typeof(int),typeof(int),typeof(System.Action<UnityEngine.Rendering.AsyncGPUReadbackRequest>))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.GraphicsBuffer a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Action<UnityEngine.Rendering.AsyncGPUReadbackRequest> a4;
				checkDelegate(l,5,out a4);
				self.RequestAsyncReadback(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Texture),typeof(int),typeof(UnityEngine.TextureFormat),typeof(System.Action<UnityEngine.Rendering.AsyncGPUReadbackRequest>))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Texture a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.TextureFormat a3;
				checkEnum(l,4,out a3);
				System.Action<UnityEngine.Rendering.AsyncGPUReadbackRequest> a4;
				checkDelegate(l,5,out a4);
				self.RequestAsyncReadback(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Texture),typeof(int),typeof(UnityEngine.Experimental.Rendering.GraphicsFormat),typeof(System.Action<UnityEngine.Rendering.AsyncGPUReadbackRequest>))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Texture a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.Experimental.Rendering.GraphicsFormat a3;
				checkEnum(l,4,out a3);
				System.Action<UnityEngine.Rendering.AsyncGPUReadbackRequest> a4;
				checkDelegate(l,5,out a4);
				self.RequestAsyncReadback(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(argc==10){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Texture a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				System.Int32 a6;
				checkType(l, 7, out a6);
				System.Int32 a7;
				checkType(l, 8, out a7);
				System.Int32 a8;
				checkType(l, 9, out a8);
				System.Action<UnityEngine.Rendering.AsyncGPUReadbackRequest> a9;
				checkDelegate(l,10,out a9);
				self.RequestAsyncReadback(a1,a2,a3,a4,a5,a6,a7,a8,a9);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Texture),typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(UnityEngine.TextureFormat),typeof(System.Action<UnityEngine.Rendering.AsyncGPUReadbackRequest>))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Texture a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				System.Int32 a6;
				checkType(l, 7, out a6);
				System.Int32 a7;
				checkType(l, 8, out a7);
				System.Int32 a8;
				checkType(l, 9, out a8);
				UnityEngine.TextureFormat a9;
				checkEnum(l,10,out a9);
				System.Action<UnityEngine.Rendering.AsyncGPUReadbackRequest> a10;
				checkDelegate(l,11,out a10);
				self.RequestAsyncReadback(a1,a2,a3,a4,a5,a6,a7,a8,a9,a10);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Texture),typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(UnityEngine.Experimental.Rendering.GraphicsFormat),typeof(System.Action<UnityEngine.Rendering.AsyncGPUReadbackRequest>))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Texture a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				System.Int32 a6;
				checkType(l, 7, out a6);
				System.Int32 a7;
				checkType(l, 8, out a7);
				System.Int32 a8;
				checkType(l, 9, out a8);
				UnityEngine.Experimental.Rendering.GraphicsFormat a9;
				checkEnum(l,10,out a9);
				System.Action<UnityEngine.Rendering.AsyncGPUReadbackRequest> a10;
				checkDelegate(l,11,out a10);
				self.RequestAsyncReadback(a1,a2,a3,a4,a5,a6,a7,a8,a9,a10);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function RequestAsyncReadback to call");
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
	static public int SetInvertCulling(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			System.Boolean a1;
			checkType(l, 2, out a1);
			self.SetInvertCulling(a1);
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
	static public int SetComputeFloatParam(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(int),typeof(float))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Single a3;
				checkType(l, 4, out a3);
				self.SetComputeFloatParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(string),typeof(float))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				System.Single a3;
				checkType(l, 4, out a3);
				self.SetComputeFloatParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetComputeFloatParam to call");
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
	static public int SetComputeIntParam(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(int),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				self.SetComputeIntParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(string),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				self.SetComputeIntParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetComputeIntParam to call");
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
	static public int SetComputeVectorParam(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(int),typeof(UnityEngine.Vector4))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.Vector4 a3;
				checkType(l, 4, out a3);
				self.SetComputeVectorParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(string),typeof(UnityEngine.Vector4))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				UnityEngine.Vector4 a3;
				checkType(l, 4, out a3);
				self.SetComputeVectorParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetComputeVectorParam to call");
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
	static public int SetComputeVectorArrayParam(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(int),typeof(UnityEngine.Vector4[]))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.Vector4[] a3;
				checkArray(l, 4, out a3);
				self.SetComputeVectorArrayParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(string),typeof(UnityEngine.Vector4[]))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				UnityEngine.Vector4[] a3;
				checkArray(l, 4, out a3);
				self.SetComputeVectorArrayParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetComputeVectorArrayParam to call");
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
	static public int SetComputeMatrixParam(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(int),typeof(UnityEngine.Matrix4x4))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.Matrix4x4 a3;
				checkValueType(l, 4, out a3);
				self.SetComputeMatrixParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(string),typeof(UnityEngine.Matrix4x4))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				UnityEngine.Matrix4x4 a3;
				checkValueType(l, 4, out a3);
				self.SetComputeMatrixParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetComputeMatrixParam to call");
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
	static public int SetComputeMatrixArrayParam(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(int),typeof(UnityEngine.Matrix4x4[]))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.Matrix4x4[] a3;
				checkArray(l, 4, out a3);
				self.SetComputeMatrixArrayParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(string),typeof(UnityEngine.Matrix4x4[]))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				UnityEngine.Matrix4x4[] a3;
				checkArray(l, 4, out a3);
				self.SetComputeMatrixArrayParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetComputeMatrixArrayParam to call");
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
	static public int SetRayTracingShaderPass(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			UnityEngine.Experimental.Rendering.RayTracingShader a1;
			checkType(l, 2, out a1);
			System.String a2;
			checkType(l, 3, out a2);
			self.SetRayTracingShaderPass(a1,a2);
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
	static public int Clear(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			self.Clear();
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
	static public int ClearRandomWriteTargets(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			self.ClearRandomWriteTargets();
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
	static public int SetViewport(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			UnityEngine.Rect a1;
			checkValueType(l, 2, out a1);
			self.SetViewport(a1);
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
	static public int EnableScissorRect(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			UnityEngine.Rect a1;
			checkValueType(l, 2, out a1);
			self.EnableScissorRect(a1);
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
	static public int DisableScissorRect(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			self.DisableScissorRect();
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
	static public int GetTemporaryRT(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				UnityEngine.RenderTextureDescriptor a2;
				checkValueType(l, 3, out a2);
				self.GetTemporaryRT(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				self.GetTemporaryRT(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(UnityEngine.RenderTextureDescriptor),typeof(UnityEngine.FilterMode))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				UnityEngine.RenderTextureDescriptor a2;
				checkValueType(l, 3, out a2);
				UnityEngine.FilterMode a3;
				checkEnum(l,4,out a3);
				self.GetTemporaryRT(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(argc==5){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				self.GetTemporaryRT(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(argc==6){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.FilterMode a5;
				checkEnum(l,6,out a5);
				self.GetTemporaryRT(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(int),typeof(int),typeof(UnityEngine.FilterMode),typeof(UnityEngine.Experimental.Rendering.GraphicsFormat))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.FilterMode a5;
				checkEnum(l,6,out a5);
				UnityEngine.Experimental.Rendering.GraphicsFormat a6;
				checkEnum(l,7,out a6);
				self.GetTemporaryRT(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(int),typeof(int),typeof(UnityEngine.FilterMode),typeof(UnityEngine.RenderTextureFormat))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.FilterMode a5;
				checkEnum(l,6,out a5);
				UnityEngine.RenderTextureFormat a6;
				checkEnum(l,7,out a6);
				self.GetTemporaryRT(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(int),typeof(int),typeof(UnityEngine.FilterMode),typeof(UnityEngine.Experimental.Rendering.GraphicsFormat),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.FilterMode a5;
				checkEnum(l,6,out a5);
				UnityEngine.Experimental.Rendering.GraphicsFormat a6;
				checkEnum(l,7,out a6);
				System.Int32 a7;
				checkType(l, 8, out a7);
				self.GetTemporaryRT(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(int),typeof(int),typeof(UnityEngine.FilterMode),typeof(UnityEngine.RenderTextureFormat),typeof(UnityEngine.RenderTextureReadWrite))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.FilterMode a5;
				checkEnum(l,6,out a5);
				UnityEngine.RenderTextureFormat a6;
				checkEnum(l,7,out a6);
				UnityEngine.RenderTextureReadWrite a7;
				checkEnum(l,8,out a7);
				self.GetTemporaryRT(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(int),typeof(int),typeof(UnityEngine.FilterMode),typeof(UnityEngine.Experimental.Rendering.GraphicsFormat),typeof(int),typeof(bool))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.FilterMode a5;
				checkEnum(l,6,out a5);
				UnityEngine.Experimental.Rendering.GraphicsFormat a6;
				checkEnum(l,7,out a6);
				System.Int32 a7;
				checkType(l, 8, out a7);
				System.Boolean a8;
				checkType(l, 9, out a8);
				self.GetTemporaryRT(a1,a2,a3,a4,a5,a6,a7,a8);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(int),typeof(int),typeof(UnityEngine.FilterMode),typeof(UnityEngine.RenderTextureFormat),typeof(UnityEngine.RenderTextureReadWrite),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.FilterMode a5;
				checkEnum(l,6,out a5);
				UnityEngine.RenderTextureFormat a6;
				checkEnum(l,7,out a6);
				UnityEngine.RenderTextureReadWrite a7;
				checkEnum(l,8,out a7);
				System.Int32 a8;
				checkType(l, 9, out a8);
				self.GetTemporaryRT(a1,a2,a3,a4,a5,a6,a7,a8);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(int),typeof(int),typeof(UnityEngine.FilterMode),typeof(UnityEngine.Experimental.Rendering.GraphicsFormat),typeof(int),typeof(bool),typeof(UnityEngine.RenderTextureMemoryless))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.FilterMode a5;
				checkEnum(l,6,out a5);
				UnityEngine.Experimental.Rendering.GraphicsFormat a6;
				checkEnum(l,7,out a6);
				System.Int32 a7;
				checkType(l, 8, out a7);
				System.Boolean a8;
				checkType(l, 9, out a8);
				UnityEngine.RenderTextureMemoryless a9;
				checkEnum(l,10,out a9);
				self.GetTemporaryRT(a1,a2,a3,a4,a5,a6,a7,a8,a9);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(int),typeof(int),typeof(UnityEngine.FilterMode),typeof(UnityEngine.RenderTextureFormat),typeof(UnityEngine.RenderTextureReadWrite),typeof(int),typeof(bool))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.FilterMode a5;
				checkEnum(l,6,out a5);
				UnityEngine.RenderTextureFormat a6;
				checkEnum(l,7,out a6);
				UnityEngine.RenderTextureReadWrite a7;
				checkEnum(l,8,out a7);
				System.Int32 a8;
				checkType(l, 9, out a8);
				System.Boolean a9;
				checkType(l, 10, out a9);
				self.GetTemporaryRT(a1,a2,a3,a4,a5,a6,a7,a8,a9);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(int),typeof(int),typeof(UnityEngine.FilterMode),typeof(UnityEngine.Experimental.Rendering.GraphicsFormat),typeof(int),typeof(bool),typeof(UnityEngine.RenderTextureMemoryless),typeof(bool))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.FilterMode a5;
				checkEnum(l,6,out a5);
				UnityEngine.Experimental.Rendering.GraphicsFormat a6;
				checkEnum(l,7,out a6);
				System.Int32 a7;
				checkType(l, 8, out a7);
				System.Boolean a8;
				checkType(l, 9, out a8);
				UnityEngine.RenderTextureMemoryless a9;
				checkEnum(l,10,out a9);
				System.Boolean a10;
				checkType(l, 11, out a10);
				self.GetTemporaryRT(a1,a2,a3,a4,a5,a6,a7,a8,a9,a10);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(int),typeof(int),typeof(UnityEngine.FilterMode),typeof(UnityEngine.RenderTextureFormat),typeof(UnityEngine.RenderTextureReadWrite),typeof(int),typeof(bool),typeof(UnityEngine.RenderTextureMemoryless))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.FilterMode a5;
				checkEnum(l,6,out a5);
				UnityEngine.RenderTextureFormat a6;
				checkEnum(l,7,out a6);
				UnityEngine.RenderTextureReadWrite a7;
				checkEnum(l,8,out a7);
				System.Int32 a8;
				checkType(l, 9, out a8);
				System.Boolean a9;
				checkType(l, 10, out a9);
				UnityEngine.RenderTextureMemoryless a10;
				checkEnum(l,11,out a10);
				self.GetTemporaryRT(a1,a2,a3,a4,a5,a6,a7,a8,a9,a10);
				pushValue(l,true);
				return 1;
			}
			else if(argc==12){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.FilterMode a5;
				checkEnum(l,6,out a5);
				UnityEngine.RenderTextureFormat a6;
				checkEnum(l,7,out a6);
				UnityEngine.RenderTextureReadWrite a7;
				checkEnum(l,8,out a7);
				System.Int32 a8;
				checkType(l, 9, out a8);
				System.Boolean a9;
				checkType(l, 10, out a9);
				UnityEngine.RenderTextureMemoryless a10;
				checkEnum(l,11,out a10);
				System.Boolean a11;
				checkType(l, 12, out a11);
				self.GetTemporaryRT(a1,a2,a3,a4,a5,a6,a7,a8,a9,a10,a11);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function GetTemporaryRT to call");
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
	static public int GetTemporaryRTArray(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==5){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				self.GetTemporaryRTArray(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(argc==6){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				self.GetTemporaryRTArray(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(argc==7){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				UnityEngine.FilterMode a6;
				checkEnum(l,7,out a6);
				self.GetTemporaryRTArray(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(UnityEngine.FilterMode),typeof(UnityEngine.Experimental.Rendering.GraphicsFormat))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				UnityEngine.FilterMode a6;
				checkEnum(l,7,out a6);
				UnityEngine.Experimental.Rendering.GraphicsFormat a7;
				checkEnum(l,8,out a7);
				self.GetTemporaryRTArray(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(UnityEngine.FilterMode),typeof(UnityEngine.RenderTextureFormat))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				UnityEngine.FilterMode a6;
				checkEnum(l,7,out a6);
				UnityEngine.RenderTextureFormat a7;
				checkEnum(l,8,out a7);
				self.GetTemporaryRTArray(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(UnityEngine.FilterMode),typeof(UnityEngine.Experimental.Rendering.GraphicsFormat),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				UnityEngine.FilterMode a6;
				checkEnum(l,7,out a6);
				UnityEngine.Experimental.Rendering.GraphicsFormat a7;
				checkEnum(l,8,out a7);
				System.Int32 a8;
				checkType(l, 9, out a8);
				self.GetTemporaryRTArray(a1,a2,a3,a4,a5,a6,a7,a8);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(UnityEngine.FilterMode),typeof(UnityEngine.RenderTextureFormat),typeof(UnityEngine.RenderTextureReadWrite))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				UnityEngine.FilterMode a6;
				checkEnum(l,7,out a6);
				UnityEngine.RenderTextureFormat a7;
				checkEnum(l,8,out a7);
				UnityEngine.RenderTextureReadWrite a8;
				checkEnum(l,9,out a8);
				self.GetTemporaryRTArray(a1,a2,a3,a4,a5,a6,a7,a8);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(UnityEngine.FilterMode),typeof(UnityEngine.Experimental.Rendering.GraphicsFormat),typeof(int),typeof(bool))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				UnityEngine.FilterMode a6;
				checkEnum(l,7,out a6);
				UnityEngine.Experimental.Rendering.GraphicsFormat a7;
				checkEnum(l,8,out a7);
				System.Int32 a8;
				checkType(l, 9, out a8);
				System.Boolean a9;
				checkType(l, 10, out a9);
				self.GetTemporaryRTArray(a1,a2,a3,a4,a5,a6,a7,a8,a9);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(UnityEngine.FilterMode),typeof(UnityEngine.RenderTextureFormat),typeof(UnityEngine.RenderTextureReadWrite),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				UnityEngine.FilterMode a6;
				checkEnum(l,7,out a6);
				UnityEngine.RenderTextureFormat a7;
				checkEnum(l,8,out a7);
				UnityEngine.RenderTextureReadWrite a8;
				checkEnum(l,9,out a8);
				System.Int32 a9;
				checkType(l, 10, out a9);
				self.GetTemporaryRTArray(a1,a2,a3,a4,a5,a6,a7,a8,a9);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(UnityEngine.FilterMode),typeof(UnityEngine.Experimental.Rendering.GraphicsFormat),typeof(int),typeof(bool),typeof(bool))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				UnityEngine.FilterMode a6;
				checkEnum(l,7,out a6);
				UnityEngine.Experimental.Rendering.GraphicsFormat a7;
				checkEnum(l,8,out a7);
				System.Int32 a8;
				checkType(l, 9, out a8);
				System.Boolean a9;
				checkType(l, 10, out a9);
				System.Boolean a10;
				checkType(l, 11, out a10);
				self.GetTemporaryRTArray(a1,a2,a3,a4,a5,a6,a7,a8,a9,a10);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(UnityEngine.FilterMode),typeof(UnityEngine.RenderTextureFormat),typeof(UnityEngine.RenderTextureReadWrite),typeof(int),typeof(bool))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				UnityEngine.FilterMode a6;
				checkEnum(l,7,out a6);
				UnityEngine.RenderTextureFormat a7;
				checkEnum(l,8,out a7);
				UnityEngine.RenderTextureReadWrite a8;
				checkEnum(l,9,out a8);
				System.Int32 a9;
				checkType(l, 10, out a9);
				System.Boolean a10;
				checkType(l, 11, out a10);
				self.GetTemporaryRTArray(a1,a2,a3,a4,a5,a6,a7,a8,a9,a10);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function GetTemporaryRTArray to call");
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
	static public int ReleaseTemporaryRT(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			self.ReleaseTemporaryRT(a1);
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
	static public int ClearRenderTarget(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==4){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Boolean a1;
				checkType(l, 2, out a1);
				System.Boolean a2;
				checkType(l, 3, out a2);
				UnityEngine.Color a3;
				checkType(l, 4, out a3);
				self.ClearRenderTarget(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rendering.RTClearFlags),typeof(UnityEngine.Color),typeof(float),typeof(System.UInt32))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RTClearFlags a1;
				checkEnum(l,2,out a1);
				UnityEngine.Color a2;
				checkType(l, 3, out a2);
				System.Single a3;
				checkType(l, 4, out a3);
				System.UInt32 a4;
				checkType(l, 5, out a4);
				self.ClearRenderTarget(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(bool),typeof(bool),typeof(UnityEngine.Color),typeof(float))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Boolean a1;
				checkType(l, 2, out a1);
				System.Boolean a2;
				checkType(l, 3, out a2);
				UnityEngine.Color a3;
				checkType(l, 4, out a3);
				System.Single a4;
				checkType(l, 5, out a4);
				self.ClearRenderTarget(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function ClearRenderTarget to call");
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
	static public int SetGlobalFloat(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int),typeof(float))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Single a2;
				checkType(l, 3, out a2);
				self.SetGlobalFloat(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(float))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.Single a2;
				checkType(l, 3, out a2);
				self.SetGlobalFloat(a1,a2);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetGlobalFloat to call");
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
	static public int SetGlobalInt(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				self.SetGlobalInt(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				self.SetGlobalInt(a1,a2);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetGlobalInt to call");
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
	static public int SetGlobalInteger(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				self.SetGlobalInteger(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				self.SetGlobalInteger(a1,a2);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetGlobalInteger to call");
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
	static public int SetGlobalVector(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int),typeof(UnityEngine.Vector4))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				UnityEngine.Vector4 a2;
				checkType(l, 3, out a2);
				self.SetGlobalVector(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(UnityEngine.Vector4))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				UnityEngine.Vector4 a2;
				checkType(l, 3, out a2);
				self.SetGlobalVector(a1,a2);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetGlobalVector to call");
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
	static public int SetGlobalColor(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int),typeof(UnityEngine.Color))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				UnityEngine.Color a2;
				checkType(l, 3, out a2);
				self.SetGlobalColor(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(UnityEngine.Color))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				UnityEngine.Color a2;
				checkType(l, 3, out a2);
				self.SetGlobalColor(a1,a2);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetGlobalColor to call");
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
	static public int SetGlobalMatrix(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int),typeof(UnityEngine.Matrix4x4))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				UnityEngine.Matrix4x4 a2;
				checkValueType(l, 3, out a2);
				self.SetGlobalMatrix(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(UnityEngine.Matrix4x4))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				UnityEngine.Matrix4x4 a2;
				checkValueType(l, 3, out a2);
				self.SetGlobalMatrix(a1,a2);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetGlobalMatrix to call");
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
	static public int EnableShaderKeyword(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			System.String a1;
			checkType(l, 2, out a1);
			self.EnableShaderKeyword(a1);
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
	static public int EnableKeyword(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.GlobalKeyword a1;
				checkValueType(l, 2, out a1);
				self.EnableKeyword(in a1);
				pushValue(l,true);
				pushValue(l,a1);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Material),typeof(UnityEngine.Rendering.LocalKeyword))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Material a1;
				checkType(l, 2, out a1);
				UnityEngine.Rendering.LocalKeyword a2;
				checkValueType(l, 3, out a2);
				self.EnableKeyword(a1,in a2);
				pushValue(l,true);
				pushValue(l,a2);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(UnityEngine.Rendering.LocalKeyword))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				UnityEngine.Rendering.LocalKeyword a2;
				checkValueType(l, 3, out a2);
				self.EnableKeyword(a1,in a2);
				pushValue(l,true);
				pushValue(l,a2);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function EnableKeyword to call");
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
	static public int DisableShaderKeyword(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			System.String a1;
			checkType(l, 2, out a1);
			self.DisableShaderKeyword(a1);
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
	static public int DisableKeyword(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.GlobalKeyword a1;
				checkValueType(l, 2, out a1);
				self.DisableKeyword(in a1);
				pushValue(l,true);
				pushValue(l,a1);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Material),typeof(UnityEngine.Rendering.LocalKeyword))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Material a1;
				checkType(l, 2, out a1);
				UnityEngine.Rendering.LocalKeyword a2;
				checkValueType(l, 3, out a2);
				self.DisableKeyword(a1,in a2);
				pushValue(l,true);
				pushValue(l,a2);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(UnityEngine.Rendering.LocalKeyword))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				UnityEngine.Rendering.LocalKeyword a2;
				checkValueType(l, 3, out a2);
				self.DisableKeyword(a1,in a2);
				pushValue(l,true);
				pushValue(l,a2);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function DisableKeyword to call");
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
	static public int SetKeyword(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.GlobalKeyword a1;
				checkValueType(l, 2, out a1);
				System.Boolean a2;
				checkType(l, 3, out a2);
				self.SetKeyword(in a1,a2);
				pushValue(l,true);
				pushValue(l,a1);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Material),typeof(UnityEngine.Rendering.LocalKeyword),typeof(bool))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Material a1;
				checkType(l, 2, out a1);
				UnityEngine.Rendering.LocalKeyword a2;
				checkValueType(l, 3, out a2);
				System.Boolean a3;
				checkType(l, 4, out a3);
				self.SetKeyword(a1,in a2,a3);
				pushValue(l,true);
				pushValue(l,a2);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(UnityEngine.Rendering.LocalKeyword),typeof(bool))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				UnityEngine.Rendering.LocalKeyword a2;
				checkValueType(l, 3, out a2);
				System.Boolean a3;
				checkType(l, 4, out a3);
				self.SetKeyword(a1,in a2,a3);
				pushValue(l,true);
				pushValue(l,a2);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetKeyword to call");
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
	static public int SetViewMatrix(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			UnityEngine.Matrix4x4 a1;
			checkValueType(l, 2, out a1);
			self.SetViewMatrix(a1);
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
	static public int SetProjectionMatrix(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			UnityEngine.Matrix4x4 a1;
			checkValueType(l, 2, out a1);
			self.SetProjectionMatrix(a1);
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
	static public int SetViewProjectionMatrices(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			UnityEngine.Matrix4x4 a1;
			checkValueType(l, 2, out a1);
			UnityEngine.Matrix4x4 a2;
			checkValueType(l, 3, out a2);
			self.SetViewProjectionMatrices(a1,a2);
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
	static public int SetGlobalDepthBias(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			System.Single a1;
			checkType(l, 2, out a1);
			System.Single a2;
			checkType(l, 3, out a2);
			self.SetGlobalDepthBias(a1,a2);
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
	static public int SetExecutionFlags(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			UnityEngine.Rendering.CommandBufferExecutionFlags a1;
			checkEnum(l,2,out a1);
			self.SetExecutionFlags(a1);
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
	static public int SetGlobalFloatArray(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int),typeof(System.Single[]))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Single[] a2;
				checkArray(l, 3, out a2);
				self.SetGlobalFloatArray(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(List<System.Single>))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.Collections.Generic.List<System.Single> a2;
				checkType(l, 3, out a2);
				self.SetGlobalFloatArray(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(List<System.Single>))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Collections.Generic.List<System.Single> a2;
				checkType(l, 3, out a2);
				self.SetGlobalFloatArray(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(System.Single[]))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.Single[] a2;
				checkArray(l, 3, out a2);
				self.SetGlobalFloatArray(a1,a2);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetGlobalFloatArray to call");
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
	static public int SetGlobalVectorArray(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int),typeof(UnityEngine.Vector4[]))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				UnityEngine.Vector4[] a2;
				checkArray(l, 3, out a2);
				self.SetGlobalVectorArray(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(List<UnityEngine.Vector4>))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.Collections.Generic.List<UnityEngine.Vector4> a2;
				checkType(l, 3, out a2);
				self.SetGlobalVectorArray(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(List<UnityEngine.Vector4>))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Collections.Generic.List<UnityEngine.Vector4> a2;
				checkType(l, 3, out a2);
				self.SetGlobalVectorArray(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(UnityEngine.Vector4[]))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				UnityEngine.Vector4[] a2;
				checkArray(l, 3, out a2);
				self.SetGlobalVectorArray(a1,a2);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetGlobalVectorArray to call");
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
	static public int SetGlobalMatrixArray(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int),typeof(UnityEngine.Matrix4x4[]))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				UnityEngine.Matrix4x4[] a2;
				checkArray(l, 3, out a2);
				self.SetGlobalMatrixArray(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(List<UnityEngine.Matrix4x4>))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.Collections.Generic.List<UnityEngine.Matrix4x4> a2;
				checkType(l, 3, out a2);
				self.SetGlobalMatrixArray(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(List<UnityEngine.Matrix4x4>))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				System.Collections.Generic.List<UnityEngine.Matrix4x4> a2;
				checkType(l, 3, out a2);
				self.SetGlobalMatrixArray(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(UnityEngine.Matrix4x4[]))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				UnityEngine.Matrix4x4[] a2;
				checkArray(l, 3, out a2);
				self.SetGlobalMatrixArray(a1,a2);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetGlobalMatrixArray to call");
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
	static public int SetLateLatchProjectionMatrices(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			UnityEngine.Matrix4x4[] a1;
			checkArray(l, 2, out a1);
			self.SetLateLatchProjectionMatrices(a1);
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
	static public int MarkLateLatchMatrixShaderPropertyID(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			UnityEngine.Rendering.CameraLateLatchMatrixType a1;
			checkEnum(l,2,out a1);
			System.Int32 a2;
			checkType(l, 3, out a2);
			self.MarkLateLatchMatrixShaderPropertyID(a1,a2);
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
	static public int UnmarkLateLatchMatrix(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			UnityEngine.Rendering.CameraLateLatchMatrixType a1;
			checkEnum(l,2,out a1);
			self.UnmarkLateLatchMatrix(a1);
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
	static public int BeginSample(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(string))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				self.BeginSample(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Profiling.CustomSampler))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Profiling.CustomSampler a1;
				checkType(l, 2, out a1);
				self.BeginSample(a1);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function BeginSample to call");
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
	static public int EndSample(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(string))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				self.EndSample(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Profiling.CustomSampler))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Profiling.CustomSampler a1;
				checkType(l, 2, out a1);
				self.EndSample(a1);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function EndSample to call");
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
	static public int IncrementUpdateCount(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			UnityEngine.Rendering.RenderTargetIdentifier a1;
			checkValueType(l, 2, out a1);
			self.IncrementUpdateCount(a1);
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
	static public int SetInstanceMultiplier(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			System.UInt32 a1;
			checkType(l, 2, out a1);
			self.SetInstanceMultiplier(a1);
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
	static public int SetRenderTarget(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.Rendering.RenderTargetIdentifier))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier a1;
				checkValueType(l, 2, out a1);
				self.SetRenderTarget(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rendering.RenderTargetBinding))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetBinding a1;
				checkValueType(l, 2, out a1);
				self.SetRenderTarget(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier a1;
				checkValueType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				self.SetRenderTarget(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(UnityEngine.Rendering.RenderTargetIdentifier))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Rendering.RenderTargetIdentifier a2;
				checkValueType(l, 3, out a2);
				self.SetRenderTarget(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rendering.RenderTargetIdentifier[]),typeof(UnityEngine.Rendering.RenderTargetIdentifier))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier[] a1;
				checkArray(l, 2, out a1);
				UnityEngine.Rendering.RenderTargetIdentifier a2;
				checkValueType(l, 3, out a2);
				self.SetRenderTarget(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(UnityEngine.Rendering.RenderBufferLoadAction),typeof(UnityEngine.Rendering.RenderBufferStoreAction))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Rendering.RenderBufferLoadAction a2;
				checkEnum(l,3,out a2);
				UnityEngine.Rendering.RenderBufferStoreAction a3;
				checkEnum(l,4,out a3);
				self.SetRenderTarget(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(int),typeof(UnityEngine.CubemapFace))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier a1;
				checkValueType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.CubemapFace a3;
				checkEnum(l,4,out a3);
				self.SetRenderTarget(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Rendering.RenderTargetIdentifier a2;
				checkValueType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				self.SetRenderTarget(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(int),typeof(UnityEngine.CubemapFace),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier a1;
				checkValueType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.CubemapFace a3;
				checkEnum(l,4,out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				self.SetRenderTarget(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(int),typeof(UnityEngine.CubemapFace))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Rendering.RenderTargetIdentifier a2;
				checkValueType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				UnityEngine.CubemapFace a4;
				checkEnum(l,5,out a4);
				self.SetRenderTarget(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rendering.RenderTargetBinding),typeof(int),typeof(UnityEngine.CubemapFace),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetBinding a1;
				checkValueType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.CubemapFace a3;
				checkEnum(l,4,out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				self.SetRenderTarget(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(UnityEngine.Rendering.RenderBufferLoadAction),typeof(UnityEngine.Rendering.RenderBufferStoreAction),typeof(UnityEngine.Rendering.RenderBufferLoadAction),typeof(UnityEngine.Rendering.RenderBufferStoreAction))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Rendering.RenderBufferLoadAction a2;
				checkEnum(l,3,out a2);
				UnityEngine.Rendering.RenderBufferStoreAction a3;
				checkEnum(l,4,out a3);
				UnityEngine.Rendering.RenderBufferLoadAction a4;
				checkEnum(l,5,out a4);
				UnityEngine.Rendering.RenderBufferStoreAction a5;
				checkEnum(l,6,out a5);
				self.SetRenderTarget(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(int),typeof(UnityEngine.CubemapFace),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Rendering.RenderTargetIdentifier a2;
				checkValueType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				UnityEngine.CubemapFace a4;
				checkEnum(l,5,out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				self.SetRenderTarget(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rendering.RenderTargetIdentifier[]),typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(int),typeof(UnityEngine.CubemapFace),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier[] a1;
				checkArray(l, 2, out a1);
				UnityEngine.Rendering.RenderTargetIdentifier a2;
				checkValueType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				UnityEngine.CubemapFace a4;
				checkEnum(l,5,out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				self.SetRenderTarget(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(argc==7){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Rendering.RenderBufferLoadAction a2;
				checkEnum(l,3,out a2);
				UnityEngine.Rendering.RenderBufferStoreAction a3;
				checkEnum(l,4,out a3);
				UnityEngine.Rendering.RenderTargetIdentifier a4;
				checkValueType(l, 5, out a4);
				UnityEngine.Rendering.RenderBufferLoadAction a5;
				checkEnum(l,6,out a5);
				UnityEngine.Rendering.RenderBufferStoreAction a6;
				checkEnum(l,7,out a6);
				self.SetRenderTarget(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetRenderTarget to call");
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
	static public int SetBufferData(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.ComputeBuffer),typeof(System.Array))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeBuffer a1;
				checkType(l, 2, out a1);
				System.Array a2;
				checkType(l, 3, out a2);
				self.SetBufferData(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.GraphicsBuffer),typeof(System.Array))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.GraphicsBuffer a1;
				checkType(l, 2, out a1);
				System.Array a2;
				checkType(l, 3, out a2);
				self.SetBufferData(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeBuffer),typeof(System.Array),typeof(int),typeof(int),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeBuffer a1;
				checkType(l, 2, out a1);
				System.Array a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				self.SetBufferData(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.GraphicsBuffer),typeof(System.Array),typeof(int),typeof(int),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.GraphicsBuffer a1;
				checkType(l, 2, out a1);
				System.Array a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				self.SetBufferData(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetBufferData to call");
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
	static public int SetBufferCounterValue(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.ComputeBuffer),typeof(System.UInt32))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeBuffer a1;
				checkType(l, 2, out a1);
				System.UInt32 a2;
				checkType(l, 3, out a2);
				self.SetBufferCounterValue(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.GraphicsBuffer),typeof(System.UInt32))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.GraphicsBuffer a1;
				checkType(l, 2, out a1);
				System.UInt32 a2;
				checkType(l, 3, out a2);
				self.SetBufferCounterValue(a1,a2);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetBufferCounterValue to call");
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
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
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
	static public int Release(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			self.Release();
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
	static public int CreateAsyncGraphicsFence(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				var ret=self.CreateAsyncGraphicsFence();
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.SynchronisationStage a1;
				checkEnum(l,2,out a1);
				var ret=self.CreateAsyncGraphicsFence(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function CreateAsyncGraphicsFence to call");
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
	static public int CreateGraphicsFence(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			UnityEngine.Rendering.GraphicsFenceType a1;
			checkEnum(l,2,out a1);
			UnityEngine.Rendering.SynchronisationStageFlags a2;
			checkEnum(l,3,out a2);
			var ret=self.CreateGraphicsFence(a1,a2);
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
	static public int WaitOnAsyncGraphicsFence(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.GraphicsFence a1;
				checkValueType(l, 2, out a1);
				self.WaitOnAsyncGraphicsFence(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rendering.GraphicsFence),typeof(UnityEngine.Rendering.SynchronisationStage))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.GraphicsFence a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Rendering.SynchronisationStage a2;
				checkEnum(l,3,out a2);
				self.WaitOnAsyncGraphicsFence(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rendering.GraphicsFence),typeof(UnityEngine.Rendering.SynchronisationStageFlags))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.GraphicsFence a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Rendering.SynchronisationStageFlags a2;
				checkEnum(l,3,out a2);
				self.WaitOnAsyncGraphicsFence(a1,a2);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function WaitOnAsyncGraphicsFence to call");
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
	static public int SetComputeFloatParams(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(string),typeof(System.Single[]))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				System.Single[] a3;
				checkParams(l,4,out a3);
				self.SetComputeFloatParams(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(int),typeof(System.Single[]))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Single[] a3;
				checkParams(l,4,out a3);
				self.SetComputeFloatParams(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetComputeFloatParams to call");
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
	static public int SetComputeIntParams(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(string),typeof(System.Int32[]))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				System.Int32[] a3;
				checkParams(l,4,out a3);
				self.SetComputeIntParams(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(int),typeof(System.Int32[]))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32[] a3;
				checkParams(l,4,out a3);
				self.SetComputeIntParams(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetComputeIntParams to call");
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
	static public int SetComputeTextureParam(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(int),typeof(string),typeof(UnityEngine.Rendering.RenderTargetIdentifier))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.String a3;
				checkType(l, 4, out a3);
				UnityEngine.Rendering.RenderTargetIdentifier a4;
				checkValueType(l, 5, out a4);
				self.SetComputeTextureParam(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(int),typeof(int),typeof(UnityEngine.Rendering.RenderTargetIdentifier))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				UnityEngine.Rendering.RenderTargetIdentifier a4;
				checkValueType(l, 5, out a4);
				self.SetComputeTextureParam(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(int),typeof(string),typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.String a3;
				checkType(l, 4, out a3);
				UnityEngine.Rendering.RenderTargetIdentifier a4;
				checkValueType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				self.SetComputeTextureParam(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(int),typeof(int),typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				UnityEngine.Rendering.RenderTargetIdentifier a4;
				checkValueType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				self.SetComputeTextureParam(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(int),typeof(string),typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(int),typeof(UnityEngine.Rendering.RenderTextureSubElement))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.String a3;
				checkType(l, 4, out a3);
				UnityEngine.Rendering.RenderTargetIdentifier a4;
				checkValueType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				UnityEngine.Rendering.RenderTextureSubElement a6;
				checkEnum(l,7,out a6);
				self.SetComputeTextureParam(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(int),typeof(int),typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(int),typeof(UnityEngine.Rendering.RenderTextureSubElement))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				UnityEngine.Rendering.RenderTargetIdentifier a4;
				checkValueType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				UnityEngine.Rendering.RenderTextureSubElement a6;
				checkEnum(l,7,out a6);
				self.SetComputeTextureParam(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetComputeTextureParam to call");
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
	static public int SetComputeBufferParam(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(int),typeof(int),typeof(UnityEngine.ComputeBuffer))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				UnityEngine.ComputeBuffer a4;
				checkType(l, 5, out a4);
				self.SetComputeBufferParam(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(int),typeof(string),typeof(UnityEngine.ComputeBuffer))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.String a3;
				checkType(l, 4, out a3);
				UnityEngine.ComputeBuffer a4;
				checkType(l, 5, out a4);
				self.SetComputeBufferParam(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(int),typeof(int),typeof(UnityEngine.GraphicsBuffer))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				UnityEngine.GraphicsBuffer a4;
				checkType(l, 5, out a4);
				self.SetComputeBufferParam(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(int),typeof(string),typeof(UnityEngine.GraphicsBuffer))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.String a3;
				checkType(l, 4, out a3);
				UnityEngine.GraphicsBuffer a4;
				checkType(l, 5, out a4);
				self.SetComputeBufferParam(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetComputeBufferParam to call");
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
	static public int SetComputeConstantBufferParam(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(int),typeof(UnityEngine.ComputeBuffer),typeof(int),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.ComputeBuffer a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				self.SetComputeConstantBufferParam(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(string),typeof(UnityEngine.ComputeBuffer),typeof(int),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				UnityEngine.ComputeBuffer a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				self.SetComputeConstantBufferParam(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(int),typeof(UnityEngine.GraphicsBuffer),typeof(int),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.GraphicsBuffer a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				self.SetComputeConstantBufferParam(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(string),typeof(UnityEngine.GraphicsBuffer),typeof(int),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				UnityEngine.GraphicsBuffer a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				self.SetComputeConstantBufferParam(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetComputeConstantBufferParam to call");
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
	static public int DispatchCompute(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(int),typeof(UnityEngine.ComputeBuffer),typeof(System.UInt32))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.ComputeBuffer a3;
				checkType(l, 4, out a3);
				System.UInt32 a4;
				checkType(l, 5, out a4);
				self.DispatchCompute(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeShader),typeof(int),typeof(UnityEngine.GraphicsBuffer),typeof(System.UInt32))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.GraphicsBuffer a3;
				checkType(l, 4, out a3);
				System.UInt32 a4;
				checkType(l, 5, out a4);
				self.DispatchCompute(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(argc==6){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				self.DispatchCompute(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function DispatchCompute to call");
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
	static public int BuildRayTracingAccelerationStructure(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingAccelerationStructure a1;
				checkType(l, 2, out a1);
				self.BuildRayTracingAccelerationStructure(a1);
				pushValue(l,true);
				return 1;
			}
			else if(argc==3){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingAccelerationStructure a1;
				checkType(l, 2, out a1);
				UnityEngine.Vector3 a2;
				checkType(l, 3, out a2);
				self.BuildRayTracingAccelerationStructure(a1,a2);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function BuildRayTracingAccelerationStructure to call");
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
	static public int SetRayTracingAccelerationStructure(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(string),typeof(UnityEngine.Experimental.Rendering.RayTracingAccelerationStructure))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				UnityEngine.Experimental.Rendering.RayTracingAccelerationStructure a3;
				checkType(l, 4, out a3);
				self.SetRayTracingAccelerationStructure(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(int),typeof(UnityEngine.Experimental.Rendering.RayTracingAccelerationStructure))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.Experimental.Rendering.RayTracingAccelerationStructure a3;
				checkType(l, 4, out a3);
				self.SetRayTracingAccelerationStructure(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetRayTracingAccelerationStructure to call");
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
	static public int SetRayTracingBufferParam(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(string),typeof(UnityEngine.ComputeBuffer))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				UnityEngine.ComputeBuffer a3;
				checkType(l, 4, out a3);
				self.SetRayTracingBufferParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(int),typeof(UnityEngine.ComputeBuffer))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.ComputeBuffer a3;
				checkType(l, 4, out a3);
				self.SetRayTracingBufferParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetRayTracingBufferParam to call");
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
	static public int SetRayTracingConstantBufferParam(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(int),typeof(UnityEngine.ComputeBuffer),typeof(int),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.ComputeBuffer a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				self.SetRayTracingConstantBufferParam(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(string),typeof(UnityEngine.ComputeBuffer),typeof(int),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				UnityEngine.ComputeBuffer a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				self.SetRayTracingConstantBufferParam(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(int),typeof(UnityEngine.GraphicsBuffer),typeof(int),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.GraphicsBuffer a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				self.SetRayTracingConstantBufferParam(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(string),typeof(UnityEngine.GraphicsBuffer),typeof(int),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				UnityEngine.GraphicsBuffer a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				self.SetRayTracingConstantBufferParam(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetRayTracingConstantBufferParam to call");
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
	static public int SetRayTracingTextureParam(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(string),typeof(UnityEngine.Rendering.RenderTargetIdentifier))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				UnityEngine.Rendering.RenderTargetIdentifier a3;
				checkValueType(l, 4, out a3);
				self.SetRayTracingTextureParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(int),typeof(UnityEngine.Rendering.RenderTargetIdentifier))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.Rendering.RenderTargetIdentifier a3;
				checkValueType(l, 4, out a3);
				self.SetRayTracingTextureParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetRayTracingTextureParam to call");
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
	static public int SetRayTracingFloatParam(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(string),typeof(float))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				System.Single a3;
				checkType(l, 4, out a3);
				self.SetRayTracingFloatParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(int),typeof(float))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Single a3;
				checkType(l, 4, out a3);
				self.SetRayTracingFloatParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetRayTracingFloatParam to call");
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
	static public int SetRayTracingFloatParams(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(string),typeof(System.Single[]))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				System.Single[] a3;
				checkParams(l,4,out a3);
				self.SetRayTracingFloatParams(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(int),typeof(System.Single[]))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Single[] a3;
				checkParams(l,4,out a3);
				self.SetRayTracingFloatParams(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetRayTracingFloatParams to call");
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
	static public int SetRayTracingIntParam(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(string),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				self.SetRayTracingIntParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(int),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				self.SetRayTracingIntParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetRayTracingIntParam to call");
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
	static public int SetRayTracingIntParams(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(string),typeof(System.Int32[]))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				System.Int32[] a3;
				checkParams(l,4,out a3);
				self.SetRayTracingIntParams(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(int),typeof(System.Int32[]))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32[] a3;
				checkParams(l,4,out a3);
				self.SetRayTracingIntParams(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetRayTracingIntParams to call");
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
	static public int SetRayTracingVectorParam(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(string),typeof(UnityEngine.Vector4))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				UnityEngine.Vector4 a3;
				checkType(l, 4, out a3);
				self.SetRayTracingVectorParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(int),typeof(UnityEngine.Vector4))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.Vector4 a3;
				checkType(l, 4, out a3);
				self.SetRayTracingVectorParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetRayTracingVectorParam to call");
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
	static public int SetRayTracingVectorArrayParam(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(string),typeof(UnityEngine.Vector4[]))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				UnityEngine.Vector4[] a3;
				checkValueParams(l,4,out a3);
				self.SetRayTracingVectorArrayParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(int),typeof(UnityEngine.Vector4[]))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.Vector4[] a3;
				checkValueParams(l,4,out a3);
				self.SetRayTracingVectorArrayParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetRayTracingVectorArrayParam to call");
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
	static public int SetRayTracingMatrixParam(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(string),typeof(UnityEngine.Matrix4x4))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				UnityEngine.Matrix4x4 a3;
				checkValueType(l, 4, out a3);
				self.SetRayTracingMatrixParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(int),typeof(UnityEngine.Matrix4x4))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.Matrix4x4 a3;
				checkValueType(l, 4, out a3);
				self.SetRayTracingMatrixParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetRayTracingMatrixParam to call");
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
	static public int SetRayTracingMatrixArrayParam(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(string),typeof(UnityEngine.Matrix4x4[]))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				UnityEngine.Matrix4x4[] a3;
				checkValueParams(l,4,out a3);
				self.SetRayTracingMatrixArrayParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Experimental.Rendering.RayTracingShader),typeof(int),typeof(UnityEngine.Matrix4x4[]))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Experimental.Rendering.RayTracingShader a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.Matrix4x4[] a3;
				checkValueParams(l,4,out a3);
				self.SetRayTracingMatrixArrayParam(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetRayTracingMatrixArrayParam to call");
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
	static public int DispatchRays(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			UnityEngine.Experimental.Rendering.RayTracingShader a1;
			checkType(l, 2, out a1);
			System.String a2;
			checkType(l, 3, out a2);
			System.UInt32 a3;
			checkType(l, 4, out a3);
			System.UInt32 a4;
			checkType(l, 5, out a4);
			System.UInt32 a5;
			checkType(l, 6, out a5);
			UnityEngine.Camera a6;
			checkType(l, 7, out a6);
			self.DispatchRays(a1,a2,a3,a4,a5,a6);
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
	static public int GenerateMips(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.Rendering.RenderTargetIdentifier))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier a1;
				checkValueType(l, 2, out a1);
				self.GenerateMips(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.RenderTexture))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.RenderTexture a1;
				checkType(l, 2, out a1);
				self.GenerateMips(a1);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function GenerateMips to call");
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
	static public int ResolveAntiAliasedSurface(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			UnityEngine.RenderTexture a1;
			checkType(l, 2, out a1);
			UnityEngine.RenderTexture a2;
			checkType(l, 3, out a2);
			self.ResolveAntiAliasedSurface(a1,a2);
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
	static public int DrawMesh(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==4){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Mesh a1;
				checkType(l, 2, out a1);
				UnityEngine.Matrix4x4 a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				self.DrawMesh(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(argc==5){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Mesh a1;
				checkType(l, 2, out a1);
				UnityEngine.Matrix4x4 a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				self.DrawMesh(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(argc==6){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Mesh a1;
				checkType(l, 2, out a1);
				UnityEngine.Matrix4x4 a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				self.DrawMesh(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(argc==7){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Mesh a1;
				checkType(l, 2, out a1);
				UnityEngine.Matrix4x4 a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				UnityEngine.MaterialPropertyBlock a6;
				checkType(l, 7, out a6);
				self.DrawMesh(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function DrawMesh to call");
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
	static public int DrawRenderer(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Renderer a1;
				checkType(l, 2, out a1);
				UnityEngine.Material a2;
				checkType(l, 3, out a2);
				self.DrawRenderer(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(argc==4){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Renderer a1;
				checkType(l, 2, out a1);
				UnityEngine.Material a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				self.DrawRenderer(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(argc==5){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Renderer a1;
				checkType(l, 2, out a1);
				UnityEngine.Material a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				self.DrawRenderer(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function DrawRenderer to call");
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
	static public int DrawRendererList(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			UnityEngine.Rendering.RendererUtils.RendererList a1;
			checkValueType(l, 2, out a1);
			self.DrawRendererList(a1);
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
	static public int DrawProcedural(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==6){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Matrix4x4 a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Material a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				UnityEngine.MeshTopology a4;
				checkEnum(l,5,out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				self.DrawProcedural(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Matrix4x4),typeof(UnityEngine.Material),typeof(int),typeof(UnityEngine.MeshTopology),typeof(int),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Matrix4x4 a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Material a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				UnityEngine.MeshTopology a4;
				checkEnum(l,5,out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				System.Int32 a6;
				checkType(l, 7, out a6);
				self.DrawProcedural(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.GraphicsBuffer),typeof(UnityEngine.Matrix4x4),typeof(UnityEngine.Material),typeof(int),typeof(UnityEngine.MeshTopology),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.GraphicsBuffer a1;
				checkType(l, 2, out a1);
				UnityEngine.Matrix4x4 a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.MeshTopology a5;
				checkEnum(l,6,out a5);
				System.Int32 a6;
				checkType(l, 7, out a6);
				self.DrawProcedural(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Matrix4x4),typeof(UnityEngine.Material),typeof(int),typeof(UnityEngine.MeshTopology),typeof(int),typeof(int),typeof(UnityEngine.MaterialPropertyBlock))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Matrix4x4 a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Material a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				UnityEngine.MeshTopology a4;
				checkEnum(l,5,out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				System.Int32 a6;
				checkType(l, 7, out a6);
				UnityEngine.MaterialPropertyBlock a7;
				checkType(l, 8, out a7);
				self.DrawProcedural(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.GraphicsBuffer),typeof(UnityEngine.Matrix4x4),typeof(UnityEngine.Material),typeof(int),typeof(UnityEngine.MeshTopology),typeof(int),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.GraphicsBuffer a1;
				checkType(l, 2, out a1);
				UnityEngine.Matrix4x4 a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.MeshTopology a5;
				checkEnum(l,6,out a5);
				System.Int32 a6;
				checkType(l, 7, out a6);
				System.Int32 a7;
				checkType(l, 8, out a7);
				self.DrawProcedural(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				return 1;
			}
			else if(argc==9){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.GraphicsBuffer a1;
				checkType(l, 2, out a1);
				UnityEngine.Matrix4x4 a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.MeshTopology a5;
				checkEnum(l,6,out a5);
				System.Int32 a6;
				checkType(l, 7, out a6);
				System.Int32 a7;
				checkType(l, 8, out a7);
				UnityEngine.MaterialPropertyBlock a8;
				checkType(l, 9, out a8);
				self.DrawProcedural(a1,a2,a3,a4,a5,a6,a7,a8);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function DrawProcedural to call");
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
	static public int DrawProceduralIndirect(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.Matrix4x4),typeof(UnityEngine.Material),typeof(int),typeof(UnityEngine.MeshTopology),typeof(UnityEngine.ComputeBuffer))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Matrix4x4 a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Material a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				UnityEngine.MeshTopology a4;
				checkEnum(l,5,out a4);
				UnityEngine.ComputeBuffer a5;
				checkType(l, 6, out a5);
				self.DrawProceduralIndirect(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Matrix4x4),typeof(UnityEngine.Material),typeof(int),typeof(UnityEngine.MeshTopology),typeof(UnityEngine.GraphicsBuffer))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Matrix4x4 a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Material a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				UnityEngine.MeshTopology a4;
				checkEnum(l,5,out a4);
				UnityEngine.GraphicsBuffer a5;
				checkType(l, 6, out a5);
				self.DrawProceduralIndirect(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Matrix4x4),typeof(UnityEngine.Material),typeof(int),typeof(UnityEngine.MeshTopology),typeof(UnityEngine.ComputeBuffer),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Matrix4x4 a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Material a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				UnityEngine.MeshTopology a4;
				checkEnum(l,5,out a4);
				UnityEngine.ComputeBuffer a5;
				checkType(l, 6, out a5);
				System.Int32 a6;
				checkType(l, 7, out a6);
				self.DrawProceduralIndirect(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.GraphicsBuffer),typeof(UnityEngine.Matrix4x4),typeof(UnityEngine.Material),typeof(int),typeof(UnityEngine.MeshTopology),typeof(UnityEngine.ComputeBuffer))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.GraphicsBuffer a1;
				checkType(l, 2, out a1);
				UnityEngine.Matrix4x4 a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.MeshTopology a5;
				checkEnum(l,6,out a5);
				UnityEngine.ComputeBuffer a6;
				checkType(l, 7, out a6);
				self.DrawProceduralIndirect(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Matrix4x4),typeof(UnityEngine.Material),typeof(int),typeof(UnityEngine.MeshTopology),typeof(UnityEngine.GraphicsBuffer),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Matrix4x4 a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Material a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				UnityEngine.MeshTopology a4;
				checkEnum(l,5,out a4);
				UnityEngine.GraphicsBuffer a5;
				checkType(l, 6, out a5);
				System.Int32 a6;
				checkType(l, 7, out a6);
				self.DrawProceduralIndirect(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.GraphicsBuffer),typeof(UnityEngine.Matrix4x4),typeof(UnityEngine.Material),typeof(int),typeof(UnityEngine.MeshTopology),typeof(UnityEngine.GraphicsBuffer))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.GraphicsBuffer a1;
				checkType(l, 2, out a1);
				UnityEngine.Matrix4x4 a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.MeshTopology a5;
				checkEnum(l,6,out a5);
				UnityEngine.GraphicsBuffer a6;
				checkType(l, 7, out a6);
				self.DrawProceduralIndirect(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Matrix4x4),typeof(UnityEngine.Material),typeof(int),typeof(UnityEngine.MeshTopology),typeof(UnityEngine.ComputeBuffer),typeof(int),typeof(UnityEngine.MaterialPropertyBlock))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Matrix4x4 a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Material a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				UnityEngine.MeshTopology a4;
				checkEnum(l,5,out a4);
				UnityEngine.ComputeBuffer a5;
				checkType(l, 6, out a5);
				System.Int32 a6;
				checkType(l, 7, out a6);
				UnityEngine.MaterialPropertyBlock a7;
				checkType(l, 8, out a7);
				self.DrawProceduralIndirect(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.GraphicsBuffer),typeof(UnityEngine.Matrix4x4),typeof(UnityEngine.Material),typeof(int),typeof(UnityEngine.MeshTopology),typeof(UnityEngine.ComputeBuffer),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.GraphicsBuffer a1;
				checkType(l, 2, out a1);
				UnityEngine.Matrix4x4 a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.MeshTopology a5;
				checkEnum(l,6,out a5);
				UnityEngine.ComputeBuffer a6;
				checkType(l, 7, out a6);
				System.Int32 a7;
				checkType(l, 8, out a7);
				self.DrawProceduralIndirect(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Matrix4x4),typeof(UnityEngine.Material),typeof(int),typeof(UnityEngine.MeshTopology),typeof(UnityEngine.GraphicsBuffer),typeof(int),typeof(UnityEngine.MaterialPropertyBlock))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Matrix4x4 a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Material a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				UnityEngine.MeshTopology a4;
				checkEnum(l,5,out a4);
				UnityEngine.GraphicsBuffer a5;
				checkType(l, 6, out a5);
				System.Int32 a6;
				checkType(l, 7, out a6);
				UnityEngine.MaterialPropertyBlock a7;
				checkType(l, 8, out a7);
				self.DrawProceduralIndirect(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.GraphicsBuffer),typeof(UnityEngine.Matrix4x4),typeof(UnityEngine.Material),typeof(int),typeof(UnityEngine.MeshTopology),typeof(UnityEngine.GraphicsBuffer),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.GraphicsBuffer a1;
				checkType(l, 2, out a1);
				UnityEngine.Matrix4x4 a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.MeshTopology a5;
				checkEnum(l,6,out a5);
				UnityEngine.GraphicsBuffer a6;
				checkType(l, 7, out a6);
				System.Int32 a7;
				checkType(l, 8, out a7);
				self.DrawProceduralIndirect(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.GraphicsBuffer),typeof(UnityEngine.Matrix4x4),typeof(UnityEngine.Material),typeof(int),typeof(UnityEngine.MeshTopology),typeof(UnityEngine.ComputeBuffer),typeof(int),typeof(UnityEngine.MaterialPropertyBlock))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.GraphicsBuffer a1;
				checkType(l, 2, out a1);
				UnityEngine.Matrix4x4 a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.MeshTopology a5;
				checkEnum(l,6,out a5);
				UnityEngine.ComputeBuffer a6;
				checkType(l, 7, out a6);
				System.Int32 a7;
				checkType(l, 8, out a7);
				UnityEngine.MaterialPropertyBlock a8;
				checkType(l, 9, out a8);
				self.DrawProceduralIndirect(a1,a2,a3,a4,a5,a6,a7,a8);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.GraphicsBuffer),typeof(UnityEngine.Matrix4x4),typeof(UnityEngine.Material),typeof(int),typeof(UnityEngine.MeshTopology),typeof(UnityEngine.GraphicsBuffer),typeof(int),typeof(UnityEngine.MaterialPropertyBlock))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.GraphicsBuffer a1;
				checkType(l, 2, out a1);
				UnityEngine.Matrix4x4 a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.MeshTopology a5;
				checkEnum(l,6,out a5);
				UnityEngine.GraphicsBuffer a6;
				checkType(l, 7, out a6);
				System.Int32 a7;
				checkType(l, 8, out a7);
				UnityEngine.MaterialPropertyBlock a8;
				checkType(l, 9, out a8);
				self.DrawProceduralIndirect(a1,a2,a3,a4,a5,a6,a7,a8);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function DrawProceduralIndirect to call");
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
	static public int DrawMeshInstanced(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==6){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Mesh a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.Matrix4x4[] a5;
				checkArray(l, 6, out a5);
				self.DrawMeshInstanced(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(argc==7){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Mesh a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.Matrix4x4[] a5;
				checkArray(l, 6, out a5);
				System.Int32 a6;
				checkType(l, 7, out a6);
				self.DrawMeshInstanced(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				return 1;
			}
			else if(argc==8){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Mesh a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.Matrix4x4[] a5;
				checkArray(l, 6, out a5);
				System.Int32 a6;
				checkType(l, 7, out a6);
				UnityEngine.MaterialPropertyBlock a7;
				checkType(l, 8, out a7);
				self.DrawMeshInstanced(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function DrawMeshInstanced to call");
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
	static public int DrawMeshInstancedProcedural(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			UnityEngine.Mesh a1;
			checkType(l, 2, out a1);
			System.Int32 a2;
			checkType(l, 3, out a2);
			UnityEngine.Material a3;
			checkType(l, 4, out a3);
			System.Int32 a4;
			checkType(l, 5, out a4);
			System.Int32 a5;
			checkType(l, 6, out a5);
			UnityEngine.MaterialPropertyBlock a6;
			checkType(l, 7, out a6);
			self.DrawMeshInstancedProcedural(a1,a2,a3,a4,a5,a6);
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
	static public int DrawMeshInstancedIndirect(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.Mesh),typeof(int),typeof(UnityEngine.Material),typeof(int),typeof(UnityEngine.ComputeBuffer))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Mesh a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.ComputeBuffer a5;
				checkType(l, 6, out a5);
				self.DrawMeshInstancedIndirect(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Mesh),typeof(int),typeof(UnityEngine.Material),typeof(int),typeof(UnityEngine.GraphicsBuffer))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Mesh a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.GraphicsBuffer a5;
				checkType(l, 6, out a5);
				self.DrawMeshInstancedIndirect(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Mesh),typeof(int),typeof(UnityEngine.Material),typeof(int),typeof(UnityEngine.ComputeBuffer),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Mesh a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.ComputeBuffer a5;
				checkType(l, 6, out a5);
				System.Int32 a6;
				checkType(l, 7, out a6);
				self.DrawMeshInstancedIndirect(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Mesh),typeof(int),typeof(UnityEngine.Material),typeof(int),typeof(UnityEngine.GraphicsBuffer),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Mesh a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.GraphicsBuffer a5;
				checkType(l, 6, out a5);
				System.Int32 a6;
				checkType(l, 7, out a6);
				self.DrawMeshInstancedIndirect(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Mesh),typeof(int),typeof(UnityEngine.Material),typeof(int),typeof(UnityEngine.ComputeBuffer),typeof(int),typeof(UnityEngine.MaterialPropertyBlock))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Mesh a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.ComputeBuffer a5;
				checkType(l, 6, out a5);
				System.Int32 a6;
				checkType(l, 7, out a6);
				UnityEngine.MaterialPropertyBlock a7;
				checkType(l, 8, out a7);
				self.DrawMeshInstancedIndirect(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Mesh),typeof(int),typeof(UnityEngine.Material),typeof(int),typeof(UnityEngine.GraphicsBuffer),typeof(int),typeof(UnityEngine.MaterialPropertyBlock))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Mesh a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				UnityEngine.GraphicsBuffer a5;
				checkType(l, 6, out a5);
				System.Int32 a6;
				checkType(l, 7, out a6);
				UnityEngine.MaterialPropertyBlock a7;
				checkType(l, 8, out a7);
				self.DrawMeshInstancedIndirect(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function DrawMeshInstancedIndirect to call");
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
	static public int DrawOcclusionMesh(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			UnityEngine.RectInt a1;
			checkValueType(l, 2, out a1);
			self.DrawOcclusionMesh(a1);
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
	static public int SetRandomWriteTarget(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int),typeof(UnityEngine.Rendering.RenderTargetIdentifier))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				UnityEngine.Rendering.RenderTargetIdentifier a2;
				checkValueType(l, 3, out a2);
				self.SetRandomWriteTarget(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(UnityEngine.ComputeBuffer))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				UnityEngine.ComputeBuffer a2;
				checkType(l, 3, out a2);
				self.SetRandomWriteTarget(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(UnityEngine.GraphicsBuffer))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				UnityEngine.GraphicsBuffer a2;
				checkType(l, 3, out a2);
				self.SetRandomWriteTarget(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(UnityEngine.ComputeBuffer),typeof(bool))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				UnityEngine.ComputeBuffer a2;
				checkType(l, 3, out a2);
				System.Boolean a3;
				checkType(l, 4, out a3);
				self.SetRandomWriteTarget(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(UnityEngine.GraphicsBuffer),typeof(bool))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				UnityEngine.GraphicsBuffer a2;
				checkType(l, 3, out a2);
				System.Boolean a3;
				checkType(l, 4, out a3);
				self.SetRandomWriteTarget(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetRandomWriteTarget to call");
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
	static public int CopyCounterValue(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.ComputeBuffer),typeof(UnityEngine.ComputeBuffer),typeof(System.UInt32))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeBuffer a1;
				checkType(l, 2, out a1);
				UnityEngine.ComputeBuffer a2;
				checkType(l, 3, out a2);
				System.UInt32 a3;
				checkType(l, 4, out a3);
				self.CopyCounterValue(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.GraphicsBuffer),typeof(UnityEngine.ComputeBuffer),typeof(System.UInt32))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.GraphicsBuffer a1;
				checkType(l, 2, out a1);
				UnityEngine.ComputeBuffer a2;
				checkType(l, 3, out a2);
				System.UInt32 a3;
				checkType(l, 4, out a3);
				self.CopyCounterValue(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeBuffer),typeof(UnityEngine.GraphicsBuffer),typeof(System.UInt32))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeBuffer a1;
				checkType(l, 2, out a1);
				UnityEngine.GraphicsBuffer a2;
				checkType(l, 3, out a2);
				System.UInt32 a3;
				checkType(l, 4, out a3);
				self.CopyCounterValue(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.GraphicsBuffer),typeof(UnityEngine.GraphicsBuffer),typeof(System.UInt32))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.GraphicsBuffer a1;
				checkType(l, 2, out a1);
				UnityEngine.GraphicsBuffer a2;
				checkType(l, 3, out a2);
				System.UInt32 a3;
				checkType(l, 4, out a3);
				self.CopyCounterValue(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function CopyCounterValue to call");
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
	static public int CopyTexture(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Rendering.RenderTargetIdentifier a2;
				checkValueType(l, 3, out a2);
				self.CopyTexture(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(argc==5){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier a1;
				checkValueType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				UnityEngine.Rendering.RenderTargetIdentifier a3;
				checkValueType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				self.CopyTexture(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(argc==7){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier a1;
				checkValueType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				UnityEngine.Rendering.RenderTargetIdentifier a4;
				checkValueType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				System.Int32 a6;
				checkType(l, 7, out a6);
				self.CopyTexture(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				return 1;
			}
			else if(argc==13){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier a1;
				checkValueType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				System.Int32 a6;
				checkType(l, 7, out a6);
				System.Int32 a7;
				checkType(l, 8, out a7);
				UnityEngine.Rendering.RenderTargetIdentifier a8;
				checkValueType(l, 9, out a8);
				System.Int32 a9;
				checkType(l, 10, out a9);
				System.Int32 a10;
				checkType(l, 11, out a10);
				System.Int32 a11;
				checkType(l, 12, out a11);
				System.Int32 a12;
				checkType(l, 13, out a12);
				self.CopyTexture(a1,a2,a3,a4,a5,a6,a7,a8,a9,a10,a11,a12);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function CopyTexture to call");
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
	static public int Blit(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.Texture),typeof(UnityEngine.Rendering.RenderTargetIdentifier))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Texture a1;
				checkType(l, 2, out a1);
				UnityEngine.Rendering.RenderTargetIdentifier a2;
				checkValueType(l, 3, out a2);
				self.Blit(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(UnityEngine.Rendering.RenderTargetIdentifier))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Rendering.RenderTargetIdentifier a2;
				checkValueType(l, 3, out a2);
				self.Blit(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Texture),typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(UnityEngine.Material))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Texture a1;
				checkType(l, 2, out a1);
				UnityEngine.Rendering.RenderTargetIdentifier a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				self.Blit(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(UnityEngine.Material))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Rendering.RenderTargetIdentifier a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				self.Blit(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Texture),typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(UnityEngine.Vector2),typeof(UnityEngine.Vector2))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Texture a1;
				checkType(l, 2, out a1);
				UnityEngine.Rendering.RenderTargetIdentifier a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Vector2 a3;
				checkType(l, 4, out a3);
				UnityEngine.Vector2 a4;
				checkType(l, 5, out a4);
				self.Blit(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Texture),typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(UnityEngine.Material),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Texture a1;
				checkType(l, 2, out a1);
				UnityEngine.Rendering.RenderTargetIdentifier a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				self.Blit(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(UnityEngine.Vector2),typeof(UnityEngine.Vector2))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Rendering.RenderTargetIdentifier a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Vector2 a3;
				checkType(l, 4, out a3);
				UnityEngine.Vector2 a4;
				checkType(l, 5, out a4);
				self.Blit(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(UnityEngine.Material),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Rendering.RenderTargetIdentifier a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				self.Blit(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(int),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Rendering.RenderTargetIdentifier a2;
				checkValueType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				self.Blit(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(argc==6){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Rendering.RenderTargetIdentifier a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Material a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				self.Blit(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(argc==7){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.Rendering.RenderTargetIdentifier a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Rendering.RenderTargetIdentifier a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Vector2 a3;
				checkType(l, 4, out a3);
				UnityEngine.Vector2 a4;
				checkType(l, 5, out a4);
				System.Int32 a5;
				checkType(l, 6, out a5);
				System.Int32 a6;
				checkType(l, 7, out a6);
				self.Blit(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function Blit to call");
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
	static public int SetGlobalTexture(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(string),typeof(UnityEngine.Rendering.RenderTargetIdentifier))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				UnityEngine.Rendering.RenderTargetIdentifier a2;
				checkValueType(l, 3, out a2);
				self.SetGlobalTexture(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(UnityEngine.Rendering.RenderTargetIdentifier))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				UnityEngine.Rendering.RenderTargetIdentifier a2;
				checkValueType(l, 3, out a2);
				self.SetGlobalTexture(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(UnityEngine.Rendering.RenderTextureSubElement))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				UnityEngine.Rendering.RenderTargetIdentifier a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Rendering.RenderTextureSubElement a3;
				checkEnum(l,4,out a3);
				self.SetGlobalTexture(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(UnityEngine.Rendering.RenderTargetIdentifier),typeof(UnityEngine.Rendering.RenderTextureSubElement))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				UnityEngine.Rendering.RenderTargetIdentifier a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Rendering.RenderTextureSubElement a3;
				checkEnum(l,4,out a3);
				self.SetGlobalTexture(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetGlobalTexture to call");
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
	static public int SetGlobalBuffer(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(string),typeof(UnityEngine.ComputeBuffer))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				UnityEngine.ComputeBuffer a2;
				checkType(l, 3, out a2);
				self.SetGlobalBuffer(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(UnityEngine.ComputeBuffer))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				UnityEngine.ComputeBuffer a2;
				checkType(l, 3, out a2);
				self.SetGlobalBuffer(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(UnityEngine.GraphicsBuffer))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				UnityEngine.GraphicsBuffer a2;
				checkType(l, 3, out a2);
				self.SetGlobalBuffer(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(UnityEngine.GraphicsBuffer))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				System.Int32 a1;
				checkType(l, 2, out a1);
				UnityEngine.GraphicsBuffer a2;
				checkType(l, 3, out a2);
				self.SetGlobalBuffer(a1,a2);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetGlobalBuffer to call");
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
	static public int SetGlobalConstantBuffer(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.ComputeBuffer),typeof(int),typeof(int),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeBuffer a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				self.SetGlobalConstantBuffer(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.ComputeBuffer),typeof(string),typeof(int),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.ComputeBuffer a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				self.SetGlobalConstantBuffer(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.GraphicsBuffer),typeof(int),typeof(int),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.GraphicsBuffer a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				self.SetGlobalConstantBuffer(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.GraphicsBuffer),typeof(string),typeof(int),typeof(int))){
				UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
				UnityEngine.GraphicsBuffer a1;
				checkType(l, 2, out a1);
				System.String a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				System.Int32 a4;
				checkType(l, 5, out a4);
				self.SetGlobalConstantBuffer(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetGlobalConstantBuffer to call");
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
	static public int SetShadowSamplingMode(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			UnityEngine.Rendering.RenderTargetIdentifier a1;
			checkValueType(l, 2, out a1);
			UnityEngine.Rendering.ShadowSamplingMode a2;
			checkEnum(l,3,out a2);
			self.SetShadowSamplingMode(a1,a2);
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
	static public int SetSinglePassStereo(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			UnityEngine.Rendering.SinglePassStereoMode a1;
			checkEnum(l,2,out a1);
			self.SetSinglePassStereo(a1);
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
	static public int IssuePluginEvent(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			System.IntPtr a1;
			checkType(l, 2, out a1);
			System.Int32 a2;
			checkType(l, 3, out a2);
			self.IssuePluginEvent(a1,a2);
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
	static public int IssuePluginEventAndData(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			System.IntPtr a1;
			checkType(l, 2, out a1);
			System.Int32 a2;
			checkType(l, 3, out a2);
			System.IntPtr a3;
			checkType(l, 4, out a3);
			self.IssuePluginEventAndData(a1,a2,a3);
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
	static public int IssuePluginCustomBlit(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			System.IntPtr a1;
			checkType(l, 2, out a1);
			System.UInt32 a2;
			checkType(l, 3, out a2);
			UnityEngine.Rendering.RenderTargetIdentifier a3;
			checkValueType(l, 4, out a3);
			UnityEngine.Rendering.RenderTargetIdentifier a4;
			checkValueType(l, 5, out a4);
			System.UInt32 a5;
			checkType(l, 6, out a5);
			System.UInt32 a6;
			checkType(l, 7, out a6);
			self.IssuePluginCustomBlit(a1,a2,a3,a4,a5,a6);
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
	static public int IssuePluginCustomTextureUpdateV2(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			System.IntPtr a1;
			checkType(l, 2, out a1);
			UnityEngine.Texture a2;
			checkType(l, 3, out a2);
			System.UInt32 a3;
			checkType(l, 4, out a3);
			self.IssuePluginCustomTextureUpdateV2(a1,a2,a3);
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
	static public int ProcessVTFeedback(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			UnityEngine.Rendering.RenderTargetIdentifier a1;
			checkValueType(l, 2, out a1);
			System.IntPtr a2;
			checkType(l, 3, out a2);
			System.Int32 a3;
			checkType(l, 4, out a3);
			System.Int32 a4;
			checkType(l, 5, out a4);
			System.Int32 a5;
			checkType(l, 6, out a5);
			System.Int32 a6;
			checkType(l, 7, out a6);
			System.Int32 a7;
			checkType(l, 8, out a7);
			System.Int32 a8;
			checkType(l, 9, out a8);
			self.ProcessVTFeedback(a1,a2,a3,a4,a5,a6,a7,a8);
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
	static public int CopyBuffer(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			UnityEngine.GraphicsBuffer a1;
			checkType(l, 2, out a1);
			UnityEngine.GraphicsBuffer a2;
			checkType(l, 3, out a2);
			self.CopyBuffer(a1,a2);
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
	static public int get_name(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.name);
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
	static public int set_name(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.name=v;
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
	static public int get_sizeInBytes(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.CommandBuffer self=(UnityEngine.Rendering.CommandBuffer)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.sizeInBytes);
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
		getTypeTable(l,"UnityEngine.Rendering.CommandBuffer");
		addMember(l,ConvertTexture);
		addMember(l,WaitAllAsyncReadbackRequests);
		addMember(l,RequestAsyncReadback);
		addMember(l,SetInvertCulling);
		addMember(l,SetComputeFloatParam);
		addMember(l,SetComputeIntParam);
		addMember(l,SetComputeVectorParam);
		addMember(l,SetComputeVectorArrayParam);
		addMember(l,SetComputeMatrixParam);
		addMember(l,SetComputeMatrixArrayParam);
		addMember(l,SetRayTracingShaderPass);
		addMember(l,Clear);
		addMember(l,ClearRandomWriteTargets);
		addMember(l,SetViewport);
		addMember(l,EnableScissorRect);
		addMember(l,DisableScissorRect);
		addMember(l,GetTemporaryRT);
		addMember(l,GetTemporaryRTArray);
		addMember(l,ReleaseTemporaryRT);
		addMember(l,ClearRenderTarget);
		addMember(l,SetGlobalFloat);
		addMember(l,SetGlobalInt);
		addMember(l,SetGlobalInteger);
		addMember(l,SetGlobalVector);
		addMember(l,SetGlobalColor);
		addMember(l,SetGlobalMatrix);
		addMember(l,EnableShaderKeyword);
		addMember(l,EnableKeyword);
		addMember(l,DisableShaderKeyword);
		addMember(l,DisableKeyword);
		addMember(l,SetKeyword);
		addMember(l,SetViewMatrix);
		addMember(l,SetProjectionMatrix);
		addMember(l,SetViewProjectionMatrices);
		addMember(l,SetGlobalDepthBias);
		addMember(l,SetExecutionFlags);
		addMember(l,SetGlobalFloatArray);
		addMember(l,SetGlobalVectorArray);
		addMember(l,SetGlobalMatrixArray);
		addMember(l,SetLateLatchProjectionMatrices);
		addMember(l,MarkLateLatchMatrixShaderPropertyID);
		addMember(l,UnmarkLateLatchMatrix);
		addMember(l,BeginSample);
		addMember(l,EndSample);
		addMember(l,IncrementUpdateCount);
		addMember(l,SetInstanceMultiplier);
		addMember(l,SetRenderTarget);
		addMember(l,SetBufferData);
		addMember(l,SetBufferCounterValue);
		addMember(l,Dispose);
		addMember(l,Release);
		addMember(l,CreateAsyncGraphicsFence);
		addMember(l,CreateGraphicsFence);
		addMember(l,WaitOnAsyncGraphicsFence);
		addMember(l,SetComputeFloatParams);
		addMember(l,SetComputeIntParams);
		addMember(l,SetComputeTextureParam);
		addMember(l,SetComputeBufferParam);
		addMember(l,SetComputeConstantBufferParam);
		addMember(l,DispatchCompute);
		addMember(l,BuildRayTracingAccelerationStructure);
		addMember(l,SetRayTracingAccelerationStructure);
		addMember(l,SetRayTracingBufferParam);
		addMember(l,SetRayTracingConstantBufferParam);
		addMember(l,SetRayTracingTextureParam);
		addMember(l,SetRayTracingFloatParam);
		addMember(l,SetRayTracingFloatParams);
		addMember(l,SetRayTracingIntParam);
		addMember(l,SetRayTracingIntParams);
		addMember(l,SetRayTracingVectorParam);
		addMember(l,SetRayTracingVectorArrayParam);
		addMember(l,SetRayTracingMatrixParam);
		addMember(l,SetRayTracingMatrixArrayParam);
		addMember(l,DispatchRays);
		addMember(l,GenerateMips);
		addMember(l,ResolveAntiAliasedSurface);
		addMember(l,DrawMesh);
		addMember(l,DrawRenderer);
		addMember(l,DrawRendererList);
		addMember(l,DrawProcedural);
		addMember(l,DrawProceduralIndirect);
		addMember(l,DrawMeshInstanced);
		addMember(l,DrawMeshInstancedProcedural);
		addMember(l,DrawMeshInstancedIndirect);
		addMember(l,DrawOcclusionMesh);
		addMember(l,SetRandomWriteTarget);
		addMember(l,CopyCounterValue);
		addMember(l,CopyTexture);
		addMember(l,Blit);
		addMember(l,SetGlobalTexture);
		addMember(l,SetGlobalBuffer);
		addMember(l,SetGlobalConstantBuffer);
		addMember(l,SetShadowSamplingMode);
		addMember(l,SetSinglePassStereo);
		addMember(l,IssuePluginEvent);
		addMember(l,IssuePluginEventAndData);
		addMember(l,IssuePluginCustomBlit);
		addMember(l,IssuePluginCustomTextureUpdateV2);
		addMember(l,ProcessVTFeedback);
		addMember(l,CopyBuffer);
		addMember(l,"name",get_name,set_name,true);
		addMember(l,"sizeInBytes",get_sizeInBytes,null,true);
		createTypeMetatable(l,constructor, typeof(UnityEngine.Rendering.CommandBuffer));
	}
}
