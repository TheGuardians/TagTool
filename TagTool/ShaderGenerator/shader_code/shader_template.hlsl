#include "shader_template_defs.hlsl"
#include "shader_code.hlsl"

float4 main(float texCoords : TEXCOORD) : COLOR
{
    return Albedo();
}