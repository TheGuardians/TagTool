#define decal_template
#define _debug_color float4(255, 85, 85, 255) / 255;
#include "template_default_defs.hlsl"
#include "template_includes.hlsl"
#include "../parameters.hlsl"

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
#ifndef flag_bump_mapping_leave
    float4 Normal;
    float4 Unknown;
#endif
};

PS_OUTPUT main(VS_OUTPUT input) : COLOR
{
	float2 texcoord_tiled = input.TexCoord.xy;
	float2 texcoord = input.TexCoord.zw;
	float3 tangentspace_x = input.TexCoord2.xyz;
	float3 tangentspace_y = input.TexCoord3.xyz;
	float3 tangentspace_z = input.TexCoord4.xyz;
	float unknown = input.TexCoord1.x;

	float4 albedo = Albedo(texcoord);
	float3 normal = Bump_Mapping(tangentspace_x, tangentspace_y, tangentspace_z, texcoord);
	float3 color = albedo.xyz;
	float alpha = albedo.w;

	PS_OUTPUT output;

#ifdef flag_render_pass_pre_lighting
	output.Diffuse = Specular(Blend_Mode(Tinting(float4(color, alpha))));
#else
	output.Diffuse.xyz = albedo.xyz * tint_color.xyz * intensity.x;
	output.Diffuse.w = albedo.w;
#endif


	
#ifndef flag_bump_mapping_leave
	output.Normal = Blend_Mode(float4(normal_export(normal), alpha));
	output.Unknown = unknown.xxxx;
#endif
	return output;
}