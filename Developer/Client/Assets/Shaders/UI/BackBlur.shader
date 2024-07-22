Shader "SD/UI/BackBlur"
{
	Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        [HideInInspector]_StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector]_Stencil ("Stencil ID", Float) = 0
        [HideInInspector]_StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector]_StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector]_StencilReadMask ("Stencil Read Mask", Float) = 255

        [HideInInspector]_ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags{ "Queue" = "Transparent" }
      //  LOD 300
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
            Name "BlurBackground"
			Tags
            {
                // Specify LightMode correctly.
          //     "LightMode" = "GrabPass"
            }			
        HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

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
                float4 screenPosition : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };
            sampler2D _CameraGrabTexture;//_CameraGrabTexture;//_CameraColorTexture;//_CameraOpaqueTexture
            sampler2D _MainTex;

            CBUFFER_START(UnityPerMaterial)
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;
            CBUFFER_END
           
			

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.vertex = UnityObjectToClipPos(v.vertex);

                OUT.screenPosition = ComputeScreenPos(OUT.vertex);

                return OUT;
            }


            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = (tex2D(_CameraGrabTexture, IN.screenPosition.xy / IN.screenPosition.w) + _TextureSampleAdd);
				color.rgb *= _Color.rgb;
                //color.a = 0.99;
                return color;
            }
        ENDHLSL
        }
    }
	//SubShader
 //   {
 //       Tags{ "Queue" = "Transparent" }
 //       LOD 100
 //       Stencil
 //       {
 //           Ref [_Stencil]
 //           Comp [_StencilComp]
 //           Pass [_StencilOp]
 //           ReadMask [_StencilReadMask]
 //           WriteMask [_StencilWriteMask]
 //       }

 //       Cull Off
 //       Lighting Off
 //       ZWrite Off
 //       ZTest [unity_GUIZTestMode]
 //       Blend SrcAlpha OneMinusSrcAlpha
 //       ColorMask [_ColorMask]

 //       Pass
 //       {
 //           Name "BlurBackground"
	//		Tags
 //           {
 //               // Specify LightMode correctly.
 //         //     "LightMode" = "GrabPass"
 //           }			
 //       HLSLPROGRAM
 //           #pragma vertex vert
 //           #pragma fragment frag
 //           #pragma target 2.0

 //           #include "UnityCG.cginc"
 //           #include "UnityUI.cginc"

 //           struct appdata_t
 //           {
 //               float4 vertex   : POSITION;
 //               UNITY_VERTEX_INPUT_INSTANCE_ID
 //           };

 //           struct v2f
 //           {
 //               float4 vertex   : SV_POSITION;
 //               UNITY_VERTEX_OUTPUT_STEREO
 //           };

 //           v2f vert(appdata_t v)
 //           {
 //               v2f OUT;
 //               UNITY_SETUP_INSTANCE_ID(v);
 //               UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
 //               OUT.vertex = UnityObjectToClipPos(v.vertex);

 //               return OUT;
 //           }

 //           fixed4 _Color;
 //           fixed4 frag(v2f IN) : SV_Target
 //           {
	//			return fixed4(0,0,0,0.9);
 //           }
 //       ENDHLSL
 //       }
 //   }	
	Fallback Off
}