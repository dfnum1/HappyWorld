Shader "SD/Particles/SD_TreeAlphaBlend" 
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
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull Mode", Float) = 0
		[Enum(Off, 0, On,1)]_Zwrite("ZWrite", Float) = 0
    }
    SubShader
	{
        Tags { "Queue" = "Transparent+100"  "IgnoreProjector" = "True"}
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite[_Zwrite]
		LOD 100
		Cull[_Cull]
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
			uniform real4 _Texture_ST;
            uniform real4 _Color;
            uniform real _Fresnel;
            uniform real4 _Color_Fresnel;
			fixed _AmbientStrength, _CurveFactor;
        CBUFFER_END
            struct VertexInput 
			{
                real4 vertex : POSITION;
                real3 normal : NORMAL;
                real2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput
			{
                real4 pos : SV_POSITION;
                real2 uv0 : TEXCOORD0;
                real4 posWorld : TEXCOORD1;
                real3 normalDir : TEXCOORD2;
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
                real3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                real3 normalDirection = i.normalDir;
                real4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(i.uv0, _Texture));
                //clip((_Color.a*_Texture_var.a) - 0.5);

				Util_AmbientColor(_Texture_var, _AmbientStrength);

                real3 emissive = ((_Color.rgb*_Texture_var.rgb*2.0)+(pow(abs(1.0-max(0,dot(normalDirection, viewDirection))),_Fresnel)*_Color_Fresnel.rgb));
                real3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,_Color.a);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDHLSL
        }	
    }
    FallBack Off
}
