using TagTool.Cache;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "breakable_surface", Tag = "bsdt", Size = 0x54)]
    public class BreakableSurface : TagStructure
	{
        public float MaximumVitality;
        public CachedTagInstance Effect;
        public CachedTagInstance Sound;
        public uint Unknown;
        public uint Unknown2;
        public uint Unknown3;
        public uint Unknown4;
        public CachedTagInstance CrackBitmap;
        public CachedTagInstance HoleBitmap;

        [TagStructure(Size = 0x24)]
        public class UnknownBlock : TagStructure
		{
            public uint Unknown;
            public uint Unknown2;
            public byte[] Unknown3;
            public uint Unknown4;
            public uint Unknown5;
        }
    }
}