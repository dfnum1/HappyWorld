Shader "SD/Object/CurvedUnlitLightMap"
{ 
	Properties
	{
		_MainColor("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_LightMap("Lightmap (RGB)", 2D) = "black" {}
		_LightStrength("Lightmap Strength",Range(0,10)) = 1
		[MaterialToggle(USE_LIGHTMAP)] _USE_LIGHTMAP("IsUseLightMap", Float) = 1.0
		_CurveFactor("CurveFactor", Float) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work 
			#pragma multi_compile_fog
				
			#include "Assets/Shaders/Includes/WorldCurvedCG.hlsl"
			
			struct appdata
			{
				float4 vertex 	: POSITION;
				float2 uv 		: TEXCOORD0;
				float2 lightUV	: TEXCOORD1;
			};

			struct v2f
			{
				float4 vertex 	: SV_POSITION;
				float2 uv 		: TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float2 lightUV	: TEXCOORD2;
				
			};
			sampler2D _MainTex;
			sampler2D _LightMap;
			CBUFFER_START(UnityPerMaterial)
			float4 _MainTex_ST;
			float4 _MainColor;
			float4	  _LightMap_ST;
			fixed _LightStrength, _CurveFactor;
			CBUFFER_END
			v2f vert(appdata v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.lightUV = TRANSFORM_TEX(v.lightUV, _LightMap);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				CURVED_WORLD_TRANSFORM_POINT(v.vertex,_CurveFactor);
				o.vertex = UnityObjectToClipPos(v.vertex);
				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv) * _MainColor;
				float4 lightCol = tex2D(_LightMap, i.lightUV);
				float4 final = col * lightCol * _LightStrength;
				final.a = lightCol.a * _MainColor.a;
				
				UNITY_APPLY_FOG(i.fogCoord, final);
				return final;
			}
			ENDHLSL
		}
	}
	FallBack Off
}
