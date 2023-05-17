Shader "CustomShaders/Health Bar" {
    Properties {
        [NoScaleOffset]_MainTex ("Texture", 2D) = "white" {}
        _Health ("Health", Range(0,1)) = 1
        _SegmentSize ("Segment Size", Range(1,30)) = 8
        _BorderSize ("Border Size", Range(0,0.5)) = 0.5
        _Round1 ("Round Edge 1", Range(0,0.5)) = 0.5
        _Round2 ("Round Edge 2", Range(0,0.5)) = 0.5
    }
    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }

        Pass {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            #define TAU 6.28318530718
            
            struct Mesh {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            //Variables
            sampler2D _MainTex;
            float _Health;
            float _SegmentSize;
            float _BorderSize;
            float _Round1;
            float _Round2;

            Interpolators vert (Mesh v) {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            float InverseLerp(float a, float b, float v){
                return (v - a) / (b - a);
            }
            #define Clamp01
            float4 frag (Interpolators i) : SV_Target {
                //Round Edge
                float2 coords = i.uv;
                coords.x *= _SegmentSize;
                float2 pointOnLine = float2(clamp(coords.x, _Round1, _SegmentSize -_Round2), 0.5);
                float sdf = distance(coords, pointOnLine) * 2 - 1; //Signed Distance Field
                clip(-sdf);
                //Border
                float borderSdf = sdf + _BorderSize;
                float pd = fwidth(borderSdf);
                float borderMask = 1 - saturate(borderSdf / pd);
                //Health Bar Color
                float healthBarMask = _Health > i.uv.x;
	            float3 healthBarColor = tex2D(_MainTex, float2(_Health, i.uv.y)); //color from tex vertical
                //Flash Health
                if (_Health < 0.2){ 
                    float flash = cos(_Time.y * 4) * 0.3 + 1;
                    healthBarColor *= flash; 
                };
	            return float4(healthBarColor * healthBarMask * borderMask, 1);
            }
            ENDCG
        }
    }
}
