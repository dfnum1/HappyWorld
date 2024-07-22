Shader "SD/UI/MaskTexture" {
Properties {
   _MainTex ("Base (RGB)", 2D) = "white" {}
    _MaskTex("Mask", 2D) = "white" {}
    _ClipFactor("ClipFactor", range(0,1)) = 0.5
    _ZoomFactor("ZoomFactor", range(0,2)) = 1

	[HDR]_TintColor("Tint Color", Color) = (1,1,1,1)

    [HideInInspector]_StencilComp("Stencil Comparison", Float) = 8
    [HideInInspector]_Stencil("Stencil ID", Float) = 0
    [HideInInspector]_StencilOp("Stencil Operation", Float) = 0
    [HideInInspector]_StencilWriteMask("Stencil Write Mask", Float) = 255
    [HideInInspector]_StencilReadMask("Stencil Read Mask", Float) = 255

    [HideInInspector]_ColorMask("Color Mask", Float) = 15
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

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float4 texcoord : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex, _MaskTex;
            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST, _MaskTex_ST;
			fixed4 _TintColor;
            fixed _ClipFactor;
            fixed _ZoomFactor;
            CBUFFER_END
            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.texcoord.zw = TRANSFORM_TEX(v.texcoord, _MaskTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord.xy*_ZoomFactor)*_TintColor;
                col.a *= tex2D(_MaskTex, i.texcoord.zw).a;
                clip(col.a - _ClipFactor);
                return col;
            }
        ENDHLSL
    }
}
FallBack Off
}
