Shader "SD/Object/CurvedUnlit_Alpha"
{ 
	Properties
	{
		[HDR]_MainColor("Main Color", Color) = (1.0, 1.0, 1.0, 1.0)
		//漫反射纹理
		_DiffuseTex("Diffuse Textrue", 2D) = "white" {}
		
		_CurveFactor("CurveFactor", Float) = 1		
		_AlphaDissolve("Alpha Dissolve(start,distance)", vector) = (3.5,3,0,0)
	}
	SubShader
	{
		Tags{ "IgnoreProjector" = "True""Queue" = "Transparent+100""RenderType" = "Opaque" }
		Pass
		{
			Cull back
			ZWrite Off
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
				float lengthInCamera : TEXCOORD3;
				float4 vertex : SV_POSITION;
			};

			sampler2D _DiffuseTex;

			CBUFFER_START(UnityPerMaterial)
			float4 _DiffuseTex_ST;
			float4 _MainColor;

			fixed2 _AlphaDissolve;
			CBUFFER_END
			v2f vert(appdata v)
			{
				v2f o = (v2f)0;
				V_CW_TransformPoint(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _DiffuseTex);

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color*_MainColor;
				o.color.a = Util_AlphaDissolve(v.vertex, _AlphaDissolve.x, _AlphaDissolve.y);
				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_DiffuseTex, i.uv) *i.color;
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}			
			ENDHLSL
		}
	}
	FallBack Off
}
