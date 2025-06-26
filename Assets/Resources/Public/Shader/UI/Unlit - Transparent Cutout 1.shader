// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Transparent Cutout 1" {

    Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
	}

	SubShader
	{
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
			Offset -1, -1
			Fog { Mode Off }
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "CgIncludes/RoCG.cginc"

			sampler2D _MainTex;
			float4 _ClipRange0 = float4(0.0, 0.0, 1.0, 1.0);
			float2 _ClipArgs0 = float2(1000.0, 1000.0);

			struct appdata_t
			{
				float4 vertex : POSITION;
				half4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : POSITION;
				half4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float2 worldPos : TEXCOORD1;
			};

			v2f o;

			v2f vert (appdata_t v)
			{
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.texcoord = v.texcoord;
				o.worldPos = v.vertex.xy * _ClipRange0.zw + _ClipRange0.xy;
//				o.vertex.xyz = o.vertex.xyz/3;
				return o;
			}

			half4 frag (v2f IN) : COLOR
			{
				// Softness factor
				float2 factor = (float2(1.0, 1.0) - abs(IN.worldPos)) * _ClipArgs0;
				
				// Sample the texture
				half4 color = tex2D(_MainTex, IN.texcoord) * IN.color;
				color.a *= clamp( min(factor.x, factor.y), 0.0, 1.0);

				if (color.r + color.g + color.b == 0)
				{
					color.a = 0;
				}

				

				// set IN.color(1,2,3) gray
				//half3 test = IN.color.rgb * 255;
				//half4 col = color * IN.color;
				//float b = step(0, test.r);
				//b *= step(test.r, 2);
				//b *= step(1, test.g);
				//b *= step(test.g, 3);
				//b *= step(2, test.b);
				//b *= step(test.b, 4);
				//half gray = dot(color.rgb, half3(0.299, 0.587, 0.114));
				//half4 a = gray*IN.color.a+(1-IN.color.a);
				//a.a = color.a;
				//col = lerp(col, a, b);
				half4 col = RoGreyCorrect(color, IN.color);
				return col;
			}
			ENDCG
		}
	}
	
	SubShader
	{
		LOD 100

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
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
}