Shader "Custom/UVMoveAlongV"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {} 
        _MovementSpeed ("Movement Speed", Float) = 1.0
        _RotationSpeed ("Rotate Speed", Float) = 1.0
        _ColorTint ("Color Tint", Color) = (1,1,1,1)       
        _Brightness ("Brightness", Float) = 0.0           
        _Contrast ("Contrast", Float) = 1.0               
        _Saturation ("Saturation", Float) = 1.0        
        _RimColor ("Rim Color", Color) = (1,1,1,1)         
        _RimPower ("Rim Power", Float) = 2.0                
        _RimIntensity ("Rim Intensity", Float) = 1.0 
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
            
            float4 _ColorTint;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _MovementSpeed;
            float _RotationSpeed;
            float _Brightness;
            float _Contrast;
            float _Saturation;
            float4 _RimColor;
            float _RimPower;
            float _RimIntensity;

            struct appdata
            {
                float3 normal : NORMAL;
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float offset = _MovementSpeed * _Time.x;
                float rotation = _RotationSpeed * _Time.x;
                float2 newUV = i.uv;
                newUV.y += offset;
                newUV.x += rotation;
                fixed4 col = tex2D(_MainTex, newUV);

                col *= _ColorTint;
                col.rgb += _Brightness;
                col.rgb = (col.rgb - 0.5) * _Contrast + 0.5;
                float gray = dot(col.rgb, float3(0.299, 0.587, 0.114));
                col.rgb = lerp(gray, col.rgb, _Saturation);

                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float rim = 1.0 - saturate(dot(i.worldNormal, viewDir));
                rim = pow(rim, _RimPower);
                col.rgb += rim * _RimColor.rgb * _RimIntensity;

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
