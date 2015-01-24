Shader "Custom/EchoesShader" {
	Properties {
		_MainTex ("Base (RGBA)", 2D) = "white" {}
		_EchoTex ("Echo (RGBA", 2D) = "white" {}
		_MainColor("Main Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_MaxRadius("Max Radius", float) = 1.0
		_MaxFade("MxaFade", float) = 1.0

		_Position0("Position0", Vector) = (0.0, 0.0, 0.0, 0.0)
		_Position1("Position1", Vector) = (0.0, 0.0, 0.0, 0.0)
		_Position2("Position2", Vector) = (0.0, 0.0, 0.0, 0.0)

		_Radius0("Radius0", float) = 0.0
		_Radius1("Radius1", float) = 0.0
		_Radius2("Radius2", float) = 0.0

		_Fade0("Fade0", float) = 0.0
		_Fade1("Fade1", float) = 0.0
		_Fade2("Fade2", float) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Geometry" }
		LOD 200
		
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf NoLighting
		#include "UnityCG.cginc"

		struct Input{
			float2 uv_MainTex;
			float3 worldPos;
		};

		sampler2D _MainTex;
		sampler2D _EchoTex;

		float4 _MainColor;
		float _DistanceFade;
		float _MaxRadius;
		float _MaxFade;

		float3 _Position0;
		float3 _Position1;
		float3 _Position2;

		float _Radius0;
		float _Radius1;
		float _Radius2;

		float _Fade0;
		float _Fade1;
		float _Fade2;

		half4 LightingNoLighting (SurfaceOutput s, half3 lightDir, half atten)
		{
			half4 c;
			c.rgb = s.Albedo;
			c.a = s.Alpha;
			return c;
		}

		float ApplyFade(Input IN, float3 position, float radius, float infade)
		{
			float size = 128.0;

			float dist = distance(IN.worldPos, position);

			if (radius >= 3 * _MaxRadius || dist >= radius)
				return 0.0;
			else
			{
				float c1 = (_DistanceFade >= 1.0) ? dist / radius : 1.0;

				c1 *= (infade <= _MaxFade) ? 1.0 - infade / _MaxFade : 0.0;
				c1 = (infade<=0) ? 1.0 : c1;
				return c1;
			}
		}

		void surf (Input IN, inout SurfaceOutput o) {
			float c1 = 0.0;

			c1 += ApplyFade(IN, _Position0, _Radius0, _Fade0);
			c1 += ApplyFade(IN, _Position1, _Radius1, _Fade1);
			c1 += ApplyFade(IN, _Position2, _Radius2, _Fade2);
			c1 /= 3.0;

			float c2 = 1.0 - c1;
			o.Albedo = _MainColor.rgb * c2 + tex2D(_MainTex, IN.uv_MainTex).rgb * c1;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
