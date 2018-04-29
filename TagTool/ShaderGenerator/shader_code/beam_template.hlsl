#define beam_template
#define _debug_color float4(255, 255, 85, 255) / 255;
#include "parameters.hlsl"

struct VS_OUTPUT
{
	float4 TexCoord : TEXCOORD;
	float4 TexCoord1 : TEXCOORD1;
	float4 TexCoord2 : TEXCOORD2;
	float4 TexCoord3 : TEXCOORD3;
	float4 TexCoord4 : TEXCOORD4;
};

float4 main(VS_OUTPUT input) : COLOR
{
	return _debug_color;
}