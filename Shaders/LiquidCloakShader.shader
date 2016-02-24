Shader "Custom/LiquidCloakShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_IndexRefract ("Index of Refraction", Range(1,5)) = 1.333
	}
	SubShader {
		Tags { "Queue" = "Transparent" }
		LOD 200
		GrabPass { "_GrabTex" }
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _GrabTex;

		struct Input {
			float2 uv_GrabTex;
			float4 screenPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _IndexRefract;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			screenUV.x = ((screenUV.x - 0.5) / _IndexRefract) + 0.5;
			screenUV.y = ((-screenUV.y + 0.5) / _IndexRefract) + 0.5;
			fixed4 c = tex2D (_GrabTex, screenUV) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
