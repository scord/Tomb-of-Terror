Shader "Custom/Sound Vision" {
	Properties
	{

	}
	SubShader{
		Tags{ "RenderType" = "Opaque" }

		Lighting Off
		CGPROGRAM
#pragma surface surf Lambert

	struct Input {
		float3 worldPos;
	};


	float3 _SoundSources[3];
	float4 _Colors[3];
	float _Dist[3];


	half4 LightingNone(SurfaceOutput s, fixed4 color) {
		half4 c;
		c.rgb = s.Albedo;
		c.a = s.Alpha;
		return c;
	}

	void surf(Input IN, inout SurfaceOutput o) {


		float speed = 15;
		for (int i = 0; i < 3; i++) {
			if (_Colors[i].w < 10.0) {
				float dist = distance(_SoundSources[i], IN.worldPos);

				if (dist < speed*_Colors[i].w) {
					o.Emission += 10/(pow(dist,2)) * _Colors[1] * (1 - _Colors[i].w/10) ;
				}
			}
		}

	
	}
	ENDCG
	}
		Fallback "Diffuse"
}

