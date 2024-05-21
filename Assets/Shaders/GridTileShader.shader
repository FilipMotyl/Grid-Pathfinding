Shader "Custom/GridTileShader"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _BorderColor ("Border Color", Color) = (0,0,0,1)
        _BorderWidth ("Border Width", Range(0,0.1)) = 0.05
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
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _Color;
            float4 _BorderColor;
            float _BorderWidth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float border = step(_BorderWidth, min(i.uv.x, min(i.uv.y, min(1.0 - i.uv.x, 1.0 - i.uv.y))));
                return lerp(_BorderColor, _Color, border);
            }
            ENDCG
        }
    }
}