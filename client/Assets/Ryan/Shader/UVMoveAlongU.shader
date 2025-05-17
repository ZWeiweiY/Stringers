Shader "Custom/UVMoveAlongU"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {} 
        _Speed ("Movement Speed", Float) = 1.0     
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
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Speed;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float offset = _Speed * _Time.x;
                float2 newUV = i.uv;
                newUV.x += offset;
                fixed4 col = tex2D(_MainTex, newUV);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
