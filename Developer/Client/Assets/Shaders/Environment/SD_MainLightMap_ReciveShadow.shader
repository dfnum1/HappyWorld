﻿Shader "SD/Environment/SD_MainLightMap_ReciveShadow" 
{
	Properties 
	{
		_AmbientStrength("Ambient Strength",Range(0,2)) = 1
		[HDR]_MainColor("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_LightMap ("Lightmap (RGB)", 2D) = "black" {}
		_LightMapColor("LightMap Color",Color) = (1,1,1,1)
		_MidLight("Lightmap Strength",Range(0,10)) = 1
		[MaterialToggle(USE_LIGHTMAP)] _USE_LIGHTMAP("IsUseLightMap", Float) = 1.0
		[MaterialToggle(USE_VARY)] _USE_VARY("Use Vary", Float) = 1.0
		_CurveFactor("CurveFactor", Float) = 1
		[MaterialToggle]_FogToggle("EnableFog", Float) = 1
		
		[Space(15)][Header(Emission Map Properties)]
		[MaterialToggle(USE_EMISSION_MAP)]EMISSIONMAP("Is Use Emission Map", Int) = 0
		[HDR]_EmissionColor("Emission Color", Color) = (1,1,1,1)
		_EmissionMap("Emission", 2D) = "black" {}
		
		_ShadowColor ("Shadow Color", Color) = (0.15,0.15,0.15,0.35)
		
		[Header(Height FOG)]
		[MaterialToggle(USE_HEIGHTFOG)] _USE_HEIGHTFOG("IsUseHeightFog", Int) = 0
		_HeightFogStrength("FogStrength", Range(0,1)) = 0
		_HeightFogColor("FogColor", Color) = (0.5821022,0.9716981,0.9316334,0)
		_HeightFogHigh("FogHigh", Float) = 5
		_HeightFogSmooth("FogSmooth", Range( 0.01 , 100)) = 0.5	
		
	}
	SubShader
	{
		Tags 
		{
			"Queue" = "Geometry+10" 
			"RenderType" = "Opaque" 
			"IgnoreProjector" = "False" 
		}
		LOD 300  Cull Back
		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest

			#pragma multi_compile_instancing
			#pragma multi_compile __ FOG_LINEAR
			#pragma multi_compile __ USE_LIGHTMAP
			#pragma multi_compile __ USE_HEIGHTFOG		
			#pragma shader_feature __ USE_EMISSION_MAP
		//	#pragma shader_feature __ USE_VARY
		
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT			

			#include "../Includes/WorldCurvedCG.hlsl"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
#if USE_LIGHTMAP
				float2 lightUV	: TEXCOORD1;
#endif
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 pos		: SV_POSITION;
				float3 positionWS     : TEXCOORD0;
				float3 uvFogCoord		: TEXCOORD1;
#if USE_LIGHTMAP
				float2 lightUV	: TEXCOORD2;
#endif
				UNITY_VERTEX_OUTPUT_STEREO
			};

			TEXTURE2D_DEF(_MainTex);
			TEXTURE2D_DEF(_LightMap);
			TEXTURE2D_DEF(_EmissionMap);

			CBUFFER_START(UnityPerMaterial)
				float4	  _MainTex_ST;
				float4	  _LightMap_ST;

				float4 _MainColor;
				float _MidLight;
				fixed4 _LightMapColor;

				fixed _FogToggle;
				fixed _AmbientStrength, _CurveFactor;
				
				float4 _ShadowColor;
				
				uniform real4 _EmissionColor;

			#if USE_HEIGHTFOG
				uniform real4 _HeightFogColor;
				uniform real _HeightFogHigh;
				uniform real _HeightFogSmooth;
				uniform real _HeightFogStrength;
			#endif

			CBUFFER_END
			
			
			v2f vert(appdata i)
			{
				v2f o = (v2f)0;
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.uvFogCoord.xy = TRANSFORM_TEX(i.texcoord, _MainTex);
#if USE_LIGHTMAP
				o.lightUV = TRANSFORM_TEX(i.lightUV, _LightMap);
#endif
				CURVED_WORLD_TRANSFORM_POINT(i.vertex,_CurveFactor);
				VertexPositionInputs vertexInput = GetVertexPositionInputs(i.vertex.xyz);
				o.pos = vertexInput.positionCS;
                o.positionWS = vertexInput.positionWS;	
				o.uvFogCoord.z = ComputeFogFactor(vertexInput.positionCS.z);
				return o;
			}

			float4 frag(v2f i) :COLOR
			{
				float4 col = SAMPLE_TEXTURE2D_DEF(_MainTex,i.uvFogCoord.xy) * _MainColor;

				Util_AmbientColor(col, _AmbientStrength);
				float4 lightCol = float4(0,0,0,0);
#if USE_LIGHTMAP
				lightCol = SAMPLE_TEXTURE2D_DEF(_LightMap, i.lightUV)*_LightMapColor;
	#if USE_VARY				
				lightCol.rgb = lightCol.rgb * 2.0 - 1.0;
				float4 final = col + lightCol * _MidLight;
	#else
				float4 final = col * lightCol * _MidLight;
				final.a = lightCol.a * _MainColor.a;
	#endif
#else
				float4 final = col * _MidLight;
				final.a = _MainColor.a;
#endif
			//dither
			//	float4x4 _RowAccess = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
			//	float2 pos = (i.screenPos.xy / i.screenPos.w) * _ScreenParams.xy;
			//	clip(0.9 - _RowAccess[floor(fmod(pos.x, 4))][floor(fmod(pos.y, 4))]);
			
#if USE_EMISSION_MAP
				final.rbg += SAMPLE_TEXTURE2D_DEF(_EmissionMap, i.uv).rgb * _EmissionColor.rgb;
#endif		
            #ifdef _MAIN_LIGHT_SHADOWS
				
                float4 shadowCoord = TransformWorldToShadowCoord(i.positionWS);
                half shadowAttenutation = MainLightRealtimeShadow(shadowCoord);
				
				float argvLight = _ShadowColor.a * clamp((lightCol.r + lightCol.g + lightCol.b),0,1);
                final = lerp(final, _ShadowColor, (1.0 - shadowAttenutation) * argvLight);
				UNITY_APPLY_FOG(i.uvFogCoord.z, final);
			#else
				UNITY_APPLY_FOG(i.uvFogCoord.z, final);
            #endif
			
#if USE_HEIGHTFOG				
				real4 heighFog = lerp( _HeightFogColor , final , saturate( ( ( i.positionWS.y + _HeightFogHigh ) / _HeightFogSmooth ) ));
				final = lerp( final , heighFog , _HeightFogStrength);
#endif					
				return  final;
			}
			ENDHLSL
		}
	}
	FallBack Off
}