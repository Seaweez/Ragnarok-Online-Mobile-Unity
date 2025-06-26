// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "RO/Effect/Transparent-AlwaysFront" {
	Properties 
	{
		_MainTex ("Albedo", 2D) = "white" {}
		_TintColor("Tint Color", Color) = (1,1,1,1)
		_Exposure("Exposure", Range(0.0, 3.0)) = 1.0

		[HideInInspector] _MaskTex ("Mask", 2D) = "white" {}
		[HideInInspector] _MainTexUVSpeed ("Main Tex UV Speed", Vector) = (0,0,0,0)
		[HideInInspector] _MaskTexUVSpeed ("Mask Tex UV Speed", Vector) = (0,0,0,0)
		
		[HideInInspector] _Mode ("__mode", Float) = 0.0
		[HideInInspector] _SrcBlend ("__src", Float) = 5.0
		[HideInInspector] _DstBlend ("__dst", Float) = 1.0
		[HideInInspector] _ZWrite1 ("__zw", Float) = 0.0
		[HideInInspector] _Cull ("__cull", Float) = 0.0

	    [Toggle(_NO_FOG_ON)]_NoFog ("No Fog", Float) = 0
	}
	
	CGINCLUDE
	#include "UnityCG.cginc"
	struct appdata_effect
	{
		fixed4 vertex : POSITION;
		fixed4 texcoord : TEXCOORD0;
		fixed4 color : COLOR;
	};
	
	struct v2f
	{
		fixed4 pos : SV_POSITION;
		fixed2 uv : TEXCOORD0;
		fixed2 maskUV : TEXCOORD1;
		fixed4 color : COLOR;
		UNITY_FOG_COORDS(2)
	};
	
	uniform sampler2D _MainTex;
	uniform fixed4 _MainTex_ST;
	uniform fixed4 _TintColor;
	uniform fixed _Exposure;

	uniform sampler2D _MaskTex;
	uniform fixed4 _MaskTex_ST;
	uniform fixed4 _MainTexUVSpeed;
	uniform fixed4 _MaskTexUVSpeed;
	
	ENDCG

	SubShader {
		Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
		
		Pass {
			Blend [_SrcBlend] [_DstBlend]
			ZWrite [_ZWrite1] Lighting Off
			Cull [_Cull]
		  	AlphaTest Greater 0.01
			ColorMask RGB
			ZTest Always
		  	
		  	CGPROGRAM
			
			#pragma multi_compile __ _ADDITIVE_ON
			#pragma multi_compile __ _MASK_ON
			#pragma multi_compile __ _UV_ANIMATION_ON
			#pragma multi_compile __ _NO_FOG_ON
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_particles
			#pragma multi_compile_fog
			
			v2f vert(appdata_effect v) 
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);

				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				#if defined(_UV_ANIMATION_ON)
				o.uv.x += _Time.y * _MainTexUVSpeed.x;
				o.uv.y += _Time.y * _MainTexUVSpeed.y;		
				#endif

				#if defined(_MASK_ON)
				o.maskUV = TRANSFORM_TEX(v.texcoord, _MaskTex);

				#if defined(_UV_ANIMATION_ON)
				o.maskUV.x += _Time.y * _MaskTexUVSpeed.x;
				o.maskUV.y += _Time.y * _MaskTexUVSpeed.y;		
				#endif	

				#endif

				o.color = v.color;
				#if !defined(_NO_FOG_ON)
				UNITY_TRANSFER_FOG(o, o.pos);
				#endif
				return o;
			}
			
			fixed4 frag(v2f i) : COLOR {	
				fixed4 c = _Exposure * i.color * _TintColor * tex2D (_MainTex, i.uv);
//				c.rgb *= c.a;
				#if defined(_MASK_ON)
				c.a *= tex2D (_MaskTex, i.uv).r;
				#endif

				#if !defined(_NO_FOG_ON)

				#if defined(_ADDITIVE_ON)
				UNITY_APPLY_FOG_COLOR(i.fogCoord, c, fixed4(0,0,0,0));
				#else
				UNITY_APPLY_FOG(i.fogCoord, c);
				#endif

				#endif
				return c;			
			}
			
			ENDCG
		}
	} 
	CustomEditor "EffectShaderGUI"
}
