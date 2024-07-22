Shader "Hidden/HL/Role/GPUSkinningData"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_AnimMap ("AnimMap", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform fixed _FrameData[200];
			uniform fixed2 _FrameStarEnd[4];

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _AnimMap;
			float4 _AnimMap_TexelSize;//x = 1/width
			uniform float4 _TimeEditor;
			v2f vert (appdata v, uint vid : SV_VertexID)
			{
				float f = _TimeEditor.y;
				fmod(f, 1.0);
				float animMap_x = (vid + 0.5) * _AnimMap_TexelSize.x;

				float animMap_y = f;// (f*(_AnimLen) + _AnimOffset)*_AnimMap_TexelSize.y;
				fmod(animMap_y, 1.0);

				float4 pos = tex2Dlod(_AnimMap, float4(animMap_x, animMap_y, 0, 0));

				v2f o;
				o.vertex = UnityObjectToClipPos(pos);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
