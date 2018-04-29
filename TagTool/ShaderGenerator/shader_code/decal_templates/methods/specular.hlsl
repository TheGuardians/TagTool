#include "../../parameters.hlsl"

#ifdef flag_specular_leave

float4 specular_leave(float4 input)
{
	return input;
}

#endif



#ifdef flag_specular_modulate

float4 specular_modulate(float4 input)
{
#ifndef flag_blend_mode_alpha_blend
	return float4(input.xyz, input.w * fade.x);
#endif

	return input; // already modulated
}

#endif
