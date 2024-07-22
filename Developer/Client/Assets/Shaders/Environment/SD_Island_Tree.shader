Shader "SD/Environment/SD_IslandTree" 
{
	Properties 
	{
		_AmbientStrength("Ambient Strength",Range(0,2)) = 1
		[HDR]_MainColor("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_CurveFactor("CurveFactor", Float) = 1
		_SpeedStrength("strength",Range(0,10)) = 0.1
		_AnimationStemX("Animation Stem X",Range(0,10)) = 2
		_AnimationStemY("Animation Stem Y",Range(0,2)) = 0.25
		_AnimationStemZ("Animation Stem Z",Range(0,2)) = 0.25
		[MaterialToggle]_FogToggle("EnableFog", Float) = 1
		
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
			#pragma multi_compile __ USE_BILLBOARD
			#include "../Includes/CommonUtil.hlsl"
			#include "../Includes/WorldCurvedCG.hlsl"

			struct appdata
			{
				real4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				real4 pos		: SV_POSITION;
				float2 uv		: TEXCOORD0;
				float4 color	: COLOR;
				UNITY_FOG_COORDS(1)
			//	real4 screenPos : TEXCOORD3;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			TEXTURE2D_DEF(_MainTex);
			

			CBUFFER_START(UnityPerMaterial)
				real4	  _MainTex_ST;
				real4 		_MainColor;
				fixed _FogToggle, _AmbientStrength;
				fixed _CurveFactor;
				fixed _SpeedStrength, _AnimationStemX, _AnimationStemY, _AnimationStemZ;
			CBUFFER_END
			
			UNITY_INSTANCING_BUFFER_START(Props)
				UNITY_DEFINE_INSTANCED_PROP(float4, _ExternFactor)
				UNITY_DEFINE_INSTANCED_PROP(float4, _ExternParam)
			UNITY_INSTANCING_BUFFER_END(Props)	
			v2f vert(appdata i)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.uv = TRANSFORM_TEX(i.texcoord, _MainTex);
				
				
				
				real2 uvOffset = float2(0,0);
				real windForce = 0;
#if INSTANCING_ON
				real4 externFactor = UNITY_ACCESS_INSTANCED_PROP(Props, _ExternFactor);
				o.color = real4(externFactor.x, externFactor.y, externFactor.z,1)*_MainColor;
				windForce = externFactor.w;
				uvOffset = UNITY_ACCESS_INSTANCED_PROP(Props, _ExternParam).xy;
#else
				o.color = _MainColor;
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

			real4 frag(v2f i) :COLOR
			{
				real4 col = SAMPLE_TEXTURE2D_DEF(_MainTex,i.uv) * i.color;
				Util_AmbientColor(col, _AmbientStrength);
				real4 final = col;
				UNITY_APPLY_FOG(i.fogCoord, final);
				return  final;
			}
			ENDHLSL
		}
	}
	FallBack Off
}