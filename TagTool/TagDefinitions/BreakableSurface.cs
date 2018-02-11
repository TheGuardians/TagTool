using TagTool.Cache;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "breakable_surface", Tag = "bsdt", Size = 0xA0, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "breakable_surface", Tag = "bsdt", Size = 0x54, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "breakable_surface", Tag = "bsdt", Size = 0x60, MinVersion = CacheVersion.HaloOnline106708)]
    public class BreakableSurface
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

        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public uint Unknown5;
        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public uint Unknown6;
        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public uint Unknown7;
        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public uint Unknown8;
        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public uint Unknown9;
        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public uint Unknown10;
        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public uint Unknown11;
        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public uint Unknown12;
        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public uint Unknown13;
        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public List<Unknown0Block> Unknown0;
        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public uint Unknown14;
        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public uint Unknown15;
        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public uint Unknown16;
        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public uint Unknown17;
        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public List<Unknown1Block> Unknown1;
        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public uint Unknown18;
        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public uint Unknown19;

        [TagField(Padding = true, Length = 12, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused1;

        [TagStructure(Size = 0x24)]
        public class Unknown0Block
        {
            public uint Unknown;
            public uint Unknown2;
            public byte[] Unknown3;
            public uint Unknown4;
            public uint Unknown5;
        }

        [TagStructure(Size = 0x24)]
        public class Unknown1Block
        {
            public uint Unknown;
            public uint Unknown2;
            public byte[] Unknown3;
            public uint Unknown4;
            public uint Unknown5;
        }
    }
}
