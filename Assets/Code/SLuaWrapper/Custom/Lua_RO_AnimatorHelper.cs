using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_AnimatorHelper : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper o;
			UnityEngine.Animator a1;
			checkType(l, 2, out a1);
			UnityEngine.Animator[] a2;
			checkArray(l, 3, out a2);
			o=new RO.AnimatorHelper(a1,a2);
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
	static public int HasState(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
			System.String a1;
			checkType(l, 2, out a1);
			var ret=self.HasState(a1);
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
	static public int RefreshSubAnimators(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.GameObject[]))){
				RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
				UnityEngine.GameObject[] a1;
				checkArray(l, 2, out a1);
				self.RefreshSubAnimators(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Animator[]))){
				RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
				UnityEngine.Animator[] a1;
				checkArray(l, 2, out a1);
				self.RefreshSubAnimators(a1);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function RefreshSubAnimators to call");
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
	static public int CrossFade(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
				RO.AnimatorHelper.CrossFadeParam a1;
				checkType(l, 2, out a1);
				self.CrossFade(a1);
				pushValue(l,true);
				return 1;
			}
			else if(argc==5){
				RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.Single a2;
				checkType(l, 3, out a2);
				System.Single a3;
				checkType(l, 4, out a3);
				System.Boolean a4;
				checkType(l, 5, out a4);
				self.CrossFade(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(argc==6){
				RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.Single a2;
				checkType(l, 3, out a2);
				System.Single a3;
				checkType(l, 4, out a3);
				System.Boolean a4;
				checkType(l, 5, out a4);
				System.Single a5;
				checkType(l, 6, out a5);
				self.CrossFade(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function CrossFade to call");
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
	static public int CrossFadeForce(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==4){
				RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.Single a2;
				checkType(l, 3, out a2);
				System.Single a3;
				checkType(l, 4, out a3);
				self.CrossFadeForce(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(argc==5){
				RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.Single a2;
				checkType(l, 3, out a2);
				System.Single a3;
				checkType(l, 4, out a3);
				System.Single a4;
				checkType(l, 5, out a4);
				self.CrossFadeForce(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function CrossFadeForce to call");
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
	static public int Play(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
				RO.AnimatorHelper.CrossFadeParam a1;
				checkType(l, 2, out a1);
				self.Play(a1);
				pushValue(l,true);
				return 1;
			}
			else if(argc==4){
				RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.Single a2;
				checkType(l, 3, out a2);
				System.Boolean a3;
				checkType(l, 4, out a3);
				self.Play(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(argc==5){
				RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.Single a2;
				checkType(l, 3, out a2);
				System.Boolean a3;
				checkType(l, 4, out a3);
				System.Single a4;
				checkType(l, 5, out a4);
				self.Play(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function Play to call");
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
	static public int PlayForce(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.Single a2;
				checkType(l, 3, out a2);
				self.PlayForce(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(argc==4){
				RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
				System.String a1;
				checkType(l, 2, out a1);
				System.Single a2;
				checkType(l, 3, out a2);
				System.Single a3;
				checkType(l, 4, out a3);
				self.PlayForce(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function PlayForce to call");
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
	static public int SetSpeed(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
			System.Single a1;
			checkType(l, 2, out a1);
			self.SetSpeed(a1);
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
	static public int ClearSpeed(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
			self.ClearSpeed();
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
	static public int Update(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
			self.Update();
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
	static public int NotifyActionEvent(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
			System.Int32 a1;
			checkType(l, 2, out a1);
			System.Object a2;
			checkType(l, 3, out a2);
			self.NotifyActionEvent(a1,a2);
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
	static public int set_rawListener(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
			RO.AnimatorHelper.RawListener v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self.rawListener=v;
			else if(op==1) self.rawListener+=v;
			else if(op==2) self.rawListener-=v;
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
	static public int set_stateChangedListener(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
			RO.AnimatorHelper.StateChangedListener v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self.stateChangedListener=v;
			else if(op==1) self.stateChangedListener+=v;
			else if(op==2) self.stateChangedListener-=v;
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
	static public int set_loopCountChangedListener(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
			RO.AnimatorHelper.LoopCountChangedListener v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self.loopCountChangedListener=v;
			else if(op==1) self.loopCountChangedListener+=v;
			else if(op==2) self.loopCountChangedListener-=v;
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
	static public int set_actionEventListener(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
			RO.AnimatorHelper.ActionEventListener v;
			int op=checkDelegate(l,2,out v);
			if(op==0) self.actionEventListener=v;
			else if(op==1) self.actionEventListener+=v;
			else if(op==2) self.actionEventListener-=v;
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
	static public int get_defaultStateName(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.defaultStateName);
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
	static public int set_defaultStateName(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.defaultStateName=v;
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
	static public int get_forceDefaultStateName(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.forceDefaultStateName);
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
	static public int set_forceDefaultStateName(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.forceDefaultStateName=v;
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
	static public int get_currentState(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			RO.AnimatorHelper self=(RO.AnimatorHelper)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.currentState);
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
		getTypeTable(l,"RO.AnimatorHelper");
		addMember(l,HasState);
		addMember(l,RefreshSubAnimators);
		addMember(l,CrossFade);
		addMember(l,CrossFadeForce);
		addMember(l,Play);
		addMember(l,PlayForce);
		addMember(l,SetSpeed);
		addMember(l,ClearSpeed);
		addMember(l,Update);
		addMember(l,NotifyActionEvent);
		addMember(l,"rawListener",null,set_rawListener,true);
		addMember(l,"stateChangedListener",null,set_stateChangedListener,true);
		addMember(l,"loopCountChangedListener",null,set_loopCountChangedListener,true);
		addMember(l,"actionEventListener",null,set_actionEventListener,true);
		addMember(l,"defaultStateName",get_defaultStateName,set_defaultStateName,true);
		addMember(l,"forceDefaultStateName",get_forceDefaultStateName,set_forceDefaultStateName,true);
		addMember(l,"currentState",get_currentState,null,true);
		createTypeMetatable(l,constructor, typeof(RO.AnimatorHelper));
	}
}
