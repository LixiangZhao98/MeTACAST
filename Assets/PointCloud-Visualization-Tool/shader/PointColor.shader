Shader "Custom/PointColor"
{

    SubShader
    {
 Tags{  "Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}

        Blend SrcAlpha OneMinusSrcAlpha 
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows keepalpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        #pragma vertex vert


        struct Input
        {
            float4 col;
        };


          struct a2v
            {
                float4 vertex: POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float3 col :TEXCOORD1;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
            };
        

void vert (inout a2v v,out Input o)
            {
                UNITY_INITIALIZE_OUTPUT(Input,o);
                o.col=float4(v.col,1);
           }
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {fixed4 c =  IN.col;
          o.Emission = c;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
