Shader "SD/Environment/SD_Grass" 
{
	Properties 
	{
		_AmbientStrength("Ambient Strength",Range(0,2)) = 1
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_LightMap ("Lightmap (RGB)", 2D) = "black" {}
		_MixLight("Lightmap Strength",Range(0,10)) = 1
		[MaterialToggle(USE_LIGHTMAP)] _USE_LIGHTMAP("IsUseLightMap", Float) = 1.0
	//	_AlphaScale("Alhpa Scale",Range(0,1)) = 1
		_Cutoff("Alpha Cutoff", Range(0,1)) = 0.5
		_MinDistance("Min Distance", Float) = 0
		_MaxDistance("Max Distance", Float) = 200
		_MinFade("Min Fade", Float) = 0
		_MaxFade("Max Fade", Float) = 200
		_CurveFactor("CurveFactor", Float) = 1
	}
	SubShader
	{
		//Tags 
		//{
		//	"Queue" = "Transparent" 
		//	"RenderType" = "Transparent" 
		//	"IgnoreProjector" = "True" 
		//	"PreviewType" = "Plane"
		//}
		//Blend SrcAlpha OneMinusSrcAlpha
		Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "False" "RenderType" = "TransparentCutout" }
		Lighting Off
		AlphaToMask On
		//ColorMask RGBA
		Cull Off 
		Lighting Off
		ZWrite Off
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

			#include "../Includes/WorldCurvedCG.hlsl"

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
			fixed _CurveFactor;
			
			float _MixLight, _MinFade, _MaxFade, _MinDistance, _MaxDistance, _Cutoff;
			//float _AlphaScale;
			float _AmbientStrength;
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

				float3 posView = UnityObjectToViewPos(i.vertex.xyz).xyz;
				float distance = length(posView);
				float fade_in = saturate((distance - _MinDistance) / _MinFade);
				float fade_out = saturate((distance - _MaxDistance) / _MaxFade);
				float fade = saturate(fade_in - fade_out);
		//		o.uv.w = fade * _AlphaScale * 0.5f + 0.5f;

				float4 s_material_animation_stem = float4(1,0.25,0.25, _Time.y);
				float4 offset = float4(0, 0, 0,0);
				float stem_scale = max(i.normal.x* s_material_animation_stem.y, 0.0f);
				float stem_angle = dot(i.vertex.xy, s_material_animation_stem.x) + s_material_animation_stem.w;
			//	float2 stem_offset = getAnimationOffset(stem_angle) * (stem_scale * fade);
				//offset.y -= length(stem_offset) * s_material_animation_stem.z;
				//offset.xz += stem_offset;

				o.pos = UnityObjectToClipPos(i.vertex+ offset);

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
				float4 final = col * lightCol * _MixLight;
				final.a = lightCol.a;
#else
				float4 final = col * _MixLight;
#endif
				clip(col.a - _Cutoff);
				UNITY_APPLY_FOG(i.fogCoord, final);
				return  final;
			}
			ENDHLSL
		}
	}
	FallBack Off
}