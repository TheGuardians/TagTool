#define terrain_template
#define _debug_color float4(0, 170, 170, 255) / 255;
#include "template_default_defs.hlsl"
#include "template_includes.hlsl"
#include "../helpers.hlsl"

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
	float3 tangentspace_x = normalize(input.TexCoord3.xyz);
	float3 tangentspace_y = normalize(input.TexCoord2.xyz);
	float3 tangentspace_z = normalize(input.TexCoord1.xyz);
	float unknown = input.TexCoord1.w;

	float4 blend_weights = tex2D(blend_map, apply_xform(texcoord, blend_map_xform));
	float weights_aggregate = blend_weights.x + blend_weights.y + blend_weights.z + blend_weights.w;
	float inverse_weights_aggregate = 1.0 / weights_aggregate;
	blend_weights *= inverse_weights_aggregate;

	float4 color_aggregate = float4(0.0, 0.0, 0.0, 0.0);
	float3 normal_aggregate = tangentspace_z;

#ifndef flag_material_0_off
	MATERIAL_RESULT material_0 = Material_0(texcoord);

	float4 color_m_0 = material_0.Color * blend_weights.x;
	color_m_0 = -blend_weights.x >= 0 ? float4(0.0, 0.0, 0.0, 0.0) : color_m_0;
	color_aggregate = color_m_0;

	float3 normal_m_0 = material_0.Normal * blend_weights.x;
	normal_m_0 = -blend_weights.x >= 0 ? float3(0.0, 0.0, 0.0) : normal_m_0;
	normal_aggregate = normal_m_0;

#endif
#ifndef flag_material_1_off
	MATERIAL_RESULT material_1 = Material_1(texcoord);

	float4 color_m_1 = material_1.Color * blend_weights.y;
	color_m_1 = -blend_weights.y >= 0 ? float4(0.0, 0.0, 0.0, 0.0) : color_m_1;
	color_aggregate += color_m_1;

	float3 normal_m_1 = material_1.Normal * blend_weights.y;
	normal_m_1 = -blend_weights.y >= 0 ? float3(0.0, 0.0, 0.0) : normal_m_1;
	normal_aggregate += normal_m_1;

#endif
#ifndef flag_material_2_off
	MATERIAL_RESULT material_2 = Material_2(texcoord);

	float4 color_m_2 = material_2.Color * blend_weights.z;
	color_m_2 = -blend_weights.z >= 0 ? float4(0.0, 0.0, 0.0, 0.0) : color_m_2;
	color_aggregate += color_m_2;

	float3 normal_m_2 = material_2.Normal * blend_weights.z;
	normal_m_2 = -blend_weights.z >= 0 ? float3(0.0, 0.0, 0.0) : normal_m_2;
	normal_aggregate += normal_m_2;

#endif
#ifndef flag_material_3_off
	MATERIAL_RESULT material_3 = Material_3(texcoord);

	float4 color_m_3 = material_3.Color * blend_weights.w;
	color_m_3 = -blend_weights.w >= 0 ? float4(0.0, 0.0, 0.0, 0.0) : color_m_3;
	color_aggregate += color_m_3;

	float3 normal_m_3 = material_3.Normal * blend_weights.w;
	normal_m_3 = -blend_weights.w >= 0 ? float3(0.0, 0.0, 0.0) : normal_m_3;
	normal_aggregate += normal_m_3;

#endif

	color_aggregate.xyz = bungie_color_processing_terrain(color_aggregate.xyz);

	normal_aggregate = normalize(normal_aggregate);

	PS_OUTPUT output;

	output.Diffuse = color_aggregate;
	output.Normal = float4(normal_export(normal_transform(tangentspace_x, tangentspace_y, tangentspace_z, normal_aggregate)), 1.0);
	output.Unknown = unknown.xxxx;

	return output;
}
