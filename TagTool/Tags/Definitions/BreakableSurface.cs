using TagTool.Cache;
using TagTool.Serialization;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "breakable_surface", Tag = "bsdt", Size = 0x54, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "breakable_surface", Tag = "bsdt", Size = 0x60, MinVersion = CacheVersion.HaloOnline106708)]
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

        [TagField(Padding = true, Length = 12, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused1;

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