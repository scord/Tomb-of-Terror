Shader "Custom/Sound Vision" {
	Properties
	{
	}


	

	SubShader{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }

		
		Pass {

		
			Blend SrcAlpha OneMinusSrcAlpha
			ZTest Less

            CGPROGRAM
			#include "UnityCG.cginc"
			#pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

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
	float3 _EchoSource[4];
	float _EchoTime[4];
	float4 _EchoColor;
	sampler2D _Waves;
	int _CurrentWave;
	float _N;
	float _EchoPower[4];

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
                  
            half4 frag (fragmentInput i) : COLOR
            {                                                                                 	                                                                                                             //for rim lighting.
         		half4 color = 0;

				float speed = 10.0f;
				float dist3 = distance(_WorldSpaceCameraPos, i.worldPos);

				for (int x = 0; x < 64; x++)
				{
					half dist = clamp(distance(_SoundSource[x], i.worldPos),0.5, 200);
					half atten = 1/dist3;
					half4 blur = half4(0, 0, 0, 1);

					float dist2 = dist / speed;
	
					float w = dist2*64.0f - floor(dist2*64.0f);
					blur += lerp(tex2D(_Waves, fixed2(dist2, (x) / _N)).rgba, tex2D(_Waves, fixed2(dist2 + 1/64.0f  , (x) / _N)).rgba, w);
					//blur += tex2D(_Waves, fixed2(clamp(speed*dist / 100.0f,0,100), x / _N)).rgba;
					blur *= atten*10.0f/pow(dist/4.0f,2);
					//blur.a = 1/atten;
					color += blur;
				}

				half falloff = 1 - saturate(dot(normalize(i.viewDir), i.normal));
				float4 base_color = _EchoColor*falloff;
				for (int x = 0; x < 4; x++)
				{
					float dist = distance(_EchoSource[x], i.worldPos);


					float dist2 = dist - speed * 2 * _EchoTime[x] * (75.0f / 64.0f);
					float enabled = max(0, sign(speed*2*_EchoTime[x] - dist));
					color += 10*(_EchoPower[x]-1)*(1 - smoothstep(0, _EchoPower[x], _EchoTime[x]))*enabled*_EchoColor / pow(max(abs(dist2),_EchoTime[x]),2);

					//color += enabled*base_color * (1 - smoothstep(0, 8, _EchoTime));
				}
				color += 5/pow(dist3,2)*_EchoColor;
				if (tex2D(_MainTex, i.uv).a <= 0.5)
					color.a = tex2D(_MainTex, i.uv).a;
				else
					color.a = (color.r + color.b + color.g) * 8;
				color.rgb = color.rgb* (1*(dist3/50) + (1-(tex2D(_MainTex, i.uv).r+ tex2D(_MainTex, i.uv).g+ tex2D(_MainTex, i.uv).b)/3))/((dist3/50)+1);


				return color;
            }
            ENDCG
        }

    }
	Fallback "Diffuse"
}

