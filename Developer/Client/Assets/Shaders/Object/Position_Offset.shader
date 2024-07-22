Shader "SD/Object/Position_Offset" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _Size ("Size", Float ) = 0.025
        _FallOff ("Fall Off", Range(0, 2)) = 1
        _Edge ("Edge", Range(0, 15)) = 0
		_OffsetSpeed("OffsetSpeed",Range(0, 1)) = 0
		_Amplitude("Amplitude",Range(0, 90)) = 0
		_CurveFactor("CurveFactor", Float) = 1
    }
    SubShader {
        Tags {"IgnoreProjector"="True""Queue"="Transparent+2""RenderType"="Transparent"}
        Pass {
            Name "FORWARD"
            Blend One One
            ZWrite Off
            Cull Off Lighting Off  
            HLSLPROGRAM
			#pragma fragmentoption ARB_precision_hint_fastest//用低精度（一般是指fp16），以提升片段着色器的运行速度，减少时间
            #pragma vertex vert
            #pragma fragment frag
			#include "Assets/Shaders/Includes/WorldCurvedCG.hlsl"

            CBUFFER_START(UnityPerMaterial)
            uniform float4 _Color;
            uniform float _FallOff;
            uniform float _Size;
            uniform float _Edge;
			
			uniform fixed _OffsetSpeed;
			uniform fixed _Amplitude, _CurveFactor;
            CBUFFER_END
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
				//Y轴上下位移
				float4 Offset = float4(0,0,0,0);
                Offset.y = sin(3.1416 * _Time.y )  * _OffsetSpeed;
				
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                v.vertex.xyz += (v.normal*(_Size*0.02+0.01));
                o.posWorld = mul(UNITY_MATRIX_M, v.vertex);
				o.pos = v.vertex+Offset;
				CURVED_WORLD_TRANSFORM_POINT(o.pos,_CurveFactor);
				o.pos = UnityObjectToClipPos(o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
              	float t =abs(cos(_Time.x*_Amplitude));
				float3 finalColor = (_Color.rgb*_FallOff*pow(saturate(dot(i.normalDir,viewDirection))*t,_Edge)); // Final Glow
                return fixed4(finalColor,1);
            }
            ENDHLSL
        }
     
    }
    FallBack Off
   
}
