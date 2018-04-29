#define shader_template
#define _debug_color float4(85, 255, 255, 255) / 255;
#include "shader_template_methods/template_default_defs.hlsl"
#include "shader_template_methods/template_includes.hlsl"
#include "parameters.hlsl"
#include "helpers.hlsl"

struct VS_OUTPUT
{
    float4 TexCoord : TEXCOORD;
    float4 TexCoord1 : TEXCOORD1;
    float4 TexCoord2 : TEXCOORD2;
    float4 TexCoord3 : TEXCOORD3;
};

struct PS_OUTPUT
{
    float4 Diffuse;
    float4 Normal;
    float4 Unknown;
};

PS_OUTPUT main(VS_OUTPUT input) : COLOR
{
    float2 texcoord = input.TexCoord.xy;
    float2 texcoord_tiled = input.TexCoord.zw;
    float3 tangentspace_x = input.TexCoord3.xyz;
    float3 tangentspace_y = input.TexCoord2.xyz;
    float3 tangentspace_z = input.TexCoord1.xyz;
    float3 unknown = input.TexCoord1.w;
    
    float4 albedo = Albedo(texcoord);

    float3 color = Unknown_Crazy_Bungie_Color_Processing(albedo.xyz);
    float3 normal = Bump_Mapping(tangentspace_x, tangentspace_y, tangentspace_z, texcoord);
    float alpha = albedo.w;

    PS_OUTPUT output;
    output.Diffuse = Blend_Mode(float4(color, albedo.w));
    output.Normal = Blend_Mode(float4(NormalExport(normal), albedo.w));

    output.Unknown = unknown.xxxx;
    return output;
}