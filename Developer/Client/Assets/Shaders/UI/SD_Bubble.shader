Shader "SD/UI/SD_Bubble"
{
	Properties
    {
       [PerRendererData] _MainTex ("Main Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _WaveDirection("强度", Range(0, 1)) = 0
        _WaveAmplitude("振幅", Range(0, 1)) = 0.53
        _WaveWavelength("波浪强度", Range(0, 10)) = 7.5
        _WaveSpeed("波浪速度", Range(0, 10)) = 0.25


        [HideInInspector]_StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector]_Stencil ("Stencil ID", Float) = 0
        [HideInInspector]_StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector]_StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector]_StencilReadMask ("Stencil Read Mask", Float) = 255

        [HideInInspector]_ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
			Name "Default"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
            #pragma multi_compile __ UNITY_UI_CLIP_RECT

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            CBUFFER_START(UnityPerMaterial)
                fixed4 _Color;
                float4 _ClipRect;
                float4 _MainTex_ST;

                float _WaveDirection;
                float _WaveAmplitude;
                float _WaveWavelength;
                float _WaveSpeed;
            CBUFFER_END
            #define M_PI								3.14159265
            float SimpleWave(float2 position, float2 direction, float wavelength, float amplitude, float speed)
            {
                float x = M_PI * dot(position, direction) / wavelength;
                float phase = speed * _Time.y;
                return amplitude * sin(x + phase);
                return amplitude * (1 - abs(sin(x + phase)));
            }

            float GetWave(float2 worldPosition)
            {
                float2 dir = float2(cos(M_PI * _WaveDirection), sin(M_PI * _WaveDirection));
                float wave = SimpleWave(worldPosition, dir, _WaveWavelength, _WaveAmplitude, _WaveSpeed);
                return wave;
            }

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                float3 worldPosition = mul(UNITY_MATRIX_M, v.vertex).xyz;
                float wave = GetWave(worldPosition.xy);
                worldPosition.xy += float2(wave, wave);
                OUT.vertex = mul(UNITY_MATRIX_VP, float4(worldPosition, 1));
                OUT.worldPosition = v.vertex;
                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = tex2D(_MainTex, IN.texcoord)* IN.color;
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                return color;
            }
         ENDHLSL
        }
    }
    FallBack Off
}