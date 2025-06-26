// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/UnlitNew/NGUIChange Color 1" 
{
	Properties 
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
	}
	
	SubShader {
	LOD 200

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"
			#include "CgIncludes/RoCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _ClipRange0 = float4(0.0, 0.0, 1.0, 1.0);
			float2 _ClipArgs0 = float2(1000.0, 1000.0);
	
			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				float2 worldPos : TEXCOORD1;
				fixed4 color : COLOR;
			};
	
			v2f o;

			v2f vert (appdata_t v)
			{
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				o.worldPos = v.vertex.xy * _ClipRange0.zw + _ClipRange0.xy;
				return o;
			}
				
			fixed4 frag (v2f IN) : COLOR
			{
				half4 color = tex2D(_MainTex, IN.texcoord);
				float b = 0;
				half4 col = RoGreyCorrect2(color, IN.color, b);
				color = RoMaskColor(color, IN.color);
				col = lerp(color, col, b);

				float2 factor = (float2(1.0, 1.0) - abs(IN.worldPos)) * _ClipArgs0;
				col.a *= clamp(min(factor.x, factor.y), 0.0, 1.0);

				return col;
			}
			
			
			
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
