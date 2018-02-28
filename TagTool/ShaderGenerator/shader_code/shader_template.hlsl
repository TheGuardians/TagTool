#include "template_default_defs.hlsl"
#include "template_includes.hlsl"
#include "parameters.hlsl"
#include "helpers.hlsl"

struct VS_OUTPUT
{
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

float3 bump_map_standard(float3 normal_x_component, float3 normal_y_component, float3 normal_z_component, float2 texture_coordinate)
{
    float4 normal_map = tex2D(bump_map, texture_coordinate);

    float2 normal_xy = normal_map.xy * 2.00787401 + -1.00787401;
    float2 normal_xy2 = normal_xy * normal_xy;
    float remainder = 1.0 - saturate(normal_xy2.x + normal_xy2.y);
    float normal_z = sqrt(remainder);

    float3 normal = float3(normal_xy, normal_z);

    normal = normalize(normal);
    normal = normal_x_component * normal.x + normal_y_component * normal.y + normal_z_component * normal.z;
    normal = normalize(normal);

    return normal;
}





float4 detail_default(float2 texture_coordinate)
{
    return tex2D(detail_map, texture_coordinate);
}


PS_OUTPUT main(VS_OUTPUT input) : COLOR
{
    PS_OUTPUT output;
    float2 texture_coordinate = input.TexCoord.xy;
    float3 normal_x_component = input.TexCoord3.xyz;
    float3 normal_y_component = input.TexCoord2.xyz;
    float3 normal_z_component = input.TexCoord1.xyz;
    float3 unknown = input.TexCoord1.w;

    float2 albedo_texture_coordinate = texture_coordinate * base_map_xform.xy;
    float2 detail_texture_coordinate = texture_coordinate * detail_map_xform.xy;
    float2 bump_texture_coordinate = texture_coordinate * bump_map_xform.xy;
    float2 bump_detail_texture_coordinate = texture_coordinate * bump_detail_map_xform.xy;
    
    float4 albedo = albedo_default(albedo_texture_coordinate);
    float4 detail = detail_default(detail_texture_coordinate);
    float4 color = albedo * detail * albedo_color;
    float3 color_postprocess = Unknown_Crazy_Bungie_Color_Processing(color.xyz);

    output.Diffuse = float4(color_postprocess, color.w);
    output.Normal = float4(1, 0, 0, color.w);
    output.Normal.xyz = bump_detail_map_detail(
        normal_x_component, 
        normal_y_component, 
        normal_z_component, 
        bump_detail_texture_coordinate, 
        bump_detail_texture_coordinate) * 0.5 + 0.5;


    output.Unknown = unknown.xxxx;
    return output;
}