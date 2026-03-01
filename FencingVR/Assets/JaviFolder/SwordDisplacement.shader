Shader "Custom/SwordDisplacement"
{
    

    Properties
    {
        _BaseMap ("Texture", 2D) = "white" {}
        _BaseColor ("Color", Color) = (1,1,1,1)
        _Amplitude ("Amplitude", Range(0, 0.5)) = 0.05
        _Frequency ("Frequency", Range(0, 10)) = 5.0
        _Speed ("Speed", Range(0, 10)) = 2.0
        _BladeZMin ("Blade Z Min", Float) = 0.0
        _BladeZMax ("Blade Z Max", Float) = 0.4
        _HandVelocityX ("Hand Velocity X", Float) = 0.0
        _HandVelocityY ("Hand Velocity Y", Float) = 0.0
        _TipOffsetX ("Tip Offset X", Float) = 0.0
        _TipOffsetY ("Tip Offset Y", Float) = 0.0
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Geometry"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing                  
            #pragma instancing_options renderingLayer
            #pragma multi_compile_fog
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl" 

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)
                UNITY_DEFINE_INSTANCED_PROP(float4, _BaseMap_ST)
                UNITY_DEFINE_INSTANCED_PROP(float4, _BaseColor)
                UNITY_DEFINE_INSTANCED_PROP(float,  _Amplitude)
                UNITY_DEFINE_INSTANCED_PROP(float,  _Frequency)
                UNITY_DEFINE_INSTANCED_PROP(float,  _Speed)
                UNITY_DEFINE_INSTANCED_PROP(float,  _BladeZMin)
                UNITY_DEFINE_INSTANCED_PROP(float,  _BladeZMax)
                UNITY_DEFINE_INSTANCED_PROP(float,  _HandVelocityX)
                UNITY_DEFINE_INSTANCED_PROP(float,  _HandVelocityY)
                UNITY_DEFINE_INSTANCED_PROP(float, _TipOffsetX)
                UNITY_DEFINE_INSTANCED_PROP(float, _TipOffsetY)
            UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float3 normalWS    : TEXCOORD1;
                float3 positionWS  : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO 
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                float3 pos = IN.positionOS.xyz;

                // blade = 0 at hilt, 1 at tip
                float blade = saturate((pos.z - _BladeZMin) / (_BladeZMax - _BladeZMin));

                // Base displacement — affects whole blade, stronger at tip
                float baseDisp = blade * blade;

                // Tip displacement — extra lag, only affects top half
                // Creates a whip curve: base bends one way, tip follows after
                float tipBlend = saturate((blade - 0.4) / 0.6);
                float tipDisp  = tipBlend * tipBlend;

                // Combine: base driven by spring, tip driven by lagged tip value
                // The difference between base and tip creates the flex curve
                pos.x += _HandVelocityX * baseDisp + (_TipOffsetX - _HandVelocityX) * tipDisp;
                pos.y += _HandVelocityY * baseDisp + (_TipOffsetY - _HandVelocityY) * tipDisp;

                OUT.positionHCS = TransformObjectToHClip(pos);
                OUT.positionWS  = TransformObjectToWorld(pos);
                OUT.normalWS    = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv          = TRANSFORM_TEX(IN.uv, _BaseMap);

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);
                half4 texColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                half4 color = texColor * _BaseColor;

                // Basic lighting
                InputData lightingInput = (InputData)0;
                lightingInput.positionWS = IN.positionWS;
                lightingInput.normalWS   = normalize(IN.normalWS);
                lightingInput.viewDirectionWS = GetWorldSpaceNormalizeViewDir(IN.positionWS);
                lightingInput.shadowCoord = float4(0,0,0,0);

                SurfaceData surfaceData = (SurfaceData)0;
                surfaceData.albedo    = color.rgb;
                surfaceData.alpha     = color.a;
                surfaceData.metallic  = 0.8;
                surfaceData.smoothness = 0.9;
                surfaceData.normalTS  = float3(0,0,1);

                return UniversalFragmentPBR(lightingInput, surfaceData);
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}