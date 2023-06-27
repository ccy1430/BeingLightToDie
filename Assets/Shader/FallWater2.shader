Shader "Unlit/FallWater2"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_FallSpeed("FallSpeed",Float) = 2
		_BaseColor("BaseColor",Color) = (0,0,0,0)
		_DeepColor("DeepColor",Color) = (0,0,1,1)
		_Seed("Seed",Float) = 1.56
	}
		SubShader
		{
			Tags
			{
				"RenderType" = "Transparent"
				"Queue" = "Transparent"
			}

			Cull Off
			Lighting Off
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float _FallSpeed;
				fixed4 _BaseColor;
				fixed4 _DeepColor;
				float _Seed;


				float random(float val)
				{
					return frac(sin(val * 1000));
				}

				float2 randomVec(float2 uv)
				{
					float angle = random(random(uv.x) + random(uv.y)) * 2 * UNITY_PI;
					return float2(cos(angle),sin(angle));
				}

				float perlinNoise(float2 uv)
				{
					float2 pi = floor(uv);
					float2 pf = uv - pi;
					float2 w = pow(pf, 3) * (6.0 * pf * pf - 15.0 * pf + 10);
					float2 g0 = randomVec(pi);
					float2 g1 = randomVec(pi + float2(1.0, 0.0));
					float2 g2 = randomVec(pi + float2(0, 1.0));
					float2 g3 = randomVec(pi + float2(1.0, 1.0));
					float p0 = dot(g0,pf);
					float p1 = dot(g1,float2(pf.x - 1,pf.y));
					float p2 = dot(g2,float2(pf.x,pf.y - 1));
					float p3 = dot(g3,pf - 1);
					return lerp(lerp(p0,p1,w.x),lerp(p2,p3,w.x),w.y);
				}

				float2 randomV2(float2 p)
				{

					p = float2(dot(p, float2(127.1, 311.7)),
						dot(p, float2(269.5, 183.3)));

					return frac(sin(p) * 43758.5453123);
				}
				float worleyNoise(float2 uv) {
					float2 i = floor(uv);
					float2 f = frac(uv);
					float m_dist = 1;
					float2 m_point;
					for (int x = -1; x < 2; x++) {
						for (int y = -1; y < 2; y++) {

							float2 neighbor = float2(x, y);
							//周围的特征点
							float2 neighborP = randomV2(i + neighbor);
							float dist = distance(f,neighborP + neighbor);
							if (dist < m_dist) {
								//最短距离
								m_dist = dist;
								//最近的特征点
								m_point = neighborP;
							}
						}
					}
					return m_dist;
				}


				float floatnoise(float p)
				{
					float pi = floor(p);
					float pf = p - pi;

					float w = pf * pf * (3.0 - 2.0 * pf);

					return random(pi) * (1 - w) + random(pi + 1) * w;
				}


				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					float2 uv = i.uv;
					//uv.y *= 2;
					uv.x += _Time.y * _FallSpeed;
					float uvnoise = perlinNoise(uv * 2);
					//uvnoise = sin(uvnoise * 1.57);
					//uvnoise = (uvnoise + 1) / 2;
					fixed4 col = lerp(_BaseColor, _DeepColor,uvnoise);
					//uv.y /= 2;
					//float dis = worleyNoise(uv);
					//col.a *= dis;
					//col.a = 3 * col.a * (1 - col.a) + col.a * col.a;
					col.a *= tex2D(_MainTex, i.uv).a;
					return col;
				}
				ENDCG
			}
		}
}
