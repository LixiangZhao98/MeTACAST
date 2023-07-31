
Shader "Custom/MCmesh"{
	//show values to edit in inspector
	Properties{
 _MainTex("Albedo", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)

        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

        _BumpScale("Scale", Float) = 1.0
        _BumpMap("Normal Map", 2D) = "bump" {}

        _EmissionColor("Color", Color) = (0,0,0)
        _EmissionMap("Emission", 2D) = "white" {}

        _DistortionStrength("Strength", Float) = 1.0
        _DistortionBlend("Blend", Range(0.0, 1.0)) = 0.5

        _SoftParticlesNearFadeDistance("Soft Particles Near Fade", Float) = 0.0
        _SoftParticlesFarFadeDistance("Soft Particles Far Fade", Float) = 1.0
        _CameraNearFadeDistance("Camera Near Fade", Float) = 1.0
        _CameraFarFadeDistance("Camera Far Fade", Float) = 2.0

        // Hidden properties
        [HideInInspector] _Mode ("__mode", Float) = 0.0
        [HideInInspector] _ColorMode ("__colormode", Float) = 0.0
        [HideInInspector] _FlipbookMode ("__flipbookmode", Float) = 0.0
        [HideInInspector] _LightingEnabled ("__lightingenabled", Float) = 0.0
        [HideInInspector] _DistortionEnabled ("__distortionenabled", Float) = 0.0
        [HideInInspector] _EmissionEnabled ("__emissionenabled", Float) = 0.0
        [HideInInspector] _BlendOp ("__blendop", Float) = 0.0
        [HideInInspector] _SrcBlend ("__src", Float) = 1.0
        [HideInInspector] _DstBlend ("__dst", Float) = 0.0
        [HideInInspector] _ZWrite ("__zw", Float) = 1.0
        [HideInInspector] _Cull ("__cull", Float) = 2.0
        [HideInInspector] _SoftParticlesEnabled ("__softparticlesenabled", Float) = 0.0
        [HideInInspector] _CameraFadingEnabled ("__camerafadingenabled", Float) = 0.0
        [HideInInspector] _SoftParticleFadeParams ("__softparticlefadeparams", Vector) = (0,0,0,0)
        [HideInInspector] _CameraFadeParams ("__camerafadeparams", Vector) = (0,0,0,0)
        [HideInInspector] _ColorAddSubDiff ("__coloraddsubdiff", Vector) = (0,0,0,0)
        [HideInInspector] _DistortionStrengthScaled ("__distortionstrengthscaled", Float) = 0.0
		
	}

	SubShader{
		
         Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        Pass
        {
            Tags {"LightMode" = "ForwardBase"}
            ZWrite off
            Blend SrcAlpha OneMinusSrcAlpha
   

			CGPROGRAM

			//include useful shader functions
			#include "UnityCG.cginc"
			#include "Triangle.cginc"
			//define vertex and fragment shader functions
			#pragma vertex vert
			#pragma fragment frag

			//tint of the texture
			fixed4 _Color;
			 fixed _Cutoff;
	    float4x4 _LocalToWorld;
        float4x4 _WorldToLocal;
			//buffers
		  StructuredBuffer<Triangle> triangleRW;
                float4 vert(uint vertex_id: SV_VertexID, uint instance_id: SV_InstanceID) : SV_POSITION{

				float3 position = triangleRW[vertex_id/3].v[vertex_id%3].vPosition;

		            unity_ObjectToWorld = _LocalToWorld;
            unity_WorldToObject = _WorldToLocal;
					   return mul(UNITY_MATRIX_VP,  mul(unity_ObjectToWorld, float4(position, 1)));
			}
          
			//the fragment shader function
			fixed4 frag() : SV_TARGET{
				//return the final color to be drawn on screen
		
				return fixed4(_Color.x,_Color.y,_Color.z,_Color.a*_Cutoff) ;
			}
			
			ENDCG
		}
	}
	Fallback "VertexLit"
}





//Shader "Custom/Standard Unlit"
//{
//    Properties
//    {
//        _MainTex("Albedo", 2D) = "white" {}
//        _Color("Color", Color) = (1,1,1,1)

//        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

//        _BumpScale("Scale", Float) = 1.0
//        _BumpMap("Normal Map", 2D) = "bump" {}

//        _EmissionColor("Color", Color) = (0,0,0)
//        _EmissionMap("Emission", 2D) = "white" {}

//        _DistortionStrength("Strength", Float) = 1.0
//        _DistortionBlend("Blend", Range(0.0, 1.0)) = 0.5

//        _SoftParticlesNearFadeDistance("Soft Particles Near Fade", Float) = 0.0
//        _SoftParticlesFarFadeDistance("Soft Particles Far Fade", Float) = 1.0
//        _CameraNearFadeDistance("Camera Near Fade", Float) = 1.0
//        _CameraFarFadeDistance("Camera Far Fade", Float) = 2.0

//        // Hidden properties
//        [HideInInspector] _Mode ("__mode", Float) = 0.0
//        [HideInInspector] _ColorMode ("__colormode", Float) = 0.0
//        [HideInInspector] _FlipbookMode ("__flipbookmode", Float) = 0.0
//        [HideInInspector] _LightingEnabled ("__lightingenabled", Float) = 0.0
//        [HideInInspector] _DistortionEnabled ("__distortionenabled", Float) = 0.0
//        [HideInInspector] _EmissionEnabled ("__emissionenabled", Float) = 0.0
//        [HideInInspector] _BlendOp ("__blendop", Float) = 0.0
//        [HideInInspector] _SrcBlend ("__src", Float) = 1.0
//        [HideInInspector] _DstBlend ("__dst", Float) = 0.0
//        [HideInInspector] _ZWrite ("__zw", Float) = 1.0
//        [HideInInspector] _Cull ("__cull", Float) = 2.0
//        [HideInInspector] _SoftParticlesEnabled ("__softparticlesenabled", Float) = 0.0
//        [HideInInspector] _CameraFadingEnabled ("__camerafadingenabled", Float) = 0.0
//        [HideInInspector] _SoftParticleFadeParams ("__softparticlefadeparams", Vector) = (0,0,0,0)
//        [HideInInspector] _CameraFadeParams ("__camerafadeparams", Vector) = (0,0,0,0)
//        [HideInInspector] _ColorAddSubDiff ("__coloraddsubdiff", Vector) = (0,0,0,0)
//        [HideInInspector] _DistortionStrengthScaled ("__distortionstrengthscaled", Float) = 0.0
//    }

//    Category
//    {
//        SubShader
//        {
//            Tags { "RenderType"="Opaque" "IgnoreProjector"="True" "PreviewType"="Plane" "PerformanceChecks"="False" }

//            BlendOp [_BlendOp]
//            Blend [_SrcBlend] [_DstBlend]
//           ZTest Greater  // delete  掉就可以复原
//ZWrite Off
//            Cull [_Cull]
//            ColorMask RGB

//            GrabPass
//            {
//                Tags { "LightMode" = "Always" }
//                "_GrabTexture"
//            }

//            Pass
//            {
//                Name "ShadowCaster"
//                Tags { "LightMode" = "ShadowCaster" }

//                BlendOp Add
//                Blend One Zero
//                ZWrite On
//                Cull Off

//                CGPROGRAM
//                #pragma target 2.5
//                #include "Triangle.cginc"
//                #include "UnityCG.cginc"
//                #pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON
//                #pragma shader_feature_local _ _COLOROVERLAY_ON _COLORCOLOR_ON _COLORADDSUBDIFF_ON
//                #pragma shader_feature_local _REQUIRE_UV2
//                #pragma multi_compile_shadowcaster
//                #pragma multi_compile_instancing
//                #pragma instancing_options procedural:vertInstancingSetup
//                #pragma vertex vert
//                #pragma fragment fragParticleShadowCaster
//                	    float4x4 _LocalToWorld;
//                        float4x4 _WorldToLocal;
//		  StructuredBuffer<Triangle> triangleRW;

//            float4 vert(uint vertex_id: SV_VertexID, uint instance_id: SV_InstanceID) : SV_POSITION{
//		    float3 position = triangleRW[vertex_id/3].v[vertex_id%3].vPosition;
//		    unity_ObjectToWorld = _LocalToWorld;
//            unity_WorldToObject = _WorldToLocal;
//					   return mul(UNITY_MATRIX_VP,  mul(unity_ObjectToWorld, float4(position, 1)));
//			}
  

//                #include "UnityStandardParticleShadow.cginc"
//                ENDCG
//            }

//            Pass
//            {
//                Name "SceneSelectionPass"
//                Tags { "LightMode" = "SceneSelectionPass" }

//                BlendOp Add
//                Blend One Zero
//                ZWrite On
//                Cull Off

//                CGPROGRAM
//                #pragma target 2.5
//                                #include "Triangle.cginc"
//                #include "UnityCG.cginc"
//                #pragma shader_feature_local _ALPHATEST_ON
//                #pragma shader_feature_local _REQUIRE_UV2
//                #pragma multi_compile_instancing
//                #pragma instancing_options procedural:vertInstancingSetup
//                #pragma vertex vert
//                #pragma fragment fragSceneHighlightPass
//                	    float4x4 _LocalToWorld;
//                        float4x4 _WorldToLocal;
//		  StructuredBuffer<Triangle> triangleRW;

//            float4 vert(uint vertex_id: SV_VertexID, uint instance_id: SV_InstanceID) : SV_POSITION{
//		    float3 position = triangleRW[vertex_id/3].v[vertex_id%3].vPosition;
//		    unity_ObjectToWorld = _LocalToWorld;
//            unity_WorldToObject = _WorldToLocal;
//					   return mul(UNITY_MATRIX_VP,  mul(unity_ObjectToWorld, float4(position, 1)));
//			}

//                #include "UnityStandardParticleEditor.cginc"
//                ENDCG
//            }

//            Pass
//            {
//                Name "ScenePickingPass"
//                Tags{ "LightMode" = "Picking" }

//                BlendOp Add
//                Blend One Zero
//                ZWrite On
//                Cull Off

//                CGPROGRAM
//                #pragma target 2.5
//                                #include "Triangle.cginc"
//                #include "UnityCG.cginc"
//                #pragma shader_feature_local _ALPHATEST_ON
//                #pragma shader_feature_local _REQUIRE_UV2
//                #pragma multi_compile_instancing
//                #pragma instancing_options procedural:vertInstancingSetup

//                              #pragma vertex vert
//                #pragma fragment fragScenePickingPass
//                	    float4x4 _LocalToWorld;
//                        float4x4 _WorldToLocal;
//		  StructuredBuffer<Triangle> triangleRW;

//            float4 vert(uint vertex_id: SV_VertexID, uint instance_id: SV_InstanceID) : SV_POSITION{
//		    float3 position = triangleRW[vertex_id/3].v[vertex_id%3].vPosition;
//		    unity_ObjectToWorld = _LocalToWorld;
//            unity_WorldToObject = _WorldToLocal;
//					   return mul(UNITY_MATRIX_VP,  mul(unity_ObjectToWorld, float4(position, 1)));
//			}

//                #include "UnityStandardParticleEditor.cginc"
//                ENDCG
//            }

//            Pass
//            {
//                Tags { "LightMode"="ForwardBase" }

//                CGPROGRAM
//                #pragma multi_compile __ SOFTPARTICLES_ON
//                #pragma multi_compile_fog
//                #pragma target 2.5
//                #include "Triangle.cginc"
//                #include "UnityCG.cginc"
//                #pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON
//                #pragma shader_feature_local _ _COLOROVERLAY_ON _COLORCOLOR_ON _COLORADDSUBDIFF_ON
//                #pragma shader_feature_local _NORMALMAP
//                #pragma shader_feature _EMISSION
//                #pragma shader_feature_local _FADING_ON
//                #pragma shader_feature_local _REQUIRE_UV2
//                #pragma shader_feature_local EFFECT_BUMP

//                #pragma vertex vertParticleUnlit
//                #pragma fragment fragParticleUnlit
//                	    float4x4 _LocalToWorld;
//                        float4x4 _WorldToLocal;
//		  StructuredBuffer<Triangle> triangleRW;

         
//                #pragma multi_compile_instancing
//                #pragma instancing_options procedural:vertInstancingSetup

//                #include "UnityStandardParticles.cginc"
//                ENDCG
//            }
//        }
//    }

//    Fallback "VertexLit"
//    CustomEditor "StandardParticlesShaderGUI"
//}