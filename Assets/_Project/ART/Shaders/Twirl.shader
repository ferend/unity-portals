Shader "Unlit/Twirl"
{
	Properties {
    _MainTex ("Texture", 2D) = "white" {}
    _Twirl ("Twirl", Float) = 1
  }
  SubShader {
	Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" }
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGB
		ZTest Greater
		Cull Off Lighting Off ZWrite Off
		
    Pass {
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag

      // Properties
      uniform sampler2D _MainTex;
      uniform float     _Twirl;

      // Vertex Input
      struct appdata {
        float4 vertex : POSITION;
        float2 uv     : TEXCOORD0;
      };

      // Vertex to Fragment
      struct v2f {
        float4 pos : SV_POSITION;
        float2 uv  : TEXCOORD0;
      };

      //------------------------------------------------------------------------
      // Vertex Shader
      //------------------------------------------------------------------------
      v2f vert(appdata v) {
        v2f o;
        o.pos = UnityObjectToClipPos(v.vertex);
        o.uv  = v.uv;
        return o;
      }

      //------------------------------------------------------------------------
      // Fragment Shader
      //------------------------------------------------------------------------
      fixed4 frag(v2f i) : SV_Target {
        // Twirl
        float2 uv    = i.uv - 0.5;
        float  theta = (0.5 - length(uv)) * _Twirl * _Time.y;
        float  s     = sin(theta);
        float  c     = cos(theta);
        uv = mul(float2x2(c, -s, s, c), uv) + 0.5;

        fixed4 color = tex2D(_MainTex, uv);

        return color;
      }
      ENDCG
    }
  }
}
