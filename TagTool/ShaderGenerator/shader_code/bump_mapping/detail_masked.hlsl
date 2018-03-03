#include "../parameters.hlsl"
#include "../helpers.hlsl"

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