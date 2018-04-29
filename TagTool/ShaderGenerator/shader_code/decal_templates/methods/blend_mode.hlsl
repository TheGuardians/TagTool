#include "../parameters.hlsl"

/*
** These blending parameters take into account for the fade parameter
*/

float4 blend_mode_opaque(float4 input)
{
	return input;
}

float4 blend_mode_additive(float4 input)
{
	return input;
}

float4 blend_mode_multiply(float4 input)
{
	float3 xyz = (input.xyz - 1.0) * fade.x + 1.0;
	return float4(xyz, input.w);
}

float4 blend_mode_alpha_blend(float4 input)
{
	return float4(input.xyz, input.w * fade.x);
}

float4 blend_mode_double_multiply(float4 input)
{
	float w = input.w * fade.x;
	float3 xyz = (input.xyz - 0.5) * w + 0.5;
	return float4(xyz, w);
}

float4 blend_mode_maximum(float4 input)
{
	return input;
}

float4 blend_mode_multiply_add(float4 input)
{
	return input;
}

float4 blend_mode_add_src_times_dstalpha(float4 input)
{
	return input;
}

float4 blend_mode_add_src_times_srcalpha(float4 input)
{
	return input;
}

float4 blend_mode_inv_alpha_blend(float4 input)
{
	return input;
}

float4 blend_mode_pre_multiplied_alpha(float4 input)
{
	return float4(input.xyz * input.w, input.w);
}