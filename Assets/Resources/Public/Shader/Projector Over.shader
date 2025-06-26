// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced '_ProjectorClip' with 'unity_ProjectorClip'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "RO/Projector/Over" { 
	Properties {
		_MainTex ("Main Texture", 2D) = "gray" {}
		_FalloffTex ("FallOff", 2D) = "white" {}
		_TintColor("Tint Color", Color) = (1,1,1,1)
		_Exposure("Exposure", Range(0.0, 10.0)) = 2.0
	}
	Subshader {
		Tags {"Queue"="Transparent" "RenderType"="Transparent"}
		Pass {
			ZWrite Off
			Fog { Color (1, 1, 1) }
			AlphaTest Greater 0
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			Offset -1, -1
 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct v2f {
				float4 uv: TEXCOORD0;
				float4 uvFalloff : TEXCOORD1;
				float4 pos : SV_POSITION;
			};
			
			float4x4 unity_Projector;
			float4x4 unity_ProjectorClip;
			
			v2f vert (float4 vertex : POSITION)
			{
				v2f o;
				o.pos = UnityObjectToClipPos (vertex);
				o.uv = mul (unity_Projector, vertex);
				o.uvFalloff = mul (unity_ProjectorClip, vertex);
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _FalloffTex;
			fixed4 _TintColor;
			float _Exposure;
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 texS = tex2Dproj (_MainTex, UNITY_PROJ_COORD(i.uv));
//				texS.a = 1.0-texS.a;
				
				fixed4 texF = tex2Dproj (_FalloffTex, UNITY_PROJ_COORD(i.uvFalloff));
//				if (0.5 >= texF.a)
//				{
//					return fixed4(1,1,1,0);
//				}
				
				texS.rgb = texS.rgb*_TintColor.rgb;
//				if (0.01 >= texF.a)
//				{
//					texS.a = 0;
//				}
//				texS.a *= texF.a * _Exposure;
//				return texS;
				fixed4 res = lerp(fixed4(1,1,1,0), texS, texF.a * _Exposure);
				return res;
			}
			ENDCG
		}
	}
}
