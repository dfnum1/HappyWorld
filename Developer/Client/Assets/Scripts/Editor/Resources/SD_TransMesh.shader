Shader "SD/Editor/SD_TransMesh" {
Properties {
	[HDR]_Color ("Color", Color) = (0.5,0.5,0.5,0.5)
	[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull Mode", Float) = 0
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
            float4 _Color;
			struct appdata_t 
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float4 color : COLOR;	
			};

			v2f vert (appdata_t v)
			{
				v2f o= (v2f)0;
				o.color = v.color * _Color;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				return i.color;
			}
		ENDHLSL 	
		}
	}
}
FallBack Off
}
