Shader "Unlit/CelShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CelMap ("Cel Map", 2D) = "white" {}
        _Brightness("Brightness", float) = 0.5
        _OutlineSize ("Outline size", float) = 0.05
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)

    }
    SubShader
    {
        Tags { "RenderType"="Opaque"}
        
    
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            
            sampler2D _MainTex, _CelMap;
            float4 _MainTex_TexelSize;
            float4 _MainTex_ST, _LightPoint;
            float _Brightness;

            struct vertIn
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 normal : NORMAL;
            };

            struct vertOut
            {
                float2 uv : TEXCOORD0;
                float4 vertex : POSITION;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };



            vertOut vert (vertIn v)
            {
                fixed4 vertWorldPos = mul(unity_ObjectToWorld, v.vertex);

                vertOut o;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = vertWorldPos;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);

                return o;
            }

            fixed4 frag (vertOut i) : SV_Target
            {

                // sample the texture
                fixed3 lightDifference = i.worldPos - _LightPoint.xyz;
                fixed3 lightDirection = normalize(lightDifference);
                fixed4 Colour = fixed4(tex2D(_MainTex, i.uv));
                Colour *= _Brightness;
                
                float2 Tex = i.uv;
                Tex.y = 0.0f;
                Tex.x = 1 - saturate(dot(lightDirection, i.worldNormal));
                float4 CelColour = tex2D(_CelMap, Tex);
               

                return Colour + Colour * CelColour;
            }
            ENDCG
        }

        Pass
        {
			Cull Front   
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			float _OutlineSize;
			float4 _OutlineColor;

            struct vertIn
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 worldPos : POSITION;
            };

            struct vertOut
            {
                float4 vertex : POSITION;
            };
            				
			vertOut vert (vertIn i)
			{
                vertOut o;
                o.vertex = UnityObjectToClipPos(float4(i.vertex.xyz+i.normal*_OutlineSize,1));
				return o;
			}
            
			float4 frag (vertOut o) : COLOR
			{
				return float4(_OutlineColor.rgb,0);					
			}          
			ENDCG
        }
        
        
    }

}
