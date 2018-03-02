#include "../helpers.hlsl"

#ifdef flag_albedo_two_change_color

float4 albedo_two_change_color(float2 texture_coordinate)
{
    float2 change_color_map_texture_coordinate = texture_coordinate * change_color_map_xform.xy;
    float4 change_color_map_sample = tex2D(change_color_map, change_color_map_texture_coordinate);

    float2 change_color_value = change_color_map_sample.xy;
    float2 change_color_value_invert = 1.0 - change_color_value;

    float3 change_primary = primary_change_color * change_color_value.x + change_color_value_invert.x;
    float3 change_secondary = secondary_change_color * change_color_value.y + change_color_value_invert.y;

    float3 change_aggregate = change_primary * change_secondary;

    float2 albedo_texture_coordinate = texture_coordinate * base_map_xform.xy;
    float2 detail_texture_coordinate = texture_coordinate * detail_map_xform.xy;

    float4 base_map_sample = tex2D(base_map, albedo_texture_coordinate);
    float4 detail_map_sample = tex2D(detail_map, detail_texture_coordinate);

    float4 base_detail_aggregate = base_map_sample * detail_map_sample;

    return float4(base_detail_aggregate.xyz * change_aggregate, base_detail_aggregate.w);
}

#endif
