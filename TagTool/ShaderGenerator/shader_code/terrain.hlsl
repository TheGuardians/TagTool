#ifndef __TERRAIN
#define __TERRAIN

#include "utilities.hlsl"
#include "texture_xform.hlsl"
#include "bump_mapping.hlsl"

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

	float3 bump_map_sample = sample_normal_2d(bump_map, apply_xform(texcoord, bump_map_xform));
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

	float2 bump_map_sample = sample_bump_map_2d(bump_map, apply_xform(texcoord, bump_map_xform));
	float2 detail_bump_sample = sample_bump_map_2d(detail_bump, apply_xform(texcoord, detail_bump_xform));
	float3 normal = reconstruct_normal(bump_map_sample + detail_bump_sample);

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

float4 get_blend_weights(float2 texcoord)
{
	float4 blend_weights = tex2D(blend_map, apply_xform(texcoord, blend_map_xform));
	float weights_aggregate = blend_weights.x + blend_weights.y + blend_weights.z + blend_weights.w;
	float inverse_weights_aggregate = 1.0 / weights_aggregate;
	blend_weights *= inverse_weights_aggregate;

	return blend_weights;
}

float4 albedo_terrain(float2 texcoord)
{
	float4 blend_weights = get_blend_weights(texcoord);
	float4 color_aggregate = float4(0.0, 0.0, 0.0, 0.0);

#ifndef flag_material_0_off
	MATERIAL_RESULT material_0 = Material_0(texcoord);

	float4 color_m_0 = material_0.Color * blend_weights.x;
	color_m_0 = -blend_weights.x >= 0 ? float4(0.0, 0.0, 0.0, 0.0) : color_m_0;
	color_aggregate = color_m_0;

#endif
#ifndef flag_material_1_off
	MATERIAL_RESULT material_1 = Material_1(texcoord);

	float4 color_m_1 = material_1.Color * blend_weights.y;
	color_m_1 = -blend_weights.y >= 0 ? float4(0.0, 0.0, 0.0, 0.0) : color_m_1;
	color_aggregate += color_m_1;

#endif
#ifndef flag_material_2_off
	MATERIAL_RESULT material_2 = Material_2(texcoord);

	float4 color_m_2 = material_2.Color * blend_weights.z;
	color_m_2 = -blend_weights.z >= 0 ? float4(0.0, 0.0, 0.0, 0.0) : color_m_2;
	color_aggregate += color_m_2;

#endif
#ifndef flag_material_3_off
	MATERIAL_RESULT material_3 = Material_3(texcoord);

	float4 color_m_3 = material_3.Color * blend_weights.w;
	color_m_3 = -blend_weights.w >= 0 ? float4(0.0, 0.0, 0.0, 0.0) : color_m_3;
	color_aggregate += color_m_3;

#endif

	return color_aggregate;
}

float3 bump_mapping_terrain(float3 tangentspace_x, float3 tangentspace_y, float3 tangentspace_z, float2 texcoord)
{
	float4 blend_weights = get_blend_weights(texcoord);
	float3 normal_aggregate = float3(0,0,0);

#ifndef flag_material_0_off
	MATERIAL_RESULT material_0 = Material_0(texcoord);

	float3 normal_m_0 = material_0.Normal * blend_weights.x;
	normal_m_0 = -blend_weights.x >= 0 ? float3(0.0, 0.0, 0.0) : normal_m_0;
	normal_aggregate += normal_m_0;

#endif
#ifndef flag_material_1_off
	MATERIAL_RESULT material_1 = Material_1(texcoord);

	float3 normal_m_1 = material_1.Normal * blend_weights.y;
	normal_m_1 = -blend_weights.y >= 0 ? float3(0.0, 0.0, 0.0) : normal_m_1;
	normal_aggregate += normal_m_1;

#endif
#ifndef flag_material_2_off
	MATERIAL_RESULT material_2 = Material_2(texcoord);

	float3 normal_m_2 = material_2.Normal * blend_weights.z;
	normal_m_2 = -blend_weights.z >= 0 ? float3(0.0, 0.0, 0.0) : normal_m_2;
	normal_aggregate += normal_m_2;

#endif
#ifndef flag_material_3_off
	MATERIAL_RESULT material_3 = Material_3(texcoord);

	float3 normal_m_3 = material_3.Normal * blend_weights.w;
	normal_m_3 = -blend_weights.w >= 0 ? float3(0.0, 0.0, 0.0) : normal_m_3;
	normal_aggregate += normal_m_3;

#endif
#if defined(flag_material_0_off) && defined(flag_material_0_off) && defined(flag_material_0_off) && defined(flag_material_0_off)
	normal_aggregate = tangentspace_z;
#endif

	return normal_transform(tangentspace_x, tangentspace_y, tangentspace_z, normal_aggregate);
}


#endif
