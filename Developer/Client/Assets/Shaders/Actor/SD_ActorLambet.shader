Shader "SD/Actor/SD_ActorLambet"
{
	Properties
	{
		//主颜色
		[HDR]_MainColor("Main Color", Color) = (1.0, 1.0, 1.0, 1.0)
		[HideInInspector]_ProjectorColor("Projector Color", Color) = (1.0, 1.0, 1.0, 1.0)

		//漫反射纹理
		[Header(Diffuse)]
		_DiffuseTex("Diffuse Textrue", 2D) = "white" {}
	
		[Header(Normal)]
		_BumpMap("Normal Tex", 2D) = "bump" {}
		_BumpValue("Normal Value", Range(0,10)) = 1

		[Header(Emission)]
		_EmissionStrength("EmissionStrength",Range(0,10)) = 0
		_EmissionMap("Emission", 2D) = "black" {}

		[Header(Rim)]
		_RimColor("RimColor",Color) = (1,1,1,1)
		_RimFactor("RimFactor",Range(0,100)) = 0
		_RimPower("Rim Power", Range(0.1, 36)) = 17
		
		//_Cubemap("Reflection Cubemap",Cube)="_Skybox"{}
		
		[Header(Fresnel)]
		[MaterialToggle(USE_FRESNEL)]_UseFresnel("Use Fresnel",Int) = 0
		[HDR]_FresnelColor("Fresnel Color",Color)=(1,1,1,1)
		_FresnelRange("Fresnel Range",Range(0,10))=5
		_FresnelScale("Fresnel Scale",Range(0,1))=0.5

		[MaterialToggle(USE_DIRLIGHT_LIGHT)]_UseDirLight("Use Real Dir Light", Int) = 0
		_ShdowLight("_Shadow Light",vector) = (70,90,70)
		[MaterialToggle]_ShadowEffect("Shadow state",Float) = 1
		_ShadowColor("Shadow Color",Color) = (0.14,0.14,0.14,1)
		_ShadowOffsetX("ShadowOffsetX",Float) = 0
		_ShadowOffsetZ("ShadowOffsetZ",Float) = 0
		_ShadowFalloff("ShadowFalloff",Float) = 0.1
		_GroundHeight("GroundHeight", Float) = 0

		_Lambet("Lambet", Float) = 0.5
		[HideInInspector]_CurveFactor("CurveFactor", Float) = 1

		[Header(Dissolve)]
		[MaterialToggle(USE_DISSOLVE)]_UseDissolve("Use Dissolve", Int) = 0
		_DissolveAmount("_DissolveAmount", Range(0, 1)) = 0.1
		_DissolveNoise("Dissolve Noise", 2D) = "white"{}
		_DissolveEdgeWidth("Dissolve Edge Wdith",  Range(0.001, 1)) = 0
		_DissolveHeightSpeed("Dissolve Height Factor(为0高度不生效)", Float) = 0
		_DissolveHeight("Dissolve Height",  Range(-20, 20)) = 0
		[HDR]_DissolveEdgeColor("Dissolve Edge Color", Color) = (0.0, 0.0, 0.0, 0.5)
		_DissolveSmoothness("Dissolve Smoothness", Range(0.001, 1)) = 0.2
		
		[Header(Outline)]
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Outline("Outline Width", Range(0, 1)) = 1
		_FarCull("FarCull",Float) = 80		

		[HideInInspector]_Alpha("Alpha", Range(0, 1)) = 1
	}
	SubShader
	{
		LOD 200
		Tags{ "IgnoreProjector" = "True""Queue" = "Geometry+200""RenderType" = "Opaque" }
		Pass
		{
			Tags{ "LightMode" = "UniversalForward" }
			Blend Off
			Cull Back

			HLSLPROGRAM
			#include "../Includes/Pass/Lambet/ActorLambetBuffer.hlsl"
			#include "../Includes/Pass/Lambet/ActorLambet.hlsl"
			#pragma shader_feature __ USE_DISSOLVE
			#pragma shader_feature __ USE_FRESNEL
			#pragma multi_compile_fog
			#pragma fragment frag
			#pragma vertex vert

			//------------------------------------------------------------
			// 顶点着色器
			//------------------------------------------------------------
			VertexToFragment vert(FragmentToVertex v)
			{
				return LambetVS(v);
			}
			//------------------------------------------------------------
			// 片元着色器
			//------------------------------------------------------------
			float4 frag(VertexToFragment input) : COLOR
			{
				return LambetFS(input);
			}
			ENDHLSL
		}
		Pass
		{
			Name "MESHSHADOW"
			Tags{ "LightMode" = "PlaneShadow" }
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Back
			ColorMask RGB
			Stencil
			{
				Ref 0
				Comp Equal
				WriteMask 255
				ReadMask 255
				//Pass IncrSat
				Pass Invert
				Fail Keep
				ZFail Keep
			}
			HLSLPROGRAM
			#pragma vertex shadow_vert
			#pragma fragment shadow_frag 
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "../Includes/Pass/Lambet/ActorLambetBuffer.hlsl"
			#include "Assets/Shaders/Includes/MeshShadow.hlsl"
			ENDHLSL
		}
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
			#include "../Includes/Pass/Lambet/ActorLambetBuffer.hlsl"
			#include "Assets/Shaders/Includes/Outline.hlsl"
			ENDHLSL
		}		
	}
	SubShader
	{
		LOD 100
		Tags{ "IgnoreProjector" = "True""Queue" = "Geometry+200""RenderType" = "Opaque" }
		Pass
		{
			Tags{ "LightMode" = "UniversalForward" }
			Blend Off
			Cull Back

			HLSLPROGRAM
			#include "../Includes/Pass/Lambet/ActorLambetBuffer.hlsl"
			#include "../Includes/Pass/Lambet/ActorLambet.hlsl"
			#pragma shader_feature __ USE_DISSOLVE
			#pragma shader_feature __ USE_FRESNEL
			#pragma multi_compile_fog
			#pragma fragment frag
			#pragma vertex vert

			//------------------------------------------------------------
			// 顶点着色器
			//------------------------------------------------------------
			VertexToFragment vert(FragmentToVertex v)
			{
				return LambetVS(v);
			}
			//------------------------------------------------------------
			// 片元着色器
			//------------------------------------------------------------
			float4 frag(VertexToFragment input) : COLOR
			{
				return LambetFS(input);
			}
			ENDHLSL
		}
	}
	FallBack Off
}
