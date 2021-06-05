using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "compute_shader", Tag = "cmpu", Size = 0x20)]
    public class ComputeShader : TagStructure
    {
        public uint Unknown0;

        public TagBlock<UnknownBlock2> Unknown4;

        public uint Unknown10;

        public TagBlock<ComputeShaderBlock> ComputeShaders;

        [TagStructure(Size = 0x2)]
        public class UnknownBlock2 : TagStructure
        {
            public short Unknown0;
        }

        [TagStructure(Size = 0x90)]
        public class ComputeShaderBlock : TagStructure
        {
            public TagData Unknown0;
            public TagData Unknown14;
            public TagData Unknown28;
            public TagBlock<UnknownBlock> Unknown3C;

            public uint Unknown48;
            public uint Unknown4C;
            public uint Unknown50;

            public TagBlock<UnknownBlock> Unknown54;

            public uint Unknown60;
            public uint Unknown64;
            public uint Unknown68;

            public TagBlock<UnknownBlock> Unknown6C;


            public uint Unknown78;
            public uint Unknown7C;
            public uint Unknown80;

            public uint Unknown84;
            public uint Unknown88;
            public uint Unknown8C;


            [TagStructure(Size = 0x8)]
            public class UnknownBlock : TagStructure
            {
                public uint Unknown0;
                public uint Unknown4;
            }
        }
    }
}
