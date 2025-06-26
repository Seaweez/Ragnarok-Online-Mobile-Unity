Shader "RO/Skybox/ProceduralEx-Mobile" {
Properties {
	_SunSize ("Sun size", Range(0,1)) = 0.04
	_AtmosphereThickness ("Atmoshpere Thickness", Range(0,5)) = 1.0
	_SkyTint ("Sky Tint", Color) = (.5, .5, .5, 1)
	_GroundColor ("Ground", Color) = (.369, .349, .341, 1)

	_Exposure("Exposure", Range(0, 8)) = 1.3
	
	_Rotation ("Rotation", Range(0, 360)) = 0
	
	[NoScaleOffset]_FrontTex ("Front (+Z)", 2D) = "transparent" {}
	[NoScaleOffset]_BackTex ("Back (-Z)", 2D) = "transparent" {}
	[NoScaleOffset]_LeftTex ("Left (+X)", 2D) = "transparent" {}
	[NoScaleOffset]_RightTex ("Right (-X)", 2D) = "transparent" {}
	[NoScaleOffset]_UpTex ("Up (+Y)", 2D) = "transparent" {}
	[NoScaleOffset]_DownTex ("Down (-Y)", 2D) = "transparent" {}
	
	_TexTint ("Tex Tint", Color) = (1, 1, 1, 0)
}

CGINCLUDE
#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "SkyboxHelper.cginc"
uniform half4 _TexTint;

v2f vert (appdata_t v)
{
	v2f OUT = vertProcedural(v);
	OUT.texcoord.xy = v.texcoord;
	return OUT;
}

half4 fragFace (v2f IN, sampler2D smp) : SV_Target
{
	half3 col = fragProcedural(IN);
	half4 tex = tex2D (smp, IN.texcoord.xy);
	tex *= _TexTint;
	return half4(lerp(col, tex.rgb, tex.a),1.0);
}

ENDCG


SubShader {
	Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
	Cull Off ZWrite Off Fog { Mode Off }

	Pass {
		CGPROGRAM
		
		#pragma vertex vert
		#pragma fragment frag
		
		uniform sampler2D _FrontTex;
		
		half4 frag (v2f IN) : SV_Target
		{
			return fragFace(IN, _FrontTex);
		}
		ENDCG 
	}
	Pass {
		CGPROGRAM
		
		#pragma vertex vert
		#pragma fragment frag
		
		uniform sampler2D _BackTex;

		half4 frag (v2f IN) : SV_Target
		{
			return fragFace(IN, _BackTex);
		}
		ENDCG 
	}
	Pass {
		CGPROGRAM
		
		#pragma vertex vert
		#pragma fragment frag
		
		uniform sampler2D _LeftTex;

		half4 frag (v2f IN) : SV_Target
		{
			return fragFace(IN, _LeftTex);
		}
		ENDCG 
	}
	Pass {
		CGPROGRAM
		
		#pragma vertex vert
		#pragma fragment frag
		
		uniform sampler2D _RightTex;

		half4 frag (v2f IN) : SV_Target
		{
			return fragFace(IN, _RightTex);
		}
		ENDCG 
	}
	Pass {
		CGPROGRAM
		
		#pragma vertex vert
		#pragma fragment frag
		
		uniform sampler2D _UpTex;

		half4 frag (v2f IN) : SV_Target
		{
			return fragFace(IN, _UpTex);
		}
		ENDCG 
	}
	Pass {
		CGPROGRAM
		
		#pragma vertex vert
		#pragma fragment frag
		
		uniform sampler2D _DownTex;

		half4 frag (v2f IN) : SV_Target
		{
			return fragFace(IN, _DownTex);
		}
		ENDCG 
	}
} 	

Fallback "Skybox/Procedural"

}
