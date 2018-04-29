#define decal_template
#include "terrain_template_methods/template_default_defs.hlsl"
#include "terrain_template_methods/template_includes.hlsl"
#include "parameters.hlsl"

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
#ifndef flag_bump_mapping_leave
    float4 Normal;
    float4 Unknown;
#endif
};

PS_OUTPUT main(VS_OUTPUT input) : COLOR
{
	float2 texcoord = input.TexCoord.xy;
	// Untested, its possible these tangents are reversed
	float3 tangentspace_x = input.TexCoord1.xyz;
	float3 tangentspace_y = input.TexCoord2.xyz;
	float3 tangentspace_z = input.TexCoord3.xyz;
	float unknown = input.TexCoord1.w;

	float4 albedo = float4(1.0, 0.0, 0.0, 1.0);
	float3 normal = tangentspace_z;
	float3 color = albedo.xyz;
	float alpha = albedo.w;

	PS_OUTPUT output;

	output.Diffuse = float4(color, alpha);
	output.Normal = float4(NormalExport(normal), alpha);
	output.Unknown = unknown.xxxx;

	return output;
}