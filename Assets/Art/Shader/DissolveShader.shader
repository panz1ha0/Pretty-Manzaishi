Shader "Custom/DissolveShader"
{
    Properties {
        _BurnAmount ("Burn Amount", Range(0.0, 1.0)) = 0.0

        _LineWidth ("Burn Line Width", Range(0.0, 0.2)) = 0.1

        _MainTex ("Base (RGB)", 2D) = "white" {}

        _BurnFirstColor ("Burn First Color", Color) = (1, 0, 0, 1)
        _BurnSecondColor ("Burn Second Color", Color) = (1, 0, 0, 1)

        _BurnMap ("Burn Map", 2D) = "white" {}
    }

    SubShader {
        Tags {"RenderType"="Opaque" "Queue"="Transparent"}

        Pass {
            Tags {"LigthMode" = "ForwardBase"}
            // Cull Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM

            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            #pragma multi_compile_fwdbase

            #pragma vertex vert
            #pragma fragment frag

            fixed _BurnAmount;
            fixed _LineWidth;
            sampler2D _MainTex;
            fixed4 _BurnFirstColor;
            fixed4 _BurnSecondColor;
            sampler2D _BurnMap;

            float4 _MainTex_ST;
            float4 _BurnMap_ST;

            struct a2v
            {
                float4 vertex : POSITION;
                float4 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uvMainTex : TEXCOORD0;
                float2 uvBurnMap : TEXCOORD1;
                float4 color : COLOR;
            };

            v2f vert(a2v v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                o.uvMainTex = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.uvBurnMap = TRANSFORM_TEX(v.texcoord, _BurnMap);

                o.color = v.color;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed3 burn = tex2D(_BurnMap, i.uvBurnMap);
                clip(burn.r - _BurnAmount);

                fixed4 albedo = tex2D(_MainTex, i.uvMainTex);

                fixed t = 1 - smoothstep(0.0, _LineWidth, burn.r - _BurnAmount);
                fixed3 burnColor = lerp(_BurnFirstColor, _BurnSecondColor, t);
                burnColor = pow(burnColor, 5);

                fixed3 finalColor = lerp(albedo, burnColor, t * step(0.0001, _BurnAmount));

                return fixed4(finalColor, i.color.a);
            }

            ENDCG
        }
    }

    FallBack "Diffuse"
}