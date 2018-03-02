#include "../helpers.hlsl"

#ifdef flag_albedo_detail_blend

float4 albedo_detail_blend(float2 texture_coordinate)
{
    float2 base_map_xform_texture_coordinate = texture_coordinate * base_map_xform.xy;
    float2 detail_map_texture_coordinate = texture_coordinate * detail_map_xform.xy;
    float2 detail_map2_texture_coordinate = texture_coordinate * detail_map2_xform.xy;

    float4 base_map_sample = tex2D(base_map, base_map_xform_texture_coordinate);
    float4 detail_map_sample = tex2D(detail_map, detail_map_texture_coordinate);
    float4 detail_map2_sample = tex2D(detail_map, detail_map2_texture_coordinate);

    float3 blended_detail = lerp(detail_map2_sample.xyz, detail_map_sample.xyz, base_map_sample.w);

    return base_map_sample * blended_detail;
}

#endif
