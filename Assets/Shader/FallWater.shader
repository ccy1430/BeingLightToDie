Shader "Unlit/FallWater"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_BaseColor("BaseColor",Color) = (1,1,1,1)
		_LandColor("LandColor",Color) = (0,0,1,1)
		_FallSpeed("FallSpeed",Float) = 2
		_Seed("Seed",float) = 0
	}
		SubShader
		{
			Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			Cull Off
			ZWrite Off


			Pass
			{
				Tags { "LightMode" = "Universal2D" }

				HLSLPROGRAM
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

				#pragma vertex CombinedShapeLightVertex
				#pragma fragment CombinedShapeLightFragment

				#pragma multi_compile USE_SHAPE_LIGHT_TYPE_0 __
				#pragma multi_compile USE_SHAPE_LIGHT_TYPE_1 __
				#pragma multi_compile USE_SHAPE_LIGHT_TYPE_2 __
				#pragma multi_compile USE_SHAPE_LIGHT_TYPE_3 __
				#pragma multi_compile _ DEBUG_DISPLAY

				struct Attributes
				{
					float3 positionOS   : POSITION;
					float4 color        : COLOR;
					float2  uv          : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct Varyings
				{
					float4  positionCS  : SV_POSITION;
					half4   color       : COLOR;
					float2  uv          : TEXCOORD0;
					half2   lightingUV  : TEXCOORD1;
					#if defined(DEBUG_DISPLAY)
					float3  positionWS  : TEXCOORD2;
					#endif
					UNITY_VERTEX_OUTPUT_STEREO
				};

				#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"

				TEXTURE2D(_MainTex);
				SAMPLER(sampler_MainTex);
				TEXTURE2D(_MaskTex);
				SAMPLER(sampler_MaskTex);
				half4 _MainTex_ST;
				float4 _BaseColor;
				float4 _LandColor;
				float _FallSpeed;
				float _Seed;

				#if USE_SHAPE_LIGHT_TYPE_0
				SHAPE_LIGHT(0)
				#endif

				#if USE_SHAPE_LIGHT_TYPE_1
				SHAPE_LIGHT(1)
				#endif

				#if USE_SHAPE_LIGHT_TYPE_2
				SHAPE_LIGHT(2)
				#endif

				#if USE_SHAPE_LIGHT_TYPE_3
				SHAPE_LIGHT(3)
				#endif

				Varyings CombinedShapeLightVertex(Attributes v)
				{
					Varyings o = (Varyings)0;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

					o.positionCS = TransformObjectToHClip(v.positionOS);

					#if defined(DEBUG_DISPLAY)
					o.positionWS = TransformObjectToWorld(v.positionOS);
					#endif

					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.lightingUV = half2(ComputeScreenPos(o.positionCS / o.positionCS.w).xy);

					o.color = v.color;
					return o;
				}

				#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/CombinedShapeLightShared.hlsl"

				/*float rand(float x) {
					return frac(sin(x) * 100000);
				}*/
				float random(float x) {
					return frac(sin(x) * 1000);
				}

				float2 randomVec(float2 uv)
				{
					float angle = random(random(uv.x) + random(uv.y)) * 2 * PI;
					return float2(cos(angle), sin(angle));
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
					float p0 = dot(g0, pf);
					float p1 = dot(g1, float2(pf.x - 1, pf.y));
					float p2 = dot(g2, float2(pf.x, pf.y - 1));
					float p3 = dot(g3, pf - 1);
					return lerp(lerp(p0, p1, w.x), lerp(p2, p3, w.x), w.y);
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
							float dist = distance(f, neighborP + neighbor);
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

				half4 CombinedShapeLightFragment(Varyings i) : SV_Target
				{
					float2 uv = i.uv;
					uv.x *= 2;
					uv.y += _Seed+_Time.y * _FallSpeed;
					float uvnoise = perlinNoise(uv);
					uvnoise = (sin(uvnoise * 1.57) + 1) / 2;
					half4 main = _BaseColor;
					uv.y /= 2;
					main.a *= uvnoise * worleyNoise(uv);
					main.a = 3 * main.a * (1 - main.a) + main.a * main.a;
					if (i.uv.y < 0.1) {
						main = lerp(_LandColor, main, 10 * i.uv.y);
					}
					if (i.uv.y > 0.9) {
						main.a = lerp(main.a, 0.5, 10*(i.uv.y-0.9));
					}
					/*if (i.uv.x < 0.1 || i.uv.x>0.9) {
						float val = 10 * (abs(i.uv.x - 0.5) - 0.4);
						main.a = lerp(main.a, 0, val);
					}*/

					const half4 mask = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv);
					SurfaceData2D surfaceData;
					InputData2D inputData;

					InitializeSurfaceData(main.rgb, main.a, mask, surfaceData);
					InitializeInputData(i.uv, i.lightingUV, inputData);

					return CombinedShapeLightShared(surfaceData, inputData);
				}
				ENDHLSL
			}


			//Pass
			//{
			//    Tags { "LightMode" = "UniversalForward" "Queue" = "Transparent" "RenderType" = "Transparent"}

			//    HLSLPROGRAM
			//    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			//    #pragma vertex UnlitVertex
			//    #pragma fragment UnlitFragment

			//    struct Attributes
			//    {
			//        float3 positionOS   : POSITION;
			//        float4 color        : COLOR;
			//        float2 uv           : TEXCOORD0;
			//        UNITY_VERTEX_INPUT_INSTANCE_ID
			//    };

			//    struct Varyings
			//    {
			//        float4  positionCS      : SV_POSITION;
			//        float4  color           : COLOR;
			//        float2  uv              : TEXCOORD0;
			//        #if defined(DEBUG_DISPLAY)
			//        float3  positionWS  : TEXCOORD2;
			//        #endif
			//        UNITY_VERTEX_OUTPUT_STEREO
			//    };

			//    TEXTURE2D(_MainTex);
			//    SAMPLER(sampler_MainTex);
			//    float4 _MainTex_ST;

			//    Varyings UnlitVertex(Attributes attributes)
			//    {
			//        Varyings o = (Varyings)0;
			//        UNITY_SETUP_INSTANCE_ID(attributes);
			//        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

			//        o.positionCS = TransformObjectToHClip(attributes.positionOS);
			//        #if defined(DEBUG_DISPLAY)
			//        o.positionWS = TransformObjectToWorld(v.positionOS);
			//        #endif
			//        o.uv = TRANSFORM_TEX(attributes.uv, _MainTex);
			//        o.color = attributes.color;
			//        o.color = float4(1, 1, 1, 1);
			//        return o;
			//    }

			//    float4 UnlitFragment(Varyings i) : SV_Target
			//    {
			//        //float4 mainTex = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
			//        float4 mainTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

			//        /*#if defined(DEBUG_DISPLAY)
			//        SurfaceData2D surfaceData;
			//        InputData2D inputData;
			//        half4 debugColor = 0;

			//        InitializeSurfaceData(mainTex.rgb, mainTex.a, surfaceData);
			//        InitializeInputData(i.uv, inputData);
			//        SETUP_DEBUG_DATA_2D(inputData, i.positionWS);

			//        if (CanDebugOverrideOutputColor(surfaceData, inputData, debugColor))
			//        {
			//            return debugColor;
			//        }
			//        #endif*/

			//        return mainTex;
			//    }
			//    ENDHLSL
			//}
		}
}
