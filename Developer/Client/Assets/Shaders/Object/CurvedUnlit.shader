Shader "SD/Object/CurvedUnlit"
{ 
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_CurveFactor("CurveFactor", Float) = 1	
		_Alpha("Alpha", Range(0,1)) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque"}
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
				
			#include "Assets/Shaders/Includes/WorldCurvedCG.hlsl"
			
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 color : TEXCOORD2;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;

			CBUFFER_START(UnityPerMaterial)
			float4 _MainTex_ST;
			float _Alpha;
			CBUFFER_END
			v2f vert(appdata v)
			{
				v2f o;
				V_CW_TransformPoint(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv) * i.color;
				UNITY_APPLY_FOG(i.fogCoord, col);
				col.a = _Alpha;
				return col;
			}			
			ENDHLSL
		}
	}
	FallBack Off
}
