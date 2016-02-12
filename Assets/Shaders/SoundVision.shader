Shader "Custom/Sound Vision" {
	Properties
	{
	}
	SubShader {
        Tags { "RenderType" = "Opaque" }
        Pass {
        	ZTest LEqual  
            ZWrite On                   // and if the depth is ok, it renders the main texture.
            Cull Back  
            LOD 200           
			AlphaTest Less 0.1
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
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
            float4 _Colors[50];  
            float2 _Volume[64];
            float3 _SoundSource[64]; 
			float3 _EchoSource;
			float _EchoTime;
			sampler2D _Waves;
            int _CurrentWave;
            float _N;
           
            fragmentInput vert (appdata_tan v)
            {
                fragmentInput o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.worldPos = mul (_Object2World, v.vertex).xyz;
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.normal = normalize(v.normal);
                o.viewDir = normalize(ObjSpaceViewDir(v.vertex));    
                return o;                                               
            }
           
            half4 frag (fragmentInput i) : COLOR
            {
            	//float dist = distance(_SoundSource[_CurrentWave], i.worldPos);
                half falloff = 1 - saturate(dot(normalize(i.viewDir), i.normal));      
                                                                                                                        	                                                                                                             //for rim lighting.
         		half4 color = 0;
             //   return clamp((base_color / abs(dist - 25*_Colors[_CurrentWave].w)),0,1);
             	//if (distance(_SoundSource[_CurrentWave], i.worldPos) < 5*_Colors[_CurrentWave].w)
             	
             	
				//float dist = distance(_SoundSource[_CurrentWave], i.worldPos);
				//if (dist < 25*_Colors[_CurrentWave].w)
				float speed = 20.0f;
				for (int x = 0; x < _N; x++)
				{
					half dist = distance(_SoundSource[x], i.worldPos);
					half4 blur = half4(0, 0, 0, 1);
	
					float w = (dist / speed)*64.0f - floor((dist / speed)*64.0f);
					blur += lerp(tex2D(_Waves, fixed2(dist / speed, (x) / 64.0f)).rgba, tex2D(_Waves, fixed2(dist / speed + 1/64.0f, (x) / 64.0f)).rgba,w);

					half4 base_color = blur * pow(falloff, 1);
					color += pow(base_color,2) / (2*pow(dist/5,2));
					color += blur / pow(dist,2);
				}

				float dist = distance(_EchoSource, i.worldPos);
				float4 base_color = float4(0,0.5,1,1)*pow(falloff, 2);
				if (_EchoTime > 0 && dist < speed*_EchoTime)
					color += base_color / (dist*_EchoTime);

				float dist2 = dist - speed * _EchoTime;
				color += clamp((float4(0,0.5,1,1) / (pow(abs(dist - speed * _EchoTime), 2))), 0, 1) / (pow(dist, 2) / 10);
								
              /*  for (int x = 0; x < _N; x++) {
                	float dist = distance(_SoundSource[x], i.worldPos);
					float t = 15 * _Colors[x].w;
					float dist2 = dist - t;
					float d = abs(dist2);
					float ss;
					half4 base_color = _Volume[x].x*_Colors[x] * pow(falloff, 1);
					if (dist < t)
						color += base_color * (1 - smoothstep(0, 15, dist))*(1-smoothstep(0,1, _Colors[x].w));
					//color += _Volume[x].x*clamp((_Colors[x] / (pow(abs(dist - 25 * _Colors[x].w), 2))), 0, 1) / (pow(dist, 2) / 10);
					//color += _Volume[x].x*(_Colors[x] / (abs(dist2)))*(1-smoothstep(0,15,dist));
					color += _Volume[x].x*_Colors[x] * (1-smoothstep(0, 0.1, d));
				}*/
			
				
				return color;
  
            }
            ENDCG
        }
         
           
        }
      
		
		
	
		Fallback "Diffuse"
}

