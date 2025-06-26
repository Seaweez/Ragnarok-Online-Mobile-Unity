// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "RO/NormalTransparent" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_TintColor("Tint Color", Color) = (1,1,1,1)
	    [Toggle]_ZWrite ("ZWrite", Float) = 0
	    [Toggle(_FRAME_ANIMATION_ON)]_FrameAnimation ("Frame Animation On", Float) = 0
		[HideInInspector] _Column ("Column", Float) = 4
	    [HideInInspector] _Row ("Row", Float) = 4
	    [HideInInspector] _Speed ("Speed", Float) = 200

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

	uniform fixed _Column;
	uniform fixed _Row;
	uniform fixed _Speed;
	
	ENDCG

	SubShader {
		Tags { "Queue" = "Transparent" }
		
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite [_ZWrite]
		
		Pass {
			CGPROGRAM

			#pragma multi_compile __ _NO_FOG_ON
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma multi_compile __ _FRAME_ANIMATION_ON
			
			v2f vert(appdata_normal v) 
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);

				#if defined(_FRAME_ANIMATION_ON)
				int frameCount = _Column * _Row;
				int index = floor(_Time.x*_Speed) % frameCount;// [0,frameCount)
				int row = index/_Column % _Row;
				int col = index % _Column;
			    float2 uv = float2(v.texcoord.x/_Column, v.texcoord.y/_Row);
			    uv.x += (1/_Column)*col;
			    uv.y += (1/_Row)*row;
			    o.uv = uv;
			    #else
			    o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				#endif

				#if !defined(_NO_FOG_ON)
				UNITY_TRANSFER_FOG(o, o.pos);
				#endif
				return o;
			}
			
			fixed4 frag(v2f i) : COLOR {		
				fixed4 c = tex2D(_MainTex, i.uv) * _TintColor;	
				#if !defined(_NO_FOG_ON)
				UNITY_APPLY_FOG(i.fogCoord, c);	
				#endif			 		
				return c;
			}
			
			ENDCG
		}
	} 
	FallBack "Diffuse"
	CustomEditor "NormalTransparentShaderGUI"
}
