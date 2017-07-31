// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Learn/VertexColor" {
	Properties {
	}

	SubShader {
		Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		struct vertInput {
			float4 position : POSITION;
			float4 color : COLOR;
		};

		struct vertOutput {
			float4 position : SV_POSITION;
			float4 color : COLOR;
		};

		vertOutput vert (vertInput input) {
			vertOutput o;
			o.position = UnityObjectToClipPos(input.position);
			o.color = input.color;
			return o;
		}

		float4 frag (vertOutput input) : COLOR {
			return input.color;
		}
		ENDCG

		}
	}
}