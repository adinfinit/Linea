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
		
		float random (in float2 _st) {
			return sin(dot(_st.xy,
								float2(12.9898,78.233))) % 1.0f *
				43758.5453123;
		}

		// Based on Morgan McGuire @morgan3d
		// https://www.shadertoy.com/view/4dS3Wd
		float noise (in float2 _st) {
			float2 i = floor(_st);
			float2 f = _st % 1.0f;

			// Four corners in 2D of a tile
			float a = random(i);
			float b = random(i + float2(1.0, 0.0));
			float c = random(i + float2(0.0, 1.0));
			float d = random(i + float2(1.0, 1.0));

			float2 u = f * f * (3.0 - 2.0 * f);

			return lerp(a, b, u.x) +
					(c - a)* u.y * (1.0 - u.x) +
					(d - b) * u.x * u.y;
		}

		#define NUM_OCTAVES 1

		float fbm ( in float2 _st) {
			float v = 0.0;
			float a = 0.5;
			float2 shift = float2(100.0, 100.0);
			// Rotate to reduce axial bias
			float2x2 rot = float2x2(cos(0.5), sin(0.5),
							-sin(0.5), cos(0.50));
			for (int i = 0; i < NUM_OCTAVES; ++i) {
				v += a * noise(_st);
				_st = mul(rot, _st) * 2.0f + shift;
				a *= 0.5;
			}
			return v;
		}

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
			float distort = fbm(random(i.uv) * random(_Time.x) * 1000) * 0.00000004;
			float fill = filling(_MainTex, _MainTex_TexelSize, i.uv + distort);
			
			// float4 x = tex2D(_MainTex, i.uv);
			// x.a = 1.0f;
			// return lerp(_BackgroundColor, x, fill);
			float2 paperPosition = (_WorldSpaceCameraPos.xy * _ParallaxSpeed + i.pos) * 0.003;
			float4 paperTexture = tex2D(_BackgroundTex, paperPosition);
			return lerp(_BackgroundColor, _LineColor, fill) * paperTexture;
		}
		ENDCG
	}
}
}
