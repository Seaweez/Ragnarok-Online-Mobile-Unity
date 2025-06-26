// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/CubeMapFacula" 
{
	Properties 
	{
		_OutlineColor ("Outline Color", Color) = (1,1,1,1)
		_OutlineWidth ("Outline Width", Range(0.0,0.1)) = 0.02
		_MainTexture ("Main Texture", 2D) = "white" {}
		_Cube("Reflection Map", Cube) = "" {}
		_ReflectionPower("Reflection Power", Range(0, 2)) = 1
	}
	
	CGINCLUDE
	#include "UnityCG.cginc"
	struct v2f
	{
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float3 normalDir : TEXCOORD1;
        float3 viewDir : TEXCOORD2;
	};
	
	// outline
	uniform fixed4 _OutlineColor;
	uniform half  _OutlineWidth;
	
	uniform sampler2D _MainTexture;
	uniform samplerCUBE _Cube;
	uniform half _ReflectionPower;
	ENDCG
	
	SubShader 
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		
		// outline
		Pass 
		{
			Tags { "Queue" = "Transparent"} 
				
			ZWrite Off
			ZTest Always
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			v2f vert(appdata_tan v) 
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				float2 dir = TransformViewToProjection(norm.xy);
				o.pos.xy += dir * _OutlineWidth;
				return o;
			}
			
			fixed4 frag(v2f i) : COLOR 
			{
				return _OutlineColor;
			}
			
			ENDCG
		}
		
		// texture
		Pass 
		{
			Tags { "Queue" = "Transparent"} 
			
			CGPROGRAM	
			#pragma vertex vert
			#pragma fragment frag
			
			v2f vert(appdata_tan v) 
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;
				o.viewDir = mul(unity_ObjectToWorld, v.vertex).xyz - _WorldSpaceCameraPos;
        		o.normalDir = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);            	
				return o;
			}
			
			fixed4 frag(v2f i) : COLOR 
			{
				fixed4 r=tex2D(_MainTexture, i.uv);	
				float3 reflectedDir = reflect(i.viewDir, normalize(i.normalDir));
				r*=texCUBE(_Cube, reflectedDir)*_ReflectionPower;				 		
				return r;				
			}
			
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
