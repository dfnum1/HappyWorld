#ifndef COMMON_MATH_INCLUDED
#define COMMON_MATH_INCLUDED


half Pow5(half x)
{
    half x2 = x * x;
    return x2 * x2 * x;
}

half sq(half x)
{
    return x * x;
}
 

half2 ParallaxOffset1Step(half h, half height, half3 viewDir)
{
    h = h * height - height / 2.0;
    half3 v = normalize(viewDir);
    v.z += 0.42;
    return h * (v.xy / v.z);
}


half2 ParallaxRaymarching(half2 uv, half2 viewDir, half _ParallaxOffset, sampler2D _HeightMap, half4 _HeightMap_ST)
{
    half2 uvOffset = 0;
    half stepSize = 0.1;
    half2 uvDelta = viewDir * (stepSize * _ParallaxOffset);


    half stepHeight = 1;
    half surfaceHeight = tex2D(_HeightMap, uv.xy * _HeightMap_ST.xy).g;
    
    for (int i = 1; i < 10 && stepHeight > surfaceHeight; i++)
    {
        uvOffset -= uvDelta;
        stepHeight -= stepSize;
        surfaceHeight = tex2D(_HeightMap, uv.xy * _HeightMap_ST.xy + uvOffset).g;
    }
    
    return uvOffset;
}


half2 GetParallaxUV(half3 wViewDir, half2 texcoord, half3 tangent, half3 binormal, half heightMap, half parallaxOffset)
{
    half2 viewL = half2(dot(wViewDir, tangent),
						   dot(wViewDir, binormal));

    half2 offset = heightMap * viewL * parallaxOffset;
	//offset.y = -offset.y;
    texcoord -= offset;

    return texcoord;
}

half3x3 GetVertTBN(half3 normalWorld, half4 tangent)
{
    half4 tangentWorld = half4(UnityObjectToWorldDir(tangent.xyz), tangent.w);
    half sign = tangentWorld.w * unity_WorldTransformParams.w;
    half3 binormal = cross(normalWorld, tangentWorld.xyz) * sign;
    half3x3 TBN = half3x3(tangentWorld.xyz, binormal, normalWorld);
    return TBN;
}

half3x3 GetBillboardPNT()
{
    half3 dirWS = UNITY_MATRIX_M._m03_m13_m23 - _WorldSpaceCameraPos;
    half3 WorldView = normalize(mul((half3x3) unity_WorldToObject, dirWS));
    half3 m0 = normalize(cross(WorldView, half3(0, 1, 0)));
    half3 m2 = normalize(WorldView);
    half3 m1 = cross(m0, m2);
    half3x3 PNT = half3x3(m0, m1, m2);
    return PNT;
}

half3 ShiftTangent(half3 T, half3 N, half shift)
{
    half3 shiftedT = T + (shift * N); // shift = shift value + shift texture
    return normalize(shiftedT);
}


half ScreenDitherToAlpha(half x, half y, half c0)
{
    const half dither[64] =
    {
        0, 32, 8, 40, 2, 34, 10, 42,
		48, 16, 56, 24, 50, 18, 58, 26,
		12, 44, 4, 36, 14, 46, 6, 38,
		60, 28, 52, 20, 62, 30, 54, 22,
		3, 35, 11, 43, 1, 33, 9, 41,
		51, 19, 59, 27, 49, 17, 57, 25,
		15, 47, 7, 39, 13, 45, 5, 37,
		63, 31, 55, 23, 61, 29, 53, 21
    };

    int xMat = int(x) & 7;
    int yMat = int(y) & 7;

    half limit = (dither[yMat * 8 + xMat] + 11.0) / 64.0;
    return lerp(limit * c0, 1.0, c0);
}

half ScreenDitherToAlphaSimple(half x, half y, half c0)
{
    const half dither[16] =
    {
        0, 8, 2, 10,
		12, 4, 14, 6,
		3, 11, 1, 9,
		15, 7, 13, 5
    };

    int xMat = int(x) & 4;
    int yMat = int(y) & 4;

    half limit = (dither[yMat + xMat]) / 16.0;
    return lerp(limit * c0, 1.0, c0);
}

half3 UnPackNormalTex(half4 normalTex)
{
    normalTex.x *= normalTex.w;
    normalTex.xy = (normalTex.xy * 2 - 1);
    normalTex.z = sqrt(1.0 - saturate(dot(normalTex.xy, normalTex.xy)));
    return normalTex;
}

#endif