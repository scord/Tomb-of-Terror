Shader "Custom/Sound Vision" {
	Properties
	{
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	struct fragmentInput {
		float4 pos : SV_POSITION;
		float3 worldPos : TEXCOORD0;
		float2 uv : TEXCOORD1;
		float3 normal : TEXCOORD2;      // Normal needed for rim lighting
		float3 viewDir : TEXCOORD3;     // as is view direction.
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;
	float2 _Volume[64];
	int _Enabled[64];
	float3 _SoundSource[64];
	sampler2D _CameraDepthNormalsTexture;
	float3 _EchoSource;
	float _EchoTime;
	float4 _EchoColor;
	sampler2D _Waves;
	int _CurrentWave;
	float _N;

	fragmentInput vert(appdata_tan v)
	{
		fragmentInput o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.worldPos = mul(_Object2World, v.vertex).xyz;
		o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
		o.normal = normalize(v.normal);
		o.viewDir = normalize(ObjSpaceViewDir(v.vertex));
		return o;
	}

	ENDCG

	SubShader{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }

		
		Pass {

			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			ZTest Less
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
                  
            half4 frag (fragmentInput i) : COLOR
            {                                                                                 	                                                                                                             //for rim lighting.
         		half4 color = 0;

				float speed = 20.0f;
				for (int x = 0; x < _N; x++)
				{
					half dist = clamp(distance(_SoundSource[x], i.worldPos),0.5,100);
					half atten = clamp(distance(_WorldSpaceCameraPos, i.worldPos),0.5,100);
					half4 blur = half4(0, 0, 0, 1);
	
					float w = (dist / speed)*64.0f - floor((dist / speed)*64.0f);
					blur += lerp(tex2D(_Waves, fixed2(dist / speed, (x) / 64.0f)).rgba, tex2D(_Waves, fixed2(dist / speed + 1/64.0f + 0.01f, (x) / 64.0f)).rgba, w);
					blur.a = 1;
					blur.a /= atten;
					color += 2*blur / (dist);
				}
				return color;
            }
            ENDCG
        }

		Pass {

			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite On                   // and if the depth is ok, it renders the main texture.
			ZTest Less
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"


			half4 frag(fragmentInput i) : COLOR
			{

				half a = sin(_Time * 20)*sin(i.worldPos.x / 5) + cos(_Time * 10)*cos(i.worldPos.z / 5) + sin(_Time * 20)*cos(i.worldPos.z / 5);
				half4 ambientColor = _EchoColor*a / 80.0f + _EchoColor / 80.0f;

				half falloff = 1 - saturate(dot(normalize(i.viewDir), i.normal));
	
				half4 color = 0;
				color += ambientColor;

				float speed = 20.0f;

				float dist = clamp(distance(_EchoSource, i.worldPos), 0.5, 100);
				float4 base_color = _EchoColor*falloff;

				float enabled = max(0, sign(speed*_EchoTime - dist));


				color += base_color / (dist*_EchoTime);

				float dist2 = dist - speed * _EchoTime * (75.0f / 64.0f);

				color += _EchoColor / abs(dist2);

				color += base_color * (1 - smoothstep(0, 8, _EchoTime)) ;
			
	
				fixed4 col = tex2D(_MainTex, i.uv);

				return enabled*color*col;
			}
			ENDCG
		}    
    }
	Fallback "Diffuse"
}

