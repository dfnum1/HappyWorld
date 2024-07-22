Shader "SD/Environment/SD_GridCell" {
Properties 
	{
		_AmbientStrength("Ambient Strength",Range(0,2)) = 0
		[HDR]_TintColor("Tint Color", Color) = (0.5,0.5,0.5,0.5)
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
		Bind "Vertex", vertex
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

			CBUFFER_START(UnityPerMaterial)
            fixed4 _TintColor;
			fixed _AmbientStrength;
			fixed _CurveFactor;
			CBUFFER_END
			struct appdata_t 
			{
				float4 vertex : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			UNITY_INSTANCING_BUFFER_START(Props)
				UNITY_DEFINE_INSTANCED_PROP(float4, _ExternColor)
			UNITY_INSTANCING_BUFFER_END(Props)			

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f vert (appdata_t v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
#if INSTANCING_ON
				o.color = UNITY_ACCESS_INSTANCED_PROP(Props, _ExternColor);
#else
				o.color = fixed4(1,0,0,1);
#endif				
				CURVED_WORLD_TRANSFORM_POINT(v.vertex,_CurveFactor);
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{		
				fixed4 col = 2.0f * i.color;
				Util_AmbientColor(col, _AmbientStrength);
				return col;
			}
			ENDHLSL 			
		}
	}
}
FallBack Off
}
