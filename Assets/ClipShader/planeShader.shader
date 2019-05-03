Shader "ClipShader/Plane"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		[HDR]_MainColor("Color", Color) = (1, 1, 1, 1)
	}

		SubShader
		{
			Tags { "RenderType" = "Opaque" "Queue" = "Geometry+1"}

			Pass
			{
				Stencil
				{
					Ref 1
					Comp Equal
					ZFail Replace
				}

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata_t {
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					half2 texcoord : TEXCOORD0;

				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float4 _MainColor;

				v2f vert(appdata_t v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.texcoord = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.texcoord) *_MainColor;
					return col;
				}
			ENDCG
		}
		}
}