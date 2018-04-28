#include "../../helpers.hlsl"

#ifdef flag_albedo_two_detail

float4 albedo_two_detail(float2 texcoord)
{
    float2 base_map_texcoord = ApplyXForm(texcoord, base_map_xform);
    float2 detail_map_texcoord = ApplyXForm(texcoord, detail_map_xform);
    float2 detail_map2_texcoord = ApplyXForm(texcoord, detail_map2_xform);

    float4 base_map_sample = tex2D(base_map, base_map_texcoord);
    float4 detail_map_sample = tex2D(detail_map, detail_map_texcoord);
    float4 detail_map2_sample = tex2D(detail_map2, detail_map2_texcoord);

    float4 detail_color = base_map_sample * detail_map_sample * detail_map2_sample;

    return detail_color;

}

#endif