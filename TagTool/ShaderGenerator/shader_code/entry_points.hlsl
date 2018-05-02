#include "parameters.hlsl"
#include "default_methods.hlsl"

#include "utilities.hlsl"

#include "albedo.hlsl"
#include "blend.hlsl"
#include "bump_mapping.hlsl"
#ifdef terrain_template
#include "terrain.hlsl"
#endif

struct VS_OUTPUT
{
	float4 TexCoord : TEXCOORD;
	float4 TexCoord1 : TEXCOORD1;
	float4 TexCoord2 : TEXCOORD2;
	float4 TexCoord3 : TEXCOORD3;
	float4 TexCoord4 : TEXCOORD4;
	float4 TexCoord5 : TEXCOORD5;
	float4 TexCoord6 : TEXCOORD6;
	float4 TexCoord7 : TEXCOORD7;
	float4 TexCoord8 : TEXCOORD8;
	float4 TexCoord9 : TEXCOORD9;
	float4 TexCoord10 : TEXCOORD10;
	float4 TexCoord11 : TEXCOORD11;
	float4 TexCoord12 : TEXCOORD12;
	float4 TexCoord13 : TEXCOORD13;
	float4 TexCoord14 : TEXCOORD14;
	float4 TexCoord15 : TEXCOORD15;
};

#ifdef entry_default_ps

struct DEFAULT_PS_INPUT
{
	float4 Color0 : COLOR0;
	float4 Color1 : COLOR1;
	float4 TexCoord : TEXCOORD;
};

struct DEFAULT_PS_OUTPUT
{
	float4 Color0 : COLOR0;
	float4 Color1 : COLOR1;
	float4 Unknown : COLOR2;
};

DEFAULT_PS_OUTPUT default_ps(DEFAULT_PS_INPUT input) : COLOR
{
	float2 texcoord = input.TexCoord.xy;
	float4 vertex_color_scale = input.Color0;
	float4 vertex_color_add = input.Color1;

	float4 albedo = Albedo(texcoord);

	float3 color = albedo.xyz * vertex_color_scale.xyz + vertex_color_add.xyz;
	float alpha = albedo.w * vertex_color_scale.w;

	DEFAULT_PS_OUTPUT output;

	output.Color0 = Black_Point(float4(color, alpha * g_exposure.w));
	output.Color1 = Black_Point(float4(color / g_exposure.y, alpha * g_exposure.z));
	output.Unknown = float4(0, 0, 0, 0);

	return output;
}

#endif
#ifdef entry_albedo_ps 

struct ALBEDO_PS_OUTPUT
{
	float4 Diffuse;
#ifndef flag_bump_mapping_leave
	float4 Normal;
	float4 Unknown;
#endif
};

ALBEDO_PS_OUTPUT albedo_ps(VS_OUTPUT input) : COLOR
{
	float2 texcoord = input.TexCoord.xy;
	float2 texcoord_tiled = input.TexCoord.zw;
	float3 tangentspace_x = input.TexCoord3.xyz;
	float3 tangentspace_y = input.TexCoord2.xyz;
	float3 tangentspace_z = input.TexCoord1.xyz;
	float3 unknown = input.TexCoord1.w;

	float4 albedo = Albedo(texcoord);
	float3 normal = Bump_Mapping(tangentspace_x, tangentspace_y, tangentspace_z, texcoord);

	float3 color = bungie_color_processing(albedo.xyz);
	float alpha = albedo.w;

	ALBEDO_PS_OUTPUT output;
	output.Diffuse = Blend_Mode(float4(color, albedo.w));
#ifndef flag_bump_mapping_leave
	output.Normal = Blend_Mode(float4(signed_normalize(normal), albedo.w));
	output.Unknown = unknown.xxxx;
#endif
	return output;
}

#endif

float4 static_default_ps() : COLOR
{
	return float4(_debug_color);
}

float4 static_per_pixel_ps() : COLOR
{
	return float4(_debug_color);
}

float4 static_per_vertex_ps() : COLOR
{
	return float4(_debug_color);
}

float4 static_sh_ps() : COLOR
{
	return float4(_debug_color);
}

float4 static_prt_ambient_ps() : COLOR
{
	return float4(_debug_color);
}

float4 static_prt_linear_ps() : COLOR
{
	return float4(_debug_color);
}

float4 static_prt_quadratic_ps() : COLOR
{
	return float4(_debug_color);
}

float4 dynamic_light_ps() : COLOR
{
	return float4(_debug_color);
}

float4 shadow_generate_ps() : COLOR
{
	return float4(_debug_color);
}

float4 shadow_apply_ps() : COLOR
{
	return float4(_debug_color);
}

float4 active_camo_ps() : COLOR
{
	return float4(_debug_color);
}

float4 lightmap_debug_mode_ps() : COLOR
{
	return float4(_debug_color);
}

float4 static_per_vertex_color_ps() : COLOR
{
	return float4(_debug_color);
}

/**
* This entry point appears to be unused
*/
float4 water_tessellation_ps() : COLOR
{
	return float4(0, 1, 2, 3);
}

float4 water_shading_ps() : COLOR
{
	return float4(_debug_color);
}

float4 dynamic_light_cine_ps() : COLOR
{
	return float4(_debug_color);
}

float4 z_only_ps() : COLOR
{
	return float4(_debug_color);
}

float4 sfx_distort_ps() : COLOR
{
	return float4(_debug_color);
}
