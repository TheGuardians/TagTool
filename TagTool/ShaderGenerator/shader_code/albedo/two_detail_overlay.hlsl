#include "../helpers.hlsl"

#ifdef flag_albedo_two_detail_overlay

float4 albedo_two_detail_overlay(float2 texture_coordinate)
{
    float2 base_map_xform_texture_coordinate = texture_coordinate * base_map_xform.xy;
    float2 detail_map_texture_coordinate = texture_coordinate * detail_map_xform.xy;
    float2 detail_map2_texture_coordinate = texture_coordinate * detail_map2_xform.xy;
    float2 detail_map_overlay_texture_coordinate = texture_coordinate * detail_map_overlay_xform.xy;

    float4 base_map_sample = tex2D(base_map, base_map_xform_texture_coordinate);
    float4 detail_map_sample = tex2D(detail_map, detail_map_texture_coordinate);
    float4 detail_map2_sample = tex2D(detail_map, detail_map2_texture_coordinate);
    float4 detail_map_overlay_sample = tex2D(detail_map, detail_map_overlay_texture_coordinate);

    float4 detail_blend = lerp(detail_map_sample, detail_map2_sample, base_map_sample.w);

    float3 detail_color = base_map_sample.xyz * detail_blend.xyz * detail_map_overlay_sample.xyz;

    float alpha = detail_map_overlay_sample.w * detail_blend.w;

    return float4(detail_color, alpha);
}

#endif
