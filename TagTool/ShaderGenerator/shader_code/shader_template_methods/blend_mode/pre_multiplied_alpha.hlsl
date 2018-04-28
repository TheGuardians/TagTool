#include "../../helpers.hlsl"

#ifdef flag_blend_mode_pre_multiplied_alpha

float4 blend_mode_pre_multiplied_alpha(float4 input)
{
    return input;
}

#endif
