Shader "SD/Particles/SD_PostScreenColor"
{
	Properties
    {
        _Strength("强度", Range(0 , 1)) = 1
		_min("阈值", Range( 0 , 1)) = 0.5
		_width("黑白过渡（数值越小越卡通）", Range( 0 , 0.1)) = 0.001
		_flip("翻转黑白效果（0.5为界限）", Range( 0 , 1)) = 0.51
		_white_color("白色区域着色", Color) = (1,1,1,0)
		_black_color("黑色区域着色", Color) = (0,0,0,0)
		_Normal("Normal",vector) = (0.299, 0.587, 0.114,1)
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Float) = 8
        [MaterialToggle(USE_CUSTOM)]_UseCustom("Use ParticeSystem Custom", Int) = 0

    }
    SubShader
    {
		Tags { "RenderPipeline" = "UniversalPipeline" }
        HLSLINCLUDE
        #include "../Includes/CommonUtil.hlsl"

        CBUFFER_START(UnityPerMaterial)
     //   float4 _CameraGrabTexture_ST;
		uniform float4 _white_color;
		uniform float4 _black_color;
		uniform float _flip;
		uniform float _min;
		uniform float _width;
        uniform float _Strength;
		uniform float3 _Normal;
        CBUFFER_END

        ENDHLSL
        LOD 300
        Pass
        {
            // 开启深度测试 关闭剔除 关闭深度写入
            ZTest [_ZTest] Cull Off ZWrite Off

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile __ USE_CUSTOM

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
                float2 custom1 : TEXCOORD1;
            };

            v2f vert(a2v v)
            {
                v2f o = (v2f)0;

                //o.pos = TransformObjectToHClip(v.vertex.xyz);
                v.vertex.xyz = float3((v.texcoord.xy * 2.0-1.0), 0.0);
				o.pos = v.vertex;
                o.screenPosition = ComputeScreenPos(v.vertex);
#if USE_CUSTOM
                o.custom1 = v.texcoord.zw;
#endif

                return o;
            }

            half4 frag(v2f i): SV_Target
            {
                // 纹理采样
                half4 renderTex = SAMPLE_TEXTURE2D(_CameraGrabTexture, sampler_CameraGrabTexture, i.screenPosition.xy / i.screenPosition.w);

				float desaturateDot2 = dot(renderTex.xyz, _Normal);
				float3 desaturateVar2 = lerp( renderTex.xyz, desaturateDot2.xxx, 1.0 );
				float smoothstepResult9 = smoothstep( _min , ( _min + _width ) , desaturateVar2.x);
				float temp_output_8_0 = ( 1.0 - smoothstepResult9 );
				float ifLocalVar13 = 0;

#if USE_CUSTOM
                _flip = i.custom1.y;
#endif
                float thresort = step(_flip, 0.5);
                ifLocalVar13 = smoothstepResult9 * thresort + temp_output_8_0 *(1- thresort);
				float4 lerpResult18 = lerp( _white_color , _black_color , ifLocalVar13);
#if USE_CUSTOM
                _Strength = i.custom1.x;
#endif

                return lerp(renderTex, lerpResult18, _Strength);
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
            ZTest [_ZTest] Cull Off ZWrite Off

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
                v2f o = (v2f)0;

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
