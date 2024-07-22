#ifndef COMMON_UTIL_HLSH
#define COMMON_UTIL_HLSH

#include "BaseDefine/CommonDefine.hlsl"

void Unity_DotProduct_float3(float3 A, float3 B, out float Out)
{
    Out = dot(A, B);
}

void Unity_Multiply_float(float A, float B, out float Out)
{
    Out = A * B;
}

void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
{
    Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
}

void Unity_Clamp_float(float In, float Min, float Max, out float Out)
{
    Out = clamp(In, Min, Max);
}

void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
{
    Out = lerp(A, B, T);
}

half getFresnelNormalize(half fresnel, half fresnelpow, half3 normal, float3 camera_direction)
{
	half camera_direction_ilength = 1 / sqrt(dot(camera_direction, camera_direction));
	return saturate(fresnel + pow(saturate(1.0f - abs(dot(camera_direction, normal) * camera_direction_ilength)), fresnelpow) * (1- fresnel));
}

inline float Util_AlphaDissolve(float4 vertex,float Start, float Distance)
{
	float4 wpos = mul(UNITY_MATRIX_M, vertex);
	float lengthInCamera = length(_WorldSpaceCameraPos - wpos.xyz);
	return saturate( (lengthInCamera- Start)/ Distance);
	//float distFar = UNITY_Z_0_FAR_FROM_CLIPSPACE(wpos.z);
	//return saturate(distFar * distFar * _ProjectionParams.x);
}

inline float Util_AlphaFadeNearFar(float4 vertex, float NearStart, float NearEnd, float FarStart, float FarEnd)
{
	//暂时用if else 替代
	float4 wpos = mul(UNITY_MATRIX_M, vertex);
	float lengthInCamera = length(_WorldSpaceCameraPos - wpos.xyz);
	if (lengthInCamera < NearEnd)
		return saturate((lengthInCamera - NearStart) / (NearEnd - NearStart));
	if(lengthInCamera > FarStart)
		return saturate((FarEnd - lengthInCamera) / (FarEnd-FarStart));

	return 1;
	//float distFar = UNITY_Z_0_FAR_FROM_CLIPSPACE(wpos.z);
	//return saturate(distFar * distFar * _ProjectionParams.x);
}

inline void Util_AmbientColor(inout float3 color, float fStrength)
{
	//color *= (float3(1, 1, 1)*(1 - fStrength) + UNITY_LIGHTMODEL_AMBIENT.rgb*fStrength);
}
inline void Util_AmbientColor(inout float4 color, float fStrength)
{
	//color.rgb *= (float3(1, 1, 1)*(1 - fStrength) + UNITY_LIGHTMODEL_AMBIENT.rgb*fStrength);
}
inline void Util_AmbientColorAndAlpha(inout float4 color, float fStrength)
{
	//color *= (float4(1,1,1,1)*(1-fStrength) + UNITY_LIGHTMODEL_AMBIENT*fStrength);
}


float4 getTreeLeafAnimationStream(float _AnimationStemX, float _AnimationStemY, float _AnimationStemZ, float strength, float fade, float windForce)
{
	float4 offset = float4(0, 0, 0, 0);
	float tempR = sin(3.1416 * _Time.y * (clamp(fade-0.5, 0, 1) + windForce));
	offset.x = tempR * _AnimationStemX*strength;
	offset.y = tempR  * _AnimationStemY*strength;
	offset.z = tempR  * _AnimationStemZ*strength;
	return offset;
}

float4 FixedTess( float tessValue )
{
	return tessValue;
}

float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
{
	float3 wpos = mul(o2w,vertex).xyz;
	float dist = distance (wpos, cameraPos);
	float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
	return f;
}

float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
{
	float4 tess;
	tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
	tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
	tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
	tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
	return tess;
}

float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
{
	float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
	float len = distance(wpos0, wpos1);
	float f = max(len * scParams.y / (edgeLen * dist), 1.0);
	return f;
}

float DistanceFromPlane (float3 pos, float4 plane)
{
	float d = dot (float4(pos,1.0f), plane);
	return d;
}

bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
{
	float4 planeTest;
	planeTest.x = (( DistanceFromPlane(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
				  (( DistanceFromPlane(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
				  (( DistanceFromPlane(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
	planeTest.y = (( DistanceFromPlane(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
				  (( DistanceFromPlane(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
				  (( DistanceFromPlane(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
	planeTest.z = (( DistanceFromPlane(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
				  (( DistanceFromPlane(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
				  (( DistanceFromPlane(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
	planeTest.w = (( DistanceFromPlane(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
				  (( DistanceFromPlane(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
				  (( DistanceFromPlane(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
	return !all (planeTest);
}

float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
{
	float3 f;
	f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
	f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
	f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

	return CalcTriEdgeTessFactors (f);
}

float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
{
	float3 pos0 = mul(o2w,v0).xyz;
	float3 pos1 = mul(o2w,v1).xyz;
	float3 pos2 = mul(o2w,v2).xyz;
	float4 tess;
	tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
	tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
	tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
	tess.w = (tess.x + tess.y + tess.z) / 3.0f;
	return tess;
}

float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
{
	float3 pos0 = mul(o2w,v0).xyz;
	float3 pos1 = mul(o2w,v1).xyz;
	float3 pos2 = mul(o2w,v2).xyz;
	float4 tess;

	if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
	{
		tess = 0.0f;
	}
	else
	{
		tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
		tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
		tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
		tess.w = (tess.x + tess.y + tess.z) / 3.0f;
	}
	return tess;
}
#endif