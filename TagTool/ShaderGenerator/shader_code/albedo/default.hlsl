#include "../parameters.hlsl"

float4 albedo_default(float2 texture_coordinate)
{
    return tex2D(base_map, texture_coordinate);
}