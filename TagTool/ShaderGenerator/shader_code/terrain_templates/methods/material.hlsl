#include "../../helpers.hlsl"

float4 material_diffuse_only(
	float2 texcoord,
	sampler base_map,
	float4 base_map_xform,
	sampler detail_map,
	float4 detail_map_xform
)
{
	float2 base_map_texcoord = apply_xform(texcoord, base_map_xform);
	float4 base_map_sample = tex2D(base_map, base_map_texcoord);

	float2 detail_map_texcoord = apply_xform(texcoord, detail_map_xform);
	float4 detail_map_sample = tex2D(base_map, detail_map_texcoord);

	return base_map_sample * detail_map_sample;
}

float4 material_diffuse_plus_specular(
	float2 texcoord,
	sampler base_map,
	float4 base_map_xform,
	sampler detail_map,
	float4 detail_map_xform
)
{
	return float4(1.0, 0.0, 1.0, 1.0);
}

float4 material_off(float2 texcoord)
{
	return float4(0.0, 0.0, 0.0, 1.0);
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
#define material_0_diffuse_only(texcoord) material_diffuse_only(texcoord, base_map_m_0, base_map_m_0_xform, detail_map_m_0, detail_map_m_0_xform)
#endif
#ifdef flag_material_1_diffuse_only
#define material_1_diffuse_only(texcoord) material_diffuse_only(texcoord, base_map_m_1, base_map_m_1_xform, detail_map_m_1, detail_map_m_1_xform)
#endif
#ifdef flag_material_2_diffuse_only
#define material_2_diffuse_only(texcoord) material_diffuse_only(texcoord, base_map_m_2, base_map_m_2_xform, detail_map_m_2, detail_map_m_2_xform)
#endif
#ifdef flag_material_3_diffuse_only
#define material_3_diffuse_only(texcoord) material_diffuse_only(texcoord, base_map_m_3, base_map_m_3_xform, detail_map_m_3, detail_map_m_3_xform)
#endif

#ifdef flag_material_0_diffuse_plus_specular
#define material_0_diffuse_plus_specular(texcoord) material_diffuse_plus_specular(texcoord, base_map_m_0, base_map_m_0_xform, detail_map_m_0, detail_map_m_0_xform)
#endif
#ifdef flag_material_1_diffuse_plus_specular
#define material_1_diffuse_plus_specular(texcoord) material_diffuse_plus_specular(texcoord, base_map_m_1, base_map_m_1_xform, detail_map_m_1, detail_map_m_1_xform)
#endif
#ifdef flag_material_2_diffuse_plus_specular
#define material_2_diffuse_plus_specular(texcoord) material_diffuse_plus_specular(texcoord, base_map_m_2, base_map_m_2_xform, detail_map_m_2, detail_map_m_2_xform)
#endif
#ifdef flag_material_3_diffuse_plus_specular
#define material_3_diffuse_plus_specular(texcoord) material_diffuse_plus_specular(texcoord, base_map_m_3, base_map_m_3_xform, detail_map_m_3, detail_map_m_3_xform)
#endif