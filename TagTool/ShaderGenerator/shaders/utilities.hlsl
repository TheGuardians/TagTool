/**
 * converts a value between -1 to 1 to a value between 0 and 1
*/
#define signed_normalize(value) (value * 0.5 + 0.5;)

float2 sample_bump_map_2d(sampler bump_map, float2 texcoord)
{
	float4 bump_map_sample = tex2D(bump_map, texcoord);

	float scale = 255.0 / 127.0;
	float offset = 128.0 / 127.0;

	return bump_map_sample.xy * scale + offset;
}

/**
 * reconstructs the positive z component of a normal using the X and Y components
 * @result z = 1.0 - sqrt(x^2 + y^2)
*/
float reconstruct_normal_z(float2 normal)
{
	return 1.0 - sqrt(dot(normal, normal));
}

/*
 * reconstructs a normal from only the X and Y components
 * @param normal the X and Y component of the normal
 * @result (x, y, 1.0 - sqrt(x^2 + y^2))
*/
float3 reconstruct_normal(float2 normal)
{
	return float3(normal, reconstruct_normal_z(normal));
}

float3 sample_normal_2d(sampler bump_map, float2 texcoord)
{
	float2 bump_sample = sample_bump_map_2d(bump_map, texcoord);

	return reconstruct_normal(bump_sample);
}