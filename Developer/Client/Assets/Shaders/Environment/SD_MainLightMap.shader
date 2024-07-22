Shader "SD/Environment/SD_MainLightMap" 
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
		LOD 100  Cull Back
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
				float2 uv		: TEXCOORD0;
				UNITY_FOG_COORDS(1)
#if USE_LIGHTMAP
				float2 lightUV	: TEXCOORD2;
#endif
#if USE_HEIGHTFOG
				float3 worldPos		: TEXCOORD3;
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
				float _AmbientStrength;
				fixed _CurveFactor;
				
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
				o.uv = TRANSFORM_TEX(i.texcoord, _MainTex);
#if USE_LIGHTMAP
				o.lightUV = TRANSFORM_TEX(i.lightUV, _LightMap);
#endif
				CURVED_WORLD_TRANSFORM_POINT(i.vertex,_CurveFactor);
				o.pos = UnityObjectToClipPos(i.vertex);
#if USE_HEIGHTFOG
				o.worldPos = mul(UNITY_MATRIX_M, i.vertex).xyz;
#endif			
				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}

			float4 frag(v2f i) :COLOR
			{
				float4 col = SAMPLE_TEXTURE2D_DEF(_MainTex,i.uv) * _MainColor;

				Util_AmbientColor(col, _AmbientStrength);
#if USE_LIGHTMAP
				float4 lightCol = SAMPLE_TEXTURE2D_DEF(_LightMap, i.lightUV)*_LightMapColor;
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
				UNITY_APPLY_FOG(i.fogCoord, final);
#if USE_HEIGHTFOG				
				real4 heighFog = lerp( _HeightFogColor , final , saturate( ( ( i.worldPos.y + _HeightFogHigh ) / _HeightFogSmooth ) ));
				final = lerp( final , heighFog , _HeightFogStrength);
#endif				
				return  final;
			}
			ENDHLSL
		}
	}
	FallBack Off
}