Shader "Hidden/NightVisionShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (0, 1, 0, 1)
		_Range("Range", Float) = .01
		_ColorMultiplier ("ColorMultiplier", Float) = 0
		_Enabled ("Enabled", Float) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float2 uv_depth: TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
				o.uv_depth = v.uv;
                return o;
            }

			sampler2D_float _CameraDepthTexture;
            sampler2D _MainTex;
			float _Range;
			float _ColorMultiplier;
			float _Enabled;
			fixed4 _Color;

			fixed lum(fixed3 col) {
				return 0.299 * col.r + 0.587 * col.g + 0.144 * col.b;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
			if (_Enabled == 1)
			{
				float depth = DecodeFloatRG(tex2D(_CameraDepthTexture, i.uv));
				float linearDepth = Linear01Depth(depth);
				linearDepth = max(0, (_Range - linearDepth) / _Range);

				col = fixed4(lum(col), lum(col), lum(col), 1) * _ColorMultiplier + linearDepth;
				return _Color * col;
			}
			else
				return col;
            }
            ENDCG
        }
    }
}
