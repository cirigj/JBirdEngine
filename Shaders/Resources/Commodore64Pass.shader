// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Commodore64Pass"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Width ("Width", float) = 0
		_Height ("Height", float) = 0
		_PixelScale ("Pixel Scale", int) = 0
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
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float _Width;
			float _Height;
			int _PixelScale;

			fixed4 frag (v2f i) : SV_Target
			{

				const float3 c64palette[16] = {
					float3(0, 0, 0), //black
					float3(1, 1, 1), //white
					float3(.533, 0, 0), //red
					float3(.667, 1, .933), //cyan
					float3(.8, .267, .8), //violet
					float3(0, .8, .333), //green
					float3(0, 0, .667), //blue
					float3(.933, .933, .467), //yellow
					float3(.867, .533, .333), //orange
					float3(.4, .267, 0), //brown
					float3(1, .467, .467), //light red
					float3(.2, .2, .2), //grey 1
					float3(.467, .467, .467), //grey 2
					float3(.667, 1, .4), //light green
					float3(0, .533, 1), //light blue
					float3(.733, .733, .733) //grey 3
				};

				int c64freq[16] = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

				fixed4 col = tex2D(_MainTex, i.uv);
				//col.rgb = c64palette[(int)(round(col.r * 4) + round(col.g * 16))];
				//return col;

				float tileX = floor(floor(i.uv.x * _Width) / (float)(_PixelScale * 8));
				float tileY = floor(floor(i.uv.y * _Height) / (float)(_PixelScale * 8));

				for (float j = 0; j < 8; j += 1) {
					for (float k = 0; k < 8; k += 1) {
						float4 pixelColor = tex2D(_MainTex, float2((tileX * 8 * (float)_PixelScale + j * (float)_PixelScale + .5) / (_Width), (tileY * 8 * (float)_PixelScale + k * (float)_PixelScale + .5) / (_Height)));
						c64freq[(int)(round(pixelColor.r * 4) + round(pixelColor.g * 16))] += 1;
					}
				}
				float3 color1 = float3(0,0,0);
				float3 color2 = float3(1,1,1);
				int freq1 = 0;
				int freq2 = 0;
				for (int id = 0; id < 16; id++) {
					if (c64freq[id] > freq1) {
						freq2 = freq1;
						color2 = color1;
						freq1 = c64freq[id];
						color1 = c64palette[id];
					}
					else if (c64freq[id] > freq2) {
						freq2 = c64freq[id];
						color2 = c64palette[id];
					}
				}
				col.rgb = c64palette[(int)(round(col.r * 4) + round(col.g * 16))];
				float dist1 = sqrt((col.r - color1.r) * (col.r - color1.r) + (col.g - color1.g) * (col.g - color1.g) + (col.b - color1.b) * (col.b - color1.b));
				float dist2 = sqrt((col.r - color2.r) * (col.r - color2.r) + (col.g - color2.g) * (col.g - color2.g) + (col.b - color2.b) * (col.b - color2.b));
				if (dist1 < dist2) {
					col.rgb = color1;
				}
				else {
					col.rgb = color2;
				}
				return col;
			}
			ENDCG
		}
	}
}
