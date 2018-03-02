#include "../parameters.hlsl"
#include "../helpers.hlsl"

float3 bump_mapping_detail(
    float3 tangentspace_x,
    float3 tangentspace_y,
    float3 tangentspace_z,
    float2 texture_coordinate
)
{
    float2 bump_texture_coordinate = texture_coordinate * bump_map_xform.xy;
    float2 bump_detail_texture_coordinate = texture_coordinate * bump_detail_map_xform.xy;

    float3 normal_src = NormalMapSample(bump_map, bump_texture_coordinate);
    float3 normal_detail = NormalMapSample(bump_detail_map, bump_detail_texture_coordinate);

    float3 normal = normal_detail * bump_detail_coefficient.xxx + normal_src;

    float3 model_normal = TangentSpaceToModelSpace(tangentspace_x, tangentspace_y, tangentspace_z, normal);

    return model_normal;
}