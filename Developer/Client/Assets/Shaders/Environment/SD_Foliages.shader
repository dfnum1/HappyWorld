Shader "SD/Environment/SD_Foliages" 
{
	Properties 
	{
		_AmbientStrength("Ambient Strength",Range(0,2)) = 1
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_SpeedStrength("strength",Range(0,10)) = 0.1
		_Cutoff("Alpha Cutoff", Range(0,1)) = 0.5
		_AnimationStemX("Animation Stem X",Range(0,10)) = 2
		_AnimationStemY("Animation Stem Y",Range(0,2)) = 0.25
		_AnimationStemZ("Animation Stem Z",Range(0,2)) = 0.25
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
		//ZWrite Off
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
			#pragma multi_compile __ USE_BILLBOARD
			#include "../Includes/CommonUtil.hlsl"
			#include "../Includes/WorldCurvedCG.hlsl"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 pos		: SV_POSITION;
				float4 uv		: TEXCOORD0;
				float4 color	: COLOR;
				UNITY_FOG_COORDS(1)
				UNITY_VERTEX_OUTPUT_STEREO
			};
			UNITY_INSTANCING_BUFFER_START(Props)
				UNITY_DEFINE_INSTANCED_PROP(float4, _ExternFactor)
				UNITY_DEFINE_INSTANCED_PROP(float4, _ExternParam)
			UNITY_INSTANCING_BUFFER_END(Props)

			sampler2D _MainTex;


			CBUFFER_START(UnityPerMaterial)
				float4	  _MainTex_ST;
				float _AmbientStrength;
				float _SpeedStrength, _Cutoff, _AnimationStemX, _AnimationStemY, _AnimationStemZ;
				fixed _CurveFactor;
			CBUFFER_END

			v2f vert(appdata i)
			{
				v2f o = (v2f)0;
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				

				float2 uvOffset = float2(0,0);
				float windForce = 0;
#if INSTANCING_ON
				float4 externFactor = UNITY_ACCESS_INSTANCED_PROP(Props, _ExternFactor);
				o.color = float4(externFactor.x, externFactor.y, externFactor.z,1);
				windForce = externFactor.w;
				uvOffset = UNITY_ACCESS_INSTANCED_PROP(Props, _ExternParam).xy;
#else
				o.color = float4(1,1,1,1);
#endif
				CURVED_WORLD_TRANSFORM_POINT(i.vertex,_CurveFactor);

				o.uv.xy = i.texcoord.xy * _MainTex_ST.xy + i.texcoord.zw * _MainTex_ST.zw + uvOffset;
				float fade = i.texcoord.y;
				float4 offset = getTreeLeafAnimationStream(_AnimationStemX, _AnimationStemY, _AnimationStemZ, _SpeedStrength, fade, windForce);

#if USE_BILLBOARD
				float3 vpos = mul((float3x3)UNITY_MATRIX_M, i.vertex.xyz + offset.xyz);
				float4 worldCoord = float4(UNITY_MATRIX_M._m03, UNITY_MATRIX_M._m13, UNITY_MATRIX_M._m23, 1);
				float4 viewPos = mul(UNITY_MATRIX_V, worldCoord) + float4(vpos.x, vpos.y, 0, 0);
				float4 outPos = mul(UNITY_MATRIX_P, viewPos);
				o.pos = outPos;
#else
				o.pos = UnityObjectToClipPos(i.vertex + offset);
#endif
				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}

			float4 frag(v2f i) :COLOR
			{
				float4 col = tex2D(_MainTex,i.uv.xy);

				Util_AmbientColor(col, _AmbientStrength);
				float4 final = col* i.color;
				clip(col.a - _Cutoff);
				UNITY_APPLY_FOG(i.fogCoord, final);
				return  final;
			}
			ENDHLSL
		}
	}
	FallBack Off
}