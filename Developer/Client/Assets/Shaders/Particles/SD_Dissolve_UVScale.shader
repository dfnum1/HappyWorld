Shader "SD/Particles/SD_Dissolve_UVScale"
{
	Properties
	{
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[ASEBegin][HDR]_MainColor("MainColor", Color) = (0.5019608,0.5019608,0.5019608,1)
		[HDR]_MainColorBack("MainColorBack", Color) = (0.5019608,0.5019608,0.5019608,1)
		[HDR]_DIssolveEdgeColor("DIssolveEdgeColor", Color) = (0.5019608,0.5019608,0.5019608,1)
		_MainTex("MainTex", 2D) = "white" {}
		_MaskTex("MaskTex", 2D) = "white" {}
		_DissolveTex("DissolveTex", 2D) = "white" {}
		_Dissolve_Edge("Dissolve_Edge", Range( 0 , 0.5)) = 0
		_Dissolve_Smooth("Dissolve_Smooth", Range( 0.51 , 1)) = 0.51
		[Enum(UnityEngine.Rendering.CullMode)]_Float2("CullModel", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[MaterialEnum(Add,1, Blend, 10)]_BlendMode("BlendMode", Int) = 10
		_CurveFactor("CurveFactor", Float) = 1
		[Enum(Off, 0, On,1)]_Zwrite("ZWrite", Float) = 0
	}

	SubShader
	{
		LOD 0

		
		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }
		
		Cull [_Float2]
		AlphaToMask Off
		Lighting Off
		ZWrite[_Zwrite]
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="UniversalForward" }
			
			Blend SrcAlpha [_BlendMode]
			ZWrite[_Zwrite]
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			#define _RECEIVE_SHADOWS_OFF 1
			#pragma multi_compile_instancing

			//#pragma prefer_hlslcc gles
			//#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag
			
			#include "Assets/Shaders/Includes/WorldCurvedCG.hlsl"
			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float3 worldPos : TEXCOORD0;
				#ifdef ASE_FOG
				float fogFactor : TEXCOORD1;
				#endif
				float4 ase_texcoord3 : TEXCOORD2;
				float4 ase_texcoord4 : TEXCOORD3;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _MainTex_ST;
			float4 _MainColor;
			float4 _MainColorBack;
			float4 _DissolveTex_ST;
			float4 _DIssolveEdgeColor;
			float4 _MaskTex_ST;
			float _Dissolve_Smooth;
			float _Dissolve_Edge;
			float _CurveFactor;
			CBUFFER_END
			sampler2D _MainTex;
			sampler2D _DissolveTex;
			sampler2D _MaskTex;


						
			VertexOutput VertexFunction ( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				
				o.ase_texcoord3.xy = v.ase_texcoord.xy;
				o.ase_texcoord4 = v.ase_texcoord1;
				
				o.ase_color = v.ase_color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord3.zw = 0;
				float3 defaultVertexValue = v.vertex.xyz;
				float3 vertexValue = defaultVertexValue;
				v.vertex.xyz = vertexValue;
				v.ase_normal = v.ase_normal;

				CURVED_WORLD_TRANSFORM_POINT(v.vertex,_CurveFactor);
				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				o.worldPos = positionWS;
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

				float3 WorldPosition = IN.worldPos;
				float4 ShadowCoords = float4( 0, 0, 0, 0 );
				float4 main_color = _MainColor;
				if(ase_vface < 0) main_color = _MainColorBack;

				float2 uv_MainTex = IN.ase_texcoord3.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode7 = tex2Dlod( _MainTex, float4( ( ( ( uv_MainTex - float2( 0.5,0.5 ) ) * (5.0 + (IN.ase_texcoord4.y - 0.0) * (0.0 - 5.0) / (1.0 - 0.0)) ) + float2( 0.5,0.5 ) ), 0, 0.0) );
				float4 switchResult162 = ( tex2DNode7 * main_color );
				float temp_output_76_0 = ( 1.0 - _Dissolve_Smooth );
				float2 uv_DissolveTex = IN.ase_texcoord3.xy * _DissolveTex_ST.xy + _DissolveTex_ST.zw;
				float4 tex2DNode31 = tex2D( _DissolveTex, uv_DissolveTex );
				float smoothstepResult75 = smoothstep( temp_output_76_0 , _Dissolve_Smooth , ( tex2DNode31.r + 1.0 + ( IN.ase_texcoord4.x * -2.0 ) ));
				float smoothstepResult177 = smoothstep( temp_output_76_0 , _Dissolve_Smooth , ( tex2DNode31.r + 1.0 + ( ( IN.ase_texcoord4.x + _Dissolve_Edge ) * -2.0 ) ));
				
				float switchResult190 = main_color.a;
				float2 uv_MaskTex = IN.ase_texcoord3.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
				float4 tex2DNode30 = tex2D( _MaskTex, uv_MaskTex );
				float clampResult42 = clamp( smoothstepResult75 , 0.0 , 1.0 );
				
				float3 BakedAlbedo = 0;
				float3 BakedEmission = 0;
				float3 Color = ( ( switchResult162 + ( ( smoothstepResult75 - smoothstepResult177 ) * _DIssolveEdgeColor ) ) * IN.ase_color ).rgb;
				float Alpha = ( switchResult190 * IN.ase_color.a * tex2DNode30.r * clampResult42 * tex2DNode7.a * tex2DNode30.a );
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;

				#ifdef _ALPHATEST_ON
					clip( Alpha - AlphaClipThreshold );
				#endif

				//#ifdef LOD_FADE_CROSSFADE
				//	LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				//#endif

				#ifdef ASE_FOG
					Color = MixFog( Color, IN.fogFactor );
				#endif

				float4 final = float4( Color, Alpha );
				final = clamp(final, 0, 2);
				return final;
			}

			ENDHLSL
		}

	}
	Fallback Off
}