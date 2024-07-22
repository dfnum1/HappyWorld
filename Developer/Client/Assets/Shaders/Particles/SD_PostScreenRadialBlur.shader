Shader "SD/Particles/SD_PostScreenRadialBlur"
{
	Properties
    {
		_BlurStrength("Blur Strength",Range( 0 , 5)) = 2.2
		_BlurWidth("Blur Width",Range( 0 , 5)) = 1
		_Center("Center",vector) = (0.5,0.5,1,1)
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" }
        LOD 300
        HLSLINCLUDE
        #include "../Includes/CommonUtil.hlsl"

		CBUFFER_START(UnityPerMaterial)
			uniform half _BlurStrength;
			uniform half _BlurWidth;
			half2 _Center;
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
				half4 renderTex = SAMPLE_TEXTURE2D(_CameraGrabTexture, sampler_CameraGrabTexture, uv);
				half samples[10] = {-0.08,-0.05,-0.03,-0.02,-0.01,0.01,0.01,0.03,0.05,0.08};

				//vector to the middle of the screen
                half2 dir = _Center - uv;
       
                //distance to center
                half dist = length(dir);
       
                //normalize direction
                dir = dir/dist;
       
                //additional samples towards center of screen
                half4 sum = renderTex;
                for(int n = 0; n < 10; n++)
                {
                    sum += SAMPLE_TEXTURE2D(_CameraGrabTexture, sampler_CameraGrabTexture, uv + dir * samples[n] * _BlurWidth);
                }
       
                //eleven samples...
                sum *= 1.0/11.0;
       
                //weighten blur depending on distance to screen center
                /*
				half t = dist * _BlurStrength / _imgWidth;
				t = clamp(t, 0.0, 1.0);
				*/		
                half t = saturate(dist * _BlurStrength);
       
                //blend original with blur
                return lerp(renderTex, sum, t);
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
                float4 screenPosition: TEXCOORD0;
            };

            v2f vert(a2v v)
            {
                v2f o = (v2f)0;
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
