Shader "Custom/BasicWaterShader_URP"
{
    Properties
    {
        _Color ("Background Color", Color) = (0.1, 0.4, 0.8, 0.8)
        _TextureColor ("Texture Color", Color) = (1, 1, 1, 1)
        _MainTex ("Water Texture", 2D) = "white" {}
        _WaveSpeed ("Wave Speed", Float) = 0.5
        _WaveStrength ("Wave Strength", Range(0, 0.1)) = 0.01
        _WaveAmount ("Wave Amount", Float) = 0.1
        _WaveFrequency ("Wave Frequency", Float) = 1
        _TextureDistortion ("Texture Distortion", Range(0, 1)) = 0.5
        _CartoonFactor ("Cartoon Factor", Range(0, 1)) = 0.5
        _TransparencySpeed ("Transparency Animation Speed", Float) = 1.0
        _TransparencyStrength ("Transparency Strength", Range(0, 1)) = 0.5
        _FoamColor ("Foam Color", Color) = (1, 1, 1, 1)
        _FoamAmount ("Foam Amount", Range(0, 1)) = 0.1
        _FoamCutoff ("Foam Cutoff", Range(0, 1)) = 0.5
        _FoamSpeed ("Foam Speed", Float) = 0.1
        _FoamNoiseScale ("Foam Noise Scale", Float) = 20
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Pass
        {
            Name "Unlit"
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float4 screenPos : TEXCOORD2;
            };

            CBUFFER_START(UnityPerMaterial)
            float4 _Color;
            float4 _TextureColor;
            float4 _FoamColor;
            float _WaveSpeed, _WaveStrength, _WaveAmount, _WaveFrequency;
            float _TextureDistortion, _CartoonFactor;
            float _TransparencySpeed, _TransparencyStrength;
            float _FoamAmount, _FoamCutoff, _FoamSpeed, _FoamNoiseScale;
            CBUFFER_END

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);

            float2 random2(float2 st)
            {
                st = float2(dot(st, float2(127.1, 311.7)),
                            dot(st, float2(269.5, 183.3)));
                return -1.0 + 2.0 * frac(sin(st) * 43758.5453123);
            }

            float gradientNoise(float2 st)
            {
                float2 i = floor(st);
                float2 f = frac(st);
                float2 u = f*f*(3.0-2.0*f);

                return lerp( lerp( dot( random2(i + float2(0.0,0.0) ), f - float2(0.0,0.0) ),
                                   dot( random2(i + float2(1.0,0.0) ), f - float2(1.0,0.0) ), u.x),
                             lerp( dot( random2(i + float2(0.0,1.0) ), f - float2(0.0,1.0) ),
                                   dot( random2(i + float2(1.0,1.0) ), f - float2(1.0,1.0) ), u.x), u.y);
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                OUT.worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.screenPos = ComputeScreenPos(OUT.positionCS);
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;

                float time = _Time.y;

                float2 waveOffset = float2(
                    gradientNoise(uv * _WaveFrequency + time * _WaveSpeed),
                    gradientNoise(uv * _WaveFrequency * 1.2 + time * _WaveSpeed * 1.1)
                ) * _WaveAmount;

                float2 distortedUV = uv + waveOffset * _WaveStrength * _TextureDistortion;

                float4 c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, distortedUV);
                c = lerp(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv), c, _TextureDistortion);
                c *= _TextureColor;

                float transparencyPulse = (sin(time * _TransparencySpeed) + 1) * 0.5;
                float textureTransparency = lerp(1, transparencyPulse, _TransparencyStrength);

                float2 foamUV = IN.worldPos.xz * _FoamNoiseScale + time * _FoamSpeed;
                float foamNoise = gradientNoise(foamUV);

                // No depth-based foam in this minimal version (URP requires extra setup for camera depth)
                float foamLine = _FoamAmount;
                float foam = saturate(foamNoise + foamLine);
                foam = smoothstep(_FoamCutoff, 1, foam);

                float3 finalColor = lerp(_Color.rgb, c.rgb, c.a * textureTransparency);
                finalColor = lerp(finalColor, _FoamColor.rgb, foam);

                float alpha = lerp(_Color.a, c.a * _TextureColor.a, c.a * textureTransparency);

                return float4(finalColor, alpha);
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}