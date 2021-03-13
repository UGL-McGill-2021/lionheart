Shader "Unlit/celltest"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Brightness("luminosity", Range(0,1)) = 0.05
        _ColorIntensity("intensity", Range(0,1)) = 0.7
        _ColourCode("Colour", COLOR) = (0,0,0,0)
        _reflection("Light_Ref", Range(0,1)) =  0.25
        
        
    }
    SubShader
    {
        //render post background
        Tags { "RenderType"="Opacity" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                half3 worldNormal: NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Brightness;
            float _ColorIntensity;
            float4 _ColourCode;
            float _reflection;

            float reflectLightDir(float3 norm, float3 direction)
            {
                float3 normalizeN = normalize(norm);
                float3 normalizeDir = normalize(direction);
                //return dot product clamped
                float dotProductNL = max(0.0, dot(normalizeN, normalizeDir));
                //divide by color intensity
                return floor(dotProductNL/_reflection);
            }   

            v2f vert (appdata v)
            {
                v2f o; 
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //get global frame normals
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // texture Sample
                fixed4 col = tex2D(_MainTex, i.uv);
                //light accumulation to reflect depending on world direction
                col *= reflectLightDir(i.worldNormal, _WorldSpaceLightPos0.xyz) * clamp(_ColorIntensity * _ColourCode + _Brightness, 0.0, 10);
                return col;
            }
            ENDCG
        }
    }
}
