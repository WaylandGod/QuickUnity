Shader "QuickUnity/FX/SimpleOcean" {
	Properties {
		_SurfaceColor ("Surface Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_WaterColor ("Water Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Refraction ("Refraction (RGB)", 2D) = "white" {}
		_Reflection ("Reflection (RGB)", 2D) = "white" {}
		_Bump ("Bump (RGB)", 2D) = "bump" {}
		_Foam ("Foam (RGB)", 2D) = "white" {}
		_MaterialSize ("Material Size", Vector) = (1.0, 1.0, 1.0, 1.0)
		_LightDirection ("Light Direction", Vector) = (0.3, -0.6, -1.0, 0.0)
	}
	SubShader {
		Pass {
			CGPROGRAM
			#pragma exclude_renderers xbox360
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct v2f {
				float4 pos : SV_POSITION;
				float4 projTexCoord : TEXCOORD0;
				float2 bumpTexCoord : TEXCOORD1;
				float3 viewDir : TEXCOORD2;
				float3 normalTexCoord : TEXCOORD3;
				float3 lightDir : TEXCOORD4;
				float2 foamStrenthAndDistance : TEXCOORD5;
			};

			float4 _MaterialSize;
			float4 _LightDirection;

			v2f vert (appdata_tan v) {
				v2f o;
				o.bumpTexCoord.xy = v.vertex.xz / float2 (_MaterialSize.x, _MaterialSize.z) * 10.0;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);

				return o;
			}

			half4 frag (v2f i) : COLOR {
				half4 o = half4 (0.0, 0.0, 0.0, 1.0);
				return o;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
