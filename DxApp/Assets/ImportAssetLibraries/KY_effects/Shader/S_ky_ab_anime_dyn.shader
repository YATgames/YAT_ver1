// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.27 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.27;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True;n:type:ShaderForge.SFN_Final,id:4795,x:35056,y:32677,varname:node_4795,prsc:2|emission-2393-OUT,alpha-9223-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:32099,y:32749,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:98b79545bfbd548e991698b962752022,ntxv:0,isnm:False|UVIN-2677-OUT;n:type:ShaderForge.SFN_Multiply,id:2393,x:34757,y:32750,varname:node_2393,prsc:2|A-5474-OUT,B-3265-RGB,C-969-OUT;n:type:ShaderForge.SFN_VertexColor,id:2053,x:34309,y:32864,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Subtract,id:5287,x:30039,y:32818,varname:node_5287,prsc:2|A-1439-OUT,B-9176-OUT;n:type:ShaderForge.SFN_Vector1,id:9176,x:29831,y:32932,varname:node_9176,prsc:2,v1:16;n:type:ShaderForge.SFN_If,id:4634,x:30223,y:32630,varname:node_4634,prsc:2|A-1439-OUT,B-9176-OUT,GT-5287-OUT,EQ-5287-OUT,LT-1439-OUT;n:type:ShaderForge.SFN_Vector1,id:9756,x:29941,y:32988,varname:node_9756,prsc:2,v1:32;n:type:ShaderForge.SFN_If,id:6486,x:30491,y:32723,varname:node_6486,prsc:2|A-1439-OUT,B-9756-OUT,GT-8482-OUT,EQ-8482-OUT,LT-4634-OUT;n:type:ShaderForge.SFN_Subtract,id:8482,x:30141,y:33096,varname:node_8482,prsc:2|A-1439-OUT,B-9756-OUT;n:type:ShaderForge.SFN_If,id:5532,x:30629,y:32967,varname:node_5532,prsc:2|A-6486-OUT,B-1118-OUT,GT-2748-OUT,EQ-2748-OUT,LT-1962-OUT;n:type:ShaderForge.SFN_If,id:8029,x:30855,y:32967,varname:node_8029,prsc:2|A-6486-OUT,B-3319-OUT,GT-5742-OUT,EQ-5742-OUT,LT-5532-OUT;n:type:ShaderForge.SFN_If,id:9617,x:31082,y:32920,varname:node_9617,prsc:2|A-6486-OUT,B-2725-OUT,GT-1090-OUT,EQ-1090-OUT,LT-8029-OUT;n:type:ShaderForge.SFN_Vector1,id:1118,x:30349,y:33058,varname:node_1118,prsc:2,v1:4;n:type:ShaderForge.SFN_Vector1,id:2748,x:30345,y:33129,varname:node_2748,prsc:2,v1:2;n:type:ShaderForge.SFN_Vector1,id:1962,x:30329,y:33309,varname:node_1962,prsc:2,v1:3;n:type:ShaderForge.SFN_Vector1,id:3319,x:30549,y:33155,varname:node_3319,prsc:2,v1:8;n:type:ShaderForge.SFN_Vector1,id:5742,x:30520,y:33330,varname:node_5742,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:2725,x:30804,y:33125,varname:node_2725,prsc:2,v1:12;n:type:ShaderForge.SFN_Vector1,id:1090,x:30804,y:33291,varname:node_1090,prsc:2,v1:0;n:type:ShaderForge.SFN_Append,id:3292,x:31238,y:32823,varname:node_3292,prsc:2|A-1439-OUT,B-9617-OUT;n:type:ShaderForge.SFN_Floor,id:8593,x:31447,y:32864,varname:node_8593,prsc:2|IN-3292-OUT;n:type:ShaderForge.SFN_Divide,id:6533,x:31675,y:32945,varname:node_6533,prsc:2|A-8593-OUT,B-5706-OUT;n:type:ShaderForge.SFN_Vector1,id:5706,x:31476,y:33086,varname:node_5706,prsc:2,v1:4;n:type:ShaderForge.SFN_Add,id:2677,x:31852,y:32895,varname:node_2677,prsc:2|A-5795-OUT,B-6533-OUT;n:type:ShaderForge.SFN_TexCoord,id:7870,x:31273,y:32562,varname:node_7870,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:5795,x:31649,y:32711,varname:node_5795,prsc:2|A-7870-UVOUT,B-2049-OUT;n:type:ShaderForge.SFN_Vector1,id:2049,x:31451,y:32769,varname:node_2049,prsc:2,v1:0.25;n:type:ShaderForge.SFN_If,id:335,x:33637,y:32831,varname:node_335,prsc:2|A-1439-OUT,B-2275-OUT,GT-5708-G,EQ-5708-G,LT-6724-OUT;n:type:ShaderForge.SFN_If,id:5474,x:33879,y:32713,varname:node_5474,prsc:2|A-1439-OUT,B-5574-OUT,GT-5708-B,EQ-5708-B,LT-335-OUT;n:type:ShaderForge.SFN_Vector1,id:2275,x:33255,y:32627,varname:node_2275,prsc:2,v1:16;n:type:ShaderForge.SFN_Vector1,id:5574,x:33652,y:32617,varname:node_5574,prsc:2,v1:32;n:type:ShaderForge.SFN_ComponentMask,id:5708,x:32453,y:32515,varname:node_5708,prsc:2,cc1:0,cc2:1,cc3:2,cc4:-1|IN-5087-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:6724,x:33483,y:32475,ptovrint:False,ptlb:pickupCh,ptin:_pickupCh,varname:node_6724,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-5708-R,B-7397-OUT;n:type:ShaderForge.SFN_ValueProperty,id:969,x:34517,y:32833,ptovrint:False,ptlb:emissive,ptin:_emissive,varname:node_969,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_ValueProperty,id:573,x:32040,y:32515,ptovrint:False,ptlb:texDensity,ptin:_texDensity,varname:node_573,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Power,id:5087,x:32263,y:32515,varname:node_5087,prsc:2|VAL-6074-RGB,EXP-573-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:4941,x:32943,y:32137,ptovrint:False,ptlb:useR,ptin:_useR,varname:node_4941,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:True|A-516-OUT,B-5708-R;n:type:ShaderForge.SFN_Vector1,id:516,x:32699,y:32261,varname:node_516,prsc:2,v1:0;n:type:ShaderForge.SFN_SwitchProperty,id:7302,x:32943,y:32280,ptovrint:False,ptlb:useG,ptin:_useG,varname:node_7302,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-516-OUT,B-5708-G;n:type:ShaderForge.SFN_SwitchProperty,id:1139,x:32943,y:32416,ptovrint:False,ptlb:useB,ptin:_useB,varname:node_1139,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-516-OUT,B-5708-B;n:type:ShaderForge.SFN_Add,id:5541,x:33142,y:32157,varname:node_5541,prsc:2|A-4941-OUT,B-7302-OUT;n:type:ShaderForge.SFN_Add,id:7397,x:33316,y:32279,varname:node_7397,prsc:2|A-5541-OUT,B-1139-OUT;n:type:ShaderForge.SFN_Power,id:7662,x:34168,y:33093,varname:node_7662,prsc:2|VAL-2254-OUT,EXP-4568-OUT;n:type:ShaderForge.SFN_Multiply,id:9223,x:34788,y:33062,varname:node_9223,prsc:2|A-879-OUT,B-7225-OUT,C-3624-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4568,x:33939,y:33142,ptovrint:False,ptlb:alphaDensity,ptin:_alphaDensity,varname:node_4568,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:7225,x:34492,y:33127,varname:node_7225,prsc:2|A-7662-OUT,B-8270-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8270,x:34168,y:33302,ptovrint:False,ptlb:alphaPower,ptin:_alphaPower,varname:node_8270,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Power,id:879,x:34602,y:32926,varname:node_879,prsc:2|VAL-2053-A,EXP-7358-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7358,x:34422,y:33021,ptovrint:False,ptlb:vertColorDensity,ptin:_vertColorDensity,varname:node_7358,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Color,id:3265,x:34131,y:32770,ptovrint:False,ptlb:color,ptin:_color,varname:node_3265,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_VertexColor,id:6686,x:29183,y:32233,varname:node_6686,prsc:2;n:type:ShaderForge.SFN_Multiply,id:1439,x:29567,y:32523,varname:node_1439,prsc:2|A-6686-R,B-5796-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5796,x:29034,y:32654,ptovrint:False,ptlb:uvDynCorrect,ptin:_uvDynCorrect,varname:node_5796,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_SwitchProperty,id:2254,x:33848,y:32986,ptovrint:False,ptlb:haveAlpha,ptin:_haveAlpha,varname:node_2254,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-5474-OUT,B-6074-A;n:type:ShaderForge.SFN_DepthBlend,id:2623,x:34363,y:33443,varname:node_2623,prsc:2|DIST-1750-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1750,x:34127,y:33473,ptovrint:False,ptlb:depthBlend,ptin:_depthBlend,varname:node_1750,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_SwitchProperty,id:3624,x:34605,y:33331,ptovrint:False,ptlb:notUseDepthBlend,ptin:_notUseDepthBlend,varname:node_3624,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-2623-OUT,B-2384-OUT;n:type:ShaderForge.SFN_Vector1,id:2384,x:34384,y:33344,varname:node_2384,prsc:2,v1:1;proporder:6074-6724-969-573-4941-7302-1139-4568-8270-7358-3265-5796-2254-1750-3624;pass:END;sub:END;*/

Shader "KY/ab_anime_dyn" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        [MaterialToggle] _pickupCh ("pickupCh", Float ) = 0
        _emissive ("emissive", Float ) = 2
        _texDensity ("texDensity", Float ) = 2
        [MaterialToggle] _useR ("useR", Float ) = 0
        [MaterialToggle] _useG ("useG", Float ) = 0
        [MaterialToggle] _useB ("useB", Float ) = 0
        _alphaDensity ("alphaDensity", Float ) = 1
        _alphaPower ("alphaPower", Float ) = 1
        _vertColorDensity ("vertColorDensity", Float ) = 1
        _color ("color", Color) = (1,1,1,1)
        _uvDynCorrect ("uvDynCorrect", Float ) = 1
        [MaterialToggle] _haveAlpha ("haveAlpha", Float ) = 0
        _depthBlend ("depthBlend", Float ) = 1
        [MaterialToggle] _notUseDepthBlend ("notUseDepthBlend", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
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
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _CameraDepthTexture;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform fixed _pickupCh;
            uniform float _emissive;
            uniform float _texDensity;
            uniform fixed _useR;
            uniform fixed _useG;
            uniform fixed _useB;
            uniform float _alphaDensity;
            uniform float _alphaPower;
            uniform float _vertColorDensity;
            uniform float4 _color;
            uniform float _uvDynCorrect;
            uniform fixed _haveAlpha;
            uniform float _depthBlend;
            uniform fixed _notUseDepthBlend;
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
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
////// Lighting:
////// Emissive:
                float node_1439 = (i.vertexColor.r*_uvDynCorrect);
                float node_5474_if_leA = step(node_1439,32.0);
                float node_5474_if_leB = step(32.0,node_1439);
                float node_335_if_leA = step(node_1439,16.0);
                float node_335_if_leB = step(16.0,node_1439);
                float node_9756 = 32.0;
                float node_6486_if_leA = step(node_1439,node_9756);
                float node_6486_if_leB = step(node_9756,node_1439);
                float node_9176 = 16.0;
                float node_4634_if_leA = step(node_1439,node_9176);
                float node_4634_if_leB = step(node_9176,node_1439);
                float node_5287 = (node_1439-node_9176);
                float node_8482 = (node_1439-node_9756);
                float node_6486 = lerp((node_6486_if_leA*lerp((node_4634_if_leA*node_1439)+(node_4634_if_leB*node_5287),node_5287,node_4634_if_leA*node_4634_if_leB))+(node_6486_if_leB*node_8482),node_8482,node_6486_if_leA*node_6486_if_leB);
                float node_9617_if_leA = step(node_6486,12.0);
                float node_9617_if_leB = step(12.0,node_6486);
                float node_8029_if_leA = step(node_6486,8.0);
                float node_8029_if_leB = step(8.0,node_6486);
                float node_5532_if_leA = step(node_6486,4.0);
                float node_5532_if_leB = step(4.0,node_6486);
                float node_2748 = 2.0;
                float node_5742 = 1.0;
                float node_1090 = 0.0;
                float2 node_2677 = ((i.uv0*0.25)+(floor(float2(node_1439,lerp((node_9617_if_leA*lerp((node_8029_if_leA*lerp((node_5532_if_leA*3.0)+(node_5532_if_leB*node_2748),node_2748,node_5532_if_leA*node_5532_if_leB))+(node_8029_if_leB*node_5742),node_5742,node_8029_if_leA*node_8029_if_leB))+(node_9617_if_leB*node_1090),node_1090,node_9617_if_leA*node_9617_if_leB)))/4.0));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_2677, _MainTex));
                float3 node_5708 = pow(_MainTex_var.rgb,_texDensity).rgb;
                float node_516 = 0.0;
                float node_5474 = lerp((node_5474_if_leA*lerp((node_335_if_leA*lerp( node_5708.r, ((lerp( node_516, node_5708.r, _useR )+lerp( node_516, node_5708.g, _useG ))+lerp( node_516, node_5708.b, _useB )), _pickupCh ))+(node_335_if_leB*node_5708.g),node_5708.g,node_335_if_leA*node_335_if_leB))+(node_5474_if_leB*node_5708.b),node_5708.b,node_5474_if_leA*node_5474_if_leB);
                float3 emissive = (node_5474*_color.rgb*_emissive);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,(pow(i.vertexColor.a,_vertColorDensity)*(pow(lerp( node_5474, _MainTex_var.a, _haveAlpha ),_alphaDensity)*_alphaPower)*lerp( saturate((sceneZ-partZ)/_depthBlend), 1.0, _notUseDepthBlend )));
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0,0,0,1));
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
