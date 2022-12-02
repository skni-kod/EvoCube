Shader "Custom/StandardFlatShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		// Deferred rendering.
		Pass
		{
			Name "DEFERRED"
			Tags{ "LightMode" = "DEFERRED" "RenderType" = "Opaque" }

			CGPROGRAM
			#pragma vertex ProcessVertex
			#pragma fragment ProcessFragment
			#pragma target 3.0
			#pragma multi_compile_instancing
			#pragma exclude_renderers nomrt
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#pragma multi_compile_prepassfinal novertexlight noshadowmask nodynlightmap nodirlightmap nolightmap
			#include "HLSLSupport.cginc"
			//#define UNITY_INSTANCED_LOD_FADE
			//#define UNITY_INSTANCED_SH
			//#define UNITY_INSTANCED_LIGHTMAPSTS
			#include "UnityShaderVariables.cginc"
			#include "UnityShaderUtilities.cginc"
			#include "UnityCG.cginc"
			#undef UNITY_SHOULD_SAMPLE_SH
			#define UNITY_SHOULD_SAMPLE_SH (!defined(UNITY_PASS_FORWARDADD) && !defined(UNITY_PASS_PREPASSBASE) && !defined(UNITY_PASS_SHADOWCASTER) && !defined(UNITY_PASS_META))
			#include "Lighting.cginc"

			fixed4 _Color;

			struct Vertex
			{
				float4 position : POSITION;
				fixed4 color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct FragmentInput
			{
				float4 position : SV_POSITION;
				float4 worldPosition : TEXCOORD0;
				half4 color : TEXCOORD1;
			};

			// Vertex Shader
			FragmentInput ProcessVertex(Vertex vertex)
			{
				UNITY_SETUP_INSTANCE_ID(vertex);

				FragmentInput output;
				output.position = UnityObjectToClipPos(vertex.position);

				// Calculate world position for face-normal calculation in fragment shader.
				output.worldPosition = mul(unity_ObjectToWorld, vertex.position);

				// ToDo: Pre-calculate sRGB color conversion for mesh vertices.
				output.color = pow((vertex.color + 0.055f) / 1.055f, 2.4f);
				output.color.a = 1.0;

				return output;
			}

			void ProcessFragment(FragmentInput input,
				out half4 outGBuffer0 : SV_Target0,
				out half4 outGBuffer1 : SV_Target1,
				out half4 outGBuffer2 : SV_Target2,
				out half4 outEmission : SV_Target3)
			{
				// Calculate surface values.
				SurfaceOutput surfaceOutput;

				// Calculate the world-space face normal.
				surfaceOutput.Normal = normalize(cross(ddy(input.worldPosition),
					ddx(input.worldPosition)));

				//surfaceOutput.Albedo = (half4)input.color;
				surfaceOutput.Albedo = _Color;
				surfaceOutput.Emission = 0.0;
				surfaceOutput.Specular = 0.0;
				surfaceOutput.Alpha = 0.0;
				surfaceOutput.Gloss = 0.0;

				// Setup lighting environment.
				UnityGI gi;
				UNITY_INITIALIZE_OUTPUT(UnityGI, gi);
				gi.indirect.diffuse = 0;
				gi.indirect.specular = 0;
				gi.light.color = 0;
				gi.light.dir = half3(0, 1, 0);

				// Note: The following adds roughly 40 shader instructions. Verify that it
				// is needed and possibly add an optimized variant for lower-end hardware.
				UnityGIInput giInput;
				UNITY_INITIALIZE_OUTPUT(UnityGIInput, giInput);
				giInput.light = gi.light;
				giInput.worldPos = input.worldPosition;
				giInput.atten = 1.0;
				giInput.lightmapUV = 0.0;
				// giInput.ambient = ShadeSHPerVertex( surfaceOutput.Normal, 0.0 );
				giInput.ambient = 0.0;
				giInput.probeHDR[0] = unity_SpecCube0_HDR;
				giInput.probeHDR[1] = unity_SpecCube1_HDR;

#if defined(UNITY_SPECCUBE_BLENDING) || defined(UNITY_SPECCUBE_BOX_PROJECTION)
				giInput.boxMin[0] = unity_SpecCube0_BoxMin;
#endif
#ifdef UNITY_SPECCUBE_BOX_PROJECTION
				giInput.boxMax[0] = unity_SpecCube0_BoxMax;
				giInput.probePosition[0] = unity_SpecCube0_ProbePosition;
				giInput.boxMax[1] = unity_SpecCube1_BoxMax;
				giInput.boxMin[1] = unity_SpecCube1_BoxMin;
				giInput.probePosition[1] = unity_SpecCube1_ProbePosition;
#endif
				LightingLambert_GI(surfaceOutput, giInput, gi);

				// Compute final g-buffer values.
				outEmission = LightingLambert_Deferred(surfaceOutput, gi,
					outGBuffer0, outGBuffer1, outGBuffer2);

				#ifndef UNITY_HDR_ON
					outEmission.rgb = exp2(-outEmission.rgb);
				#endif
			}
			ENDCG
		}

		// Forward rendering with ambient and main directional light for previews/thumbnails.
		Pass
		{
			Tags { "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma vertex ProcessVertex
			#pragma fragment ProcessFragment
			#pragma target 3.0
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"			

			fixed4 _Color;

			struct Vertex
			{
				float4 position : POSITION;
				fixed4 color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct FragmentInput
			{
				float4 position : SV_POSITION;
				float4 worldPosition : TEXCOORD0;
				fixed4 color : TEXCOORD1;
			};

			// Vertex Shader
			FragmentInput ProcessVertex(Vertex vertex)
			{
				vertex.color = _Color;
				UNITY_SETUP_INSTANCE_ID(vertex);

				FragmentInput output;
				output.position = UnityObjectToClipPos(vertex.position);

				// Calculate world position for face-normal calculation in fragment shader.
				output.worldPosition = mul(unity_ObjectToWorld, vertex.position);

				// ToDo: Pre-calculate sRGB color conversion in mesh vertices.
				output.color = pow((vertex.color + 0.055f) / 1.055f, 2.4f);
				return output;
			}

			// Fragment Shader
			fixed4 ProcessFragment(FragmentInput input) : SV_Target
			{
				// Compute the normal from the interpolated world position.
				half3 normal = normalize(cross(ddy(input.worldPosition),
					ddx(input.worldPosition)));

				// Calculate lighting and final color.
				half nl = saturate(dot(normal, _WorldSpaceLightPos0.xyz));
				return fixed4(input.color.rgb * (_LightColor0.rgb * nl + // Diffuse
					ShadeSH9(half4(normal.xyz, 1))), 1); // Ambient
			}
			ENDCG
		}

		// Optimized shadow caster shader.
		Pass
		{
			Tags { "LightMode" = "ShadowCaster" }

			CGPROGRAM
			#pragma vertex ProcessVertex
			#pragma fragment ProcessFragment
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"

			struct FragmentInput
			{
				V2F_SHADOW_CASTER;
			};

			// Vertex Shader
			FragmentInput ProcessVertex(appdata_base v)
			{
				UNITY_SETUP_INSTANCE_ID(v);
				FragmentInput output;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(output);
				return output;
			}

			// Fragment Shader
			float4 ProcessFragment(FragmentInput input) : SV_Target
			{
				SHADOW_CASTER_FRAGMENT(input);
			}
			ENDCG
		}
	}
}
