Shader "Unlit/SpaceUnlit"
{
    Properties
    {
        _MainTex ("BG", 2D) = "white" {}
        _CloudTex ("Nebula", 2D) = "white" {}
        _StarsTex ("Stars", 2D) = "white" {}
        _Scroll ("Cloud Scroll", Vector) = (0,0,0,0)
        _StarBrightness ("Star Brightness", Float) = 3
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _CloudTex;
            float4 _CloudTex_ST;
            sampler2D _StarsTex;
            float4 _StarsTex_ST;
            float4 _Scroll;
            float _StarBrightness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                float2 scrollUV = float2((i.uv.x + _Time.x * _Scroll.x) % 1, (i.uv.y + _Time.x * _Scroll.y) % 1);
                fixed4 cloudCol = tex2D(_CloudTex, scrollUV);
                fixed4 starCol = tex2D(_StarsTex, i.uv);
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                starCol *= _StarBrightness * max(sin(i.uv.x * 100 + i.uv.y * 50 + _Time.w), 0);
                return col + cloudCol * cloudCol.a + starCol;
            }
            ENDCG
        }
    }
}
