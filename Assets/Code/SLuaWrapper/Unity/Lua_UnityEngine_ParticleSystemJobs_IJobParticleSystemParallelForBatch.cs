using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_ParticleSystemJobs_IJobParticleSystemParallelForBatch : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Execute(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#endif
			UnityEngine.ParticleSystemJobs.IJobParticleSystemParallelForBatch self=(UnityEngine.ParticleSystemJobs.IJobParticleSystemParallelForBatch)checkSelf(l);
			UnityEngine.ParticleSystemJobs.ParticleSystemJobData a1;
			checkValueType(l, 2, out a1);
			System.Int32 a2;
			checkType(l, 3, out a2);
			System.Int32 a3;
			checkType(l, 4, out a3);
			self.Execute(a1,a2,a3);
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
		getTypeTable(l,"UnityEngine.ParticleSystemJobs.IJobParticleSystemParallelForBatch");
		addMember(l,Execute);
		createTypeMetatable(l,null, typeof(UnityEngine.ParticleSystemJobs.IJobParticleSystemParallelForBatch));
	}
}
