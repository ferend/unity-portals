Shader "Custom/Portal2"
{
    Properties
    {
            _MainTex ("Texture", 2D) = "white" {}
        _InactiveColour ("Inactive Colour", Color) = (1, 1, 1, 1)
        _TwirlStrenght ("Twirl Strenght",float)=1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
    
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
            };

            struct Interpolator
            {
                float4 uv : SV_POSITION;
                float4 screenPos : TEXCOORD0;
            };
            
            uniform sampler2D _MainTex;
            float4 _InactiveColour;
            int displayMask ; // set to 1 to display texture, otherwise will draw test colour
            uniform float  _TwirlStrength = 10;

            float2 twirl(Interpolator i)
            {
                float2 uv    = i.uv - 0.5;  
                float  theta = (0.5 - length(uv)) *_TwirlStrength ;
                float  s     = sin(theta);
                float  c     = cos(theta);
                float2 res =  mul(float2x2(c, -s, s, c), uv) + 0.5;
                return res;
            }

            Interpolator vert (MeshData v)
            {
                Interpolator o;
                o.uv = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.uv);
                return o;
            }
            
            float4 frag (Interpolator i) : SV_Target
            {
                float2 uv = i.screenPos.xy / i.screenPos.w;
                float4 portalCol = tex2D(_MainTex, uv);
                return portalCol * displayMask + _InactiveColour  * (1-displayMask);
            }
            ENDCG
        }
    }
        Fallback "Standard" // for shadows
}
