using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_Rendering_ScriptableRenderContext : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.ScriptableRenderContext o;
			o=new UnityEngine.Rendering.ScriptableRenderContext();
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
	static public int BeginRenderPass(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.ScriptableRenderContext self;
			checkValueType(l,1,out self);
			System.Int32 a1;
			checkType(l, 2, out a1);
			System.Int32 a2;
			checkType(l, 3, out a2);
			System.Int32 a3;
			checkType(l, 4, out a3);
			Unity.Collections.NativeArray<UnityEngine.Rendering.AttachmentDescriptor> a4;
			checkValueType(l, 5, out a4);
			System.Int32 a5;
			checkType(l, 6, out a5);
			self.BeginRenderPass(a1,a2,a3,a4,a5);
			pushValue(l,true);
			setBack(l,self);
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
	static public int BeginScopedRenderPass(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.ScriptableRenderContext self;
			checkValueType(l,1,out self);
			System.Int32 a1;
			checkType(l, 2, out a1);
			System.Int32 a2;
			checkType(l, 3, out a2);
			System.Int32 a3;
			checkType(l, 4, out a3);
			Unity.Collections.NativeArray<UnityEngine.Rendering.AttachmentDescriptor> a4;
			checkValueType(l, 5, out a4);
			System.Int32 a5;
			checkType(l, 6, out a5);
			var ret=self.BeginScopedRenderPass(a1,a2,a3,a4,a5);
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
	static public int BeginSubPass(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				UnityEngine.Rendering.ScriptableRenderContext self;
				checkValueType(l,1,out self);
				Unity.Collections.NativeArray<System.Int32> a1;
				checkValueType(l, 2, out a1);
				System.Boolean a2;
				checkType(l, 3, out a2);
				self.BeginSubPass(a1,a2);
				pushValue(l,true);
				setBack(l,self);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(Unity.Collections.NativeArray<System.Int32>),typeof(Unity.Collections.NativeArray<System.Int32>),typeof(bool))){
				UnityEngine.Rendering.ScriptableRenderContext self;
				checkValueType(l,1,out self);
				Unity.Collections.NativeArray<System.Int32> a1;
				checkValueType(l, 2, out a1);
				Unity.Collections.NativeArray<System.Int32> a2;
				checkValueType(l, 3, out a2);
				System.Boolean a3;
				checkType(l, 4, out a3);
				self.BeginSubPass(a1,a2,a3);
				pushValue(l,true);
				setBack(l,self);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(Unity.Collections.NativeArray<System.Int32>),typeof(bool),typeof(bool))){
				UnityEngine.Rendering.ScriptableRenderContext self;
				checkValueType(l,1,out self);
				Unity.Collections.NativeArray<System.Int32> a1;
				checkValueType(l, 2, out a1);
				System.Boolean a2;
				checkType(l, 3, out a2);
				System.Boolean a3;
				checkType(l, 4, out a3);
				self.BeginSubPass(a1,a2,a3);
				pushValue(l,true);
				setBack(l,self);
				return 1;
			}
			else if(argc==5){
				UnityEngine.Rendering.ScriptableRenderContext self;
				checkValueType(l,1,out self);
				Unity.Collections.NativeArray<System.Int32> a1;
				checkValueType(l, 2, out a1);
				Unity.Collections.NativeArray<System.Int32> a2;
				checkValueType(l, 3, out a2);
				System.Boolean a3;
				checkType(l, 4, out a3);
				System.Boolean a4;
				checkType(l, 5, out a4);
				self.BeginSubPass(a1,a2,a3,a4);
				pushValue(l,true);
				setBack(l,self);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function BeginSubPass to call");
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
	static public int BeginScopedSubPass(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				UnityEngine.Rendering.ScriptableRenderContext self;
				checkValueType(l,1,out self);
				Unity.Collections.NativeArray<System.Int32> a1;
				checkValueType(l, 2, out a1);
				System.Boolean a2;
				checkType(l, 3, out a2);
				var ret=self.BeginScopedSubPass(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(Unity.Collections.NativeArray<System.Int32>),typeof(Unity.Collections.NativeArray<System.Int32>),typeof(bool))){
				UnityEngine.Rendering.ScriptableRenderContext self;
				checkValueType(l,1,out self);
				Unity.Collections.NativeArray<System.Int32> a1;
				checkValueType(l, 2, out a1);
				Unity.Collections.NativeArray<System.Int32> a2;
				checkValueType(l, 3, out a2);
				System.Boolean a3;
				checkType(l, 4, out a3);
				var ret=self.BeginScopedSubPass(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(Unity.Collections.NativeArray<System.Int32>),typeof(bool),typeof(bool))){
				UnityEngine.Rendering.ScriptableRenderContext self;
				checkValueType(l,1,out self);
				Unity.Collections.NativeArray<System.Int32> a1;
				checkValueType(l, 2, out a1);
				System.Boolean a2;
				checkType(l, 3, out a2);
				System.Boolean a3;
				checkType(l, 4, out a3);
				var ret=self.BeginScopedSubPass(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==5){
				UnityEngine.Rendering.ScriptableRenderContext self;
				checkValueType(l,1,out self);
				Unity.Collections.NativeArray<System.Int32> a1;
				checkValueType(l, 2, out a1);
				Unity.Collections.NativeArray<System.Int32> a2;
				checkValueType(l, 3, out a2);
				System.Boolean a3;
				checkType(l, 4, out a3);
				System.Boolean a4;
				checkType(l, 5, out a4);
				var ret=self.BeginScopedSubPass(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function BeginScopedSubPass to call");
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
	static public int EndSubPass(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.ScriptableRenderContext self;
			checkValueType(l,1,out self);
			self.EndSubPass();
			pushValue(l,true);
			setBack(l,self);
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
	static public int EndRenderPass(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.ScriptableRenderContext self;
			checkValueType(l,1,out self);
			self.EndRenderPass();
			pushValue(l,true);
			setBack(l,self);
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
	static public int Submit(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.ScriptableRenderContext self;
			checkValueType(l,1,out self);
			self.Submit();
			pushValue(l,true);
			setBack(l,self);
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
	static public int SubmitForRenderPassValidation(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.ScriptableRenderContext self;
			checkValueType(l,1,out self);
			var ret=self.SubmitForRenderPassValidation();
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
	static public int DrawRenderers(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==4){
				UnityEngine.Rendering.ScriptableRenderContext self;
				checkValueType(l,1,out self);
				UnityEngine.Rendering.CullingResults a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Rendering.DrawingSettings a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Rendering.FilteringSettings a3;
				checkValueType(l, 4, out a3);
				self.DrawRenderers(a1,ref a2,ref a3);
				pushValue(l,true);
				pushValue(l,a2);
				pushValue(l,a3);
				setBack(l,self);
				return 3;
			}
			else if(argc==5){
				UnityEngine.Rendering.ScriptableRenderContext self;
				checkValueType(l,1,out self);
				UnityEngine.Rendering.CullingResults a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Rendering.DrawingSettings a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Rendering.FilteringSettings a3;
				checkValueType(l, 4, out a3);
				UnityEngine.Rendering.RenderStateBlock a4;
				checkValueType(l, 5, out a4);
				self.DrawRenderers(a1,ref a2,ref a3,ref a4);
				pushValue(l,true);
				pushValue(l,a2);
				pushValue(l,a3);
				pushValue(l,a4);
				setBack(l,self);
				return 4;
			}
			else if(argc==6){
				UnityEngine.Rendering.ScriptableRenderContext self;
				checkValueType(l,1,out self);
				UnityEngine.Rendering.CullingResults a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Rendering.DrawingSettings a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Rendering.FilteringSettings a3;
				checkValueType(l, 4, out a3);
				Unity.Collections.NativeArray<UnityEngine.Rendering.ShaderTagId> a4;
				checkValueType(l, 5, out a4);
				Unity.Collections.NativeArray<UnityEngine.Rendering.RenderStateBlock> a5;
				checkValueType(l, 6, out a5);
				self.DrawRenderers(a1,ref a2,ref a3,a4,a5);
				pushValue(l,true);
				pushValue(l,a2);
				pushValue(l,a3);
				setBack(l,self);
				return 3;
			}
			else if(argc==8){
				UnityEngine.Rendering.ScriptableRenderContext self;
				checkValueType(l,1,out self);
				UnityEngine.Rendering.CullingResults a1;
				checkValueType(l, 2, out a1);
				UnityEngine.Rendering.DrawingSettings a2;
				checkValueType(l, 3, out a2);
				UnityEngine.Rendering.FilteringSettings a3;
				checkValueType(l, 4, out a3);
				UnityEngine.Rendering.ShaderTagId a4;
				checkValueType(l, 5, out a4);
				System.Boolean a5;
				checkType(l, 6, out a5);
				Unity.Collections.NativeArray<UnityEngine.Rendering.ShaderTagId> a6;
				checkValueType(l, 7, out a6);
				Unity.Collections.NativeArray<UnityEngine.Rendering.RenderStateBlock> a7;
				checkValueType(l, 8, out a7);
				self.DrawRenderers(a1,ref a2,ref a3,a4,a5,a6,a7);
				pushValue(l,true);
				pushValue(l,a2);
				pushValue(l,a3);
				setBack(l,self);
				return 3;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function DrawRenderers to call");
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
	static public int DrawShadows(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.ScriptableRenderContext self;
			checkValueType(l,1,out self);
			UnityEngine.Rendering.ShadowDrawingSettings a1;
			checkValueType(l, 2, out a1);
			self.DrawShadows(ref a1);
			pushValue(l,true);
			pushValue(l,a1);
			setBack(l,self);
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
	static public int ExecuteCommandBuffer(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.ScriptableRenderContext self;
			checkValueType(l,1,out self);
			UnityEngine.Rendering.CommandBuffer a1;
			checkType(l, 2, out a1);
			self.ExecuteCommandBuffer(a1);
			pushValue(l,true);
			setBack(l,self);
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
	static public int ExecuteCommandBufferAsync(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.ScriptableRenderContext self;
			checkValueType(l,1,out self);
			UnityEngine.Rendering.CommandBuffer a1;
			checkType(l, 2, out a1);
			UnityEngine.Rendering.ComputeQueueType a2;
			checkEnum(l,3,out a2);
			self.ExecuteCommandBufferAsync(a1,a2);
			pushValue(l,true);
			setBack(l,self);
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
	static public int SetupCameraProperties(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				UnityEngine.Rendering.ScriptableRenderContext self;
				checkValueType(l,1,out self);
				UnityEngine.Camera a1;
				checkType(l, 2, out a1);
				System.Boolean a2;
				checkType(l, 3, out a2);
				self.SetupCameraProperties(a1,a2);
				pushValue(l,true);
				setBack(l,self);
				return 1;
			}
			else if(argc==4){
				UnityEngine.Rendering.ScriptableRenderContext self;
				checkValueType(l,1,out self);
				UnityEngine.Camera a1;
				checkType(l, 2, out a1);
				System.Boolean a2;
				checkType(l, 3, out a2);
				System.Int32 a3;
				checkType(l, 4, out a3);
				self.SetupCameraProperties(a1,a2,a3);
				pushValue(l,true);
				setBack(l,self);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SetupCameraProperties to call");
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
	static public int StereoEndRender(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				UnityEngine.Rendering.ScriptableRenderContext self;
				checkValueType(l,1,out self);
				UnityEngine.Camera a1;
				checkType(l, 2, out a1);
				self.StereoEndRender(a1);
				pushValue(l,true);
				setBack(l,self);
				return 1;
			}
			else if(argc==3){
				UnityEngine.Rendering.ScriptableRenderContext self;
				checkValueType(l,1,out self);
				UnityEngine.Camera a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				self.StereoEndRender(a1,a2);
				pushValue(l,true);
				setBack(l,self);
				return 1;
			}
			else if(argc==4){
				UnityEngine.Rendering.ScriptableRenderContext self;
				checkValueType(l,1,out self);
				UnityEngine.Camera a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				System.Boolean a3;
				checkType(l, 4, out a3);
				self.StereoEndRender(a1,a2,a3);
				pushValue(l,true);
				setBack(l,self);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function StereoEndRender to call");
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
	static public int StartMultiEye(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				UnityEngine.Rendering.ScriptableRenderContext self;
				checkValueType(l,1,out self);
				UnityEngine.Camera a1;
				checkType(l, 2, out a1);
				self.StartMultiEye(a1);
				pushValue(l,true);
				setBack(l,self);
				return 1;
			}
			else if(argc==3){
				UnityEngine.Rendering.ScriptableRenderContext self;
				checkValueType(l,1,out self);
				UnityEngine.Camera a1;
				checkType(l, 2, out a1);
				System.Int32 a2;
				checkType(l, 3, out a2);
				self.StartMultiEye(a1,a2);
				pushValue(l,true);
				setBack(l,self);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function StartMultiEye to call");
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
	static public int StopMultiEye(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.ScriptableRenderContext self;
			checkValueType(l,1,out self);
			UnityEngine.Camera a1;
			checkType(l, 2, out a1);
			self.StopMultiEye(a1);
			pushValue(l,true);
			setBack(l,self);
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
	static public int DrawSkybox(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.ScriptableRenderContext self;
			checkValueType(l,1,out self);
			UnityEngine.Camera a1;
			checkType(l, 2, out a1);
			self.DrawSkybox(a1);
			pushValue(l,true);
			setBack(l,self);
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
	static public int InvokeOnRenderObjectCallback(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.ScriptableRenderContext self;
			checkValueType(l,1,out self);
			self.InvokeOnRenderObjectCallback();
			pushValue(l,true);
			setBack(l,self);
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
	static public int DrawGizmos(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.ScriptableRenderContext self;
			checkValueType(l,1,out self);
			UnityEngine.Camera a1;
			checkType(l, 2, out a1);
			UnityEngine.Rendering.GizmoSubset a2;
			checkEnum(l,3,out a2);
			self.DrawGizmos(a1,a2);
			pushValue(l,true);
			setBack(l,self);
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
	static public int DrawWireOverlay(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.ScriptableRenderContext self;
			checkValueType(l,1,out self);
			UnityEngine.Camera a1;
			checkType(l, 2, out a1);
			self.DrawWireOverlay(a1);
			pushValue(l,true);
			setBack(l,self);
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
	static public int DrawUIOverlay(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.ScriptableRenderContext self;
			checkValueType(l,1,out self);
			UnityEngine.Camera a1;
			checkType(l, 2, out a1);
			self.DrawUIOverlay(a1);
			pushValue(l,true);
			setBack(l,self);
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
	static public int Cull(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.ScriptableRenderContext self;
			checkValueType(l,1,out self);
			UnityEngine.Rendering.ScriptableCullingParameters a1;
			checkValueType(l, 2, out a1);
			var ret=self.Cull(ref a1);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a1);
			return 3;
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
	static public int CreateRendererList(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.ScriptableRenderContext self;
			checkValueType(l,1,out self);
			UnityEngine.Rendering.RendererUtils.RendererListDesc a1;
			checkValueType(l, 2, out a1);
			var ret=self.CreateRendererList(a1);
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
	static public int PrepareRendererListsAsync(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.ScriptableRenderContext self;
			checkValueType(l,1,out self);
			System.Collections.Generic.List<UnityEngine.Rendering.RendererUtils.RendererList> a1;
			checkType(l, 2, out a1);
			self.PrepareRendererListsAsync(a1);
			pushValue(l,true);
			setBack(l,self);
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
	static public int QueryRendererListStatus(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.ScriptableRenderContext self;
			checkValueType(l,1,out self);
			UnityEngine.Rendering.RendererUtils.RendererList a1;
			checkValueType(l, 2, out a1);
			var ret=self.QueryRendererListStatus(a1);
			pushValue(l,true);
			pushEnum(l,(int)ret);
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
	/*[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int EmitWorldGeometryForSceneView_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Camera a1;
			checkType(l, 1, out a1);
			UnityEngine.Rendering.ScriptableRenderContext.EmitWorldGeometryForSceneView(a1);
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
	}*/
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int EmitGeometryForCamera_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Camera a1;
			checkType(l, 1, out a1);
			UnityEngine.Rendering.ScriptableRenderContext.EmitGeometryForCamera(a1);
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
	static public int op_Equality(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.ScriptableRenderContext a1;
			checkValueType(l, 1, out a1);
			UnityEngine.Rendering.ScriptableRenderContext a2;
			checkValueType(l, 2, out a2);
			var ret=(a1==a2);
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
	static public int op_Inequality(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Rendering.ScriptableRenderContext a1;
			checkValueType(l, 1, out a1);
			UnityEngine.Rendering.ScriptableRenderContext a2;
			checkValueType(l, 2, out a2);
			var ret=(a1!=a2);
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
		getTypeTable(l,"UnityEngine.Rendering.ScriptableRenderContext");
		addMember(l,BeginRenderPass);
		addMember(l,BeginScopedRenderPass);
		addMember(l,BeginSubPass);
		addMember(l,BeginScopedSubPass);
		addMember(l,EndSubPass);
		addMember(l,EndRenderPass);
		addMember(l,Submit);
		addMember(l,SubmitForRenderPassValidation);
		addMember(l,DrawRenderers);
		addMember(l,DrawShadows);
		addMember(l,ExecuteCommandBuffer);
		addMember(l,ExecuteCommandBufferAsync);
		addMember(l,SetupCameraProperties);
		addMember(l,StereoEndRender);
		addMember(l,StartMultiEye);
		addMember(l,StopMultiEye);
		addMember(l,DrawSkybox);
		addMember(l,InvokeOnRenderObjectCallback);
		addMember(l,DrawGizmos);
		addMember(l,DrawWireOverlay);
		addMember(l,DrawUIOverlay);
		addMember(l,Cull);
		addMember(l,CreateRendererList);
		addMember(l,PrepareRendererListsAsync);
		addMember(l,QueryRendererListStatus);
		//addMember(l,EmitWorldGeometryForSceneView_s);
		addMember(l,EmitGeometryForCamera_s);
		addMember(l,op_Equality);
		addMember(l,op_Inequality);
		createTypeMetatable(l,constructor, typeof(UnityEngine.Rendering.ScriptableRenderContext),typeof(System.ValueType));
	}
}
