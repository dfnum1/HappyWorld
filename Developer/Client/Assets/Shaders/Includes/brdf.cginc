#ifndef BRDF_INCLUDED
#define BRDF_INCLUDED

//------------------------------------------------------------------------------
// Include
//------------------------------------------------------------------------------

#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "common_math.cginc"

//------------------------------------------------------------------------------
// Specular BRDF
//------------------------------------------------------------------------------

float D_GGX(float roughness, float NoH,float NoV, const float3 h)
{
    // Walter et al. 2007, "Microfacet Models for Refraction through Rough Surfaces"

    // In mediump, there are two problems computing 1.0 - NoH^2
    // 1) 1.0 - NoH^2 suffers floating point cancellation when NoH^2 is close to 1 (highlights)
    // 2) NoH doesn't have enough precision around 1.0
    // Both problem can be fixed by computing 1-NoH^2 in highp and providing NoH in highp as well

    // However, we can do better using Lagrange's identity:
    //      ||a x b||^2 = ||a||^2 ||b||^2 - (a . b)^2
    // since N and H are unit vectors: ||N x H||^2 = 1.0 - NoH^2
    // This computes 1.0 - NoH^2 directly (which is close to zero in the highlights and has
    // enough precision).
    // Overall this yields better performance, keeping all computations in mediump
    float3 NxH = cross(NoV, h);
    float oneMinusNoHSquared = dot(NxH, NxH);


    float a = NoH * roughness;
    float k = roughness / (oneMinusNoHSquared + a * a);
    float d = k * k * (1.0 / UNITY_PI);
    return d;
}

float D_GGX_Anisotropic(float at, float ab, float ToH, float BoH, float NoH)
{
    // Burley 2012, "Physically-Based Shading at Disney"
    
    float a2 = at * ab;
    float3 d = float3(ab * ToH, at * BoH, a2 * NoH);
    float d2 = dot(d, d);
    float b2 = a2 / d2;
    return a2 * b2 * b2 * (1.0 / UNITY_PI);
}

inline float GGXTerm(float NdotH, float roughness2)
{
    float a2 = roughness2;
    float d = (NdotH * a2 - NdotH) * NdotH + 1.0f; // 2 mad
    return 0.31830988618f * a2 / (d * d + 1e-7f); // This function is not intended to be running on Mobile,
                                                // therefore epsilon is smaller than what can be represented by float
}

inline float SmithJointGGXVisibilityTerm(float NdotL, float NdotV, float roughness)
{
    // Ref: http://jcgt.org/published/0003/02/03/paper.pdf

    // Approximation of the above formulation (simplify the sqrt, not mathematically correct but close enough)
    float a = roughness;
    float lambdaV = NdotL * (NdotV * (1 - a) + a);
    float lambdaL = NdotV * (NdotL * (1 - a) + a);

    return 0.5f / (lambdaV + lambdaL + 1e-5f);
}

float V_SmithGGXCorrelated_Anisotropic(float at, float ab, float ToV, float BoV,
        float ToL, float BoL, float NoV, float NoL)
{
    // Heitz 2014, "Understanding the Masking-Shadowing Function in Microfacet-Based BRDFs"
    // TODO: lambdaV can be pre-computed for all the lights, it should be moved out of this function
    float lambdaV = NoL * length(float3(at * ToV, ab * BoV, NoV));
    float lambdaL = NoV * length(float3(at * ToL, ab * BoL, NoL));
    float v = 0.5 / (lambdaV + lambdaL);
    return v;
}

float D_InvGGX(float roughness2, float NoH)
{
    float a2 = roughness2 * roughness2;
    float A = 4;
    float d = (NoH - a2 * NoH) * NoH + a2;
    return 1.0 / (UNITY_PI * (1 + A * a2)) * (1 + 4 * a2 * a2 / (d * d));
}

float D_Charlie(float roughness, float NoH)
{
    // Estevez and Kulla 2017, "Production Friendly Microfacet Sheen BRDF"
    float invAlpha = 1.0 / roughness;
    float cos2h = NoH * NoH;
    float sin2h = max(1.0 - cos2h, 0.0078125); // 2^(-14/2), so sin2h^2 > 0 in fp16
    return (2.0 + invAlpha) * pow(sin2h, invAlpha * 0.5) / (2.0 * UNITY_PI);
}

float AshikhminD(float roughness2, float NoH)
{
    float m2 = roughness2;
    float cos2h = NoH * NoH;
    float sin2h = 1. - cos2h;
    float sin4h = sin2h * sin2h;
    return (sin4h + 4. * exp(-cos2h / (sin2h * m2))) / (UNITY_PI * (1. + 4. * m2) * sin4h);
}

float V_Neubelt(float NoV, float NoL)
{
    // Neubelt and Pettineo 2013, "Crafting a Next-gen Material Pipeline for The Order: 1886"
    return 1.0 / (4.0 * (NoL + NoV - NoL * NoV));
}


float3 F_Schlick(const float3 f0, float f90, float VoH)
{
    // Schlick 1994, "An Inexpensive BRDF Model for Physically-Based Rendering"
    return f0 + (f90 - f0) * Pow5(1.0 - VoH);
}

float3 F_Schlick(const float3 f0, float VoH)
{
    float f = pow(1.0 - VoH, 5.0);
    return f + f0 * (1.0 - f);
}

float F_Schlick(float f0, float f90, float VoH)
{
    return f0 + (f90 - f0) * Pow5(1.0 - VoH);
}

float KajiyaSpecular(float3 T, float3 V, float3 L, float exponent)
{
    float3 H = normalize(L + V);
    float dotTH = dot(T, H);
    float sinTH = sqrt(1 - dotTH * dotTH);
    float dirAtten = smoothstep(-1.0, 0.0, dot(T, H));
    return dirAtten * pow(sinTH, exponent);
}


float GetSpecTerm(float roughness2, float nl, float nv, float nh)
{    
    float lambdaV = nl * (nv * (1 - roughness2) + roughness2);
	float lambdaL = nv * (nl * (1 - roughness2) + roughness2);
	float V = 0.5f / (lambdaV + lambdaL + 1e-5f);
	float a2 = roughness2 * roughness2;
	float d = (nh * a2 - nh) * nh + 1.0f;
	float D = 0.31830988618f * a2 / (d * d + 1e-7f);

	float specularTerm = V * D * 3.14159265359f;
    specularTerm = max(0,specularTerm * nl);
    return specularTerm;
}


//------------------------------------------------------------------------------
// Diffuse BRDF
//------------------------------------------------------------------------------

// Energy conserving wrap diffuse term, does *not* include the divide by pi
float Fd_Wrap(float NoL, float w)
{
    return saturate((NoL + w) / sq(1.0 + w));
}

float Fd_Lambert()
{
    return 1.0 / UNITY_PI;
}

float Fd_Burley(float roughness, float NoV, float NoL, float LoH)
{
    // Burley 2012, "Physically-Based Shading at Disney"
    float f90 = 0.5 + 2.0 * roughness * LoH * LoH;
    float lightScatter = F_Schlick(1.0, f90, NoL);
    float viewScatter = F_Schlick(1.0, f90, NoV);
    return lightScatter * viewScatter * (1.0 / UNITY_PI);
}

float DisneyDiffuse(float NdotV, float NdotL, float LdotH, float perceptualRoughness)
{
    // Disney diffuse must be multiply by diffuseAlbedo / PI. This is done outside of this function.
    
    float fd90 = 0.5 + 2 * LdotH * LdotH * perceptualRoughness;
    // Two schlick fresnel term
    float lightScatter = (1 + (fd90 - 1) * Pow5(1 - NdotL));
    float viewScatter = (1 + (fd90 - 1) * Pow5(1 - NdotV));

    return lightScatter * viewScatter * NdotL;
}

//------------------------------------------------------------------------------
// Specular Lobes
//------------------------------------------------------------------------------
float3 anisotropicLobe(const float3 anisotropicT, const float3 anisotropicB, float anisotropy,
        const float3 f0, float roughness, 
        const float3 lightDir, const float3 viewDir, const float3 h,
        float NoV, float NoL, float NoH, float LoH)
{

    float3 l = lightDir;
    float3 t = anisotropicT;
    float3 b = anisotropicB;
    float3 v = viewDir;

    float ToV = dot(t, v);
    float BoV = dot(b, v);
    float ToL = dot(t, l);
    float BoL = dot(b, l);
    float ToH = dot(t, h);
    float BoH = dot(b, h);

    // Anisotropic parameters: at and ab are the roughness along the tangent and bitangent
    // to simplify materials, we derive them from a single roughness parameter
    // Kulla 2017, "Revisiting Physically Based Shading at Imageworks"
    float at = max(roughness * (1.0 + anisotropy), 0.007921);
    float ab = max(roughness * (1.0 - anisotropy), 0.007921);

    // specular anisotropic BRDF
    float D = D_GGX_Anisotropic(at, ab, ToH, BoH, NoH);
    float V = V_SmithGGXCorrelated_Anisotropic(at, ab, ToV, BoV, ToL, BoL, NoV, NoL);
    float3 F = F_Schlick(f0, LoH);

    return (D * V) * F;
}

float sheenLobe(float roughness,float NoV, float NoL, float NoH)
{

    float D = D_Charlie(roughness, NoH);
    float V = V_Neubelt(NoV, NoL);
    return D * V;  
}

#endif