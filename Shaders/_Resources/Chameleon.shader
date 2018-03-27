Shader "Custom/Chameleon" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_IndexRefract ("Index of Refraction", Range(1,5)) = 1.333
		_Transparency ("Transparency", Range(0,1)) = 0
		_Color1 ("Color1", Color) = (1,1,1,1)
		_Color2 ("Color2", Color) = (1,1,1,1)
		_Color3 ("Color3", Color) = (1,1,1,1)
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
			float2 uv_MainTex;
			float4 screenPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _IndexRefract;
		sampler2D _MainTex;
		float _Transparency;
		fixed4 _Color1;
		fixed4 _Color2;
		fixed4 _Color3;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			screenUV.x = ((screenUV.x - 0.5) / _IndexRefract) + 0.5;
			screenUV.y = ((-screenUV.y + 0.5) / _IndexRefract) + 0.5;
			fixed4 cTrans = (tex2D (_GrabTex, screenUV) * _Color);
			fixed4 cRaw = tex2D (_MainTex, IN.uv_MainTex.xy);
			fixed4 cSkin = _Color2 * clamp(cRaw.r * -2 + 1, 0, 1) + _Color1 * (1 - (abs(cRaw.r * 2 - 0.5))) + _Color3 * clamp(cRaw.r * 2 - 1, 0, 1);
			o.Albedo = cTrans * _Transparency + cSkin * (1 - _Transparency);
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = 1.0;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
