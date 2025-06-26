// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "RO/UI/Mask" 
{
	Properties 
	{
		_MainTex ("Mask", 2D) = "black" {}
	}
	
	CGINCLUDE
	#include "UnityCG.cginc"
	struct appdata_mask
	{
		half4 vertex : POSITION;
		half4 texcoord : TEXCOORD0;
		half4 color : COLOR;
	};
	
	struct v2f
	{
		half4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
		half4 color : COLOR;
	};
	
	uniform sampler2D _MainTex;
	uniform half4 _MainTex_ST;
	
	ENDCG

	SubShader {
		Tags { "RenderType"="Transparent" }
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass {
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			v2f vert(appdata_mask v) 
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.color = v.color;
				return o;
			}
			
			half4 frag(v2f i) : COLOR 
			{				
				half4 col = tex2D (_MainTex, i.uv);
				col.rgb = i.color.rgb;
				col.a = (1 - col.a) * i.color.a;
				return col;
			}
			
			ENDCG
		}
	} 
}