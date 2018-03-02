#include "../parameters.hlsl"
#include "../helpers.hlsl"

#ifdef flag_bump_mapping_standard

float3 bump_mapping_standard(
    float3 tangentspace_x,
    float3 tangentspace_y,
    float3 tangentspace_z,
    float2 texture_coordinate
)
{
    float2 bump_texture_coordinate = texture_coordinate * bump_map_xform.xy;

    float3 normal = NormalMapSample(bump_map, bump_texture_coordinate);

    float3 model_normal = TangentSpaceToModelSpace(tangentspace_x, tangentspace_y, tangentspace_z, normal);

    return model_normal;
}

#endif
