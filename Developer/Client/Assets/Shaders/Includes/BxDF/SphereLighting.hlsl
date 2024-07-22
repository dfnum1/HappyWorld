#ifndef _SPHERE_LIGHTING_HLSL_
#define _SPHERE_LIGHTING_HLSL_

#include "..\BaseDefine\ConstDefine.hlsl"
#include "..\BaseDefine\CommonDefine.hlsl"
#include "..\BaseDefine\SurfaceBase.hlsl"

float3 _SphereLight0Pos;
real  _SphereLight0Radius;
real4 _SphereLight0Color;

Light SphereLightInit(Surface s, float3 lightPos, float radius, float4 color)
{
    Light data = (Light) 0;
    
    float3 L = lightPos - s.posWorld;

    float len = length(L);

    float3 lightDir = L / len;

    float3 r = reflect(L, s.normalWorld);

    float3 centerToRay = L * r * r - L;

    float3 closestPoint = L + centerToRay * clamp(radius / length(centerToRay), 0.0, 1.0);

    data.direction = normalize(closestPoint);
    data.color = color.rgb;
    data.distanceAttenuation = 1.0;
    data.shadowAttenuation = 1.0;

    return data;
}

#endif