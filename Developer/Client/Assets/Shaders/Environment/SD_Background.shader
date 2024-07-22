Shader "SD/Environment/SD_Background" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_AmbientStrength("Ambient Strength",Range(0,2)) = 1
		_LightMap ("Lightmap (RGB)", 2D) = "black" {}
		_MixLight("Lightmap Strength",Range(0,10)) = 1
		[MaterialToggle(USE_LIGHTMAP)] _USE_LIGHTMAP("IsUseLightMap", Float) = 1.0
		_Cutoff("Alpha Cutoff", Range(0,1)) = 0.5
		_CurveFactor("CurveFactor", Float) = 1
		_AlphaFade("Alpha Fade(near,near distance, far, far distance)", vector) = (3,0.5,200,100)
		_FogDistance("Fog Distance", Float) = 0
		_FogHeight("Fog Height", Range(1,100)) = 1
		_FogHeightOffset("Fog Height Offset", Range(-100,100)) = 0
	}
	SubShader
	{
		Tags 
		{
			"Queue" = "Transparent" 
			"RenderType" = "Transparent" 
			"IgnoreProjector" = "True" 
			"PreviewType" = "Plane"
		}
		Blend SrcAlpha OneMinusSrcAlpha
//		Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "False" "RenderType" = "TransparentCutout" }
		Lighting Off
		AlphaToMask On
		//ColorMask RGBA
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
				UNITY_VERTEX_OUTPUT_STEREO
			};
			sampler2D _MainTex;
			sampler2D _LightMap;

			CBUFFER_START(UnityPerMaterial)
			float4	  _MainTex_ST;
			float4	  _LightMap_ST;
			
			float _MixLight, _Cutoff, _FogHeight, _FogHeightOffset;
			float _AmbientStrength;
			fixed4 _AlphaFade;
			float _FogDistance;
			fixed _CurveFactor;
			CBUFFER_END
			v2f vert(appdata i)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.uv.xy = i.texcoord.xy*_MainTex_ST.xy + i.texcoord.zw*_MainTex_ST.zw;
#if USE_LIGHTMAP
				o.lightUV = TRANSFORM_TEX(i.lightUV, _LightMap);
#endif

				CURVED_WORLD_TRANSFORM_POINT(i.vertex, _CurveFactor);

				o.pos = UnityObjectToClipPos(i.vertex);
				
				float3 center = UNITY_MATRIX_M._14_24_34;
				float4 worldPos = mul(UNITY_MATRIX_M, i.vertex);
				
				o.uv.z = max(0, center.y -worldPos.y- _FogHeightOffset)/ max(0.001,abs(_FogHeight));

				o.uv.w = Util_AlphaFadeNearFar(i.vertex, _AlphaFade.x, _AlphaFade.x+ _AlphaFade.y, _AlphaFade.z, _AlphaFade.z+ _AlphaFade.w);
				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}

			float4 frag(v2f i) :COLOR
			{
				float4 col = tex2D(_MainTex,i.uv.xy);
				Util_AmbientColor(col, _AmbientStrength);
			//	col.a *= i.uv.w;

#if USE_LIGHTMAP
				float4 lightCol = tex2D(_LightMap, i.lightUV);
				float4 final = col * lightCol * _MixLight;
				final.a = lightCol.a;
#else
				float4 final = col * _MixLight;
#endif
				final.rgb += unity_FogColor.rgb*i.uv.z;
				clip(col.a - _Cutoff);
				final.a *= i.uv.w;
				UNITY_APPLY_FOG(i.fogCoord+ _FogDistance*0.01*unity_FogParams.w, final);
				return  final;
			}
			ENDHLSL
		}
	}
	FallBack Off
}