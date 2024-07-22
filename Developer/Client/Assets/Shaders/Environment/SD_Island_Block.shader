Shader "SD/Environment/SD_IslandBlock"
{
	Properties
	{
		_MainTexture("Main", 2D) = "black" {}
		[HDR]_MainColor("Main Color", Color) = (1,1,1,1)
		_AO("AO", 2D) = "white" {}
		_AOStrength("AOStrength", Range( 0 , 10)) = 3
		
		[Header(LightMap)]
		_LightMap("LightMap", 2D) = "white" {}
		_LightMapStrength("LightMapStrength", Range( 0.01 , 1)) = 1
		
		[Header(Light)]
		_LightColor("Base Light Color", Color) = (1,1,1,0)
		_AddLightColor("Add Light Color", Color) = (0.07196257,0.0754717,0.0509078,0)
		_AddLightPower("Add Light Power", Range( 0.01 , 2)) = 2	
		_AddLight("Add Light", Color) = (1,1,1,1)
		_ShadowColor("ShadowColor", Color) = (0.4705882,0.4705882,0.4705882,0)
		
		[Header(RimColor)]
		_RimColor("RimColor", Color) = (0.3960784,0.3529412,0.3843137,0)
		_RimHight("RimHight", Float) = 5
		_RimLerpSmooth("RimLerpSmooth", Float) = 0.5
		
		[Header(Height FOG)]
		_FogStrength("FogStrength", Range(0,1)) = 0
		_FogColor("FogColor", Color) = (0.5821022,0.9716981,0.9316334,0)
		_FogHigh("FogHigh", Float) = 5
		_FogSmooth("FogSmooth", Range( 0.01 , 100)) = 0.5

		[HideInInspector] _SrcBlend("__src", Float) = 1.0
		[HideInInspector] _DstBlend("__dst", Float) = 0.0
		[HideInInspector] _ZWrite("__zw", Float) = 1.0
		[HideInInspector] _Cull("__cull", Float) = 2.0
	}

	SubShader
	{
		Tags{"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "Lit" "IgnoreProjector" = "True" "ShaderModel" = "4.5"}
		Pass
		{
			Name "ForwardLit"
			Tags{"LightMode" = "UniversalForward"}
			Blend[_SrcBlend][_DstBlend]
			ZWrite[_ZWrite]
			Cull[_Cull]
	HLSLPROGRAM
			#include "../Includes/CommonUtil.hlsl"
			#include "../Includes/WorldCurvedCG.hlsl"	
			#pragma multi_compile_instancing
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT				
			#pragma vertex vert
			#pragma fragment frag
			struct appdata
			{
				float4 vertex: POSITION;
				float2 uv_texcoord: TEXCOORD0;
				float2 uv2_texcoord2: TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 pos			: SV_POSITION;
				float2 uv			: TEXCOORD0;
				float4 aoLightmap	: TEXCOORD1;
				float3 worldPos		: TEXCOORD2;
				float3 positionWS     : TEXCOORD3;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform sampler2D _MainTexture;
			uniform sampler2D _AO;
			uniform sampler2D _LightMap;
			
			CBUFFER_START(UnityPerMaterial)
				uniform float4 _MainTexture_ST;
				real4 _MainColor;
				uniform real4 _AO_ST;
				uniform real _AOStrength;
				uniform real4 _LightColor;
				uniform real4 _LightMap_ST;
				uniform real _LightMapStrength;
				uniform real4 _RimColor;
				uniform real4 _ShadowColor;
				uniform real _RimHight;
				uniform real _RimLerpSmooth;
				uniform real4 _FogColor;
				uniform real _FogHigh;
				uniform real _FogSmooth;
				uniform real _FogStrength;
				uniform real _AddLightPower;
				uniform real4 _AddLight;
			CBUFFER_END
			
			v2f vert(appdata i)
			{
				v2f o = (v2f)0;
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);	
				o.pos = UnityObjectToClipPos(i.vertex);			
				o.uv = TRANSFORM_TEX(i.uv_texcoord,_MainTexture);
				o.worldPos = mul(UNITY_MATRIX_M, i.vertex).xyz;
				o.aoLightmap.xy = TRANSFORM_TEX(i.uv2_texcoord2,_AO);
				o.aoLightmap.zw = TRANSFORM_TEX(i.uv2_texcoord2,_LightMap);
				
				VertexPositionInputs vertexInput = GetVertexPositionInputs(i.vertex.xyz);
				o.positionWS = vertexInput.positionWS;	
				return o;
			}

			float4 frag(v2f i) :COLOR
			{
				real4 baseColor = tex2D( _MainTexture, i.uv )*_MainColor;
				real4 tex2DNode6 = tex2D( _LightMap, i.aoLightmap.zw );
				real3 decodeLightMap59 = DecodeLightmap((tex2DNode6.r).xxxx);
				real3 temp_output_116_0 = saturate( ( decodeLightMap59 / _LightMapStrength ) );
				real3 ase_worldPos = i.worldPos;
				real4 lerpResult70 = lerp( _RimColor , _ShadowColor , saturate( ( ( ase_worldPos.y + _RimHight ) / _RimLerpSmooth ) ));
				real4 baseMulAo =  baseColor * tex2D( _AO, i.aoLightmap.xy ) * _AOStrength;
				
				real4 calcShadowAdd = saturate( lerpResult70 * float4( ( 1.0 - temp_output_116_0 ) , 0.0 ) );
				real4 lightAdd = _LightColor * float4( temp_output_116_0 , 0.0 ) + calcShadowAdd;
				
				real4 temp_output_49_0 = baseMulAo * lightAdd;

				real4 lerpResult136 = lerp( _FogColor , temp_output_49_0 , saturate( ( ( ase_worldPos.y + _FogHigh ) / _FogSmooth ) ));
				real4 lerpResult139 = lerp( temp_output_49_0 , lerpResult136 , _FogStrength);
				real temp_output_228_0 = ( tex2DNode6.g + 0.0 );
				real4 finalColor = ( lerpResult139 + ( ( temp_output_228_0 / _AddLightPower ) * _AddLight ) );
				finalColor.a = 1;
			#ifdef _MAIN_LIGHT_SHADOWS
                float4 shadowCoord = TransformWorldToShadowCoord(i.positionWS);
                half shadowAttenutation = MainLightRealtimeShadow(shadowCoord);
				
				float argvLight = 0.6f;//_ShadowColor.a;
                finalColor = lerp(finalColor, float4(0,0,0,0.81), (1.0 - shadowAttenutation) * argvLight);
            #endif
				return finalColor;
			}
		ENDHLSL			
		}
        Pass
        {
            Name "DepthOnly"
            Tags{"LightMode" = "DepthOnly"}

            ZWrite On
            ColorMask 0

            HLSLPROGRAM
            #pragma target 2.0

            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            #pragma multi_compile_instancing
            #pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap

            #include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitPasses.hlsl"
            ENDHLSL
        }   	
	}
	Fallback Off
}