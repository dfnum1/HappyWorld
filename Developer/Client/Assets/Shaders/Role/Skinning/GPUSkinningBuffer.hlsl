#ifndef GPU_SKINNING_BUFFER_HLSH
#define GPU_SKINNING_BUFFER_HLSH
#include "../../Includes/WorldCurvedCG.hlsl"

sampler2D _MatcapTex;
sampler2D _DiffuseTex;
sampler2D _BumpMap;
sampler2D _MatCap;
sampler2D _EmissionMap;
sampler2D _AOMap;

//#if USE_DISSOLVE		
	sampler2D _DissolveNoise;
//#endif
fixed2 _SKIN_UVS[5];
CBUFFER_START(UnityPerMaterial)
	float4 _MainColor;
	float4 _ProjectorColor;

	float4 _DiffuseTex_ST;

	fixed _AOStrength;

	fixed _MatCapStrength;

	fixed _BumpValue;
	fixed _EmissionStrength;

	fixed3 _RimColor;
	fixed _RimFactor;
	fixed _RimEdgePower;

	fixed3 _ShdowLight;
	
	fixed _CurveFactor;

	//#if USE_DISSOLVE	
		float4 _DissolveNoise_ST;
		fixed _DissolveAmount;
		fixed _DissolveSmoothness;
		float4 _DissolveEdgeColor;
		fixed _DissolveEdgeWidth;
		fixed _DissolveHeight;
		fixed _DissolveHeightSpeed;
	//#endif
	
	// MESH SHADOW
	fixed   _ShadowEffect;
	fixed _ShadowFalloff;
	fixed _ShadowOffsetX;
	fixed _ShadowOffsetZ;
	fixed _GroundHeight;
	fixed _UseDirLight;
	fixed4 _ShadowColor;
	
	// OUTLINE
	fixed4 _OutlineColor;
	float _Outline;
	float _FarCull;	

CBUFFER_END

#endif