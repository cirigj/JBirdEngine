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
		_SatCorrect ("Saturation Correction", float) = 0
		_CustomPalette ("CustomPalette", 3D) = "white" {}
		_TexSize ("Texture Size", int) = 0
		_Scale ("LUT Scale", float) = 0
		_Offset ("LUT Offset", float) = 0
		_GBAColor1 ("GBA Color 1", Color) = (0,0,0,1)
		_GBAColor2 ("GBA Color 2", Color) = (.33,.33,.33,1)
		_GBAColor3 ("GBA Color 3", Color) = (.67,.67,.67,1)
		_GBAColor4 ("GBA Color 4", Color) = (1,1,1,1)
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
			float _SatCorrect;
			sampler3D _CustomPalette;
			int _TexSize;
			float _Scale;
			float _Offset;
			float4 _GBAColor1;
			float4 _GBAColor2;
			float4 _GBAColor3;
			float4 _GBAColor4;

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
				if (hue >= 360) {
					hue -= 360;
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
					s = round((s + _SatCorrect) * 2) / 2;

					//adjust value
					if ((int)pixelX % 2 == (int)pixelY % 2) {
						v -= _Checkering;
					}
					v = round(v * 2) / 2;

					//convert to RGB
					float R = saturate(abs((hue / 360) * 6 - 3) - 1);
					float G = saturate(2 - abs((hue / 360) * 6 - 2));
					float B = saturate(2 - abs((hue / 360) * 6 - 4));
					float3 hueRGB = float3(R,G,B);
					float4 endColor = float4(((hueRGB - 1) * s + 1) * v, 1.0);

					return endColor;

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
					s = clamp(round((s + _SatCorrect) * 2) / 3 + .1, 0, 1);

					//adjust value
					if ((int)pixelX % 2 == (int)pixelY % 2) {
						v -= _Checkering;
					}
					v = clamp(round(v * 3) / 4 + .6, 0, 1);

					//convert to RGB
					float R = saturate(abs((hue / 360) * 6 - 3) - 1);
					float G = saturate(2 - abs((hue / 360) * 6 - 2));
					float B = saturate(2 - abs((hue / 360) * 6 - 4));
					float3 hueRGB = float3(R,G,B);
					float4 endColor = float4(((hueRGB - 1) * s + 1) * v, 1.0);

					return endColor;

				}

				//NES Emulation mode
				if (_Mode == 2) {

					//adjust hue
					hue = round(hue / 30) * 30;
					if (hue >= 360) {
						hue -= 360;
					}

					//adjust saturation
					if ((int)pixelX % 2 == (int)pixelY % 2) {
						s -= _Checkering;
					}
					s = clamp(round(s + _SatCorrect), 0, 1);

					//get luma
					float luma = r * 0.299 + g * 0.587 + b * 0.114;
					if ((int)pixelX % 2 == (int)pixelY % 2) {
						luma -= _Checkering;
					}
					luma = clamp(round(luma * 5) / 5, 0, 1);

                    float rVal = 0.299;
                    float gVal = 0.587;
                    float bVal = 0.114;

                    float red = 0;
                    float green = 0;
                    float blue = 0;

                    switch (hue) {
                    case 0: //red
                        if (luma > rVal) {
                            red = 1;
                            green = blue = (luma - rVal) / (gVal + bVal);
                        }
                        else {
                            red = (luma / rVal);
                            green = blue = 0;
                        }
                        break;
                    case 60: //yellow
                        if (luma > rVal + gVal) {
                            red = green = 1;
                            blue = (luma - rVal - gVal) / bVal;
                        }
                        else {
                            red = green = (luma / (rVal + gVal));
                            blue = 0;
                        }
                        break;
                    case 120: //green
                        if (luma > gVal) {
                            green = 1;
                            red = blue = (luma - gVal) / (rVal + bVal);
                        }
                        else {
                            green = (luma / gVal);
                            red = blue = 0;
                        }
                        break;
                    case 180: //cyan
                        if (luma > gVal + bVal) {
                            green = blue = 1;
                            red = (luma - gVal - bVal) / rVal;
                        }
                        else {
                            green = blue = (luma / (gVal + bVal));
                            red = 0;
                        }
                        break;
                    case 240: //blue
                        if (luma > bVal) {
                            blue = 1;
                            red = green = (luma - bVal) / (rVal + gVal);
                        }
                        else {
                            blue = (luma / bVal);
                            red = green = 0;
                        }
                        break;
                    case 300: //magenta
                        if (luma > rVal + bVal) {
                            red = blue = 1;
                            green = (luma - rVal - bVal) / gVal;
                        }
                        else {
                            red = blue = (luma / (rVal + bVal));
                            green = 0;
                        }
                        break;
                    default:
                        float fHue = (float)hue;
                        if (hue > 0 && hue < 60) { //between red and yellow
                            float huePercentage = (60 / (fHue - 0));
                            float xVal = (rVal * huePercentage + gVal);
                            float x = luma / xVal;
                            red = x * huePercentage;
                            green = x;
                            blue = 0;
                            if (red > 1) {
                                red = 1;
                                green = 1 / huePercentage;
                                float yVal = (gVal * (1 - green) + bVal);
                                float y = (luma - rVal - gVal * green) / yVal;
                                green += y * (1 - (1 / huePercentage));
                                blue = y;
                            }
                        }
                        else if (hue > 60 && hue < 120) { //between yellow and green
                            float huePercentage = (60 / (120 - fHue));
                            float xVal = (gVal * huePercentage + rVal);
                            float x = luma / xVal;
                            green = x * huePercentage;
                            red = x;
                            blue = 0;
                            if (green > 1) {
                                green = 1;
                                red = 1 / huePercentage;
                                float yVal = (rVal * (1 - red) + bVal);
                                float y = (luma - gVal - rVal * red) / yVal;
                                red += y * (1 - (1 / huePercentage));
                                blue = y;
                            }
                        }
                        else if (hue > 120 && hue < 180) { //between green and cyan
                            float huePercentage = (60 / (fHue - 120));
                            float xVal = (gVal * huePercentage + bVal);
                            float x = luma / xVal;
                            green = x * huePercentage;
                            blue = x;
                            red = 0;
                            if (green > 1) {
                                green = 1;
                                blue = 1 / huePercentage;
                                float yVal = (bVal * (1 - blue) + rVal);
                                float y = (luma - gVal - bVal * blue) / yVal;
                                blue += y * (1 - (1 / huePercentage));
                                red = y;
                            }
                        }
                        else if (hue > 180 && hue < 240) { //between cyan and blue
                            float huePercentage = (60 / (240 - fHue));
                            float xVal = (bVal * huePercentage + gVal);
                            float x = luma / xVal;
                            blue = x * huePercentage;
                            green = x;
                            red = 0;
                            if (blue > 1) {
                                blue = 1;
                                green = 1 / huePercentage;
                                float yVal = (gVal * (1 - green) + rVal);
                                float y = (luma - bVal - gVal * green) / yVal;
                                green += y * (1 - (1 / huePercentage));
                                red = y;
                            }
                        }
                        else if (hue > 240 && hue < 300) { //between blue and magenta
                            float huePercentage = (60 / (fHue - 240));
                            float xVal = (bVal * huePercentage + rVal);
                            float x = luma / xVal;
                            blue = x * huePercentage;
                            red = x;
                            green = 0;
                            if (blue > 1) {
                                blue = 1;
                                red = 1 / huePercentage;
                                float yVal = (rVal * (1 - red) + gVal);
                                float y = (luma - bVal - rVal * red) / yVal;
                                red += y * (1 - (1 / huePercentage));
                                green = y;
                            }
                        }
                        else if (hue > 300 && hue < 360) { //between magenta and red
                            float huePercentage = (60 / (360 - fHue));
                            float xVal = (rVal * huePercentage + bVal);
                            float x = luma / xVal;
                            red = x * huePercentage;
                            blue = x;
                            green = 0;
                            if (red > 1) {
                                red = 1;
                                blue = 1 / huePercentage;
                                float yVal = (bVal * (1 - blue) + gVal);
                                float y = (luma - rVal - bVal * blue) / yVal;
                                blue += y * (1 - (1 / huePercentage));
                                green = y;
                            }
                        }
                        break;
                    }
                    if (s == 0) {
                        return fixed4 (luma, luma, luma, 1.0);
                    }
                    else {
                        return fixed4(red, green, blue, 1.0);
                    }

                }

				//custom palette
				if (_Mode == 3) {
					if ((int)pixelX % 2 == (int)pixelY % 2) {
						col.rgb += _Checkering;
					}
					//col.rgb = sqrt(col.rgb);
					col.r = floor(col.r * (float)_TexSize) / (float)_TexSize;
					col.g = floor(col.g * (float)_TexSize) / (float)_TexSize;
					col.b = floor(col.b * (float)_TexSize) / (float)_TexSize;
					col.rgb = tex3D(_CustomPalette, col.rgb * _Scale + _Offset).rgb;
					//col.rgb = col.rgb*col.rgb; 
					return col;
				}

				//grayscale ramp
				if (_Mode == 4) {
					float luma = r * 0.299 + g * 0.587 + b * 0.114;
					if ((int)pixelX % 2 == (int)pixelY % 2) {
						luma -= _Checkering;
					}
					//luma = clamp(round(luma * 3) / 3, 0, 1);
					//return fixed4 (luma, luma, luma, 1.0);
					if (luma <= .25) {
						return _GBAColor1;
					}
					else if (luma <= .5) {
						return _GBAColor2;
					}
					else if (luma <= .75) {
						return _GBAColor3;
					}
					else {
						return _GBAColor4;
					}

				}

                return fixed4(1,1,1,1);
                
			}
			ENDCG
		}
	}
}
