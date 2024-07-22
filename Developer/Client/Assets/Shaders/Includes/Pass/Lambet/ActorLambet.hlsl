#ifndef LAMBET_PBR_HLSH
#define LAMBET_PBR_HLSH

#include "../Includes/WorldCurvedCG.hlsl"

struct FragmentToVertex
{
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	float4 tangent: TANGENT;
	float2 uv : TEXCOORD0;
#if USE_DISSOLVE
	float3 dissolve : TEXCOORD1;
	UNITY_FOG_COORDS(2)
#else
	UNITY_FOG_COORDS(1)
#endif
};

//顶点输出(片元输入)结构
struct VertexToFragment
{
	float2 uv : TEXCOORD0;
	float4 vertex : SV_POSITION;
	float3 worldNormal : TEXCOORD1;
	float3 worldPos: TEXCOORD2;
    float3 tangent: TEXCOORD3;
    float3 bitangent: TEXCOORD4;

#if USE_DISSOLVE
	float3 dissolve : TEXCOORD5;
	UNITY_FOG_COORDS(6)
#else
	UNITY_FOG_COORDS(5)
#endif
};

VertexToFragment LambetVS(FragmentToVertex v)
{
	VertexToFragment output;
	CURVED_WORLD_TRANSFORM_POINT(v.vertex,_CurveFactor);
	output.vertex = UnityObjectToClipPos(v.vertex);
	output.uv = TRANSFORM_TEX(v.uv, _DiffuseTex);
	output.worldNormal =  normalize(mul((float3x3)UNITY_MATRIX_M, v.normal));
	output.worldPos = mul(UNITY_MATRIX_M, v.vertex).xyz;

	output.tangent = normalize(mul(UNITY_MATRIX_M, v.tangent).xyz);
	output.bitangent = normalize(cross(output.worldNormal, output.tangent.xyz));
#if USE_DISSOLVE
	output.dissolve.xy = TRANSFORM_TEX(v.uv, _DissolveNoise);
	output.dissolve.z = (output.worldPos.y - _DissolveHeight*(1 - _DissolveAmount*_DissolveHeightSpeed))*_DissolveHeightSpeed;
#endif


	UNITY_TRANSFER_FOG(output, output.vertex);
	return output;
}

float4 LambetFS(VertexToFragment input)
{
	Light mainLight = GetMainLight();
	float3 lightDir = mainLight.direction;
	float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - input.worldPos);
	half3 refDir = reflect(-viewDir, input.worldNormal);
	float4 diffuse = tex2D(_DiffuseTex, input.uv);
	
	float3x3 tangentMatrix = transpose(float3x3(input.tangent, input.bitangent, input.worldNormal));
	float3 texNormal = UnpackNormalScale(tex2D(_BumpMap, input.uv),_BumpValue).rgb;
	float3 normal = mul(tangentMatrix, texNormal);
	fixed nl = dot(normal, lightDir);
	half nv = saturate(dot(normal, viewDir));
	fixed lambet_half = nl*_Lambet+(1-_Lambet);
	
	half Rim = 1.0 - max(0, nv);
	diffuse.rgb += _RimColor *pow(Rim, 1/_RimPower)*_RimFactor;
	
	diffuse.rbg += tex2D(_EmissionMap, input.uv.xy).rgb* _EmissionStrength;
	
	
	//fixed3 reflection=texCUBE(_Cubemap,refDir).rgb;
	//fixed3 diffuseColor = lerp(diffuse.rgb, reflection, saturate(fresnel)*_FresnelColor);
#if USE_FRESNEL
	half fresnel = getFresnelNormalize(_FresnelScale, _FresnelRange, normal, viewDir);
	diffuse.rgb = diffuse.rgb*(1.0f - fresnel) + _FresnelColor*fresnel;
#endif

	fixed3 lambet = diffuse.rgb*mainLight.color.rgb * lambet_half;
	
	float4 finalColor = float4(lambet*_MainColor.rgb *_ProjectorColor.rgb, _MainColor.a*_Alpha);

#if USE_DISSOLVE
	fixed dissove = tex2D(_DissolveNoise, input.dissolve.xy).r;
	dissove = dissove+ (1- _DissolveAmount);
//	float dissolve_alpha = 1 -input.dissolve.z*dissove;
//	clip(dissolve_alpha);
	clip(dissove - 1);
	float edge_area = saturate(1 -saturate((dissove - 1 + _DissolveEdgeWidth) / _DissolveSmoothness));
	edge_area *= _DissolveEdgeColor.a*saturate(_DissolveAmount);
	finalColor.rgb = lerp( finalColor.rgb, _DissolveEdgeColor.rgb*10, edge_area);
#else
	finalColor = clamp(finalColor, 0, 1.5);
#endif

	UNITY_APPLY_FOG(input.fogCoord, finalColor);
	return finalColor;
}


#endif