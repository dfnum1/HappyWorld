Shader "Distant Lands/Stylized Dynamic Coral"
{
	Properties
	{
		[HDR] _TopColor1("Top Color", Color) = (0.3160377, 1, 0.695684, 1)
		[HDR] _MainColor1("Main Color", Color) = (0.3160377, 1, 0.695684, 1)
		[HDR] _Emmision1("Emmision", Color) = (0, 0, 0, 1)
		_GradientSmoothness2("Gradient Smoothness", Float) = 0.5
		_GradientOffset2("Gradient Offset", Float) = 0
		[HideInInspector] __dirty("", Int) = 1
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" "Queue"="Geometry" "IgnoreProjector"="True" "CanUseSpriteAtlas"="True" }
		Cull Back
		LOD 100

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO				
			};

			v2f vert(appdata v)
			{
				v2f o = (v2f)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);	
				
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.vertex.xyz;
				return o;
			}

			CBUFFER_START(UnityPerMaterial)
			fixed4 _MainColor1;
			fixed4 _TopColor1;
			float _GradientOffset2;
			float _GradientSmoothness2;
			fixed4 _Emmision1;
			CBUFFER_END

			fixed4 frag(v2f i) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( i );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( i );
				
				float3 ase_vertex3Pos = mul(unity_WorldToObject, float4(i.uv, 0));
				fixed4 lerpResult9 = lerp(_MainColor1, _TopColor1, saturate(((distance(ase_vertex3Pos, float3(0, 0, 0)) - _GradientOffset2) * _GradientSmoothness2)));
				fixed4 result;
				result.rgb = lerpResult9.rgb;
				result.a = 1;
				return result;
			}
			ENDHLSL
		}
		Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            #pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON

            // -------------------------------------
            // Universal Pipeline keywords

            // This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }
	}
	Fallback Off
}