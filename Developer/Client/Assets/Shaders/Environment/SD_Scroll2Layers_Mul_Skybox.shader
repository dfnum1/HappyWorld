Shader "SD/Environment/Scroll 2 Layers Multiplicative_Skybox" 
{
Properties 
{
	_MainTex ("Base layer (RGB)", 2D) = "white" {}
	_DetailTex ("2nd layer (RGB)", 2D) = "white" {}
	_ScrollX ("Base layer Scroll speed X", Float) = 1.0
	_ScrollY ("Base layer Scroll speed Y", Float) = 0.0
	_Scroll2X ("2nd layer Scroll speed X", Float) = 1.0
	_Scroll2Y ("2nd layer Scroll speed Y", Float) = 0.0
	_AMultiplier ("Layer Multiplier", Float) = 0.5
	_FogLerpDistance("FogLerpDistance", Float) = 100
	_FogMinHeight("FogMinHeight", Float) = 0
	_FogColorPower("FogColorPower",Float) = 1.0
}

	SubShader 
	{
		Tags { "Queue"="Geometry+10" "RenderType"="Opaque""IgnoreProjector"="True" }
		
		Lighting Off     
		
		//ZWrite Off
			

		HLSLINCLUDE

		//#undef FOG_LINEAR
		#pragma fragmentoption ARB_precision_hint_fastest	
		#pragma multi_compile __ FOG_LINEAR

		//#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
		#include "Assets/Shaders/Includes/WorldCurvedCG.hlsl"
		sampler2D _MainTex;
		sampler2D _DetailTex;
		CBUFFER_START(UnityPerMaterial)
		float4 _MainTex_ST;
		float4 _DetailTex_ST;
		
		float _ScrollX;
		float _ScrollY;
		float _Scroll2X;
		float _Scroll2Y;
		float _AMultiplier;

		float _FogLerpDistance;
		float _FogMinHeight;
		float _FogColorPower;
		CBUFFER_END
		struct v2f
		{
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			float2 uv2 : TEXCOORD1;
			fixed4 color : TEXCOORD2;
			float  fogFactor : TEXCOORD3;
		};

		
		v2f vert (appdata_full v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv = TRANSFORM_TEX(v.texcoord.xy,_MainTex) + frac(float2(_ScrollX, _ScrollY) * _Time);
			o.uv2 = TRANSFORM_TEX(v.texcoord.xy,_DetailTex) + frac(float2(_Scroll2X, _Scroll2Y) * _Time);
			o.color = fixed4(_AMultiplier, _AMultiplier, _AMultiplier, _AMultiplier);
			
			float4 worldPos = mul(UNITY_MATRIX_M, v.vertex);

			o.fogFactor = clamp(  (worldPos.y - _FogMinHeight) / _FogLerpDistance, 0, 1);
				
			return o;
		}
		ENDHLSL


		Pass 
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest		
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 o;
				fixed4 tex = tex2D (_MainTex, i.uv);
				fixed4 tex2 = tex2D (_DetailTex, i.uv2);
				
				o = (tex * tex2) * i.color;
				o = lerp(unity_FogColor * _FogColorPower, o, i.fogFactor);

				return o;
			}
			ENDHLSL 
		}	
	}
	FallBack Off
}
