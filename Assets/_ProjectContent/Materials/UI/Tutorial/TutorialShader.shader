Shader "UI/RectHoleMask"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,0.7)
        _HoleCenter ("Hole Center (UV)", Vector) = (0.5, 0.5, 0, 0)
        _HoleSize ("Hole Size (UV)", Vector) = (0.3, 0.2, 0, 0)
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

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 _Color;
            float4 _HoleCenter;
            float4 _HoleSize;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // Вычисляем границы дырки
                float2 holeMin = _HoleCenter.xy - _HoleSize.xy * 0.5;
                float2 holeMax = _HoleCenter.xy + _HoleSize.xy * 0.5;

                // Проверяем, находится ли текущий пиксель внутри дырки
                bool insideHole = (uv.x >= holeMin.x) && (uv.x <= holeMax.x) &&
                                  (uv.y >= holeMin.y) && (uv.y <= holeMax.y);

                if (insideHole)
                    discard; // прозрачная дырка

                return _Color;
            }
            ENDCG
        }
    }
}