using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public static class OptimizationUtils 
	{
		public static void OptiRenderer(Renderer r)
		{
			r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			r.receiveShadows = false;
			r.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
			r.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
		}
	}
} // namespace RO
