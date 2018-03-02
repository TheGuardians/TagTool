#include "../parameters.hlsl"
#include "../helpers.hlsl"

#ifdef flag_bump_mapping_off

float3 bump_mapping_off(
    float3 tangentspace_x,
    float3 tangentspace_y,
    float3 tangentspace_z,
    float2 texture_coordinate
)
{
    float3 normal = float3(0, 0, -1);

    float3 model_normal = TangentSpaceToModelSpace(tangentspace_x, tangentspace_y, tangentspace_z, normal);

    return model_normal;
}

#endif