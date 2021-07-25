// Shader created with Shader Forge v1.40 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.40;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,cpap:True,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:7215,x:33152,y:32416,varname:node_7215,prsc:2|emission-5134-OUT;n:type:ShaderForge.SFN_RgbToHsv,id:3453,x:32043,y:32996,varname:node_3453,prsc:2|IN-9933-RGB;n:type:ShaderForge.SFN_HsvToRgb,id:9493,x:32424,y:32997,varname:node_9493,prsc:2|H-7355-OUT,S-3453-SOUT,V-3453-VOUT;n:type:ShaderForge.SFN_Posterize,id:7355,x:32239,y:33096,varname:node_7355,prsc:2|IN-3453-HOUT,STPS-1540-OUT;n:type:ShaderForge.SFN_Vector1,id:1540,x:32043,y:33172,varname:node_1540,prsc:2,v1:20;n:type:ShaderForge.SFN_SceneDepth,id:2473,x:32583,y:32385,varname:node_2473,prsc:2;n:type:ShaderForge.SFN_SceneDepth,id:9755,x:32583,y:32258,varname:node_9755,prsc:2|UV-5629-OUT;n:type:ShaderForge.SFN_TexCoord,id:7451,x:32201,y:32162,varname:node_7451,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:5629,x:32394,y:32258,varname:node_5629,prsc:2|A-7451-UVOUT,B-9927-OUT;n:type:ShaderForge.SFN_ScreenParameters,id:8208,x:32001,y:32378,varname:node_8208,prsc:2;n:type:ShaderForge.SFN_Divide,id:9927,x:32201,y:32324,varname:node_9927,prsc:2|A-9944-OUT,B-8208-PXW;n:type:ShaderForge.SFN_Vector1,id:9944,x:32030,y:32324,varname:node_9944,prsc:2,v1:1;n:type:ShaderForge.SFN_Blend,id:5703,x:32755,y:32323,varname:node_5703,prsc:2,blmd:17,clmp:True|SRC-9755-OUT,DST-2473-OUT;n:type:ShaderForge.SFN_Subtract,id:7456,x:32394,y:32385,varname:node_7456,prsc:2|A-7451-UVOUT,B-9927-OUT;n:type:ShaderForge.SFN_Code,id:7442,x:32259,y:32530,varname:node_7442,prsc:2,code:ZgBsAG8AYQB0ADQAIABjAGQAbgAgAD0AIAB0AGUAeAAyAEQAKABUAGUAeAAsACAAVQBWACkAOwAKAGYAbABvAGEAdAAzACAAbgAgAD0AIABEAGUAYwBvAGQAZQBWAGkAZQB3AE4AbwByAG0AYQBsAFMAdABlAHIAZQBvACgAYwBkAG4AKQAgACoAIABmAGwAbwBhAHQAMwAoADEALgAwACwAIAAxAC4AMAAsACAALQAxAC4AMAApADsACgByAGUAdAB1AHIAbgAgAG4AOwA=,output:2,fname:nm,width:247,height:112,input:1,input:12,input_1_label:UV,input_2_label:Tex|A-8098-UVOUT,B-4001-TEX;n:type:ShaderForge.SFN_TexCoord,id:8098,x:31937,y:32528,varname:node_8098,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Lerp,id:5134,x:32881,y:32748,varname:node_5134,prsc:2|A-9493-OUT,B-6125-RGB,T-3296-OUT;n:type:ShaderForge.SFN_Color,id:6125,x:32640,y:32958,ptovrint:False,ptlb:OutLineColor,ptin:_OutLineColor,varname:node_6125,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Tex2dAsset,id:4001,x:31937,y:32680,ptovrint:False,ptlb:CameraDepthNormalsTexture,ptin:_CameraDepthNormalsTexture,varname:node_4001,glob:True,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Tex2d,id:9933,x:31866,y:32996,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_9933,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Distance,id:286,x:32624,y:32591,varname:node_286,prsc:2|A-7442-OUT,B-6236-OUT;n:type:ShaderForge.SFN_Code,id:6236,x:32259,y:32675,varname:node_6236,prsc:2,code:ZgBsAG8AYQB0ADQAIABjAGQAbgAgAD0AIAB0AGUAeAAyAEQAKABUAGUAeAAsACAAVQBWACkAOwAKAGYAbABvAGEAdAAzACAAbgAgAD0AIABEAGUAYwBvAGQAZQBWAGkAZQB3AE4AbwByAG0AYQBsAFMAdABlAHIAZQBvACgAYwBkAG4AKQAgACoAIABmAGwAbwBhAHQAMwAoADEALgAwACwAIAAxAC4AMAAsACAALQAxAC4AMAApADsACgByAGUAdAB1AHIAbgAgAG4AOwA=,output:2,fname:nm2,width:247,height:112,input:1,input:12,input_1_label:UV,input_2_label:Tex|A-5629-OUT,B-4001-TEX;n:type:ShaderForge.SFN_Blend,id:3296,x:32812,y:32498,varname:node_3296,prsc:2,blmd:10,clmp:True|SRC-5703-OUT,DST-286-OUT;proporder:6125-9933;pass:END;sub:END;*/

Shader "Hidden/posteffect" {
    Properties {
        _OutLineColor ("OutLineColor", Color) = (0.5,0.5,0.5,1)
        _MainTex ("MainTex", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma target 3.0
            uniform sampler2D _CameraDepthTexture;
            float3 nm( float2 UV , sampler2D Tex ){
            float4 cdn = tex2D(Tex, UV);
            float3 n = DecodeViewNormalStereo(cdn) * float3(1.0, 1.0, -1.0);
            return n;
            }
            
            uniform sampler2D _CameraDepthNormalsTexture; uniform float4 _CameraDepthNormalsTexture_ST;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            float3 nm2( float2 UV , sampler2D Tex ){
            float4 cdn = tex2D(Tex, UV);
            float3 n = DecodeViewNormalStereo(cdn) * float3(1.0, 1.0, -1.0);
            return n;
            }
            
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float4, _OutLineColor)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv0 : TEXCOORD0;
                float4 projPos : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
////// Lighting:
////// Emissive:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float4 node_3453_k = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 node_3453_p = lerp(float4(float4(_MainTex_var.rgb,0.0).zy, node_3453_k.wz), float4(float4(_MainTex_var.rgb,0.0).yz, node_3453_k.xy), step(float4(_MainTex_var.rgb,0.0).z, float4(_MainTex_var.rgb,0.0).y));
                float4 node_3453_q = lerp(float4(node_3453_p.xyw, float4(_MainTex_var.rgb,0.0).x), float4(float4(_MainTex_var.rgb,0.0).x, node_3453_p.yzx), step(node_3453_p.x, float4(_MainTex_var.rgb,0.0).x));
                float node_3453_d = node_3453_q.x - min(node_3453_q.w, node_3453_q.y);
                float node_3453_e = 1.0e-10;
                float3 node_3453 = float3(abs(node_3453_q.z + (node_3453_q.w - node_3453_q.y) / (6.0 * node_3453_d + node_3453_e)), node_3453_d / (node_3453_q.x + node_3453_e), node_3453_q.x);;
                float node_1540 = 20.0;
                float4 _OutLineColor_var = UNITY_ACCESS_INSTANCED_PROP( Props, _OutLineColor );
                float node_9927 = (1.0/_ScreenParams.r);
                float2 node_5629 = (i.uv0+node_9927);
                float node_286 = distance(nm( i.uv0 , _CameraDepthNormalsTexture ),nm2( node_5629 , _CameraDepthNormalsTexture ));
                float3 emissive = lerp((lerp(float3(1,1,1),saturate(3.0*abs(1.0-2.0*frac(floor(node_3453.r * node_1540) / (node_1540 - 1)+float3(0.0,-1.0/3.0,1.0/3.0)))-1),node_3453.g)*node_3453.b),_OutLineColor_var.rgb,saturate(( node_286 > 0.5 ? (1.0-(1.0-2.0*(node_286-0.5))*(1.0-saturate(abs(max(0, LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, node_5629)) - _ProjectionParams.g)-max(0, LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sceneUVs)) - _ProjectionParams.g))))) : (2.0*node_286*saturate(abs(max(0, LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, node_5629)) - _ProjectionParams.g)-max(0, LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sceneUVs)) - _ProjectionParams.g)))) )));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
