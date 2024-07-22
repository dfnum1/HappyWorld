Shader "SD/Particles/SD_Alpha_Blended" {
Properties 
	{
		_AmbientStrength("Ambient Strength",Range(0,2)) = 0
		[HDR]_TintColor("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_CurveFactor("CurveFactor", Float) = 1
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull Mode", Float) = 0
		[Enum(Off, 0, On,1)]_Zwrite("ZWrite", Float) = 0
	}

Category{
	Tags { "Queue" = "Transparent+100"  "IgnoreProjector" = "True"}
	Blend SrcAlpha OneMinusSrcAlpha
	Cull[_Cull]
	Lighting Off
	ZWrite[_Zwrite]
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
			#pragma multi_compile_instancing
			#include "Assets/Shaders/Includes/WorldCurvedCG.hlsl"

            sampler2D _MainTex;

			CBUFFER_START(UnityPerMaterial)
            fixed4 _TintColor;
			float4 _MainTex_ST;
			fixed _AmbientStrength, _CurveFactor;
			CBUFFER_END
			struct appdata_t 
			{
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f vert (appdata_t v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
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
