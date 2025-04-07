Shader "Custom/CircleMaskShader"
{
    Properties
    {
        _MainTex("Base (RGB), Alpha (A)", 2D) = "white" {}
        _Center("Mask Center", Vector) = (0.5, 0.5, 0, 0) // 以纹理坐标为基础
        _Radius("Radius", Range(0, 0.5)) = 0.5 // 范围0到0.5
    }

        SubShader
        {
            Tags { "RenderType" = "Transparent" }
            LOD 100

            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            ZTest LEqual

            Pass
            {
                HLSLPROGRAM
                #pragma target 3.0
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex : SV_POSITION;
                    float2 texcoord : TEXCOORD0;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float2 _Center; // 中心
                float _Radius;  // 半径

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // 获取纹理颜色
                    fixed4 col = tex2D(_MainTex, i.texcoord);

                // 计算当前像素与中心的距离
                float distanceToCenter = distance(i.texcoord, _Center);

                // 如果超出半径则设置为透明
                if (distanceToCenter > _Radius)
                {
                    col.a = 0; // 透明
                }

                return col;
            }
            ENDHLSL
        }
        }
            FallBack "Diffuse"
}
