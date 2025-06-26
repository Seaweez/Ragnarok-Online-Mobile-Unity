// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "RO/SceneObject/Unlit" {
	Properties 
	{
		[HideInInspector] _MainTex ("Albedo", 2D) = "white" {}
		[HideInInspector] _Color("Color", Color) = (1,1,1,1)
		[HideInInspector] _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
		
		[HideInInspector] _Mode ("__mode", Float) = 0.0
		[HideInInspector] _SrcBlend ("__src", Float) = 1.0
		[HideInInspector] _DstBlend ("__dst", Float) = 0.0
		[HideInInspector] _ZWrite ("__zw", Float) = 1.0

		[HideInInspector] _CutX ("__cutx", Float) = 0.0
		[HideInInspector] _CutY ("__cuty", Float) = 0.0

	    [Toggle(_NO_FOG_ON)]_NoFog ("No Fog", Float) = 0
	}
	
	CGINCLUDE
	#include "UnityCG.cginc"
	struct appdata_scene
	{
		half4 vertex : POSITION;
		half4 texcoord : TEXCOORD0;
		#ifndef LIGHTMAP_OFF
		half4 texcoord1 : TEXCOORD1;
		#endif
	};
	struct v2f
	{
		half4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
		UNITY_FOG_COORDS(1)
	};
	
	uniform sampler2D _MainTex;
	uniform half4 _MainTex_ST;
	uniform half4 _Color;
	uniform half _Cutoff;
	uniform half _CutX;
	uniform half _CutY;
	
	ENDCG

	SubShader {
		Tags { }
		
		Pass {
			Blend [_SrcBlend] [_DstBlend]
			ZWrite [_ZWrite]

			CGPROGRAM
			
			#pragma multi_compile __ _ALPHATEST_ON
//			#pragma multi_compile __ _ALPHABLEND_ON
//			#pragma multi_compile __ _ALPHAPREMULTIPLY_ON
			#pragma multi_compile __ _NO_FOG_ON
			#pragma multi_compile __ _CUT_ON
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			v2f vert(appdata_scene v) 
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				
				#if !defined(_NO_FOG_ON)
				UNITY_TRANSFER_FOG(o, o.pos);
				#endif
				return o;
			}
			
			half4 frag(v2f i) : COLOR {	
				half4 c = tex2D (_MainTex, i.uv);
				c.rgb *= _Color.rgb;
				
				#if defined(_ALPHATEST_ON)
				clip (c.a - _Cutoff);
				#endif				
				
				half4 lightColor = UNITY_LIGHTMODEL_AMBIENT;
				c.rgb *= lightColor.rgb * lightColor.a;
				
				#if !defined(_NO_FOG_ON)
				UNITY_APPLY_FOG(i.fogCoord, c);
				#endif

				#if defined(_CUT_ON)
				if (_CutY > i.uv.y || (1-_CutY) < i.uv.y)
				{
					c.a = 0;
				}
				else if (_CutX > i.uv.x || (1-_CutX) < i.uv.x)
				{
					c.a = 0;
				}
				#endif
				return c;			
			}
			
			ENDCG
		}
	} 
	FallBack "Standard"
	CustomEditor "SceneObjectShaderGUI"
}
