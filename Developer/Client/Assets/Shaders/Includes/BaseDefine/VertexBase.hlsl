#ifndef	_VERTEX_BASE_HLSL_
#define _VERTEX_BASE_HLSL_

#include "CommonDefine.hlsl"
#include "../BxDF/BxDFBaseFunction.hlsl"


struct appdata_base 
{
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	float4 texcoord : TEXCOORD0;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct appdata_tan 
{
	float4 vertex : POSITION;
	float4 tangent : TANGENT;
	float3 normal : NORMAL;
	float4 texcoord : TEXCOORD0;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct appdata_img
{
	float4 vertex : POSITION;
	half2 texcoord : TEXCOORD0;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct appdata_full 
{
	float4 vertex : POSITION;
	float4 tangent : TANGENT;
	float3 normal : NORMAL;
	float4 texcoord : TEXCOORD0;
	float4 texcoord1 : TEXCOORD1;
	float4 texcoord2 : TEXCOORD2;
	float4 texcoord3 : TEXCOORD3;
	fixed4 color : COLOR;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct VertexData
{
	float4 posWorld;
	real3 normalWorld;
	real3 bitangent;
	real3 tangentWorld;
	float4 posMVP;
};

// Warning: Do not use UNITY_MATRIX_MV anymore!!!
// Because:
//#define UNITY_MATRIX_MV    mul(UNITY_MATRIX_V, UNITY_MATRIX_M)
//#define UNITY_MATRIX_T_MV  transpose(UNITY_MATRIX_MV)
//#define UNITY_MATRIX_IT_MV transpose(mul(UNITY_MATRIX_I_M, UNITY_MATRIX_I_V))
//#define UNITY_MATRIX_MVP   mul(UNITY_MATRIX_VP, UNITY_MATRIX_M)

VertexData BuildVertexData(float4 vertex, float3 normal, float4 tangent)
{
	VertexData data = (VertexData)0;
	data.posWorld		= mul(UNITY_MATRIX_M, float4(vertex.xyz, 1));
	data.normalWorld	= normalize(mul(UNITY_MATRIX_M, float4(normal, 0)).xyz);
	data.tangentWorld	= normalize(mul(UNITY_MATRIX_M, float4(tangent.xyz, 0.0)).xyz);
	data.bitangent		= normalize( cross(data.normalWorld, data.tangentWorld) * tangent.w * unity_WorldTransformParams.w);	
	data.posMVP			= mul(UNITY_MATRIX_VP, data.posWorld);
	return data;
}

// Remove bitangent and tangent ALU
VertexData BuildVertexDataSimple(float4 vertex, float3 normal, float4 tangent)
{
	VertexData data		= (VertexData)0;
	data.posWorld		= mul(UNITY_MATRIX_M, float4(vertex.xyz, 1));
	data.normalWorld	= mul(UNITY_MATRIX_M, float4(normal, 0)).xyz;
	data.posMVP			= mul(UNITY_MATRIX_VP, data.posWorld);
	return data;
}


#endif