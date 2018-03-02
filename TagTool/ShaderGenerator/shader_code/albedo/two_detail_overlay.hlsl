#include "../helpers.hlsl"

#ifdef flag_albedo_two_detail_overlay

float4 albedo_two_detail_overlay(float2 texcoord)
{
    float2 base_map_texcoord = ApplyXForm(texcoord, base_map_xform);
    float2 detail_map_texcoord = ApplyXForm(texcoord, detail_map_xform);
    float2 detail_map2_texcoord = ApplyXForm(texcoord, detail_map2_xform);
    float2 detail_map_overlay_texcoord = ApplyXForm(texcoord, detail_map_overlay_xform);

    float4 base_map_sample = tex2D(base_map, base_map_texcoord);
    float4 detail_map_sample = tex2D(detail_map, detail_map_texcoord);
    float4 detail_map2_sample = tex2D(detail_map2, detail_map2_texcoord);
    float4 detail_map_overlay_sample = tex2D(detail_map_overlay, detail_map_overlay_texcoord);

    float4 detail_blend = lerp(detail_map_sample, detail_map2_sample, base_map_sample.w);

    float3 detail_color = base_map_sample.xyz * detail_blend.xyz * detail_map_overlay_sample.xyz;

    float alpha = detail_map_overlay_sample.w * detail_blend.w;

    return float4(detail_color, alpha);
}

#endif
