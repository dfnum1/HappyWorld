
Shader "SD/Particles/SD_Post" {
    Properties {
        [HDR]_MainColor ("Main Color", Color) = (0.5,0.5,0.5,0.5)
        _MainTexture ("Main Texture", 2D) = "white" {}
        _fanwei ("fanwei", Range(0, 8)) = 3
        _pow ("pow", Range(0, 2)) = 1
        _Speed ("Speed", Range(0, 5)) = 1
		[MaterialEnum(Add,1, Blend, 10)]_BlendMode("BlendMode", Int) = 10
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Overlay+1"
            "RenderType"="Overlay"
        }
		LOD 100
        Pass {
            Blend SrcAlpha [_BlendMode]
            Cull Off
            ZTest Always
            ZWrite Off
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define _GLOSSYENV 1
           #include "Assets/Shaders/Includes/WorldCurvedCG.hlsl"
            uniform sampler2D _MainTexture;
          //  #include "UnityPBSLighting.cginc"
          //  #include "UnityStandardBRDF.cginc"
            CBUFFER_START(UnityPerMaterial)
            uniform float4 _MainColor;
            uniform float4 _MainTexture_ST;
            uniform float _fanwei;
            uniform float _pow;
            uniform float _Speed;
            CBUFFER_END
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                v.vertex.xyz = float3((o.uv0*2.0+-1.0),0.0);
                o.pos = v.vertex;
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float4 node_9441 = _Time;
                float node_8976_ang = node_9441.g;
                float node_8976_spd = _Speed;
                float node_8976_cos = cos(node_8976_spd*node_8976_ang);
                float node_8976_sin = sin(node_8976_spd*node_8976_ang);
                float2 node_8976_piv = float2(0.5,0.5);
                float2 node_9926 = (i.uv0*2.0+-1.0).rg;
                float node_5184 = (1-abs(atan2(node_9926.g,node_9926.r)/3.14159265359));
                float2 node_8976 = (mul(float2(node_5184,node_5184)-node_8976_piv,float2x2( node_8976_cos, -node_8976_sin, node_8976_sin, node_8976_cos))+node_8976_piv);
                float4 _MainTexture_var = tex2D(_MainTexture,TRANSFORM_TEX(node_8976, _MainTexture));
                float node_7943 = (saturate(pow((distance(i.uv0,float2(0.5,0.5))*1.6+0.0),exp2(_fanwei)))*_pow*_MainColor.a*i.vertexColor.a);
                float3 emissive = (_MainColor.rgb*_MainTexture_var.rgb*node_7943*(i.vertexColor.rgb*2.0));
                float3 finalColor = emissive;
                return fixed4(finalColor,(_MainTexture_var.a*node_7943));
            }
            ENDHLSL
        }
    }
    FallBack Off
}
