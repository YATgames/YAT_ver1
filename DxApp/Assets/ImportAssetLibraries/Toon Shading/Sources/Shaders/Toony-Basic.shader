// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Toon/Basic" {
	Properties {
		_Color ("Main Color", Color) = (.5,.5,.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_DirtyTex("Dirty (RGB)", 2D) = "clear" { }
		_DirtyRate("Dirty Rate", Range(0.0, 1.0)) = 0
		_ToonShade ("ToonShader Cubemap(RGB)", CUBE) = "" { /*Texgen CubeNormal*/ }
		_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
	}


	SubShader {
		Tags { "RenderType"="Opaque" }
		Pass {
			Name "BASE"
			Cull Off
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest 

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _DirtyTex;
			float _DirtyRate;
			samplerCUBE _ToonShade;
			float4 _MainTex_ST;
			float4 _Color;
			float4 _AmbientColor;

			struct appdata {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
			};
			
			struct v2f {
				float4 pos : POSITION;
				float2 texcoord : TEXCOORD0;
				float3 cubenormal : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.cubenormal = mul (UNITY_MATRIX_MV, float4(v.normal,0));			
				return o;
			}

			float4 frag (v2f i) : COLOR
			{
				float4 col = _Color * tex2D(_MainTex, i.texcoord) * _AmbientColor;
				
				if (_DirtyRate > 0)
				{
					float4 colDirty = tex2D(_DirtyTex, i.texcoord);
					if (colDirty.a > 0)
					{
						float alphaDirty = colDirty.a * _DirtyRate;
						col.rgb = (col.rgb * (1 - alphaDirty))+ (colDirty.rgb * alphaDirty);
					}
				}

				float4 cube = texCUBE(_ToonShade, i.cubenormal);
				return float4(2.0f * cube.rgb * col.rgb, col.a);
			}
			ENDCG			
		}
	} 

	SubShader {
		Tags { "RenderType"="Opaque" }
		Pass {
			Name "BASE"
			Cull Off
			SetTexture [_MainTex] {
				constantColor [_Color]
				Combine texture * constant
			} 
			SetTexture [_ToonShade] {
				combine texture * previous DOUBLE, previous
			}
		}
	} 
	
	Fallback "VertexLit"
}
