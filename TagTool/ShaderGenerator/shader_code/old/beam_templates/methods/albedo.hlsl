#include "../../helpers.hlsl"

#ifdef flag_albedo_diffuseonly

float4 albedo_diffuseonly(float2 texcoord)
{
	float4 base_map_sample = tex2D(base_map, texcoord);
	return base_map_sample;
}

#endif

#ifdef flag_albedo_palettized

float4 albedo_palettized(float2 texcoord)
{
	return _debug_color;
}

#endif

#ifdef flag_albedo_palettized_plus_alpha

float4 albedo_palettized_plus_alpha(float2 texcoord)
{
	float4 base_map_sample = tex2D(base_map, texcoord);
	float2 pallete_location = float2(1.0, 0.0) * base_map_sample.x; // assume the base_map is greyscale, remove Y component

	float4 palette_map_sample = tex2D(palette, pallete_location);

	float4 alpha_map_sample = tex2D(alpha_map, texcoord);

	return float4(palette_map_sample.xyz, alpha_map_sample.w);
}

#endif