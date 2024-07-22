Shader "SD/Particles/SD_UV_Mask_Ground"
{
	Properties
	{
		_AmbientStrength("Ambient Strength",Range(0,2)) = 0
		[HDR]_MainColor("MainColor", Color) = (1,1,1,1)
		[MaterialToggle] _MainTexBlack("Use Main Black", Float) = 0
		_MainTex("MainTex", 2D) = "white" {}
		
		[MaterialToggle] _MaskTexBlack("Use Mask Black", Float) = 0
		_Mask("Mask", 2D) = "white" {}
		[MaterialToggle]_CustomSwitch("CustomSwitch", Float) = 0
		_ControlFloat("ControlFloat", Float) = 0
		_SmoothMin("SmoothMin", Float) = 0
		_SmoothMax("SmoothMax", Float) = 0
		[HideInInspector]_01("0-1", Float) = 0
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
		
		[MaterialEnum(Add,1, Blend, 10)]_BlendMode("BlendMode", Int) = 10
		_CurveFactor("CurveFactor", Float) = 1
	}
	Category
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+100" "IgnoreProjector" = "True" }
		Cull Back
		Blend SrcAlpha [_BlendMode]	
		SubShader
		{
			Pass
			{
				HLSLPROGRAM
				#pragma target 3.0
				#pragma vertex vert
				#pragma fragment frag
				#include "Assets/Shaders/Includes/WorldCurvedCG.hlsl"
				struct appdata_t
				{
					float4 vertex : POSITION;
					fixed4 vertexColor : COLOR;
					float2 texcoord : TEXCOORD0;
					float2 custom1 : TEXCOORD1;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					fixed4 vertexColor : COLOR;
					float4 uv_texcoord : TEXCOORD0;
					float2 uv2_texcoord2 : TEXCOORD1;
				};	

				uniform sampler2D _MainTex;
				uniform sampler2D _Mask;
				CBUFFER_START(UnityPerMaterial)
				uniform float4 _MainColor;
				
				uniform float4 _MainTex_ST;
				uniform float _SmoothMin;
				uniform float _SmoothMax;
				
				uniform float4 _Mask_ST;
				uniform float _ControlFloat;
				uniform float _CustomSwitch;
				uniform float _01;
				
				fixed _MainTexBlack;
				fixed _MaskTexBlack;
				
				fixed _AmbientStrength, _CurveFactor;
				CBUFFER_END
				v2f vert(appdata_t v)
				{
					v2f o;
					o.vertexColor = v.vertexColor;
					o.uv_texcoord.xy		= TRANSFORM_TEX(v.texcoord, _MainTex);
					o.uv_texcoord.zw		= TRANSFORM_TEX(v.texcoord, _Mask);
					o.uv2_texcoord2 = v.custom1;
					
					CURVED_WORLD_TRANSFORM_POINT(v.vertex,_CurveFactor);
					o.vertex = UnityObjectToClipPos(v.vertex);
					
					return o;
				}	
				fixed4 frag(v2f i) : SV_Target
				{
					float2 uv_MainTex = i.uv_texcoord.xy;
					float4 tex2DNode7 = tex2D( _MainTex, uv_MainTex );

					Util_AmbientColor(tex2DNode7, _AmbientStrength);
					float2 uv_Mask = i.uv_texcoord.zw;
					float lerpResult14 = lerp( _ControlFloat , 0.0 , _CustomSwitch);
					float lerpResult19 = lerp( lerpResult14 , i.uv2_texcoord2.x , _CustomSwitch);
					
					float4 maskTex = tex2D( _Mask, uv_Mask );
					float mask_mask = maskTex.r*_MaskTexBlack +  maskTex.a*(1-_MaskTexBlack);
					float smoothstepResult2 = smoothstep( _SmoothMin , _SmoothMax , ( mask_mask + lerpResult19 ));
					float temp_output_5_0 = saturate( smoothstepResult2 );
					float4 emission = ( _MainColor * ( ( 2.0 * tex2DNode7 ) * temp_output_5_0 ) * i.vertexColor );
					float main_mask = tex2DNode7.r*_MainTexBlack +  tex2DNode7.a*(1-_MainTexBlack);
					float lerpResult25 = lerp( ( main_mask* 2.0 ) , 1.0 , _01);
					emission.a = ( _MainColor.a * temp_output_5_0 * lerpResult25 * i.vertexColor.a );
					return emission;
				}
				ENDHLSL
			}
		}
	}
	FallBack Off
}