#include "default_definitions.hlsl"

float4 albedo_constant_color()
{
    return float4(1, 0, 0, 1);
}

float4 main(float texCoords : TEXCOORD) : COLOR
{
    return Albedo;
}