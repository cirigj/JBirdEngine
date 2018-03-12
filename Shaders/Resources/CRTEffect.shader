// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/CRTEffect"
{
	Properties
	{
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Lerp ("Lerping in and out", Float) = 1
		_Lines ("CRT Lines", Float) = 64
		_LineDarkness ("Line Darkness", Float) = 2.5
		_ScanLineTime ("Scan Line Time", Float) = 1
		_ScanLineSize ("Scan Line Size", Range(0,1)) = 0.05
		_ScanLineAlpha ("Scan Line Alpha", Range(0,1)) = 0.1
		_Static ("Static", Range(-.5,.5)) = 0.2
		_CRTBrightness ("CRT Brightness", Range(0,1)) = 0.5
		_CRTSize ("CRT Pixels", Int) = 256
		_Fisheye ("Fisheye Map", 2D) = "black" {}
		_FisheyeStrength ("Fisheye Strength", Float) = .05
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
			float _Lerp;
			float _Lines;
			float _LineDarkness;
			float _ScanLineTime;
			float _ScanLineSize;
			float _ScanLineAlpha;
			float _Static;
			float _CRTBrightness;
			int _CRTSize;
			sampler2D _Fisheye;
			float _FisheyeStrength;

			float pi = 3.14159265359;

			float rand(float2 inVec)  {
	            return frac(sin(dot(inVec * sin(_Time.x), float2(12.9898,78.233))) * 43758.5453);
	        }

			fixed4 frag (v2f i) : SV_Target
			{
				float3 fisheyeUV = tex2D(_Fisheye, i.uv);
				i.uv -= (fisheyeUV.xy * 2 - 1) * _FisheyeStrength * _Lerp;
				i.uv = saturate(i.uv);
				fixed4 col = tex2D(_MainTex, i.uv);
				float4 o;
				o.rgb = col.rgb;
				o.r = saturate(min(col.r, col.r + (_CRTBrightness * sign(fmod((round(i.uv.y * _CRTSize) + round(i.uv.x * _CRTSize * 3)), 3) - 2) / 2) * _Lerp));
				o.b = saturate(min(col.b, col.b + (_CRTBrightness * sign(fmod((round(i.uv.y * _CRTSize) + round(i.uv.x * _CRTSize * 3)) + 1, 3) - 2) / 2) * _Lerp));
				o.g = saturate(min(col.g, col.g + (_CRTBrightness * sign(fmod((round(i.uv.y * _CRTSize) + round(i.uv.x * _CRTSize * 3)) + 2, 3) - 2) / 2) * _Lerp));
				o.rgb += float3(1,1,1) * _Lerp * _ScanLineAlpha * saturate(_ScanLineAlpha * sign(-abs(fmod(i.uv.y - (_Time.y + 100) / _ScanLineTime, 1)) + _ScanLineSize));
				o.rgb += float3(1,1,1) * _Lerp * _Static * rand(i.uv);
				o.rgb -= float3(1,1,1) * _Lerp * _LineDarkness * round(fmod(i.uv.y, 1.0 / _Lines) * _Lines);
				o.a = 1.0;
				return o;
			}
			ENDCG
		}
	}
}
