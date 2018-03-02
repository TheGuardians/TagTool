#include "../helpers.hlsl"

#ifdef flag_albedo_two_detail_black_point

float4 albedo_two_detail_black_point(float2 texture_coordinate)
{
    return float4(1, 0, 0, 1); // Red not implemented output
}

#endif
