Shader "SD/Fogs/HeightFogGlobal"
{
	Properties
	{
		[HideInInspector] _MainTex0("Texture", 2D) = "white" {}
		_DFogColorStart("DFog Color Start", Color) = (0.01,0.5,1,1)
		//_DFogColorFar("DFog Color Far", Color) = (0.01,0.5,1,1)
		//_FogColorDuo("FogColorDuo",float) = 1.0

		_FogDistanceStart("FogDistanceStart",float) = 0.1
		_FogDistanceEnd("FogDistanceEnd",float) = 0.1
		_DistanceFallOff("DistanceFallOff",float) = 0.1
		_FogDensity("FogDensity",float) = 0.5
		_ContrastTone("ContrastTone",float) = 1.0
			
		_CamearRangeMin("CamearRangeMin",float) = 0.0
		_CamearRangeMax("CamearRangeMax",float) = 0.0
		_FogHeightThinOff("FogHeightThinOff",float) = 0.0
		_FogHeightThickOff("FogHeightThickOff",float) = 0.0
		_FogHeightFallOff("FogHeightFallOff",float) = 0.0
			
		_DirectionalIntensity("Directional Intensity", float) = 1
		_DirectionalFalloff("Directional Falloff", float) = 2
		_DirectionalOcclusion("Directional Occulsion", float) = 0.5
		_DirectionalColor("Directional Color", Color) = (1,0.7793103,0.5,1)

		_SkyboxFogIntensity("Skybox Fog Intensity", float) = 1
		_SkyboxFogHeight("Skybox Fog Height", float) = 1
		_SkyboxFogFill("Skybox Fog Fill", float) = 1

		_NoiseModeBlend("NoiseModeBlend",float) = 1.
		_NoiseIntensity("NoiseIntensity",float) = 1.
		_NoiseDistanceEnd("NoiseDistanceEnd",float) = 1.0
		_NoiseScale("NoiseScale",float) = 1.0
		_NoiseSpeed("NoiseSpeed",Vector) = (0.5,0,0.5,0)
	}
	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "IgnoreProjector" = "true"}
		Fog { Mode Off }

		Blend SrcAlpha OneMinusSrcAlpha
		Cull Front
		ColorMask RGBA
		ZWrite Off
		ZTest Always
		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"
			#pragma multi_compile AHF_NOISEMODE_OFF AHF_NOISEMODE_PROCEDURAL3D
			//#include "UnityLightingCommon.cginc"
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD2;
			};

			sampler2D _CameraDepthTexture;
			CBUFFER_START(UnityPerMaterial)
			//uniform float4 _MainTex_TexelSize;
			float  _FogDensity, _DistanceFallOff, _ContrastTone;
			float _NoiseModeBlend,_NoiseIntensity, _NoiseDistanceEnd, _NoiseScale;
			float3 _NoiseSpeed;
			float _DirectionalIntensity, _DirectionalFalloff;
			float3 _DirectionalColor;
			uniform float _SkyboxFogIntensity;
			uniform float _SkyboxFogHeight;
			uniform float _SkyboxFogFill;
			float _CamearRangeMin;
			float _CamearRangeMax;
			float _FogHeightThinOff;
			float _FogHeightThickOff;
			float _FogHeightFallOff;
			float3 _DFogColorStart;
			float _DirectionalOcclusion;
			float _FogDistanceStart;
			float _FogDistanceEnd;
			//float _FogColorDuo;
		    //float3 _DFogColorFar;
			CBUFFER_END
			float2 UnStereo(float2 UV)
			{
#if UNITY_SINGLE_PASS_STEREO
				float4 scaleOffset = unity_StereoScaleOffset[unity_StereoEyeIndex];
				UV.xy = (UV.xy - scaleOffset.zw) / scaleOffset.xy;
#endif
				return UV;
			}

			float3 mod3D289(float3 x) { return x - floor(x / 289.0) * 289.0; }
			float4 mod3D289(float4 x) { return x - floor(x / 289.0) * 289.0; }
			float4 permute(float4 x) { return mod3D289((x * 34.0 + 1.0) * x); }
			float4 taylorInvSqrt(float4 r) { return 1.79284291400159 - r * 0.85373472095314; }
			float snoise(float3 v)
			{
				const float2 C = float2(1.0 / 6.0, 1.0 / 3.0);
				float3 i = floor(v + dot(v, C.yyy));
				float3 x0 = v - i + dot(i, C.xxx);
				float3 g = step(x0.yzx, x0.xyz);
				float3 l = 1.0 - g;
				float3 i1 = min(g.xyz, l.zxy);
				float3 i2 = max(g.xyz, l.zxy);
				float3 x1 = x0 - i1 + C.xxx;
				float3 x2 = x0 - i2 + C.yyy;
				float3 x3 = x0 - 0.5;
				i = mod3D289(i);
				float4 p = permute(permute(permute(i.z + float4(0.0, i1.z, i2.z, 1.0)) + i.y + float4(0.0, i1.y, i2.y, 1.0)) + i.x + float4(0.0, i1.x, i2.x, 1.0));
				float4 j = p - 49.0 * floor(p / 49.0);  // mod(p,7*7)
				float4 x_ = floor(j / 7.0);
				float4 y_ = floor(j - 7.0 * x_);  // mod(j,N)
				float4 x = (x_ * 2.0 + 0.5) / 7.0 - 1.0;
				float4 y = (y_ * 2.0 + 0.5) / 7.0 - 1.0;
				float4 h = 1.0 - abs(x) - abs(y);
				float4 b0 = float4(x.xy, y.xy);
				float4 b1 = float4(x.zw, y.zw);
				float4 s0 = floor(b0) * 2.0 + 1.0;
				float4 s1 = floor(b1) * 2.0 + 1.0;
				float4 sh = -step(h, 0.0);
				float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
				float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
				float3 g0 = float3(a0.xy, h.x);
				float3 g1 = float3(a0.zw, h.y);
				float3 g2 = float3(a1.xy, h.z);
				float3 g3 = float3(a1.zw, h.w);
				float4 norm = taylorInvSqrt(float4(dot(g0, g0), dot(g1, g1), dot(g2, g2), dot(g3, g3)));
				g0 *= norm.x;
				g1 *= norm.y;
				g2 *= norm.z;
				g3 *= norm.w;
				float4 m = max(0.6 - float4(dot(x0, x0), dot(x1, x1), dot(x2, x2), dot(x3, x3)), 0.0);
				m = m * m;
				m = m * m;
				float4 px = float4(dot(x0, g0), dot(x1, g1), dot(x2, g2), dot(x3, g3));
				return 42.0 * dot(m, px);
			}

			float Remap(float from, float fromMin, float fromMax, float toMin, float toMax)
			{
				float fromAbs = from - fromMin;
				float fromMaxAbs = fromMax - fromMin;

				float normal = fromAbs / fromMaxAbs;

				float toMaxAbs = toMax - toMin;
				float toAbs = toMaxAbs * normal;

				float to = toAbs + toMin;

				return to;
			}

			v2f vert(appdata v)
			{
				v2f o;
				float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(ase_clipPos);
				o.vertex = ase_clipPos;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float2 uv = i.screenPos.xy / i.screenPos.w;
				//float uvz = (UNITY_NEAR_CLIP_VALUE >= 0) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float depthRaw = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, uv));
				float depth = depthRaw;
				#ifdef UNITY_REVERSED_Z
					depth = (1.0 - depthRaw);
				#endif
				float linearDepth01 = Linear01Depth(depthRaw);

				float linearEyeDepth = LinearEyeDepth(depthRaw);
				float2 ScreenUnStereo235_NormalXY = UnStereo(uv);
				float3 appendResult244_g1045 = (float3(ScreenUnStereo235_NormalXY.x, ScreenUnStereo235_NormalXY.y, depth));
				float4 appendResult220_g1045 = (float4((appendResult244_g1045 * 2.0 + -1.0), 1.0));
				float4 break229_g1045 = mul(unity_CameraInvProjection, appendResult220_g1045);
				float3 appendResult237_g1045 = (float3(break229_g1045.x, break229_g1045.y, break229_g1045.z));
				float4 appendResult233_g1045 = (float4(((appendResult237_g1045 / break229_g1045.w) * float3(1, 1, -1)), 1.0));
				float4 break245_g1045 = mul(unity_CameraToWorld, appendResult233_g1045);
				float3 worldPos = (float3(break245_g1045.x, break245_g1045.y, break245_g1045.z));

				float distanceView = distance(worldPos, _WorldSpaceCameraPos);
				float FogZEdge = saturate(step(_ProjectionParams.z * 0.95, distanceView));
				float3 normalize_WorldPosition = normalize(float3(worldPos.x, worldPos.y + distanceView * _SkyboxFogHeight, worldPos.z));
				float saferPower309_g1045 = max(saturate((abs(normalize_WorldPosition.y) - 1.0) / (0 - 1.0)), 0.0001) * FogZEdge;
				float lerpResult179_g1045 = lerp(pow(saferPower309_g1045, 4.0), 1.0, _SkyboxFogFill * FogZEdge);
				float SkyboxFogHeightMask = saturate(lerpResult179_g1045 * _SkyboxFogIntensity)+saturate(-normalize_WorldPosition.y) * FogZEdge;
				//return float4(SkyboxFogHeightMask, SkyboxFogHeightMask, SkyboxFogHeightMask, 1.0);

				float heightRegulator = (1.0-saturate((_WorldSpaceCameraPos.y - _CamearRangeMin) / (_CamearRangeMax - _CamearRangeMin)));
				float HeightLerp = saturate((worldPos.y - _FogHeightThinOff) / (_FogHeightThickOff - _FogHeightThinOff));
				float HeightDensity = pow(HeightLerp, _FogHeightFallOff);
				//HeightDensity = HeightDensity == 1.0 ? 0.0 : HeightDensity;
				float UE4FogDensity = exp(-_FogHeightFallOff * HeightLerp);
				float FogFactor = lerp(saturate((1 - exp2(-UE4FogDensity)) / UE4FogDensity) * HeightDensity ,HeightDensity, _FogHeightFallOff*0.1);
				FogFactor = lerp(FogFactor, SkyboxFogHeightMask, FogZEdge);

				//float FogDistanceMask12_g1045 = pow(abs(saturate(((distanceView - _FogDistanceStart) / (_FogDistanceEnd - _FogDistanceStart)))), _DirectionalFalloff);
				//float3 lerpResult258_Color = lerp((_DFogColorStart).rgb, (_DFogColorFar).rgb, saturate((FogZEdge+ linearDepth01)*0.5 * _FogColorDuo));
				float3 normalizeResult318_ViewDist = normalize(worldPos - _WorldSpaceCameraPos);
				float dotResult145_LV = dot(normalizeResult318_ViewDist, _WorldSpaceLightPos0.xyz);
				float appendResult233_g8762 = dot(_WorldSpaceLightPos0.xyz, float3(0, 1, 0));
				float nolinearDepth01 = pow(linearDepth01, _DirectionalOcclusion*4.0);
				appendResult233_g8762 = smoothstep(-0.1, 0.05, appendResult233_g8762)* nolinearDepth01;
				float DirectionalMask30_g1045 = saturate(pow(abs(((dotResult145_LV* 0.5 + 0.5) * _DirectionalIntensity)), _DirectionalFalloff))* appendResult233_g8762;
				float3 _DFogColor = lerp(_DFogColorStart, _DirectionalColor, DirectionalMask30_g1045);

				float factor = (linearEyeDepth - _FogDistanceStart) / (_FogDistanceEnd - _FogDistanceStart);
				factor = saturate(factor);
				factor = smoothstep(0, _DistanceFallOff,factor)+ heightRegulator* factor;

#if defined(AHF_NOISEMODE_PROCEDURAL3D)
				float simplePerlin3D193_g1045 = snoise(((worldPos * (1.0 / _NoiseScale)) + (-_NoiseSpeed * _Time.y)));
				float NoiseDistanceMask7_g1045 = saturate(((distanceView - _NoiseDistanceEnd) / (0.0 - _NoiseDistanceEnd)));
				float NoiseSimplex3D24_g1045 = lerp(1.0, (simplePerlin3D193_g1045 * 0.5 + 0.5), (NoiseDistanceMask7_g1045 * _NoiseIntensity * _NoiseModeBlend));
				factor = (factor * NoiseSimplex3D24_g1045);
#endif

				//if (linear01Depth < _DepthluminanceSky * 0.01)
				//	col.rgb += col.rgb * linear01Depth * 1.5;
				//else
				//	col.rgb += col.rgb * Remap(linear01Depth, _DepthluminanceSky * 0.01, 1.0, _DepthluminanceSky * 0.01,0) * 1.5;
				
				float4 col = float4(_DFogColor.rgb * saturate(_FogDensity), saturate(FogFactor * factor * _FogDensity));
				float3 tmp2 = FogFactor * _FogDensity;
				col.rgb = lerp(col.rgb, _DFogColor.rgb, SkyboxFogHeightMask);
				col.a = saturate(max(col.a, SkyboxFogHeightMask));
				return lerp(float4(_DFogColor.rgb * col.a, col.a),col, _ContrastTone);
			}
			ENDHLSL
		}
	}
	FallBack Off
}
