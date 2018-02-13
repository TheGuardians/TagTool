using TagTool.Common;
using TagTool.Serialization;
using TagTool.Shaders;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "global_vertex_shader", Tag = "glvs", Size = 0x1C)]
    public class GlobalVertexShader
    {
        public List<VertexTypeShaders> VertexTypes;
        public uint Unknown2;
        public List<Shader> Shaders;

        [TagStructure(Size = 0xC)]
        public class VertexTypeShaders
        {
            public List<DrawMode> DrawModes;

            [TagStructure(Size = 0x10)]
            public class DrawMode
            {
                public uint Unknown;
                public uint Unknown2;
                public uint Unknown3;
                public int ShaderIndex;
            }
        }

        [TagStructure(Size = 0x50)]
        public class Shader
        {
            public byte[] Unknown;
            public byte[] Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;
            public List<ParameterBlock> Parameters;
            public uint Unknown8;
            public uint Unknown9;
            public VertexShaderReference ShaderReference;

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
