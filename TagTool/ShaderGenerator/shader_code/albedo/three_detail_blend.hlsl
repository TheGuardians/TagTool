#include "../helpers.hlsl"

#ifdef flag_albedo_three_detail_blend

float4 albedo_detail_blend(float2 texture_coordinate)
{
    float2 base_map_xform_texture_coordinate = texture_coordinate * base_map_xform.xy;
    float2 detail_map_texture_coordinate = texture_coordinate * detail_map_xform.xy;
    float2 detail_map2_texture_coordinate = texture_coordinate * detail_map2_xform.xy;
    float2 detail_map3_texture_coordinate = texture_coordinate * detail_map3_xform.xy;

    float4 base_map_sample = tex2D(base_map, base_map_xform_texture_coordinate);
    float4 detail_map_sample = tex2D(detail_map, detail_map_texture_coordinate);
    float4 detail_map2_sample = tex2D(detail_map, detail_map2_texture_coordinate);
    float4 detail_map3_sample = tex2D(detail_map, detail_map3_texture_coordinate);

    float alpha2 = saturate(base_map_sample.w * 2.0); // I don't understand why this is so
    float4 blended_detailA = lerp(detail_map_sample, detail_map2_sample, alpha2);

    float alpha2b = base_map_sample.w * 2.0 + -1.0; // I don't understand why this is so
    float4 blended_detailB = lerp(blended_detailA, detail_map3_sample, alpha2b);

    float3 color = base_map_sample.xyz * blended_detailB.xyz;

    // the alpha2 component there handles transparency throughout the entire interpolation
    // however, because of that, I think this entire function could be re-arranged and
    // that could lead to better assembly generation

    return float4(color, blended_detailB.w); 

}

#endif
