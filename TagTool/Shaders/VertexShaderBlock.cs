using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Serialization;

namespace TagTool.Shaders
{
    [TagStructure(Size = 0x50)]
    public class VertexShaderBlock
    {
        public byte[] Unknown;
        public byte[] PCShaderBytecode;
        public List<ShaderParameter> XboxParameters;
        public uint Unknown6;
        public List<ShaderParameter> PCParameters;
        public uint Unknown8;
        public uint Unknown9;
        public VertexShaderReference XboxShaderReference;
    }
}
