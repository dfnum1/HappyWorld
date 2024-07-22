Shader "SD/Environment/SD_Element"
{
   Properties
    {
        [NoScaleOffset]
        _MainTexture ("纹理", 2D) = "black"{}
        _MainScaleU ("纹理缩放U", Range(0.01, 10)) = 1.0
		_MainScaleV("纹理缩放V", Range(0.01, 10)) = 1.0
        _MainSpeed ("扰动速度", Range(0.0, 1.0)) = 1.0
		_MainMulti("颜色增倍", Range(0.0, 10)) = 1.0
		_MainAlpha("Alpha", Range(0.0, 1.0)) = 1.0

		[MaterialToggle(USE_WORLD_UV)] _UseWorldUV("Use World UV", Int) = 1

		[NoScaleOffset]
		_MatCap("MatCap", 2D) = "white" {}
		_MatCapStrength("MatCapStrength", Range(0.0, 10.0)) = 1.0
		_CurveFactor("CurveFactor", Float) = 1

		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSrc("Blend Src", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDst("Blend Dst", Float) = 10
    }
    SubShader
    {
        LOD 100

		Tags{ "Queue" = "Transparent+100"  "IgnoreProjector" = "True" }
		Blend [_BlendSrc] [_BlendDst]

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

			#pragma multi_compile __ USE_WORLD_UV

            #include "../Includes/WorldCurvedCG.hlsl"

            sampler2D _MainTexture;
            sampler2D _MatCap;
            CBUFFER_START(UnityPerMaterial)
            float _MainScaleU, _MainScaleV;
            float _MainSpeed;
			float _MainAlpha;
			float _MainMulti;
			half _MatCapStrength;
			fixed _CurveFactor;
            CBUFFER_END
            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 uv : TEXCOORD0;
				float3 normal : NORMAL;
                float3 worldPosition : TEXCOORD1;
                UNITY_FOG_COORDS(4)
            };

            v2f vert (appdata v)
            {
                v2f o = (v2f)0;
				CURVED_WORLD_TRANSFORM_POINT(v.vertex,_CurveFactor);

                o.worldPosition = mul(UNITY_MATRIX_M, v.vertex).xyz;
				o.normal = UnityObjectToWorldNormal(v.normal);
                o.vertex = mul(UNITY_MATRIX_VP, float4(o.worldPosition, 1));
				o.uv.xy = v.uv;
				o.uv.z = dot(normalize(UNITY_MATRIX_IT_MV[0].xyz), normalize(v.normal));
				o.uv.w = dot(normalize(UNITY_MATRIX_IT_MV[1].xyz), normalize(v.normal));
				o.uv.zw = o.uv.zw * 0.5 + 0.5
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }


            float4 frag (v2f i) : SV_Target
            {
#if USE_WORLD_UV
				float2 uv = i.worldPosition.xy;
				uv += i.worldPosition.yz*saturate(abs(dot(i.normal, float3(1.0f, 0.0f, 0.0f)))-0.5);
				uv += i.worldPosition.xz*saturate(abs(dot(i.normal, float3(0.0f, 1.0f, 0.0f))) - 0.5);
#else
				float2 uv = i.uv.xy;
#endif
				uv += _MainSpeed*_Time.y;
				uv.x /= _MainScaleU;
				uv.y /= _MainScaleV;

                float4 diffuseColor = tex2D(_MainTexture, uv)*_MainMulti;
				float4 color = diffuseColor;
				color.rgb *= tex2D(_MatCap, i.uv.zw).rgb*_MatCapStrength;
				color.a = _MainAlpha;
                UNITY_APPLY_FOG(i.fogCoord, color);

                return color;
            }
            ENDHLSL
        }
    }
    FallBack Off
}
