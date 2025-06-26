// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "RO/Color" 
{
	Properties 
	{
		_Color("Main Color", Color) = (1,1,1,1)
	}
	
	CGINCLUDE
	#include "UnityCG.cginc"
	struct v2f
	{
		fixed4 pos : SV_POSITION;
	};
	
	uniform fixed4 _Color;
	
	ENDCG

	SubShader {
		Tags { "RenderType"="Opaque" }
		
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
				fixed4 col = _Color;
				UNITY_OPAQUE_ALPHA(col.a);
				return col;
			}
			
			ENDCG
		}
	} 
	FallBack "Unlit/Color"
}
