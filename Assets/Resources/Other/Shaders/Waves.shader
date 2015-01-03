
       Shader "Custom/Waves" {
            Properties {
                _Color ("Main Color", Color) = (1,1,1,1)
                _ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
                _MainTex ("Base (RGB) RefStrength (A)", 2D) = "white" {}
                _Cube ("Reflection Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }
                _BumpMap ("Normalmap", 2D) = "bump" {}
            }
             
            SubShader {
                Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
                Cull Off Lighting Off
                LOD 300
               
             
            CGPROGRAM
            #pragma surface surf Lambert alpha vertex:vert
             
            sampler2D _MainTex;
            sampler2D _BumpMap;
            samplerCUBE _Cube;
             
            fixed4 _Color;
            fixed4 _ReflectColor;
             
            struct Input {
                float2 uv_MainTex;
                float2 uv_BumpMap;
                float3 worldRefl;
                INTERNAL_DATA
            };
             
            void vert (inout appdata_full v) {
                float phase = _Time * 30.0;
                float4 wpos = mul( _Object2World, v.vertex);
                float offset = (wpos.x + (wpos.z * 0.2)) * 0.5;
                wpos.y = sin(phase + offset) * 0.1;
                          v.vertex = mul(_World2Object, wpos);
            }
             
            void surf (Input IN, inout SurfaceOutput o) {
                fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
                fixed4 c = tex * _Color;
                o.Albedo = c.rgb;
               
                o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
               
                float3 worldRefl = WorldReflectionVector (IN, o.Normal);
                fixed4 reflcol = texCUBE (_Cube, worldRefl);
                o.Emission = reflcol.rgb;
                o.Alpha = 0.9;
            }
            ENDCG
            }
             
            FallBack "Reflective/VertexLit"
            }

