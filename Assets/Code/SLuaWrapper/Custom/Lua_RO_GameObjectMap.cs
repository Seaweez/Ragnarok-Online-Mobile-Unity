using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_GameObjectMap : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ClearInfoPool(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			self.ClearInfoPool();
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
	static public int GetInfoCountInPool(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			var ret=self.GetInfoCountInPool();
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
	static public int CreateInfo(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			System.Int64 a2;
			checkType(l, 3, out a2);
			var ret=self.CreateInfo(a1,a2);
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
	static public int RecycleInfo(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			RO.GameObjectInfo.Info a1;
			checkType(l, 2, out a1);
			self.RecycleInfo(a1);
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
	static public int SetCullingGroupCamera(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			UnityEngine.Camera a1;
			checkType(l, 2, out a1);
			self.SetCullingGroupCamera(a1);
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
	static public int ClearCullingGroupCamera(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			UnityEngine.Camera a1;
			checkType(l, 2, out a1);
			self.ClearCullingGroupCamera(a1);
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
	static public int Register_Transform(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			System.Int64 a2;
			checkType(l, 3, out a2);
			UnityEngine.Transform a3;
			checkType(l, 4, out a3);
			var ret=self.Register_Transform(a1,a2,a3);
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
	static public int Register_TransformForCulling(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			System.Int64 a2;
			checkType(l, 3, out a2);
			UnityEngine.Transform a3;
			checkType(l, 4, out a3);
			System.Single a4;
			checkType(l, 5, out a4);
			var ret=self.Register_TransformForCulling(a1,a2,a3,a4);
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
	static public int Unregister(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			System.Int64 a2;
			checkType(l, 3, out a2);
			var ret=self.Unregister(a1,a2);
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
	static public int Clear(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			self.Clear(a1);
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
	static public int ClearAll(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			self.ClearAll();
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
	static public int Get_Transform(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			System.Int64 a2;
			checkType(l, 3, out a2);
			var ret=self.Get_Transform(a1,a2);
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
	static public int Get_Info(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			System.Int64 a2;
			checkType(l, 3, out a2);
			var ret=self.Get_Info(a1,a2);
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
	static public int GetChangedInfo(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			var ret=self.GetChangedInfo();
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
	static public int UpdateCullingGroup(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			self.UpdateCullingGroup();
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
	static public int GetTransform_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			var ret=RO.GameObjectMap.GetTransform(a1,a2);
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
	static public int TransformIsNull_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			var ret=RO.GameObjectMap.TransformIsNull(a1,a2);
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
	static public int DestroyGameObject_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			RO.GameObjectMap.DestroyGameObject(a1,a2);
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
	static public int SetGameObjectActive_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Int32 a3;
			checkType(l, 3, out a3);
			RO.GameObjectMap.SetGameObjectActive(a1,a2,a3);
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
	static public int RestoreBehaviours_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			RO.GameObjectMap.RestoreBehaviours(a1,a2);
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
	static public int SetRenderQueue_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Int32 a3;
			checkType(l, 3, out a3);
			RO.GameObjectMap.SetRenderQueue(a1,a2,a3);
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
	static public int SetUIDepth_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Int32 a3;
			checkType(l, 3, out a3);
			RO.GameObjectMap.SetUIDepth(a1,a2,a3);
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
	static public int FollowTransform_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Int32 a3;
			checkType(l, 3, out a3);
			System.Int64 a4;
			checkType(l, 4, out a4);
			UnityEngine.Vector3 a5;
			checkType(l, 5, out a5);
			RO.GameObjectMap.FollowTransform(a1,a2,a3,a4,a5);
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
	static public int FollowTransformWithRotation_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Int32 a3;
			checkType(l, 3, out a3);
			System.Int64 a4;
			checkType(l, 4, out a4);
			UnityEngine.Vector3 a5;
			checkType(l, 5, out a5);
			RO.GameObjectMap.FollowTransformWithRotation(a1,a2,a3,a4,a5);
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
	static public int GetForwardPosition_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			checkType(l, 3, out a3);
			System.Single a4;
			System.Single a5;
			System.Single a6;
			RO.GameObjectMap.GetForwardPosition(a1,a2,a3,out a4,out a5,out a6);
			pushValue(l,true);
			pushValue(l,a4);
			pushValue(l,a5);
			pushValue(l,a6);
			return 4;
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
	static public int GetPosition_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			System.Single a4;
			System.Single a5;
			RO.GameObjectMap.GetPosition(a1,a2,out a3,out a4,out a5);
			pushValue(l,true);
			pushValue(l,a3);
			pushValue(l,a4);
			pushValue(l,a5);
			return 4;
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
	static public int GetLocalPosition_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			System.Single a4;
			System.Single a5;
			RO.GameObjectMap.GetLocalPosition(a1,a2,out a3,out a4,out a5);
			pushValue(l,true);
			pushValue(l,a3);
			pushValue(l,a4);
			pushValue(l,a5);
			return 4;
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
	static public int GetRotation_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			System.Single a4;
			System.Single a5;
			System.Single a6;
			RO.GameObjectMap.GetRotation(a1,a2,out a3,out a4,out a5,out a6);
			pushValue(l,true);
			pushValue(l,a3);
			pushValue(l,a4);
			pushValue(l,a5);
			pushValue(l,a6);
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
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetLocalRotation_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			System.Single a4;
			System.Single a5;
			System.Single a6;
			RO.GameObjectMap.GetLocalRotation(a1,a2,out a3,out a4,out a5,out a6);
			pushValue(l,true);
			pushValue(l,a3);
			pushValue(l,a4);
			pushValue(l,a5);
			pushValue(l,a6);
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
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetEulerAngles_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			System.Single a4;
			System.Single a5;
			RO.GameObjectMap.GetEulerAngles(a1,a2,out a3,out a4,out a5);
			pushValue(l,true);
			pushValue(l,a3);
			pushValue(l,a4);
			pushValue(l,a5);
			return 4;
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
	static public int GetLocalEulerAngles_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			System.Single a4;
			System.Single a5;
			RO.GameObjectMap.GetLocalEulerAngles(a1,a2,out a3,out a4,out a5);
			pushValue(l,true);
			pushValue(l,a3);
			pushValue(l,a4);
			pushValue(l,a5);
			return 4;
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
	static public int GetScale_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			System.Single a4;
			System.Single a5;
			RO.GameObjectMap.GetScale(a1,a2,out a3,out a4,out a5);
			pushValue(l,true);
			pushValue(l,a3);
			pushValue(l,a4);
			pushValue(l,a5);
			return 4;
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
	static public int GetLocalScale_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			System.Single a4;
			System.Single a5;
			RO.GameObjectMap.GetLocalScale(a1,a2,out a3,out a4,out a5);
			pushValue(l,true);
			pushValue(l,a3);
			pushValue(l,a4);
			pushValue(l,a5);
			return 4;
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
	static public int InverseTransformPointByVector3_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			checkType(l, 3, out a3);
			System.Single a4;
			checkType(l, 4, out a4);
			System.Single a5;
			checkType(l, 5, out a5);
			System.Single a6;
			System.Single a7;
			System.Single a8;
			RO.GameObjectMap.InverseTransformPointByVector3(a1,a2,a3,a4,a5,out a6,out a7,out a8);
			pushValue(l,true);
			pushValue(l,a6);
			pushValue(l,a7);
			pushValue(l,a8);
			return 4;
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
	static public int InverseTransformPointByTransform_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Int32 a3;
			checkType(l, 3, out a3);
			System.Int64 a4;
			checkType(l, 4, out a4);
			UnityEngine.Space a5;
			checkEnum(l,5,out a5);
			System.Single a6;
			System.Single a7;
			System.Single a8;
			RO.GameObjectMap.InverseTransformPointByTransform(a1,a2,a3,a4,a5,out a6,out a7,out a8);
			pushValue(l,true);
			pushValue(l,a6);
			pushValue(l,a7);
			pushValue(l,a8);
			return 4;
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
	static public int InverseTransformVector_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			checkType(l, 3, out a3);
			System.Single a4;
			checkType(l, 4, out a4);
			System.Single a5;
			checkType(l, 5, out a5);
			System.Single a6;
			System.Single a7;
			System.Single a8;
			RO.GameObjectMap.InverseTransformVector(a1,a2,a3,a4,a5,out a6,out a7,out a8);
			pushValue(l,true);
			pushValue(l,a6);
			pushValue(l,a7);
			pushValue(l,a8);
			return 4;
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
	static public int TransformPoint_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			checkType(l, 3, out a3);
			System.Single a4;
			checkType(l, 4, out a4);
			System.Single a5;
			checkType(l, 5, out a5);
			System.Single a6;
			System.Single a7;
			System.Single a8;
			RO.GameObjectMap.TransformPoint(a1,a2,a3,a4,a5,out a6,out a7,out a8);
			pushValue(l,true);
			pushValue(l,a6);
			pushValue(l,a7);
			pushValue(l,a8);
			return 4;
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
	static public int SetLocalEulerAngleY_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			checkType(l, 3, out a3);
			RO.GameObjectMap.SetLocalEulerAngleY(a1,a2,a3);
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
	static public int LocalRotateToByAxisY_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			checkType(l, 3, out a3);
			System.Single a4;
			checkType(l, 4, out a4);
			System.Single a5;
			checkType(l, 5, out a5);
			RO.GameObjectMap.LocalRotateToByAxisY(a1,a2,a3,a4,a5);
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
	static public int LocalRotateDeltaByAxisY_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			checkType(l, 3, out a3);
			RO.GameObjectMap.LocalRotateDeltaByAxisY(a1,a2,a3);
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
	static public int SetLocalPosition_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			checkType(l, 3, out a3);
			System.Single a4;
			checkType(l, 4, out a4);
			System.Single a5;
			checkType(l, 5, out a5);
			RO.GameObjectMap.SetLocalPosition(a1,a2,a3,a4,a5);
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
	static public int SetLocalRotation_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			checkType(l, 3, out a3);
			System.Single a4;
			checkType(l, 4, out a4);
			System.Single a5;
			checkType(l, 5, out a5);
			System.Single a6;
			checkType(l, 6, out a6);
			RO.GameObjectMap.SetLocalRotation(a1,a2,a3,a4,a5,a6);
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
	static public int SetLocalEulerAngles_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			checkType(l, 3, out a3);
			System.Single a4;
			checkType(l, 4, out a4);
			System.Single a5;
			checkType(l, 5, out a5);
			RO.GameObjectMap.SetLocalEulerAngles(a1,a2,a3,a4,a5);
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
	static public int SetLocalScale_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			checkType(l, 3, out a3);
			System.Single a4;
			checkType(l, 4, out a4);
			System.Single a5;
			checkType(l, 5, out a5);
			RO.GameObjectMap.SetLocalScale(a1,a2,a3,a4,a5);
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
	static public int SetPosition_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			checkType(l, 3, out a3);
			System.Single a4;
			checkType(l, 4, out a4);
			System.Single a5;
			checkType(l, 5, out a5);
			RO.GameObjectMap.SetPosition(a1,a2,a3,a4,a5);
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
	static public int SetRotation_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			checkType(l, 3, out a3);
			System.Single a4;
			checkType(l, 4, out a4);
			System.Single a5;
			checkType(l, 5, out a5);
			System.Single a6;
			checkType(l, 6, out a6);
			RO.GameObjectMap.SetRotation(a1,a2,a3,a4,a5,a6);
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
	static public int SetEulerAngles_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Int64 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			checkType(l, 3, out a3);
			System.Single a4;
			checkType(l, 4, out a4);
			System.Single a5;
			checkType(l, 5, out a5);
			RO.GameObjectMap.SetEulerAngles(a1,a2,a3,a4,a5);
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
	static public int get_cullingGroupBoundingDistances(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.cullingGroupBoundingDistances);
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
	static public int set_cullingGroupBoundingDistances(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			System.Single[] v;
			checkArray(l,2,out v);
			self.cullingGroupBoundingDistances=v;
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
	static public int get_cullingGroupTypes(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.cullingGroupTypes);
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
	static public int set_cullingGroupTypes(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			System.Int32[] v;
			checkArray(l,2,out v);
			self.cullingGroupTypes=v;
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
	static public int get_cullingGroupOnlySetActiveTypes(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.cullingGroupOnlySetActiveTypes);
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
	static public int set_cullingGroupOnlySetActiveTypes(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			System.Int32[] v;
			checkArray(l,2,out v);
			self.cullingGroupOnlySetActiveTypes=v;
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
	static public int get_cullingGroupSphereCount(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.cullingGroupSphereCount);
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
	static public int set_cullingGroupSphereCount(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.cullingGroupSphereCount=v;
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
	static public int get_cullingGroupReference(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.cullingGroupReference);
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
	static public int set_cullingGroupReference(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			UnityEngine.Transform v;
			checkType(l,2,out v);
			self.cullingGroupReference=v;
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
	static public int get_infoPool(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.GameObjectMap self=(RO.GameObjectMap)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.infoPool);
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
		getTypeTable(l,"RO.GameObjectMap");
		addMember(l,ClearInfoPool);
		addMember(l,GetInfoCountInPool);
		addMember(l,CreateInfo);
		addMember(l,RecycleInfo);
		addMember(l,SetCullingGroupCamera);
		addMember(l,ClearCullingGroupCamera);
		addMember(l,Register_Transform);
		addMember(l,Register_TransformForCulling);
		addMember(l,Unregister);
		addMember(l,Clear);
		addMember(l,ClearAll);
		addMember(l,Get_Transform);
		addMember(l,Get_Info);
		addMember(l,GetChangedInfo);
		addMember(l,UpdateCullingGroup);
		addMember(l,GetTransform_s);
		addMember(l,TransformIsNull_s);
		addMember(l,DestroyGameObject_s);
		addMember(l,SetGameObjectActive_s);
		addMember(l,RestoreBehaviours_s);
		addMember(l,SetRenderQueue_s);
		addMember(l,SetUIDepth_s);
		addMember(l,FollowTransform_s);
		addMember(l,FollowTransformWithRotation_s);
		addMember(l,GetForwardPosition_s);
		addMember(l,GetPosition_s);
		addMember(l,GetLocalPosition_s);
		addMember(l,GetRotation_s);
		addMember(l,GetLocalRotation_s);
		addMember(l,GetEulerAngles_s);
		addMember(l,GetLocalEulerAngles_s);
		addMember(l,GetScale_s);
		addMember(l,GetLocalScale_s);
		addMember(l,InverseTransformPointByVector3_s);
		addMember(l,InverseTransformPointByTransform_s);
		addMember(l,InverseTransformVector_s);
		addMember(l,TransformPoint_s);
		addMember(l,SetLocalEulerAngleY_s);
		addMember(l,LocalRotateToByAxisY_s);
		addMember(l,LocalRotateDeltaByAxisY_s);
		addMember(l,SetLocalPosition_s);
		addMember(l,SetLocalRotation_s);
		addMember(l,SetLocalEulerAngles_s);
		addMember(l,SetLocalScale_s);
		addMember(l,SetPosition_s);
		addMember(l,SetRotation_s);
		addMember(l,SetEulerAngles_s);
		addMember(l,"cullingGroupBoundingDistances",get_cullingGroupBoundingDistances,set_cullingGroupBoundingDistances,true);
		addMember(l,"cullingGroupTypes",get_cullingGroupTypes,set_cullingGroupTypes,true);
		addMember(l,"cullingGroupOnlySetActiveTypes",get_cullingGroupOnlySetActiveTypes,set_cullingGroupOnlySetActiveTypes,true);
		addMember(l,"cullingGroupSphereCount",get_cullingGroupSphereCount,set_cullingGroupSphereCount,true);
		addMember(l,"cullingGroupReference",get_cullingGroupReference,set_cullingGroupReference,true);
		addMember(l,"infoPool",get_infoPool,null,true);
		createTypeMetatable(l,null, typeof(RO.GameObjectMap),typeof(RO.SingleTonGO<RO.GameObjectMap>));
	}
}
