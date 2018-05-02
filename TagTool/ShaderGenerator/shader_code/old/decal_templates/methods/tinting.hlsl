#ifdef flag_tinting_none

float4 tinting_none(float4 color)
{
	return color;
}

#endif

#ifdef flag_tinting_unmodulated

float4 tinting_unmodulated(float4 input)
{
	return float4(input.xyz * tint_color.xyz * intensity.z, input.w);
}

#endif

#ifdef flag_tinting_partially_modulated

float4 tinting_partially_modulated(float4 input)
{
	return input; // Not implemented
}

#endif

#ifdef flag_tinting_fully_modulated

float4 tinting_fully_modulated(float4 input)
{
	float3 color = input.xyz;
	float magnitude = length(color);
	magnitude *= sqrt(1.0 / 3.0);

	float3 tinted_color = lerp(magnitude, 1.0, tint_color.xyz);

	return float4(tinted_color * intensity.z, input.w);
}

#endif
