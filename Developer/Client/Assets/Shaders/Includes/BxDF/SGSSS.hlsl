#ifndef _SGSSS_BASE_HLSL_
#define _SGSSS_BASE_HLSL_

#include "../BaseDefine/ConstDefine.hlsl"
#include "../BaseDefine/CommonDefine.hlsl"
//#include "CommonBufferDef.hlsl"
#include "BxDFBaseFunction.hlsl"

#if defined(USE_SGSSS)

//CBUFFER_START(UnityPerMaterial)
//real _ScatterAmtX;
//real _ScatterAmtY;
//real _ScatterAmtZ;
//half _SkinToneMap;
//half _SkinStrength;
//CBUFFER_END

struct FSphericalGaussian
{
	half3	Axis;		// u
	half	Sharpness;	// L
	//float	Amplitude;	// a
};

// Inner product with cosine lobe
// Assumes G is normalized
float DotCosineLobe(FSphericalGaussian G, float3 N)
{
	const float muDotN = dot(G.Axis, N);

	const float c0 = 0.36;
	const float c1 = 0.25 / c0;

	float eml = exp(-G.Sharpness);
	float em2l = eml * eml;
	float rl = rcp(G.Sharpness);

	float scale = 1.0f + 2.0f * em2l - rl;
	float bias = (eml - em2l) * rl - em2l;

	float x = sqrt(1.0 - scale);
	float x0 = c0 * muDotN;
	float x1 = c1 * x;

	float n = x0 + x1;
	float y = (abs(x0) <= x1) ? n * n / x : saturate(muDotN);

	return scale * y + bias;
}

real EricCurvature(real3 vertexNormal, real3 worldPos)
{
	real DeltaN = length(abs(ddx(vertexNormal)) + abs(ddy(vertexNormal)));
	real DeltaP = length(abs(ddx(worldPos)) + abs(ddy(worldPos)));
	real curvature = DeltaN / DeltaP;
	return curvature;
}

FSphericalGaussian MakeNormalizedSG(real3 LightDir, float Sharpness)
{
	FSphericalGaussian SG;
	SG.Axis = LightDir;
	SG.Sharpness = Sharpness;
	//SG.Amplitude = Sharpness / ((2*PI)*(1-exp(-2* Sharpness)));
	return SG;
}

half3 SGDifusseLighting(Surface s, Light L)
{
	FSphericalGaussian RedKernel = MakeNormalizedSG(L.direction, 1/max(_ScatterAmtX, 1e-3));
	FSphericalGaussian GreenKernel = MakeNormalizedSG(L.direction, 1 / max(_ScatterAmtY, 1e-3));
	FSphericalGaussian BlueKernel = MakeNormalizedSG(L.direction, 1 / max(_ScatterAmtZ, 1e-3));
	half3 Diffuse = half3(DotCosineLobe(RedKernel, s.normalWorld), DotCosineLobe(GreenKernel, s.normalWorld), DotCosineLobe(BlueKernel, s.normalWorld));

	// Reinhard tone mapping
	//Diffuse *= 1 / _SkinToneMap;
	//Diffuse = Diffuse / (1.0f + Diffuse);

	// cry engine tone mapping 
	//Diffuse = 1 - exp(-_SkinToneMap * Diffuse);

	// filmic/aces tone mapping
	half3 x = max(0, Diffuse)* _SkinStrength;
	Diffuse = (x * (6.2 * x + 0.5)) / (x * (6.2 * x + 1.7) + _SkinToneMap);
	return Diffuse*L.shadowAttenuation* L.distanceAttenuation;
}

#endif

#endif