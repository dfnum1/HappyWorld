Shader "Distant Lands/Stylized Glass"
{
	Properties
	{
		_Tint("Tint", Color) = (1, 1, 1, 0.4901961)
		_Smoothess("Smoothess", Float) = 0.5
		[HideInInspector] __dirty("", Int) = 1
	}

	SubShader
	{
		Tags { "RenderType"="Transparent" "RenderPipeline"="UniversalPipeline" "Queue"="Transparent" "IgnoreProjector"="True" "CanUseSpriteAtlas"="True" }
		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO				
			};
			CBUFFER_START(UnityPerMaterial)
			fixed4 _Tint;
			float _Smoothess;
			CBUFFER_END
			v2f vert(appdata v)
			{
				v2f o = (v2f)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);	
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = _Tint;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( i );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( i );			
				fixed4 result;
				result.rgb = i.color.rgb;
				result.a = _Tint.a;
				return result;
			}
			ENDHLSL
		}
	}
	Fallback Off
}