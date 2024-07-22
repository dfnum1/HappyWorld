Shader "SD/Particles/SD_Additive_TwoSide" {
Properties {
	_AmbientStrength("Ambient Strength",Range(0,2)) = 0
	[HDR]_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	_MainTex ("Particle Texture", 2D) = "white" {}
	[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull Mode", Float) = 0
	_CurveFactor("CurveFactor", Float) = 1
	[Enum(Off, 0, On,1)]_Zwrite("ZWrite", Float) = 0
}

Category {
	Tags { "Queue"="Transparent+100" "IgnoreProjector"="True" "RenderType"="BloomLayer_AdditiveTwoSize" }
	Blend SrcAlpha One
	//AlphaTest Greater.01// 只渲染透明度大于.01的像素
	ColorMask RGB
	Cull [_Cull]
	Lighting Off 
	ZWrite[_Zwrite]
	Fog { Mode Off }
	LOD 100
	SubShader 
	{
		Pass 
		{
		HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#include "Assets/Shaders/Includes/WorldCurvedCG.hlsl"

            sampler2D _MainTex;
			CBUFFER_START(UnityPerMaterial)
            fixed4 _TintColor;
			fixed _AmbientStrength, _CurveFactor;
			float4 _MainTex_ST;
			CBUFFER_END
			struct appdata_t 
			{
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;	
			};

			v2f vert (appdata_t v)
			{
				v2f o= (v2f)0;
				o.color = v.color * _TintColor;
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);	

				CURVED_WORLD_TRANSFORM_POINT(v.vertex,_CurveFactor);
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{		
				fixed4 col = 2.0f * i.color * tex2D(_MainTex, i.texcoord);			
				Util_AmbientColor(col, _AmbientStrength);
				return col;
			}
		ENDHLSL 	
		}
	}
}
FallBack Off
}
