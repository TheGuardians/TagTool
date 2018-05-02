#ifndef __BUMP_MAPPING
#define __BUMP_MAPPING

#include "texture_xform.hlsl"
#include "utilities.hlsl"

float2 sample_bump_map_2d(sampler bump_map, float2 texcoord)
{
	float4 bump_map_sample = tex2D(bump_map, texcoord);

	float scale = 255.0 / 127.0;
	float offset = 128.0 / 127.0;

	return bump_map_sample.xy * scale - offset;
}

/**
* reconstructs the positive z component of a normal using the X and Y components
* @result z = 1.0 - sqrt(saturate(x^2 + y^2))
*/
float reconstruct_normal_z(float2 normal)
{
	float remainder = 1.0 - saturate(dot(normal, normal));
	float normal_z = sqrt(remainder);
	return normal_z;
}

/*
* reconstructs a normal from only the X and Y components
* @param normal the X and Y component of the normal
* @result (x, y, 1.0 - sqrt(x^2 + y^2))
*/
float3 reconstruct_normal(float2 normal)
{
	return float3(normal, reconstruct_normal_z(normal));
}

float3 sample_normal_2d(sampler bump_map, float2 texcoord)
{
	float2 bump_sample = sample_bump_map_2d(bump_map, texcoord);

	return reconstruct_normal(bump_sample);
}

float3 normal_transform(
	float3 tangentspace_x,
	float3 tangentspace_y,
	float3 tangentspace_z,
	float3 normal
)
{
	float3 src_normal = normalize(normal);
	float3 surface_normal = tangentspace_x * src_normal.x + tangentspace_y * src_normal.y + tangentspace_z * src_normal.z;
	float3 result = normalize(surface_normal);

	return result;
}

#ifdef flag_bump_mapping_leave

/**
 * This is supposed to be filtered out and never actually used
*/
float3 bump_mapping_leave(float3 tangentspace_x, float3 tangentspace_y, float3 tangentspace_z, float2 texcoord)
{
	return tangentspace_z;
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
	return tangentspace_z;
}

#endif

#ifdef flag_bump_mapping_standard

float3 bump_mapping_standard(float3 tangentspace_x, float3 tangentspace_y, float3 tangentspace_z, float2 texcoord)
{
	float2 bump_map_texcoord = apply_xform(texcoord, bump_map_xform);
	float3 normal = sample_normal_2d(bump_map, bump_map_texcoord);
	float3 model_normal = normal_transform(tangentspace_x, tangentspace_y, tangentspace_z, normal);
	return model_normal;
}

#endif

#ifdef flag_bump_mapping_detail

float3 bump_mapping_detail_ext(float3 tangentspace_x, float3 tangentspace_y, float3 tangentspace_z, float2 texcoord)
{
	float3 bump_map_sample = sample_normal_2d(bump_map, apply_xform(texcoord, bump_map_xform));
	float3 bump_detail_map_sample = sample_normal_2d(bump_detail_map, apply_xform(texcoord, bump_detail_map_xform));

	float3 normal = bump_map_sample + bump_detail_map_sample * bump_detail_coefficient.x;

	return normal_transform(tangentspace_x, tangentspace_y, tangentspace_z, normal);
}

#endif

#ifdef flag_bump_mapping_standard_mask

float3 bump_mapping_standard_mask(float3 tangentspace_x, float3 tangentspace_y, float3 tangentspace_z, float2 texcoord)
{
	return tangentspace_z; // Not implemented
}

#endif

#ifdef flag_bump_mapping_detail_masked

float3 bump_mapping_detail_masked(float3 tangentspace_x, float3 tangentspace_y, float3 tangentspace_z, float2 texcoord)
{
	return tangentspace_z; // Not implemented yet, so just return surface normal
}

#endif

#ifdef flag_bump_mapping_detail_plus_detail_masked

float3 bump_mapping_detail_plus_detail_masked(float3 tangentspace_x, float3 tangentspace_y, float3 tangentspace_z, float2 texcoord)
{
	return tangentspace_z; // Not implemented yet, so just return surface normal
}

#endif

#endif
