#include "../parameters.hlsl"
#include "../helpers.hlsl"

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
