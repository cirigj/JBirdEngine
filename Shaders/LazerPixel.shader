Shader "Hidden/LazerPixel"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Width ("Width", float) = 0
		_Height ("Height", float) = 0
		_PixelScale ("Pixel Scale", int) = 0
		_Checkering ("Checkering", float) = .05
		_Mode ("Color Palette Mode", int) = 0
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
			float _Width;
			float _Height;
			int _PixelScale;
			float _Checkering;
			int _Mode;

			fixed4 frag (v2f i) : SV_Target
			{
				float pixelX = floor(i.uv.x * _Width / (float)_PixelScale);
				float pixelY = floor(i.uv.y * _Height / (float)_PixelScale);
				float totalRed = 0; float totalGreen = 0; float totalBlue = 0;
				float numColors = 0;
				for (float i = 0; i < _PixelScale; i += 1) {
					for (float j = 0; j < _PixelScale; j += 1) {
						float4 pixelColor = tex2D(_MainTex, float2((pixelX * (float)_PixelScale + i) / _Width, (pixelY * (float)_PixelScale + j) / _Height));
						totalRed += pixelColor.r;
						totalGreen += pixelColor.g;
						totalBlue += pixelColor.b;
						numColors += 1;
					}
				}
				fixed4 col = fixed4(totalRed / numColors, totalGreen / numColors, totalBlue / numColors, 1.0);

				//get hue
				float hue;
				float r = totalRed / numColors;
				float g = totalGreen / numColors;
				float b = totalBlue / numColors;
				float maxC = max(r, max(g, b));
				float minC = min(r, min(g, b));
				if (maxC == r) {
					hue = ((g - b) / (maxC - minC));
				} else if (maxC == g) {
					hue = 2 + ((b - r) / (maxC - minC));
				} else {
					hue = 4 + ((r - g) / (maxC - minC));
				}
				hue *= 60;
				if (hue < 0) {
					hue += 360;
				}

				//other HSV stuff
				float s = (maxC - minC) / maxC;
				float v = maxC;

				//51-color mode
				if (_Mode == 0) {

					//adjust hue
					hue = round(hue / 30) * 30;

					//adjust saturation
					if ((int)pixelX % 2 == (int)pixelY % 2) {
						s -= _Checkering;
					}
					s = round(s * 2) / 2;

					//adjust value
					if ((int)pixelX % 2 == (int)pixelY % 2) {
						v -= _Checkering;
					}
					v = round(v * 2) / 2;

				}

				// A E S T H E T I C  M O D E
				else if (_Mode == 1) {

					//adjust hue
					int closestMargin = 360;
					int closestHue = 0;
					int hues[14] = {27, 51, 127, 157, 181, 182, 224, 225, 228, 269, 294, 316, 327, 348};
					for (int i = 0; i < 14; i++) {
						int margin = abs(hue - hues[i]);
						if (margin < closestMargin) {
							closestMargin = margin;
							closestHue = hues[i];
						}
					}
					hue = closestHue;

					//adjust saturation
					if ((int)pixelX % 2 == (int)pixelY % 2) {
						s -= _Checkering;
					}
					s = clamp(round(s * 2) / 3 + .1, 0, 1);

					//adjust value
					if ((int)pixelX % 2 == (int)pixelY % 2) {
						v -= _Checkering;
					}
					v = clamp(round(v * 3) / 4 + .6, 0, 1);

				}

				//convert to RGB
				float R = saturate(abs((hue / 360) * 6 - 3) - 1);
				float G = saturate(2 - abs((hue / 360) * 6 - 2));
				float B = saturate(2 - abs((hue / 360) * 6 - 4));
				float3 hueRGB = float3(R,G,B);
				float4 endColor = float4(((hueRGB - 1) * s + 1) * v, 1.0);

				//get luma
				float luma = r * 0.299 + g * 0.587 + b * 0.114;
				float clampedLuma = round(luma * 4) / 4;

				//fixed4 col = fixed4(clampedLuma, clampedLuma, clampedLuma, 1.0);
				//return col;

				return endColor;
			}
			ENDCG
		}
	}
}
