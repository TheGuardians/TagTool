#define beam_template
#define _debug_color float4(255, 255, 85, 255) / 255;
#include "template_default_defs.hlsl"
#include "template_includes.hlsl"
#include "../helpers.hlsl"

struct VS_OUTPUT
{
	float4 Color0 : COLOR0;
	float4 Color1 : COLOR1;
	float4 TexCoord : TEXCOORD;
};

struct PS_OUTPUT
{
	float4 Color0 : COLOR0;
	float4 Color1 : COLOR1;
	float4 Unknown : COLOR2;
};

PS_OUTPUT main(VS_OUTPUT input) : COLOR
{
	float2 texcoord = input.TexCoord.xy;
	float4 vertex_color_scale = input.Color0;
	float4 vertex_color_add = input.Color1;

	float4 albedo = Albedo(texcoord);

	float3 color = albedo.xyz * vertex_color_scale.xyz + vertex_color_add.xyz;
	float alpha = albedo.w * vertex_color_scale.w;

	PS_OUTPUT output;

	// High Frequency
	output.Color0 = Black_Point(float4(color, alpha * g_exposure.w));

	// Low Frequency
	output.Color1 = Black_Point(float4(color / g_exposure.y, alpha * g_exposure.z));

	output.Unknown = float4(0, 0, 0, 0);

	return output;
}
