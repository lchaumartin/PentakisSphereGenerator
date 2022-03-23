Shader "Custom/PentakisSphere"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Emission("Emission (RGB)", 2D) = "black" {}
        _Height("Height", 2D) = "black" {}
        _HeightMultiplier("Amount", Range(0,1)) = 1.0
        _Glossiness("Smoothness", 2D) = "grey" {}
        _GlossinessMultiplier("Amount", Range(0,1)) = 1.0
        _Metallic("Metallic", Range(0,1)) = 0.0
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }

            CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            #pragma surface surf Standard vertex:vert


            sampler2D _MainTex;
            sampler2D _Emission;
            sampler2D _Height;
            half _HeightMultiplier;
            sampler2D _Glossiness;
            half _GlossinessMultiplier;
            half _Metallic;
            fixed4 _Color;

            struct Input
            {
                fixed2 UV;
                float3 localPos : SV_POSITION;
            };

            float3 HeightToNormal(float height, float3 normal, float3 pos)
            {
                float3 worldDirivativeX = ddx(pos);
                float3 worldDirivativeY = ddy(pos);
                float3 crossX = cross(normal, worldDirivativeX);
                float3 crossY = cross(normal, worldDirivativeY);
                float3 d = abs(dot(crossY, worldDirivativeX));
                float3 inToNormal = ((((height + ddx(height)) - height) * crossY) + (((height + ddy(height)) - height) * crossX)) * sign(d);
                inToNormal.y *= -1.0;
                return normalize((d * normal) - inToNormal);
            }

            void vert(inout appdata_full v, out Input o) {
                UNITY_INITIALIZE_OUTPUT(Input, o);
                o.localPos = v.vertex.xyz;
                o.UV = clamp(float2(atan2(v.vertex.z, v.vertex.x) / (2.0 * 3.14159) + 0.5, asin(v.vertex.y / length(v.vertex.xyz)) / 3.14159 + 0.5), 0.0, 1.0);
                v.vertex.xyz = v.vertex.xyz * (1.0 + tex2Dlod(_Height, float4(o.UV, 0, 0)) * _HeightMultiplier);
            }

            void surf(Input IN, inout SurfaceOutputStandard o)
            {
                float3 pos =  IN.localPos.xyz;
                float2 uv =  float2(atan2(pos.z, pos.x) / (2.0 * 3.14159) + 0.5, asin(pos.y / length(pos)) / 3.14159 + 0.5);
                fixed4 c = tex2D(_MainTex, uv) * _Color;
                o.Emission = tex2D(_Emission, uv).rgb;
                o.Albedo = c.rgb;
                o.Metallic = _Metallic;
                o.Smoothness = tex2D(_Glossiness, uv)* _GlossinessMultiplier;
                o.Alpha = c.a;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
