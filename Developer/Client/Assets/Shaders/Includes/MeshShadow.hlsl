#ifndef MESHSHDOW_UTILITIES
#define MESHSHDOW_UTILITIES

#include "Assets/Shaders/Includes/WorldCurvedCG.hlsl"

//CBUFFER_START(UnityPerMaterial)
//fixed   _ShadowEffect;
//fixed _ShadowFalloff;
//fixed _ShadowOffsetX;
//fixed _ShadowOffsetZ;
//fixed _CurveFactor;
//fixed _GroundHeight;
//fixed _UseDirLight;
//fixed4 _ShadowColor;
//fixed4 _ShdowLight;
//CBUFFER_END

struct shadow_v2f
{
	float4 pos : SV_POSITION;
	float4 col : COLOR;
};

float4 ShadowProjectPos(float4 vertPos)
{
    float4 shadowPos;

    //得到顶点的世界空间坐标
    float4 worldPos = mul(UNITY_MATRIX_M , vertPos);

    //灯光方向
	float3 lightDir = GetMainLight().direction*_UseDirLight + normalize(_ShdowLight.xyz)*(1-_UseDirLight);

    //阴影的世界空间坐标（低于地面的部分不做改变）
    shadowPos.y = min(worldPos.y , _GroundHeight);
	float rate = max(0 , worldPos.y - _GroundHeight) / lightDir.y;
    shadowPos.x = worldPos.x - (lightDir.x +_ShadowOffsetZ) * rate; 
	shadowPos.z = worldPos.z - (lightDir.z + _ShadowOffsetX) * rate; 
	shadowPos.w = worldPos.w;
    return shadowPos;
}


shadow_v2f shadow_vert(appdata_base v)
{
	_GroundHeight += 0.1f;
	//得到中心点世界坐标
    float3 center = float3(unity_ObjectToWorld[0].w , _GroundHeight , unity_ObjectToWorld[2].w);
	shadow_v2f o = (shadow_v2f)0;
	float4 shadowPos = ShadowProjectPos(v.vertex);
	float4 vertex = mul(unity_WorldToObject, shadowPos);
	CURVED_WORLD_TRANSFORM_POINT(vertex,_CurveFactor);
	o.pos = UnityObjectToClipPos(vertex);
	
	o.col = _ShadowColor;

    //计算阴影衰减
    float falloff = 1-saturate(distance(shadowPos.xyz, center) * _ShadowFalloff);

    //阴影颜色
    o.col.a *= falloff;
				
	return o;
}

fixed4 shadow_frag(shadow_v2f i) :SV_Target
{
	fixed alpha = (1-_DissolveAmount);
	fixed4 col = i.col*_ShadowEffect*alpha;
	
	return col;
}
#endif