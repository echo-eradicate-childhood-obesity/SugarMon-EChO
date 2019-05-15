Shader "Unlit/ScannerLineShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BarSpeed("Scan Bar Speed",Range(10.0,30.0))=20.0
		_Color("Scanner Line Color",Color) = (1.0,1.0,1.0,1.0)
		_Alpha("Scanner Line Alpha",Range(0,1))=0.5
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent"}
		LOD 100
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			// make fog work
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float _BarSpeed;
			float _Alpha;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				o.vertex.y += lerp(0.5, 0, (_Time * _BarSpeed) % 2);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				col.rgb = _Color.rgb;
				col.a -= _Alpha;
				return col;
			}
			ENDCG
		}
	}
}
