Shader "SD/Particles/SD_Particle"
{
	Properties
	{
		[Toggle] _UseVertexColor("Use Vertex Color", Int) = 1
		[HDR]_MainColor("MainColor", Color) = (0.5019608,0.5019608,0.5019608,1)
		_MainTex("MainTex", 2D) = "white" {}
		[Toggle] _TexBlackAlpha("Use Black Alpha", Int) = 0
		[Enum(UnityEngine.Rendering.CullMode)]_Float2("CullModel", Float) = 0
		[MaterialEnum(Add,1, Blend, 10)]_BlendMode("BlendMode", Int) = 10
		_CurveFactor("CurveFactor", Float) = 1
	}

	SubShader
	{
		LOD 0

		
		Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "Queue" = "Transparent" }
		
		Cull [_Float2]
		ZWrite Off
		AlphaToMask Off
		Lighting Off
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="UniversalForward" }
			
			Blend SrcAlpha [_BlendMode]
			ZWrite Off
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			#define _RECEIVE_SHADOWS_OFF 1
			#pragma multi_compile_instancing

			//#pragma prefer_hlslcc gles

			#pragma vertex vert
			#pragma fragment frag

			#include "Assets/Shaders/Includes/WorldCurvedCG.hlsl"

			struct VertexInput
			{
				float4 vertex : POSITION;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 ase_color : COLOR;
				float2 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _MainTex_ST;
			float4 _MainColor;
			//float4 _MaskTex_ST;
			fixed _CurveFactor;
			fixed _TexBlackAlpha;
			fixed _UseVertexColor;
			CBUFFER_END
			sampler2D _MainTex;
		
			VertexOutput VertexFunction ( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				
				o.ase_texcoord = TRANSFORM_TEX(v.ase_texcoord,_MainTex);
				
				o.ase_color = (float4(v.ase_color.r,v.ase_color.g,v.ase_color.b,1)*(1-_UseVertexColor) + _UseVertexColor*v.ase_color)*_MainColor;

				CURVED_WORLD_TRANSFORM_POINT(v.vertex,_CurveFactor);
				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#ifdef ASE_FOG
				o.fogFactor = ComputeFogFactor( positionCS.z );
				#endif
				o.clipPos = positionCS;
				return o;
			}

			
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}

			float4 frag ( VertexOutput IN , float ase_vface : VFACE ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );
				
				float4 main = tex2D(_MainTex, IN.ase_texcoord);
				float4 Color = main *IN.ase_color;

				Color.a *= main.r * _TexBlackAlpha + main.a * (1 - _TexBlackAlpha);
				#ifdef ASE_FOG
					Color = MixFog( Color, IN.fogFactor );
				#endif

				return Color;
			}

			ENDHLSL
		}
	}
	Fallback Off
}