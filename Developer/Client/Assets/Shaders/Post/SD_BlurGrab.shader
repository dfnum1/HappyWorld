// 需要优化

Shader "SD/Post/SD_BlurGrab"
{
	Properties
    {
        _Distance("Sampling Distance", Range (0, 0.01)) = 0.005
        _Brightness("Brightness", Range (0, 1)) = 0.7
        _Iterations("Iteration", Range (1, 10)) = 3
         _Strength("Strength", Range(0, 1)) = 1
    }

    SubShader
    {
		Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
	//	LOD 300

        ZTest Always
        ZWrite Off
        Cull Off

        Pass
        {
			Name "Blit-Blur"
        HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "../Includes/BaseDefine/VertexBase.hlsl"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };
#if UNITY_VERSION >= 202000
			TEXTURE2D_X(_SourceTex);
            SAMPLER(sampler_SourceTex);
#else
            TEXTURE2D_X(_BlitTex);
            SAMPLER(sampler_BlitTex);
#endif
            
            CBUFFER_START(UnityPerMaterial)
            half _Distance;
            half _Brightness;
            int _Iterations;
            fixed _Strength;
            CBUFFER_END
            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.vertex = TransformObjectToHClip(v.vertex.xyz);
                OUT.texcoord = v.texcoord.xy;
                return OUT;
            }


            half4 frag(v2f IN) : SV_Target
            {
				//return fixed4(0,0,0,0.8);
				float2 uv = IN.texcoord.xy;
                half dist = _Distance * _Strength;
#if UNITY_VERSION >= 202000
                half4 color = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_SourceTex, uv);
                if (dist > 0)
                {
                    int xx = 0;
                    for (int x = -_Iterations; x < _Iterations; x++)
                    {
                        for (int y = -_Iterations; y < _Iterations; y++)
                        {
                            half2 pos = half2 (x * dist, y * _Distance);
                            color += SAMPLE_TEXTURE2D_X(_SourceTex, sampler_SourceTex, uv + pos);
                            xx++;
                        }
                    }
                    color.xyz = (color.xyz / xx) * _Brightness;
                }
#else
                half4 color = SAMPLE_TEXTURE2D_X(_BlitTex, sampler_BlitTex, uv);
                if (dist > 0)
                {
                    int xx = 0;
                    for (int x = -_Iterations; x < _Iterations; x++)
                    {
                        for (int y = -_Iterations; y < _Iterations; y++)
                        {
                            half2 pos = half2 (x * dist, y * _Distance);
                            color += SAMPLE_TEXTURE2D_X(_BlitTex, sampler_BlitTex, uv + pos);
                            xx++;
                        }
                    }
                    color.xyz = (color.xyz / xx) * _Brightness;
                }
#endif
                return color;
            }
        ENDHLSL
        }
    }
	FallBack Off
}