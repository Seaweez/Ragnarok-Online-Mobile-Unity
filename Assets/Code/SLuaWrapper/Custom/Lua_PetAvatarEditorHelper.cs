using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_PetAvatarEditorHelper : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetStates(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			PetAvatarEditorHelper self=(PetAvatarEditorHelper)checkSelf(l);
			UnityEngine.Animator a1;
			checkType(l, 2, out a1);
			var ret=self.GetStates(a1);
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
	static public int PlayAnim(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			PetAvatarEditorHelper self=(PetAvatarEditorHelper)checkSelf(l);
			System.String a1;
			checkType(l, 2, out a1);
			System.String a2;
			checkType(l, 3, out a2);
			self.PlayAnim(a1,a2);
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
	static public int AutoCalculateCollider(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			PetAvatarEditorHelper self=(PetAvatarEditorHelper)checkSelf(l);
			UnityEngine.Transform a1;
			checkType(l, 2, out a1);
			var ret=self.AutoCalculateCollider(a1);
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
	static public int SetUndoListener(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			PetAvatarEditorHelper self=(PetAvatarEditorHelper)checkSelf(l);
			System.Action a1;
			checkDelegate(l,2,out a1);
			System.Action a2;
			checkDelegate(l,3,out a2);
			self.SetUndoListener(a1,a2);
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
		getTypeTable(l,"PetAvatarEditorHelper");
		addMember(l,GetStates);
		addMember(l,PlayAnim);
		addMember(l,AutoCalculateCollider);
		addMember(l,SetUndoListener);
		createTypeMetatable(l,null, typeof(PetAvatarEditorHelper),typeof(UnityEngine.MonoBehaviour));
	}
}
