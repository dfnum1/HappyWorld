Shader "SD/Environment/SD_Water"
{
   Properties
    {
	    _AmbientStrength("Ambient Strength",Range(0,2)) = 1
	    [Header(Base Color)]
        _ShallowColor ("水岸颜色", Color) = (0.44, 0.95, 0.36, 1.0)
        _DeepColor ("近水颜色", Color) =  (0.0, 0.05, 0.19, 1.0)
        _FarColor ("远水颜色", Color) = (0.04, 0.27, 0.75, 1.0)
		
        [Header(DepthAndDistance)]
		[MaterialToggle(USE_DEPTH)] _USE_DEPTH("IsUseDepth", Int) = 0
		_Depth("深度蒙版", 2D) = "back" {}
        _DepthDensity ("深度", Range(0.0, 2.0)) = 0.4
        _DistanceDensity ("菲尼尔距离", Range(0.0, 0.2)) = 0.047
		_ShadowFactor("水岸到近水过渡", Range(0.0, 1.0)) = 1

        [Header(Waves)]
        [NoScaleOffset]
        _WaveNormalMap ("法线贴图", 2D) = "bump"{}
        _WaveNormalScale ("缩放", Range(0.0, 100)) = 25
        _WaveNormalSpeed ("强度", Range(0.0, 1.0)) = 0.05
       

        [Header(Foam)]
        [NoScaleOffset]
        _FoamTexture ("噪点纹理", 2D) = "black"{}
        _FoamScale ("纹理缩放", Range(0.0, 100)) = 1.0
		_FoamWidth ("浪边大小", Range(0.1, 100)) = 1.0
		_FoamContribution ("纹理强度", Range(0.0, 1.0)) = 1.0
        _FoamSpeed ("扰动速度", Range(0.0, 1.0)) = 1.0
        _FoamNoiseScale ("噪点强度", Range(0.0, 1.0)) = 0.5

        [Header(Edge Foam)]
        _EdgeFoamColor ("浪边颜色", Color) = (1, 1, 1, 1)
		_EdgeWaveTexture("浪噪点图", 2D) = "black"{}
        _EdgeFoamScale ("Scale",  Range(0, 1)) = 1
		_EdgeNoiseCutoff("边缘缩放", Range(0, 10)) = 1
		_EdgeNoiseSpeedX("扰动速度X轴", Range(-1, 1)) = 0.03
		_EdgeNoiseSpeedY("扰动速度Y轴", Range(-1, 1)) = 0.03
		_EdgeDistortion("浪边扭曲纹理", 2D) = "white" {}
		_EdgeDistortionAmount("边缘扭曲强度", Range(0, 5)) = 2

        [Header(Vertex Waves #1)]
        _Wave1Direction ("强度", Range(0, 1)) = 0
        _Wave1Amplitude ("振幅", Range(0, 1)) = 0.53
        _Wave1Wavelength ("波浪强度", Range(0, 10)) = 7.5
        _Wave1Speed ("波浪速度", Range(0, 10)) = 0.25

        [Header(Vertex Waves #2)]
        _Wave2Direction ("强度", Range(0, 1)) = 0
        _Wave2Amplitude ("振幅", Range(0, 1)) = 0.15
        _Wave2Wavelength ("波浪强度", Range(0, 10)) = 7.5
        _Wave2Speed ("波浪速度", Range(0, 10)) = 1

		_CurveFactor("CurveFactor", Float) = 1
    }
    SubShader
    {
        LOD 100

        Tags { "RenderPipeline" = "UniversalPipeline" "Queue" = "Transparent" }

        Pass
        {
            Tags { "LightMode" = "UniversalForward" }
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
			
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
			#pragma shader_feature __ USE_DEPTH
			
            #include "../Includes/WorldCurvedCG.hlsl"
            #include "../Includes/WaterUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"			
            sampler2D _WaveNormalMap;
            sampler2D _FoamTexture;
            sampler2D _EdgeWaveTexture;
            sampler2D _Depth;
            sampler2D _EdgeDistortion;
			#if USE_DEPTH
			TEXTURE2D(_CameraDepthTexture); SAMPLER(sampler_CameraDepthTexture);
			#endif
            CBUFFER_START(UnityPerMaterial)
            // Densities.
            float _DepthDensity;
            float _DistanceDensity;
			float _ShadowFactor;

            // Wave Normal Map.
            
            float _WaveNormalScale;
            float _WaveNormalSpeed;

            // Base Color.
            float3 _ShallowColor;
            float3 _DeepColor;
            float3 _FarColor;

            // Foam.
            
			float4 _FoamTexture_ST;
            float _FoamScale;
			float _FoamWidth;
            float _FoamNoiseScale;
            float _FoamSpeed;
            float _FoamContribution;

            // Edge Foam.
			
			float4 _EdgeWaveTexture_ST;
            float3 _EdgeFoamColor;
            float _EdgeFoamScale;
			float _EdgeNoiseCutoff;
			float _EdgeNoiseSpeedX;
			float _EdgeNoiseSpeedY;
			float _EdgeDistortionAmount;

			
			float4 _EdgeDistortion_ST;

            // Wave 1.
            float _Wave1Direction;
            float _Wave1Amplitude;
            float _Wave1Wavelength;
            float _Wave1Speed;

            // Wave 2.
            float _Wave2Direction;
            float _Wave2Amplitude;
            float _Wave2Wavelength;
            float _Wave2Speed;

			fixed _AmbientStrength, _CurveFactor;
            CBUFFER_END
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 uv : TEXCOORD0;
				float4 noise : TEXCOORD1;
                float3 worldPosition : TEXCOORD2;
                float4 screenPosition : TEXCOORD3;
                UNITY_FOG_COORDS(4)
            };

            // Returns the total wave height offset at the given world position,
            // based on the set wave properties.
            float GetWaveHeight(float2 worldPosition)
            {
                float2 dir1 = float2(cos(M_PI * _Wave1Direction), sin(M_PI * _Wave1Direction));
                float2 dir2 = float2(cos(M_PI * _Wave2Direction), sin(M_PI * _Wave2Direction));
                float wave1 = SimpleWave(worldPosition, dir1, _Wave1Wavelength, _Wave1Amplitude, _Wave1Speed);
                float wave2 = SimpleWave(worldPosition, dir2, _Wave2Wavelength, _Wave2Amplitude, _Wave2Speed);
                return wave1 + wave2;
            }

            // Approximates the normal of the wave at the given world position. The d
            // parameter controls the "sharpness" of the normal.
            float3x3 GetWaveTBN(float2 worldPosition, float d)
            {
                float waveHeight = GetWaveHeight(worldPosition);
                float waveHeightDX = GetWaveHeight(worldPosition - float2(d, 0));
                float waveHeightDZ = GetWaveHeight(worldPosition - float2(0, d));
                
                // Calculate the partial derivatives in the Z and X directions, which
                // are the tangent and binormal vectors respectively.
                float3 tangent = normalize(float3(0, waveHeight - waveHeightDZ, d));
                float3 binormal = normalize(float3(d, waveHeight - waveHeightDX, 0));

                // Cross the results to get the normal vector, and return the TBN matrix.
                // Note that the TBN matrix is orthogonal, i.e. TBN^-1 = TBN^T.
                // We exploit this fact to speed up the inversion process.
                float3 normal = normalize(cross(binormal, tangent));
                return transpose(float3x3(tangent, binormal, normal));
            }

            v2f vert (appdata v)
            {
                v2f o;

				CURVED_WORLD_TRANSFORM_POINT(v.vertex,_CurveFactor);

                o.worldPosition = mul(UNITY_MATRIX_M, v.vertex).xyz;
                o.worldPosition.y += GetWaveHeight(o.worldPosition.xz);
                o.vertex = mul(UNITY_MATRIX_VP, float4(o.worldPosition, 1));

         //       o.grabPosition = ComputeGrabScreenPos(o.vertex);
				o.screenPosition = ComputeScreenPos(o.vertex);

                o.uv.xy = v.uv;
				o.uv.zw = TRANSFORM_TEX(v.uv, _EdgeDistortion);
				o.noise.xy = TRANSFORM_TEX(v.uv, _FoamTexture);
				o.noise.zw = TRANSFORM_TEX(v.uv, _EdgeWaveTexture);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }


			float4 alphaBlend(float4 top, float4 bottom)
			{
				float3 color = (top.rgb * top.a) + (bottom.rgb * (1 - top.a));
				float alpha = bottom.a;// top.a + bottom.a * (1 - top.a);

				return float4(color, alpha);
			}

            float4 frag (v2f i) : SV_Target
            {
			#if USE_DEPTH
				float2 screenUV = i.screenPosition.xy / i.screenPosition.w;
				float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, screenUV).r;
				float depth_Buffer = abs(LinearEyeDepth(depth,_ZBufferParams) - LinearEyeDepth(i.vertex.z,_ZBufferParams));//LinearEyeDepth(depth, _ZBufferParams);
				float depthDifference = depth_Buffer;//abs(LinearEyeDepth(fragDepth) - LinearEyeDepth(i.vertex.z));
				float depthGap = depth_Buffer - i.screenPosition.w;
				float foamDistance = _FoamWidth;
			#else
				float depth = tex2D(_Depth, i.uv.xy).r;
				float depthDifference = 1 - depth;
				float foamDistance = 1;
			#endif
				float eyeDistance = length(i.worldPosition - _WorldSpaceCameraPos);	

                // Calculate the view vector.
                float3 viewDirWS = normalize(i.worldPosition - _WorldSpaceCameraPos);

                // ------------------- //
                // NORMAL CALCULATIONS //
                // ------------------- //

                // Get the tangent to world matrix.
                float3x3 tangentToWorld = GetWaveTBN(i.worldPosition.xz, 0.01);

                // Sample the wave normal map and calculate the world-space normal for this fragment.
                float3 normalTS = MotionFourWayChaos(_WaveNormalMap, i.worldPosition.xz/_WaveNormalScale, _WaveNormalSpeed, true);
            //    float3 normalWS = mul(tangentToWorld, normalTS);

                // ------------------------------ //
                // SAMPLE DEPTH AND COLOR BUFFERS //
                // ------------------------------ //

                // Calculate the position of this fragment in screen space to
                // use as uv's in screen-space texture look ups.
           //     float2 screenCoord = i.grabPosition.xy / i.grabPosition.w;

                // Sample the grab-pass and depth textures based on the screen coordinate
                // to get the frag color and depth of the fragment behind this one.
				float3 fragColor = float3(1,1,1);
				float fragDepth = depth;

                // ------------------------ //
                // DEPTH AND DISTANCE MASKS //
                // ------------------------ //
                // Calculate the distance the viewing ray travels underwater,
                // as well as the transmittance for that distance.
				float opticalDepth = depthDifference;// abs(LinearEyeDepth(fragDepth) - LinearEyeDepth(i.vertex.z));
                float transmittance = exp(-_DepthDensity * opticalDepth);

                // Also calculate how far away the fragment is from the camera.
                float distanceMask = exp(-_DistanceDensity * eyeDistance);

                // ----------- //
                // SHADOW MASK //
                // ----------- //

                float shadowMask = _ShadowFactor;

                // ---------- //
                // BASE COLOR //
                // ---------- //

                // Calculate the base color based on the transmittance and distance mask.
                float3 baseColor = fragColor * _ShallowColor;
                baseColor = lerp(_DeepColor, baseColor, transmittance *  shadowMask);
                baseColor = lerp(_FarColor, baseColor, distanceMask);

                // ---------- //
                // FOAM COLOR //
                // ---------- //

                // Distort the world space uv coordinates by the normal map.
				float2 foamUV = (i.worldPosition.xz / _FoamScale) + (_FoamNoiseScale * normalTS.xz);

                // Sample the foam texture and modulate the result by the distance mask and shadow mask.
                float3 foamColor = MotionFourWayChaos(_FoamTexture, foamUV, _FoamSpeed, false);
                foamColor = foamColor * distanceMask;
                foamColor = foamColor * _FoamContribution;


              //  float3 color = baseColor + reflectedColor + foamColor;
				float3 color = baseColor + foamColor;

				Util_AmbientColor(color, _AmbientStrength);
				// --------------- //
				// EDGE FOAM COLOR //
				// --------------- //
				float foamDepthDifference01 = saturate(depthDifference / foamDistance);
				float surfaceNoiseCutoff = foamDepthDifference01 * _EdgeNoiseCutoff;
				float2 distortSample = (tex2D(_EdgeDistortion, i.uv.zw).xy * 2 - 1) * _EdgeDistortionAmount;
				float2 noiseUV = float2((i.noise.z + _Time.y * _EdgeNoiseSpeedX) + distortSample.x,
					(i.noise.w + _Time.y * _EdgeNoiseSpeedY) + distortSample.y);
				float surfaceNoiseSample = tex2D(_EdgeWaveTexture, noiseUV).r;
				float surfaceNoise = smoothstep(surfaceNoiseCutoff - 0.1, surfaceNoiseCutoff + 0.1, surfaceNoiseSample);
				float4 edgeFoamColor = float4( lerp(color,_EdgeFoamColor.rgb, _EdgeFoamScale), surfaceNoise);
				// ----------- //
				// FINAL COLOR //
				// ----------- //

				float4 final = alphaBlend(edgeFoamColor, float4(color, 1));
                // Apply fog.
                UNITY_APPLY_FOG(i.fogCoord, final);
				
                return final;
            }
            ENDHLSL
        }
  }
    FallBack Off
}
