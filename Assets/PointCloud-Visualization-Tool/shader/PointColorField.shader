Shader "Custom/PointColorField"
{
    Properties
    {
        [Space(50)]
        [Enum(CustomColor,0,FieldColor,1)] _ColorMappingMode ("ColorMappingMode", Int) = 0
        [Header(CustomColor)]
        [HDR] _Color ("Color", Color) = (1,1,1,1)
        [Header(FieldColor)]
        [HDR] _InterpColorSmall ("InterpColorSmall", Color)= (0, 0, 0, 1)
        [HDR] _InterpColorLarge ("InterpColorLarge", Color)= (0, 0, 0, 1)
    }
    SubShader
    {
        Tags
        {
            "Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows keepalpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        #pragma vertex vert

        fixed4 _Color;
        float4 _InterpColorSmall;
        float4 _InterpColorLarge;
        int _ColorMappingMode;

        struct Input
        {
            float4 col;
        };


        struct a2v
        {
            float4 vertex: POSITION;
            float2 uv : TEXCOORD0;
            float3 normal : NORMAL;
            float3 lp :TEXCOORD1;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };


        float3 HSVToRGB(float3 c)
        {
            float3 rgb = clamp(abs(fmod(c.x * 6.0 + float3(0.0, 4.0, 2.0), 6) - 3.0) - 1.0, 0, 1);
            rgb = rgb * rgb * (3.0 - 2.0 * rgb);
            return c.z * lerp(float3(1, 1, 1), rgb, c.y);
        }


        float3 RGBToHSV(float3 c)
        {
            float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
            float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
            float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

            float d = q.x - min(q.w, q.y);
            float e = 1.0e-10;
            return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
        }

        float4 InterpColorPoint_Field(float lp) // define the interpolation of the point based on the field value
        {
            float3 smallHSV = RGBToHSV(_InterpColorSmall.xyz);
            float3 largeHSV = RGBToHSV(_InterpColorLarge.xyz);
            return float4(
                HSVToRGB(float3(lerp(smallHSV.x, largeHSV.x, lp), lerp(smallHSV.y, largeHSV.y, lp),
                    lerp(smallHSV.z, largeHSV.z, lp))), lerp(_InterpColorSmall.a, _InterpColorLarge.a, lp));
        }

        void vert(inout a2v v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.col = InterpColorPoint_Field(v.lp.x);
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c;
            if (_ColorMappingMode == 1)
                c = IN.col;
            else
                c = _Color;
            o.Emission = c;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}