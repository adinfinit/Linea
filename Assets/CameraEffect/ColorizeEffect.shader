#warning Upgrade NOTE: unity_Scale shader variable was removed; replaced '_WorldSpaceCameraPos.w' with '1.0'

Shader "Hidden/ColorizeEffect" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_LineColor("Line Color", Color) = (0, 0, 0, 1)
	_BackgroundColor("Background Color", Color) = (0.5, 0.5, 0.5, 1)
	_BackgroundTex("Background Texture", 2D) = "white" {}
	_ScreenSize("Screen Size", Vector) = (0, 0, 0, 0)
	_DisplacementMultiplier("Displacement multiplier", Float) = 0.001
	_TexSizeMult("Texture Size Multiplier", Float) = 10.0
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
		uniform float2 _ScreenSize;
		uniform float _DisplacementMultiplier;
		uniform float _TexSizeMult;
		
		float random(float3 co)
		{
			return frac(sin( dot(co.xyz ,float3(12.9898,78.233,45.5432) )) * 43758.5453);
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
			float tick = int(_Time.x*30.0f);
			float2 worldPos = _WorldSpaceCameraPos + i.uv * _ScreenSize;
			float2 displacement = float2(random(float3(worldPos, tick)), random(float3(worldPos, tick)));
			float fill = filling(_MainTex, _MainTex_TexelSize, i.uv + displacement * _DisplacementMultiplier);
			
			// float4 x = tex2D(_MainTex, i.uv);
			// x.a = 1.0f;
			// return lerp(_BackgroundColor, x, fill);
			// Overlay blend
			float4 a = tex2D(_BackgroundTex, worldPos / _TexSizeMult).g;
			float4 b = lerp(_BackgroundColor, _LineColor, fill);
			float comp = step(0.5f, a);
			return comp*2*a*b+(1-comp)*(1-2*(1-a)*(1-b));
			//return float4(worldPos, 0, 1.0);
		}
		ENDCG
	}
}
}
