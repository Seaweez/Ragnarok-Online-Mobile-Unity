using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_LuaGameObject : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetProperty(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.LuaGameObject self=(RO.LuaGameObject)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			var ret=self.GetProperty(a1);
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
	static public int GetComponentProperty(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.LuaGameObject self=(RO.LuaGameObject)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			var ret=self.GetComponentProperty(a1);
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
	static public int Register(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.LuaGameObject self=(RO.LuaGameObject)checkSelf(l);
			self.Register();
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
	static public int Unregister(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.LuaGameObject self=(RO.LuaGameObject)checkSelf(l);
			self.Unregister();
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
	static public int ObjectIsNull_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Object a1;
			checkType(l, 1, out a1);
			var ret=RO.LuaGameObject.ObjectIsNull(a1);
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
	static public int DestroyObject_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Object a1;
			checkType(l, 1, out a1);
			RO.LuaGameObject.DestroyObject(a1);
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
	static public int DestroyGameObject_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Component a1;
			checkType(l, 1, out a1);
			RO.LuaGameObject.DestroyGameObject(a1);
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
			UnityEngine.GameObject a1;
			checkType(l, 1, out a1);
			RO.LuaGameObject.RestoreBehaviours(a1);
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
			UnityEngine.GameObject a1;
			checkType(l, 1, out a1);
			System.Int32 a2;
			checkType(l, 2, out a2);
			RO.LuaGameObject.SetRenderQueue(a1,a2);
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
			UnityEngine.GameObject a1;
			checkType(l, 1, out a1);
			System.Int32 a2;
			checkType(l, 2, out a2);
			RO.LuaGameObject.SetUIDepth(a1,a2);
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
			UnityEngine.Transform a1;
			checkType(l, 1, out a1);
			UnityEngine.Transform a2;
			checkType(l, 2, out a2);
			UnityEngine.Vector3 a3;
			checkType(l, 3, out a3);
			RO.LuaGameObject.FollowTransform(a1,a2,a3);
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
			UnityEngine.Transform a1;
			checkType(l, 1, out a1);
			UnityEngine.Transform a2;
			checkType(l, 2, out a2);
			UnityEngine.Vector3 a3;
			checkType(l, 3, out a3);
			RO.LuaGameObject.FollowTransformWithRotation(a1,a2,a3);
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
			UnityEngine.Transform a1;
			checkType(l, 1, out a1);
			System.Single a2;
			checkType(l, 2, out a2);
			System.Single a3;
			System.Single a4;
			System.Single a5;
			RO.LuaGameObject.GetForwardPosition(a1,a2,out a3,out a4,out a5);
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
	static public int GetPosition_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Transform a1;
			checkType(l, 1, out a1);
			System.Single a2;
			System.Single a3;
			System.Single a4;
			RO.LuaGameObject.GetPosition(a1,out a2,out a3,out a4);
			pushValue(l,true);
			pushValue(l,a2);
			pushValue(l,a3);
			pushValue(l,a4);
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
			UnityEngine.Transform a1;
			checkType(l, 1, out a1);
			System.Single a2;
			System.Single a3;
			System.Single a4;
			RO.LuaGameObject.GetLocalPosition(a1,out a2,out a3,out a4);
			pushValue(l,true);
			pushValue(l,a2);
			pushValue(l,a3);
			pushValue(l,a4);
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
			UnityEngine.Transform a1;
			checkType(l, 1, out a1);
			System.Single a2;
			System.Single a3;
			System.Single a4;
			System.Single a5;
			RO.LuaGameObject.GetRotation(a1,out a2,out a3,out a4,out a5);
			pushValue(l,true);
			pushValue(l,a2);
			pushValue(l,a3);
			pushValue(l,a4);
			pushValue(l,a5);
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
			UnityEngine.Transform a1;
			checkType(l, 1, out a1);
			System.Single a2;
			System.Single a3;
			System.Single a4;
			System.Single a5;
			RO.LuaGameObject.GetLocalRotation(a1,out a2,out a3,out a4,out a5);
			pushValue(l,true);
			pushValue(l,a2);
			pushValue(l,a3);
			pushValue(l,a4);
			pushValue(l,a5);
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
			UnityEngine.Transform a1;
			checkType(l, 1, out a1);
			System.Single a2;
			System.Single a3;
			System.Single a4;
			RO.LuaGameObject.GetEulerAngles(a1,out a2,out a3,out a4);
			pushValue(l,true);
			pushValue(l,a2);
			pushValue(l,a3);
			pushValue(l,a4);
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
			UnityEngine.Transform a1;
			checkType(l, 1, out a1);
			System.Single a2;
			System.Single a3;
			System.Single a4;
			RO.LuaGameObject.GetLocalEulerAngles(a1,out a2,out a3,out a4);
			pushValue(l,true);
			pushValue(l,a2);
			pushValue(l,a3);
			pushValue(l,a4);
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
			UnityEngine.Transform a1;
			checkType(l, 1, out a1);
			System.Single a2;
			System.Single a3;
			System.Single a4;
			RO.LuaGameObject.GetScale(a1,out a2,out a3,out a4);
			pushValue(l,true);
			pushValue(l,a2);
			pushValue(l,a3);
			pushValue(l,a4);
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
			UnityEngine.Transform a1;
			checkType(l, 1, out a1);
			System.Single a2;
			System.Single a3;
			System.Single a4;
			RO.LuaGameObject.GetLocalScale(a1,out a2,out a3,out a4);
			pushValue(l,true);
			pushValue(l,a2);
			pushValue(l,a3);
			pushValue(l,a4);
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
			UnityEngine.Transform a1;
			checkType(l, 1, out a1);
			System.Single a2;
			checkType(l, 2, out a2);
			RO.LuaGameObject.SetLocalEulerAngleY(a1,a2);
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
			UnityEngine.Transform a1;
			checkType(l, 1, out a1);
			UnityEngine.Vector3 a2;
			checkType(l, 2, out a2);
			RO.LuaGameObject.LocalRotateToByAxisY(a1,a2);
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
			UnityEngine.Transform a1;
			checkType(l, 1, out a1);
			System.Single a2;
			checkType(l, 2, out a2);
			RO.LuaGameObject.LocalRotateDeltaByAxisY(a1,a2);
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
	static public int InverseTransformPointByVector3_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Transform a1;
			checkType(l, 1, out a1);
			UnityEngine.Vector3 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			System.Single a4;
			System.Single a5;
			RO.LuaGameObject.InverseTransformPointByVector3(a1,a2,out a3,out a4,out a5);
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
	static public int InverseTransformPointByTransform_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Transform a1;
			checkType(l, 1, out a1);
			UnityEngine.Transform a2;
			checkType(l, 2, out a2);
			UnityEngine.Space a3;
			checkEnum(l,3,out a3);
			System.Single a4;
			System.Single a5;
			System.Single a6;
			RO.LuaGameObject.InverseTransformPointByTransform(a1,a2,a3,out a4,out a5,out a6);
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
	static public int InverseTransformVector_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Transform a1;
			checkType(l, 1, out a1);
			UnityEngine.Vector3 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			System.Single a4;
			System.Single a5;
			RO.LuaGameObject.InverseTransformVector(a1,a2,out a3,out a4,out a5);
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
	static public int TransformPoint_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Transform a1;
			checkType(l, 1, out a1);
			UnityEngine.Vector3 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			System.Single a4;
			System.Single a5;
			RO.LuaGameObject.TransformPoint(a1,a2,out a3,out a4,out a5);
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
	static public int WorldToViewportPointByVector3_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Camera a1;
			checkType(l, 1, out a1);
			UnityEngine.Vector3 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			System.Single a4;
			System.Single a5;
			RO.LuaGameObject.WorldToViewportPointByVector3(a1,a2,out a3,out a4,out a5);
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
	static public int WorldToViewportPointByTransform_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Camera a1;
			checkType(l, 1, out a1);
			UnityEngine.Transform a2;
			checkType(l, 2, out a2);
			UnityEngine.Space a3;
			checkEnum(l,3,out a3);
			System.Single a4;
			System.Single a5;
			System.Single a6;
			RO.LuaGameObject.WorldToViewportPointByTransform(a1,a2,a3,out a4,out a5,out a6);
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
	static public int ScreenToWorldPointByVector3_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Camera a1;
			checkType(l, 1, out a1);
			UnityEngine.Vector3 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			System.Single a4;
			System.Single a5;
			RO.LuaGameObject.ScreenToWorldPointByVector3(a1,a2,out a3,out a4,out a5);
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
	static public int ScreenToWorldPointByTransform_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Camera a1;
			checkType(l, 1, out a1);
			UnityEngine.Transform a2;
			checkType(l, 2, out a2);
			UnityEngine.Space a3;
			checkEnum(l,3,out a3);
			System.Single a4;
			System.Single a5;
			System.Single a6;
			RO.LuaGameObject.ScreenToWorldPointByTransform(a1,a2,a3,out a4,out a5,out a6);
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
	static public int WorldToScreenPointByVector3_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Camera a1;
			checkType(l, 1, out a1);
			UnityEngine.Vector3 a2;
			checkType(l, 2, out a2);
			System.Single a3;
			System.Single a4;
			System.Single a5;
			RO.LuaGameObject.WorldToScreenPointByVector3(a1,a2,out a3,out a4,out a5);
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
	static public int WorldToScreenPointByTransform_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Camera a1;
			checkType(l, 1, out a1);
			UnityEngine.Transform a2;
			checkType(l, 2, out a2);
			UnityEngine.Space a3;
			checkEnum(l,3,out a3);
			System.Single a4;
			System.Single a5;
			System.Single a6;
			RO.LuaGameObject.WorldToScreenPointByTransform(a1,a2,a3,out a4,out a5,out a6);
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
	static public int GetTouchPosition_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Boolean a2;
			checkType(l, 2, out a2);
			System.Single a3;
			System.Single a4;
			RO.LuaGameObject.GetTouchPosition(a1,a2,out a3,out a4);
			pushValue(l,true);
			pushValue(l,a3);
			pushValue(l,a4);
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
	static public int GetTouchDeltaPosition_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Int32 a1;
			checkType(l, 1, out a1);
			System.Single a2;
			System.Single a3;
			RO.LuaGameObject.GetTouchDeltaPosition(a1,out a2,out a3);
			pushValue(l,true);
			pushValue(l,a2);
			pushValue(l,a3);
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
	static public int GetMousePosition_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			System.Single a1;
			System.Single a2;
			System.Single a3;
			RO.LuaGameObject.GetMousePosition(out a1,out a2,out a3);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			pushValue(l,a3);
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
	static public int TrimNameClone_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.GameObject a1;
			checkType(l, 1, out a1);
			RO.LuaGameObject.TrimNameClone(a1);
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
	static public int get_type(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.LuaGameObject self=(RO.LuaGameObject)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.type);
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
	static public int set_type(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.LuaGameObject self=(RO.LuaGameObject)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.type=v;
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
	static public int get_ID(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.LuaGameObject self=(RO.LuaGameObject)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ID);
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
	static public int set_ID(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.LuaGameObject self=(RO.LuaGameObject)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.ID=v;
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
	static public int get_properties(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.LuaGameObject self=(RO.LuaGameObject)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.properties);
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
	static public int set_properties(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.LuaGameObject self=(RO.LuaGameObject)checkSelf(l);
			System.String[] v;
			checkArray(l,2,out v);
			self.properties=v;
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
	static public int get_components(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.LuaGameObject self=(RO.LuaGameObject)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.components);
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
	static public int set_components(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.LuaGameObject self=(RO.LuaGameObject)checkSelf(l);
			UnityEngine.Component[] v;
			checkArray(l,2,out v);
			self.components=v;
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
	static public int get_registered(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.LuaGameObject self=(RO.LuaGameObject)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.registered);
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
		getTypeTable(l,"RO.LuaGameObject");
		addMember(l,GetProperty);
		addMember(l,GetComponentProperty);
		addMember(l,Register);
		addMember(l,Unregister);
		addMember(l,ObjectIsNull_s);
		addMember(l,DestroyObject_s);
		addMember(l,DestroyGameObject_s);
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
		addMember(l,SetLocalEulerAngleY_s);
		addMember(l,LocalRotateToByAxisY_s);
		addMember(l,LocalRotateDeltaByAxisY_s);
		addMember(l,InverseTransformPointByVector3_s);
		addMember(l,InverseTransformPointByTransform_s);
		addMember(l,InverseTransformVector_s);
		addMember(l,TransformPoint_s);
		addMember(l,WorldToViewportPointByVector3_s);
		addMember(l,WorldToViewportPointByTransform_s);
		addMember(l,ScreenToWorldPointByVector3_s);
		addMember(l,ScreenToWorldPointByTransform_s);
		addMember(l,WorldToScreenPointByVector3_s);
		addMember(l,WorldToScreenPointByTransform_s);
		addMember(l,GetTouchPosition_s);
		addMember(l,GetTouchDeltaPosition_s);
		addMember(l,GetMousePosition_s);
		addMember(l,TrimNameClone_s);
		addMember(l,"type",get_type,set_type,true);
		addMember(l,"ID",get_ID,set_ID,true);
		addMember(l,"properties",get_properties,set_properties,true);
		addMember(l,"components",get_components,set_components,true);
		addMember(l,"registered",get_registered,null,true);
		createTypeMetatable(l,null, typeof(RO.LuaGameObject),typeof(UnityEngine.MonoBehaviour));
	}
}
