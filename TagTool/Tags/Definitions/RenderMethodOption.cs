using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using TagTool.Shaders;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "render_method_option", Tag = "rmop", Size = 0xC)]
    public class RenderMethodOption : TagStructure
    {
        public List<ParameterBlock> Parameters;

        [TagStructure(Size = 0x48, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x4C, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagStructure(Size = 0x54, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x48, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x60, MinVersion = CacheVersion.HaloReach)]
        public class ParameterBlock : TagStructure
        {
            public StringId Name;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public StringId UiNameOverride;
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
            [TagField(MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
            public short DefaultComparisionFunction;
            public DefaultAddressModeValue DefaultAddressMode;
            public short AnisotropyAmount;

            [TagField(MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC, Flags = TagFieldFlags.Padding, Length = 0x2)]
            public byte[] Padding;

            public ArgbColor DefaultColor;
            public float DefaultBitmapScale;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public UiAndUsageFlags UsageFlags;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public FunctionTypeEnum ForceFunctionType;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public ColorGraphTypeEnum ForceFunctionColorCount;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float SuggestedRealMin;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float SuggestedRealMax;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int TicksFromMinToMax;

            public byte[] HelpText;

            public enum OptionDataType : uint
            {
                Bitmap = 0,
                Color = 1,
                Real = 2,
                Int = 3,
                Bool = 4,
                ArgbColor = 5
            }

            public enum DefaultFilterModeValue : short
            {
                Trilinear,
                Point,
                Bilinear,
                Anisotropic1,
                Anisotropic2Expensive,
                Anisotropic3Expensive,
                Anisotropic4EXPENSIVE,
                LightprobeTextureArray,
                ComparisonPoint,
                ComparisonBilinear
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

            [Flags]
            public enum UiAndUsageFlags : uint
            {
                UseForceFunctionType = 1 << 0,
                UseForceFunctionColorCount = 1 << 1,
                ParameterInvisibleInUi = 1 << 2,
                LockFunctionValue = 1 << 3
            }

            public enum FunctionTypeEnum : short
            {
                Identity,
                Constant,
                Transition,
                Periodic,
                Linear,
                LinearKey,
                MultiLinearKey,
                Spline,
                MultiSpline,
                Exponent,
                Spline2
            }

            public enum ColorGraphTypeEnum : short
            {
                Scalar,
                Constant,
                _2Color,
                _3Color,
                _4Color
            }
        }
    }
}