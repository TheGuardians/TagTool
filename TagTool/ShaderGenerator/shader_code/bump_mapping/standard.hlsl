#include "../parameters.hlsl"
#include "../helpers.hlsl"

float3 bump_mapping_standard(
    float3 normal_x_component,
    float3 normal_y_component,
    float3 normal_z_component,
    float2 normal_texture_coordinate,
    float2 normal_detail_texture_coordinate
)
{
    float3 normal = NormalMapSample(bump_map, normal_texture_coordinate);

    normal = normalize(normal);
    normal = normal_x_component * normal.x + normal_y_component * normal.y + normal_z_component * normal.z;
    normal = normalize(normal);

    return normal;
}