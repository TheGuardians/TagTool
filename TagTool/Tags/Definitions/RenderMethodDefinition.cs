using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "render_method_definition", Size = 0x5C, Tag = "rmdf")]
    public class RenderMethodDefinition
    {
        public CachedTagInstance RenderMethodOptions;
        public List<Method> Methods;
        public List<DrawMode> DrawModes;
        public List<UnknownBlock> Unknown;
        public CachedTagInstance GlobalPixelShader;
        public CachedTagInstance GlobalVertexShader;
        public int Unknown2;
        public int Unknown3;

        [TagField(Padding = true, Length = 4, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;

        [TagStructure(Size = 0x18)]
        public class Method
        {
            public StringId Type;
            public List<ShaderOption> ShaderOptions;
            public StringId Unknown;
            public StringId Unknown2;

            [TagStructure(Size = 0x1C)]
            public class ShaderOption
            {
                public StringId Type;
                public CachedTagInstance Option;
                public StringId Unknown;
                public StringId Unknown2;
            }
        }

        [TagStructure(Size = 0x10)]
        public class DrawMode
        {
            public uint Mode;
            public List<UnknownBlock2> Unknown2;

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

        [TagStructure(Size = 0x10)]
        public class UnknownBlock
        {
            public short Unknown1;
            public short Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
        }
    }
}