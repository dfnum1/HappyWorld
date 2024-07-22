Shader "SD/Environment/SD_Lava_Distort_New" {
Properties {
	_AmbientStrength("Ambient Strength",Range(0,2)) = 0
	_Distort("Distort(xy:Lava,zw:Mask)",vector) = (0,0,0,0)
	_Speed("Speed(xy:Lava,zw:Lava Tiling)",vector) = (0,0,0,0)
	_LavaColor("Lava Color", Color) = (1,1,1,1)
	_LavaTex ("LavaTex RGB", 2D) = "white" {}
	_MaskColor("Mask Color", Color) = (1,1,1,1)
	_MaskTex ("MaskTex (A)", 2D) = "white" {}
	_DistortTex ("Distort (A)", 2D) = "white" {}
}
SubShader {
		Tags{"Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True"}
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest	

			#pragma multi_compile __ FOG_LINEAR

			#include "Assets/Shaders/Includes/WorldCurvedCG.hlsl"
			sampler2D_half _MaskTex,_DistortTex,_LavaTex;			
			struct appdata_t {
				fixed4 vertex : POSITION;
				fixed2 texcoord : TEXCOORD0;
				UNITY_FOG_COORDS(1)
			};

			struct v2f {
				fixed4 pos : SV_POSITION;
				fixed2 uv0 : TEXCOORD0;
				fixed2 uv1 : TEXCOORD1;
				UNITY_FOG_COORDS(2)
			};
			//²Î¿¼photoshop»ìºÏÁ½²ã
			float4 alphaBlend(float4 top, float4 bottom)
			{
				float3 color = (top.rgb * top.a) + (bottom.rgb * (1 - top.a));
				float alpha = top.a + bottom.a * (1 - top.a);
				return float4(color, alpha);
			}
			CBUFFER_START(UnityPerMaterial)
			float4 _LavaColor,_MaskColor;
			float4 _Distort,_Speed;

			float _AmbientStrength;
			CBUFFER_END
			v2f vert (appdata_t v)
			{
				v2f o=(v2f)0;
				o.uv0 =v.texcoord;
				o.uv1 =v.texcoord*_Speed.zw+_Speed.xy*_Time.y;
				o.pos = UnityObjectToClipPos(v.vertex);
				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed mask = tex2D(_MaskTex, i.uv0).r;
				fixed distort = tex2D(_DistortTex, i.uv0).r;
				fixed4 tex1 = tex2D(_LavaTex, fixed2(i.uv1.x-distort*_Distort.x,i.uv1.y-distort*_Distort.y));
				fixed4 tex2 = tex2D(_LavaTex, fixed2(i.uv1.x-mask*_Distort.z,i.uv1.y-mask*_Distort.w))*1.8;
					   tex2 = fixed4(tex2.rgb,mask);
                       tex1.a = 1-mask;

				fixed4 final = alphaBlend(tex2*_MaskColor, tex1*_LavaColor);

				Util_AmbientColor(final, _AmbientStrength);
				UNITY_APPLY_FOG(i.fogCoord, final);
				return final;
			}
			ENDHLSL 
		}
	}	
	FallBack Off
}
