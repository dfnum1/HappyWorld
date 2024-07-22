#ifndef _PBR_PASS_HLSL_
#define	_PBR_PASS_HLSL_

#include "../../BaseDefine/VertexBase.hlsl"
#include "../../BaseDefine/SurfaceBase.hlsl"
#include "PBRBuffer.hlsl"
#include "../../BxDF/SphereLighting.hlsl"
#include "../../BxDF/EnvUtils.hlsl"
#include "../../BxDF/MixShading.hlsl"
#include "../Includes/WorldCurvedCG.hlsl"

struct Attributes
{
	float4 vertex			: POSITION;
	float2 uv0				: TEXCOORD0;
	float2 uv1				: TEXCOORD1;
	float3 normal			: NORMAL;
	float4 tangent			: TANGENT;

	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
	float4 positionCS : SV_POSITION;
	real2 uv : TEXCOORD0;
	real3 tangentWorld : TEXCOORD2;
	real3 bitangent : TEXCOORD3;
	real3 normalWorld : TEXCOORD4;
	float3 posWorld : TEXCOORD5;
#if USE_DISSOLVE
	real3 dissolve : TEXCOORD1;
#endif
	UNITY_VERTEX_INPUT_INSTANCE_ID
	UNITY_VERTEX_OUTPUT_STEREO
};

Varyings ActorVert(Attributes v)
{
	Varyings o = (Varyings)0;

	UNITY_SETUP_INSTANCE_ID(v);
	UNITY_TRANSFER_INSTANCE_ID(v, o);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

	CURVED_WORLD_TRANSFORM_POINT(v.vertex,_CurveFactor);
	VertexData vData = BuildVertexData(v.vertex, v.normal, v.tangent);

	o.uv				= v.uv0;
	o.tangentWorld.xyz	= vData.tangentWorld.xyz;
	o.bitangent.xyz		= vData.bitangent.xyz;
	o.normalWorld.xyz	= vData.normalWorld.xyz;
	o.posWorld.xyz		= vData.posWorld.xyz;
	o.positionCS		= vData.posMVP;

#if USE_DISSOLVE
	o.dissolve.xy = v.uv0*(1- _DissolveUVSwitch) + v.uv1* _DissolveUVSwitch;
	o.dissolve.z = (o.posWorld.y - _DissolveHeight * (1 - _DissolveAmount * _DissolveHeightSpeed)) * _DissolveHeightSpeed;
#endif

	return o;
}

real4 ActorFrag(Varyings i) : SV_TARGET
{
	UNITY_SETUP_INSTANCE_ID(i);
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

	Surface s = (Surface)0;

#ifdef USE_HEIGHT_MAP
	float h = SAMPLE_TEXTURE2D_DEF(_HeightMap, i.uv);
	BuildSurfaceUV(s, i.uv, h, _Height);
#else
	BuildSurfaceUV(s, i.uv);
#endif

	real4 baseAlpha = SAMPLE_TEXTURE2D_DEF(_BaseMap, s.uv0);
	real3 baseColor = baseAlpha.xyz*_BaseColor.xyz;

	real3 normalRGAO = SAMPLE_TEXTURE2D_DEF(_NormalAOTex, s.uv0).xyz;

	real3 MetalRoughMask = SAMPLE_TEXTURE2D_DEF(_MetalRoughMaskTex, s.uv0).xyz;

	BuildSurfaceDirtection(s, normalRGAO.xy, i.tangentWorld, i.bitangent, i.normalWorld, i.posWorld);

	real4 envCol = real4(0.5,0.5,0.5,1);

	//return MetalRoughMask.z;

	BuildSurfaceColor(s, baseColor, _BaseColor.w* baseAlpha.a, envCol, MetalRoughMask.y * _Roughness, MetalRoughMask.x * _Metallic, lerp(1.0, normalRGAO.z, _AOStrength), 0.01);

	real4 shadowCoord = TransformWorldToShadowCoord(i.posWorld.xyz);
	Light mainLight = GetMainLight();

	real NoL = max(0, dot(s.normalWorld, mainLight.direction));

	real4 GI = envCol;
	GI.xyz = s.diffColor * GI.xyz + s.specColor * GetReflectionLighting(s, GI);

	FDirectLighting lighting = MaskMergeLit(s, s.normalWorld, s.viewDir, NoL, mainLight, MetalRoughMask.z);

	real4 col = 0;
	col.a = s.alpha;
	col.rgb = lighting.Diffuse + lighting.Specular + lighting.Transmission;
	//col.rgb += lighting_1.Diffuse + lighting_1.Specular + lighting_1.Transmission;
	col.rgb += GI.rgb;

#if defined(USE_EMISSION_MAP)
	col.rgb += Emission(TEXTURE_SAMPLE_ARGS_DEF(_EmissionTex), s.uv0, _EmissionColor.rgb);
#endif

#if USE_DISSOLVE
	fixed dissove_flag = saturate(_DissolveAmount);
	fixed dissove = tex2D(_DissolveNoise, i.dissolve.xy).r;
	dissove = dissove + (1 - _DissolveAmount);
	float dissolve_alpha = 1 - i.dissolve.z*dissove;
	clip(dissolve_alpha);
	clip(dissove - 1);
	float edge_area = saturate(1 - saturate((dissove - 1 + _DissolveEdgeWidth) / _DissolveSmoothness));
	edge_area *= _DissolveEdgeColor.a*dissove_flag;
	col.rgb = lerp(col.rgb, _DissolveEdgeColor.rgb * 10, edge_area);
#endif	
	col = clamp(col, 0, 2);
	return col;
}

#endif