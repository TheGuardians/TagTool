#include "../helpers.hlsl"

#ifdef flag_albedo_chameleon

float4 albedo_constant_color(float2 texture_coordinate)
{
    return float4(1, 0, 0, 1); // Red not implemented output
}

#endif
