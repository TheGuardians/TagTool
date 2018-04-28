#include "../../helpers.hlsl"

#ifdef flag_albedo_two_change_color

float4 albedo_two_change_color(float2 texcoord)
{
    float2 base_map_texcoord = ApplyXForm(texcoord, base_map_xform);
    float2 detail_map_texcoord = ApplyXForm(texcoord, detail_map_xform);
    float2 change_color_map_texcoord = ApplyXForm(texcoord, change_color_map_xform);

    float4 base_map_sample = tex2D(base_map, detail_map_texcoord);
    float4 detail_map_sample = tex2D(detail_map, detail_map_texcoord);
    float4 change_color_map_sample = tex2D(change_color_map, change_color_map_texcoord);

    float2 change_color_value = change_color_map_sample.xy;
    float2 change_color_value_invert = 1.0 - change_color_value;

    float3 change_primary = primary_change_color * change_color_value.x + change_color_value_invert.x;
    float3 change_secondary = secondary_change_color * change_color_value.y + change_color_value_invert.y;

    float3 change_aggregate = change_primary * change_secondary;

    float4 base_detail_aggregate = base_map_sample * detail_map_sample;

    return float4(base_detail_aggregate.xyz * change_aggregate, base_detail_aggregate.w);
}

#endif
