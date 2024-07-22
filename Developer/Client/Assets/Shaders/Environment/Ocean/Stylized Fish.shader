Shader "Distant Lands/Stylized Fish"
{
	Properties
	{
		_Atlas("Atlas", 2D) = "white" {}
		_WaveAmount("Wave Amount", Vector) = (0,0,0,0)
		_TimeScale("Time Scale", Vector) = (1,1,0,0)
		_WaveWidth("Wave Width", Vector) = (1,1,0,0)
	}

	SubShader
	{
		LOD 0
		Tags{ "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry+0" }
		Cull Off
		Pass
		{
			Name "Forward"
			Tags { "LightMode"="UniversalForward" }		
			HLSLPROGRAM
			#include "UnityCG.cginc"
			#pragma multi_compile_instancing
			#pragma vertex vert
            #pragma fragment frag
			
			struct appdata
			{
				float4 vertex : POSITION;
				float2 texcoord: TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			
			uniform sampler2D _Atlas;

			CBUFFER_START(UnityPerMaterial)
			uniform float2 _WaveWidth;
			uniform float2 _TimeScale;
			uniform float2 _WaveAmount;
			uniform float4 _Atlas_ST;
			CBUFFER_END

			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}


			v2f vert(appdata v)
			{
				v2f o = (v2f)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);				
				
				float3 ase_vertex3Pos = v.vertex.xyz;
				float temp_output_58_0 = abs( ase_vertex3Pos.z );
				float mulTime39 = _Time.y * _TimeScale.x;
				float3 ase_objectScale = float3( length( unity_ObjectToWorld[ 0 ].xyz ), length( unity_ObjectToWorld[ 1 ].xyz ), length( unity_ObjectToWorld[ 2 ].xyz ) );
				float3 temp_output_40_0 = ( ( ase_objectScale * float3( 50,50,50 ) ) + ( 0.0 - temp_output_58_0 ) );
				float3 rotatedValue64 = RotateAroundAxis( float3( 0,0,0 ), ase_vertex3Pos, float3(0,1,0), ( temp_output_58_0 * sin( ( _WaveWidth.x * ( mulTime39 + temp_output_40_0 ) ) ) * _WaveAmount.x ).x );
				float mulTime70 = _Time.y * _TimeScale.y;
				float3 rotatedValue60 = RotateAroundAxis( float3( 0,0,0 ), ase_vertex3Pos, float3(0,0,1), ( _WaveAmount.y * sin( ( _WaveWidth.y * ( mulTime70 + temp_output_40_0 ) ) ) * temp_output_58_0 ).x );
				v.vertex.xyz += ( ( rotatedValue64 - ase_vertex3Pos ) + ( rotatedValue60 - ase_vertex3Pos ) );
				v.vertex.w = 1;
				
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv_texcoord =TRANSFORM_TEX(v.texcoord,_Atlas);	
				return o;
			}

			half4 frag(v2f i): SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( i );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( i );
				
				half4 result = tex2D( _Atlas, i.uv_texcoord );
				result.a = 1;
				return result;
			}

			ENDHLSL
		}
	}
	Fallback Off
}