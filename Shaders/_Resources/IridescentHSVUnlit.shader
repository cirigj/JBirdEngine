Shader "Unlit/IridescentHSV" {
	Properties{
		_StartHue("Starting Color", Range(0,359)) = 0.0
		_EndHue("Ending Color", Range(0,359)) = 0.0
		_StartSat("Starting Saturation", Range(0,1)) = 0.0
		_EndSat("Ending Saturation", Range(0,1)) = 0.0
		_StartVal("Starting Value", Range(0,1)) = 0.0
		_EndVal("Ending Value", Range(0,1)) = 0.0
		_Alpha("Transparency", Range(0,1)) = 1.0
		_Pow("Power", Range(.01,10)) = .5
	}

	SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 viewDir : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
			};

			half _StartHue;
			half _EndHue;
			half _StartSat;
			half _EndSat;
			half _StartVal;
			half _EndVal;
			half _Alpha;
			half _Pow;

			half getAngleBetweenVectors(fixed3 vec1, fixed3 vec2) {
				return clamp((acos(clamp(dot(vec1, vec2), -1, 1)) * 57.29578) / 180, 0, 1);
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

			half3 colorFromHSV(half h, half s, half v) {
				half c = v * s;
				half x = c * (1 - abs((h / 60 - 2 * floor(h / 120)) - 1));
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

			v2f vert(float4 vertex : POSITION, float3 normal : NORMAL)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(vertex);
				o.viewDir = normalize(ObjSpaceViewDir(vertex));
				o.worldNormal = UnityObjectToWorldNormal(normal);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				//fixed4 col = getColor(i.viewDir, i.worldNormal);
				fixed4 col;
				col.rgb = getColor(i.viewDir, i.worldNormal);
				col.a = _Alpha;
				return col;
			}
			ENDCG

		}
	}
}