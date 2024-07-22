// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "SD/UI/SD_GuideMask"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15

        _Center("Center",vector) = (0,0,0,0)
        _SliderX("SliderX",Range(0,1500)) = 1500
        _SliderY("SliderY",Range(0,1500)) = 1500

		_DiamondX("DiamondX",Range(0,150)) = 50

		[MaterialEnum(None,0, Rect,1, Circle, 2, Diamond, 3, SmallCircle, 4)]_MaskType("Type", Int) = 0
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
			Name "Default"
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_ALPHACLIP

			struct appdata_t
			{
                float4 vertex   : POSITION;
				float2 texcoord : TEXCOORD0;
                float4 color    : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float4 worldPosition : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};
			CBUFFER_START(UnityPerMaterial)
			fixed4 _Color;
			fixed4 _TextureSampleAdd;
			float4 _ClipRect;
			float2 _Center;
            float _SliderX;
            float _SliderY;
			float _DiamondX;
			float _MaskType;
			CBUFFER_END
			v2f vert(appdata_t v)
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				              
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                OUT.color = v.color * _Color;
                return OUT;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f IN) : SV_Target
			{
				half4 color = IN.color;
				#ifdef UNITY_UI_ALPHACLIP
					color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
					clip(color.a - 0.001);
				#endif				
				if(_MaskType == 1)
				{
					float2 dis = IN.worldPosition.xy - _Center.xy;
					color.a *= (abs(dis.x) > _SliderX) || (abs(dis.y) > _SliderY);
				}
				else if(_MaskType == 2)
				{
					color.a *= (distance(IN.worldPosition.xy, _Center.xy) > max(_SliderY, _SliderX));
					color.rgb *= color.a;
				}
				else if (_MaskType == 3)//ÁâÐÎ
				{
					float2 dis = IN.worldPosition.xy - _Center.xy;
					color.a *= (abs(dis.x) > (_SliderX - _DiamondX * abs(dis.y / _SliderY))) || (abs(dis.y) > _SliderY);
				}
				else if (_MaskType == 4)//Ð¡Ô²
				{
					color.a *= (distance(IN.worldPosition.xy, _Center.xy) > min(_SliderY, _SliderX));
					color.rgb *= color.a;
				}

				return color;
			}
			ENDHLSL
		}
	}	
	FallBack Off
}