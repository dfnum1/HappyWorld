Shader "SD/Environment/SD_IslandGrass"
{
	Properties
	{
		_MainTexture("MainTexture", 2D) = "white" {}
		_MainColor("MainColor", Color) = (1,1,1,0)
		_AO("AO", 2D) = "white" {}
		_WindMap("WindMap", 2D) = "white" {}
		_windPower("windPower", Range( 0 , 0.5)) = 0.01
		_WindSpeed("WindSpeed", Range( 0 , 2)) = 0.108998
		_offsetZ("offsetZ", Float) = 4.19
		_offsetX("offsetX", Float) = 4.19
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Pass
		{
			Tags{ "LightMode" = "UniversalForward" }
		HLSLPROGRAM
			#include "../Includes/CommonUtil.hlsl"
			#include "../Includes/WorldCurvedCG.hlsl"
			#pragma multi_compile_instancing
			#pragma vertex vert
			#pragma fragment frag
			struct appdata
			{
				float4 vertex: POSITION;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 pos			: SV_POSITION;
				float4 uv			: TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform sampler2D _WindMap;
			uniform sampler2D _MainTexture;
			uniform sampler2D _AO;
			CBUFFER_START(UnityPerMaterial)
				uniform real _WindSpeed;
				uniform real _offsetX;
				uniform real _offsetZ;
				uniform real _windPower;
				uniform real4 _MainColor;
				uniform real4 _MainTexture_ST;
				uniform real4 _AO_ST;
			CBUFFER_END

			v2f vert(appdata i)
			{
				v2f o = (v2f)0;
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);	
				
				real mulTime35 = _Time.y * _WindSpeed;
				real2 appendResult47 = (float2(( mulTime35 + _offsetX ) , ( mulTime35 + _offsetZ )));
				real2 uv_TexCoord34 = i.texcoord.xy * float2( -0.11,1 ) + appendResult47;
				real4 tex2DNode8 = tex2Dlod( _WindMap, float4( uv_TexCoord34, 0, 0.0) );
				real temp_output_27_0 = pow( abs(i.texcoord.xy.y), 2.95f );
				real temp_output_7_0 = ( (-1.0 + (tex2DNode8.g - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) * temp_output_27_0 * _windPower );
				real3 appendResult11 = (float3(temp_output_7_0 , ( (-1.0 + (tex2DNode8.g - 0.0) * (0.0 - -1.0) / (1.0 - 0.0)) * _windPower * temp_output_27_0 ) , temp_output_7_0));
				o.pos = UnityObjectToClipPos(i.vertex + float4(appendResult11,0));		
				o.uv.xy = TRANSFORM_TEX(i.texcoord,_MainTexture);	
				o.uv.zw = TRANSFORM_TEX(i.texcoord,_AO);	
				return o;
			}

			float4 frag(v2f i) :COLOR
			{
				float4 blendOpSrc48 = ( _MainColor * tex2D( _MainTexture, i.uv.xy ) );
				float4 blendOpDest48 = tex2D( _AO,  i.uv.zw );
				real4 finalColor = ( saturate( ( blendOpSrc48 * blendOpDest48 ) ));
				finalColor.a = 1;
				return finalColor;
			}
		ENDHLSL
		}
	}
	Fallback Off
}