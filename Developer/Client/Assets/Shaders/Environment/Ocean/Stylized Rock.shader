Shader "Distant Lands/Stylized Rock"
{
	Properties
	{
		[MainColor] _BaseColor("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Off
		Pass
		{
			HLSLPROGRAM
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
			#pragma multi_compile_instancing
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma shader_feature _ALPHATEST_ON		
			#pragma vertex vert
			#pragma fragment frag
		
			
			CBUFFER_START(UnityPerMaterial)
			uniform float4 _BaseColor;
			CBUFFER_END

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent: TANGENT;				
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 vertexColor : COLOR;
				float3 worldNormal : TEXCOORD1;
				float3 worldPos: TEXCOORD2;
				float3 tangent: TEXCOORD3;
				float3 bitangent: TEXCOORD4;				
				float4 shadowCoord : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO	
			};
			
			VertexOutput vert(VertexInput v)
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);	
				
				float3 worldPos = TransformObjectToWorld(v.vertex.xyz);
					
				o.clipPos = TransformWorldToHClip(worldPos);
				o.vertexColor = v.color*_BaseColor;
				
				o.shadowCoord = TransformWorldToShadowCoord(worldPos);
				o.worldNormal =  normalize(mul((float3x3)UNITY_MATRIX_M, v.normal));
				o.worldPos = mul(UNITY_MATRIX_M, v.vertex).xyz;

				o.tangent = normalize(mul(UNITY_MATRIX_M, v.tangent).xyz);
				o.bitangent = normalize(cross(o.worldNormal, o.tangent.xyz));		
				return o;
			}

			half4 frag(VertexOutput i) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				half3 ambient = half3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w);
				half shadow = MainLightRealtimeShadow(i.shadowCoord);
				
				Light mainLight = GetMainLight();
				float3 lightDir = mainLight.direction;
				float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos);
				half3 refDir = reflect(-viewDir, i.worldNormal);
				half nl = dot(i.worldNormal, refDir);
	
				 half4 finalCol = i.vertexColor*nl;
				finalCol.rgb = lerp(finalCol.rgb * ambient.rgb, finalCol.rgb, shadow);
				return finalCol;
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
        Pass
        {
            Name "DepthOnly"
            Tags{"LightMode" = "DepthOnly"}

            ZWrite On
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            #pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5

            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
            ENDHLSL
        }		
	}
	Fallback Off
}