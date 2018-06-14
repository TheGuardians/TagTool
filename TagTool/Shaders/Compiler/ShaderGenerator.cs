using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Shaders.Compiler
{
    class ShaderGenerator
    {
        public class IncludeExt : Include
        {
            protected override int Open(D3D.INCLUDE_TYPE includeType, string filename, ref string result)
            {
                return 0;
            }
        }

        public ShaderGenerator()
        {
            IncludeExt include = new IncludeExt();

            D3D.SHADER_MACRO[] macros = { new D3D.SHADER_MACRO { Name = "test\0", Definition = "1.0\0" } };
            var test_data = D3DCompiler.CompileFromFile("test.hlsl", "main", "ps_3_0", macros, 0, 0, include);
        }
    }
}
