#include "template_default_defs.hlsl"
#include "template_includes.hlsl"
#include "parameters.hlsl"

float4 main(float texCoords : TEXCOORD) : COLOR
{
    return Albedo();
}