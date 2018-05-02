#include "../../helpers.hlsl"

#ifdef flag_

float4 albedo_diffuseonly(float2 texcoord)
{
	float4 base_map_sample = tex2D(base_map, texcoord);

	return base_map_sample;
}

#endif

#ifdef flag_albedo_palettized

float4 albedo_palettized(float2 texcoord)
{
	return float4(1, 0, 0, 1); // Not implemented
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

#ifdef flag_albedo_diffuse_plus_alpha

float4 albedo_diffuse_plus_alpha(float2 texcoord)
{
	float4 base_map_sample = tex2D(base_map, texcoord);
	float4 alpha_map_sample = tex2D(alpha_map, texcoord);

	return float4(base_map_sample.xyz, alpha_map_sample.w);
}

#endif

#ifdef flag_albedo_emblem_change_color

float4 albedo_emblem_change_color(float2 texcoord)
{
	return float4(1, 0, 0, 1); // Not implemented
}

#endif

#ifdef flag_albedo_change_color

float4 albedo_change_color(float2 texcoord)
{
	// NOTE: I have absolutely no idea if this is correct.
	// I haven't been able to find a good shader sample to
	// base this code from yet as they're all being optimized
	// with other junk

	float4 change_color_map_sample = tex2D(change_color_map, texcoord);
	float3 color = change_color_map_sample.xyz;
	float3 inverted_color = 1.0 - color;

	float3 red = primary_change_color.xyz * change_color_map_sample.x + inverted_color.x;
	float3 green = secondary_change_color.xyz * change_color_map_sample.y + inverted_color.y;
	float3 blue = tertiary_change_color.xyz * change_color_map_sample.z + inverted_color.z;
	float alpha = change_color_map_sample.w;

	float3 aggregate = red * green * blue;

	return float4(aggregate, change_color_map_sample.w);
}

#endif

#ifdef flag_albedo_diffuse_plus_alpha_mask

float4 albedo_diffuse_plus_alpha_mask(float2 texcoord)
{
	return float4(1, 0, 0, 1); // Not implemented
}

#endif

#ifdef flag_albedo_palettized_plus_alpha_mask

float4 albedo_palettized_plus_alpha_mask(float2 texcoord)
{
	return float4(1, 0, 0, 1); // Not implemented
}

#endif

#ifdef flag_albedo_vector_alpha

float4 albedo_vector_alpha(float2 texcoord)
{
	return float4(1, 0, 0, 1); // Not implemented
}

#endif

#ifdef flag_albedo_vector_alpha_drop_shadow

float4 albedo_vector_alpha_drop_shadow(float2 texcoord)
{
	return float4(1, 0, 0, 1); // Not implemented
}

#endif

