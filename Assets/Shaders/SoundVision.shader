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
            float4 _Colors[100];  
            
            float3 _SoundSource[100]; 
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
                half4 base_color = _Colors[_CurrentWave] * pow(falloff, 1);                                                                                                            	                                                                                                             //for rim lighting.
         		half4 color = 0;
             //   return clamp((base_color / abs(dist - 25*_Colors[_CurrentWave].w)),0,1);
             	//if (distance(_SoundSource[_CurrentWave], i.worldPos) < 5*_Colors[_CurrentWave].w)
             	
             	
				float dist = distance(_SoundSource[_CurrentWave], i.worldPos);
				if (dist < 25*_Colors[_CurrentWave].w)
					color += base_color/(dist*_Colors[_CurrentWave].w);
				
								                        
								
                for (int x = 0; x < _N; x++) {
                	dist = distance(_SoundSource[x], i.worldPos);
					float dist2 = dist - 25*_Colors[x].w;
					color += clamp((_Colors[x] / (pow(abs(dist - 25*_Colors[x].w), 2))),0,1)  / (pow(dist,2)/10);
				}
			
				
				return color;
  
            }
            ENDCG
        }
         
           
        }
      
		
		
	
		Fallback "Diffuse"
}

