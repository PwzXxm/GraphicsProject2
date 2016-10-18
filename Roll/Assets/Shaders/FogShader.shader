Shader "Custom/FogShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Intensity("Intensity", Range(0, 1)) = 0.5
		_Scale("Scale", Range(0, 5)) = 1.0
		_Pow("Pow", Range(0, 3)) = 0.5
		_Alpha("Alpha", Range(0, 3)) = 0.75
	}
	SubShader {
		Tags { "Queue"="Transparent" "IgnorePorjector"="True" "RenderType"="Transpartent" }
		LOD 200

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "AutoLight.cginc"
			#include "UnityCG.cginc"

			float4 _Color;
			sampler2D _MainTex;
			uniform float _Intensity;
			uniform float _Scale;
			uniform float _Pow;
			uniform float _Alpha;
			uniform float _Offset;

			struct vertIn
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
				float4 color : COLOR;
				float2 uv : TEXCOORD1;
			};

			struct vertOut
			{
				float4 pos : POSITION;
				float4 color : COLOR;
				float4 texcoord : TEXCOORD2;
				float3 normal : TEXCOORD3;
				float3 worldPos : TEXCOORD4;
				float2 uv : TEXCOORD5;
			};

			vertOut vert(vertIn v)
			{
				vertOut o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				o.normal = normalize(v.normal).xyz;
				o.texcoord = v.texcoord;
				o.color = v.color;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				return o;
			}

			fixed4 frag(vertOut v): COLOR
			{
				float3 view = normalize(_WorldSpaceCameraPos - v.worldPos);
				float4 o = tex2D(_MainTex, _Scale * v.uv);
				float alpha = o.a;
				o.rgb = pow(_Intensity*o.r, _Pow)*_Color.rgb;
				alpha *= (v.color.a - length(v.uv - float2(0.5, 0.5))*2)*_Alpha;
				o.a = alpha;

				return o;
			}

			ENDCG
			Lighting On
			Cull Off
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
		}
	}
}
