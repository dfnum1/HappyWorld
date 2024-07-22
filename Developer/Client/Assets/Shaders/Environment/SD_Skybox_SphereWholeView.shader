Shader "Skybox/SphereWholeView" 
{
	Properties 
	{
		_Tint ("Tint Color", Color) = (.5, .5, .5, .5)
	//	_Exposure ("Exposure", Range(0, 1)) = 1.0
		_MainTex ("WholeView", 2D) = "grey" {}
		_Rotation("RotationPhi", Float) = 0
		_RotationTheta("RotationTheta", Float) = 0.5
		_FogToggle("EnableFog", Range(0, 1)) = 0.5
	}

	SubShader 
	{
		Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
		Cull Off ZWrite Off

		Pass {

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile __ FOG_LINEAR

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			CBUFFER_START(UnityPerMaterial)
				float4 _MainTex_ST;
				half4 _Tint;
			//	fixed _Exposure;
				fixed _FogToggle;
				fixed _Rotation;
				fixed _RotationTheta;
			CBUFFER_END

			struct appdata_t
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
			};

			v2f vert (appdata_t v)
			{
				float3 d = normalize(v.vertex.xyz);
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				float theta = 1- (acos(d.y) + _RotationTheta) * UNITY_INV_PI;
				float phi = 0.5 + (atan2(d.x, d.z) + _Rotation )* UNITY_INV_TWO_PI ;
				o.uv = float2(phi, theta);

				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				half4 tex = tex2D (_MainTex, TRANSFORM_TEX(i.uv, _MainTex))* _Tint;
				tex.a = _Tint.a;
				UNITY_APPLY_FOG(_FogToggle, tex);
				return tex;
			}
			ENDHLSL
		}
	}
	Fallback Off
}
