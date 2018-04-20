Shader "Hidden/ColorizeEffect" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_LineColor("Line Color", Color) = (0, 0, 0, 1)
	_BackgroundColor("Background Color", Color) = (0.5, 0.5, 0.5, 1)
	_BackgroundTex("Background Texture", 2D) = "white" {}
	_ParallaxSpeed("Background Scrolling Speed", Float) = 1.0
}
SubShader {
	Pass {
		CGPROGRAM
		#pragma vertex vert_img
		#pragma fragment frag

		#include "UnityCG.cginc"

		uniform sampler2D _MainTex;
		uniform float4    _MainTex_TexelSize;

		uniform float4 _BackgroundColor;
		uniform sampler2D _BackgroundTex;
		uniform float4 _LineColor;
		uniform float _ParallaxSpeed;
		
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

		float filling(sampler2D tex, float4 texSize, float2 uv) {
			float4 x = tex2D(tex, uv);
			float s = 0.0f;
			s += sampleCros(tex, uv, texSize);
			s += sampleDiag(tex, uv, texSize) * 0.7071f;
			s /= 4.0f + 4.0f * 0.7071f;
			
			// fill stray empty pixels
			if(x.a < 0.7f && s > 0.5f){
				if(s > 0.6f) {
					return 1.0f;
				}
				return max(x.a, (s - 0.5f) / 0.1f);
			}

			return x.a;
		}

		float4 frag(v2f_img i) : COLOR {
			float fill = filling(_MainTex, _MainTex_TexelSize, i.uv);
			
			// float4 x = tex2D(_MainTex, i.uv);
			// x.a = 1.0f;
			// return lerp(_BackgroundColor, x, fill);
			return lerp(_BackgroundColor, _LineColor, fill) * tex2D(_BackgroundTex, (_WorldSpaceCameraPos.xy * _ParallaxSpeed + i.pos) * 0.003);
		}
		ENDCG
	}
}
}
