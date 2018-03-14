Shader "Custom/IridescentHSV" {
	Properties {
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_StartHue ("Starting Color", Range(0,359)) = 0.0
		_EndHue ("Ending Color", Range(0,359)) = 0.0
		_StartSat ("Starting Saturation", Range(0,1)) = 0.0
		_EndSat("Ending Saturation", Range(0,1)) = 0.0
		_StartVal("Starting Value", Range(0,1)) = 0.0
		_EndVal("Ending Value", Range(0,1)) = 0.0
		_Pow ("Power", Range(.01,10)) = .5
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input {
			float3 viewDir;
			float3 worldNormal;
		};

		half _Glossiness;
		half _Metallic;
		half _StartHue;
		half _EndHue;
		half _StartSat;
		half _EndSat;
		half _StartVal;
		half _EndVal;
		half _Pow;

		half getAngleBetweenVectors(fixed3 vec1, fixed3 vec2) {
			return clamp((acos(clamp(dot(vec1, vec2), -1, 1)) * 57.29578) / 180, 0, 1);
		}

		half getHue(half3 color) {
			half hue;
			half cMax = max(color.r, max(color.g, color.b));
			half cMin = min(color.r, min(color.g, color.b));
			half delta = cMax - cMin;
			//HUE
			if (delta == 0) {
				hue = 0;
			}
			else if (cMax == color.r) {
				hue = ((color.g - color.b)) / delta;
			}
			else if (cMax == color.g) {
				hue = 2 + (color.b - color.r) / delta;
			}
			else {
				hue = 4 + (color.r - color.g) / delta;
			}
			//Convert to degrees
			hue *= 60;
			if (hue < 0) {
				hue += 360;
			}
			return hue;
		}

		half shiftHue(half startHue, half degrees) {
			half newHue = startHue + degrees;
			if (newHue > 360) {
				newHue -= 360;
			}
			if (newHue < 0) {
				newHue += 360;
			}
			return newHue;
		}

		half getVal(half3 color) {
			return max(color.r, max(color.g, color.b));
		}

		half getSat(half3 color) {
			half cMax = max(color.r, max(color.g, color.b));
			half cMin = min(color.r, min(color.g, color.b));
			half delta = cMax - cMin;
			if (cMax == 0) {
				return 0;
			}
			else {
				return delta / cMax;
			}
		}

		half3 colorFromHSV(half h, half s, half v) {
			half c = v * s;
			half x = c * (1 - abs((h/60 - 2 * floor(h/120)) - 1));
			half m = v - c;
			half r = 0;
			half g = 0;
			half b = 0;
			if (0 <= h && h < 60) {
				r = c + m;
				g = x + m;
				b = m;
			}
			else if (60 <= h && h < 120) {
				r = x + m;
				g = c + m;
				b = m;
			}
			else if (120 <= h && h < 180) {
				r = m;
				g = c + m;
				b = x + m;
			}
			else if (180 <= h && h < 240) {
				r = m;
				g = x + m;
				b = c + m;
			}
			else if (240 <= h && h < 300) {
				r = x + m;
				g = m;
				b = c + m;
			}
			else {
				r = c + m;
				g = m;
				b = x + m;
			}
			return half3(r, g, b);
		}

		half getLerpHue(half t) {
			half startHue = _StartHue;
			half endHue = _EndHue;
			half hueDiff = endHue - startHue;
			if (hueDiff > 180) {
				hueDiff -= 360;
			}
			if (hueDiff < -180) {
				hueDiff += 360;
			}
			return shiftHue(startHue, hueDiff * t);
		}

		half getLerpSat(half t) {
			return lerp(_StartSat, _EndSat, t);
		}

		half getLerpVal(half t) {
			return lerp(_StartVal, _EndVal, t);
		}

		half3 getColor(fixed3 vD, fixed3 wN) {
			half t = pow(getAngleBetweenVectors(vD, wN), _Pow);
			return colorFromHSV(getLerpHue(t), getLerpSat(t), getLerpVal(t));
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			//fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			//o.Albedo = c.rgb;

			half t = getAngleBetweenVectors(IN.viewDir, IN.worldNormal);
			half w = getLerpHue(t) / 360;
			//o.Albedo = half3(t, t, t);

			o.Albedo = getColor(IN.viewDir, IN.worldNormal);
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = 1.0;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
