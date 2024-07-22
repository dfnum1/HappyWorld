Shader "SD/Particles/SD_ScaleMask" {
Properties{
	    [HDR]_MainColor ("MainColor", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		[Toggle] _MaskTexWhite("Use Mask White", Float) = 0
	    _AlphaTex("Mask", 2D) = "white" {}
		//_Scale("Scale", Range(0.1, 5)) = 1
		[MaterialEnum(Add,1, Blend, 10)]_BlendMode("BlendMode", Int) = 10
		_CurveFactor("CurveFactor", Float) = 1
}

SubShader{
	Tags{ "Queue" = "Transparent+100" "IgnoreProjector" = "True" "RenderType" = "Transparent" }	
	Cull Off
	Lighting Off 
	ZWrite Off
	Blend SrcAlpha [_BlendMode]
	Pass{
		HLSLPROGRAM
		#pragma fragmentoption ARB_precision_hint_fastest
		#pragma vertex vert
		#pragma fragment frag

		#define UNITY_PASS_FORWARDBASE
		#include "Assets/Shaders/Includes/WorldCurvedCG.hlsl"

	struct vertexInput {
		fixed4 vertex : POSITION;
		fixed2 texcoord : TEXCOORD0;
		fixed2 texcoord1: TEXCOORD1;
		fixed4 vertexColor : COLOR;
	};

	struct VertexOutput {
		fixed4 pos : SV_POSITION;
		fixed2 uv : TEXCOORD0;
		fixed2 uv1 : TEXCOORD1;
		fixed4 vertexColor : COLOR;
	};
	sampler2D_half _MainTex;
	sampler2D_half _AlphaTex;
	CBUFFER_START(UnityPerMaterial)
	fixed4 _MainColor;
	fixed4 _MainTex_ST;
	fixed4 _AlphaTex_ST;
	fixed _MaskTexWhite;
	fixed _CurveFactor;
	//fixed _Scale;
	CBUFFER_END
	VertexOutput vert(vertexInput v)
	{
		VertexOutput o=(VertexOutput)0;
		CURVED_WORLD_TRANSFORM_POINT(v.vertex, _CurveFactor);
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv  =v.texcoord;
		o.uv1 =v.texcoord1;
		o.vertexColor = v.vertexColor;
		return o;
	}

	fixed4 frag(VertexOutput i) : SV_Target
	{    
		fixed2 center = fixed2(.5,.5);
		fixed4 alphaMask = tex2D(_AlphaTex, i.uv);
		fixed alpha = alphaMask.a* _MaskTexWhite + alphaMask.r*(1- _MaskTexWhite);
		i.uv = i.uv-center;  
		fixed4 col =tex2D(_MainTex, i.uv*float2(1.0/i.uv1.x,1.0/i.uv1.x)+center);
		col=col*i.vertexColor*_MainColor;
		return fixed4(col.rgb*alpha, saturate(col.r*alpha*i.vertexColor.a*_MainColor.a));
	}
		ENDHLSL
	}
	}
			FallBack Off
}