Shader "SD/URP/Actor/ActorPBR"
{
	Properties
	{
		_AO_slider("AO_slider", Float) = 1
		[MainTexture]_BaseMap("DiffuseTex", 2D) = "white" {}
		[HDR]_BaseColor("Base Color", Color) = (1,1,1,1)
		_Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

		//_LUTTex("Ramp Texture", 2D) = "white"{}
		//[MaterialToggle(USE_SGSSS)]USE_SGSSS("Use SGSSS", Int) = 1
		_ScatterAmtX("SkinScatter R", Range(0,1)) = 0.5
		_ScatterAmtY("SkinScatter G", Range(0,1)) = 0.5
		_ScatterAmtZ("SkinScatter B", Range(0,1)) = 0.5
		_SkinStrength("Skin Strength", Range(0,5)) = 1
		_SkinToneMap("Skin Tone Maping", Range(0,2)) = 0.8
		

		//[KeywordEnum(DISNEY_DIFFUSE,  O_N_DIFFUSE, GOTANDA_DIFFUSE, LAMBERT_DIFFUSE)]USE("DiffuseMode", Float) = 3

		[Space(15)][Header(Metallic Roughness Mask Map Properties)]
		_MetalRoughMaskTex("MetalRoughMaskTex", 2D) = "white" {}
		_Metallic("Metallic", Range(0,1)) = 1.0
		_Roughness("Roughness", Range(0,1)) = 1.0

		[MaterialToggle(USE_NORMAL_MAP)]NORMALMAP("Is Use NormalMap", Int) = 0
		_NormalAOTex("NormalRG B:AO", 2D) = "bump" {}
		_AOStrength("Occlusion", Range(0,1)) = 0.5

		//[KeywordEnum(CUBE,  MATCAP, UNITY_CUBE)]USE_ENV("EnvMode", Float) = 2

		[Space(15)][Header(Emission Map Properties)]
		[MaterialToggle(USE_EMISSION_MAP)]EMISSIONMAP("Is Use Emission Map", Int) = 0
		_EmissionTex("Emission Map", 2D) = "black" {}
		[HDR]_EmissionColor("Emission Color", Color) = (1,1,1,1)
		

		[Header(Outline)]
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Outline("Outline Width", Range(0, 1)) = 1
		_FarCull("FarCull",Float) = 0
		
		_CurveFactor("CurveFactor", Float) = 1

		[Header(Dissolve)]
		[MaterialToggle(USE_DISSOLVE)]_UseDissolve("Use Dissolve", Int) = 0
		_DissolveAmount("_DissolveAmount", Range(0, 1)) = 0.1
		_DissolveNoise("Dissolve Noise", 2D) = "white"{}
		_DissolveEdgeWidth("Dissolve Edge Wdith",  Range(0.001, 1)) = 0
		_DissolveHeightSpeed("Dissolve Height Factor(为0高度不生效)", Float) = 0
		_DissolveHeight("Dissolve Height",  Range(-20, 20)) = 0
		[HDR]_DissolveEdgeColor("Dissolve Edge Color", Color) = (0.0, 0.0, 0.0, 0.5)
		_DissolveSmoothness("Dissolve Smoothness", Range(0.001, 1)) = 0.2
		_DissolveUVSwitch("DissolveUV Switch", Int) = 0
		
		//[MaterialToggle(USE_DIRLIGHT_LIGHT)]_UseDirLight("Use Real Dir Light", Int) = 0
		//_ShdowLight("_Shadow Light",vector) = (70,90,70)
		//[MaterialToggle]_ShadowEffect("Shadow state",Float) = 1
		//_ShadowColor("Shadow Color",Color) = (0.14,0.14,0.14,1)
		//_ShadowOffsetX("ShadowOffsetX",Float) = 0
		//_ShadowOffsetZ("ShadowOffsetZ",Float) = 0
		//_ShadowFalloff("ShadowFalloff",Float) = 0.1

		[MaterialToggle(_MAIN_LIGHT_SHADOWS)]MAIN_LIGHT_SHADOWS("MAIN_LIGHT_SHADOWS", Int) = 1
		[MaterialToggle(_SHADOWS_SOFT)]SHADOWS_SOFT("SHADOWS_SOFT", Int) = 1

		// Blending state
		[HideInInspector] _Surface("__surface", Float) = 0.0
		[HideInInspector] _Blend("__blend", Float) = 0.0
		[HideInInspector] _AlphaClip("__clip", Float) = 0.0
		[HideInInspector] _SrcBlend("__src", Float) = 1.0
		[HideInInspector] _DstBlend("__dst", Float) = 0.0

		[HideInInspector] _ZWrite("__zw", Float) = 1.0
		[HideInInspector] _Cull("__cull", Float) = 2.0
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"}
		LOD 300

		Pass
		{
			Name "ForwardLit"
			Tags{"LightMode" = "UniversalForward"}

			HLSLPROGRAM
			#pragma vertex		ActorVert
			#pragma fragment	ActorFrag

			#define USE_SGSSS
			//#define USE_LUT
			#define USE_ANISOTROPIC
			#define USE_LAMBERT_DIFFUSE
			#define USE_ENV_CUBE

//			#pragma multi_compile USE_DISNEY_DIFFUSE
//			#pragma multi_compile USE_O_N_DIFFUSE 
//			#pragma multi_compile USE_GOTANDA_DIFFUSE
//			#pragma multi_compile USE_GOTANDA_DIFFUSE
//			#pragma multi_compile USE_LAMBERT_DIFFUSE
//			#pragma shader_feature __ USE_SGSSS

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma shader_feature _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma shader_feature _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma shader_feature _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _SHADOWS_SOFT
			//#pragma shader_feature _ _MIXED_LIGHTING_SUBTRACTIVE
			
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			#pragma shader_feature __ USE_HEIGHT_MAP
			#pragma multi_compile __ USE_NORMAL_MAP
			#pragma multi_compile __ USE_EMISSION_MAP
			#pragma shader_feature __ USE_DISSOLVE

			//#pragma multi_compile USE_ENV_CUBE
			//#pragma multi_compile USE_ENV_CUBE2D
			//#pragma multi_compile USE_ENV_UNITY_CUBE

			#include "../Includes/BaseDefine/CommonDefine.hlsl"
			#include "../Includes/BaseDefine/VertexBase.hlsl"
			#include "../Includes/Pass/PBR/PBRPass.hlsl"

			ENDHLSL
		}

        Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ZTest LEqual
            Cull[_Cull]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma target 2.0

			// -------------------------------------
			// Material Keywords
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment
			#include "../Includes/Pass/PBR/PBRBuffer.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }

	//	Pass
    //   {
    //       Name "DepthOnly"
    //       Tags{"LightMode" = "DepthOnly"}
	//
    //       ZWrite On
    //       ColorMask 0
    //       Cull[_Cull]
	//
    //       HLSLPROGRAM
    //       // Required to compile gles 2.0 with standard srp library
    //       #pragma prefer_hlslcc gles
    //       #pragma target 2.0
	//
    //       #pragma vertex DepthOnlyVertex
    //       #pragma fragment DepthOnlyFragment
	//	
	//		// -------------------------------------
	//		// Material Keywords
	//		#pragma shader_feature _ALPHATEST_ON
	//		#pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
	//
    //       //--------------------------------------
    //       // GPU Instancing
    //       #pragma multi_compile_instancing
	//
    //       #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
    //       #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
    //       ENDHLSL
    //   }
		Pass
		{
			Name "Outline"
			Tags{ "LightMode" = "Outline" }
			Cull Front
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile __ USE_DISSOLVE
			#pragma multi_compile_fog
			#include "../Includes/Pass/PBR/PBRBuffer.hlsl"
			#include "Assets/Shaders/Includes/Outline.hlsl"
			ENDHLSL
		}
		//Pass
		//{
		//	Name "MESHSHADOW"
		//	Tags{ "LightMode" = "PlaneShadow" }
		//	Blend SrcAlpha OneMinusSrcAlpha
		//	ZWrite Off
		//	Cull Back
		//	ColorMask RGB
		//	Stencil
		//	{
		//		Ref 0
		//		Comp Equal
		//		WriteMask 255
		//		ReadMask 255
		//		//Pass IncrSat
		//		Pass Invert
		//		Fail Keep
		//		ZFail Keep
		//	}
		//	HLSLPROGRAM
		//	#pragma vertex shadow_vert
		//	#pragma fragment shadow_frag 
		//	#pragma fragmentoption ARB_precision_hint_fastest
		//  #include "../Includes/Pass/PBR/PBRBuffer.hlsl"
		//	#include "Assets/Shaders/Includes/MeshShadow.hlsl"
		//	ENDHLSL
		//}		
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"}
		LOD 100

		Pass
		{
			Name "ForwardLit"
			Tags{"LightMode" = "UniversalForward"}

			HLSLPROGRAM
			#pragma vertex		ActorVert
			#pragma fragment	ActorFrag

			#define USE_LAMBERT_DIFFUSE
			
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			#pragma multi_compile __ USE_NORMAL_MAP
			#pragma multi_compile __ USE_EMISSION_MAP
			#pragma shader_feature __ USE_DISSOLVE

			#include "../Includes/BaseDefine/CommonDefine.hlsl"
			#include "../Includes/BaseDefine/VertexBase.hlsl"
			#include "../Includes/Pass/PBR/PBRPassLOD100.hlsl"

			ENDHLSL
		}	
	}	
}