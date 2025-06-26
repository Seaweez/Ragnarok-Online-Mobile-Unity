// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "RO/WaterSimpleWave"
{
	Properties
	{		
		_WaterTex ("Water Texture", 2D) = "white" {}	
		_WaterNormalTex ("Water Normal Map", 2D) = "white" {}
		_WaterColor ("Water Color", Color) = (1, 1, 1, 1)
		_WaterAlpha ("Water Alpha", Range(0, 1)) = 0.45
		_TimeFactor ("Water Flow Factor", Range(0,20)) = 0.5	
		_WaveHeightFactor ("Water Wave Height Factor", Range(0, 10)) = 0.1

	    [Toggle(_NO_FOG_ON)]_NoFog ("No Fog", Float) = 0

		[Toggle(_SMOOTH_TRANSPARENT_ON)]_SmoothTransparen ("Smooth Transparen On", Float) = 0
		[HideInInspector]_TransparentDistance ("Water Transparent Distance", Range(0, 2)) = 1
	}
	CGINCLUDE
	#include "UnityCG.cginc"
	struct v2f
	{
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float2 uv1 : TEXCOORD1;
		#if defined(_SMOOTH_TRANSPARENT_ON)
		float2 uv2 : TEXCOORD2;
		UNITY_FOG_COORDS(3)
		#else
		UNITY_FOG_COORDS(2)
		#endif
	};
	uniform sampler2D _WaterTex;
	float4 _WaterTex_ST;
	uniform sampler2D _WaterNormalTex;
	float4 _WaterNormalTex_ST;
	uniform fixed4 _WaterColor;
	uniform fixed _WaterAlpha;
	uniform half _TimeFactor;
	uniform float _WaveHeightFactor;

	uniform float _TransparentDistance;
	ENDCG
	SubShader
	{		
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		Pass {
			Cull Back
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma multi_compile __ _SMOOTH_TRANSPARENT_ON
			#pragma multi_compile __ _NO_FOG_ON
			#pragma target 4.0 FOO BAR
			
			v2f vert(appdata_full v)
			{
				v2f o;				
				o.uv = TRANSFORM_TEX(v.texcoord, _WaterTex).xy;
				float time = _Time.x * _TimeFactor;
				o.uv.x += time;
				o.uv.y += 0.5 * time;				
				o.uv1 = TRANSFORM_TEX(v.texcoord1, _WaterNormalTex).xy;
				o.pos = UnityObjectToClipPos(v.vertex);	

				#if defined(_SMOOTH_TRANSPARENT_ON)
				o.uv2 = v.texcoord1.xy;
				#endif

				#if !defined(_NO_FOG_ON)
				UNITY_TRANSFER_FOG(o, o.pos);		
				#endif	
				return o;
			}
			
			fixed4 frag(v2f i) : COLOR
			{
				fixed3 nml = UnpackNormal(tex2D(_WaterNormalTex, i.uv1));
				i.uv += _WaveHeightFactor * nml.xy;
				fixed4 c = tex2D(_WaterTex, i.uv); 
				c *= _WaterColor;
				c.a = _WaterAlpha;

				#if defined(_SMOOTH_TRANSPARENT_ON)
				float2 t = i.uv2-0.5;
				c.a *= 1-sqrt(t.x*t.x+t.y*t.y)*2*_TransparentDistance;
				#endif

				#if !defined(_NO_FOG_ON)
				UNITY_APPLY_FOG(i.fogCoord, c);
				#endif	
				return c;
			}
			ENDCG	
		}
	}
	CustomEditor "WaterSimpleWaveShaderGUI"
}