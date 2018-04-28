#include "../../helpers.hlsl"

#ifdef flag_albedo_color_mask_hard_light

float4 albedo_color_mask_hard_light(float2 texture_coordinate)
{
    return float4(1, 0, 0, 1); // Red not implemented output
}

#endif
