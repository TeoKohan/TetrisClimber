Shader "Block/DefaulBlockShader" {
	Properties {
		_Health ("Health", Range(0.0,1.0)) = 1.0
		_Color ("Color", Color) = (1,1,1,1)
		_CrackTex ("Crack Texture (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
        _RimPower ("Rim Power", Range(0.25,10.0)) = 3.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _CrackTex;

		struct Input {
			float2 uv_CrackTex;
			float3 viewDir;
		};

		half _Health;
		half _Glossiness;
		half _Metallic;
		half _RimPower;
		fixed4 _Color;
		fixed4 _RimColor;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void vert (inout appdata_full v) {
		  v.vertex.xyz += clamp(( 1 - _Health) - 0.8, 0.0, 1.0) * 0.1 * sin(_Time[1] * (1 /   clamp((pow(_Health, 2) * 5), 0.025, 1.0)   ));
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 col = _Color;
			fixed4 tex = tex2D (_CrackTex, IN.uv_CrackTex / (_Health * 4 + 1));

			fixed4 c = col * ((1 - _Health) * tex + _Health * float4(1.0, 1.0, 1.0, 1.0));
			 c = col * (clamp(0.5 - _Health, 0.0, 1.0) * tex * 2 + clamp(_Health + 0.5, 0.0, 1.0) * float4(1.0, 1.0, 1.0, 1.0));
			
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;

			half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
			o.Emission = _RimColor.rgb * pow (rim, _RimPower);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
