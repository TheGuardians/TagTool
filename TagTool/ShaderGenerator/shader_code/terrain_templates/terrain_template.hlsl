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

float3 bungie_color_processing_terrain(float3 color)
{
	float4 c0 = float4(2.00787401, -1.00787401, 0, 1);
	float4 c1 = float4(4.59478998, 0.00313080009, 12.9200001, 0.416666657);
	float4 c2 = float4(1.05499995, -0.0549999997, 0.5, 0);

	float4 r2, r3, r5;

	r2.xyz = color;

	r3.x = c1.x; //mov r3.x, c1.x
	r2.w = r3.x * global_albedo_tint.x; //mul r2.w, r3.x, c60.x
	r3.xyz = r2.xyz * r2.w; //mul r3.xyz, r2.w, r2
	r2.xyz = r2.xyz * -r2.w + debug_tint.xyz; //mad r2.xyz, r2, -r2.w, c58
	r2.xyz = r2.xyz * debug_tint.w + r3.xyz; //mad r2.xyz, c58.w, r2, r3
											 //log r3.x, r2.x
											 //log r3.y, r2.y
											 //log r3.z, r2.z
	r3.xyz = log(r2.xyz);
	r3.xyz = r3.xyz * c1.w; //mul r3.xyz, r3, c1.w
							//exp r5.x, r3.x
							//exp r5.y, r3.y
							//exp r5.z, r3.z
	r5.xyz = exp(r3.xyz);
	r3.xyz = r5.xyz * c2.x + c2.y; //mad r3.xyz, r5, c2.x, c2.y
	r5.xyz = -r2.xyz + c1.y; //add r5.xyz, -r2, c1.y
	r2.xyz = r2.xyz * c1.z; //mul r2.xyz, r2, c1.z
							//cmp oC0.xyz, r5, r2, r3
	float3 result = r5.xyz >= 0 ? r2.xyz : r3.xyz;

	return result;
}

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
	//#ifndef flag_material_3_off
	MATERIAL_RESULT material_3 = Material_3(texcoord);

	float4 color_m_3 = material_3.Color * blend_weights.w;
	color_m_3 = -blend_weights.w >= 0 ? float4(0.0, 0.0, 0.0, 0.0) : color_m_3;
	color_aggregate += color_m_3;

	float3 normal_m_3 = material_3.Normal * blend_weights.w;
	normal_m_3 = -blend_weights.w >= 0 ? float3(0.0, 0.0, 0.0) : normal_m_3;
	normal_aggregate += normal_m_3;

	//#endif

	color_aggregate.xyz = bungie_color_processing_terrain(color_aggregate.xyz);

	normal_aggregate = normalize(normal_aggregate);

	PS_OUTPUT output;

	output.Diffuse = color_aggregate;
	output.Normal = float4(normal_export(normal_transform(tangentspace_x, tangentspace_y, tangentspace_z, normal_aggregate)), 1.0);
	output.Unknown = unknown.xxxx;

	return output;
}
