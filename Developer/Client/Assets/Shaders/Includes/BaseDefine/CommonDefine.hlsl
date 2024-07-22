#ifndef _COMMON_DEFINE_HLSL_
#define	_COMMON_DEFINE_HLSL_

#include "ConstDefine.hlsl"

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/EntityLighting.hlsl"

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityInput.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/ImageBasedLighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonLighting.hlsl"

// samplerstate only can be use for dx11 dx12 and metal.
// in GL mode we always need is textureName with sampler_##textureName.
#define TEXTURE2D_SAMPLE_PARAM_DEF(textureName) TEXTURE2D_PARAM( textureName, sampler##textureName)

#define TEXTURECUBE_SAMPLE_PARAM_DEF(textureName) TEXTURECUBE_PARAM( textureName, sampler##textureName)

#define TEXTURE3D_SAMPLE_PARAM_DEF(textureName) TEXTURE3D_PARAM( textureName, sampler##textureName)

#define TEXTURE_SAMPLE_ARGS_DEF(textureName)   \
	TEXTURE2D_ARGS(textureName, sampler##textureName) // 3D CUBE marco is the same way.

#define TEXTURE2D_DEF(textureName) \
	TEXTURE2D(textureName);\
	SAMPLER( sampler##textureName );

#define SAMPLE_TEXTURE2D_DEF(textureName, coord2) \
	SAMPLE_TEXTURE2D(textureName, sampler##textureName, coord2)

#define SAMPLE_TEXTURE2D_LOD_DEF(textureName, coord2, lod) \
	SAMPLE_TEXTURE2D_LOD(textureName, sampler##textureName, coord2, lod)

#define TEXTURE2D_ARRAY_DEF(textureName) \
	TEXTURE2D_ARRAY(textureName);\
	SAMPLER( sampler##textureName );

#define SAMPLE_TEXTURE2D_ARRAY_DEF(textureName, coord2, index) \
	SAMPLE_TEXTURE2D_ARRAY(textureName, sampler##textureName, coord2, index)

#define TEXTURECUBE_DEF(textureName) \
	TEXTURECUBE(textureName); \
	SAMPLER(sampler##textureName)

#define SAMPLE_TEXTURECUBE_DEF(textureName, coord3) \
	SAMPLE_TEXTURECUBE(textureName, sampler##textureName, coord3)                           

#define SAMPLE_TEXTURECUBE_LOD_DEF(textureName, coord3, lod) \
	SAMPLE_TEXTURECUBE_LOD(textureName, sampler##textureName, coord3, lod)

#define TEXTURE3D_DEF(textureName) \
	TEXTURE3D(textureName); \
	SAMPLER(sampler##textureName)

#define SAMPLE_TEXTURE3D_DEF(textureName)  \
	SAMPLE_TEXTURE3D(textureName, sampler##textureName, coord3)  

real4 UnityObjectToClipPos(real4 vertex)
{
	return mul(UNITY_MATRIX_VP, mul(UNITY_MATRIX_M, vertex));
}

real3 UnityObjectToWorldNormal(real3 normal)
{
	return mul(UNITY_MATRIX_M, real4(normal,0)).xyz;
}
real4 UnityObjectToWorldNormal(real4 normal)
{
	return mul(UNITY_MATRIX_M, normal);
}

//#define UnityObjectToWorldNormal TransformObjectToWorldNormal
#define UnityObjectToWorldDir TransformObjectToWorldDir
inline float3 UnityObjectToViewPos(in float3 pos)
{
	return mul(UNITY_MATRIX_V, mul(UNITY_MATRIX_M, float4(pos, 1.0))).xyz;
}

#if defined(UNITY_PASS_PREPASSBASE) || defined(UNITY_PASS_DEFERRED) || defined(UNITY_PASS_SHADOWCASTER)
	#undef FOG_LINEAR
	#undef FOG_EXP
	#undef FOG_EXP2
#endif

#define UNITY_FOG_COORDS_PACKED(idx, vectype) vectype fogCoord : TEXCOORD##idx;
#if defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2)
	#define UNITY_FOG_COORDS(idx) UNITY_FOG_COORDS_PACKED(idx, float1)
	#define UNITY_TRANSFER_FOG(o,outpos) ; o.fogCoord.x = ComputeFogFactor(outpos.z);
#else
	#define UNITY_FOG_COORDS(idx)
	#define UNITY_TRANSFER_FOG(o,outpos)
#endif


#if defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2)
	#define UNITY_APPLY_FOG_COLOR(coord, col, fogCol) col.rgb = MixFog(col.rgb, coord)
#else
#define UNITY_APPLY_FOG_COLOR(coord,col,fogCol)
#endif

#ifdef UNITY_PASS_FORWARDADD
#define UNITY_APPLY_FOG(coord,col) UNITY_APPLY_FOG_COLOR(coord, col, half4(0,0,0,0))
#else
#define UNITY_APPLY_FOG(coord,col) UNITY_APPLY_FOG_COLOR(coord, col, unity_FogColor)
#endif

half Alpha(half albedoAlpha, half4 color, half cutoff)
{
#if !defined(_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A) && !defined(_GLOSSINESS_FROM_BASE_ALPHA)
	half alpha = albedoAlpha * color.a;
#else
	half alpha = color.a;
#endif

#if defined(_ALPHATEST_ON)
	clip(alpha - cutoff);
#endif

	return alpha;
}
half4 SampleAlbedoAlpha(float2 uv, TEXTURE2D_PARAM(albedoAlphaMap, sampler_albedoAlphaMap))
{
	return SAMPLE_TEXTURE2D(albedoAlphaMap, sampler_albedoAlphaMap, uv);
}

real3 DecodeLightmap(real4 input)
{
	return DecodeLightmap(input, half4(LIGHTMAP_HDR_MULTIPLIER, LIGHTMAP_HDR_EXPONENT, 0.0h, 0.0h));
}


real Pow2(real x)
{
	return x * x;
}

real2 Pow2(real2 x)
{
	return x * x;
}

real3 Pow2(real3 x)
{
	return x * x;
}

real4 Pow2(real4 x)
{
	return x * x;
}

real Pow3(real x)
{
	return x * x*x;
}

real2 Pow3(real2 x)
{
	return x * x*x;
}

real3 Pow3(real3 x)
{
	return x * x*x;
}

real4 Pow3(real4 x)
{
	return x * x*x;
}

//real Pow4(real x)
//{
//	real xx = x * x;
//	return xx * xx;
//}

real2 Pow4(real2 x)
{
	real2 xx = x * x;
	return xx * xx;
}

real3 Pow4(real3 x)
{
	real3 xx = x * x;
	return xx * xx;
}

real4 Pow4(real4 x)
{
	real4 xx = x * x;
	return xx * xx;
}

real Pow5(real x)
{
	real xx = x * x;
	return xx * xx * x;
}

real2 Pow5(real2 x)
{
	real2 xx = x * x;
	return xx * xx * x;
}

real3 Pow5(real3 x)
{
	real3 xx = x * x;
	return xx * xx * x;
}

real4 Pow5(real4 x)
{
	real4 xx = x * x;
	return xx * xx * x;
}

real Pow6(real x)
{
	real xx = x * x;
	return xx * xx * xx;
}

real2 Pow6(real2 x)
{
	real2 xx = x * x;
	return xx * xx * xx;
}

real3 Pow6(real3 x)
{
	real3 xx = x * x;
	return xx * xx * xx;
}

real4 Pow6(real4 x)
{
	real4 xx = x * x;
	return xx * xx * xx;
}

real Square(real x)
{
	return x * x;
}

real2 Square(real2 x)
{
	return x * x;
}

real3 Square(real3 x)
{
	return x * x;
}

real3 UnpackNormalRG(real2 normalTexVal)
{
	real3 normalTS;
	normalTS.xy = (normalTexVal - 0.5) * 2.0;
	normalTS.z = sqrt(max(0, 1.0 - normalTS.x * normalTS.x - normalTS.y * normalTS.y));
	return normalize(normalTS);
}

#endif
