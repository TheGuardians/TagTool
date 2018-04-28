#include "../../helpers.hlsl"

#ifdef flag_albedo_detail_blend

float4 albedo_detail_blend(float2 texcoord)
{
    float2 base_map_texcoord = ApplyXForm(texcoord, base_map_xform);
    float2 detail_map_texcoord = ApplyXForm(texcoord, detail_map_xform);
    float2 detail_map2_texcoord = ApplyXForm(texcoord, detail_map2_xform);

    float4 base_map_sample = tex2D(base_map, base_map_texcoord);
    float4 detail_map_sample = tex2D(detail_map, detail_map_texcoord);
    float4 detail_map2_sample = tex2D(detail_map2, detail_map2_texcoord);

    float3 blended_detail = lerp(detail_map_sample.xyz, detail_map2_sample.xyz, base_map_sample.w);

    return float4(base_map_sample.xyz * blended_detail, base_map_sample.w);
}

#endif
