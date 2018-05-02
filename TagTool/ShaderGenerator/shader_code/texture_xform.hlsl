#ifndef __TEXTURE_XFORM
#define __TEXTURE_XFORM

#define default_xform float4(1.0, 1.0, 0.0, 0.0)

float2 apply_xform(float2 texcoord, float4 xform)
{
	return texcoord * xform.xy + xform.zw;
}

#endif