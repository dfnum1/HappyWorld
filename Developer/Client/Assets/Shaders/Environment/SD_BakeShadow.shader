Shader "SD/Environment/SD_BakeShadow"
{
	Properties
	{
		_MainTexture("MainTexture", 2D) = "white" {}
		_ShadowColor("ShadowColor", Color) = (0,0,0,1)
		_ShadowDistens("ShadowDistens", Range( -1 , 1)) = 0.3
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
		HLSLPROGRAM
			#include "../Includes/CommonUtil.hlsl"
			#include "../Includes/WorldCurvedCG.hlsl"	
			#pragma vertex vert
			#pragma fragment frag			
			#pragma multi_compile_instancing
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			
			uniform sampler2D _MainTexture;
			CBUFFER_START(UnityPerMaterial)
				uniform real4 _MainTexture_ST;
				uniform real _ShadowDistens;
				uniform real4 _ShadowColor;
			CBUFFER_END

			v2f vert(appdata i)
			{
				v2f o = (v2f)0;
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);	
				o.uv = TRANSFORM_TEX(i.uv, _MainTexture);
				o.pos = UnityObjectToClipPos(i.vertex);
				return o;
			}

			float4 frag(v2f i) :COLOR
			{
				real shadowR = tex2D( _MainTexture, i.uv).r;
				real temp_output_24_0 = step(shadowR , 0.57 );
				real4 col = ( _ShadowColor * temp_output_24_0 );
				
				real temp_output_25_0 = ( temp_output_24_0 * saturate( (1.0 + (distance( i.uv , float2( 0,1.09 ) ) - 0.0) * (_ShadowDistens - 1.0) / (1.0 - 0.0)) ) );
				col.a = _ShadowColor.a * temp_output_25_0;
				return col;
			}
		ENDHLSL	
		}
	}
	Fallback Off
}