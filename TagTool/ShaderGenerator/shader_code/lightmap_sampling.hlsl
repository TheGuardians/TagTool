
float3 sample_lightprobe_texture_array(sampler3D lightproe, float2 texcoord)
{
	float3 lightmap_illumination = float3(1.0, 1.0, 1.0);
	if (k_is_lightmap_exist)
	{
		float3 texcoord0 = float3(texcoord, 0.0625);
		float4 sample0 = tex3D(lightprobe_texture_array, texcoord0);

		float3 texcoord1 = float3(texcoord, 0.1875);
		float4 sample1 = tex3D(lightprobe_texture_array, texcoord1);

		float3 color_aggregate = sample0.xyz + sample1.xyz;
		float3 alpha_aggregate = sample0.w * sample1.w * p_lightmap_compress_constant_0.x;

		color_aggregate = color_aggregate * 2.0 - 2.0;
		color_aggregate = saturate(color_aggregate * alpha_aggregate);

		lightmap_illumination = color_aggregate;
	}

	return lightmap_illumination;
}
