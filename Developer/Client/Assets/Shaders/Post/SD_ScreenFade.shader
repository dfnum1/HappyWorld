Shader "SD/Post/SD_ScreenFade"
{
   Properties
    {
        _Color("Color", Color) = (0,0,0,1)
    }
    SubShader
    {
        LOD 100
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Opaque" }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Color[_Color]
        Pass
        {
        }
    }
	Fallback Off
}
