#include "../helpers.hlsl"

struct MATERIAL_RESULT
{
	float4 Color;
	float3 Normal;
};

MATERIAL_RESULT material_diffuse_only(
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

	MATERIAL_RESULT result;

	result.Color = color;
	result.Normal = bump_map_sample;

	return result;
}

MATERIAL_RESULT material_diffuse_plus_specular(
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

	MATERIAL_RESULT result;

	result.Color = color;
	result.Normal = bump_map_sample;

	return result;
}

MATERIAL_RESULT material_off(float2 texcoord)
{
	MATERIAL_RESULT result;

	result.Color = float4(0.0, 0.0, 0.0, 0.0);
	result.Normal = float3(0.0, 0.0, 1.0);

	return result;
}

#ifdef flag_material_0_off
#define material_0_off(texcoord) material_off(texcoord)
#endif
#ifdef flag_material_1_off
#define material_1_off(texcoord) material_off(texcoord)
#endif
#ifdef flag_material_2_off
#define material_2_off(texcoord) material_off(texcoord)
#endif
#ifdef flag_material_3_off
#define material_3_off(texcoord) material_off(texcoord)
#endif

#ifdef flag_material_0_diffuse_only
#define material_0_diffuse_only(texcoord) material_diffuse_only(texcoord, base_map_m_0, base_map_m_0_xform, detail_map_m_0, detail_map_m_0_xform, bump_map_m_0, bump_map_m_0_xform)
#endif
#ifdef flag_material_1_diffuse_only
#define material_1_diffuse_only(texcoord) material_diffuse_only(texcoord, base_map_m_1, base_map_m_1_xform, detail_map_m_1, detail_map_m_1_xform, bump_map_m_1, bump_map_m_1_xform)
#endif
#ifdef flag_material_2_diffuse_only
#define material_2_diffuse_only(texcoord) material_diffuse_only(texcoord, base_map_m_2, base_map_m_2_xform, detail_map_m_2, detail_map_m_2_xform, bump_map_m_2, bump_map_m_2_xform)
#endif
#ifdef flag_material_3_diffuse_only
#define material_3_diffuse_only(texcoord) material_diffuse_only(texcoord, base_map_m_3, base_map_m_3_xform, detail_map_m_3, detail_map_m_3_xform, bump_map_m_3, bump_map_m_3_xform)
#endif

#ifdef flag_material_0_diffuse_plus_specular
#define material_0_diffuse_plus_specular(texcoord) material_diffuse_plus_specular(texcoord, base_map_m_0, base_map_m_0_xform, detail_map_m_0, detail_map_m_0_xform, bump_map_m_0, bump_map_m_0_xform)
#endif
#ifdef flag_material_1_diffuse_plus_specular
#define material_1_diffuse_plus_specular(texcoord) material_diffuse_plus_specular(texcoord, base_map_m_1, base_map_m_1_xform, detail_map_m_1, detail_map_m_1_xform, bump_map_m_1, bump_map_m_1_xform)
#endif
#ifdef flag_material_2_diffuse_plus_specular
#define material_2_diffuse_plus_specular(texcoord) material_diffuse_plus_specular(texcoord, base_map_m_2, base_map_m_2_xform, detail_map_m_2, detail_map_m_2_xform, bump_map_m_2, bump_map_m_2_xform)
#endif
#ifdef flag_material_3_diffuse_plus_specular
#define material_3_diffuse_plus_specular(texcoord) material_diffuse_plus_specular(texcoord, base_map_m_3, base_map_m_3_xform, detail_map_m_3, detail_map_m_3_xform, bump_map_m_3, bump_map_m_3_xform)
#endif