Shader "SD/Environment/SD_IslandCellMask" {
Properties 
	{
		_AmbientStrength("Ambient Strength",Range(0,2)) = 0
		[HDR]_TintColor("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("FogTexture", 2D) = "white" {}
		_MaskTex("Mask", 2D) = "back"{}
		_CurveFactor("CurveFactor", Float) = 1
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull Mode", Float) = 0
	}

Category{
	Tags { "Queue" = "Transparent+100"  "IgnoreProjector" = "True"}
	Blend SrcAlpha OneMinusSrcAlpha
	Cull[_Cull]
	Lighting Off
	ZWrite Off
	Fog { Mode Off }	
	LOD 100
	BindChannels 
	{
		Bind "Color", color
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
	}

	SubShader 
	{
		Pass 
		{
	        HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#include "Assets/Shaders/Includes/WorldCurvedCG.hlsl"

            sampler2D _MainTex;
			sampler2D _MaskTex;

			CBUFFER_START(UnityPerMaterial)
            fixed4 _TintColor;
			float4 _MainTex_ST;
			fixed _AmbientStrength;
			fixed _CurveFactor;
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
				float2 maskCoord : TEXCOORD1;
			};


			v2f vert (appdata_t v)
			{
				v2f o;
				o.color = v.color * _TintColor;
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);	
				o.maskCoord = v.texcoord;

				CURVED_WORLD_TRANSFORM_POINT(v.vertex,_CurveFactor);
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{		
				fixed4 col = 2.0f * i.color * tex2D(_MainTex, i.texcoord);
				col.a = tex2D(_MaskTex, i.maskCoord).a;
				return col;
			}
			ENDHLSL 			
		}
	}
}
FallBack Off
}
