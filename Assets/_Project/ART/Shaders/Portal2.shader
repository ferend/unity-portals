Shader "Custom/Portal2"
{
    Properties
    {
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
            };

            struct Interpolator
            {
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD0;
            };



            sampler2D _MainTex;
            float4 _InactiveColour;
            int displayMask ; // set to 1 to display texture, otherwise will draw test colour
            float  _TwirlStrength;
            

            Interpolator vert (MeshData v)
            {
                Interpolator o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }
            
            float4 frag (Interpolator i) : SV_Target
            {
                   float2 uv = i.screenPos.xy / i.screenPos.w;
                float4 portalCol = tex2D(_MainTex, uv);
                return portalCol * displayMask + _InactiveColour * (1-displayMask);
            }
            ENDCG
        }
    }
        Fallback "Standard" // for shadows
}
