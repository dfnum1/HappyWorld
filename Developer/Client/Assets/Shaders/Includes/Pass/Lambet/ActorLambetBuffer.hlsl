#ifndef LAMBET_PBR_BUFFER_HLSH
#define LAMBET_PBR_BUFFER_HLSH

#include "../Includes/WorldCurvedCG.hlsl"

sampler2D _DiffuseTex;
uniform sampler2D _BumpMap;
uniform sampler2D _EmissionMap;
uniform sampler2D _AOMap;
samplerCUBE _Cubemap;
//#if USE_DISSOLVE
sampler2D _DissolveNoise;
//#endif

CBUFFER_START(UnityPerMaterial)
	float4 _MainColor;
	float4 _ProjectorColor;
	float _Alpha;
	//float4 _DiffuseColor;

	float4 _DiffuseTex_ST;


	uniform fixed _BumpValue;


	uniform float _EmissionStrength;

	float _Lambet;

	float3 _RimColor;
	float _RimFactor;
	float _RimPower;


	//float _AOStrength;

	//fixed4 _LightDir;

	fixed4 _FresnelColor;
	fixed _FresnelScale;

	half _FresnelRange;

	fixed _CurveFactor;

	//#if USE_DISSOLVE
	float4 _DissolveNoise_ST;
	half _DissolveAmount;
	half _DissolveSmoothness;
	float4 _DissolveEdgeColor;
	float _DissolveEdgeWidth;
	float _DissolveHeight;
	fixed _DissolveHeightSpeed;
	//#endif

	// MESH SHADOW
	fixed   _ShadowEffect;
	fixed _ShadowFalloff;
	fixed _ShadowOffsetX;
	fixed _ShadowOffsetZ;
	fixed _GroundHeight;
	fixed _UseDirLight;
	fixed4 _ShdowLight;
	fixed4 _ShadowColor;
	
	// OUTLINE
	fixed4 _OutlineColor;
	float _Outline;
	float _FarCull;	

CBUFFER_END

#endif