using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Shaders;

namespace TagTool.ShaderGenerator.Types
{
    public class ShaderGeneratorResult
    {
        public byte[] ByteCode;
        public List<ShaderParameter> Parameters;
    }
}
