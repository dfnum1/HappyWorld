Shader "SD/Environment/SD_TreeMaster" 
{
	Properties 
	{
		_AmbientStrength("Ambient Strength",Range(0,2)) = 1
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_LightMap ("Lightmap (RGB)", 2D) = "black" {}
		_MixLight("Lightmap Strength",Range(0,10)) = 1
		[MaterialToggle(USE_LIGHTMAP)] _USE_LIGHTMAP("IsUseLightMap", Float) = 1.0
		_CurveFactor("CurveFactor", Float) = 1
		_AlphaFade("Alpha Fade(near,near distance, far, far distance)", vector) = (3,0.5,200,100)
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
		Lighting Off
		Cull Back 
		Lighting Off
		ZWrite On
		LOD 100 
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

			#include "Assets/Shaders/Includes/WorldCurvedCG.hlsl"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal	: NORMAL;
				float4 texcoord : TEXCOORD0;
#if USE_LIGHTMAP
				float2 lightUV	: TEXCOORD1;
#endif
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 pos		: SV_POSITION;
				float4 uv		: TEXCOORD0;
				UNITY_FOG_COORDS(1)
#if USE_LIGHTMAP
				float2 lightUV	: TEXCOORD2;
#endif
#if USE_HEIGHTFOG
				float3 worldPos		: TEXCOORD3;
#endif
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _MainTex;
			sampler2D _LightMap;

			CBUFFER_START(UnityPerMaterial)
			float4	  _MainTex_ST;
			float4	  _LightMap_ST;
			fixed _MixLight;
			//fixed _Cutoff;
			fixed4 _AlphaFade;
			float _FogDistance;
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
				o.uv.xy = i.texcoord.xy*_MainTex_ST.xy + i.texcoord.zw*_MainTex_ST.zw;
#if USE_LIGHTMAP
				o.lightUV = TRANSFORM_TEX(i.lightUV, _LightMap);
#endif

				CURVED_WORLD_TRANSFORM_POINT(i.vertex,_CurveFactor);

				o.pos = UnityObjectToClipPos(i.vertex);

				o.uv.w = Util_AlphaFadeNearFar(i.vertex, _AlphaFade.x, _AlphaFade.x + _AlphaFade.y, _AlphaFade.z, _AlphaFade.z + _AlphaFade.w);
	#if USE_HEIGHTFOG
				o.worldPos = mul(UNITY_MATRIX_M, i.vertex).xyz;
#endif				
				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}

			float4 frag(v2f i) :COLOR
			{
				float4 col = tex2D(_MainTex,i.uv.xy);

				Util_AmbientColor(col, _AmbientStrength);
				col.a *= i.uv.w;

#if USE_LIGHTMAP
				float4 lightCol = tex2D(_LightMap, i.lightUV);
				float4 final = col * lightCol * _MixLight;
				final.a = lightCol.a;
#else
				float4 final = col * _MixLight;
#endif
				UNITY_APPLY_FOG(i.fogCoord+ _FogDistance*0.01*unity_FogParams.w, final);
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