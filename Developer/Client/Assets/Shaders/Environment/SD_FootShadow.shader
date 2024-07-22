Shader "SD/Environment/SD_FootShadow" 
{
	Properties 
	{
		_AmbientStrength("Ambient Strength",Range(0,2)) = 1
		_MainTex ("Base (RGB)", 2D) = "white" {}
		//_Cutoff("Alpha Cutoff", Range(0,1)) = 0.5
		[HDR]_TintColor("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_CurveFactor("CurveFactor", Float) = 1
	}
	SubShader
	{
		Tags { "Queue" = "Transparent+100"  "IgnoreProjector" = "True"}
		Blend SrcAlpha OneMinusSrcAlpha
		Cull[_Cull]
		Lighting Off
		ZWrite[_Zwrite]
		LOD 100
		Pass
		{
			Tags{ "LightMode" = "UniversalForward" }
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest

			#pragma multi_compile_instancing
			#pragma multi_compile __ FOG_LINEAR
			#include "Assets/Shaders/Includes/WorldCurvedCG.hlsl"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 pos		: SV_POSITION;
				float2 uv		: TEXCOORD0;
				UNITY_FOG_COORDS(1)
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _MainTex;


			CBUFFER_START(UnityPerMaterial)
				float4	  _MainTex_ST;
				fixed _AmbientStrength;
				fixed4 _TintColor;
			//	fixed _Cutoff;
				fixed _CurveFactor;
			CBUFFER_END

			v2f vert(appdata i)
			{
				v2f o = (v2f)0;
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				
				CURVED_WORLD_TRANSFORM_POINT(i.vertex,_CurveFactor);

				o.uv.xy = i.texcoord.xy * _MainTex_ST.xy + i.texcoord.zw * _MainTex_ST.zw;
				o.pos = UnityObjectToClipPos(i.vertex);
				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex,i.uv);

				Util_AmbientColor(col, _AmbientStrength);
				float4 final = col*_TintColor;
			//	clip(final.a - _Cutoff);
				UNITY_APPLY_FOG(i.fogCoord, final);
				return  final;
			}
			ENDHLSL
		}
	}
	FallBack Off
}