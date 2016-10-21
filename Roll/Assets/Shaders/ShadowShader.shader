Shader "Custom/ShadowShader" {
	Properties{
		_MainTex("_MainTex", 2D) = "white"{}
	}
	SubShader{
		Pass{

		Tags{ "LightMode" = "ForwardBase" }

		CGPROGRAM

#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fwdbase
#include "UnityCG.cginc"
#include "AutoLight.cginc"

		uniform sampler2D _MainTex;

		struct v2f
	{
		float4 pos : SV_POSITION;
		float4 tex : TEXCOORD0;

		LIGHTING_COORDS(1,2)
	};


	v2f vert(appdata_base v) {
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.tex = v.texcoord;

		TRANSFER_VERTEX_TO_FRAGMENT(o);

		return o;
	}

	fixed4 frag(v2f i) : COLOR{

		float attenuation = LIGHT_ATTENUATION(i);
	return tex2D(_MainTex, i.tex.xy)  * attenuation;
	}

		ENDCG
	}
	}
		Fallback "VertexLit"
}
