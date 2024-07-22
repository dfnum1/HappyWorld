#ifndef MATCAP_HLSH
#define MATCAP_HLSH

#include "PBRLib.hlsl"
#include "../Includes/WorldCurvedCG.hlsl"

//顶点输出(片元输入)结构
struct VertexToFragment
{
	float4 diffuseUVAndMatCapCoords : TEXCOORD0;
	float4 position : SV_POSITION;
	float3 worldSpaceReflectionVector : TEXCOORD1;
	float3 worldPos : TEXCOORD2;
	float4 TtoW0 : TEXCOORD3;
	float4 TtoW1 : TEXCOORD4;
	float4 TtoW2 : TEXCOORD5;//xyz 存储着 从切线空间到世界空间的矩阵，w存储着世界坐标
#if USE_DISSOLVE
	float3 dissolve : TEXCOORD6;
	UNITY_FOG_COORDS(7)
#else
	UNITY_FOG_COORDS(6)
#endif
	
};

VertexToFragment MatCapVS(appdata_tan v)
{
	VertexToFragment output;

	//漫反射UV坐标准备：存储于TEXCOORD1的前两个坐标xy。
	output.diffuseUVAndMatCapCoords.xy = TRANSFORM_TEX(v.texcoord, _DiffuseTex);

	//MatCap坐标准备：将法线从模型空间转换到观察空间，存储于TEXCOORD1的后两个纹理坐标zw
	output.diffuseUVAndMatCapCoords.z = dot(normalize(UNITY_MATRIX_IT_MV[0].xyz), normalize(v.normal));
	output.diffuseUVAndMatCapCoords.w = dot(normalize(UNITY_MATRIX_IT_MV[1].xyz), normalize(v.normal));
	//归一化的法线值区间[-1,1]转换到适用于纹理的区间[0,1]
	output.diffuseUVAndMatCapCoords.zw = output.diffuseUVAndMatCapCoords.zw * 0.5 + 0.5;

	//坐标变换
	CURVED_WORLD_TRANSFORM_POINT(v.vertex,_CurveFactor);
	output.position = UnityObjectToClipPos(v.vertex);

	//世界空间位置
	output.worldPos = mul(UNITY_MATRIX_M, v.vertex).xyz;

	//世界空间法线
	float3 worldNormal = normalize(mul((float3x3)UNITY_MATRIX_M, v.normal));
	half3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz).xyz;
	half3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w;

#if USE_DISSOLVE
	output.dissolve.xy = TRANSFORM_TEX(v.texcoord, _DissolveNoise);
	output.dissolve.z = (output.worldPos.y - _DissolveHeight*(1 - _DissolveAmount*_DissolveHeightSpeed))*_DissolveHeightSpeed;
#endif

	//世界空间反射向量
	output.worldSpaceReflectionVector = reflect(output.worldPos - _WorldSpaceCameraPos.xyz, worldNormal);


	//前3x3存储着从切线空间到世界空间的矩阵，后3x1存储着世界坐标
	output.TtoW0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, output.worldPos.x);
	output.TtoW1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, output.worldPos.y);
	output.TtoW2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, output.worldPos.z);

	UNITY_TRANSFER_FOG(output, output.position);
	return output;
}

float4 MatCapFS(VertexToFragment input)
{
	float3 worldPos = float3(input.TtoW0.w,input.TtoW1.w,input.TtoW2.w);//世界坐标

	half3 normal = UnpackNormal(tex2D(_BumpMap, input.diffuseUVAndMatCapCoords.xy));
	half3 normalTangent = normal*_BumpValue;
	normalTangent.z = sqrt(1.0 - saturate(dot(normalTangent.xy, normalTangent.xy)));
	half3 worldNormal = normalize(half3(dot(input.TtoW0.xyz, normalTangent)*(_BumpValue + 1), dot(input.TtoW1.xyz, normalTangent)*(_BumpValue + 1), dot(input.TtoW2.xyz, normalTangent)));
	float3 lightDir = -GetMainLight().direction * _UseDirLight + _ShdowLight*(1- _UseDirLight);

	float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - worldPos);

	half3 refDir = reflect(-viewDir, worldNormal);
	half3 halfDir = normalize(lightDir + viewDir);
	half nv = saturate(dot(worldNormal, viewDir));
	half nl = saturate(dot(worldNormal, lightDir));
	half nh = saturate(dot(worldNormal, halfDir));
	half lv = saturate(dot(lightDir, viewDir));
	half lh = saturate(dot(lightDir, halfDir));

	float roughness = saturate(max(tex2D(_RoughnessMap, input.diffuseUVAndMatCapCoords.xy).r, _RoughnessStrength + EPS));
	float metalness = 0;// tex2D(_MetallicGlossMap, input.diffuseUVAndMatCapCoords.xy).r*_MetallicStrength;
	float occlusion = saturate(tex2D(_AOMap, input.diffuseUVAndMatCapCoords.xy).r);

	//漫反射颜色
	float4 diffuseColor = tex2D(_DiffuseTex, input.diffuseUVAndMatCapCoords.xy);
	Util_AmbientColor(diffuseColor,_AmbientStrength);
	diffuseColor.rgb += tex2D(_EmissionMap, input.diffuseUVAndMatCapCoords.xy).rgb * _EmissionStrength;

	half Rim = 1.0 - max(0, nv);
	float rimX = pow(Rim, 1 / _RimEdgePower)*_RimFactor;
	diffuseColor.rgb = diffuseColor.rgb*(1 - rimX) + _RimColor *rimX;

	//计算1 - 反射率,漫反射总比率
	half oneMinusReflectivity = (1 - metalness);// *unity_ColorSpaceDielectricSpec.a;
												//计算漫反射率
	half3 diffColor = diffuseColor.xyz * oneMinusReflectivity;

	//half3 indirectDiffuse =occlusion;//计算间接光漫反射
	half3 indirectDiffuse = lerp(lerp(1.0, occlusion, _AOStrength), 1.0, metalness * (1.0 - roughness) * (1.0 - roughness));

	indirectDiffuse *= diffColor;

	half V = ComputeSmithJointGGXVisibilityTerm(nl, nv, roughness);//计算BRDF高光反射项，可见性V
	half D = ComputeGGXTerm(nh, roughness);//计算BRDF高光反射项,法线分布函数D
	half3 F = 1;// ComputeFresnelTerm(_FresnelColor, lh);//计算BRDF高光反射项，菲涅尔项F

	half3 specularTerm =  V * D * F;//计算镜面反射项
	half3 diffuseTerm = ComputeDisneyDiffuseTerm(nv, nl, lh, roughness, diffColor);//计算漫反射项
	specularTerm = clamp(specularTerm, 0, 1);
																				   //从提供的MatCap纹理中，提取出对应光照信息
	float3 matCapColor = tex2D(_MatCap, input.diffuseUVAndMatCapCoords.zw).rgb;

	float3 baseColor = M_PI * (diffuseTerm + specularTerm) * nl*matCapColor.rgb*_MatCapStrength*D;
	//细节颜色和主颜色进行插值，成为新的主颜色
	half3 mainColor = baseColor + indirectDiffuse;

	//最终颜色
	float4 finalColor = float4(mainColor*_MainColor.rgb *_ProjectorColor.rgb, _MainColor.a*_Alpha);
#if USE_DISSOLVE
	fixed dissove = tex2D(_DissolveNoise, input.dissolve.xy).r;
	dissove = dissove + (1 - _DissolveAmount);
	float dissolve_alpha = 1 - input.dissolve.z*dissove;
	clip(dissolve_alpha);
	clip(dissove - 1);
	float edge_area = saturate(1 - saturate((dissove - 1 + _DissolveEdgeWidth) / _DissolveSmoothness));
	edge_area *= _DissolveEdgeColor.a*saturate(_DissolveAmount);
	finalColor.rgb = lerp(finalColor.rgb, _DissolveEdgeColor.rgb * 10, edge_area);
#else
	finalColor = clamp(finalColor, 0, 1.5);
#endif

	UNITY_APPLY_FOG(input.fogCoord, finalColor);
	return finalColor;
}


#endif