Shader "Unlit/Skydome"
{
Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorA("ColA", Color) = (1,1,1,1)
        _ColorB("ColB", Color) = (1,1,1,1)
        _ColorC("ColC", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work

            #include "UnityCG.cginc"

            struct Meshdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolator
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _ColorA;
            float4 _ColorB;
            float4 _ColorC;

            Interpolator vert (Meshdata v)
            {
                Interpolator o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag (Interpolator i) : SV_Target
            {
                float t = 1-i.uv.y;
                if (t < 0.5) {
                    return lerp(_ColorA, _ColorB,t*2);
                }
                 return lerp(_ColorB, _ColorC,(t-0.5)*2);
            }
            ENDCG
        }
    }
}
