#include "../../helpers.hlsl"

struct MATERIAL_RESULT
{
	float4 Color;
	float3 Normal;
};

MATERIAL_RESULT material_get_default(
	float2 texcoord,
	sampler base_map,
	float4 base_map_xform,
	sampler detail_map,
	float4 detail_map_xform,
	sampler bump_map,
	float4 bump_map_xform
)
{
	float4 base_map_sample = tex2D(base_map, apply_xform(texcoord, base_map_xform));
	float4 detail_map_sample = tex2D(detail_map, apply_xform(texcoord, detail_map_xform));
	float4 color = base_map_sample * detail_map_sample;

	float3 bump_map_sample = normal_x2reconstruct_sample(bump_map, apply_xform(texcoord, bump_map_xform));
	float3 normal = bump_map_sample;

	MATERIAL_RESULT result;

	result.Color = color;
	result.Normal = normal;

	return result;
}

MATERIAL_RESULT material_get_detail_bump(
	float2 texcoord,
	sampler base_map,
	float4 base_map_xform,
	sampler detail_map,
	float4 detail_map_xform,
	sampler bump_map,
	float4 bump_map_xform,
	sampler detail_bump,
	float4 detail_bump_xform
)
{
	float4 base_map_sample = tex2D(base_map, apply_xform(texcoord, base_map_xform));
	float4 detail_map_sample = tex2D(detail_map, apply_xform(texcoord, detail_map_xform));
	float4 color = base_map_sample * detail_map_sample;

	float2 bump_map_sample = normal_x2_sample(bump_map, apply_xform(texcoord, bump_map_xform));
	float2 detail_bump_sample = normal_x2_sample(detail_bump, apply_xform(texcoord, detail_bump_xform));
	float3 normal = reconstruct_x2_normal(bump_map_sample + detail_bump_sample);

	MATERIAL_RESULT result;

	result.Color = color;
	result.Normal = normal;

	return result;
}

MATERIAL_RESULT material_off(float2 texcoord)
{
	MATERIAL_RESULT result;

	result.Color = float4(0.0, 0.0, 0.0, 0.0);
	result.Normal = float3(0.0, 0.0, 1.0);

	return result;
}

#define material_0_off(texcoord) material_off(texcoord)
#define material_1_off(texcoord) material_off(texcoord)
#define material_2_off(texcoord) material_off(texcoord)
#define material_3_off(texcoord) material_off(texcoord)

#ifndef flag_use_detail_bumps

#define material_0_diffuse_only(texcoord) material_get_default(texcoord, base_map_m_0, base_map_m_0_xform, detail_map_m_0, detail_map_m_0_xform, bump_map_m_0, bump_map_m_0_xform)
#define material_1_diffuse_only(texcoord) material_get_default(texcoord, base_map_m_1, base_map_m_1_xform, detail_map_m_1, detail_map_m_1_xform, bump_map_m_1, bump_map_m_1_xform)
#define material_2_diffuse_only(texcoord) material_get_default(texcoord, base_map_m_2, base_map_m_2_xform, detail_map_m_2, detail_map_m_2_xform, bump_map_m_2, bump_map_m_2_xform)
#define material_3_diffuse_only(texcoord) material_get_default(texcoord, base_map_m_3, base_map_m_3_xform, detail_map_m_3, detail_map_m_3_xform, bump_map_m_3, bump_map_m_3_xform)
#define material_0_diffuse_plus_specular(texcoord) material_get_default(texcoord, base_map_m_0, base_map_m_0_xform, detail_map_m_0, detail_map_m_0_xform, bump_map_m_0, bump_map_m_0_xform)
#define material_1_diffuse_plus_specular(texcoord) material_get_default(texcoord, base_map_m_1, base_map_m_1_xform, detail_map_m_1, detail_map_m_1_xform, bump_map_m_1, bump_map_m_1_xform)
#define material_2_diffuse_plus_specular(texcoord) material_get_default(texcoord, base_map_m_2, base_map_m_2_xform, detail_map_m_2, detail_map_m_2_xform, bump_map_m_2, bump_map_m_2_xform)
#define material_3_diffuse_plus_specular(texcoord) material_get_default(texcoord, base_map_m_3, base_map_m_3_xform, detail_map_m_3, detail_map_m_3_xform, bump_map_m_3, bump_map_m_3_xform)

#else

#define material_0_diffuse_only(texcoord) material_get_detail_bump(texcoord, base_map_m_0, base_map_m_0_xform, detail_map_m_0, detail_map_m_0_xform, bump_map_m_0, bump_map_m_0_xform, detail_bump_m_0, detail_bump_m_0_xform)
#define material_1_diffuse_only(texcoord) material_get_detail_bump(texcoord, base_map_m_1, base_map_m_1_xform, detail_map_m_1, detail_map_m_1_xform, bump_map_m_1, bump_map_m_1_xform, detail_bump_m_1, detail_bump_m_1_xform)
#define material_2_diffuse_only(texcoord) material_get_detail_bump(texcoord, base_map_m_2, base_map_m_2_xform, detail_map_m_2, detail_map_m_2_xform, bump_map_m_2, bump_map_m_2_xform, detail_bump_m_2, detail_bump_m_2_xform)
#define material_3_diffuse_only(texcoord) material_get_detail_bump(texcoord, base_map_m_3, base_map_m_3_xform, detail_map_m_3, detail_map_m_3_xform, bump_map_m_3, bump_map_m_3_xform, detail_bump_m_3, detail_bump_m_3_xform)
#define material_0_diffuse_plus_specular(texcoord) material_get_detail_bump(texcoord, base_map_m_0, base_map_m_0_xform, detail_map_m_0, detail_map_m_0_xform, bump_map_m_0, bump_map_m_0_xform, detail_bump_m_0, detail_bump_m_0_xform)
#define material_1_diffuse_plus_specular(texcoord) material_get_detail_bump(texcoord, base_map_m_1, base_map_m_1_xform, detail_map_m_1, detail_map_m_1_xform, bump_map_m_1, bump_map_m_1_xform, detail_bump_m_1, detail_bump_m_1_xform)
#define material_2_diffuse_plus_specular(texcoord) material_get_detail_bump(texcoord, base_map_m_2, base_map_m_2_xform, detail_map_m_2, detail_map_m_2_xform, bump_map_m_2, bump_map_m_2_xform, detail_bump_m_2, detail_bump_m_2_xform)
#define material_3_diffuse_plus_specular(texcoord) material_get_detail_bump(texcoord, base_map_m_3, base_map_m_3_xform, detail_map_m_3, detail_map_m_3_xform, bump_map_m_3, bump_map_m_3_xform, detail_bump_m_3, detail_bump_m_3_xform)

#endif