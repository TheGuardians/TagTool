using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "decal_system", Tag = "decs", Size = 0x24, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "decal_system", Tag = "decs", Size = 0x2C, MinVersion = CacheVersion.HaloOnline106708)]
    public class DecalSystem
    {
        public uint Unknown;
        public float Unknown2;
        public float Unknown3;
        public float Unknown4;
        public float Unknown5;
        public List<DecalSystemBlock> DecalSystem2;
        public float Unknown6;

        [TagField(Padding = true, Length = 8, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;

        [TagStructure(Size = 0x74)]
        public class DecalSystemBlock
        {
            public StringId Name;
            public uint Unknown;
            public RenderMethod RenderMethod;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;
            public float Unknown6;
            public float Unknown7;
            public float Unknown8;
            public float Unknown9;
            public float Unknown10;
            public float Unknown11;
            public float Unknown12;
        }
    }
}
