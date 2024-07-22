
Shader "SD/Particles/SD_Dissolve" 
{
    Properties 
	{
		_AmbientStrength("Ambient Strength",Range(0,2)) = 0
        [HDR]_Color ("Color", Color) = (0.5,0.5,0.5,0.5)

        _MainTex ("MainTex", 2D) = "white" {}
		[MaterialToggle(USE_MASK_WHITE)] _MaskTexWhite("Use Mask White", Int) = 0
		_MaskTex ("MaskTex", 2D) = "white" {}
		_MaskSpeedX("Mask UV Offset X", float) = 0
		_MaskSpeedY("Mask UV Offset Y", float) = 0
		
		[MaterialToggle(USE_DISTURB_WHITE)] _DisturbTexWhite("Use Disturb White", Int) = 0
        _DisturbTex("DisturbTex", 2D) = "white" {}
		//_Disturb("_Disturb x(uv1.x): hard disturb| y(uv1.y): soft disturb ", Vector) = (0, 0, 0, 0)
		[MaterialEnum(Back,2, Front, 1, Off, 0)]_CullMode("CullMode", Int) = 2
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
		[MaterialEnum(One,1, OneMinusSrcAlpha, 10)]_BlendMode("BlendMode", Int) = 10
        [MaterialToggle(USE_ANIM)]_UseAnim("Use Anim", Int) = 0
		_SpeedX("UV Offset X", float) = 0
		_SpeedY("UV Offset Y", float) = 0

		_NoiseTex("Noise Tex", 2D) = "white" {}//扰动噪点贴图
		_HeatSpeed("Heat speed",Vector) = (0,0,0,0)//扰动速度
		_HeatForce("Heat Force", range(0,5)) = 0//扰动强度


		_Dissolve_Smoothstep_Min("Dissolve_Smoothstep_Min", Range(0 , 1)) = 0
		_Dissolve_Smoothstep_Max("Dissolve_Smoothstep_Max", Range(0 , 1)) = 0

		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull Mode", Float) = 0
		[Enum(Off, 0, On,1)]_Zwrite("ZWrite", Float) = 0
		_CurveFactor("CurveFactor", Float) = 1
		}
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent+100"
            "RenderType"="Transparent"
        }
		LOD 100
        Pass {
			Blend SrcAlpha[_BlendMode]
			Cull [_Cull]
            
			ZWrite [_Zwrite]
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#include "Assets/Shaders/Includes/WorldCurvedCG.hlsl"
			#pragma multi_compile __ USE_ANIM
			#pragma multi_compile __ USE_MASK_WHITE
			#pragma multi_compile __ USE_DISTURB_WHITE

			uniform sampler2D _MainTex, _MaskTex;
			uniform sampler2D _DisturbTex;
			uniform sampler2D _NoiseTex;
			CBUFFER_START(UnityPerMaterial)
			fixed4 _HeatSpeed;
		//	fixed4 _UVspeed;
			uniform float4 _MainTex_ST,_MaskTex_ST;
            uniform float4 _Color;
             uniform float4 _DisturbTex_ST;
			uniform float4 _NoiseTex_ST;
			uniform fixed _HeatForce;
			//uniform float4 _Disturb;

			uniform fixed _Dissolve_Smoothstep_Min;
			uniform fixed _Dissolve_Smoothstep_Max;

			fixed _AmbientStrength, _CurveFactor;
#if USE_ANIM
			float _SpeedX;
			float _SpeedY;
#endif
			float _MaskSpeedX;
			float _MaskSpeedY;
			CBUFFER_END
            struct VertexInput {
                float4 vertex : POSITION;
				float4 vertexColor : COLOR;
                float2 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
				float4 custom1 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
				float4 maskuv : TEXCOORD2;
				float2 noiseuv : TEXCOORD3;
                float4 vertexColor : COLOR;
            };		
            VertexOutput vert (VertexInput v) 
			{
                VertexOutput o = (VertexOutput)0;
				#if USE_ANIM
				float2 offset	= frac(float2(_SpeedX, _SpeedY) * _Time.y);
				o.uv0.xy = TRANSFORM_TEX(v.texcoord0, _MainTex);
                o.uv0.zw = TRANSFORM_TEX(v.texcoord0, _DisturbTex)+offset;
				#else
                o.uv0.xy = TRANSFORM_TEX(v.texcoord0, _MainTex);
                o.uv0.zw = TRANSFORM_TEX(v.texcoord0, _DisturbTex);
				#endif
				o.uv1	= v.texcoord1;
                o.vertexColor = v.vertexColor;
				CURVED_WORLD_TRANSFORM_POINT(v.vertex,_CurveFactor);
                o.pos = UnityObjectToClipPos( v.vertex );
				o.maskuv.xy = TRANSFORM_TEX(v.texcoord0, _MaskTex) + frac(float2(_MaskSpeedX, _MaskSpeedY) * _Time.y);
				o.noiseuv = TRANSFORM_TEX(v.texcoord0, _NoiseTex);
                return o;
            }

            float4 frag(VertexOutput i) : COLOR 
			{
				float2 uv = i.uv0.xy;
				half4 offsetColor1 = tex2D(_NoiseTex, i.noiseuv.xy + _Time.xz * _HeatSpeed.x);
				half4 offsetColor2 = tex2D(_NoiseTex, i.noiseuv.xy - _Time.yx * _HeatSpeed.y);
				uv.x += ((offsetColor1.r + offsetColor2.r)-1) * _HeatForce;
				uv.y += ((offsetColor1.g + offsetColor2.g)-1) * _HeatForce;

                float4 _MainTex_var		= tex2D(_MainTex, uv + i.uv1.zw) *_Color * i.vertexColor;
#if USE_MASK_WHITE
				_MainTex_var.a *= tex2D(_MaskTex, i.maskuv.xy).a;
#else
				_MainTex_var.a *= tex2D(_MaskTex, i.maskuv.xy).r;
#endif
              //  float4 _DisturbTex_var	= tex2D(_DisturbTex, i.uv0.zw);

				Util_AmbientColor(_MainTex_var, _AmbientStrength);
				float disturbIntensity = i.uv1.x;
#if USE_DISTURB_WHITE				
				float clampResult = clamp((tex2D(_DisturbTex, i.uv0.zw).a+ disturbIntensity), 0, 1);
#else
				float clampResult = clamp((tex2D(_DisturbTex, i.uv0.zw).r+ disturbIntensity), 0, 1);
#endif
				float smoothstepResult = smoothstep(_Dissolve_Smoothstep_Min, _Dissolve_Smoothstep_Max, clampResult);
				clampResult = _MainTex_var.a*clamp(smoothstepResult, -1, 1);

                return float4( _MainTex_var.rgb, clamp(clampResult,0,1))*2;
            }
            ENDHLSL
        }
    }
	FallBack Off
}
