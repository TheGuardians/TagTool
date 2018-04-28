#include "../helpers.hlsl"

#ifdef flag_bump_mapping_detail

float3 bump_mapping_detail(
	float3 tangentspace_x,
	float3 tangentspace_y,
	float3 tangentspace_z,
	float2 texcoord
)
{
	float2 bump_map_texcoord = ApplyXForm(texcoord, bump_map_xform);
	float2 bump_detail_map_texcoord = ApplyXForm(texcoord, bump_detail_map_xform);

	float3 normal_src = NormalMapSample(bump_map, bump_map_texcoord);
	float3 normal_detail = NormalMapSample(bump_detail_map, bump_detail_map_texcoord);

	float3 normal = normal_detail * bump_detail_coefficient.xxx + normal_src;

	float3 model_normal = TangentSpaceToModelSpace(tangentspace_x, tangentspace_y, tangentspace_z, normal);

	return model_normal;
}

#endif

#ifdef flag_bump_mapping_detail_masked

float3 bump_mapping_detail_masked(
	float3 tangentspace_x,
	float3 tangentspace_y,
	float3 tangentspace_z,
	float2 texcoord
)
{
	return tangentspace_z; // Not implemented yet, so just return surface normal
}

#endif

#ifdef flag_bump_mapping_detail_plus_detail_masked

float3 bump_mapping_detail_plus_detail_masked(
	float3 tangentspace_x,
	float3 tangentspace_y,
	float3 tangentspace_z,
	float2 texcoord
)
{
	return tangentspace_z; // Not implemented yet, so just return surface normal
}

#endif

#ifdef flag_bump_mapping_off

float3 bump_mapping_off(
	float3 tangentspace_x,
	float3 tangentspace_y,
	float3 tangentspace_z,
	float2 texture_coordinate
)
{
	// It appears that HO just exports the tangentspace_z directly without worrying about size.
	// Going to do the same for the sake of performance and sanity but the relevant code is below.

	//float3 normal = float3(0, 0, 1);
	//float3 model_normal = TangentSpaceToModelSpace(tangentspace_x, tangentspace_y, tangentspace_z, normal);
	//return model_normal;

	return tangentspace_z;
}

#endif

#ifdef flag_bump_mapping_standard

float3 bump_mapping_standard(
	float3 tangentspace_x,
	float3 tangentspace_y,
	float3 tangentspace_z,
	float2 texcoord
)
{
	float2 bump_map_texcoord = ApplyXForm(texcoord, bump_map_xform);

	float3 normal = NormalMapSample(bump_map, bump_map_texcoord);

	float3 model_normal = TangentSpaceToModelSpace(tangentspace_x, tangentspace_y, tangentspace_z, normal);

	return model_normal;
}

#endif
