using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using TagTool.Shaders;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "render_method_option", Tag = "rmop", Size = 0xC)]
    public class RenderMethodOption : TagStructure
    {
        public List<OptionBlock> Options;

        [TagStructure(Size = 0x48, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x54, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x48, MinVersion = CacheVersion.HaloOnline106708)]
        public class OptionBlock : TagStructure
        {
            public StringId Name;
            public OptionDataType Type;
            public RenderMethodExtern RenderMethodExtern;
            public CachedTag DefaultSamplerBitmap;

            public float DefaultFloatArgument;

            public int LayerCount;      // 0, 1, or 4 (found by listing all rmop with value >0 and noticed layers_of_4 had this value as 4)
            public short Unknown5;
            public short UnknownDepthRelated;

            public short Unknown6;      // 0, 1 or 3, probably an enum, confirmed short value
            public short Unknown6_1;    //always 0
            public ArgbColor DefaultColor;

            public float DetailMapTilingFactor;

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