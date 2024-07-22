
Shader "SD/UI/FlowTexture" {
Properties {
    [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
	[HDR]_Color("Tint Color", Color) = (1,1,1,1)
    _NoiseTex("Noise Tex", 2D) = "white" {}
    _AnimationStemX("Animation Stem X",Range(0,2)) = 0
    _AnimationStemY("Animation Stem Y",Range(0,2)) = 0
    _AnimStrength("AnimStrength", range(0,0.1)) = 0

    [HideInInspector]_StencilComp("Stencil Comparison", Float) = 8
    [HideInInspector]_Stencil("Stencil ID", Float) = 0
    [HideInInspector]_StencilOp("Stencil Operation", Float) = 0
    [HideInInspector]_StencilWriteMask("Stencil Write Mask", Float) = 255
    [HideInInspector]_StencilReadMask("Stencil Read Mask", Float) = 255

    [HideInInspector]_ColorMask("Color Mask", Float) = 15
    [Toggle(USE_GRAY)] _UseGray("Use Gray", Float) = 0
    [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
}

SubShader {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Stencil
        {
            Ref[_Stencil]
            Comp[_StencilComp]
            Pass[_StencilOp]
            ReadMask[_StencilReadMask]
            WriteMask[_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest[unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask[_ColorMask]
    Pass {
        HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
            #pragma multi_compile __ USE_GRAY
            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                float2 noiseuv : TEXCOORD1;
                float4 worldPosition : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
			fixed4 _Color;

            float4 _ClipRect;

            float4	  _NoiseTex_ST;

            fixed _AnimStrength;
            fixed _AnimationStemX;
            fixed _AnimationStemY;
            CBUFFER_END
            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.worldPosition = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.noiseuv = TRANSFORM_TEX(v.texcoord, _NoiseTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half4 offsetColor = tex2D(_NoiseTex, i.noiseuv + float2(_AnimationStemX * _Time.x, _AnimationStemY * _Time.y));
                half2 uv_offset = 0;
                uv_offset.x = (offsetColor.r - 1) * _AnimStrength;
                uv_offset.y = (offsetColor.g - 1) * _AnimStrength;

                fixed4 col = tex2D(_MainTex, i.texcoord+ uv_offset)* _Color;

#ifdef UNITY_UI_CLIP_RECT
                col.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
#endif

#ifdef UNITY_UI_ALPHACLIP
                clip(col.a - 0.001);
#endif

#ifdef USE_GRAY
                float grey = dot(col.rgb, fixed3(0.22, 0.707, 0.071));
                return half4(grey, grey, grey, col.a);
#endif
                return col;
            }
        ENDHLSL
    }
}
FallBack Off
}
