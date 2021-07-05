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
        [TagStructure(Size = 0x48, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x60, MinVersion = CacheVersion.HaloReach)]
        public class OptionBlock : TagStructure
        {
            public StringId Name;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public uint Unknown;
            public OptionDataType Type;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public RenderMethodExtern RenderMethodExtern;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public RenderMethodExternReach RenderMethodExternReach;

            public CachedTag DefaultSamplerBitmap;
            public float DefaultFloatArgument;
            public uint DefaultIntBoolArgument;

            public short Flags;
            public DefaultFilterModeValue DefaultFilterMode;
            public DefaultAddressModeValue DefaultAddressMode;
            public short AnisotropyAmount;

            public ArgbColor DefaultColor;
            public float DefaultBitmapScale;

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
            [TagField(MinVersion = CacheVersion.HaloReach, MaxVersion = CacheVersion.HaloReach)]
            public uint Unknown19;
            [TagField(MinVersion = CacheVersion.HaloReach, MaxVersion = CacheVersion.HaloReach)]
            public uint Unknown20;

            public enum OptionDataType : uint
            {
                Sampler = 0,
                Float4 = 1,
                Float = 2,
                Integer = 3,
                Boolean = 4,
                IntegerColor = 5
            }

            public enum DefaultFilterModeValue : short
            {
                Trilinear,
                Point,
                Bilinear,
                unused_00,
                Anisotropic_2x,
                unused_01,
                Anisotropic_4x,
                LightprobeTextureArray,
                TextureArrayQuadlinear,
                TextureArrayQuadanisotropic_2x
            }

            public enum DefaultAddressModeValue : short
            {
                Wrap,
                Clamp,
                Mirror,
                BlackBorder,
                MirrorOnce,
                MirrorOnceBorder
            }
        }
    }
}