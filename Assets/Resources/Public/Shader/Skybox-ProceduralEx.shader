Shader "RO/Skybox/ProceduralEx" {
Properties {
	_SunSize ("Sun size", Range(0,1)) = 0.04
	_AtmosphereThickness ("Atmoshpere Thickness", Range(0,5)) = 1.0
	_SkyTint ("Sky Tint", Color) = (.5, .5, .5, 1)
	_GroundColor ("Ground", Color) = (.369, .349, .341, 1)

	_Exposure("Exposure", Range(0, 8)) = 1.3
	
	_Rotation ("Rotation", Range(0, 360)) = 0
	
	[NoScaleOffset] _Tex ("Cubemap", Cube) = "transparent" {}
	[NoScaleOffset] _TexAlpha ("Cubemap Alpha", Cube) = "white" {}
	[MaterialToggle] UseTexAlpha ("Use TexAlpha", Float) = 0
	_TexTint ("Cubemap Tint", Color) = (1, 1, 1, 0)
}

SubShader {
	Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
	Cull Off ZWrite Off Fog { Mode Off }

	Pass {
		
		CGPROGRAM
		#pragma multi_compile _ USETEXALPHA_ON
		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#include "SkyboxHelper.cginc"
		
		uniform samplerCUBE _Tex;
		uniform samplerCUBE _TexAlpha;
//		uniform half4 _Tex_HDR;
		uniform half4 _TexTint;

		v2f vert (appdata_t v)
		{
			return vertProcedural(v);
		}

		half4 frag (v2f IN) : SV_Target
		{
			half3 col = fragProcedural(IN);
			
			half4 tex = texCUBE (_Tex, IN.texcoord);
#ifdef USETEXALPHA_ON
			half4 texAlpha = texCUBE (_TexAlpha, IN.texcoord);
			tex.a = texAlpha.r;
#endif

			tex *= _TexTint;

			float3 step = lerp(col, tex.rgb, tex.a);
			return  half4(step, 1.0);

		}
		ENDCG 
	}
} 	


Fallback "Skybox/Procedural"

}
