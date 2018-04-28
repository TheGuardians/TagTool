#include "../../helpers.hlsl"

#ifdef flag_albedo_constant_color

float4 albedo_constant_color(float2 texture_coordinate)
{
    return albedo_color;
}

#endif
