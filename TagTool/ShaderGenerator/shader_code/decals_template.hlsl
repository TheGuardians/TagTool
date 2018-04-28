#define decal_template
#include "decal_template_methods/template_default_defs.hlsl"
#include "decal_template_methods/template_includes.hlsl"
#include "parameters.hlsl"

uniform sampler2D base_map : register(s2);
uniform sampler2D bump_map : register(s3);

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

float4 GetBaseMap(float2 texcoord)
{
    return tex2D(base_map, texcoord);
}

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
	float alpha = albedo.w * fade;

	PS_OUTPUT output;

	output.Diffuse = Blend_Mode(float4(color, alpha));
#ifdef flag_bump_mapping_leave
	output.Normal = float4(0.0);
#else
	output.Normal = Blend_Mode(float4(NormalExport(normal), alpha));
#endif

	output.Unknown = float4(unknown);
	return output;
}