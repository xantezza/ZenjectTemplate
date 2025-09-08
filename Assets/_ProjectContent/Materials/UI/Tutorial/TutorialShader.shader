Shader "UI/RectHoleMask"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,0.8)
        _HoleCenter ("Hole Center", Vector) = (0.5, 0.5, 0, 0)
        _HoleSize ("Hole Size", Vector) = (0.3, 0.2, 0, 0)
        _Radius ("Radius", Float) = 0.01
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            float4 _HoleCenter;
            float4 _HoleSize;
            float _Radius;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                float2 holeMin = _HoleCenter.xy - _HoleSize.xy * 0.5;
                float2 holeMax = _HoleCenter.xy + _HoleSize.xy * 0.5;

                float2 dist = max(holeMin - uv, uv - holeMax);
                dist = max(dist, 0.0);

                float distLength = length(dist);

                float alpha = step(_Radius, distLength);//smoothstep(_Radius, _Radius * 0.5, distLength);

                return fixed4(_Color.rgb, _Color.a * alpha);
            }
            ENDCG
        }
    }
}