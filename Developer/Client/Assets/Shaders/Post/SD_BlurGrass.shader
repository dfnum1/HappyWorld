Shader "SD/Post/SD_BlurGrass"
{
Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Brightness("Brightness", Range (1, 30)) = 0.7
		_Color ("Tint", Color) = (1,1,1,1)
    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        // #0
        Pass
        {
			Tags
            {
                // Specify LightMode correctly.
                "LightMode" = "GrabPass"
            }			
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment fragHorizontal
            #include "../Includes/BlurTexture.hlsl"
            ENDHLSL
        }

        // #1
        Pass
        {
			Tags
            {
                // Specify LightMode correctly.
                "LightMode" = "GrabPass"
            }			
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment fragVertical
            #include "../Includes/BlurTexture.hlsl"
            ENDHLSL
        }
    }	
}