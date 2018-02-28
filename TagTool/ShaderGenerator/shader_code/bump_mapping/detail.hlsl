#include "../parameters.hlsl"
#include "../helpers.hlsl"

float3 bump_detail_map_detail(
    float3 normal_x_component,
    float3 normal_y_component,
    float3 normal_z_component,
    float2 normal_texture_coordinate,
    float2 normal_detail_texture_coordinate
)
{
    float3 normal_src = NormalMapSample(bump_map, normal_texture_coordinate);
    float3 normal_detail = NormalMapSample(bump_detail_map, normal_detail_texture_coordinate);

    float3 normal = normal_detail * bump_detail_coefficient.xxx + normal_src;

    normal = normalize(normal);
    normal = normal_x_component * normal.x + normal_y_component * normal.y + normal_z_component * normal.z;
    normal = normalize(normal);

    return normal;
}