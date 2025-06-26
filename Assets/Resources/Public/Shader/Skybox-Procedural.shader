Shader "RO/Skybox/Procedural" {
Properties {
	[KeywordEnum(None, Simple, High Quality)] _SunDisk ("Sun", Int) = 2
	_SunSize ("Sun Size", Range(0,1)) = 0.04
	
	_AtmosphereThickness ("Atmoshpere Thickness", Range(0,5)) = 1.0
	_SkyTint ("Sky Tint", Color) = (.5, .5, .5, 1)
	_GroundColor ("Ground", Color) = (.369, .349, .341, 1)

	_Exposure("Exposure", Range(0, 8)) = 1.3
}

SubShader {
	Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
	Cull Off ZWrite Off

	Pass {

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#include "SkyboxHelper.cginc"

		v2f vert (appdata_t v)
		{
			return vertProcedural(v);
		}
		half4 frag (v2f IN) : SV_Target
		{
			half3 col = fragProcedural(IN);
			return half4(col,1.0);
		}
		ENDCG 
	}
}


Fallback Off

}
