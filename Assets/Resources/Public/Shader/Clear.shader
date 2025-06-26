// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "RO/Clear" 
{
	Properties 
	{
	}
	
	CGINCLUDE
	#include "UnityCG.cginc"
	struct v2f
	{
		fixed4 pos : SV_POSITION;
	};
	
	ENDCG

	SubShader {
		Tags { "Queue" = "Transparent" }
		
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite off
		
		Pass {
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			v2f vert(fixed4 vertex : POSITION) 
			{
				v2f o;
				o.pos = UnityObjectToClipPos(vertex);
				return o;
			}
			
			fixed4 frag(v2f i) : COLOR 
			{					 		
				return fixed4(0,0,0,0);
			}
			
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
