#include "../helpers.hlsl"

#ifdef flag_albedo_diffuseonly

float4 albedo_diffuseonly(float2 texcoord)
{
	float2 base_map_texcoord = texcoord;

	float4 base_map_sample = tex2D(base_map, base_map_texcoord);

	return base_map_sample;
}

#endif
