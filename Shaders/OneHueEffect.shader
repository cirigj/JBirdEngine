Shader "Hidden/OneHueEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Hue ("Hue to not convert to greyscale", Range(0,359)) = 0
		_Tolerance ("Number of hues around the current hue to accept", Range(0, 180)) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			int _Hue;
			int _Tolerance;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				//get hue
				float hue;
				float r = col.r;
				float g = col.g;
				float b = col.b;
				float maxC = max(r, max(g, b));
				float minC = min(r, min(g, b));
				if (maxC == r) {
					hue = ((g - b) / (maxC - minC));
				} else if (maxC == g) {
					hue = 2 + ((b - r) / (maxC - minC));
				} else {
					hue = 4 + ((r - g) / (maxC - minC));
				}
				int hueInt = (int)(hue * 60);
				if (hueInt < 0) {
					hueInt += 360;
				}
				//don't adjust the color if it is the right hue
				if (hueInt >= (_Hue - _Tolerance) % 360 && hueInt <= (_Hue + _Tolerance) % 360) {
					return col;
				}
				//convert to greyscale on all other hues
				float luma = r * 0.299 + g * 0.587 + b * 0.114;
				return float4 (luma, luma, luma, col.a);
			}
			ENDCG
		}
	}
}
