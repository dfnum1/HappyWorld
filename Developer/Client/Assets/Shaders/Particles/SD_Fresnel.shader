Shader "SD/Particles/SD_Fresnel" {
    Properties {
		_AmbientStrength("Ambient Strength",Range(0,2)) = 0
	    [HDR]_TintColor ("GlowColor", Color) = (0,0,1,1)
        _GlowIntensity ("GlowIntensity", Float ) = 1       
        _Fresnel_value ("Fresnel_value", Float ) = 2		
		_CurveFactor("CurveFactor", Float) = 1
    }
    SubShader {
        Tags {  "IgnoreProjector"="True" "Queue"="Transparent+100"  "RenderType"="Transparent"   }     
		LOD 100
        Pass {
			Blend SrcAlpha One
			Fog { Mode Off }
            ZWrite Off            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#include "Assets/Shaders/Includes/WorldCurvedCG.hlsl"

			#pragma multi_compile __ FOG_LINEAR
            CBUFFER_START(UnityPerMaterial)
            uniform fixed4 _TintColor;         
            uniform fixed _Fresnel_value,_GlowIntensity;

			fixed _AmbientStrength, _CurveFactor;
            CBUFFER_END
            struct VertexInput {
                float4 vertex : POSITION;				
                float3 normal : NORMAL;
				fixed4 color  : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
				fixed4 color : COLOR;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;				
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(UNITY_MATRIX_M, v.vertex);
				CURVED_WORLD_TRANSFORM_POINT(v.vertex,_CurveFactor);
                o.pos = UnityObjectToClipPos(v.vertex );
				o.color = v.color;
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;      
                fixed rim = pow(1.0-clamp(dot(normalDirection, viewDirection),-0.999,0.999),_Fresnel_value);
                float4 finalColor = rim*_GlowIntensity*_TintColor*2 * i.color;			
				Util_AmbientColor(finalColor, _AmbientStrength);
                return finalColor;
            }
            ENDHLSL
        }
    }
    FallBack Off
}
