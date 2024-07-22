Shader "SD/Object/Rotation_Offset" {
  Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}       
		_VertexColor("Vertex Color( r, g, b)",Vector) = (1,0.5,0.5,0)		
		_RotationSpeed("RotationSpeed",Range(0, 15)) = 0
		_OffsetSpeed("OffsetSpeed",Range(0, 1)) = 0
		_Rotation("RotationAngel", Vector) = (0.0,0.0,0.0,1)
		//_Axis("AxisPoint", Vector) = (0.0,0.0,0.0,1)
		_CurveFactor("CurveFactor", Float) = 1
    }
    SubShader {
        Tags {
            "RenderType"="Opaque" "Queue" = "Geometry""DisableBatching"="True"
        }
        Pass {
            Name "FORWARD"
			Cull Back
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "Assets/Shaders/Includes/WorldCurvedCG.hlsl"        
            uniform sampler2D _MainTex; 

			CBUFFER_START(UnityPerMaterial)
			uniform fixed _RotationSpeed;
			uniform fixed _OffsetSpeed;
			uniform fixed4 _VertexColor;
			uniform fixed4 _Rotation;
			//uniform fixed4 _Axis;
			fixed _CurveFactor;
			CBUFFER_END
            struct VertexInput {
                fixed4 vertex : POSITION;				
                fixed2 texcoord0 : TEXCOORD0;
				fixed4 color : COLOR;
				
            };
            struct VertexOutput
			{
                fixed4 pos : SV_POSITION;
                fixed2 uv0 : TEXCOORD0;				
			  	fixed4 posWorld : TEXCOORD3;
				fixed4 color : COLOR;
				
            };

			//旋转函数
			float4x4 Rotation(float4 rotation)
			{
			float radx=radians(rotation.x);
			float rady=radians(rotation.y)+ _Time.y*_RotationSpeed;
			float radz=radians(rotation.z);
			float sinX=sin(radx);
			float cosX=cos(radx);
			float sinY=sin(rady);
			float cosY=cos(rady);
			float sinZ=sin(radz);
			float cosZ=cos(radz);			
			return float4x4 (cosY*cosZ,-cosY*sinZ,sinY,0.0,
			                 cosX*sinZ+sinX*sinY*cosZ,cosX*cosZ-sinX*sinY*sinZ,-sinX*cosY,0.0,
					         sinX*sinZ-cosX*sinY*cosZ,sinX*cosZ+cosX*sinY*sinZ,cosX*cosY,0.0,
							0.0,0.0,0.0,1);
			}
            VertexOutput vert (VertexInput v) 
			{
                VertexOutput o = (VertexOutput)0;
				o.color = v.color;
				//Y轴上下位移
				float4 Offset = float4(0,0,0,0);
                Offset.y = sin(3.1416 * _Time.y )  * _OffsetSpeed;
				/*//计算旋转角度
                float angle = length(v.vertex)+ _Time.y*_RotationSpeed;
                //绕Y轴旋转矩阵
				  float4x4 RM={
                    float4(cos(angle), 0 , sin(angle) , 0),
                    float4(0 , 1 ,0 , 0),
                    float4(-sin(angle) , 0 , cos(angle),0),
                    float4(0 , 1 ,0 ,1)
                }; 
                float4x4 RM={
                    float4(0, 0 , 0 , 0),
                    float4(0 , 1 ,0 , 0),
                    float4(0 , 0 , 0,0),
                    float4(0 , 1 ,0 ,1)
                };  */      
                o.uv0 = v.texcoord0;							
				v.vertex = mul(Rotation(_Rotation),v.vertex);
				v.vertex +=Offset;
				o.pos = v.vertex;
				CURVED_WORLD_TRANSFORM_POINT(o.pos,_CurveFactor);
				o.pos = UnityObjectToClipPos(o.pos);
				o.posWorld = mul(UNITY_MATRIX_M, v.vertex);	
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
              
                fixed4 _Diffuse_map_var = tex2D(_MainTex,i.uv0);
				fixed3 diff=_Diffuse_map_var.rgb;
				       diff = lerp(diff, _VertexColor.xyz,i.color.rgb)+diff;	
                return fixed4(diff,1);
            }
            ENDHLSL
        }
    }
    FallBack Off
}
