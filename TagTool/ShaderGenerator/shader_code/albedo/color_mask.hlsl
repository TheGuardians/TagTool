#include "../helpers.hlsl"

#ifdef flag_albedo_color_mask

float4 albedo_color_mask(float2 texcoord)
{
    float2 base_map_texcoord = ApplyXForm(texcoord, base_map_xform);
    float2 detail_map_texcoord = ApplyXForm(texcoord, detail_map_xform);
    float2 color_mask_map_texcoord = ApplyXForm(texcoord, color_mask_map_xform);

    float4 base_map_sample = tex2D(base_map, base_map_texcoord);
    float4 detail_map_sample = tex2D(detail_map, detail_map_texcoord);
    float4 color_mask_map_sample = tex2D(color_mask_map, color_mask_map_texcoord);

    float4 color = base_map_sample * detail_map_sample;
    
    float3 color_mask_invert = 1.0 - color_mask_map_sample.xyz;
    float4 neutral_invert = float4((1.0 / neutral_gray.xyz), 1.0);

    float4 masked_color0 = color_mask_map_sample.x * albedo_color;
    float4 masked_color1 = color_mask_map_sample.y * albedo_color2;
    float4 masked_color2 = color_mask_map_sample.z * albedo_color3;

    masked_color0 = masked_color0 * neutral_invert + color_mask_invert.xxxx;
    masked_color1 = masked_color1 * neutral_invert + color_mask_invert.yyyy;
    masked_color2 = masked_color2 * neutral_invert + color_mask_invert.zzzz;

    float4 result = color * masked_color0 * masked_color1 * masked_color2;

    return result;
}

#endif
