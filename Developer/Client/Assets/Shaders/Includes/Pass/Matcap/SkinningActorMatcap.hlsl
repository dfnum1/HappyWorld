#ifndef MATCAP_PBR_HLSH
#define MATCAP_PBR_HLSH

#include "../Includes/WorldCurvedCG.hlsl"
#include "../Includes/SkinInclude.hlsl"

struct FragmentToVertex
{
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	float4 tangent: TANGENT;
	float2 texcoord : TEXCOORD0;
	float4 texcoord1 : TEXCOORD1;
	float4 texcoord2 : TEXCOORD2;
#if USE_DISSOLVE
	float3 dissolve : TEXCOORD3;
	UNITY_FOG_COORDS(4)
#else
	UNITY_FOG_COORDS(3)
#endif
};

struct VertexToFragment
{
	float4 diffuseUVAndMatCapCoords : TEXCOORD0;
	float4 position : SV_POSITION;
	float3 worldPos : TEXCOORD1;
	float4 TtoW0 : TEXCOORD2;
	float4 TtoW1 : TEXCOORD3;
	float4 TtoW2 : TEXCOORD4;//xyz 存储着 从切线空间到世界空间的矩阵，w存储着世界坐标
#if USE_DISSOLVE
	float3 dissolve : TEXCOORD5;
	UNITY_FOG_COORDS(6)
#else
	UNITY_FOG_COORDS(5)
#endif
};

//------------------------------------------------------------
// 顶点着色器
//------------------------------------------------------------
VertexToFragment MatcapVS(FragmentToVertex v)
{
	VertexToFragment output;

	float4 pos = gpuSkin4(v.vertex, v.texcoord1, v.texcoord2);
	v.vertex = UnityObjectToClipPos(pos);

	//漫反射UV坐标准备：存储于TEXCOORD1的前两个坐标xy。
	output.diffuseUVAndMatCapCoords.xy = TRANSFORM_TEX(v.texcoord + _SKIN_UVS[gpu_Skin_uv()].xy, _DiffuseTex);

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
	half3 worldTangent = normalize(mul(UNITY_MATRIX_M, float4(v.tangent.xyz, 0.0)).xyz);
	half3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w;


	//前3x3存储着从切线空间到世界空间的矩阵，后3x1存储着世界坐标
	output.TtoW0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, output.worldPos.x);
	output.TtoW1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, output.worldPos.y);
	output.TtoW2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, output.worldPos.z);

#if USE_DISSOLVE
	output.dissolve.xy = TRANSFORM_TEX(v.texcoord + _SKIN_UVS[gpu_Skin_uv()].xy, _DissolveNoise);// +frac(_DissTex_Scroll.xy * _Time.x);
	output.dissolve.z = (output.worldPos.y - _DissolveHeight * (1 - _DissolveAmount * _DissolveHeightSpeed)) * _DissolveHeightSpeed;
#endif
	UNITY_TRANSFER_FOG(output, output.position);
	return output;
}
//------------------------------------------------------------
// 片元着色器
//------------------------------------------------------------
float4 MatcapFS(VertexToFragment input) : COLOR
{
	float3 worldPos = float3(input.TtoW0.w,input.TtoW1.w,input.TtoW2.w);//世界坐标

	half3 normal = UnpackNormal(tex2D(_BumpMap, input.diffuseUVAndMatCapCoords.xy));
	half3 normalTangent = normal * _BumpValue;
	normalTangent.z = sqrt(1.0 - saturate(dot(normalTangent.xy, normalTangent.xy)));
	half3 worldNormal = normalize(half3(dot(input.TtoW0.xyz, normalTangent) * (_BumpValue + 1), dot(input.TtoW1.xyz, normalTangent) * (_BumpValue + 1), dot(input.TtoW2.xyz, normalTangent)));

#if USE_DIRLIGHT_LIGHT
	Light mainLight = GetMainLight();
	float3 lightDir = -mainLight.direction;
#else
	float3 lightDir = _ShdowLight;// float3(0, 0, 1);
#endif
	float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - worldPos);
	half nv = saturate(dot(worldNormal, viewDir));
	half nl = saturate(dot(worldNormal, lightDir));

	float occlusion = saturate(tex2D(_AOMap, input.diffuseUVAndMatCapCoords.xy).r);



	float3 diffuseColor = tex2D(_DiffuseTex, input.diffuseUVAndMatCapCoords.xy).rgb;
	diffuseColor += tex2D(_EmissionMap, input.diffuseUVAndMatCapCoords.xy).rgb * _EmissionStrength;

	half3 indirectDiffuse = lerp(1.0, occlusion, _AOStrength) * diffuseColor;

	float3 baseColor = diffuseColor;

	float3 matCapColor = tex2D(_MatCap, input.diffuseUVAndMatCapCoords.zw).rgb;

	half3 mainColor = baseColor * matCapColor.rgb * _MatCapStrength * nl + indirectDiffuse;

	half Rim = 1.0 - max(0, nv);
	float rimX = pow(Rim, 1 / _RimEdgePower) * _RimFactor;
	mainColor.rgb = mainColor.rgb * (1 - rimX) + _RimColor * rimX;

	float4 finalColor = float4(mainColor * _MainColor.rgb * _ProjectorColor.rgb, _MainColor.a);

	#if USE_DISSOLVE
		fixed dissove = tex2D(_DissolveNoise, input.dissolve.xy).r;
		dissove = dissove + (1 - _DissolveAmount);
		float dissolve_alpha = 1 - input.dissolve.z * dissove;
		clip(dissolve_alpha);
		clip(dissove - 1);
		float edge_area = saturate(1 - saturate((dissove - 1 + _DissolveEdgeWidth) / _DissolveSmoothness));
		edge_area *= _DissolveEdgeColor.a * saturate(_DissolveAmount);
		finalColor.rgb = lerp(finalColor.rgb, _DissolveEdgeColor.rgb * 10, edge_area);
	#else
		finalColor = clamp(finalColor, 0, 1.5);
	#endif	

	UNITY_APPLY_FOG(input.fogCoord, finalColor);
	return finalColor;
}

#endif