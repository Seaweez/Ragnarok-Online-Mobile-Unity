using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RO_ModelUtils : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsThreeDMaxImportedModel_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.Transform a1;
			checkType(l, 1, out a1);
			var ret=RO.ModelUtils.IsThreeDMaxImportedModel(a1);
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
	static public int AdjustSize_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.GameObject a1;
			checkType(l, 1, out a1);
			System.Single a2;
			checkType(l, 2, out a2);
			RO.ModelUtils.AdjustSize(a1,a2);
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
	static public int AdjustProjectorSize_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				UnityEngine.GameObject a1;
				checkType(l, 1, out a1);
				System.Single a2;
				checkType(l, 2, out a2);
				var ret=RO.ModelUtils.AdjustProjectorSize(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				UnityEngine.Projector a1;
				checkType(l, 1, out a1);
				System.Single a2;
				checkType(l, 2, out a2);
				System.Single a3;
				checkType(l, 3, out a3);
				RO.ModelUtils.AdjustProjectorSize(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function AdjustProjectorSize to call");
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
	static public int AdjustProjector_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.GameObject a1;
			checkType(l, 1, out a1);
			UnityEngine.Projector a2;
			checkType(l, 2, out a2);
			System.Single a3;
			checkType(l, 3, out a3);
			System.Single a4;
			checkType(l, 4, out a4);
			RO.ModelUtils.AdjustProjector(a1,a2,a3,a4);
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
	static public int AdjustSpriteSize_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(UnityEngine.SpriteRenderer),typeof(float))){
				UnityEngine.SpriteRenderer a1;
				checkType(l, 1, out a1);
				System.Single a2;
				checkType(l, 2, out a2);
				RO.ModelUtils.AdjustSpriteSize(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.GameObject),typeof(float))){
				UnityEngine.GameObject a1;
				checkType(l, 1, out a1);
				System.Single a2;
				checkType(l, 2, out a2);
				var ret=RO.ModelUtils.AdjustSpriteSize(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function AdjustSpriteSize to call");
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
	static public int AdjustSprite_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.GameObject a1;
			checkType(l, 1, out a1);
			UnityEngine.SpriteRenderer a2;
			checkType(l, 2, out a2);
			System.Single a3;
			checkType(l, 3, out a3);
			RO.ModelUtils.AdjustSprite(a1,a2,a3);
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
	static public int get_threeDMaxImportedModelRotation(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			pushValue(l,true);
			pushValue(l,RO.ModelUtils.threeDMaxImportedModelRotation);
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
		getTypeTable(l,"RO.ModelUtils");
		addMember(l,IsThreeDMaxImportedModel_s);
		addMember(l,AdjustSize_s);
		addMember(l,AdjustProjectorSize_s);
		addMember(l,AdjustProjector_s);
		addMember(l,AdjustSpriteSize_s);
		addMember(l,AdjustSprite_s);
		addMember(l,"threeDMaxImportedModelRotation",get_threeDMaxImportedModelRotation,null,false);
		createTypeMetatable(l,null, typeof(RO.ModelUtils));
	}
}
