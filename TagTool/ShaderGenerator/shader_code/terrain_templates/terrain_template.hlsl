#define terrain_template
#define _debug_color float4(0, 170, 170, 255) / 255;
#include "template_default_defs.hlsl"
#include "template_includes.hlsl"
#include "../helpers.hlsl"

struct VS_OUTPUT
{
    float4 TexCoord : TEXCOORD;
    float4 TexCoord1 : TEXCOORD1;
    float4 TexCoord2 : TEXCOORD2;
    float4 TexCoord3 : TEXCOORD3;
};

struct PS_OUTPUT
{
    float4 Diffuse;
#ifndef flag_bump_mapping_leave
    float4 Normal;
    float4 Unknown;
#endif
};

float4 sample_texture(sampler _sampler, float2 texcoord, float4 xform)
{
	return tex2D(_sampler, apply_xform(texcoord, xform));
}

float4 sample_material(sampler _base_map, sampler _detail_map, float2 texcoord, float4 _base_xform, float4 _detail_xform)
{
	float4 base_map_sample = sample_texture(_base_map, texcoord, _base_xform);
	float4 detail_map_sample = sample_texture(_detail_map, texcoord, _detail_xform);
	float4 color = base_map_sample * detail_map_sample;
	return color;
}

float3 bungie_color_processing_terrain(float3 color)
{
	float4 c0 = float4(2.00787401, -1.00787401, 0, 1);
	float4 c1 = float4(4.59478998, 0.00313080009, 12.9200001, 0.416666657);
	float4 c2 = float4(1.05499995, -0.0549999997, 0.5, 0);

	float4 r2, r3, r5;

	r2.xyz = color;

	r3.x = c1.x; //mov r3.x, c1.x
	r2.w = r3.x * global_albedo_tint.x; //mul r2.w, r3.x, c60.x
	r3.xyz = r2.xyz * r2.w; //mul r3.xyz, r2.w, r2
	r2.xyz = r2.xyz * -r2.w + debug_tint.xyz; //mad r2.xyz, r2, -r2.w, c58
	r2.xyz = r2.xyz * debug_tint.w + r3.xyz; //mad r2.xyz, c58.w, r2, r3
											 //log r3.x, r2.x
											 //log r3.y, r2.y
											 //log r3.z, r2.z
	r3.xyz = log(r2.xyz);
	r3.xyz = r3.xyz * c1.w; //mul r3.xyz, r3, c1.w
							//exp r5.x, r3.x
							//exp r5.y, r3.y
							//exp r5.z, r3.z
	r5.xyz = exp(r3.xyz);
	r3.xyz = r5.xyz * c2.x + c2.y; //mad r3.xyz, r5, c2.x, c2.y
	r5.xyz = -r2.xyz + c1.y; //add r5.xyz, -r2, c1.y
	r2.xyz = r2.xyz * c1.z; //mul r2.xyz, r2, c1.z
							//cmp oC0.xyz, r5, r2, r3
	float3 result = r5.xyz >= 0 ? r2.xyz : r3.xyz;

	return result;
}

PS_OUTPUT main(VS_OUTPUT input) : COLOR
{
	float2 texcoord = input.TexCoord.xy;
	// Untested, its possible these tangents are reversed
	float3 tangentspace_x = input.TexCoord3.xyz;
	float3 tangentspace_y = input.TexCoord2.xyz;
	float3 tangentspace_z = input.TexCoord1.xyz;
	float unknown = input.TexCoord1.w;

	float4 albedo = float4(1.0, 0.0, 0.0, 1.0);
	float3 normal = tangentspace_z;
	float3 color = albedo.xyz;
	float alpha = albedo.w;

	//mad r0.xy, v0, c70, c70.zwzw
	//texld r0, r0, s11
	//mad r1.xy, v0, c71, c71.zwzw
	//texld r1, r1, s12
	//mul r0, r0, r1
	float4 albedo_m_3 = sample_material(base_map_m_3, detail_map_m_3, texcoord, base_map_m_3_xform, detail_map_m_3_xform);
	//mad r1.xy, v0, c67, c67.zwzw
	//texld r1, r1, s8
	//mad r2.xy, v0, c68, c68.zwzw
	//texld r2, r2, s9
	//mul r1, r1, r2
	float4 albedo_m_2 = sample_material(base_map_m_2, detail_map_m_2, texcoord, base_map_m_2_xform, detail_map_m_2_xform);
	//mad r2.xy, v0, c64, c64.zwzw
	//texld r2, r2, s5
	//mad r3.xy, v0, c65, c65.zwzw
	//texld r3, r3, s6
	//mul r2, r2, r3
	float4 albedo_m_1 = sample_material(base_map_m_1, detail_map_m_1, texcoord, base_map_m_1_xform, detail_map_m_1_xform);
	//mad r3.xy, v0, c61, c61.zwzw
	//texld r3, r3, s2
	//mad r4.xy, v0, c62, c62.zwzw
	//texld r4, r4, s3
	//mul r3, r3, r4
	float4 albedo_m_0 = sample_material(base_map_m_0, detail_map_m_0, texcoord, base_map_m_0_xform, detail_map_m_0_xform);

	//mad r4.xy, v0, c59, c59.zwzw
	//texld r4, r4, s0
	float4 blend_weights = sample_texture(blend_map, texcoord, blend_map_xform);

	//add r5.x, r4.y, r4.x
	//add r5.x, r4.z, r5.x
	//add r5.x, r4.w, r5.x
	float weights_aggregate = blend_weights.x + blend_weights.y + blend_weights.z + blend_weights.w;

	//rcp r5.x, r5.x
	float inverse_weights_aggregate = 1.0 / weights_aggregate;

	//NOTE: The total weights size of blend_weights equals 1. x + y + z + w = 1.0
	//mul r4, r4, r5.x
	blend_weights *= inverse_weights_aggregate;

	float4 color_aggregate;
	{
		// mul r3, r3, r4.x
		// cmp r3, -r4.x, c0.z, r3

		float4 color_m_0 = albedo_m_0 * blend_weights.x;
		color_m_0 = -blend_weights.x >= 0 ? float4(0.0, 0.0, 0.0, 0.0) : color_m_0;

		color_aggregate = color_m_0;
	}
	{
		// mad r2, r2, r4.y, r3
		// cmp r2, -r4.y, r3, r2

		float4 color_m_1 = albedo_m_1 * blend_weights.y;
		color_m_1 = -blend_weights.y >= 0 ? float4(0.0, 0.0, 0.0, 0.0) : color_m_1;

		color_aggregate += color_m_1;
	}
	{
		// mad r1, r1, r4.z, r2
		// cmp r1, -r4.z, r2.wxyz, r1.wxyz

		float4 color_m_2 = albedo_m_2 * blend_weights.z;
		color_m_2 = -blend_weights.z >= 0 ? float4(0.0, 0.0, 0.0, 0.0) : color_m_2;

		color_aggregate += color_m_2;
	}
	{
		//mad r0, r0.wxyz, r4.w, r1
		//cmp r2.xyz, -r4.w, r1.yzww, r0.yzww
		float4 color_m_3 = albedo_m_3 * blend_weights.w;
		color_m_3 = -blend_weights.w >= 0 ? float4(0.0, 0.0, 0.0, 0.0) : color_m_3;

		color_aggregate += color_m_3;
	}

	color_aggregate.xyz = bungie_color_processing_terrain(color_aggregate.xyz);

	PS_OUTPUT output;

	output.Diffuse = color_aggregate;
	output.Normal = float4(normal_export(tangentspace_z), 1.0);
	output.Unknown = unknown.xxxx;

	return output;
}
