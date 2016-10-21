Shader "Custom/PhongShader" {
	Properties
	{
		_PointLightColor("Point Light Color", Color) = (0, 0, 0)
		_PointLightPosition("Point Light Position", Vector) = (0.0, 0.0, 0.0)
		_MainTex("_MainTex", 2D) = "white"{}
	}
		SubShader
	{
		Pass
	{
		CGPROGRAM

		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"

		uniform float3 _PointLightColor;
		uniform float3 _PointLightPosition;
		uniform sampler2D _MainTex;

	struct vertIn
	{
		float4 vertex : POSITION;
		float4 normal : NORMAL;
		float4 color : COLOR;
		float4 texcoord : TEXCOORD0;
	};

	struct vertOut
	{
		float4 vertex : SV_POSITION;
		float4 color : COLOR;
		float4 worldVertex : TEXCOORD1;
		float3 worldNormal : TEXCOORD2;
		float4 tex : TEXCOORD3;
	};

	// Implementation of the vertex shader
	vertOut vert(vertIn v)
	{
		vertOut o;

		// Convert Vertex position and corresponding normal into world coords
		// Note that we have to multiply the normal by the transposed inverse of the world 
		// transformation matrix (for cases where we have non-uniform scaling; we also don't
		// care about the "fourth" dimension, because translations don't affect the normal) 
		float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
		float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));

		// Transform vertex in world coordinates to camera coordinates, and pass colour
		o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
		o.color = v.color;

		// Pass out the world vertex position and world normal to be interpolated
		// in the fragment shader (and utilised)
		o.worldVertex = worldVertex;
		o.worldNormal = worldNormal;


		o.tex = v.texcoord;

		return o;
	}

	// Implementation of the fragment shader
	fixed4 frag(vertOut v) : COLOR
	{
		// Our interpolated normal might not be of length 1
		float3 interpNormal = normalize(v.worldNormal);

		float4 texColor = tex2D(_MainTex, v.tex.xy);

		// Calculate ambient RGB intensities
		float Ka = 1;
		//float3 amb = v.color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;
		float3 amb = texColor.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

		// Calculate diffuse RBG reflections, we save the results of L.N because we will use it again
		// (when calculating the reflected ray in our specular component)
		float fAtt = 1;
		float Kd = 1.2f;
		float3 L = normalize(_PointLightPosition - v.worldVertex.xyz);
		float LdotN = dot(L, interpNormal);
		float3 dif = fAtt * _PointLightColor.rgb * Kd * texColor.rgb * saturate(LdotN);

		// Calculate specular reflections
		float Ks = 1.0f;
		float specN; // Values>>1 give tighter highlights
		float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);

		// Using Blinn-Phong approximation:
		specN = 200.0f; // We usually need a higher specular power when using Blinn-Phong
		float3 H = normalize(V + L);
		float3 spe = fAtt * _PointLightColor.rgb * Ks * pow(saturate(dot(interpNormal, H)), specN);

		// Combine Phong illumination model components
		float4 returnColor = float4(0.0f, 0.0f, 0.0f, 0.0f);
		returnColor.rgb = amb.rgb + dif.rgb + spe.rgb;
		returnColor.a = texColor.a;

		return returnColor;
	}
		ENDCG
	}
	}
}
