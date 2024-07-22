#ifndef BLURTEXTURE_UTILITIES
#define BLURTEXTURE_UTILITIES

#include "CommonUtil.hlsl"
#include "BaseDefine/VertexBase.hlsl"

CBUFFER_START(UnityPerMaterial)
    half4 _MainTex_ST;
	half _Brightness;
	half4 _Color;
CBUFFER_END

TEXTURE2D(_MainTex);    SAMPLER(sampler_MainTex);
float4 _MainTex_TexelSize;

struct Attributes
{
    float4 positionOS       : POSITION;
    float4 normalOS         : NORMAL;
    half4  color            : COLOR;
    float2 uv               : TEXCOORD0;
};

struct Varyings
{
    float4 positionCS       : SV_POSITION;
    half4  color            : COLOR;
    float2  uv               : TEXCOORD0;
};

Varyings vert(Attributes input)
{
    Varyings output = (Varyings)0;
    float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
    float4 positionCS = TransformWorldToHClip(positionWS);
    output.positionCS = positionCS;
    output.color = input.color;
    output.uv = input.uv;
    return output;
}

half4 fragHorizontal(Varyings input) : COLOR
{
    float texelSize = _MainTex_TexelSize.x;
	#define GRABPIXEL(weight,kernelx) SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv + float2(texelSize * kernelx*_Brightness, 0))*_Color * weight
	half4 color = half4(0,0,0,0);
   // color += GRABPIXEL(0.05, -4.0);
   // color += GRABPIXEL(0.09, -3.0);
    color += GRABPIXEL(0.12, -2.0);
    color += GRABPIXEL(0.15, -1.0);
    color += GRABPIXEL(0.18,  0.0);
	color += GRABPIXEL(0.15,  1.0);
	color += GRABPIXEL(0.12,  2.0);
	//color += GRABPIXEL(0.09,  3.0);
	//color += GRABPIXEL(0.05,  4.0);
    return half4(color.rgb, 1);
}

half4 fragVertical(Varyings input) : COLOR
{
    float texelSize = _MainTex_TexelSize.y;
	#define GRABPIXEL(weight,kernelx) SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv + float2(texelSize * kernelx*_Brightness, 0))*_Color  * weight
	half4 color = half4(0,0,0,0);
   // color += GRABPIXEL(0.05, -4.0);
   // color += GRABPIXEL(0.09, -3.0);
    color += GRABPIXEL(0.12, -2.0);
    color += GRABPIXEL(0.15, -1.0);
    color += GRABPIXEL(0.18,  0.0);
	color += GRABPIXEL(0.15,  1.0);
	color += GRABPIXEL(0.12,  2.0);
	//color += GRABPIXEL(0.09,  3.0);
	//color += GRABPIXEL(0.05,  4.0);
    return half4(color.rgb, 1);
}

#endif