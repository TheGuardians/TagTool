#include "../../helpers.hlsl"

#ifdef flag_bump_mapping_leave

//NOTE: This should not be set to output but actually never set the output result at all.
#define bump_mapping_leave(tangentspace_x, tangentspace_y, tangentspace_z, texcoord) ()

#endif

#ifdef flag_bump_mapping_standard

// identical
#include "../shader_template_methods/bump_mapping.hlsl"

#endif

#ifdef flag_bump_mapping_standard_mask

float3 bump_mapping_standard_mask(
	float3 tangentspace_x,
	float3 tangentspace_y,
	float3 tangentspace_z,
	float2 texcoord
)
{
	return tangentspace_z; // Not implemented
}

#endif