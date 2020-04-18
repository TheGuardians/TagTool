using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloShaderGenerator;
using HaloShaderGenerator.Globals;

namespace TagTool.Shaders.ShaderGenerator
{
    /// <summary>
    /// Wrapper class for the Shader Generator Project, the project itself should only be referenced here
    /// </summary>
    public static class ShaderGenerator
    {
        public static ShaderGeneratorResult GenerateGlobalVertexShaderShader(Geometry.VertexType vertex, EntryPoint entryPoint)
        {
            // poor man's way of dealing with circular reference
            byte[] bytecode = HaloShaderGenerator.Shader.ShartedVertexShaderGenerator.GenerateSharedVertexShader((VertexType)vertex, (ShaderStage)entryPoint);
            return new ShaderGeneratorResult(bytecode);
        }
    }
}
