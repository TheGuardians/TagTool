#ifndef __HELPERS
#define __HELPERS

#include "parameters.hlsl"
#include "editor_only.hlsl"

float3 NormalExport(float3 normal)
{
    return normal * 0.5 + 0.5;
}

float3 NormalMapSample(sampler map, float2 texture_coordinate)
{
    float4 normal_map = tex2D(map, texture_coordinate);

    float2 normal_xy = normal_map.xy * 2.00787401 + -1.00787401;
    float2 normal_xy2 = normal_xy * normal_xy;
    float remainder = 1.0 - saturate(normal_xy2.x + normal_xy2.y);
    float normal_z = sqrt(remainder);

    return float3(normal_xy, normal_z);
}

float3 TangentSpaceToModelSpace(
    float3 tangentspace_x,
    float3 tangentspace_y,
    float3 tangentspace_z,
    float3 normal
)
{
    float3 src_normal = normalize(normal);
    float3 surface_normal = tangentspace_x * src_normal.x + tangentspace_y * src_normal.y + tangentspace_z * src_normal.z;
    float3 result = normalize(surface_normal);

    return result;
}

float2 ApplyXForm(float2 texcoord, float4 xform)
{
    return texcoord * xform.xy + xform.zw;
}

float3 Unknown_Crazy_Bungie_Color_Processing(float3 color)
{
    float4 r0 = float4(color, 0);
    float4 r1 = float4(0, 0, 0, 0);
    float4 r2 = float4(0, 0, 0, 0);

    // BEGIN RETARDED CODE
    r1.xyz = color.xyz * 4.59478998;
    r1.w = 4.59478998;
    r0.xyz = r0.xyz * -r1.xyz + debug_tint.xyz;
    r0.xyz = debug_tint.www * r0.xyz + r1.xyz;
    r1.xyz = log(r0.xyz);
    r1.xyz = r1.xyz * (5.0 / 12.0); // 5/12
    r2.xyz = exp(r1.xyz);
    r1.xyz = r2.xyz * 1.055 -0.055;
    r2.xyz = (-r0.xyz) * 12.92;
    r0.xyz = r0.xyz * 12.92;
    // END RETARDED CODE

    return r2.xyz >= 0 ? r0.xyz : r1.xyz;
}


#endif