
Shader "RO/T4M/4 Textures"  //by czj
{
	Properties 
	{
		_Splat0 ("Layer 1", 2D) = "white" {}
		_Splat1 ("Layer 2", 2D) = "white" {}
		_Splat2 ("Layer 3", 2D) = "white" {}
		_Splat3 ("Layer 4", 2D) = "white" {}
		_Control ("Control (RGBA)", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1) 
		_AlbedoScale ("Albedo Scale", Range(0.0, 1.0)) = 1.0 
	}
                
	SubShader
	{
		Tags
		{
			"RenderType" = "Opaque"
		}
		
		Pass
		{
			Name "FORWARD"
			Tags
			{
				"LightMode" = "ForwardBase"
			}

			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#define UNITY_PASS_FORWARDBASE
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "Lighting.cginc"

			#pragma multi_compile LIGHTMAP_ON
			#pragma multi_compile_fog
			#pragma multi_compile __ _Ambient_ON
			#pragma target 3.0

			sampler2D _Splat0;
			uniform float4 _Splat0_ST;
			sampler2D _Splat1;
			uniform float4 _Splat1_ST;
			sampler2D _Splat2;
			uniform float4 _Splat2_ST;
			sampler2D _Splat3;
			uniform float4 _Splat3_ST;
			sampler2D _Control;
			uniform float4 _Control_ST;



			float _AlbedoScale;
			float4 _Color;

			struct VertexInput 
			{
				float4 vertex	 : POSITION;
				float2 texcoord0 : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;

			};

			struct v2f 
			{
				float4 pos  : SV_POSITION;
				float2 uv	: TEXCOORD0;
				float4 uv01 : TEXCOORD1;
				float4 uv23 : TEXCOORD2;
				LIGHTING_COORDS(3, 4)
				UNITY_FOG_COORDS(5)
				#if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
					float4 ambientOrLightmapUV : TEXCOORD6;
				#endif
			};

			v2f vert(VertexInput v)
			{
				v2f o;
				o.pos	  = UnityObjectToClipPos(v.vertex);
				o.uv01.xy = TRANSFORM_TEX(v.texcoord0, _Splat0);
				o.uv01.zw = TRANSFORM_TEX(v.texcoord0, _Splat1);
				o.uv23.xy = TRANSFORM_TEX(v.texcoord0, _Splat2);
				o.uv23.zw = TRANSFORM_TEX(v.texcoord0, _Splat3);
				o.uv	  = TRANSFORM_TEX(v.texcoord0, _Control);

				#ifdef LIGHTMAP_ON
					o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
					o.ambientOrLightmapUV.zw = 0;
				#elif UNITY_SHOULD_SAMPLE_SH

				#endif
				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				half4 c;

				half4 splat_control = tex2D(_Control, i.uv).rgba;
				fixed3 lay1 = tex2D(_Splat0, i.uv01.xy);
				fixed3 lay2 = tex2D(_Splat1, i.uv01.wz);
				fixed3 lay3 = tex2D(_Splat2, i.uv23.xy);
				fixed3 lay4 = tex2D(_Splat3, i.uv23.wz);
				c.rgb = (lay1 * splat_control.r + lay2 * splat_control.g + lay3 * splat_control.b + lay4 * splat_control.a)* _Color.rgb * _AlbedoScale;

				#if LIGHTMAP_ON
					half4 bakedColorTex = UNITY_SAMPLE_TEX2D(unity_Lightmap, i.ambientOrLightmapUV);
					half3 bakedColor = DecodeLightmap(bakedColorTex);
					c.rgb *= bakedColor;
				#endif
				

				#if defined(_Ambient_ON)
					half4 lightColor = UNITY_LIGHTMODEL_AMBIENT;
					c.rgb *= lightColor.rgb * lightColor.a;
				#endif
				
				UNITY_APPLY_FOG(i.fogCoord, c);
				return c;
			}
			ENDCG
		}
	}
	//Fallback "Diffuse"
}