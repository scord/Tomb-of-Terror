Shader "Custom/Sound Vision" {
	Properties
	{

	}
	SubShader{
		Tags{ "RenderType" = "Opaque" }

		Lighting Off
		CGPROGRAM
#pragma surface surf None

	struct Input {
		float3 worldPos;
		float3 worldNormal;
		float4 color : COLOR;
	};

	float _N;
	float3 _SoundSource[100];
	float4 _Colors[100];


	half4 LightingNone(SurfaceOutput s, fixed3 lightDir, fixed atten) {
		half4 c;
		c.rgb = s.Albedo;
		c.a = atten;
		return c;
	}

	void surf(Input IN, inout SurfaceOutput o) {


		float speed = 25;
		for (int i = 0; i < _N; i++) {

				float dist = distance(_SoundSource[i], IN.worldPos);

				o.Emission += clamp((_Colors[i].xyz / pow(abs(dist - speed*_Colors[i].w), 2)),0,1)/ (pow(dist,1.5)/5);

		}


	}
	ENDCG
	}
		Fallback "Diffuse"
}

