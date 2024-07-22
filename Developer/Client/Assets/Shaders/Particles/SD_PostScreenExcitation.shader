Shader "SD/Particles/SD_PostScreenExcitation"
{
	Properties
    {
        _DisplaceTex("DIsplacement", 2D) = "white" {}//噪点图
        _Magnitude("Magnitude", Range(0, 0.1)) = 1//偏移强度
        _Strength("strength", Range(0,1)) = 1//流动速度
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" }
        LOD 300
        HLSLINCLUDE
        #include "../Includes/CommonUtil.hlsl"

        sampler2D _DisplaceTex;
		CBUFFER_START(UnityPerMaterial)
            float _Magnitude;
            float _Strength;
        CBUFFER_END

        ENDHLSL

        Pass
        {
            // 开启深度测试 关闭剔除 关闭深度写入
            ZTest Off Cull Off ZWrite Off

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            // 声明纹理
            TEXTURE2D(_CameraGrabTexture);
            // 声明采样器
            SAMPLER(sampler_CameraGrabTexture);

            struct a2v
            {
                float4 vertex: POSITION;

                float4 texcoord: TEXCOORD0;
            };

            struct v2f
            {
                float4 pos: SV_POSITION;
                float4 screenPosition: TEXCOORD0;
            };

            v2f vert(a2v v)
            {
                v2f o;

                //o.pos = TransformObjectToHClip(v.vertex.xyz);
                v.vertex.xyz = float3((v.texcoord.xy * 2.0-1.0), 0.0);
				o.pos = v.vertex;
                o.screenPosition = ComputeScreenPos(v.vertex);

                return o;
            }

            half4 frag(v2f i): SV_Target
            {
				half2 uv = i.screenPosition.xy / i.screenPosition.w;
                float2 disp = tex2D(_DisplaceTex, uv - _Time.xy * _Strength).xy;
                disp = ((disp * 2) - 1) * _Magnitude;
				half4 renderTex = SAMPLE_TEXTURE2D(_CameraGrabTexture, sampler_CameraGrabTexture, uv +disp);
                return renderTex;
            }            
            ENDHLSL            
        }
    }
	SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" }
        LOD 100
        Pass
        {
            // 开启深度测试 关闭剔除 关闭深度写入
            ZTest Off Cull Off ZWrite Off

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "../Includes/CommonUtil.hlsl"

            struct a2v
            {
                float4 vertex: POSITION;
                float4 texcoord: TEXCOORD0;
            };

            struct v2f
            {
                float4 pos: SV_POSITION;
            };

            v2f vert(a2v v)
            {
                v2f o;

                //o.pos = TransformObjectToHClip(v.vertex.xyz);
                v.vertex.xyz = float3((v.texcoord.xy * 2.0-1.0), 0.0);
				o.pos = v.vertex;

                return o;
            }

            half4 frag(v2f i): SV_Target
            {
				discard;
				return half4(1,0,0,1);
            }            
            ENDHLSL            
        }
    }
    Fallback Off
}
