#ifndef MATCAP_ROLE_BUFFER_HLSH
#define MATCAP_ROLE_BUFFER_HLSH

#include "../Includes/WorldCurvedCG.hlsl"
sampler2D _DetailTex;
sampler2D _MatCap;
sampler2D _RoughnessMap;
uniform sampler2D _BumpMap;
uniform sampler2D _EmissionMap;
uniform sampler2D _AOMap;
//#if USE_DISSOLVE
sampler2D _DissolveNoise;
//#endif
sampler2D _DiffuseTex;
CBUFFER_START(UnityPerMaterial)

float4 _MainColor;
float4 _ProjectorColor;
float _Alpha;
//float4 _DetailTex_ST;
//float _DetailTexDepthOffset;
//float4 _DiffuseColor;

	float4 _DiffuseTex_ST;

	half _MatCapStrength;


	half _MetallicStrength;
	half _RoughnessStrength;


	uniform fixed _BumpValue;


	uniform float _EmissionStrength;

	float3 _RimColor;
	float _RimFactor;
	float _RimEdgePower;


	float _AOStrength;

	//float4 _FresnelColor;

	fixed _AmbientStrength;
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
	fixed3 _ShdowLight;
	fixed4 _ShadowColor;

	// OUTLINE
	fixed4 _OutlineColor;
	float _Outline;
	float _FarCull;	
CBUFFER_END

#endif