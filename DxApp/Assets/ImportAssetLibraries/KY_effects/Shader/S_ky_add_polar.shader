// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.27 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.27;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True;n:type:ShaderForge.SFN_Final,id:4795,x:34550,y:32710,varname:node_4795,prsc:2|emission-3898-OUT;n:type:ShaderForge.SFN_VertexColor,id:2053,x:33212,y:32990,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:880,x:33466,y:32873,ptovrint:False,ptlb:emissive,ptin:_emissive,varname:node_880,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:3575,x:32951,y:32850,ptovrint:False,ptlb:baseTexDensity,ptin:_baseTexDensity,varname:node_3575,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Power,id:8758,x:33237,y:32806,varname:node_8758,prsc:2|VAL-9169-RGB,EXP-3575-OUT;n:type:ShaderForge.SFN_Multiply,id:3898,x:34070,y:32784,varname:node_3898,prsc:2|A-8758-OUT,B-880-OUT,C-2053-RGB,D-5866-OUT;n:type:ShaderForge.SFN_Multiply,id:5866,x:34169,y:33136,varname:node_5866,prsc:2|A-673-OUT,B-2053-A,C-746-OUT,D-2163-OUT;n:type:ShaderForge.SFN_Power,id:1567,x:33183,y:33171,varname:node_1567,prsc:2|VAL-9047-R,EXP-9704-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9704,x:32916,y:33216,ptovrint:False,ptlb:alphaDensity,ptin:_alphaDensity,varname:node_9704,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Add,id:6878,x:33472,y:33303,varname:node_6878,prsc:2|A-2053-R,B-2053-G,C-2053-B;n:type:ShaderForge.SFN_Clamp01,id:746,x:33875,y:33304,varname:node_746,prsc:2|IN-3741-OUT;n:type:ShaderForge.SFN_Power,id:3741,x:33689,y:33345,varname:node_3741,prsc:2|VAL-6878-OUT,EXP-8595-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8595,x:33407,y:33465,ptovrint:False,ptlb:vertColorDensity,ptin:_vertColorDensity,varname:node_8595,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:8940,x:33183,y:33335,ptovrint:False,ptlb:alphaPower,ptin:_alphaPower,varname:node_8940,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:673,x:33689,y:33154,varname:node_673,prsc:2|A-1567-OUT,B-8940-OUT;n:type:ShaderForge.SFN_Tex2d,id:9047,x:32768,y:33051,ptovrint:False,ptlb:maskTex,ptin:_maskTex,varname:node_9047,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:00d1dc61304fe4183bb5dae9ee4dd842,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:9169,x:32641,y:32711,ptovrint:False,ptlb:baseTex,ptin:_baseTex,varname:node_9169,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:b4e03d3d1079042ef866bcaa089de714,ntxv:0,isnm:False|UVIN-3714-OUT;n:type:ShaderForge.SFN_TexCoord,id:8106,x:30925,y:32456,varname:node_8106,prsc:2,uv:0;n:type:ShaderForge.SFN_ComponentMask,id:8219,x:31468,y:32692,varname:node_8219,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-2818-OUT;n:type:ShaderForge.SFN_Tau,id:4411,x:31824,y:32733,varname:node_4411,prsc:2;n:type:ShaderForge.SFN_Length,id:1092,x:31664,y:32404,varname:node_1092,prsc:2|IN-2818-OUT;n:type:ShaderForge.SFN_ArcTan2,id:4708,x:31690,y:32702,varname:node_4708,prsc:2,attp:0|A-8219-G,B-8219-R;n:type:ShaderForge.SFN_Divide,id:7856,x:31906,y:32577,varname:node_7856,prsc:2|A-4708-OUT,B-4411-OUT;n:type:ShaderForge.SFN_Add,id:3062,x:32084,y:32577,varname:node_3062,prsc:2|A-7856-OUT,B-9298-OUT;n:type:ShaderForge.SFN_Vector1,id:9298,x:31947,y:32758,varname:node_9298,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Append,id:4114,x:32239,y:32487,varname:node_4114,prsc:2|A-1092-OUT,B-3062-OUT;n:type:ShaderForge.SFN_Append,id:3261,x:32188,y:32813,varname:node_3261,prsc:2|A-3652-OUT,B-9791-OUT;n:type:ShaderForge.SFN_Add,id:3714,x:32391,y:32658,varname:node_3714,prsc:2|A-4114-OUT,B-3261-OUT;n:type:ShaderForge.SFN_Multiply,id:5869,x:31149,y:32531,varname:node_5869,prsc:2|A-8106-UVOUT,B-952-OUT;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:2818,x:31316,y:32657,varname:node_2818,prsc:2|IN-5869-OUT,IMIN-5942-OUT,IMAX-952-OUT,OMIN-5886-OUT,OMAX-952-OUT;n:type:ShaderForge.SFN_Vector1,id:5942,x:31098,y:32758,varname:node_5942,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:4496,x:30950,y:32900,varname:node_4496,prsc:2,v1:-1;n:type:ShaderForge.SFN_Multiply,id:5886,x:31120,y:32838,varname:node_5886,prsc:2|A-952-OUT,B-4496-OUT;n:type:ShaderForge.SFN_ValueProperty,id:952,x:30765,y:32756,ptovrint:False,ptlb:polarDensity,ptin:_polarDensity,varname:node_952,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:3652,x:31906,y:32855,ptovrint:False,ptlb:polarOffsetX,ptin:_polarOffsetX,varname:node_3652,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:9791,x:31906,y:32970,ptovrint:False,ptlb:polarOffsetY,ptin:_polarOffsetY,varname:node_9791,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_DepthBlend,id:7938,x:33875,y:33546,varname:node_7938,prsc:2|DIST-9958-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9958,x:33568,y:33560,ptovrint:False,ptlb:depthBlend,ptin:_depthBlend,varname:node_9958,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_SwitchProperty,id:2163,x:34088,y:33624,ptovrint:False,ptlb:useDepthBlend,ptin:_useDepthBlend,varname:node_2163,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:True|A-1337-OUT,B-7938-OUT;n:type:ShaderForge.SFN_Vector1,id:1337,x:33869,y:33760,varname:node_1337,prsc:2,v1:1;proporder:880-3575-9704-8595-8940-9047-9169-952-3652-9791-9958-2163;pass:END;sub:END;*/

Shader "KY/add_polar" {
    Properties {
        _emissive ("emissive", Float ) = 1
        _baseTexDensity ("baseTexDensity", Float ) = 1
        _alphaDensity ("alphaDensity", Float ) = 1
        _vertColorDensity ("vertColorDensity", Float ) = 1
        _alphaPower ("alphaPower", Float ) = 1
        _maskTex ("maskTex", 2D) = "white" {}
        _baseTex ("baseTex", 2D) = "white" {}
        _polarDensity ("polarDensity", Float ) = 1
        _polarOffsetX ("polarOffsetX", Float ) = 0
        _polarOffsetY ("polarOffsetY", Float ) = 0
        _depthBlend ("depthBlend", Float ) = 1
        [MaterialToggle] _useDepthBlend ("useDepthBlend", Float ) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _CameraDepthTexture;
            uniform float _emissive;
            uniform float _baseTexDensity;
            uniform float _alphaDensity;
            uniform float _vertColorDensity;
            uniform float _alphaPower;
            uniform sampler2D _maskTex; uniform float4 _maskTex_ST;
            uniform sampler2D _baseTex; uniform float4 _baseTex_ST;
            uniform float _polarDensity;
            uniform float _polarOffsetX;
            uniform float _polarOffsetY;
            uniform float _depthBlend;
            uniform fixed _useDepthBlend;
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
                o.pos = UnityObjectToClipPos(v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
////// Lighting:
////// Emissive:
                float node_5942 = 0.0;
                float node_5886 = (_polarDensity*(-1.0));
                float2 node_2818 = (node_5886 + ( ((i.uv0*_polarDensity) - node_5942) * (_polarDensity - node_5886) ) / (_polarDensity - node_5942));
                float2 node_8219 = node_2818.rg;
                float2 node_3714 = (float2(length(node_2818),((atan2(node_8219.g,node_8219.r)/6.28318530718)+0.5))+float2(_polarOffsetX,_polarOffsetY));
                float4 _baseTex_var = tex2D(_baseTex,TRANSFORM_TEX(node_3714, _baseTex));
                float4 _maskTex_var = tex2D(_maskTex,TRANSFORM_TEX(i.uv0, _maskTex));
                float3 emissive = (pow(_baseTex_var.rgb,_baseTexDensity)*_emissive*i.vertexColor.rgb*((pow(_maskTex_var.r,_alphaDensity)*_alphaPower)*i.vertexColor.a*saturate(pow((i.vertexColor.r+i.vertexColor.g+i.vertexColor.b),_vertColorDensity))*lerp( 1.0, saturate((sceneZ-partZ)/_depthBlend), _useDepthBlend )));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
