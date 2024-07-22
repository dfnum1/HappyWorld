Shader "SD/Role/SD_CurvedRole" 
{
	Properties
	{
		[HDR]_Color("Color",Color)=(1,1,1,1)
		_AmbientStrength("Ambient Strength",Range(0,2)) = 0
		_MainTex ("Texture", 2D) = "white" {}
		
		[MaterialToggle(USE_DIRLIGHT_LIGHT)]_UseDirLight("Use Real Dir Light", Int) = 0
		_ShdowLight("_Shadow Light",vector) = (70,90,70)
		[MaterialToggle]_ShadowEffect("Shadow state",Float) = 1
		_ShadowColor("Shadow Color",Color) = (0.14,0.14,0.14,1)
		_ShadowOffsetX("ShadowOffsetX",Float) = 0
		_ShadowOffsetZ("ShadowOffsetZ",Float) = 0
		_ShadowFalloff("ShadowFalloff",Float) = 0.1
		_GroundHeight("GroundHeight", Float) = 0
	
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
		
		[HideInInspector]_CurveFactor("CurveFactor", Float) = 1
		[HideInInspector]_Alpha("Alpha", Range(0,1)) = 1
	}
	SubShader
	{
	//	LOD 100
		Tags{"IgnoreProjector" = "True""Queue" = "Geometry+200""RenderType" = "Opaque"}
		Pass
		{
			Name "FORWARD"
		//	Tags{"LightMode" = "ForwardBase"}
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma shader_feature __ USE_DISSOLVE
			
			#include "./CurvedRoleBuffer.hlsl"
			#include "Assets/Shaders/Includes/WorldCurvedCG.hlsl"
			
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 color : TEXCOORD2;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				CURVED_WORLD_TRANSFORM_POINT(v.vertex,_CurveFactor);
				o.uv = v.uv;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color*_Color;
				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv) * i.color;
				Util_AmbientColor(col, _AmbientStrength);
				#if USE_DISSOLVE
					fixed dissove = tex2D(_DissolveNoise, input.dissolve.xy).r;
					dissove = dissove + (1 - _DissolveAmount);
					float dissolve_alpha = 1 - input.dissolve.z*dissove;
					clip(dissolve_alpha);
					clip(dissove - 1);
					float edge_area = saturate(1 - saturate((dissove - 1 + _DissolveEdgeWidth) / _DissolveSmoothness));
					edge_area *= _DissolveEdgeColor.a*saturate(_DissolveAmount);
					col.rgb = lerp(col.rgb, _DissolveEdgeColor.rgb * 10, edge_area);
				#endif				
				UNITY_APPLY_FOG(i.fogCoord, col);
				col.a = _Alpha;
				return col;
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
			#include "./CurvedRoleBuffer.hlsl"
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
			#include "./CurvedRoleBuffer.hlsl"
			#include "Assets/Shaders/Includes/Outline.hlsl"
			ENDHLSL
		}	
	}	
	FallBack Off
}
