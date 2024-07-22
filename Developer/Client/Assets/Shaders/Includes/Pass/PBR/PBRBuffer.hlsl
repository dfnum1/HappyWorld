#ifndef _PBR_BUFFER_HLSL_
#define	_PBR_BUFFER_HLSL_

#include "../../BaseDefine/ConstDefine.hlsl"
#include "../../BaseDefine/CommonDefine.hlsl"

//#if USE_DISSOLVE	
sampler2D _DissolveNoise;
//#endif
TEXTURE2D_DEF(_BaseMap);
TEXTURE2D_DEF(_EmissionTex);
TEXTURE2D_DEF(_NormalAOTex);
TEXTURE2D_DEF(_MetalRoughMaskTex);

CBUFFER_START(UnityPerMaterial)
	real4	_BaseMap_ST;
	real4	_BaseColor;
	real	_Cutoff;

	real    _AOStrength;
	real	_Roughness;
	real	_Metallic;
//	real	_Hair;

	real4   _EmissionColor;

	real _CurveFactor;

	//#if USE_DISSOLVE	

	float4 _DissolveNoise_ST;
	fixed _DissolveAmount;
	fixed _DissolveSmoothness;
	float4 _DissolveEdgeColor;
	fixed _DissolveEdgeWidth;
	fixed _DissolveHeight;
	fixed _DissolveHeightSpeed;
	fixed _DissolveUVSwitch;
	//#endif

	//! SSS
	real _ScatterAmtX;
	real _ScatterAmtY;
	real _ScatterAmtZ;
	half _SkinToneMap;
	half _SkinStrength;

	// OUTLINE
	fixed4 _OutlineColor;
	float _Outline;
	float _FarCull;	

CBUFFER_END

#endif