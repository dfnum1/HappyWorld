Shader "SD/Particles/SD_Dissolve_UVpanner_Rim"
{
	Properties
	{
		[Toggle] _UseVertexColor("Use Vertex Color", Int) = 1
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HDR]_MainColor("MainColor", Color) = (0.5019608,0.5019608,0.5019608,1)
		[HDR]_MainColorBack("MainColorBack", Color) = (0.5019608,0.5019608,0.5019608,1)
		[HDR]_DIssolveEdgeColor("DIssolveEdgeColor", Color) = (0.5019608,0.5019608,0.5019608,1)
		_MainTex("MainTex", 2D) = "white" {}
		_MaskTex("MaskTex", 2D) = "white" {}
		_DissolveTex("DissolveTex", 2D) = "white" {}
		[Toggle] _UVpanner_MainTex_ParticleControl("UVpanner_MainTex_ParticleControl", Float) = 0
		_UVpanner_MainTex("UVpanner_MainTex", Vector) = (0,0,0,0)
		_UVpanner_MaskTex("UVpanner_MaskTex", Vector) = (0,0,0,0)
		_UVpanner_DissolveTex("UVpanner_DissolveTex", Vector) = (0,0,0,0)
		[Toggle] _Dissolve_Intensity_ParticleControl("Dissolve_Intensity_ParticleControl", Float) = 0
		_Dissolve_Intensity("Dissolve_Intensity", Range( 0 , 1)) = 0
		_Dissolve_Edge("Dissolve_Edge", Range( 0 , 0.5)) = 0
		_Dissolve_Smooth("Dissolve_Smooth", Range( 0.51 , 1)) = 0.51
		_Twist_Intensity("Twist_Intensity", Range( -1 , 1)) = 0
		[Toggle] _Rim_On("Rim_On", Float) = 0
		[HDR]_RimColor("RimColor", Color) = (0.5019608,0.5019608,0.5019608,0.5019608)
		_Rim_Width("Rim_Width", Float) = 7.1
		[Enum(UnityEngine.Rendering.CullMode)]_Float2("CullModel", Float) = 0
		[MaterialEnum(Add,1, Blend, 10)]_BlendMode("BlendMode", Int) = 10
		[Enum(Off, 0, On,1)]_Zwrite("ZWrite", Float) = 0
		_CurveFactor("CurveFactor", Float) = 1
	}

	SubShader
	{
		LOD 0

		
		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }
		
		Cull [_Float2]
		ZWrite[_Zwrite]
		AlphaToMask Off
		Lighting Off
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
				float4 ase_texcoord5 : TEXCOORD4;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _RimColor;
			float4 _MainTex_ST;
			float4 _DissolveTex_ST;
			float4 _MainColor;
			float4 _MainColorBack;
			float4 _DIssolveEdgeColor;
			float4 _MaskTex_ST;
			float2 _UVpanner_MainTex;
			float2 _UVpanner_DissolveTex;
			float2 _UVpanner_MaskTex;
			float _Rim_Width;
			float _Twist_Intensity;
			float _Dissolve_Smooth;
			float _Dissolve_Intensity;
			float _Dissolve_Edge;
			float _CurveFactor;
			fixed _UseVertexColor;
			float _UVpanner_MainTex_ParticleControl, _Dissolve_Intensity_ParticleControl, _Rim_On;
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

				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord3.xyz = ase_worldNormal;
				
				o.ase_texcoord4.xy = v.ase_texcoord.xy;
				o.ase_texcoord5 = v.ase_texcoord1;
				
				o.ase_color = v.ase_color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord3.w = 0;
				o.ase_texcoord4.zw = 0;
					float3 defaultVertexValue = float3(0, 0, 0);
				float3 vertexValue = defaultVertexValue;
					v.vertex.xyz += vertexValue;
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
				
				ase_vface = clamp(ase_vface,-1,1);
				float4 main_color = _MainColor;
				if(ase_vface < 0) main_color = _MainColorBack;
				
				float4 temp_cast_0 = (0.0).xxxx;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = IN.ase_texcoord3.xyz;
				float dotResult116 = clamp(dot( ase_worldViewDir , ( ase_worldNormal * ase_vface ) ),-1,1);
				float4 staticSwitch161 = ( pow( abs( 1.0 - dotResult116 +0.0001) , _Rim_Width ) * _RimColor )* _Rim_On + temp_cast_0*(1- _Rim_On);
				float2 uv_MainTex = IN.ase_texcoord4.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 appendResult86 = (float2(_UVpanner_MainTex.x , _UVpanner_MainTex.y));
				float2 appendResult100 = (float2(( uv_MainTex.x + IN.ase_texcoord5.z ) , ( uv_MainTex.y + IN.ase_texcoord5.w )));
				float2 staticSwitch96 = appendResult100* _UVpanner_MainTex_ParticleControl + (uv_MainTex + (appendResult86 * _TimeParameters.x))*(1- _UVpanner_MainTex_ParticleControl);
				float2 uv_DissolveTex = IN.ase_texcoord4.xy * _DissolveTex_ST.xy + _DissolveTex_ST.zw;
				float2 appendResult168 = (float2(_UVpanner_DissolveTex.x , _UVpanner_DissolveTex.y));
				float4 tex2DNode31 = tex2D( _DissolveTex, ( uv_DissolveTex + ( appendResult168 * _TimeParameters.x ) ) );
				float4 tex2DNode7 = tex2D( _MainTex, ( staticSwitch96 + ( _Twist_Intensity * tex2DNode31.r ) ) );
				float4 switchResult162 = ( tex2DNode7 * main_color );
				float temp_output_76_0 = ( 1.0 - _Dissolve_Smooth );
				float staticSwitch78 = IN.ase_texcoord5.x* _Dissolve_Intensity_ParticleControl + _Dissolve_Intensity*(1- _Dissolve_Intensity_ParticleControl);
				float smoothstepResult75 = smoothstep( temp_output_76_0 , _Dissolve_Smooth , ( tex2DNode31.r + 1.0 + ( staticSwitch78 * -2.0 ) ));
				float smoothstepResult177 = smoothstep( temp_output_76_0 , _Dissolve_Smooth , ( tex2DNode31.r + 1.0 + ( ( staticSwitch78 + _Dissolve_Edge ) * -2.0 ) ));
				
				float switchResult190 = main_color.a;
				float2 uv_MaskTex = IN.ase_texcoord4.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
				float2 appendResult109 = (float2(_UVpanner_MaskTex.x , _UVpanner_MaskTex.y));
				float4 tex2DNode30 = tex2D( _MaskTex, ( uv_MaskTex + ( appendResult109 * _TimeParameters.x ) ) );
				float clampResult42 = clamp( smoothstepResult75 , 0.0 , 1.0 );
				
				float3 BakedAlbedo = 0;
				float3 BakedEmission = 0;
				float3 Color = ( staticSwitch161 + ( ( switchResult162 + ( ( smoothstepResult75 - smoothstepResult177 ) * _DIssolveEdgeColor ) ) * IN.ase_color ) ).rgb;
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
				final = clamp(final, 0,4);
				return final;
			}

			ENDHLSL
		}
	}
	Fallback Off
}