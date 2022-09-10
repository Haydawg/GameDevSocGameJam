Shader "hjm141/RadiationEffectShader"
{
    // Unity Inspector properties
    Properties
    {

        _Texture("Texture", 2D) = "white"{}
        _InflationFactor("Inflation Factor", Range(-.1, .5)) = 0.1
        _PulsationfieldColor("_PulsationfieldColor", Color) = (1,1,1,1)
        [HDR] _EmissionColor("Color", Color) = (0,0,0)
        _Glossiness("Smoothness", Range(0,1)) = 0.5
    }

    SubShader
    {
        CGPROGRAM
        #pragma surface surf Lambert

        // The input structure to our surface function
        struct Input
        {
            float2 uv_Texture;
        };

        fixed4 _EmissionColor;
        sampler2D _Texture;
        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Albedo = tex2D(_Texture, IN.uv_Texture);
            o.Emission = c.rgb * tex2D(_Texture, IN.uv_Texture).a * _EmissionColor;
            o.Smoothness = _Glossiness;
        }
        ENDCG

        Pass
        {
            ZWrite On
            ColorMask 0

            CGPROGRAM
            // Define a vertex shader in the vert function
            #pragma vertex vert
            // Define a fragment shader in the frag function
            #pragma fragment frag

            // Include UnityCG.cginc for helpful functions
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
            };

            float _InflationFactor;

            float4 vert (appdata v) : SV_POSITION
            {
                return UnityObjectToClipPos(v.vertex + v.normal * _InflationFactor);
            }

            fixed4 frag (float4 i : SV_POSITION) : SV_Target
            {
                return (0,0,0,0);
            }
            ENDCG
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Blend DstColor SrcColor
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            // Include UnityCG.cginc for helpful functions
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
            };

            // The uniform variable for our inflation factor
            float _InflationFactor;
            fixed4 _PulsationfieldColor;
            float4 vert (appdata v) : SV_POSITION
            {
                // Shift the vertex position in the direction of the vertex normal
                // by the length of the inflation factor, transform this to 
                // homogeneous clip space, and then return this
                return UnityObjectToClipPos(v.vertex + v.normal * _InflationFactor);
            }

            // The fragment function, takes a variable of type float4 named
            // i and semantically tagged SV_POSITION (the homogeneous clip space
            // vertex position) and returns a colour semantically tagged with SV_Target
            fixed4 frag(float4 i : SV_POSITION) : SV_Target
            {
                // TODO: Return the force field colour
                return _PulsationfieldColor;
            }
            ENDCG
        }
    }

    // Fallback to the Diffuse lighting model
    FallBack "Diffuse"
}
