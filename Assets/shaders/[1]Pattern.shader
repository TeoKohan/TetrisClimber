// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Learn/[1]Pattern" {
	Properties {
	_MainTex ("Main Texture(RGB) Transp(A)", 2D) = "White"{}

	_OnColour ("On Color (RGBA)", Color) = (1,1,1,1)
	_OffColour ("Off Color (RGBA)", Color) = (1,1,1,1)

	_Speed("Speed", Range(-10,10)) = 1
	_WaveLength("Wavelenght ", Range(0,50)) = 1

	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha

		Fog { Mode Off }

		Pass {

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		uniform half4 _OnColour;
		uniform half4 _OffColour;
		uniform half _Speed;
		uniform half _WaveLength;

		struct vertOutput {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD;
		float3 wPos : TEXCOORD1;
		};

		vertOutput vert (appdata_full i) {
			vertOutput o;
			o.pos = UnityObjectToClipPos(i.vertex);
			o.uv = i.texcoord;
			o.wPos = mul(unity_ObjectToWorld, i.vertex).xyz;
			return o;
		}

		//float4 frag(v2f_img i) : COLOR
		half4 frag(vertOutput o) : COLOR {
				half4 mainColour = tex2D(_MainTex, o.uv);
				half s = sin((o.wPos.y + _Time * _Speed) * _WaveLength);
				half c = (cos(s + 3.14 / 2) + 2) / 2;
				s = (s + 2) / 2;
				half4 oncolour = half4(_OnColour * s);
				half4 offcolour = half4(_OffColour * c);
				return half4 ((oncolour * s + offcolour * c).rgb * mainColour.rgb, 1);
			}
			ENDCG

		}
	}
}