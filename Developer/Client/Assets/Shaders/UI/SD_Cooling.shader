// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "SD/UI/Cooling"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Angle ("_Angle", Range(0, 360)) = 0
		 _Offset("Offset", vector) = (0.5,0.5,0,0)
		_Color ("Color", Color) = (0, 0, 0, 1)
		
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
	}
	SubShader
	{		
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}	
		Stencil
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}
		
		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_CLIP_RECT
			#pragma multi_compile __ UNITY_UI_ALPHACLIP

 
			struct appdata
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
 
			struct v2f
			{			
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};
 
			sampler2D _MainTex;
			
			CBUFFER_START(UnityPerMaterial)
			float4 _MainTex_ST;
			half _Angle;
			fixed4 _Color;
			fixed2 _Offset;
			
			fixed4 _TextureSampleAdd;
			float4 _ClipRect;
			CBUFFER_END
 
			v2f vert (appdata v)
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				OUT.worldPosition = v.vertex;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

				OUT.color = v.color ;
				return OUT;
			}
			
			fixed4 frag (v2f IN) : SV_Target
			{
				half4 col = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
 
				float2 uv = IN.texcoord -_Offset;
				float radian = atan2(uv.y, uv.x)*57.3f+180;
 
				float2 radian2 = 360-_Angle;
				fixed v = step(radian, radian2);
				v = saturate(v);
				return col*(_Color*(1-v) + v);
			}
			ENDCG
		}
	}
	FallBack Off
}