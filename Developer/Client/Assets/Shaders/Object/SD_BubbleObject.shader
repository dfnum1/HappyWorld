Shader "SD/Object/SD_BubbleObject"
{
	Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _WaveDirection("强度", Range(0, 1)) = 0.058
        _WaveAmplitude("振幅", Range(0, 1)) = 0.086
        _WaveWavelength("波浪强度", Range(0, 10)) = 4.4
        _WaveSpeed("波浪速度", Range(0, 10)) = 3.09
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

        Cull Back
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
			Name "Default"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"

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
                float4 _MainTex_ST;

                float _WaveDirection;
                float _WaveAmplitude;
                float _WaveWavelength;
                float _WaveSpeed;
            CBUFFER_END
            #define M_PI								3.14159265
            float SimpleWave(float3 position, float3 direction, float wavelength, float amplitude, float speed)
            {
                float x = M_PI * dot(position, direction) / wavelength;
                float phase = speed * _Time.y;
                return amplitude * sin(x + phase);
                return amplitude * (1 - abs(sin(x + phase)));
            }

            float GetWave(float3 worldPosition)
            {
                float3 dir = float3(cos(M_PI * _WaveDirection), sin(M_PI * _WaveDirection), cos(M_PI * _WaveDirection));
                float wave = SimpleWave(worldPosition, dir, _WaveWavelength, _WaveAmplitude, _WaveSpeed);
                return wave;
            }

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                float3 worldPosition = mul(UNITY_MATRIX_M, v.vertex).xyz;
                float wave = GetWave(worldPosition.xyz);
                worldPosition.xyz += float3(wave, wave,wave);
                OUT.vertex = mul(UNITY_MATRIX_VP, float4(worldPosition, 1));
                OUT.worldPosition = v.vertex;
                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = tex2D(_MainTex, IN.texcoord)* IN.color;
                return color;
            }
         ENDHLSL
        }
    }
    FallBack Off
}