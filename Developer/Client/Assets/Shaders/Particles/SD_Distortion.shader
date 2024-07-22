// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32609,y:32477,varname:node_4795,prsc:2|alpha-7966-OUT,refract-2191-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:31675,y:32689,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_VertexColor,id:2053,x:31829,y:32862,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Append,id:5264,x:31906,y:32706,varname:node_5264,prsc:2|A-6074-R,B-6074-G;n:type:ShaderForge.SFN_Vector1,id:7966,x:32143,y:32512,varname:node_7966,prsc:2,v1:0;n:type:ShaderForge.SFN_Multiply,id:2191,x:32207,y:32836,varname:node_2191,prsc:2|A-5264-OUT,B-6074-A,C-2053-A,D-9537-OUT;n:type:ShaderForge.SFN_Slider,id:9537,x:31613,y:33050,ptovrint:False,ptlb:Amount,ptin:_Amount,varname:node_9537,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;proporder:6074-9537;pass:END;sub:END;*/

Shader "SD/Particles/SD_Distortion" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _Amount ("Amount", Range(0, 1)) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
     //   GrabPass{ }
        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Assets/Shaders/Includes/WorldCurvedCG.hlsl"

            uniform sampler2D _CameraGrabTexture;
            uniform sampler2D _MainTex;
            CBUFFER_START(UnityPerMaterial)
             uniform float4 _MainTex_ST;
            uniform fixed _Amount;
            CBUFFER_END

            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                float4 projPos : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                o.projPos.z = -TransformWorldToView(TransformObjectToWorld(v.vertex.xyz)).z;
                //COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float2 sceneUVs = (i.projPos.xy / i.projPos.w) + (float2(_MainTex_var.r,_MainTex_var.g)*_MainTex_var.a*i.vertexColor.a*_Amount);
                float4 sceneColor = tex2D(_CameraGrabTexture, sceneUVs);
////// Lighting:
                float3 finalColor = 0;
				return fixed4(lerp(sceneColor.rgb, finalColor, 0.0), 1);
            }
            ENDHLSL
        }
    }
    FallBack Off
}
