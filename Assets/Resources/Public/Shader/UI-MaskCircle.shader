// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "RO/UI/MaskCircle"
{
	Properties
	{
		_InnerRadius ("Inner Radius", Range(0, 1)) = 0
		_OutterRadius ("Outter Radius", Range(0, 1)) = 0.3
		_CircleX ("Circle X", Range(0, 1)) = 0.5
		_CircleY ("Circle Y", Range(0, 1)) = 0.5
		_Alpha ("Alpha", Range(0, 1)) = 0.7
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite off
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			uniform float _InnerRadius;
			uniform float _OutterRadius;
			uniform float _CircleX;
			uniform float _CircleY;
			uniform float _Alpha;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 circle = float2(_CircleX, _CircleY);
				float dist = distance(circle, i.uv);
				dist = clamp(dist, _InnerRadius, _OutterRadius);
				float alpha = (dist-_InnerRadius) / (_OutterRadius-_InnerRadius);
				fixed4 col = fixed4(0,0,0,alpha*_Alpha);			
				return col;
			}
			ENDCG
		}
	}
}
