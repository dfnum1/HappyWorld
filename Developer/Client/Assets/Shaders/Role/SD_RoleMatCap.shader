Shader "SD/Role/SD_RoleMatCap"
{
	Properties
	{
		//主颜色
		[HDR]_MainColor("Main Color", Color) = (1.0, 1.0, 1.0, 1.0)
		[HideInInspector]_ProjectorColor("Projector Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_AmbientStrength("Ambient Strength",Range(0,2)) = 0

		//漫反射纹理
		_DiffuseTex("Diffuse Textrue", 2D) = "white" {}

		_BumpMap("Normal Tex", 2D) = "bump" {}
		_BumpValue("Normal Value", Range(-10,10)) = 1

		//Material Capture纹理
		_MatCap("MatCap", 2D) = "white" {}
		_MatCapStrength("MatCap Strength", Range(1, 10.0)) = 1

		[HideInInspector]_MetallicGlossMap("Metallic",2D) = "white"{} //金属图，r通道存储金属度，a通道存储光滑度
		_RoughnessMap("Roughness",2D) = "white"{}
		[HideInInspector]_MetallicStrength("MetallicStrength",Range(0,1)) = 1 //金属强度
		_RoughnessStrength("RoughnessStrength",Range(0,1)) = 1 //光滑强度

		_AOStrength("Ambient Occlusion Strength",Range(0,10)) = 1 //光滑强度
		_AOMap("Ambient Occlusion Map (RGB)", 2D) = "white" {}

		_EmissionStrength("EmissionStrength",Range(0,10)) = 0
		_EmissionMap("Emission", 2D) = "black" {}


		[HDR]_RimColor("RimColor",Color) = (1,1,1,1)
		_RimFactor("RimFactor",Range(0,1)) = 0
		_RimEdgePower("Rim Power", Range(0.01, 1)) = 0.6

		[HideInInspector][Toggle]_UseDirLight("Use Real Dir Light", Int) = 0
		[HideInInspector][Toggle]_UseCameraDir("Use CameraDir Replace Light Dir", Int) = 0
	//	_FresnelColor("Fresnel Color", Color) = (1.0, 1.0, 1.0, 1.0)


		[Toggle]_UseDirLight("Use Real Dir Light", Int) = 0
		_ShdowLight("_Shadow Light",vector) = (70,90,70)
		[Toggle]_ShadowEffect("Shadow state",Float) = 1
		_ShadowColor("Shadow Color",Color) = (0.14,0.14,0.14,1)
		_ShadowOffsetX("ShadowOffsetX",Float) = 0
		_ShadowOffsetZ("ShadowOffsetZ",Float) = 0
		_ShadowFalloff("ShadowFalloff",Float) = 0.1
		_GroundHeight("GroundHeight", Float) = 0

		[HideInInspector]_CurveFactor("CurveFactor", Float) = 1
		[HideInInspector]_Alpha("_Alpha", Float) = 1

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
	}
	SubShader
	{
	//	LOD 400
		Tags{ "IgnoreProjector" = "True""Queue" = "Geometry+200""RenderType" = "Opaque" }
		Pass
		{
			Tags{ "LightMode" = "UniversalForward" }
			Blend Off
			Cull Back

			HLSLPROGRAM
			#include "./MatCapBuffer.hlsl"
			#include "./MatCap.hlsl"
			#pragma multi_compile __ USE_DISSOLVE
			#pragma multi_compile_fog
			#pragma fragment frag
			#pragma vertex vert

			//------------------------------------------------------------
			// 顶点着色器
			//------------------------------------------------------------
			VertexToFragment vert(appdata_tan v)
			{
				return MatCapVS(v);
			}
			//------------------------------------------------------------
			// 片元着色器
			//------------------------------------------------------------
			float4 frag(VertexToFragment input) : COLOR
			{
				return MatCapFS(input);
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
			#include "./MatCapBuffer.hlsl"
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
			#include "./MatCapBuffer.hlsl"
			#include "Assets/Shaders/Includes/Outline.hlsl"
			ENDHLSL
		}			
	}
}