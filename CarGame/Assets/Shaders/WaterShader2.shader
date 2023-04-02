Shader "Unlit/WaterShader2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LightPoint ("PointLightPos", Vector) = (0, 0, 0)

        _Scale ("Scale", float) = 1
        _Speed ("Speed", float) = 1
        _Frequency ("Frequency", float) = 1

        _Speed2 ("Tex Speed", vector) = (1,1,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct vertIn
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct vertOut
            {
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldPosition : TEXCOORD2;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                
            };

            sampler2D _MainTex;
            float4 _MainTex_ST, _LightPoint, _Speed2;

            float _Scale, _Speed, _Frequency;

            vertOut vert(vertIn v)
            {
                fixed4 vertWorldPos = mul(unity_ObjectToWorld, v.vertex);
                half offsetvert = 1.5f * vertWorldPos.x * vertWorldPos.x + 0.7f * vertWorldPos.y * vertWorldPos.y;
                half value = _Scale * sin(_Time.x * _Speed + offsetvert * _Frequency);
                v.vertex.y += value;

                vertOut o;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPosition = vertWorldPos;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                o.uv.x += _Speed2.x * _Time.x;
                o.uv.y += _Speed2.y * _Time.x;

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (vertOut i) : SV_Target
            {
                // sample the texture
                fixed3 lightDifference = i.worldPosition - _LightPoint.xyz;
                fixed3 lightDirection = normalize(lightDifference);
                fixed intensity = -0.5 * dot(lightDirection, i.worldNormal) + 0.5;
                fixed4 col = fixed4(intensity * tex2D(_MainTex, i.uv));
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }

            
            ENDCG
        }

    }
}
