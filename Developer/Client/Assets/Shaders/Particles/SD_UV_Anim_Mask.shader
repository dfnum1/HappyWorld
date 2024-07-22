Shader "SD/Particles/SD_UV_Anim_Mask"
{
	Properties
	{
		_AmbientStrength("Ambient Strength",Range(0,2)) = 0
		[HDR]_TintColor("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex("Particle Texture", 2D) = "white" {}
		[Toggle] _MaskTexWhite("Use Mask White", Int) = 0
		_Mask("Mask ( R Channel )", 2D) = "white" {}
		[MaterialEnum(Add,1, Blend, 10)]_BlendMode("BlendMode", Int) = 10
		[Toggle]_UseAnim("Use Anim", Int) = 0
		//[Toggle]_UseVertexColor("Use Vertex Color", float) = 1
		[Toggle]_UseCustom("Use ParticeSystem Custom", Int) = 0
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull Mode", Float) = 0
		_SpeedX("UV Offset X", float) = 0
		_SpeedY("UV Offset Y", float) = 0
		_CurveFactor("CurveFactor", Float) = 1
		[Enum(Off, 0, On,1)]_Zwrite("ZWrite", Float) = 0
	}

	Category
	{
		Tags { "Queue" = "Transparent+100" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Blend SrcAlpha [_BlendMode]
		Cull [_Cull]
		Lighting Off
		ZWrite[_Zwrite]
		LOD 100
		SubShader
		{
			Pass
			{
				HLSLPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest

				#include "Assets/Shaders/Includes/WorldCurvedCG.hlsl"

				sampler2D _MainTex;
				sampler2D _Mask;

				CBUFFER_START(UnityPerMaterial)
				fixed4 _TintColor;
				float4 _MainTex_ST;
				float4 _Mask_ST;
				//fixed _UseVertexColor;

				fixed _MaskTexWhite;
				fixed _UseCustom;
				fixed _UseAnim;
				float _SpeedX;
				float _SpeedY;

				fixed _AmbientStrength, _CurveFactor;
				CBUFFER_END
				struct appdata_t
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					float2 custom1 : TEXCOORD1;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					float2 texcoordMask : TEXCOORD1;
				};

				v2f vert(appdata_t v)
				{
					v2f o;
					o.color = v.color;//v.color*_UseVertexColor + fixed4(1,1,1,v.color.a)*(1-_UseVertexColor);

					float2 offset = frac(float2(_SpeedX, _SpeedY) * _Time.y) * _UseAnim;
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex) + offset + v.custom1 * _UseCustom;
					o.texcoordMask = TRANSFORM_TEX(v.texcoord, _Mask);
					
					CURVED_WORLD_TRANSFORM_POINT(v.vertex,_CurveFactor);
					o.vertex = UnityObjectToClipPos(v.vertex);
					
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 c = tex2D(_MainTex, i.texcoord);
					Util_AmbientColor(c, _AmbientStrength);

					fixed4 mask = tex2D(_Mask, i.texcoordMask);
					c.a *= (mask.a * _MaskTexWhite + mask.r * (1 - _MaskTexWhite));
					fixed4 finalColor = 2.0f * i.color * _TintColor * c;				
					return finalColor;
				}
				ENDHLSL
			}
		}
	}
	FallBack Off
}
