Shader "SD/Particles/SD_PostScreenBroken"
{
	Properties
	{
		[HDR] _TintColor("Tint Color", Color) = (1,1,1,1)
		_ScreenBroken("ScreenBroken", 2D) = "white" {}
		_Strength("Strength", Range(0, 20)) = 0
		_scaleX("scaleX",Range(0,1)) = 0
		_scaleY("scaleY",Range(0,1)) = 0
		_Luminance("Luminance",Range(0,1)) = 0
	}

	SubShader
	{
	Tags { "RenderPipeline" = "UniversalPipeline" }
	LOD 100

	Pass
	{
		HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

		   #include "../Includes/CommonUtil.hlsl"
			#pragma multi_compile __ USE_CUSTOM

			struct appdata_t {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f {
				float4 pos: SV_POSITION;
				float4 screenPosition: TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _ScreenBroken;
			// 声明纹理
			TEXTURE2D(_CameraGrabTexture);
			// 声明采样器
			SAMPLER(sampler_CameraGrabTexture);

			CBUFFER_START(UnityPerMaterial)
		//	float4 _CameraGrabTexture_ST;
			float4 _ScreenBroken_ST;
			fixed _Strength;
			fixed4 _TintColor;
			fixed  _scaleX, _scaleY;
			fixed _Luminance;
			CBUFFER_END
			v2f vert(appdata_t v)
			{
				v2f o = (v2f)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				v.vertex.xyz = float3((v.texcoord.xy * 2.0 - 1.0), 0.0);
				o.pos = v.vertex;
				o.screenPosition = ComputeScreenPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				half2 uv = i.screenPosition.xy / i.screenPosition.w;
				half2 bumpUV = uv - 0.5;
				bumpUV *= half2(_scaleX, _scaleY);
				bumpUV += 0.5;
				half2 bump = UnpackNormal(tex2D(_ScreenBroken, bumpUV)).rg;
				uv = bump * _Strength + uv;
				half4 renderTex = SAMPLE_TEXTURE2D(_CameraGrabTexture, sampler_CameraGrabTexture, uv) * _TintColor;
				half4 lum = Luminance(renderTex);
				return lerp(renderTex, lum, _Luminance);
			}
		ENDHLSL
		}
	}
	FallBack Off
}
