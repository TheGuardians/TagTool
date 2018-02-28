#ifndef __HELPERS
#define __HELPERS

float3 NormalMapSample(sampler map, float2 texture_coordinate)
{
    float4 normal_map = tex2D(map, texture_coordinate);

    float2 normal_xy = normal_map.xy * 2.00787401 + -1.00787401;
    float2 normal_xy2 = normal_xy * normal_xy;
    float remainder = 1.0 - saturate(normal_xy2.x + normal_xy2.y);
    float normal_z = sqrt(remainder);

    return float3(normal_xy, normal_z);
}

#endif