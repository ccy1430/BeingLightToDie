Shader "Custom/UIBloom"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_LightColor("Light Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Size("Size", Int) = 1
	}
		SubShader
		{
			Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha
			// No culling or depth
			Cull Off ZWrite Off ZTest Always

			CGINCLUDE
		#include "UnityCG.cginc"

		struct v2f {
			float4 pos : SV_POSITION;
			float2 uv: TEXCOORD0;
		};

		sampler2D _MainTex;
		float4 _MainTex_ST;
		half4 _MainTex_TexelSize;
		float4 _LightColor;
		int _Size;

		ENDCG

		Pass {

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct selfv2f {
				float4 pos :POSITION;
				float2 uv:TEXCOORD0;
				fixed4 color : COLOR;

			};

			selfv2f vert(appdata_full IN)
			{
				selfv2f OUT;
				OUT.pos = UnityObjectToClipPos(IN.vertex);
				OUT.uv = TRANSFORM_TEX(IN.texcoord, _MainTex);
				OUT.color = IN.color;
				return OUT;
			}

			fixed4 frag(selfv2f IN) : SV_Target
			{
				fixed4 color = fixed4(IN.color.rgb, IN.color.a * tex2D(_MainTex, IN.uv).a);
				fixed4 c = _LightColor;
				float sum = tex2D(_MainTex, IN.uv).a * 4;
				/*for (int 1 = 1; 1 <= _Size; 1++) {

				}*/
				sum += tex2D(_MainTex, IN.uv + _Size * _MainTex_TexelSize.xy * half2(1, 0)).a * 2;
				sum += tex2D(_MainTex, IN.uv + _Size * _MainTex_TexelSize.xy * half2(-1 * 1, 0)).a * 2;
				sum += tex2D(_MainTex, IN.uv + _Size * _MainTex_TexelSize.xy * half2(0, 1 * 1)).a * 2;
				sum += tex2D(_MainTex, IN.uv + _Size * _MainTex_TexelSize.xy * half2(0, -1 * 1)).a * 2;

				sum += tex2D(_MainTex, IN.uv + _Size * _MainTex_TexelSize.xy * half2(-1, 1)).a;
				sum += tex2D(_MainTex, IN.uv + _Size * _MainTex_TexelSize.xy * half2(-1, -1 * 1)).a;
				sum += tex2D(_MainTex, IN.uv + _Size * _MainTex_TexelSize.xy * half2(1, 1)).a;
				sum += tex2D(_MainTex, IN.uv + _Size * _MainTex_TexelSize.xy * half2(1, -1)).a;

				//c.a = sum / (16);
				sum = sum / (16);
				c.a = sum;
				return lerp(c, color, sum);
				//return c;
				//return step(0.5, color.a) * color  + step(0.5, 1 - color.a) * c;
			}

			ENDCG
		}
		}
}
