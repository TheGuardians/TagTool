#include "../helpers.hlsl"

#ifdef flag_albedo_color_mask

float4 albedo_color_mask(float2 texture_coordinate)
{
    return float4(1, 0, 0, 1); // Red not implemented output
}

#endif
