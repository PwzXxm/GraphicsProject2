Shader "Custom/ToonShader" {
	Properties {
		_MainTex("Main Texture", 2D) = "white"{}
		_Color("Diffuse Material Color", Color) = (1, 1, 1, 1)
		_UnlitColor("Unlit Color", Color) = (0.5, 0.5, 0.5, 1)
		_SpecularColor("Specular Material Color", Color) = (1, 1, 1, 1)
		_DiffuseThreshold("Lighting Threshold", Range(-1.1, 1)) = 0.1
		_OutlineThickness("Outline Thickness", Range(0, 1)) = 0.1
		_Shininess("Shininess", Range(0.5, 1)) = 1
	}
	SubShader {
			PASS {
		Tags { "LightMode" = "ForwardBase" }

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		uniform sampler2D _MainTex;
		uniform float4 _LightColor0;
		uniform float4 _MainTex_ST;

		uniform float4 _Color;
		uniform float4 _UnlitColor;
		uniform float4 _SpecularColor;
		uniform float _DiffuseThreshold;
		uniform float _Shininess;
		uniform float _OutlineThickness;

		struct vertIn
		{
			float4 vertex : POSITION;
			float4 texcoord : TEXCOORD0;
			float3 normal : NORMAL;
		};

		struct vertOut
		{
			float4 pos : SV_POSITION;
			float3 normalDir : TEXCOORD1;
			float4 lightDir : TEXCOORD2;
			float3 viewDir : TEXCOORDD3;
			float2 uv : TEXCOORD0;
		};

		vertOut vert(vertIn i)
		{
			vertOut o;
			float4 pos = mul(unity_ObjectToWorld, i.vertex);
			o.viewDir = normalize(_WorldSpaceCameraPos.xyz - pos.xyz);
			o.normalDir = normalize(mul(float4(i.normal, 0.0), unity_WorldToObject).xyz);

			float3 fragToLight = (_WorldSpaceCameraPos.xyz - pos.xyz);
			float3 normalFrag = (normalize(lerp(_WorldSpaceLightPos0.xyz, fragToLight, _WorldSpaceLightPos0.w)));
			o.lightDir = float4(normalFrag, lerp(1.0, 1.0 / length(fragToLight), _WorldSpaceLightPos0.w));
			o.pos = mul(UNITY_MATRIX_MVP, i.vertex);
			o.uv = i.texcoord;
			return o;
		}

		float4 frag(vertOut i) : COLOR{
			float nDotL = saturate(dot(i.normalDir, i.lightDir.xyz));
			float diffuseCut = saturate((max(_DiffuseThreshold, nDotL) - _DiffuseThreshold) * 1000);
			float specularCut = saturate(max(_Shininess, dot(reflect(-i.lightDir.xyz, i.normalDir), i.viewDir)) - _Shininess) * 1000;
			float outlineStr = saturate((dot(i.normalDir, i.viewDir) - _OutlineThickness) * 1000);

			float3 ambientL = (1 - diffuseCut) * _UnlitColor.xyz;
			float3 diffuseL = (1 - specularCut) * _Color.xyz*diffuseCut;
			float3 specularL = _SpecularColor.xyz * specularCut;

			float4 outColor = float4((ambientL + diffuseL) * outlineStr + specularL, 1.0);

			return outColor + tex2D(_MainTex, i.uv);
		}

		ENDCG
			}

	}
}