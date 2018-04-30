#include "../../helpers.hlsl"

#ifdef flag_black_point_off

float4 black_point_off(float4 input)
{
	return input;
}

#endif

#ifdef flag_black_point_on

float4 black_point_on(float4 input)
{
	return input; // Not implemented
}

#endif