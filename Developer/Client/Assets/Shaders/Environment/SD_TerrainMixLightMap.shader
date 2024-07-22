Shader "SD/Environment/SD_TerrainMixLightMap" 
{
	Properties 
	{
		_AmbientStrength("Ambient Strength",Range(0,2)) = 1
		_MixTex ("MixTex (RGB)", 2D) = "white" {}
		 _Weight("Blend Weight" , Range(0.001,10)) = 0.2
		_MainTex ("Main (RGBA)", 2D) = "white" {}
		_Layer1Tex ("Layer1 (RGBA)", 2D) = "black" {}
		_Layer2Tex ("Layer2 (RGBA)", 2D) = "black" {}
		_LightMap ("Lightmap (RGBA)", 2D) = "black" {}
		_LightMapColor("LightMap Color",Color) = (1,1,1,1)
		_MidLight("Lightmap Strength",Range(0,10)) = 1
		[MaterialToggle(USE_LIGHTMAP)] _USE_LIGHTMAP("IsUseLightMap", Float) = 1.0
		[MaterialToggle(USE_VARY)] _USE_VARY("Use Vary", Float) = 1.0
		[HideInInspector]_CurveFactor("CurveFactor", Float) = 1
		_FogDistance("Fog Distance", Float) = 0
		
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
				float2 uv1		: TEXCOORD1;
				float2 uv2		: TEXCOORD2;
				UNITY_FOG_COORDS(3)
#if USE_LIGHTMAP
				float2 lightUV	: TEXCOORD4;
#endif
#if USE_HEIGHTFOG
				float3 worldPos		: TEXCOORD5;
#endif
				UNITY_VERTEX_OUTPUT_STEREO
			};
			sampler2D _MainTex;
			sampler2D _Layer1Tex;
			sampler2D _Layer2Tex;
			sampler2D _LightMap;
			sampler2D _MixTex;
			CBUFFER_START(UnityPerMaterial)
			float4	  _MainTex_ST;
			float4	  _Layer1Tex_ST;
			float4	  _Layer2Tex_ST;
			float4	  _LightMap_ST;
			float4 	  _MixTex_ST;
			//float4 _MidColor;
			fixed _MidLight;
			real4 _LightMapColor;
			float _FogDistance;
			fixed _Weight;
			fixed _AmbientStrength, _CurveFactor;
			
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
				o.uv1 = TRANSFORM_TEX(i.texcoord, _Layer1Tex);
				o.uv2 = TRANSFORM_TEX(i.texcoord, _Layer2Tex);
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
			
			inline half4 Blend(half high1 ,half high2,half high3, half4 control) 
			{
				half4 blend ;
				blend.r =high1 * control.r;
				blend.g =high2 * control.g;
				blend.b =high3 * control.b;
				blend.a = 0;

				half maxF = max(blend.r, max(blend.g, max(blend.b, blend.a)));
				blend = max(blend - maxF +_Weight , 0) * control;
				return blend/(blend.r + blend.g + blend.b + blend.a);
			}			

			float4 frag(v2f i) :COLOR
			{
				float4 mixFactor = tex2D(_MixTex, i.uv);
				float4 main = tex2D(_MainTex,i.uv);
				float4 layer1 = tex2D(_Layer1Tex,i.uv1);
				float4 layer2 = tex2D(_Layer2Tex,i.uv2);
				
				half4 blend = Blend(main.a, layer1.a, layer2.a, mixFactor.a);
				float4 finalCol = blend.r * main + blend.g * layer1 + blend.b * layer2;// + blend.a * lay4;

				Util_AmbientColor(finalCol, _AmbientStrength);
#if USE_LIGHTMAP
				float4 lightCol = tex2D(_LightMap, i.lightUV)*_LightMapColor;
	#if USE_VARY		
				lightCol.rgb = lightCol.rgb * 2.0 - 1.0;
				float4 final = (main * mixFactor.r + layer1*mixFactor.g + layer2*mixFactor.b) + lightCol * _MidLight;
	#else		
				finalCol = (main * mixFactor.r + layer1*mixFactor.g + layer2*mixFactor.b) * lightCol * _MidLight;
				finalCol.a = lightCol.a;
	#endif				
#else
				finalCol = (main * mixFactor.r + layer1*mixFactor.g + layer2*mixFactor.b) * _MidLight;
#endif

				UNITY_APPLY_FOG(i.fogCoord + _FogDistance*0.01*unity_FogParams.w, finalCol);
#if USE_HEIGHTFOG				
				real4 heighFog = lerp( _HeightFogColor , finalCol, saturate( ( ( i.worldPos.y + _HeightFogHigh ) / _HeightFogSmooth ) ));
				finalCol = lerp(finalCol, heighFog , _HeightFogStrength);
#endif					
				return  finalCol;
			}
			ENDHLSL
		}
	}
	FallBack Off
}