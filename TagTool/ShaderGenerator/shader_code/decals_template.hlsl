#include "template_default_defs.hlsl"
#include "template_includes.hlsl"
#include "parameters.hlsl"

#define TexCoord_Default 0
#define TexCoord_Tiled 1

struct VS_OUTPUT
{
    //dcl_texcoord v0
    //dcl_texcoord1 v1.x
    //dcl_texcoord2 v2.xyz
    //dcl_texcoord3 v3.xyz
    //dcl_texcoord4 v4.xyz

    float4 TexCoord : TEXCOORD;
    float4 TexCoord1 : TEXCOORD1;
    float4 TexCoord2 : TEXCOORD2;
    float4 TexCoord3 : TEXCOORD3;
    float4 TexCoord4 : TEXCOORD4;

};

struct PS_OUTPUT
{
    float4 Diffuse;
    float4 Normal;
    float4 Unknown;
};

uniform sampler2D base_map : register(s2);
uniform sampler2D bump_map : register(s3);

float4 GetBaseMap(float2 texcoord)
{
    return tex2D(base_map, texcoord);
}

float3 bump_map_standard(float3 normal_vA, float3 normal_vB, float3 normal_vC, float2 texcoord)
{
    float4 normal_map = tex2D(bump_map, texcoord);

    float2 normal_xy = normal_map.xy * 2.00787401 + -1.00787401;
    float2 normal_xy2 = normal_xy * normal_xy;
    float remainder = 1.0 - saturate(normal_xy2.x + normal_xy2.y);
    float normal_z = sqrt(remainder);

    float3 normal = float3(normal_xy, normal_z);

    normal = normalize(normal);
    normal = normal_vA * normal.x + normal_vB * normal.y + normal_vC * normal.z;
    normal = normalize(normal);

    return normal;
}


PS_OUTPUT main(VS_OUTPUT input) : COLOR
{
    PS_OUTPUT output;

    float4 base_map = GetBaseMap(input.TexCoord.zw);

    float3 color = base_map.xyz;
    float alpha = base_map.w * fade;
    float3 color_pma = color * alpha;

    output.Diffuse = float4(color_pma, alpha);


    float3 normal = GetSurfaceNormal(input.TexCoord2.xyz, input.TexCoord3.xyz, input.TexCoord4.xyz, input.TexCoord.zw);
    
    
    output.Normal = float4(normal * 0.5 + 0.5, alpha);
    output.Unknown = input.TexCoord1.xxxx;
    return output;
}