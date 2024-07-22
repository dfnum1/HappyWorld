#ifndef _MIX_COMMON_HLSL_
#define _MIX_COMMON_HLSL_

#include "..\BaseDefine\ConstDefine.hlsl"
#include "..\BaseDefine\CommonDefine.hlsl"
#include "BxDFContext.hlsl"
#include "BxDFBaseFunction.hlsl"
#include "UEBasePhysicBxDF.hlsl"
#include "UEShadingBase.hlsl"
#include "LUTBase.hlsl"
#include "SGSSS.hlsl"
#include "Anisotropic.hlsl"

real3 CalFalloffColor(BxDFContext c, real NoL, Light l, Surface s)
{
#if defined(USE_SGSSS)
	return l.color * SGDifusseLighting(s,l)* NoL;
#elif defined(USE_LUT)
	return l.color * CalLUTLightFalloff(l, NoL, s.uv0);
#else
	return l.color * l.distanceAttenuation * l.shadowAttenuation * NoL;
#endif
}

real3 CalMergeFalloffColor(BxDFContext c, real NoL, Light l, Surface s, bool isLUT)
{
#if defined(USE_SGSSS)
	return isLUT? (l.color* SGDifusseLighting(s, l) * NoL) : (l.color * l.distanceAttenuation * l.shadowAttenuation * NoL);
#elif defined(USE_LUT)
	
	return isLUT ? l.color * CalLUTLightFalloffSimple(l, NoL, s.uv0) : l.color * l.distanceAttenuation * l.shadowAttenuation * NoL;
	
#else
    return l.color * l.distanceAttenuation * l.shadowAttenuation * NoL;
#endif
}

real3 CalDiffuse(Surface s, BxDFContext c, real NoL)
{
#if defined(USE_GOTANDA_DIFFUSE)
	return Diffuse_Gotanda(s.diffColor, s.roughness, c.NoV, NoL, c.VoH);
#elif  defined(USE_O_N_DIFFUSE)
	return Diffuse_OrenNayar(s.diffColor, s.roughness, c.NoV, NoL, c.VoH);
#elif defined(USE_DISNEY_DIFFUSE)
	return Diffuse_Burley(s.diffColor, s.roughness, c.NoV, NoL, c.VoH);
#else // defined(USE_LAMBERT_DIFFUSE)
	return  Diffuse_Lambert(s.diffColor);
#endif
}


real3 MergeSchlickBackMannSpec(Surface s, real roughness, real3 specColor, BxDFContext c, real NoL, Light l, bool isAnisotropic)
{
    real a2 = Pow4(roughness);
	//real Energy = EnergyNormalization(a2, Context.VoH, AreaLight);

    real3 spec;

#if defined(USE_ANISOTROPIC)
	if (isAnisotropic)
	{
	   real roughnessT = 0.0;
		real roughnessB = 0.0;

		//Normal shift
		real shiftAmount = dot(s.normalWorld, s.viewDir);
		s.normalWorld = shiftAmount < 0.0f ? normalize(s.normalWorld + s.viewDir * (-shiftAmount + 1e-5f)) : s.normalWorld;

		_ConvertAnisotropyToRoughness(s.roughness, s.anisotropyLv, roughnessT, roughnessB);

		real3 H	 = normalize(s.viewDir + l.direction);

		real VoT = dot(s.viewDir, s.tangentWorld);
		real VoB = dot(s.viewDir, s.bitangent);
		real ToL = dot(s.tangentWorld, l.direction);
		real BoL = dot(s.bitangent, l.direction);

		real ToH = dot(s.tangentWorld,  H);
		real BoH = dot(s.bitangent,		H);

		real D = D_GGXAnisotropic( ToH, BoH, c.NoH, roughnessT, roughnessB);

		real Vis = V_SmithJointGGXAnisotropic( VoT, VoB, c.NoV, ToL, BoL, c.NoL, roughnessT, roughnessB);

		real3 F = F_Schlick(specColor, c.VoH);
	
		spec = D * Vis  * F;
	}
	else
	{
		real D = _D_GGX(a2, c.NoH); // *Energy;

		real Vis = Vis_SmithJointApprox(a2, c.NoV, NoL);
	
		real3 F = _F_Schlick(specColor, c.VoH);

		spec = D * Vis * F;
	}
#else
	// Generalized microfacet specular
    real D = D_GGX(a2, c.NoH); // *Energy;

    real Vis = Vis_SmithJointApprox(a2, c.NoV, NoL);

	//real Vis = Vis_Smith(a2, c.NoV, NoL);
	

    real3 F = _F_Schlick(specColor, c.VoH);

    spec = D * Vis * F;
#endif

    return spec;
}

//This shading model using UE4 and Unity and substance mix.
real3 SchlickBackMannSpec(Surface s, real roughness, real3 specColor, BxDFContext c, real NoL, Light l)
{
	real a2 = Pow4(roughness);
	//real Energy = EnergyNormalization(a2, Context.VoH, AreaLight);

	real3 spec;

#if defined(USE_ANISOTROPIC)
	real roughnessT = 0.0;
	real roughnessB = 0.0;

	//Normal shift
	real shiftAmount = dot(s.normalWorld, s.viewDir);
	s.normalWorld = shiftAmount < 0.0f ? normalize(s.normalWorld + s.viewDir * (-shiftAmount + 1e-5f)) : s.normalWorld;

	_ConvertAnisotropyToRoughness(s.roughness, s.anisotropyLv, roughnessT, roughnessB);

	real3 H	 = normalize(s.viewDir + l.direction);

	real VoT = dot(s.viewDir, s.tangentWorld);
	real VoB = dot(s.viewDir, s.bitangent);
	real ToL = dot(s.tangentWorld, l.direction);
	real BoL = dot(s.bitangent, l.direction);

	real ToH = dot(s.tangentWorld,  H);
	real BoH = dot(s.bitangent,		H);

	real D = D_GGXAnisotropic( ToH, BoH, c.NoH, roughnessT, roughnessB);

	real Vis = V_SmithJointGGXAnisotropic( VoT, VoB, c.NoV, ToL, BoL, c.NoL, roughnessT, roughnessB);

	real3 F = F_Schlick(specColor, c.VoH);

	spec = D * Vis  * F;
#else
	// Generalized microfacet specular
	real D = D_GGX(a2, c.NoH);// *Energy;

	real Vis = Vis_SmithJointApprox(a2, c.NoV, NoL);

	//real Vis = Vis_Smith(a2, c.NoV, NoL);
	

	real3 F = _F_Schlick(specColor, c.VoH);

	spec = D* Vis* F;
#endif

	return spec;
}

real3 BlinnPhone(Surface s, real roughness, real3 specColor, BxDFContext c, real NoL, Light l)
{
	real a2 = Pow4(roughness);
	real3 spec;

	real D  = D_Blinn(a2, c.NoH);
	real Vis = Vis_Implicit();
	real3 F = _F_Schlick(specColor, c.VoH);
	spec = D * Vis * F;

	return spec;
}

real3 BlinnKelemenPhone(Surface s, real roughness, real3 specColor, BxDFContext c, real NoL, Light l)
{
	real a2 = Pow4(roughness);
	real3 spec;

	real D = D_Blinn(a2, c.NoH);
	real Vis = Vis_Kelemen(c.VoH);
	real3 F = _F_Schlick(specColor, c.VoH);
	spec = D * Vis * F;

	return spec;
}
#endif