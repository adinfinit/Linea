Shader "Hidden/LineEffect" {
Properties{
	_MainTex("Base (RGB)", 2D) = "white" {}
	_Iterations("Iterations", Range(1, 5)) = 1
	_LineWidth("LineWidth", Range(0, 1)) = 1
}
SubShader{
	Tags { "RenderType" = "Opaque" }
	LOD 200

	Pass {
		CGPROGRAM
		#pragma vertex vert_img
		#pragma fragment frag

		#include "UnityCG.cginc"

		uniform sampler2D _MainTex;
		uniform float4    _MainTex_TexelSize;

		uniform float _LineWidth;
		uniform int _Iterations;

		float sampleCros(sampler2D tex, float2 uv, float2 q) {
			float s = 0.0f;
			s += tex2D(tex, uv + float2(q.x, 0)).a;
			s += tex2D(tex, uv + float2(-q.x, 0)).a;
			s += tex2D(tex, uv + float2(0, q.y)).a;
			s += tex2D(tex, uv + float2(0, -q.y)).a;
			return s;
		}

		float sampleDiag(sampler2D tex, float2 uv, float2 q) {
			float s = 0.0f;
			s += tex2D(tex, uv + float2(-q.x, q.y)).a;
			s += tex2D(tex, uv + float2(-q.x, -q.y)).a;
			s += tex2D(tex, uv + float2(q.x, -q.y)).a;
			s += tex2D(tex, uv + float2(q.x, q.y)).a;
			return s;
		}

		float detectEdge(sampler2D tex, float4 texSize, float2 uv) {
			float4 x = tex2D(tex, uv);

			// always display red colors
			float reddish = length(x - float4(1.0, 0.0, 0.0, 1.0));
			if(reddish < 0.1f) {
				return 1.0f - reddish / 0.1f;
			}

			float greenish = length(x - float4(0.0, 1.0, 0.0, 1.0));
			if(greenish < 0.1f) {
				return 0.0f;
			}

			// make aliased lines and background transparent
			const float MIN = 0.9f;
			if(x.a <= MIN){
				return x.a;
			}

			float  s = x.a;
			float2 q = texSize.xy;

			// sample box at 1px dist
			s += sampleCros(tex, uv, q);
			s += sampleDiag(tex, uv, q);

			float MAX = 1.0f + 2.0f * 4.0f;

			// sample circles with 16 points
			[unroll(5)] for(int i = 1; i <= _Iterations; i++){
				float2 q = texSize.xy * (i + 1);
				s += sampleCros(tex, uv, q);
				s += sampleDiag(tex, uv, q*float2(0.7071f, 0.7071f));
				s += sampleDiag(tex, uv, q*float2(0.9238f, 0.3827f));
				s += sampleDiag(tex, uv, q*float2(0.3827f, 0.9238f));
				MAX += 16.0f;
			}

			float ALIAS = 4.0f / MAX;
			s /= MAX;

			// are we outside a shape
			if(x.a < 0.7f){
				// alias outer edge
				return s / ALIAS;
			}

			// inside the shapes
			if (s < _LineWidth - ALIAS) {
				return 1.0f;
			}
			// alias inner edge
			if (s < _LineWidth) {
				float distanceToEdge = (_LineWidth - s) / ALIAS;
				return clamp(distanceToEdge, 0.0f, x.a);
			}

			// make insides transparent
			return 0.0f;
		}

		float4 frag(v2f_img i) : COLOR{
			float r = detectEdge(_MainTex, _MainTex_TexelSize, i.uv);
			float4 x = tex2D(_MainTex, i.uv);

			return float4(r,r,r, x.a);
		}
		ENDCG
	}
}
}