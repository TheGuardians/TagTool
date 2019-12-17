using TagTool.Cache;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "effect_globals", Tag = "effg", Size = 0xC)]
    public class EffectGlobals : TagStructure
	{
        public List<UnknownBlock> Unknown;

        [TagStructure(Size = 0x14)]
        public class UnknownBlock : TagStructure
		{
            public uint Unknown;
            public uint Unknown2;
            public List<UnknownBlock2> Unknown3;

            [TagStructure(Size = 0x10)]
            public class UnknownBlock2 : TagStructure
			{
                public uint Unknown;
                public uint Unknown2;
                public uint Unknown3;
                public uint Unknown4;
            }
        }
    }
}
