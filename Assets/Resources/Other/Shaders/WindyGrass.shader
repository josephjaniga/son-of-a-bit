// Upgrade NOTE: replaced 'glstate.matrix.mvp' with 'UNITY_MATRIX_MVP'

Shader "Nature/Windy Grass" {
	Properties {
		_Color ("Main Color", Color) = (.5, .5, .5, 0)
		_Color2 ("Fade Color", Color) = (1, .9, .8, 0)
		_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
		_Cutoff ("Base Alpha cutoff", Range (0,1)) = .5
	}
	SubShader {
		Tags { "Queue" = "Transparent" }
		BindChannels {
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}

		Pass {
			Lighting On
CGPROGRAM
// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it uses non-square matrices
#pragma exclude_renderers gles
// vertex Vertex

#include "waves.cginc"		// Get in the helper wave functions
#include "UnityCG.cginc"		// Get standard Unity constants

struct Appdata {
	float4 vertex;
	float3 normal; 
	float4 texcoord;
	float4 color;
};

struct v2f {
	float4 pos : POSITION;
	float4 color : COLOR0;
	float4 uv : TEXCOORD0;
	float fog : FOGC; 
}; 

uniform float4 _Color, _Color2, _SkyLight;

v2f Vertex(Appdata v) {
	v2f o;

	const float4 _waveXSize = float4(0.012, 0.02, -0.06, 0.048) * 2;
	const float4 _waveZSize = float4 (0.006, .02, -0.02, 0.1) * 2;
	const float4 waveSpeed = float4 (0.3, .3, .08, .07) * 4;

	float4 _waveXmove = _waveXSize * waveSpeed * 25;
	float4 _waveZmove = _waveZSize * waveSpeed * 25;
	
	// We model the wind as basic waves...

	// Calculate the wind input to leaves from their vertex positions....
	// for now, we transform them into world-space x/z positions... 
	// Later on, we should actually be able to do the whole calc's in post-projective space
	float3 worldPos = mul ((float3x4)_Object2World, v.vertex);
	
	// This is the input to the sinusiodal warp
	float4 waves;
	waves = worldPos.x * _waveXSize;
	waves += worldPos.z * _waveZSize;

	// Add in time to model them over time
	waves += _Time.x * waveSpeed;

	float4 s, c;
	waves = frac (waves);
	FastSinCos (waves, s,c);

	float waveAmount = v.texcoord.y;
	s *= waveAmount;

	// Faste winds move the grass more than slow winds
	s *= normalize (waveSpeed);

	s = s * s;
	float fade = dot (s, 1.3);
	s = s * s;
	float3 waveMove = float3 (0,0,0);
	waveMove.x = dot (s, _waveXmove);
	waveMove.z = dot (s, _waveZmove);
	
	v.vertex.xz -= mul ((float3x3)_World2Object, waveMove).xz;

	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	o.fog = o.pos.w;
	o.uv = v.texcoord;
	o.color = lerp (_Color, _Color2, fade.xxxx);
//	o.color *= _SkyLight;
	return o;
}
ENDCG
			AlphaTest Greater [_Cutoff]
			Cull Off
			SetTexture [_MainTex] { combine texture * primary double, texture}
			
		}
	}	
	SubShader {	
		Tags { "Queue" = "Transparent" }
		Pass {
			Blend SrcAlpha oneMinusSrcAlpha
			AlphaTest Greater [_Cutoff]
			Cull Off
			color [_Color]
			ZTest Less
			SetTexture [_MainTex] { combine texture * primary DOUBLE, texture}
		}

	}
}