Shader "Custom/HoloCRTShader" {
	Properties {
		//_Color ("Color", Color) = (1,1,1,1)
		_Hue ("Hue", Range(0,360)) = 0
		_HueShift ("Hue Shift Rate", Float) = 0
		_Alpha ("Alpha", Range(0,1)) = 1
		_HoloTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 1
		_Metallic ("Metallic", Range(0,1)) = 0
		_Lines ("CRT Lines", Float) = 20
		_FlickerRate ("Flicker Rate", Float) = 0.5
		_FlickerAlpha ("Flicker Alpha", Range(0,1)) = 0.1
		_ScanLineTime ("Scan Line Time", Float) = 1
		_ScanLineSize ("Scan Line Size", Range(0,1)) = 0.05
		_ScanLineAlpha ("Scan Line Alpha", Range(0,1)) = 0.1
		_CRTBrightness ("CRT Brightness", Range(0,1)) = 0.5
		_CRTSize ("CRT Pixels", Int) = 256
	}
	SubShader {
		Tags { "Queue"="Transparent+10" "RenderType" = "Transparent" }
		LOD 200
		GrabPass { "_HoloTex" }
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _HoloTex;

		struct Input {
			float2 uv_HoloTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _Hue;
		float _HueShift;
		float _Alpha;
		float _Lines;
		float _FlickerRate;
		float _FlickerAlpha;
		float _ScanLineTime;
		float _ScanLineSize;
		float _ScanLineAlpha;
		float _CRTBrightness;
		int _CRTSize;

		float pi = 3.14159265359;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			float2 newUV = float2(IN.uv_HoloTex.x, IN.uv_HoloTex.y);
			fixed4 c = tex2D (_HoloTex, newUV); //* _Color;
			//convert tex color to HSV
			float cmax = max(max(c.r, max(c.g, c.b)), .004);
			float cmin = max(min(c.r, min(c.g, c.b)), .004);
			float s = (cmax - cmin) / cmax;
			float v = cmax + fmod((float)IN.uv_HoloTex.y, 1 / _Lines);
			//convert to RGB
			_Hue = fmod(_Hue + _Time.y * _HueShift, 360);
			float R = saturate(abs((_Hue / 360) * 6 - 3) - 1);
			float G = saturate(2 - abs((_Hue / 360) * 6 - 2));
			float B = saturate(2 - abs((_Hue / 360) * 6 - 4));
			float3 hueRGB = float3(R,G,B);
			float4 endColor = float4(((hueRGB - 1) * s + 1) * v, _Alpha);
			o.Albedo = endColor.rgb;
			o.Albedo.r = saturate(min(endColor.r, endColor.r + _CRTBrightness * sign(fmod((round(IN.uv_HoloTex.y * _CRTSize) + round(IN.uv_HoloTex.x * _CRTSize * 3)), 3) - 2) / 2));
			o.Albedo.b = saturate(min(endColor.b, endColor.b + _CRTBrightness * sign(fmod((round(IN.uv_HoloTex.y * _CRTSize) + round(IN.uv_HoloTex.x * _CRTSize * 3)) + 1, 3) - 2) / 2));
			o.Albedo.g = saturate(min(endColor.g, endColor.g + _CRTBrightness * sign(fmod((round(IN.uv_HoloTex.y * _CRTSize) + round(IN.uv_HoloTex.x * _CRTSize * 3)) + 2, 3) - 2) / 2));
			o.Smoothness = _Glossiness;
			o.Metallic = _Metallic;
			o.Alpha = saturate(_Alpha + _FlickerAlpha * sin(_Time.y * _FlickerRate) + saturate(_ScanLineAlpha * sign(-abs(fmod(IN.uv_HoloTex.y - (_Time.y + 100) / _ScanLineTime, 1)) + _ScanLineSize)));
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
