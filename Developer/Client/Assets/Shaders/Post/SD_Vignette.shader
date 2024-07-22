Shader "SD/Post/SD_Vignette"
{
	Properties
    {
     
    }
	HLSLINCLUDE
	#include "../Includes/BaseDefine/VertexBase.hlsl"
	struct v2f 
	{
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};
	CBUFFER_START(UnityPerMaterial)
	half4 _Vignette_Color;
	half2 _Vignette_Center; // UV space
	half4 _Vignette_Settings; // x: intensity, y: smoothness, z: roundness, w: rounded
	CBUFFER_END
	v2f vert( appdata_img v ) 
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;
		return o;
	} 
	half4 frag(v2f i) : SV_Target
	{
		half4 color = half4(0,0,0,_Vignette_Color.a);
		float2 uvDistorted = i.uv;
        {
            half2 d = abs(uvDistorted - _Vignette_Center) * _Vignette_Settings.x;
            d.x *= lerp(1.0, _ScreenParams.x / _ScreenParams.y, _Vignette_Settings.w);
            d = pow(saturate(d), _Vignette_Settings.z); // Roundness
            half vfactor = pow(saturate(1.0 - dot(d, d)), _Vignette_Settings.y);
            color.rgb *= lerp(_Vignette_Color.rgb, (1.0).xxx, vfactor);
            color.a = lerp(1.0, color.a, vfactor);
        }
		return color;
	}
	ENDHLSL
	
	Subshader 
	{
		Pass 
		{
			Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Opaque" }
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDHLSL
		}
	}	
	Fallback Off
}
