Shader "SD/Environment/SD_TreeLeaf" 
{
	Properties 
	{
		_AmbientStrength("Ambient Strength",Range(0,2)) = 1
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_LightMap ("Lightmap (RGB)", 2D) = "black" {}
		_MixLight("Lightmap Strength",Range(0,10)) = 1
		[MaterialToggle(USE_LIGHTMAP)] _USE_LIGHTMAP("IsUseLightMap", Float) = 1.0
		_Cutoff("Alpha Cutoff", Range(0,1)) = 0.5
		_CurveFactor("CurveFactor", Float) = 1
		_AlphaFade("Alpha Fade(near,near distance, far, far distance)", vector) = (3,0.5,200,100)
		//_FogDistance("Fog Distance", Float) = 0

		[MaterialToggle(USE_VERTEX_ANIMATION)] _USE_VERTEX_ANIMATION("Use Vertex Animation", Float) = 0
		_SpeedStrength("strength",Range(0,10)) = 0.1
		_AnimationStemX("Animation Stem X",Range(0,10)) = 2
		_AnimationStemY("Animation Stem Y",Range(0,2)) = 0.25
		_AnimationStemZ("Animation Stem Z",Range(0,2)) = 0.25
		
		[Header(Height FOG)]
		[MaterialToggle(USE_HEIGHTFOG)] _USE_HEIGHTFOG("IsUseHeightFog", Int) = 0
		_HeightFogStrength("FogStrength", Range(0,1)) = 0
		_HeightFogColor("FogColor", Color) = (0.5821022,0.9716981,0.9316334,0)
		_HeightFogHigh("FogHigh", Float) = 5
		_HeightFogSmooth("FogSmooth", Range( 0.01 , 100)) = 0.5			
	}
	SubShader
	{
		//Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" "PreviewType" = "Plane"}
		//Blend SrcAlpha OneMinusSrcAlpha
		//ZWrite On

		Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }
		AlphaToMask On
		ColorMask RGB

		Cull Back 
		Lighting Off
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
			#pragma multi_compile __ USE_VERTEX_ANIMATION
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
				float _MixLight, _Cutoff;
				float _SpeedStrength, _AnimationStemX, _AnimationStemY, _AnimationStemZ;
				fixed4 _AlphaFade;
				//float _FogDistance;
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

				float4 offset = float4(0,0,0,0);
#if USE_VERTEX_ANIMATION
				float fade = i.texcoord.y;
				offset = getTreeLeafAnimationStream(_AnimationStemX, _AnimationStemY, _AnimationStemZ, _SpeedStrength, fade, 0);
#endif
				o.pos = UnityObjectToClipPos(i.vertex)+ offset;

				o.uv.w = Util_AlphaFadeNearFar(i.vertex, _AlphaFade.x, _AlphaFade.x+ _AlphaFade.y, _AlphaFade.z, _AlphaFade.z+ _AlphaFade.w);

#if USE_HEIGHTFOG
				o.worldPos = mul(UNITY_MATRIX_M, i.vertex+offset).xyz;
#endif		
				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}

			float4 frag(v2f i) :COLOR
			{
				float4 col = tex2D(_MainTex,i.uv.xy);
			//	col.a *= i.uv.w;
				Util_AmbientColor(col, _AmbientStrength);

#if USE_LIGHTMAP
				float4 lightCol = tex2D(_LightMap, i.lightUV);
				float4 final = col * _MixLight;
				final.rgb *= lightCol.rgb;
				final.a = col.a;
#else
				float4 final = col * _MixLight;
#endif
				final.a *= i.uv.w;
				clip(final.a - _Cutoff);
				//UNITY_APPLY_FOG(i.fogCoord+_FogDistance*0.01*unity_FogParams.w, final);
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