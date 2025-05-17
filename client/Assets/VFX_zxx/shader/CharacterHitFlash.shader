Shader "Custom/CharacterHitFlash"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)  
        _FlashColor ("Flash Color", Color) = (1,0,0,1) 
        _FlashAmount ("Flash Intensity", Range(0,1)) = 0
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

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _BaseColor;
            float4 _FlashColor;
            float _FlashAmount;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 颜色混合：原色和受伤时的红色闪烁
                fixed4 finalColor = lerp(_BaseColor, _FlashColor, _FlashAmount);
                return finalColor;
            }
            ENDCG
        }
    }
}
