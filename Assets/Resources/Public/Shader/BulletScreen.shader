// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "RO/Effect/BulletScreen" {
	Properties 
	{
		_MainTex ("Albedo", 2D) = "white" {}
		_TintColor("Tint Color", Color) = (1,1,1,1)
		_Exposure("Exposure", Range(0.0, 3.0)) = 1.0
	}
	
	CGINCLUDE
	#include "UnityCG.cginc"
	struct appdata_bullet
	{
		fixed4 vertex : POSITION;
		fixed4 texcoord : TEXCOORD0;
		fixed4 color : COLOR;
	};
	
	struct v2f
	{
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		fixed4 color : COLOR;
	};
	
	uniform sampler2D _MainTex;
	uniform float4 _MainTex_ST;
	uniform float4 _TintColor;
	uniform float _Exposure;
	
	ENDCG

	SubShader {
		Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
		
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite off Lighting Off
		  	AlphaTest Greater 0.01
			ColorMask RGB
		  	
		  	CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			v2f vert(appdata_bullet v) 
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.color = v.color;
				return o;
			}
			
			fixed4 frag(v2f i) : COLOR {
				fixed4 c = tex2D (_MainTex, i.uv);
				c.rgb *= _Exposure * _TintColor.rgb;
//				c.a = (c.r+c.g+c.b)/3;
//				c.a = _TintColor.a;
				c.a = (c.r+c.g+c.b);
				return c;
			}
			
			ENDCG
		}
	}
}
