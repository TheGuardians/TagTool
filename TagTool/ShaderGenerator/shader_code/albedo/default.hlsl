#include "../parameters.hlsl"

float4 albedo_default(float2 texture_coordinate)
{
    float2 albedo_texture_coordinate = texture_coordinate * base_map_xform.xy;
    float2 detail_texture_coordinate = texture_coordinate * detail_map_xform.xy;

    float4 base_map_sample = tex2D(base_map, albedo_texture_coordinate);
    float4 detail_map_sample = tex2D(detail_map, detail_texture_coordinate);

    return base_map_sample * detail_map_sample * albedo_color;
}