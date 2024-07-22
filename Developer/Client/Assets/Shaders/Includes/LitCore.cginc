//v0.6
#ifndef LITCORE_INCLUDED
#define LITCORE_INCLUDED

#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "common_math.cginc"
#include "brdf.cginc"

//base prop
sampler2D _MainTex;
sampler2D _BumpMap;

half _Metallic,_Smoothness,_Fresnel, _Occ;
half _Saturation;
//half _Contrast;
half4 _Gradation;
half4 _LightColor0;
half4 _Color;
half _Cutoff;

#if defined(MSO)
sampler2D _MetallicGlossMap; 
half4 _MetallicGlossMap_ST;
#endif

#if defined (SPECULARCOLOR)
sampler2D _SpecularMap;
#endif

#if defined(SCATTER)
half4 _ScatterColor;
half _ScatterDistortion, _ScatteringScale, _ScatteringSPower;
#endif

#if defined (SKIN)
sampler2D _ScatterMap;
sampler2D _SkinLut;
half _SkinScatter;
#endif

#if defined (HAIR)
	#if defined (HAIRTANGENTMAP)
		sampler2D _HairTangentMap;
	#elif defined(HAIRTILINGFLOW)
		sampler2D _HairTilingFlowmap;
		half _HairTiling;
#endif

sampler2D _HairAOTex;

half4 _SpecColor1;
half _SpecularAmount1;
half _SpecShift1;
half _SpecPower1;
half _SpecJitter1;
half _SpecMask1;

half4 _SpecColor2;
half  _SpecularAmount2;
half _SpecShift2;
half _SpecPower2;
half _SpecJitter2;
half _SpecMask2;
#endif 

#if defined(TRANSPARENT)
half _Transparent;
#endif

#if defined(PARALLAX)
half _ParallaxOffset;
sampler2D _HeightMap;
#endif

#if defined(FUZZ)
half4 _FuzzColor;
half _Fuzz, _FuzzScatter;
#endif

#if defined(JEWELRY)
sampler2D _IDMASK;
samplerCUBE _CubeMap;
#endif

sampler2D _DepthTex;
half _DepthOffset;


struct VertexInputBase
{
    float4 vertex   : POSITION;
    half3 normal   : NORMAL;
    half2 uv0      : TEXCOORD0;
    half2 uv1      : TEXCOORD1;
    half4 tangent   : TANGENT;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct VertexOutputBase
{
    float4 pos : SV_POSITION;
	
	half4 eyeVec                         : TEXCOORD1;    // eyeVec.xyz | gradationColor
	float4 tangentToWorldAndPackedData[3] : TEXCOORD2;    // [3x3:tangentToWorld | 1x3:viewDirForParallax or worldPos]
	UNITY_LIGHTING_COORDS(5, 6)

    half4 ambientOrLightmapUV             : TEXCOORD7;    // SH or Lightmap UV

#if defined (HAIR)
	half4 tex                            : TEXCOORD0;
    half4 screenUV						: TEXCOORD8;
#else
	half2 tex                            : TEXCOORD0;
#endif
	//UNITY_VERTEX_INPUT_INSTANCE_ID
};


VertexOutputBase vertBase (VertexInputBase v)
{
    VertexOutputBase o; 
    UNITY_SETUP_INSTANCE_ID(v);
    //UNITY_TRANSFER_INSTANCE_ID(v, o);
    UNITY_INITIALIZE_OUTPUT(VertexOutputBase, o);

	
    half4 posWorld = mul(UNITY_MATRIX_M, v.vertex);
#if defined (BILLBOARD)
    half3x3 PNT = GetBillboardPNT();
	
	v.vertex.xyz = mul(v.vertex.xyz, PNT);
    v.normal.xyz = mul(v.normal.xyz, PNT);
	v.tangent.xyz = mul(v.tangent.xyz, PNT);
		
	posWorld.xyz = mul(UNITY_MATRIX_M, half4(v.vertex.xyz, 1.0)).xyz;
#endif	
    o.tangentToWorldAndPackedData[0].w = posWorld.x;
    o.tangentToWorldAndPackedData[1].w = posWorld.y;
    o.tangentToWorldAndPackedData[2].w = posWorld.z;

	//mul(UNITY_MATRIX_VP, mul(UNITY_MATRIX_M, half4(v.vertex.xyz, 1.0)));
    o.pos = mul(UNITY_MATRIX_VP, posWorld);

    o.tex.xy = v.uv0.xy;
    o.eyeVec.xyz = normalize(posWorld.xyz - _WorldSpaceCameraPos);
    o.eyeVec.w = 1;//dot((v.vertex.z * _Gradation.x + _Gradation.y), (v.vertex.y * _Gradation.z + _Gradation.w));
	
    half3 normalWorld = UnityObjectToWorldNormal(v.normal);
	float3x3 TBN = GetVertTBN(normalWorld, v.tangent);
    o.tangentToWorldAndPackedData[0].xyz = TBN[0];
    o.tangentToWorldAndPackedData[1].xyz = TBN[1];
    o.tangentToWorldAndPackedData[2].xyz = TBN[2];

    UNITY_TRANSFER_LIGHTING(o, v.uv1);

    o.ambientOrLightmapUV.rgb = max(half3(0,0,0), ShadeSH9 (half4(normalWorld, 1.0)));


#if defined (HAIR)

#ifdef _AOCHANNLE_UV1
	o.tex.zw = v.uv0;
#elif _AOCHANNLE_UV2
	o.tex.zw = v.uv1;
#endif
    o.screenUV = ComputeScreenPos(o.pos);
#endif
    return o;
}

fixed4 fragBase(VertexOutputBase i, fixed facing : VFACE) : SV_Target
{
    //UNITY_SETUP_INSTANCE_ID(i);
	
#if defined (MULTIPLY)
	return half4 (_Color.rgb,tex2D(_MainTex, i.tex.xy).r*_Color.a);
#endif

    // Input
	half3 eyeVec = normalize(i.eyeVec.xyz);
	half3 posWorld = half3(i.tangentToWorldAndPackedData[0].w, i.tangentToWorldAndPackedData[1].w, i.tangentToWorldAndPackedData[2].w);
	half3 tangent = i.tangentToWorldAndPackedData[0].xyz;
	half3 binormal = i.tangentToWorldAndPackedData[1].xyz;
	float3 normal = i.tangentToWorldAndPackedData[2].xyz;
    if (facing)
        normal = facing * normal;
    float3 viewDir = -eyeVec.xyz;

	half2 texcoord = i.tex.xy;
	
#if defined (PARALLAX)
	half heightMap = tex2D(_HeightMap, i.tex.xy).r;
	texcoord =  GetParallaxUV(viewDir,i.tex.xy,tangent,binormal,heightMap , _ParallaxOffset);
#endif
    half4 albedo = tex2D(_MainTex, texcoord) * _Color;
	
#if defined (ADDMULTIPLY)
	albedo.rgb *=albedo.a;
#endif

	half metallic =  _Metallic;
	half smoothness = _Smoothness;   
    half occ = 1;
#if defined (MSO)
	half4 metallicGloss = tex2D(_MetallicGlossMap, texcoord * _MetallicGlossMap_ST.xy);
	metallic += metallicGloss.r;
	smoothness += metallicGloss.g;
	occ = lerp(1,metallicGloss.b,_Occ);
#endif

#if defined (HAIR)
	half HairAOTex = tex2D(_HairAOTex, i.tex.zw).r;
	occ = lerp(1, HairAOTex, _Occ);
#endif


	// Input End
#if defined (HAIRTANGENTMAP)
	half3 hairTangentmap = tex2D(_HairTangentMap, texcoord).rgb;
	hairTangentmap = hairTangentmap * 2 - 1;
	half3 hairTangent = (tangent*hairTangentmap.x) + (binormal*hairTangentmap.y) + (normal*hairTangentmap.z);
	hairTangent = normalize(hairTangent);
#endif
  
	// Bump
#if defined (NORMALTEX)
	half4 normalTex = tex2D(_BumpMap, texcoord);
    normalTex.x *= normalTex.w;
    normalTex.xy = (normalTex.xy * 2 - 1);
    normalTex.z = sqrt(1.0 - saturate(dot(normalTex.xy, normalTex.xy)));
	normal = normalize(tangent * normalTex.x + binormal * normalTex.y + normal * normalTex.z);
#endif
	// Bump End
	
    half roughness = 1 - smoothness;
    half3 specColor = lerp(unity_ColorSpaceDielectricSpec.rgb, albedo.rgb, metallic);
    half oneMinusReflectivity = unity_ColorSpaceDielectricSpec.a - metallic * unity_ColorSpaceDielectricSpec.a;
#if !defined (GI)
    oneMinusReflectivity = min(1,oneMinusReflectivity + 0.4);
#endif
    half3 diffColor = albedo.rgb * oneMinusReflectivity;
	
	
	// Light   
#if defined (BILLBOARD)
    posWorld = UNITY_MATRIX_M._m03_m13_m23 + normal.xyz * 0.5;
	//posWorld = mul(UNITY_MATRIX_M, half4(normal.xyz * 0.5, 1.0)).xyz;
#endif
	
    UNITY_LIGHT_ATTENUATION(atten, i, posWorld);
	
#if defined (BILLBOARD) && defined(UNITY_INSTANCING_ENABLED)
    atten = 1;
#endif
	
    half3 mainLight = _LightColor0.rgb * saturate(atten);

#ifndef USING_DIRECTIONAL_LIGHT
    half3 lightDir = normalize(UnityWorldSpaceLightDir(posWorld.xyz));
#else
	half3 lightDir = _WorldSpaceLightPos0.xyz;
#endif

#if defined (SPECULARCOLOR)
    half3 SpecularMap = tex2D(_SpecularMap, texcoord).rgb;
    specColor = SpecularMap.rgb;
#endif

	// Dir
	half3 halfDir = normalize(half3(lightDir.xyz) + viewDir);
	half nv = abs(dot(normal, viewDir));
	half nl = saturate(dot(normal, lightDir.xyz));
	half nh = saturate(dot(normal, halfDir));
	half lh = saturate(dot(lightDir.xyz, halfDir));
	// Dir End


	// GI
    half3 GIdiffuse = 0.5;
#if defined (ADDPASS)
	GIdiffuse = 0;
#endif
    half3 GIspecular = 0;
#if defined (GI)
	half3 reflUVW = reflect(eyeVec.xyz, normal);
	half GIperceptualRoughness = roughness * (1.7 - 0.7*roughness);
	half mip = GIperceptualRoughness * 6;
	GIspecular = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0,reflUVW.xyz, mip).rgb;

#if defined (JEWELRY)
	half3 cubemapColor = texCUBElod(_CubeMap,half4(reflUVW.xyz,0)).rgb;
	cubemapColor *= i.ambientOrLightmapUV.rgb * albedo.rgb;
	GIspecular = lerp(GIspecular,cubemapColor,metallicGloss.a); 
#endif 
	
	GIspecular = DecodeHDR(GIspecular.rgbr * occ, unity_SpecCube0_HDR);
    GIdiffuse = occ * i.ambientOrLightmapUV.rgb;

    half grazingTerm = saturate(smoothness + (1 - oneMinusReflectivity));
    half3 fresnelLerp = lerp (specColor, grazingTerm,  pow (1 - nv,5)*_Fresnel);
#endif
	// GI End

	
	// Diffuse term
	half3 diffuseTerm = 0;
#ifdef HAIR
	diffuseTerm =lerp(0.25,1,nl);
	mainLight = saturate(mainLight + nl*2);
#else
    half fd90 = 0.5 + 2 * lh * lh * roughness;
    half lightScatter   = (1 + (fd90 - 1) * pow(1 - nl,5));
    half viewScatter    = (1 + (fd90 - 1) * pow(1 - nv,5));
	 diffuseTerm = lightScatter * viewScatter * nl;
#endif 
	// Diffuse term End
	
	// Specular
	half roughness2 = roughness * roughness;
	roughness2 = max(roughness2, 0.002);
	half surfaceReduction = 1.0 / (roughness2*roughness2 + 1.0);

	half specularTerm = 0;
	half3 fresnelTerm = 0; 
#ifndef HAIR
	specularTerm = GetSpecTerm(roughness2, nl, nv, nh);	
	fresnelTerm = specColor + (1-specColor) * pow ( 1-lh,5);
#endif


#if defined (SKIN)
	specularTerm = lerp(specularTerm, GetSpecTerm(roughness2 * 0.3, nl, nv, nh), 0.15);
#endif
	specularTerm *= any(specColor) ? 1.0 : 0.0;
    
    half3 specularColor = specularTerm * fresnelTerm ;
	// Specular End
	

		 
	// Hair
#if defined (HAIR)

	half3 t1 = 0;
	half3 t2 = 0;

	#if defined (HAIRTANGENTMAP)
		half3 worldTangent = hairTangent;
		t1 = ShiftTangent(worldTangent, normal, _SpecShift1 );
		t2 = ShiftTangent(worldTangent, normal, _SpecShift2 );
	#endif

	#if defined (HAIRTILINGFLOW)
		half3 worldBinormal = normalize(binormal);
		half shiftTex = tex2D(_HairTilingFlowmap, half2(texcoord.x * _HairTiling, texcoord.y)) -0.5 ;
		t1 = ShiftTangent(worldBinormal, normal, _SpecShift1 + shiftTex * _SpecJitter1);
		t2 = ShiftTangent(worldBinormal, normal, _SpecShift2 + shiftTex * _SpecJitter2);
	#endif

    half3 spec1 =KajiyaSpecular(t1, viewDir, lightDir.xyz, _SpecPower1 * 128.0)*_SpecColor1 * _SpecularAmount1; 
    half3 spec2 =KajiyaSpecular(t2, viewDir, lightDir.xyz, _SpecPower2 * 128.0)*_SpecColor2 * _SpecularAmount2;// to do: spec2 need spec mask

    specularColor += saturate(spec1) * pow(albedo.a, _SpecMask1);
	specularColor += saturate(spec2) * pow(albedo.a, _SpecMask2);
	//fix bug v0.4.3
	specularColor *= atten * _LightColor0.rgb;
#endif
	// Hair End

	// Skin
#if defined (SKIN)
	half scatterMap = tex2D(_ScatterMap, texcoord).r;
	half curvture = min(1, scatterMap + _SkinScatter);
#ifdef _BLURREDNORMAL_ON
	half3 wNLowR = lerp(normal, i.tangentToWorldAndPackedData[2].xyz, 1.0);
	half3 wNLowG = lerp(normal, i.tangentToWorldAndPackedData[2].xyz, 0.7);
	half3 wNLowB = lerp(normal, i.tangentToWorldAndPackedData[2].xyz, 0.3);
	half3 diffLdn = half3(dot(wNLowR, lightDir), dot(wNLowG, lightDir), dot(wNLowB, lightDir));
	diffLdn = diffLdn * 0.5 + 0.5;
	half3 blurDiff;
	blurDiff.r = tex2D(_SkinLut, half4(diffLdn.r, curvture, 0, 0)).r;
	blurDiff.g = tex2D(_SkinLut, half4(diffLdn.g, curvture, 0, 0)).g;
	blurDiff.b = tex2D(_SkinLut, half4(diffLdn.b, curvture, 0, 0)).b;
	diffuseTerm = blurDiff;
#else
	diffuseTerm = tex2Dlod(_SkinLut, half4(dot(lightDir, normal)*0.5 + 0.5, curvture, 0, 0));
#endif
#endif
	// Skin End

	// Fuzz
#if defined (FUZZ)
	half mircor = saturate(pow((1.0 - nv), 10.0 * _FuzzScatter));
	specularColor += GIspecular*_FuzzColor.rgb * mircor * _Fuzz * 5.0f * nl;
#endif
	// Fuzz End

	// Scatter
#if defined (SCATTER)
	half scatterAmount = 0;

#if defined (HAIR)	
	half scatterFresnel = pow( saturate(1-  abs(dot(i.tangentToWorldAndPackedData[2].xyz, viewDir))), _ScatteringSPower);
	half3 scatterDir = lightDir + normal * _ScatterDistortion;
	half scatterLight = pow(saturate(dot(viewDir, -scatterDir)), _ScatteringSPower) * (1 - nv) * (1 - nl);
	scatterAmount = scatterFresnel + _ScatteringScale * scatterLight;
#endif

#if defined (SKIN)
	half3 scatterDir = lightDir + normal * _ScatterDistortion;
	scatterAmount = pow(saturate(dot(viewDir, -scatterDir)), _ScatteringSPower) * _ScatteringScale;
#endif
	diffuseTerm += scatterAmount * _ScatterColor.rgb;
#endif
	// Scatter End
    
	// Final Color
	half3 color = diffColor * (GIdiffuse + mainLight * diffuseTerm);
	color += specularColor * mainLight;

#if defined (GI)
	color += surfaceReduction * GIspecular * fresnelLerp;
#endif

	
	// Saturation & Gradation
	color = lerp(Luminance(color), color, _Saturation);
	//color = saturate(lerp(half3(0.5, 0.5, 0.5), color, _Contrast));
	color *= i.eyeVec.w;

	
    half alpha = albedo.a;
	
#if defined (HAIR)  
	half2 screenPixel = (i.screenUV.xy / i.screenUV.w)* _ScreenParams.xy;
	/*
#ifdef _DITHERMODE_DITHER64
    albedo.a = ScreenDitherToAlpha(screenPixel.x, screenPixel.y,  albedo.a);
#elif _DITHERMODE_DITHER16
	albedo.a = ScreenDitherToAlphaSimple(screenPixel.x, screenPixel.y, albedo.a);
#endif
	*/
    alpha = albedo.a;
#endif


	
#if defined (EYE)
	alpha = Luminance(specularColor * mainLight);
#endif

#if defined (TRANSPARENT)
    alpha = saturate (alpha * _Transparent);
#endif

#if defined (ALPHABLEND) 
    color *= alpha;
#endif

#if defined (CLIP)
    clip(alpha - _Cutoff);
#endif
    

	return fixed4(color.rgb, alpha);
}

#endif