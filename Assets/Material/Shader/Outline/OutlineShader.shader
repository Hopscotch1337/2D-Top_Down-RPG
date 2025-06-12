Shader "Custom/SpriteOutline"
{
    Properties
    {
        [PerRendererData]_MainTex ("Sprite Texture", 2D) = "white" {}
        [PerRendererData]_Color   ("Tint Color",       Color) = (1,1,1,1)
        _OutlineColor              ("Outline Color",    Color) = (0,0,0,1)
        _OutlineThickness          ("Thickness",     Range(0,1)) = 0.05
    }
    SubShader
    {
        Tags
        {
            "Queue"           = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType"      = "Transparent"
            "PreviewType"     = "Plane"
            "CanUseSpriteAtlas" = "True"
        }
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // PerRendererData sorgt dafür, dass der SpriteRenderer _MainTex und _Color
            // bei jedem Sprite-Draw mit den korrekten Werten füllt.
            sampler2D _MainTex;
            float4   _MainTex_TexelSize;
            fixed4   _Color;

            fixed4 _OutlineColor;
            float  _OutlineThickness;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                float4 color  : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv     : TEXCOORD0;
                fixed4 color  : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv     = v.uv;
                o.color  = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Sample Sprite
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                if (col.a > 0.01)
                    return col;

                // ansonsten suche Rand-Alpha
                float2 off = _OutlineThickness * _MainTex_TexelSize.xy;
                float sum = 
                    tex2D(_MainTex, i.uv + float2(-off.x,  0)).a +
                    tex2D(_MainTex, i.uv + float2( off.x,  0)).a +
                    tex2D(_MainTex, i.uv + float2( 0, -off.y)).a +
                    tex2D(_MainTex, i.uv + float2( 0,  off.y)).a;

                if (sum > 0)
                    return _OutlineColor;

                return 0;
            }
            ENDCG
        }
    }
}