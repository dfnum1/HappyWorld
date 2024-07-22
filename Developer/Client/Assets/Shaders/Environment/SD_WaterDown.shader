Shader "SD/Environment/WaterDown" {
    Properties {
        _MainTex        ("颜色贴图", 2d) = "white"{}
        _WarpTex        ("扰动图", 2d) = "gray"{}
        _EmitTex    ("RGB:自发光", 2d)                = "black" {}
        _Speed          ("X：流速X Y：流速Y", vector) = (1.0, 1.0, 0.5, 1.0)
        _Warp1Params    ("X：大小 Y：流速X Z：流速Y W：强度", vector) = (1.0, 1.0, 0.5, 1.0)
        _Warp2Params    ("X：大小 Y：流速X Z：流速Y W：强度", vector) = (2.0, 0.5, 0.5, 1.0)
        _WarpController    ("WarpDir", vector) = (1.0, 0.0, 0.0, 0.0)

        [Header(Emission)]
            [HideInInspect] _EmitInt    ("自发光强度", range(1, 10))         = 1
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }


            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma target 3.0

            uniform sampler2D _MainTex;
            uniform sampler2D _WarpTex;
            uniform sampler2D _EmitTex;
            CBUFFER_START(UnityPerMaterial)
            // 输入参数
            uniform float4 _MainTex_ST;
            uniform half2 _Speed;
            uniform half4 _Warp1Params;
            uniform half4 _Warp2Params;
            uniform half2 _WarpController;

            uniform float _EmitInt;
            CBUFFER_END
            
            // 输入结构
            struct VertexInput {
                float4 vertex : POSITION;       // 顶点位置 总是必要
                float2 uv : TEXCOORD0;          // UV信息 采样贴图用
            };
            // 输出结构
            struct VertexOutput {
                float4 pos : SV_POSITION;       // 顶点位置 总是必要
                float2 uv0 : TEXCOORD0;         // UV信息 采样Mask
                float2 uv1 : TEXCOORD1;         // UV信息 采样Noise1
                float2 uv2 : TEXCOORD2;         // UV信息 采样Noise2
            };
            // 输入结构>>>顶点Shader>>>输出结构
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                    o.pos = UnityObjectToClipPos( v.vertex);    // 顶点位置 OS>CS
                    o.uv0 = (v.uv) - frac(_Time.x * _Speed);
                    o.uv1 = v.uv * _Warp1Params.x + frac(_Time.x * _Warp1Params.yz)* _WarpController.x;
                    o.uv2 = v.uv * _Warp2Params.x + frac(_Time.x * _Warp2Params.yz)* _WarpController.x;
                return o;
            }
            // 输出结构>>>像素
            float4 frag(VertexOutput i) : COLOR {
            //自发光和强度
                float3 var_EmitTex = tex2D(_EmitTex, i.uv0).rgb;
                float3 emission = var_EmitTex * _EmitInt ;//* (sin(_Time.x) * 0.5 + 0.5);


                half3 var_Warp1 = tex2D(_WarpTex, i.uv1).rgb;      // 扰动1
                half3 var_Warp2 = tex2D(_WarpTex, i.uv2).rgb;      // 扰动2
                // 扰动混合
                half2 warp = (var_Warp1.xy - 0.5) * _Warp1Params.w +
                             (var_Warp2.xy - 0.5) * _Warp2Params.w;
                // 扰动UV
                float2 warpUV = i.uv0 + warp;
                // 采样MainTex
                half4 var_MainTex = tex2D(_MainTex, warpUV);
                half3 finalRGB = var_MainTex * emission;
                return float4(finalRGB, 1.0);
            }
            ENDHLSL
        }
    }
    FallBack Off
}