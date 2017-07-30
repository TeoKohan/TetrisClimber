// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/Bounce" {
	Properties {
		_Amount ("Extrusion Amount", Range(-1,1)) = 0.5
		_Color ("Color", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
        _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
		_PlayerFeet ("Player", Vector) = (0, 0, 0, 0)
		}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		half _RimPower;
		fixed4 _Color;
		fixed4 _RimColor;
		float _Amount;
		float3 _PlayerFeet;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END
	
		void vert (inout appdata_full v) {
		  float4 world_space_vertex = mul( unity_ObjectToWorld, v.vertex );
          world_space_vertex.xyz += v.normal * _Amount * sin(world_space_vertex.x + world_space_vertex.y + world_space_vertex.z) * _SinTime[3] * clamp(distance(world_space_vertex.xyz, _PlayerFeet.xyz), 0, 1) - clamp((5-distance(world_space_vertex.xyz, _PlayerFeet.xyz)), 0, 1) *  v.normal * _Amount * 0.5;
		  v.vertex = mul( unity_WorldToObject, world_space_vertex );
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			o.Albedo = _Color.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = _Color.a;

			half rim = (1.0 - saturate(dot (normalize(IN.viewDir), o.Normal))) / 10;
            o.Emission = _RimColor.rgb * pow (rim, _RimPower);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
