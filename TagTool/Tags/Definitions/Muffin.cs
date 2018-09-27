using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "muffin", Tag = "mffn", Size = 0x38)]
    public class Muffin : TagStructure
	{
        public CachedTagInstance RenderModel;
        public uint Unknown;
        public uint Unknown2;
        public uint Unknown3;
        public int Unknown4;
        public List<LocationsBlock> Locations;
        public List<UnknownBlock> Unknown5;

        [TagStructure(Size = 0x8)]
        public class LocationsBlock : TagStructure
		{
            public StringId Name;
            public short Unknown;
            public short Unknown2;
        }

        [TagStructure(Size = 0x70)]
        public class UnknownBlock : TagStructure
		{
            public short Unknown;
            public short Unknown2;
            public uint Unknown3;
            public float Unknown4;
            public float Unknown5;
            public float Unknown6;
            public TagFunction Unknown7 = new TagFunction { Data = new byte[0] };
            public float Unknown8;
            public TagFunction Unknown9 = new TagFunction { Data = new byte[0] };
            public float Unknown10;
            public float Unknown11;
            public float Unknown12;
            public float Unknown13;
            public float Unknown14;
            public float Unknown15;
            public float Unknown16;
            public float Unknown17;
            public CachedTagInstance Effect;
        }
    }
}