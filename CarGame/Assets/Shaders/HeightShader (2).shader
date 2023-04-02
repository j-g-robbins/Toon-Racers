Shader "Unlit/HeightShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LightPos ("Light Pos", vector) = (0,0,0, 0)
        _LightColour ("Light Colour", COLOR) = (1,1,1,1)
        _C1 ("Colour 1", COLOR) = (0,0,0,1)
        _C2 ("Colour 2", COLOR) = (0,0,0,1)
        _C3 ("Colour 3", COLOR) = (0,0,0,1)
        _C4 ("Colour 4", COLOR) = (0,0,0,1)
        _H1 ("Height 1", float) = 0
        _H2 ("Height 2", float) = 0
        _H3 ("Height 3", float) = 0
        _MixDist ("Colour Mix", float) = 1
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

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _LightPos, _LightColour;
            float4 _C1, _C2, _C3, _C4;
            float _H1, _H2, _H3, _MixDist;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col;
                float distance;
                float height = i.worldPos.y;
                if (height < _H1) {
                    distance = abs(height - _H1);
                    if (distance < _MixDist) {
                        col = (distance / _MixDist) * _C1 + (1 - (distance / _MixDist)) * _C2;
                    }
                    else {
                        col = _C1;
                    }                   
                }
                else if (height < _H2) {
                    distance = abs(height - _H2);
                    if (distance < _MixDist) {
                        col = (distance / _MixDist) * _C2 + (1 - (distance / _MixDist)) * _C3;
                    }
                    else {
                        col = _C2;
                    }         
                }
                else if (height < _H3) {
                    distance = abs(height - _H3);
                    if (distance < _MixDist) {
                        col = (distance / _MixDist) * _C3 + (1 - (distance / _MixDist)) * _C4;
                    }
                    else {
                        col = _C3;
                    }         
                }
                else {
                    col = _C4;
                }

                fixed3 lightDifference = i.worldPos - _LightPos.xyz;
                fixed3 lightDirection = normalize(lightDifference);
                fixed intensity = -0.5 * dot(lightDirection, i.worldNormal) + 0.5;
                col = intensity * col * _LightColour;

                return col;
            }
            ENDCG
        }
    }
}
