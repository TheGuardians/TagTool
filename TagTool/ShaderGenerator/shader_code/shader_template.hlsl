#include "shader_template_defs.hlsl"
#include "shader_code.hlsl"
#include "uniforms.hlsl"
#include "test.hlsl"

float4 main(float texCoords : TEXCOORD) : COLOR
{
    return Albedo();
}