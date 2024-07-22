Shader "Distant Lands/Stylized Fog"
{
	Properties
	{
		[HDR]_FogColor1("Fog Color 1", Color) = (1,0,0.8999224,1)
		[HDR]_FogColor2("Fog Color 2", Color) = (1,0,0,1)
		[HDR]_FogColor3("Fog Color 3", Color) = (1,0,0.7469492,1)
		[HDR]_FogColor4("Fog Color 4", Color) = (0,0.8501792,1,1)
		[HDR]_FogColor5("Fog Color 5", Color) = (0.164721,0,1,1)
		_ColorStart1("Color Start 1", Float) = 1
		_ColorStart2("Color Start 2", Float) = 2
		_ColorStart3("Color Start 3", Float) = 3
		_ColorStart4("Color Start 4", Float) = 4
		_FogDepthMultiplier("Fog Depth Multiplier", Float) = 1
		_LightFalloff("Light Falloff", Float) = 1
		LightIntensity("Light Intensity", Float) = 0
		[HDR]_LightColor("Light Color", Color) = (0,0,0,0)
		_FogSmoothness("Fog Smoothness", Float) = 0
		_FogIntensity("Fog Intensity", Float) = 1
		_FogOffset("Fog Offset", Float) = 1
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Pass
		{
			//ColorMask 0
			ZWrite On

			Tags{ "RenderType" = "UniversalForward"  "Queue" = "Transparent+1" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
			Cull Front
			ZWrite Off
			ZTest Always
			Blend SrcAlpha OneMinusSrcAlpha
			
			HLSLPROGRAM
			
			#include "../../Includes/CommonUtil.hlsl"
			struct Input
			{
				float4 screenPos;
				float3 worldPos;
			};
			#pragma vertex vert
			#pragma fragment frag	
			
			TEXTURE2D(_CameraGrabTexture);
			SAMPLER(sampler_CameraGrabTexture);
			
			sampler2D _CameraDepthTexture;
			sampler2D _CameraOpaqueTexture;

			CBUFFER_START(UnityPerMaterial)
			uniform float4 _FogColor1;
			uniform float4 _FogColor2;
			uniform float _FogDepthMultiplier;
			
			uniform float _ColorStart1;
			uniform float4 _FogColor3;
			uniform float _ColorStart2;
			uniform float4 _FogColor4;
			uniform float _ColorStart3;
			uniform float4 _FogColor5;
			uniform float _ColorStart4;
			uniform float4 _LightColor;
			uniform half LightIntensity;
			uniform half _LightFalloff;
			uniform float _FogSmoothness;
			uniform float _FogOffset;
			uniform float _FogIntensity;
			CBUFFER_END
			
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
				float4 screenPos : TEXCOORD2;
			};

			inline float4 ASE_ComputeGrabScreenPos( float4 pos )
			{
				#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
				#else
				float scale = 1.0;
				#endif
				float4 o = pos;
				o.y = pos.w * 0.5f;
				o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
				return o;
			}


			float3 HSVToRGB( float3 c )
			{
				float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
				float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
				return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
			}


			float3 RGBToHSV(float3 c)
			{
				float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
				float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
				float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
				float d = q.x - min( q.w, q.y );
				float e = 1.0e-10;
				return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
			}

			v2f vert(appdata v)
			{
				v2f o;
				float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(ase_clipPos);
				o.vertex = ase_clipPos;
				o.worldPos = TransformObjectToWorld(v.vertex.xyz);
				return o;
			}			

			float4 frag(v2f i) : SV_Target
			{
				float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
				float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
				float4 screenColor16 = tex2D(_CameraOpaqueTexture,ase_grabScreenPos.xy/ase_grabScreenPos.w);//SAMPLE_TEXTURE2D(_CameraGrabTexture, sampler_CameraGrabTexture,ase_grabScreenPos.xy/ase_grabScreenPos.w);
				float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float depthRaw = tex2D(_CameraDepthTexture, ase_screenPosNorm.xy).r;
				float depth = depthRaw;
				#ifdef UNITY_REVERSED_Z
					depth = (1.0 - depthRaw);
				#endif
				float eyeDepth80 = 1.0 / (_ZBufferParams.z * depthRaw + _ZBufferParams.w);//LinearEyeDepth(depthRaw);
				float temp_output_102_0 = ( _FogDepthMultiplier * sqrt( eyeDepth80 ) );
				float temp_output_1_0_g1 = temp_output_102_0;
				float4 lerpResult28_g1 = lerp( _FogColor1 , _FogColor2 , saturate( ( temp_output_1_0_g1 / _ColorStart1 ) ));
				float4 lerpResult41_g1 = lerp( saturate( lerpResult28_g1 ) , _FogColor3 , saturate( ( ( _ColorStart1 - temp_output_1_0_g1 ) / ( _ColorStart1 - _ColorStart2 ) ) ));
				float4 lerpResult35_g1 = lerp( lerpResult41_g1 , _FogColor4 , saturate( ( ( _ColorStart2 - temp_output_1_0_g1 ) / ( _ColorStart2 - _ColorStart3 ) ) ));
				float4 lerpResult113_g1 = lerp( lerpResult35_g1 , _FogColor5 , saturate( ( ( _ColorStart3 - temp_output_1_0_g1 ) / ( _ColorStart3 - _ColorStart4 ) ) ));
				float4 temp_output_81_0 = lerpResult113_g1;
				float3 hsvTorgb108 = RGBToHSV( _LightColor.rgb );
				float3 hsvTorgb113 = RGBToHSV( temp_output_81_0.rgb );
				float3 hsvTorgb110 = HSVToRGB( float3(hsvTorgb108.x,hsvTorgb108.y,( hsvTorgb108.z * hsvTorgb113.z )) );
				float3 ase_worldPos = i.worldPos;
				float3 normalizeResult84 = normalize( ( ase_worldPos - _WorldSpaceCameraPos ) );
				#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
				float3 ase_worldlightDir = 0;
				#else //aseld
				float3 ase_worldlightDir = normalize( _WorldSpaceCameraPos.xyz- ase_worldPos);
				#endif //aseld
				float dotResult83 = dot( normalizeResult84 , ase_worldlightDir );
				half LightMask95 = saturate( pow( abs( ( (dotResult83*0.5 + 0.5) * LightIntensity ) ) , _LightFalloff ) );
				float temp_output_52_0 = ( temp_output_81_0.a * saturate( temp_output_102_0 ) );
				float4 lerpResult97 = lerp( temp_output_81_0 , float4( hsvTorgb110 , 0.0 ) , saturate( ( LightMask95 * ( 1.5 * temp_output_52_0 ) ) ));
				float4 lerpResult17 = lerp( screenColor16 , lerpResult97 , temp_output_52_0);
				//o.Emission = lerpResult17.rgb;
				float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) ).xyz;
				float Alpha = saturate( ( ( 1.0 - saturate( ( ( ase_vertex3Pos.y * ( 1.0 / _FogSmoothness ) ) + ( 1.0 - _FogOffset ) ) ) ) * _FogIntensity ) );
				
				float4 finalColor = lerpResult17;
				finalColor.a = Alpha;
				return finalColor;
			}

			ENDHLSL
		}
	}
	Fallback Off
}