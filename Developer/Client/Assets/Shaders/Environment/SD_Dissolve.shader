Shader "SD/Environment/SD_Dissolve" 
{
    Properties 
	{
		_AmbientStrength("Ambient Strength",Range(0,2)) = 1
        _MainTex ("Base(RGB)", 2D) = "white" {}
		_DissolveMap("Dissolve Map",2D) = "white"{}
		_DissolveThreshold("Dissolve Threshold",Range(0.0,1.0)) = 0.0
		_DissolveEdge("Dissolve Edge Width",Range(0.0,0.2)) = 0.1
		_DissolveColorA("Dissolve Color A",Color) = (1,1,1,1)
		_DissolveColorB("Dissolve Color B",Color) = (1,1,1,1)
	}
	SubShader 
	{
		Tags { "Queue"="Geometry+200"  "RenderType"="Opaque""IgnoreProjector" = "True"  }
		Pass 
		{
			Name "FORWARD"
			Cull Back Lighting Off ZWrite On
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "../Includes/WorldCurvedCG.hlsl"
			sampler2D _MainTex;
			sampler2D _DissolveMap;

			CBUFFER_START(UnityPerMaterial)
			float4 _MainTex_ST;
			float4 _DissolveMap_ST;
			float _DissolveThreshold;
			float _DissolveEdge;
			fixed4 _DissolveColorA;
			fixed4 _DissolveColorB;

			float _AmbientStrength;
			CBUFFER_END
			struct VertexInput 
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};
			struct VertexOutput 
			{
				float4 pos : SV_POSITION;
				float2 uvMainTex : TEXCOORD0;
				float2 uvMapTex : TEXCOORD1;
			};
			VertexOutput vert (VertexInput v) 
			{
				VertexOutput o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uvMainTex = TRANSFORM_TEX(v.texcoord,_MainTex);
				o.uvMapTex = TRANSFORM_TEX(v.texcoord,_DissolveMap);
				return o;
			}
			half4 frag(VertexOutput i) : COLOR 
			{
				fixed3 map = tex2D(_DissolveMap,i.uvMapTex).rgb;
				Util_AmbientColor(map, _AmbientStrength);
				
				clip(map.r-_DissolveThreshold);
				
				fixed3 albedo = tex2D(_MainTex,i.uvMainTex).rgb;
				fixed3 diffuse = tex2D(_MainTex,i.uvMainTex).rgb;

				fixed t = 1-smoothstep(0.0,_DissolveEdge,map.r-_DissolveThreshold);
				fixed3 burnColor = lerp(_DissolveColorA.rgb,_DissolveColorB.rgb,t);
				burnColor = pow(burnColor,2);

				fixed3 finalColor = lerp(diffuse,burnColor,t*step(0.0001,_DissolveThreshold));
				return fixed4(finalColor,1);
			}
			ENDHLSL
		}
	} 
	FallBack Off
}
