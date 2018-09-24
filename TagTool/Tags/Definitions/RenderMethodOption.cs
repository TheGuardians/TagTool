using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "render_method_option", Tag = "rmop", Size = 0xC, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "render_method_option", Tag = "rmop", Size = 0x18, MinVersion = CacheVersion.HaloOnline106708)]
    public class RenderMethodOption : TagStructure
	{
        public List<OptionBlock> Options;

        [TagField(Padding = true, Length = 12, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;

        [TagStructure(Size = 0x48, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x54, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x48, MinVersion = CacheVersion.HaloOnline106708)]
        public class OptionBlock : TagStructure
		{
            public StringId Name;
            public OptionDataType Type;
            public uint Unknown2;
            public CachedTagInstance Bitmap;

            public float Unknown3;

            public uint Unknown4;
            public uint Unknown5;

            public short Unknown6;
            public short Unknown7;
            public short Unknown8;
            public short Unknown9;
            
            public float Unknown10;

            public uint Unknown11;
            public uint Unknown12;
            public uint Unknown13;
            public uint Unknown14;
            public uint Unknown15;

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public uint Unknown16;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public uint Unknown17;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public uint Unknown18;

            public enum OptionDataType : uint
            {
                Sampler = 0,
                Float4 = 1,
                Float = 2,
                Integer = 3,
                Boolean = 4,
                IntegerColor = 5
            }
        }
    }
}