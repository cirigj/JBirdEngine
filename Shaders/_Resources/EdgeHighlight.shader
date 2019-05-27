Shader "Unlit/EdgeHighlight" {
	Properties{
		_Color("Color 1", Color) = (1,1,1,1)
		_Color2("Color 2", Color) = (1,1,1,1)
		_BlinkSpeed("Blink Speed", Float) = 1
		_CutOff("CutOff", Range(0,1)) = 0.1
		_FallOff("FallOff", Range(0,1)) = 0.1
		_Pow("Power", Range(.01,10)) = .5
		_BlinkTime("Time", Float) = 0
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

	half4 _Color;
	half4 _Color2;
	half _BlinkSpeed;
	half _BlinkTime;
	half _CutOff;
	half _FallOff;
	half _Pow;

	half getAngleBetweenVectors(fixed3 vec1, fixed3 vec2) {
		return clamp((acos(clamp(dot(vec1, vec2), -1, 1)) * 57.29578) / 180, 0, 1);
	}

	half4 getColor(fixed3 vD, fixed3 wN) {
		half t = pow(getAngleBetweenVectors(vD, wN), _Pow);
		t = clamp((t - _CutOff) / (_FallOff), 0, 1);
		return lerp(half4(0, 0, 0, 0), lerp(_Color, _Color2, sin(_BlinkTime * _BlinkSpeed)), t);
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
		col = getColor(i.viewDir, i.worldNormal);
		return col;
	}
		ENDCG

	}
	}
}