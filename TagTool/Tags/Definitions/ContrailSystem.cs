using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "contrail_system", Tag = "cntl", Size = 0xC)]
    public class ContrailSystem : TagStructure
	{
        public List<ContrailSystemBlock> Contrail;

        [TagStructure(Size = 0x26C, Align = 0x10, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x294, Align = 0x10, MinVersion = CacheVersion.HaloReach)]
        public class ContrailSystemBlock : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;
            public float Unknown1;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;
            public float Unknown6;
            public float Unknown7;
            public float Unknown8;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float UnknownReach;

            public TagMapping ShaderFunction1;
            public TagMapping ShaderFunction2;
            public TagMapping ShaderFunction3;

            public uint Unknown9;
            public uint Unknown10;
            public uint Unknown11;
            public uint Unknown12;
            public uint Unknown13;
            public uint Unknown14;

            public TagMapping ShaderFunction4;
            public TagMapping ShaderFunction5;

            public uint Unknown15;
            public uint Unknown16;
            public uint Unknown17;
            public uint Unknown18;

            public TagMapping ShaderFunction6;
            public TagMapping ShaderFunction7;

            public ushort Unknown19_Flags;
            public ushort Unknown19_Unused;

            public RenderMethod RenderMethod;

            public float TileY;
            public float TileX;
            public float ScrollSpeedY;
            public float ScrollSpeedX;

            public TagMapping ShaderFunction8;
            public TagMapping ShaderFunction9;
            public TagMapping ShaderFunction10;
            public TagMapping ShaderFunction11;
            public TagMapping ShaderFunction12;
            public TagMapping ShaderFunction13;

            public int Unknown20;
            public int Unknown21;

            public List<UnknownBlock> Unknown22;
            public List<CompiledFunction> CompiledFunctions;
            public List<CompiledColorFunction> CompiledColorFunctions;

            [TagStructure(Size = 0x10, Align = 0x10)]
            public class UnknownBlock : TagStructure
			{
                public uint Unknown;
                public uint Unknown2;
                public uint Unknown3;
                public uint Unknown4;
            }

            [TagStructure(Size = 0x40, Align = 0x10)]
            public class CompiledFunction : TagStructure
			{
                public uint Unknown;
                public uint Unknown2;
                public uint Unknown3;
                public uint Unknown4;
                public uint Unknown5;
                public uint Unknown6;
                public uint Unknown7;
                public uint Unknown8;
                public uint Unknown9;
                public uint Unknown10;
                public uint Unknown11;
                public uint Unknown12;
                public uint Unknown13;
                public uint Unknown14;
                public uint Unknown15;
                public uint Unknown16;
            }

            [TagStructure(Size = 0x10, Align = 0x10)]
            public class CompiledColorFunction : TagStructure
			{
                public float Red;
                public float Green;
                public float Blue;
                public float Magnitude;
            }
        }
    }
}
