Shader "Mobile/QuickUnity/Simple Ocean" {
	Properties {
		_SurfaceColor ("Surface Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_WaterColor ("Water Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Refraction ("Refraction (RGB)", 2D) = "white" {}
		_Reflection ("Reflection (RGB)", 2D) = "white" {}
		_Bump ("Bump (RGB)", 2D) = "bump" {}
		_Foam ("Foam (RGB)", 2D) = "white" {}
		_MaterialSize ("Material Size", Vector) = (1.0, 1.0, 1.0, 1.0)
		_LightDir ("Light Direction", Vector) = (0.3, -0.6, -1.0, 0.0)
		_LightIntensity ("Light Intensity", float) = 100.0
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
				float3 objSpaceNormal : TEXCOORD3;
				float3 lightDir : TEXCOORD4;
				float2 foamStrengthAndDistance : TEXCOORD5;
			};

			float4 _MaterialSize;
			float4 _LightDir;

			v2f vert(appdata_tan v) {
				v2f o;
				o.bumpTexCoord.xy = v.vertex.xz / float2(_MaterialSize.x, _MaterialSize.z) * 10.0;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

				// Calculate the strength and distance of foam.
				o.foamStrengthAndDistance.x = v.tangent.w;
				o.foamStrengthAndDistance.y = clamp(o.pos.z, 0.0, 1.0);

				// Calculate the projection texture coord.
				float4 projSource = float4(v.vertex.x, 0.0, v.vertex.z, 1.0);
				o.projTexCoord = mul(UNITY_MATRIX_MVP, projSource);

				// Calculate the direction of view and light.
				float3 objSpaceViewDir = ObjSpaceViewDir(v.vertex);
				float3 binormal = cross(normalize(v.normal), normalize(v.tangent.xyz));
				float3x3 rotation = float3x3(v.tangent.xyz, binormal, v.normal);

				o.objSpaceNormal = v.normal;
				o.viewDir = mul(rotation, objSpaceViewDir);
				o.lightDir = mul(rotation, float3(_LightDir.xyz));

				return o;
			}

			sampler2D _Refraction;
			sampler2D _Reflection;
			sampler2D _Bump;
			sampler2D _Foam;
			half4 _SurfaceColor;
			half4 _WaterColor;
			float _LightIntensity;

			half4 frag(v2f i) : COLOR {
				half4 o = half4(0.0, 0.0, 0.0, 1.0);

				half3 normalViewDir = normalize(i.viewDir);
				half4 buv = half4(i.bumpTexCoord.x + _Time.x * 0.03, i.bumpTexCoord.y + _SinTime.x * 0.2, i.bumpTexCoord.x + _Time.y * 0.04, i.bumpTexCoord.y + _SinTime.y * 0.5);

				half3 tangentNormal0 = tex2D(_Bump, buv.xy).rgb * 2.0 - 1.0;
				half3 tangentNormal1 = tex2D(_Bump, buv.zw).rgb * 2.0 - 1.0;
				half3 tangentNormal = normalize(tangentNormal0 + tangentNormal1);

				float2 projTexCoord = 0.5 * i.projTexCoord.xy * float2(1, _ProjectionParams.x) / i.projTexCoord.w + float2(0.5, 0.5);
				float2 bumpSampleOffset = i.objSpaceNormal.xz * 0.05 + tangentNormal.xy * 0.05;

				// Calculate refraction and reflection colors
				half3 reflection = tex2D(_Reflection, projTexCoord.xy + bumpSampleOffset).rgb * _SurfaceColor.rgb;
				half3 refraction = tex2D(_Refraction, projTexCoord.xy + bumpSampleOffset).rgb * _WaterColor.rgb;

				float fresnelLookup = dot(tangentNormal, normalViewDir);

				float bias = 0.06;
				float power = 4.0;
				float fresnelTerm = bias + (1.0 - bias) * pow(1.0 - fresnelLookup, power);

				float foamStrength = i.foamStrengthAndDistance.x * 1.8;

				half4 foam = clamp(tex2D(_Foam, i.bumpTexCoord.xy * 1.0) - 0.5, 0.0, 1.0) * foamStrength;
				float3 halfVec = normalize(normalViewDir - normalize(i.lightDir));
				float specular = pow(max(dot(halfVec, tangentNormal.xyz), 0.0), _LightIntensity);

				o.rgb = lerp(refraction, reflection, fresnelTerm) + clamp(foam.r, 0.0, 1.0) + specular;
				o.a = 1.0;

				return o;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
