#ifndef __UTILITIES
#define __UTILITIES

/**
* converts a value between -1 to 1 to a value between 0 and 1
*/
#define signed_normalize(value) (value * 0.5 + 0.5)

#ifndef param_debug_tint
static const float4 debug_tint = float4(0, 0, 0, 0);
#endif

#ifdef terrain_template

float3 bungie_color_processing(float3 color)
{
	float4 c0 = float4(2.00787401, -1.00787401, 0, 1);
	float4 c1 = float4(4.59478998, 0.00313080009, 12.9200001, 0.416666657);
	float4 c2 = float4(1.05499995, -0.0549999997, 0.5, 0);

	float4 r2, r3, r5;

	r2.xyz = color;

	r3.x = c1.x; //mov r3.x, c1.x
	r2.w = r3.x * global_albedo_tint.x; //mul r2.w, r3.x, c60.x
	r3.xyz = r2.xyz * r2.w; //mul r3.xyz, r2.w, r2
	r2.xyz = r2.xyz * -r2.w + debug_tint.xyz; //mad r2.xyz, r2, -r2.w, c58
	r2.xyz = r2.xyz * debug_tint.w + r3.xyz; //mad r2.xyz, c58.w, r2, r3
											 //log r3.x, r2.x
											 //log r3.y, r2.y
											 //log r3.z, r2.z
	r3.xyz = log(r2.xyz);
	r3.xyz = r3.xyz * c1.w; //mul r3.xyz, r3, c1.w
							//exp r5.x, r3.x
							//exp r5.y, r3.y
							//exp r5.z, r3.z
	r5.xyz = exp(r3.xyz);
	r3.xyz = r5.xyz * c2.x + c2.y; //mad r3.xyz, r5, c2.x, c2.y
	r5.xyz = -r2.xyz + c1.y; //add r5.xyz, -r2, c1.y
	r2.xyz = r2.xyz * c1.z; //mul r2.xyz, r2, c1.z
							//cmp oC0.xyz, r5, r2, r3
	float3 result = r5.xyz >= 0 ? r2.xyz : r3.xyz;

	return result;
}

#else

float3 bungie_color_processing(float3 color)
{
	float4 r0 = float4(color, 0);
	float4 r1 = float4(0, 0, 0, 0);
	float4 r2 = float4(0, 0, 0, 0);

	// BEGIN RETARDED CODE
	r1.xyz = color.xyz * 4.59478998;
	r1.w = 4.59478998;
	r0.xyz = r0.xyz * -r1.xyz + debug_tint.xyz;
	r0.xyz = debug_tint.www * r0.xyz + r1.xyz;
	r1.xyz = log(r0.xyz);
	r1.xyz = r1.xyz * (5.0 / 12.0); // 5/12
	r2.xyz = exp(r1.xyz);
	r1.xyz = r2.xyz * 1.055 - 0.055;
	r2.xyz = (-r0.xyz) * 12.92;
	r0.xyz = r0.xyz * 12.92;
	// END RETARDED CODE

	return r2.xyz >= 0 ? r0.xyz : r1.xyz;
}

#endif

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

#endif