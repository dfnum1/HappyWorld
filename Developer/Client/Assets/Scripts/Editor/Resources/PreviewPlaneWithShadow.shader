// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Preview Plane With Shadow" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_ShadowTexture ("Shadowmap", 2D) = "white" {}
	_Alphas ("Alphas", Vector) = (1.0,0.3,0,0)
}
SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "ForceSupported"="True"}

	Blend One OneMinusSrcAlpha // premultiplied alpha blend
	ZWrite Off

	Pass {
		Tags {"LightMode" = "UniversalForward"}

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"

		struct appdata {
			float4 pos : POSITION;
			float3 nor : NORMAL;
			float2 uv : TEXCOORD0;
		};

		struct v2f {
			float2 uv : TEXCOORD0;
			float2 shadowUV : TEXCOORD1;
			fixed4 color : COLOR0;
			float4 pos : SV_POSITION;
		};

		float4 _MainTex_ST;
		float4x4 _ShadowTextureMatrix;

		v2f vert (appdata v) {
			v2f o;
			o.pos = UnityObjectToClipPos (v.pos);
			o.uv = TRANSFORM_TEX(v.uv,_MainTex);

			float4 wpos = mul(unity_ObjectToWorld, v.pos);
			o.shadowUV = mul(_ShadowTextureMatrix, wpos).xy;

			o.color.rgb = ShadeVertexLights(v.pos, v.nor);
			o.color.a = 1.0;

			return o;
		}

		sampler2D _MainTex;
		sampler2D _ShadowTexture;
		fixed4 _Alphas;

		fixed4 frag (v2f i) : COLOR0 
		{
			// Texture, apply lighting and premultiply with alpha
			fixed4 col = tex2D(_MainTex, i.uv);
			col.rgb *= i.color.rgb;
			col.a *= _Alphas.x;
			col.rgb *= col.a;

			// Shadow mask, with 30% alpha
			fixed shadow = tex2D(_ShadowTexture, i.shadowUV).r * _Alphas.y;
			col.rgb = lerp (col.rgb, fixed3(0,0,0), shadow);
			col.a = max(col.a, shadow);

			return col;
		}
		ENDCG
	}
}

}
