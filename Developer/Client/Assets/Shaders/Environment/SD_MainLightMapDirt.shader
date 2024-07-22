Shader "SD/Environment/SD_MainLightMapDirt" 
{
	Properties 
	{
		[HDR]_MainColor("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		[NoScaleOffset]DirtTexture("Dirt", 2D) = "white" {}
		 _Tiling("Tiling", Float) = 0.2
		 _DirtDirection("Dirt Direction", Vector) = (0, 1, 0, 0)
		 _SnowLevel("SnowLevel", Float) = 30
		_LightMap ("Lightmap (RGB)", 2D) = "black" {}
		_LightMapColor("LightMap Color",Color) = (1,1,1,1)
		_MidLight("Lightmap Strength",Range(0,10)) = 1
		[MaterialToggle(USE_LIGHTMAP)] _USE_LIGHTMAP("IsUseLightMap", Float) = 1.0
		[MaterialToggle(USE_VARY)] _USE_VARY("Use Vary", Float) = 1.0
		_CurveFactor("CurveFactor", Float) = 1
		[MaterialToggle]_FogToggle("EnableFog", Float) = 1
		
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
		LOD 100 
		Cull Back
		Lighting Off
        //ZWrite Off
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
		//	#pragma shader_feature __ USE_VARY

			#include "../Includes/WorldCurvedCG.hlsl"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
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
				float3 WorldSpacePosition : TEXCOORD3;
				float3 WorldSpaceNormal : TEXCOORD4;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			TEXTURE2D_DEF(_MainTex);
			TEXTURE2D_DEF(_LightMap);
			TEXTURE2D(DirtTexture); SAMPLER(samplerDirtTexture); float4 DirtTexture_TexelSize;

			CBUFFER_START(UnityPerMaterial)
				float4	  _MainTex_ST;
				float4	  _LightMap_ST;

				float4 _MainColor;
				float _MidLight;
				fixed4 _LightMapColor;

				fixed _FogToggle;
				
				fixed _Tiling;
				fixed _SnowLevel;
				float4 _DirtDirection;
				fixed _CurveFactor;
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
				
				float3 normalWS = UnityObjectToWorldNormal(i.normal);
				float3 unnormalizedNormalWS = normalWS;
                const float renormFactor = 1.0 / length(unnormalizedNormalWS);
				
				o.WorldSpaceNormal = renormFactor*normalWS; 
				o.WorldSpacePosition = mul(UNITY_MATRIX_M, i.vertex).xyz;
				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}

			float4 frag(v2f i) :COLOR
			{
				float4 col = SAMPLE_TEXTURE2D_DEF(_MainTex,i.uv) * _MainColor;

				float3 AbsoluteWorldSpacePosition = GetAbsolutePositionWS(i.WorldSpacePosition);
				float3 Triplanar_UV = AbsoluteWorldSpacePosition * _Tiling;
				float3 Triplanar_Blend = pow(abs(i.WorldSpaceNormal), 1);
				Triplanar_Blend /= dot(Triplanar_Blend, 1.0);
				float4 Triplanar_X = SAMPLE_TEXTURE2D(DirtTexture, samplerDirtTexture, Triplanar_UV.zy);
                float4 Triplanar_Y = SAMPLE_TEXTURE2D(DirtTexture, samplerDirtTexture, Triplanar_UV.xz);
                float4 Triplanar_Z = SAMPLE_TEXTURE2D(DirtTexture, samplerDirtTexture, Triplanar_UV.xy);
                float4 _Triplanar_Out_0 = Triplanar_X * Triplanar_Blend.x 
												+ Triplanar_Y * Triplanar_Blend.y 
												+ Triplanar_Z * Triplanar_Blend.z;
												
				float _DotProduct_Out = saturate(dot(i.WorldSpaceNormal, _DirtDirection.xyz));
				
				float _Multiply_Out = _SnowLevel*1.5;
				float2 _Vector2_Out = float2(-50, _Multiply_Out);
				
				float _Remap_Out;
				Unity_Remap_float(_DotProduct_Out, float2 (-1, 1), _Vector2_Out, _Remap_Out);
				
				float _Clamp_Out;
                Unity_Clamp_float(_Remap_Out, 0, 1, _Clamp_Out);
				
				float4 final_lerp;
				Unity_Lerp_float4(col, _Triplanar_Out_0, _Clamp_Out.xxxx, final_lerp);
#if USE_LIGHTMAP
				float4 lightCol = SAMPLE_TEXTURE2D_DEF(_LightMap, i.lightUV)*_LightMapColor;
	#if USE_VARY				
				lightCol.rgb = lightCol.rgb * 2.0 - 1.0;
				float4 final = final_lerp + lightCol * _MidLight;
	#else
				float4 final = final_lerp * lightCol * _MidLight;
				final.a = lightCol.a * _MainColor.a;
	#endif
#else
				float4 final = final_lerp * _MidLight;
				final.a = _MainColor.a;
#endif				
				UNITY_APPLY_FOG(i.fogCoord, final);
#if USE_HEIGHTFOG				
				real4 heighFog = lerp( _HeightFogColor , final , saturate( ( ( i.WorldSpacePosition.y + _HeightFogHigh ) / _HeightFogSmooth ) ));
				final = lerp( final , heighFog , _HeightFogStrength);
#endif						
				return  final;
			}
			ENDHLSL
		}
	}
	FallBack Off
}