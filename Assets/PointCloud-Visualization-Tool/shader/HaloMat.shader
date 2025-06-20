Shader "Instanced/Halo" {
    Properties {
        [Space(50)]
        [Enum(CustomColor,0,FieldColor,1)]  _ColorMappingMode ("ColorMappingMode", Int) = 0
        [Header(CustomColor)] 
        _CustomColor ("CustomColor", Color) = (0, 0, 0, 1)
        [PowerSlider(6)] _CustomOutlineTransparentExponent ("CustomOutlineTransparentExponent", Range(0.0625, 16)) = 1
        [Header(FieldColor)] 
        _InterpColorSmall ("InterpColorSmall", Color)= (0, 0, 0, 1)
        _InterpColorLarge ("InterpColorLarge", Color)= (0, 0, 0, 1)
        [PowerSlider(6)] _FieldOutlineTransparentExponent ("FieldOutlineTransparentExponent", Range(0.0625, 16)) = 1
        
        [Space(50)]
        [Enum(CustomRadius,0,FieldRadius,1)]  _RadiusMappingMode ("RadiusMappingMode", Int) = 0
        [Header(CustomRadius)] 
        _CustomRadius ("CustomRadius",  Range(0.0, 40))=1

        [Header(FieldRadius)] 
        _FieldMaxRadius ("FieldMaxRadius", Float)=2
        _FieldMinRadius ("FieldMinRadius", Float)=0
    }
    SubShader {
       // Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
                //ZWrite off
               // Blend SrcAlpha OneMinusSrcAlpha

     Tags{"RenderType"="Opaque" "Queue"="Geometry"}
        LOD 100

       
        Pass {
 Cull off
            CGPROGRAM

            
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            StructuredBuffer<float4> positionBuffer;
            float4 _CustomColor;
            float4 _InterpColorSmall;
            float4 _InterpColorLarge;
            float _minValue;
            float _maxValue;
            int _ColorMappingMode;
            float _CustomOutlineTransparentExponent;
            float _FieldOutlineTransparentExponent;
            int _RadiusMappingMode;
            float _CustomRadius;
            float _FieldMaxRadius;
            float _FieldMinRadius;
            float3 _CamPos; 

            float4x4 CreateRotationMatrix(float3 src, float3 dst)
            {
    float3 v = cross(src, dst);
    float c = dot(src, dst);
                

    float k = 1.0 / (1.0 + c);

    float4x4 r = float4x4(
        v.x * v.x * k + c,     v.y * v.x * k - v.z,   v.z * v.x * k + v.y,   0,
        v.x * v.y * k + v.z,   v.y * v.y * k + c,     v.z * v.y * k - v.x,   0,
        v.x * v.z * k - v.y,   v.y * v.z * k + v.x,   v.z * v.z * k + c,     0,
        0,                    0,                    0,                      1
    );

    return r;
}
            
            float LpMapping(float lp)
            {
                return lp;
            }
            float3 HSVToRGB( float3 c)
            {
                float3 rgb = clamp( abs(fmod(c.x*6.0+float3(0.0,4.0,2.0),6)-3.0)-1.0, 0, 1);
            rgb = rgb*rgb*(3.0-2.0*rgb);
            return c.z * lerp( float3(1,1,1), rgb, c.y);
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

            float4 InterpColor_Field(float lp)   // define the interpolation of the tube based on the field value
            {
            float3 smallHSV=RGBToHSV(_InterpColorSmall.xyz);
            float3 largeHSV=RGBToHSV(_InterpColorLarge.xyz);
            return float4(HSVToRGB(float3(lerp(smallHSV.x,largeHSV.x,lp),lerp(smallHSV.y,largeHSV.y,lp),lerp(smallHSV.z,largeHSV.z,lp))),lerp(_InterpColorSmall.a,_InterpColorLarge.a,lp)) ;
            }

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal: NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 localPosition:TEXCOORD2;
                float lp:TEXCOORD4;
                float3 worldNormal:TEXCOORD5;
                float3 worldPos:TEXCOORD6;
                float sidelength:TEXCOORD3;
            };

            float4x4 _LocalToWorld;

            v2f vert (appdata v, uint instanceID : SV_InstanceID)
            {
                float4 data = positionBuffer[instanceID];
                float3 localPosition;
                float sidelength;
                if(_RadiusMappingMode==0)
                {
                    localPosition =  v.vertex.xyz  *_CustomRadius;
                    sidelength=2*_CustomRadius;
            }
                else
                {
                float radius=_FieldMinRadius+data.w*(_FieldMaxRadius-_FieldMinRadius);
                localPosition = v.vertex.xyz *radius;
                     sidelength=2*radius;
                }
                float4x4 rotationMatrix = CreateRotationMatrix(float3(0, 0, 1), normalize(_CamPos-data.xyz));
                float4 rotatedPosition = mul(rotationMatrix, float4(localPosition, 1.0));

                v2f o;
                o.localPosition=localPosition;

                o.worldNormal = v.normal;
                o.sidelength=sidelength;
                o.worldPos=mul(_LocalToWorld,float4(data.xyz +rotatedPosition.xyz,1));
                o.lp=LpMapping(data.w);
                o.vertex = mul(UNITY_MATRIX_VP, float4(o.worldPos, 1.0f));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // if(length(i.uv.xy)>0.25)
                //     discard;
                
                float viewDotNorm = dot(normalize(i.worldNormal), normalize(i.worldPos - _WorldSpaceCameraPos));
                viewDotNorm = saturate( viewDotNorm);


                fixed4 col;
                if(_ColorMappingMode==0)
                {col= _CustomColor; 
                    viewDotNorm = pow (viewDotNorm, _CustomOutlineTransparentExponent);
                col.a*=viewDotNorm;
                }
                else 
                {col=InterpColor_Field(i.lp);
                    viewDotNorm = pow (viewDotNorm, _FieldOutlineTransparentExponent);
                col.a*=viewDotNorm;
                }
                return col;
            }

            ENDCG
        }
    }
}