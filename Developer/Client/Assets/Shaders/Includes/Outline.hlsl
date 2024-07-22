#ifndef OUTLINE_UTILITIES
#define OUTLINE_UTILITIES

#include "Assets/Shaders/Includes/WorldCurvedCG.hlsl"

struct appdata
{
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	float4 texcoord : TEXCOORD0;
};

struct v2f
{
	float4 vertex : SV_POSITION;
#if USE_DISSOLVE
	float3 dissolve : TEXCOORD1;
#endif
};

v2f vert (appdata v)
{
	v2f o = (v2f)0;
	CURVED_WORLD_TRANSFORM_POINT(v.vertex,_CurveFactor);
	o.vertex = TransformObjectToHClip(v.vertex.xyz);
	
	
	float dist = distance(_WorldSpaceCameraPos, mul(unity_ObjectToWorld, v.vertex).xyz);
	float factor = saturate(_FarCull);
	dist = dist * (1 - factor) + dist * saturate(_FarCull - dist) * factor;

	float4 pos = mul(UNITY_MATRIX_MV, v.vertex);

	float3 normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
	pos = pos + float4(normalize(normal), 0) * _Outline * 0.002 * dist;
	
	o.vertex = mul(UNITY_MATRIX_P, pos);


#if USE_DISSOLVE
	fixed3 worldPos = mul(UNITY_MATRIX_M, v.vertex).xyz;
	 o.dissolve.xy = TRANSFORM_TEX(v.texcoord, _DissolveNoise);// +frac(_DissTex_Scroll.xy * _Time.x);
	 o.dissolve.z = (worldPos.y - _DissolveHeight*(1 - _DissolveAmount*_DissolveHeightSpeed))*_DissolveHeightSpeed;
#endif
	return o;
}

fixed4 frag (v2f i) : SV_Target
{
	fixed4 finalColor = _OutlineColor;
#if USE_DISSOLVE
	fixed dissove = tex2D(_DissolveNoise, i.dissolve.xy).r;
	dissove = dissove + (1 - _DissolveAmount);
	float dissolve_alpha = 1 - i.dissolve.z*dissove;
	clip(dissolve_alpha);
	clip(dissove - 1);
	float edge_area = saturate(1 - saturate((dissove - 1 + _DissolveEdgeWidth) / _DissolveSmoothness));
	edge_area *= _DissolveEdgeColor.a*saturate(_DissolveAmount);
	finalColor.rgb = lerp(finalColor.rgb, _DissolveEdgeColor.rgb * 10, edge_area);
#else
	finalColor = clamp(finalColor, 0, 1.5);
#endif	
	return finalColor;
}
#endif