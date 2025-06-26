// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "RO/Normal" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_TintColor("Tint Color", Color) = (1,1,1,1)

		[Toggle(_CUTOFF_ON)]_CutoffEnable ("Cutoff", Float) = 0
	    [Toggle(_NO_FOG_ON)]_NoFog ("No Fog", Float) = 0
	}
	
	CGINCLUDE
	#include "UnityCG.cginc"
	struct appdata_normal
	{
		fixed4 vertex : POSITION;
		fixed4 texcoord : TEXCOORD0;
	};
	
	struct v2f
	{
		fixed4 pos : SV_POSITION;
		fixed2 uv : TEXCOORD0;
		fixed4 color : COLOR;
		UNITY_FOG_COORDS(1)
	};
	
	uniform sampler2D _MainTex;
	uniform float4 _MainTex_ST;
	uniform float4 _TintColor;
	
	ENDCG

	SubShader {
		Tags { "Queue" = "Geometry" }
		
		Pass {
			CGPROGRAM

			#pragma multi_compile __ _CUTOFF_ON
			#pragma multi_compile __ _NO_FOG_ON
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			v2f vert(appdata_normal v) 
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				#if !defined(_NO_FOG_ON)
				UNITY_TRANSFER_FOG(o, o.pos);
				#endif
				return o;
			}
			
			fixed4 frag(v2f i) : COLOR {		
				fixed4 c = tex2D(_MainTex, i.uv) * _TintColor;	

				#if defined(_CUTOFF_ON)
				clip (c.a - 0.5);
				#endif

				#if !defined(_NO_FOG_ON)
				UNITY_APPLY_FOG(i.fogCoord, c);	
				#endif				 		
				return c;
			}
			
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
