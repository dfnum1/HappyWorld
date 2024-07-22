Shader "SD/Particles/SD_Tree" 
{
    Properties 
	{
		_AmbientStrength("Ambient Strength",Range(0,2)) = 0
	    [HDR]_Color ("Color", Color) = (0.5,0.5,0.5,1)
        _Texture ("Texture", 2D) = "white" {}
		[HDR]_Color_Fresnel ("Color_Fresnel", Color) = (0.5,0.5,0.5,1)
        _Fresnel ("Fresnel", Float ) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
			_CurveFactor("CurveFactor", Float) = 1
    }
    SubShader
	{
        Tags
		{
			"RenderType"="Opaque"
            "Queue"="Transparent+100"
			"IgnoreProjector" = "True"
        }
		LOD 100
		Cull Back
		Lighting Off
        Pass 
		{
            HLSLPROGRAM
			#include "Assets/Shaders/Includes/WorldCurvedCG.hlsl"
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            uniform sampler2D _Texture;
        CBUFFER_START(UnityPerMaterial)
			uniform float4 _Texture_ST;
            uniform float4 _Color;
            uniform float _Fresnel;
            uniform float4 _Color_Fresnel;
			fixed _AmbientStrength, _CurveFactor;
        CBUFFER_END
            struct VertexInput 
			{
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput
			{
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                UNITY_FOG_COORDS(3)
            };
            VertexOutput vert (VertexInput v)
			{
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(UNITY_MATRIX_M, v.vertex);
				CURVED_WORLD_TRANSFORM_POINT(v.vertex,_CurveFactor);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR 
			{
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(i.uv0, _Texture));
                clip((_Color.a*_Texture_var.a) - 0.5);

				Util_AmbientColor(_Texture_var, _AmbientStrength);

                float3 emissive = ((_Color.rgb*_Texture_var.rgb*2.0)+(pow(abs(1.0-max(0,dot(normalDirection, viewDirection))),_Fresnel)*_Color_Fresnel.rgb));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDHLSL
        }	
    }
    FallBack Off
}
