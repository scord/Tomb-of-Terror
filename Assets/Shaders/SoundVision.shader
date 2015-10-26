Shader "Custom/Sound Vision" {
	Properties
    	{

		}
    SubShader {
      	Tags { "RenderType" = "Opaque" }
      
		Lighting Off
      	CGPROGRAM
      	#pragma surface surf Lambert
      	
      	struct Input {
          float3 worldPos;
		};
      

      	float3 _SoundSources[3];
      	float3 _Colors[3];

      	
      	half4 LightingNone (SurfaceOutput s, fixed4 color) {
      		half4 c;
      		c.rgb = s.Albedo;
      		c.a = s.Alpha;
      		return c;
      	}

		void surf (Input IN, inout SurfaceOutput o) {
		
			for (int i = 0; i < 3; i++) {
			float t = _Time.y;
			float speed = 15;
			float w = 25;
			float dist = distance(_SoundSources[i], IN.worldPos);

			if (fmod(dist, w) < fmod(speed*t, w) && fmod(dist, w) > fmod(speed*t,w)- 5) {
				o.Emission += (fmod(dist, w) - fmod(speed*t, w) + 5.0) * _Colors[i] / 5.0 ;
			} else 
			if (fmod(dist, w) < fmod(speed*t, w) + w && fmod(dist, w) > fmod(speed*t,w) - 5+ w ){
				o.Emission += (fmod(dist, w) - (fmod(speed*t, w) + w) + 5.0) * _Colors[i] / 5.0 ;
			}
			}
		}
		ENDCG
	}
	Fallback "Diffuse"
}
		  	
