using TagTool.Common;
using TagTool.Serialization;
using TagTool.Shaders;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "global_pixel_shader", Tag = "glps", Size = 0x1C)]
    public class GlobalPixelShader
    {
        public List<DrawMode> DrawModes;
        public uint Unknown2;
        public List<Shader> Shaders;

        [TagStructure(Size = 0x10)]
        public class DrawMode
        {
            public List<UnknownBlock2> Unknown;
            public uint Unknown2;

            [TagStructure(Size = 0x10)]
            public class UnknownBlock2
            {
                public uint Unknown;
                public List<UnknownBlock> Unknown2;

                [TagStructure(Size = 0x4)]
                public class UnknownBlock
                {
                    public uint Unknown;
                }
            }
        }

        [TagStructure(Size = 0x50)]
        public class Shader
        {
            public byte[] Unknown;
            public byte[] ByteCode;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;
            public List<ParameterBlock> Parameters;
            public uint Unknown8;
            public uint Unknown9;
            public PixelShaderReference ShaderReference;

            [TagStructure(Size = 0x8)]
            public class ParameterBlock
            {
                public StringId ParameterName;
                public short RegisterIndex;
                public byte RegisterCount;
                public RType RegisterType;

                public enum RType : byte
                {
                    Boolean = 0,
                    Integer = 1,
                    Float = 2,
                    Sampler = 3
                }
            }
        }
    }
}
