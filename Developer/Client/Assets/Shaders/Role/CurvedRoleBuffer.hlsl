#ifndef CURVED_ROLE_BUFFER_HLSH
#define CURVED_ROLE_BUFFER_HLSH

#include "../Includes/WorldCurvedCG.hlsl"	
	
sampler2D _MainTex;
//#if USE_DISSOLVE
sampler2D _DissolveNoise;
//#endif
CBUFFER_START(UnityPerMaterial)
	float _Alpha;
	float4 _Color;
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