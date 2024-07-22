Shader "SD/Particles/SD_Mask_Ground" {
    Properties {
		_AmbientStrength("Ambient Strength",Range(0,2)) = 0
        [HDR]_Color ("Color", Color) = (0.5,0.5,0.5,1)
        [HDR]_Tex ("Tex", 2D) = "white" {}
        _Mask ("Mask", 2D) = "white" {}
        [MaterialToggle] _UseAnim ("Use Anim", Float ) = 0
        _Intensity ("Intensity", Range(-50, 50)) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
			_CurveFactor("CurveFactor", Float) = 1
    }
    SubShader
	{
        Tags 
		{
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
		LOD 100
        Pass 
		{
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#include "../Includes/WorldCurvedCG.hlsl"
            #pragma multi_compile_fog
            uniform sampler2D _Tex;
            uniform sampler2D _Mask;
            CBUFFER_START(UnityPerMaterial)
            uniform float4 _Tex_ST;
             uniform float4 _Mask_ST;
            uniform float4 _Color;
            uniform fixed _UseAnim;
            uniform fixed _Intensity;
			fixed _AmbientStrength, _CurveFactor;
            CBUFFER_END
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.vertexColor = v.vertexColor;
				CURVED_WORLD_TRANSFORM_POINT(v.vertex,_CurveFactor);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );

                float4 _Tex_var = tex2D(_Tex,TRANSFORM_TEX(i.uv0, _Tex));
                float node_9365 = 2.0;
                float3 emissive = (_Color.rgb*_Tex_var.rgb*i.vertexColor.rgb*node_9365);
                float3 finalColor = emissive;
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
				Util_AmbientColor(_Mask_var, _AmbientStrength);
                fixed4 finalRGBA = fixed4(finalColor,(saturate(pow(abs(_Mask_var.r),lerp( i.uv1.r, _Intensity, _UseAnim )))*_Tex_var.a*i.vertexColor.a*_Color.a*node_9365));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDHLSL
        }
    }
    FallBack Off
}
