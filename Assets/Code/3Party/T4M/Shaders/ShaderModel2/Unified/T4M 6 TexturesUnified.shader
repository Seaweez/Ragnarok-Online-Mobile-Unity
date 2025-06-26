Shader "RO/T4M/6 Textures" {
Properties {
	_Splat0 ("Layer 1", 2D) = "white" {}
	_Splat1 ("Layer 2", 2D) = "white" {}
	_Splat2 ("Layer 3", 2D) = "white" {}
	_Splat3 ("Layer 4", 2D) = "white" {}
	_Splat4 ("Layer 5", 2D) = "white" {}
	_Splat5 ("Layer 6", 2D) = "white" {}
	_Control ("Control (RGBA)", 2D) = "white" {}
	_Control2 ("Control2 (RGBA)", 2D) = "white" {}
//	_MainTex ("Never Used", 2D) = "white" {}
	
	_Color("Color", Color) = (1,1,1,1) 
	_AlbedoScale ("Albedo Scale", Range(0.0, 1.0)) = 1.0 
}
                
SubShader {
	Tags {
   "SplatCount" = "6"
   "RenderType" = "Opaque"
	}
CGPROGRAM
#pragma multi_compile __ _Ambient_ON
#pragma surface surf T4M exclude_path:prepass noforwardadd
#pragma exclude_renderers xbox360 ps3
#pragma target 4.0
inline fixed4 LightingT4M (SurfaceOutput s, fixed3 lightDir, fixed atten)
{
	fixed diff = dot (s.Normal, lightDir);
	fixed4 c;
	c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten * 2);
	c.a = 0.0;
	return c;
}

struct Input {
	float2 uv_Control : TEXCOORD0;
	float2 uv_Splat0 : TEXCOORD1;
	float2 uv_Splat1 : TEXCOORD2;
	float2 uv_Splat2 : TEXCOORD3;
	float2 uv_Splat3 : TEXCOORD4;
	float2 uv_Splat4 : TEXCOORD5;
//	float2 uv_Splat5 : TEXCOORD6;
};
 
sampler2D _Control,_Control2;
sampler2D _Splat0,_Splat1,_Splat2,_Splat3,_Splat4,_Splat5;
float4 _Color;
fixed _AlbedoScale;

float4 _LightColor;
float _LightScale;
 
void surf (Input IN, inout SurfaceOutput o) {
	fixed4 splat_control = tex2D (_Control, IN.uv_Control).rgba;
	fixed3 splat_control2 = tex2D (_Control2, IN.uv_Control).rgb;
		
	fixed3 col;
    col  = splat_control.r * tex2D (_Splat0, IN.uv_Splat0).rgb;
    col += splat_control.g * tex2D (_Splat1, IN.uv_Splat1).rgb;
    col += splat_control.b * tex2D (_Splat2, IN.uv_Splat2).rgb;
	fixed3 col2;
	col2 = splat_control2.r * tex2D (_Splat3, IN.uv_Splat3).rgb;
    col2 += splat_control2.g* tex2D (_Splat4, IN.uv_Splat4).rgb;
    col2 += splat_control2.b* tex2D (_Splat5, IN.uv_Splat4).rgb;
				
	col += splat_control.a * col2.rgb;
				
	o.Alpha = 0.0;
	o.Albedo = col.rgb * _Color.rgb * _AlbedoScale ;
	
	#if defined(_Ambient_ON)
	fixed4 lightColor = UNITY_LIGHTMODEL_AMBIENT;
	o.Albedo.rgb *= lightColor.rgb * lightColor.a;
	#endif
}
ENDCG 
}
// Fallback to Diffuse
Fallback "Diffuse"
}
